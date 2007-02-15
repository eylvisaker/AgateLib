//TODO
//TODO
//TODO
//TODO
// Decide on a method of transparency between the database & the list object.  Is there a way to keep the data set synchronous with the DB automatically?


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
///  Static Data Access Layer for the ForumCategories table
/// </summary>
public static class CategoryDAL
{
    private static int max_name_length = 50;
    private static String table_name = "ForumCategories";


    /// <summary>
    /// Insert a Forum Category
    /// </summary>
    /// <param name="name"> Name of the Category</param>
    ///
    public static bool Insert(String name)
    {
        if (!check_name(name))
            throw new System.ArgumentException(name + " is an invalid name");
        /////////////////////////////////////////////////////////////////////

        DbConnection connection = get_connection();

        DbCommand max_position_cmd = new DbCommand("SELECT MAX([Position]) FROM " + table_name, connection);

        String insert_query_string = "INSERT INTO " + table_name +
            " (Name, [Position], CreationDate)" +
            " Values(?,?,?)";

        DbDataReader reader = null;

        // Open connection, query for the max position, update db with new category of position + 1
        try
        {
            connection.Open();
            reader = max_position_cmd.ExecuteReader();

            // If there are no Categories in the table, this will be incremented to 0.
            int max_position = -1;

            if (reader.Read() && !reader.IsDBNull(0))
            {
                //TODO: verify actual bit size of OleDb.Integer
                max_position = reader.GetInt32(0);
            }

            DbCommand cmd = new DbCommand(insert_query_string, connection);
            cmd.Parameters.Add("@Name", DbType.WChar, max_name_length).Value = name;
            cmd.Parameters.Add("@Position", DbType.Integer).Value = max_position + 1;
            cmd.Parameters.Add("@CreationDate", DbType.Date).Value = DateTime.Now.ToString();
            cmd.ExecuteNonQuery();

            return true;
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
    ///  Delete a single category from the table
    /// </summary>
    /// <param name="category_id">primary key for the category</param>
    /// TODO: The deletion of the children should probably be done in a transaction
    public static void Delete(int category_id)
    {
        if (!check_category_id(category_id))
            throw new System.ArgumentException(category_id.ToString() + "is an Invalid ID number");
        ///////////////////////////////////////////////////////////////////////////////////////////
        /*
         * - retrieve data set, create list
         * - move row to be deleted to the bottom (highest position) of the list
         * - create data adaper & delete/update commands
         * - delete all children( boards )
         * - delete row
         * - update
         */


        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);

        // Create an explicit UPDATE command
        DbDataAdapter da = new DbDataAdapter();

        da.UpdateCommand = update_command_for_delete_operation();
        da.DeleteCommand = delete_command_for_delete_operation();

        category_list.MoveToBottom(category_id);
        DataRow row = category_list.GetRowByID(category_id);

        if (BoardDAL.DeleteAllByParentID(category_id))
        {
            row.Delete();
            da.Update(category_list.DataSet.Tables[table_name].GetChanges());
        }
    }



    /// <summary>
    ///  Update a category name
    /// </summary>
    /// <param name="category_id"></param>
    /// <param name="name"></param>
    public static void Update(int category_id, String name)
    {
        if (!check_category_id(category_id))
            throw new System.ArgumentException(category_id.ToString() + "is an Invalid ID number");

        if (!check_name(name))
            throw new System.ArgumentException(name + " is an invalid name");
        /////////////////////////////////////////////////////////////////////
        /*
        * - create command, set command text for query
        * - bind parameter values
        * - set command connection
        * - open connection, execute query, close connection
        */

        DbCommand update_cmd = new DbCommand();
        update_cmd.CommandText =
            "UPDATE " + table_name +
            " SET Name = ?" +
            " WHERE id = ?";

        update_cmd.Parameters.Add("@Name", DbType.WChar, max_name_length).Value = name;
        update_cmd.Parameters.Add("@id", DbType.Integer, 0).Value = category_id;

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
    ///  Move the category to the top of the list (position 0)
    /// </summary>
    /// <param name="category_id">primary key of category</param>
    public static void MoveToTop(int category_id)
    {
        if (!check_category_id(category_id))
            throw new System.ArgumentException(category_id.ToString() + "is an Invalid ID number");
        ///////////////////////////////////////////////////////////////////////////////////////////
        /*
         * - retrieve data set, create list
         * - move row to the top of the list
         * - create update command, set text & bind parameters
         * - create/set data adapter
         * - update
         */

        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);
        category_list.MoveToTop(category_id);

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

        DataTable table = category_list.DataSet.Tables[table_name].GetChanges();

        if( table != null )
            da.Update( table );
    }

    /// <summary>
    ///  Move the category to the bottom of the list (greatest position)
    /// </summary>
    /// <param name="category_id">primary key of category</param>
    public static void MoveToBottom(int category_id)
    {
        if (!check_category_id(category_id))
            throw new System.ArgumentException(category_id.ToString() + "is an Invalid ID number");
        ///////////////////////////////////////////////////////////////////////////////////////////

        /*
        * - retrieve data set, create list
        * - move row to the bottom of the list
        * - create update command, set text & bind parameters
        * - create/set data adapter
        * - update
        */


        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);
        category_list.MoveToBottom(category_id);

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

        DataTable table = category_list.DataSet.Tables[table_name].GetChanges();

        if (table != null)
            da.Update(table);
    }


