//TODO: deleting of children, etc should be transacted in some way.

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using DbConnection = System.Data.OleDb.OleDbConnection;
using DbCommand = System.Data.OleDb.OleDbCommand;
using DbType = System.Data.OleDb.OleDbType;
using DbDataReader = System.Data.OleDb.OleDbDataReader;
using DbParameter = System.Data.OleDb.OleDbParameter;
using DbTransaction = System.Data.OleDb.OleDbTransaction;
using DbException = System.Data.OleDb.OleDbException;
using DbDataAdapter = System.Data.OleDb.OleDbDataAdapter;

/// <summary>
///  Static Data Access Layer for the ForumBoards table
/// </summary>
public static class BoardDAL
{
    private static int max_name_length = 50;
    private static int max_description_length = 100;
    private static string table_name = "ForumBoards";


    /// <summary>
    /// Insert a Forum Board
    /// </summary>
    /// <param name="name"> Name of the Board</param>
    /// <param name="parent_id"> ID of the category the board will be under</param>
    /// <param name="description"> Brief description of the Board </param>
    ///
    public static void Insert(String name, int parent_id, String description)
    {
        if( !check_name( name ) )
            throw new System.ArgumentException("Board name length must be less than " +
                max_name_length.ToString(), " characters");

        if( !check_description( description ) )
            throw new System.ArgumentException("Board description length must be less than " + 
                max_description_length.ToString(), " characters");

        if( !check_parent_id( parent_id ) )
            throw new System.ArgumentException(parent_id.ToString() + " is an Invalid parent ID");
        ///////////////////////////////////////////////////////////////////////////////////////////



        DbConnection connection = get_connection();
        
        String insert_query_string = "INSERT INTO " + table_name +
            " (name, [position], creation_date, parent_id, description)" +
            " Values(?,?,?,?,?)";

        DbDataReader reader = null;

        // Open connection, query for the max position, update db with new Board of position + 1
        try
        {
            connection.Open();

            // max position within category.  returns -1 if category is empty
            int max_position = BoardDAL.MaxPosition( parent_id );

            DbCommand cmd = new DbCommand(insert_query_string, connection);
            cmd.Parameters.Add("@name", DbType.WChar, max_name_length).Value = name;
            cmd.Parameters.Add("@position", DbType.Integer).Value = max_position + 1;
            cmd.Parameters.Add("@creation_date", DbType.Date).Value = DateTime.Now.ToString();
            cmd.Parameters.Add("@parent_id", DbType.Integer).Value = parent_id;
            cmd.Parameters.Add("@description", DbType.WChar).Value = description;
            cmd.ExecuteNonQuery();
        }
        catch (DbException)
        {
            throw;
        }
        finally
        {
            connection.Close();
        }
    }


    /// <summary>
    ///  Delete a single board from the table
    /// </summary>
    /// <param name="board_id">primary key for the board</param>
    public static void Delete(int board_id)
    {
        if( !check_board_id(board_id) )
            throw new System.ArgumentException(board_id.ToString() + " is an Invalid Board ID");
        /////////////////////////////////////////////////////////////////////////////////////////

        /*
         * - Get parent ID
         * - retrieve data set, create list
         * - move row to be deleted to the bottom (highest position) of the list
         * - create data adaper & delete/update commands
         * - delete row
         * - update
         */

        int parent_id = GetParentID(board_id);

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(parent_id), table_name);
        board_list.MoveToBottom(board_id);

        DbDataAdapter da = new DbDataAdapter();

        da.UpdateCommand = update_command_for_delete_operation();
        da.DeleteCommand = delete_command_for_delete_operation();

        
        DataRow row = board_list.GetRowByID(board_id);

