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

//TODO
// Change all actions to be done on sets with parent_id only.


public static class BoardDAL
{
    private static int max_name_length = 50;
    private static int max_description_length = 100;
    private static string table_name = "ForumBoards";

    public static bool Insert(String name, int parent_id, String description)
    {
        check_name( name );
        check_description( description );
        check_parent_id( parent_id );


        DbConnection connection = get_connection();
        
        DbCommand max_position_cmd = new DbCommand("SELECT MAX([position]) FROM " + table_name, connection);

        String insert_query_string = "INSERT INTO " + table_name +
            " (name, [position], creation_date, parent_id, description)" +
            " Values(?,?,?,?,?)";

        DbDataReader reader = null;

        // Open connection, query for the max position, update db with new Board of position + 1
        try
        {
            connection.Open();
            reader = max_position_cmd.ExecuteReader();

            // If there are no Boards in the table, this will be incremented to 0.
            int max_position = -1;

            if (reader.Read() && !reader.IsDBNull(0))
            {
                //TODO: verify actual bit size of OleDb.Integer
                max_position = reader.GetInt32(0);
            }

            DbCommand cmd = new DbCommand(insert_query_string, connection);
            cmd.Parameters.Add("@name", DbType.WChar, max_name_length).Value = name;
            cmd.Parameters.Add("@position", DbType.Integer).Value = max_position + 1;
            cmd.Parameters.Add("@creation_date", DbType.Date).Value = DateTime.Now.ToString();
            cmd.Parameters.Add("@parent_id", DbType.Integer).Value = parent_id;
            cmd.Parameters.Add("@description", DbType.WChar).Value = description;
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


    public static void Delete(int board_id)
    {
        if (!BoardDAL.Exists(board_id))
            throw new System.ArgumentException("Attempt to delete a Board that does not exist.");

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(), table_name);

        // Create an explicit UPDATE command
        DbDataAdapter da = new DbDataAdapter();

        da.UpdateCommand = update_command_for_delete_operation();
        da.DeleteCommand = delete_command_for_delete_operation();

        board_list.MoveToBottom(board_id);
        DataRow row = board_list.GetRowByID(board_id);

        row.Delete();
        da.Update(board_list.DataSet.Tables[table_name].GetChanges());
    }


    public static void Update(int primary_id, String name, int parent_id, String description)
    {
        check_name(name);
        check_parent_id(parent_id);
        check_description(description);

        if (!BoardDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to update a Board that does not exist.");


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
        update_cmd.Parameters.Add("@id", DbType.Integer, 0).Value = primary_id;

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





    public static void MoveToTop(int primary_id)
    {
        if (!BoardDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to change the position of a category that does not exist.");

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(), table_name);
        board_list.MoveToTop(primary_id);

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


    public static void MoveToBottom(int primary_id)
    {
        if (!BoardDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to change the position of a Board that does not exist.");

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(), table_name);
        board_list.MoveToBottom(primary_id);

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


    public static void MoveUp(int primary_id)
    {
        if (!BoardDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to change the position of a Board that does not exist.");

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(), table_name);
        board_list.MoveUp(primary_id);

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

    public static void MoveDown(int primary_id)
    {
        if (!BoardDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to change the position of a Board that does not exist.");

        ActAsList board_list = new ActAsList(BoardDAL.DataSet(), table_name);
        board_list.MoveDown(primary_id);

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









    public static DataRow GetRowByID(int row_id)
    {
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



    public static bool Exists(int primary_id)
    {
        return (GetRowByID(primary_id) != null);
    }


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


    public static DataSet DataSet(int parent_id)
    {
        DbConnection connection = get_connection();

        String sql = "SELECT * from " + table_name + " WHERE parent_id = " + parent_id.ToString();


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

    

    private static DbConnection get_connection()
    {
        String conn_string = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Access"].ConnectionString;
        return new DbConnection(conn_string);
    }


    private static void check_name( String name )
    {
        if (name.Length > max_name_length)
        {
            throw new System.ArgumentException("Board name length must be less than "
                + max_name_length.ToString() + " characters");
        }
    }


    private static void check_description(String description)
    {
        if( description.Length > max_description_length )
        {
            throw new System.ArgumentException("Board description length must be less than "
                + max_description_length.ToString(), " characters");
        }
    }


    private static void check_parent_id( int parent_id )
    {
        if( !CategoryDAL.Exists( parent_id ) )
        {
            throw new System.ArgumentException("Parent does not exist");
        }

        if( parent_id < 0 )
        {
            throw new System.ArgumentException("Parent ID is negative");
        }
    }


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