    /// <summary>
    ///  Move the category up 1 position in the list (greatest position)
    /// </summary>
    /// <param name="category_id">primary key of category</param>
    public static void MoveUp(int category_id)
    {
        if (!check_category_id(category_id))
            throw new System.ArgumentException(category_id.ToString() + "is an Invalid ID number");
        ///////////////////////////////////////////////////////////////////////////////////////////
        /*
        * - retrieve data set, create list
        * - move row up 1 position in the list
        * - create update command, set text & bind parameters
        * - create/set data adapter
        * - update
        */

        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);
        category_list.MoveUp(category_id);

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

        DataTable table = category_list.DataSet.Tables[table_name].GetChanges();

        if (table != null)
            da.Update(table);
    }


    /// <summary>
    ///  Move the category down 1 position in the list (greatest position)
    /// </summary>
    /// <param name="category_id">primary key of category</param>
    public static void MoveDown(int category_id)
    {
        if (!check_category_id(category_id))
            throw new System.ArgumentException(category_id.ToString() + "is an Invalid ID number");
        ///////////////////////////////////////////////////////////////////////////////////////////

        /*
         * - retrieve data set, create list
         * - move row down 1 position in the list
         * - create update command, set text & bind parameters
         * - create/set data adapter
         * - update
         */


        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);
        category_list.MoveDown(category_id);

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

        DataTable table = category_list.DataSet.Tables[table_name].GetChanges();

        if (table != null)
            da.Update(table);


    }


    /// <summary>
    /// dataset for the entire table
    /// </summary>
    /// <returns></returns>
    public static DataSet DataSet()
    {
        DbConnection connection = get_connection();

        String sql = "SELECT * from " + table_name;
        DbDataAdapter da = new DbDataAdapter(sql, connection);

        DataSet ds = new DataSet();
        try
        {
            connection.Open();
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
    /// retrieve the category with the given id
    /// </summary>
    /// <param name="row_id"></param>
    /// <returns></returns>
    public static DataRow GetRowByID(int row_id)
    {
        // retrieve entire table, pull out corresponding row
        DataRow[] rows_with_id = DataSet().Tables[table_name].Select("id = '" + row_id.ToString() + "'");

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
    /// Tests to see if a category with the id exists
    /// </summary>
    /// <param name="primary_id"></param>
    /// <returns></returns>
    public static bool Exists(int primary_id)
    {
        return (GetRowByID(primary_id) != null);
    }





    // ///////////////////////// PRIVATE FUNCTIONS ///////////////////////


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
    ///  checks to see if the name is a valid Category name
    /// </summary>
    /// <remarks>
    ///  length of name must be <= max_name_length
    /// </remarks>
    /// <param name="name"></param>
    /// <returns></returns>
    /// 
    /// TODO: We need to clean the name (check for security issues, etc)
    private static bool check_name(String name)
    {
        if (name.Length > max_name_length)
            return false;

        return true;
    }

    /// <summary>
    /// checks to see if Category id is valid
    /// </summary>
    /// <remarks>
    /// board_id must be >= 0 and it must exist in the ForumBoards table
    /// </remarks>
    /// <param name="board_id"></param>
    /// <returns></returns>
    private static bool check_category_id(int category_id)
    {
        if( category_id < 0 || !CategoryDAL.Exists(category_id) )
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
        DbCommand cmd = new DbCommand();
        cmd.CommandText =
            "UPDATE " + table_name +
            " SET Name = ?, [Position] = ?" +
            " WHERE id = ?";

        // SET Name = ?, [Position] = ?, CreationDate = ?"
        cmd.Parameters.Add("@Name", DbType.WChar, 50, "Name");
        cmd.Parameters.Add("@Position", DbType.Integer, 0, "Position");

        // where id = 
        cmd.Parameters.Add("@id", DbType.Integer, 0, "id");

        cmd.Connection = get_connection();
        return cmd;
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