        if (ThreadDAL.DeleteAllByParentID(board_id))
        {
            row.Delete();
            da.Update(board_list.DataSet.Tables[table_name].GetChanges());
        }
    }


    /// <summary>
    ///  Delete all children of the parent Category
    /// </summary>
    /// <param name="parent_id"> primary key for the parent</param>
    /// <returns>returns true on success</returns>
    public static bool DeleteAllByParentID( int parent_id )
    {
        if (!check_parent_id(parent_id))
            throw new System.ArgumentException(parent_id.ToString() + " is an Invalid parent ID");
        //////////////////////////////////////////////////////////////////////////////////////////

        /*
         * - create delete command, assign text, bind relevant values for query
         * - get connection
         * - open connection, execute query, then close connection
         */

        DbCommand delete_cmd = new DbCommand();
        delete_cmd.CommandText =
            "DELETE FROM " + table_name +
            " WHERE parent_id = ?";

        delete_cmd.Parameters.Add("@id", DbType.Integer).Value = parent_id;

        delete_cmd.Connection = get_connection();

        DataSet ds = BoardDAL.DataSet(parent_id);

        // delete all children
        foreach (DataRow row in ds.Tables[table_name].Rows)
        {
            ThreadDAL.DeleteAllByParentID((int)row["id"]);
        }

        try
        {
            delete_cmd.Connection.Open();
            delete_cmd.ExecuteNonQuery();

            return true;
        }
        catch (DbException e)
        {
            throw;
        }
        finally
        {
            delete_cmd.Connection.Close();
        }
    }


    /// <summary>
    ///  Update a boards name, parent id, & description
    /// </summary>
    /// <param name="board_id"></param>
    /// <param name="name"></param>
    /// <param name="parent_id"> primary id of category</param>
    /// <param name="description"></param>
    public static void Update(int board_id, String name, int parent_id, String description)
    {
        if (!check_name(name))
            throw new System.ArgumentException("Board name length must be less than " +
                max_name_length.ToString(), " characters");

        if (!check_description(description))
            throw new System.ArgumentException("Board description length must be less than " +
                max_description_length.ToString(), " characters");

        if (!check_parent_id(parent_id))
            throw new System.ArgumentException(parent_id.ToString() + " is an Invalid parent ID");

        if (!check_board_id(board_id))
            throw new System.ArgumentException(board_id.ToString() + " is an Invalid Board ID");
        ///////////////////////////////////////////////////////////////////////////////////////////

        /*
         * - create command, set command text for query
         * - bind parameter values
         * - set command connection
         * - open connection, execute query, close connection
         */

        DbCommand update_cmd = new DbCommand();
        update_cmd.CommandText =
            "UPDATE " + table_name +
            " SET name = ?, parent_id = ?, description = ?" +
            " WHERE id = ?";

        // SET name = ?, [Position] = ?, parent = ?, CreationDate = ?"
        update_cmd.Parameters.Add("@name", DbType.WChar, max_name_length).Value = name;
        update_cmd.Parameters.Add("@parent_id", DbType.Integer, 0).Value = parent_id;
        update_cmd.Parameters.Add("@description", DbType.WChar, max_description_length).Value = description;

        // where id = 
        update_cmd.Parameters.Add("@id", DbType.Integer, 0).Value = board_id;

        DbConnection connection = get_connection();
        update_cmd.Connection = connection;

        try
        {
            connection.Open();
            update_cmd.ExecuteNonQuery();
        }
        catch (DbException)
        { throw; }
        finally
        { connection.Close(); }

    }




    /// <summary>
    ///  Move the board to the top of the list (position 0)
    /// </summary>
    /// <param name="board_id">primary key of board</param>
    public static void MoveToTop(int board_id)
    {
        if (!check_board_id(board_id))
            throw new System.ArgumentException(board_id.ToString() + " is an Invalid Board ID");
        ////////////////////////////////////////////////////////////////////////////////////////

        /*
         * - retrieve the parent category ID
         * - retrieve data set, create list
         * - move row to the top of the list
         * - create update command, set text & bind parameters
         * - create/set data adapter
         * - update
         */

        int parent_id = GetParentID(board_id);

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(parent_id), table_name);
        board_list.MoveToTop(board_id);

        DbCommand update_cmd = new DbCommand();
        update_cmd.CommandText =
            "UPDATE " + table_name +
            " SET [position] = ?" +
            " WHERE id = ?";

        update_cmd.Parameters.Add("@position", DbType.Integer, 0, "position");
        update_cmd.Parameters.Add("@id", DbType.Integer, 0, "id");
        update_cmd.Connection = get_connection();

        DbDataAdapter da = new DbDataAdapter();
        da.UpdateCommand = update_cmd;

        DataTable table = board_list.DataSet.Tables[table_name].GetChanges();

        if (table != null)
            da.Update(table);
    }

    /// <summary>
    ///  Move the board to the bottom of the list (greatest position)
    /// </summary>
    /// <param name="board_id">primary key of board</param>
    public static void MoveToBottom(int board_id)
    {
        if (!check_board_id(board_id))
            throw new System.ArgumentException(board_id.ToString() + " is an Invalid Board ID");
        ////////////////////////////////////////////////////////////////////////////////////////

        /*
         * - retrieve the parent category ID
         * - retrieve data set, create list
         * - move row to the bottom of the list
         * - create update command, set text & bind parameters
         * - create/set data adapter
         * - update
         */


        int parent_id = GetParentID(board_id);

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(parent_id), table_name);
        board_list.MoveToBottom(board_id);

        DbCommand update_cmd = new DbCommand();
        update_cmd.CommandText =
            "UPDATE " + table_name + 
            " SET [Position] = ?" +
            " WHERE id = ?";

        update_cmd.Parameters.Add("@Position", DbType.Integer, 0, "position");
        update_cmd.Parameters.Add("@id", DbType.Integer, 0, "id");
        update_cmd.Connection = get_connection();

        DbDataAdapter da = new DbDataAdapter();
        da.UpdateCommand = update_cmd;

        DataTable table = board_list.DataSet.Tables[table_name].GetChanges();

        if (table != null)
            da.Update(table);
    }


    /// <summary>
    ///  Move the board up 1 position in the list (greatest position)
    /// </summary>
    /// <param name="board_id">primary key of board</param>
    public static void MoveUp(int board_id)
    {
        if (!check_board_id(board_id))
            throw new System.ArgumentException(board_id.ToString() + " is an Invalid Board ID");
        ////////////////////////////////////////////////////////////////////////////////////////

        /*
         * - retrieve the parent category ID
         * - retrieve data set, create list
         * - move row up 1 position in the list
         * - create update command, set text & bind parameters
         * - create/set data adapter
         * - update
         */


        int parent_id = GetParentID(board_id);

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(parent_id), table_name);
        board_list.MoveUp(board_id);

        DbCommand update_cmd = new DbCommand();
        update_cmd.CommandText =
            "UPDATE " + table_name +
            " SET [position] = ?" +
            " WHERE id = ?";

        update_cmd.Parameters.Add("@position", DbType.Integer, 0, "position");
        update_cmd.Parameters.Add("@id", DbType.Integer, 0, "id");
        update_cmd.Connection = get_connection();

        DbDataAdapter da = new DbDataAdapter();
        da.UpdateCommand = update_cmd;

        DataTable table = board_list.DataSet.Tables[table_name].GetChanges();

        if (table != null)
            da.Update(table);
    }


    /// <summary>
    ///  Move the board down 1 position in the list (greatest position)
    /// </summary>
    /// <param name="board_id">primary key of board</param>
    public static void MoveDown(int board_id)
    {
        if (!check_board_id(board_id))
            throw new System.ArgumentException(board_id.ToString() + " is an Invalid Board ID");
        ////////////////////////////////////////////////////////////////////////////////////////

        /*
         * - retrieve the parent category ID
         * - retrieve data set, create list
         * - move row down 1 position in the list
         * - create update command, set text & bind parameters
         * - create/set data adapter
         * - update
         */

        int parent_id = GetParentID(board_id);

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(parent_id), table_name);
        board_list.MoveDown(board_id);

        DbCommand update_cmd = new DbCommand();
        update_cmd.CommandText =
            "UPDATE " + table_name +
            " SET [position] = ?" +
            " WHERE id = ?";

        update_cmd.Parameters.Add("@position", DbType.Integer, 0, "position");
        update_cmd.Parameters.Add("@id", DbType.Integer, 0, "id");
        update_cmd.Connection = get_connection();

        DbDataAdapter da = new DbDataAdapter();
        da.UpdateCommand = update_cmd;

        DataTable table = board_list.DataSet.Tables[table_name].GetChanges();

        if (table != null)
            da.Update(table);
    }



    /// <summary>
    /// retrieve the board  with the given id
    /// </summary>
    /// <param name="board_id"></param>
    /// <returns></returns>
    public static DataRow GetRowByID(int board_id)
    {
        // retrieve entire table, pull out corresponding row
        DataRow[] rows_with_id = DataSet().Tables[table_name].Select("id = '" + board_id.ToString() + "'");

        if (rows_with_id.Length == 1)
        {
            return rows_with_id[0];

        }
        else if (rows_with_id.Length > 1)
        {
            throw new System.ApplicationException("DataSet is showing duplicated primary keys");
        }
        else
            return null;
    }


    /// <summary>
    /// Tests to see if a board with the id exists
    /// </summary>
    /// <param name="primary_id"></param>
    /// <returns></returns>
    public static bool Exists(int board_id)
    {
        return (GetRowByID(board_id) != null);
    }


    /// <summary>
    ///  Retrieves data set of all boards with the given parent id
    /// </summary>
    /// <param name="parent_id"></param>
    /// <returns></returns>
    public static DataSet DataSet(int parent_id)
    {
        if (!check_parent_id(parent_id))
            throw new System.ArgumentException(parent_id.ToString() + " is an Invalid parent ID");
        ///////////////////////////////////////////////////////////////////////////////////////////

        DbConnection connection = get_connection();
        String sql = "SELECT * FROM " + table_name + " WHERE parent_id = " + parent_id.ToString();

        DbDataAdapter da = new DbDataAdapter(sql, connection);

        try
        {
            connection.Open();

            DataSet ds = new DataSet();
            da.Fill(ds, table_name);

            return ds;
        }
        catch (DbException e)
        {
            throw;
        }
        finally
        {
            connection.Close();
        }
    }


    /// <summary>
    /// dataset for the entire table
    /// </summary>
    /// <returns></returns>
    private static DataSet DataSet()
    {
        DbConnection connection = get_connection();

        String sql = "SELECT * from " + table_name;
        DbDataAdapter da = new DbDataAdapter(sql, connection);

        try
        {
            connection.Open();

            DataSet ds = new DataSet();
            da.Fill(ds, table_name);

            return ds;
        }
        catch (DbException e)
        {
            throw;
        }
        finally
        {
            connection.Close();
        }
    }



    /// <summary>
    /// retrieve the parent id of the given board
    /// </summary>
    /// <param name="board_id"></param>
    /// <returns></returns>
    private static int GetParentID(int board_id)
    {
        if (!check_board_id(board_id))
            throw new System.ArgumentException(board_id.ToString() + " is an Invalid Board ID");
        ////////////////////////////////////////////////////////////////////////////////////////


        DbConnection connection = get_connection();
        
        String sql = "SELECT parent_id FROM " + table_name + " WHERE id = " + board_id.ToString();
        DbCommand cmd = new DbCommand(sql, connection);

        /*
         * - open connection, execute reader
         * - retrieve parent_id, check parent_id, then return parent_id
         */
        try
        {
            connection.Open();
            DbDataReader reader = cmd.ExecuteReader();

            int parent_id = -1;
            if ( reader.Read() && !reader.IsDBNull(0))
            {
                //TODO: verify actual bit size of OleDb.Integer
                parent_id = reader.GetInt32(0);

                if (!check_parent_id(parent_id))
                    throw new System.ArgumentException(parent_id.ToString() + " is an Invalid parent ID");

                return parent_id;
            }
            else
                throw new System.ApplicationException("boards with id: "+board_id.ToString()+" has no parent ID value");
        }
        catch (DbException e)
        {
            throw;
        }
        finally
        {
            connection.Close();
        }
    }


    /// <summary>
    /// returns the greatest position available in a category
    /// </summary>
    /// <param name="parent_id"></param>
    /// <returns> returns -1 when no boards exist in category</returns>
    private static int MaxPosition(int parent_id)
    {
        DataSet ds = BoardDAL.DataSet(parent_id);

        int max = -1;
        int tmp = -1;
        foreach (DataRow row in ds.Tables[table_name].Rows)
        {
            tmp = (int)row["position"];
            if (tmp > max)
                max = tmp;
        }

        return max;
    }

    
    /// <summary>
    ///  returns the connection for the table
    /// </summary>
    /// <returns></returns>
    private static DbConnection get_connection()
    {
        String conn_string = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Access"].ConnectionString;
        return new DbConnection(conn_string);
    }

    /// <summary>
    ///  checks to see if the name is a valid board name
    /// </summary>
    /// <remarks>
    ///  length of name must be <= max_name_length
    /// </remarks>
    /// <param name="name"></param>
    /// <returns></returns>
    /// 
    /// TODO: We need to clean the name (check for security issues, etc)
    private static bool check_name( String name )
    {
        if (name.Length > max_name_length)
            return false;
        
        return true;
    }

    /// <summary>
    ///  checks to see if the description is a valid 
    /// </summary>
    /// <remarks>
    /// length of description must be <= max_description_length
    /// </remarks>
    /// <param name="description"></param>
    /// <returns></returns>
    /// 
    /// TODO: We need to clean the name (check for security issues, etc)
    private static bool check_description(String description)
    {
        if (description.Length > max_description_length)
            return false;

        return true;
    }


    /// <summary>
    ///  checks to see if parent_id is valid
    /// </summary>
    /// <remarks>
    /// parent_id must be >= 0 and it must exist in the ForumCategories table
    /// </remarks>
    /// <param name="parent_id"></param>
    /// <returns></returns>
    private static bool check_parent_id( int parent_id )
    {
        if( parent_id < 0 || !CategoryDAL.Exists( parent_id ) )
            return false;

        return true;
    }

    /// <summary>
    /// checks to see if board id is valid
    /// </summary>
    /// <remarks>
    /// board_id must be >= 0 and it must exist in the ForumBoards table
    /// </remarks>
    /// <param name="board_id"></param>
    /// <returns></returns>
    private static bool check_board_id(int board_id)
    {
        if (board_id < 0 || !BoardDAL.Exists(board_id) )
            return false;

        return true;
    }
        
        

    /// <summary>
    /// returns the update command for updating *all columns* in the table via a *dataset*
    /// </summary>
    /// <remarks>
    ///  this method is meant for a very specific purpose and is not general in any sense of the word.
    ///  use with caution
    /// </remarks>
    /// <returns></returns>
    private static DbCommand update_command_for_delete_operation()
    {
        DbCommand update_cmd = new DbCommand();

        update_cmd.CommandText =
            "UPDATE " + table_name + 
            " SET name = ?, [position] = ?, parent_id = ?, description = ?" +
            " WHERE id = ?";
         
        // SET name = ?, [Position] = ?, parent = ?, CreationDate = ?"
        update_cmd.Parameters.Add("@name", DbType.WChar, max_name_length, "name");
        update_cmd.Parameters.Add("@position", DbType.Integer, 0,"position");
        update_cmd.Parameters.Add("@parent_id", DbType.Integer, 0,"parent_id");
        update_cmd.Parameters.Add("@description", DbType.WChar, max_description_length,"description");

        // where id = 
        update_cmd.Parameters.Add("@id", DbType.Integer, 0, "id");

        update_cmd.Connection = get_connection();
        return update_cmd;
    }


    /// <summary>
    /// returns the delete command for deleting a row in the table via a *dataset*
    /// </summary>
    /// <remarks>
    ///  this method is meant for a very specific purpose and is not general in any sense of the word.
    ///  use with caution
    /// </remarks>
    /// <returns></returns>
    private static DbCommand delete_command_for_delete_operation()
    {
        DbCommand cmd = new DbCommand();
        cmd.CommandText =
            "DELETE FROM " + table_name +
            " WHERE id = ?";

        // Bind parameters to appropriate columns for DELETE command
        cmd.Parameters.Add("@id", DbType.Integer, 0, "id");

        cmd.Connection = get_connection();
        return cmd;
    }
}
