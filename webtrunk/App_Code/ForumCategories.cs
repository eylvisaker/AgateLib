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








public static class CategoryDAL
{
    private static int max_name_length = 50;
    private static String table_name = "ForumCategories";

    //TODO: Implement roles based authority
    //PRECONDITIONS:
    //  cat_name is <= max_category_name_length
    //
    public static bool Insert(String cat_name)
    {
        DbConnection connection = get_connection();

        if (cat_name.Length >= max_name_length)
        {
            throw new System.ArgumentException("category name length must be less than "
                + cat_name.ToString(), "cat_name");
        }


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
            cmd.Parameters.Add("@Name", DbType.WChar, max_name_length).Value = cat_name;
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



    public static void Update(int primary_id, String name)
    {
        if (!CategoryDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to update a category that does not exist.");


        DbCommand update_cmd = new DbCommand();
        update_cmd.CommandText =
            "UPDATE " + table_name +
            " SET Name = ?" +
            " WHERE id = ?";

        update_cmd.Parameters.Add("@Name", DbType.WChar, max_name_length).Value = name;
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




    public static void Delete(int row_id)
    {
        if (!CategoryDAL.Exists(row_id))
            throw new System.ArgumentException("Attempt to delete a category that does not exist.");

        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);

        // Create an explicit UPDATE command
        DbDataAdapter da = new DbDataAdapter();

        da.UpdateCommand = update_command_for_delete_operation();
        da.DeleteCommand = delete_command_for_delete_operation();

        category_list.MoveToBottom(row_id);
        DataRow row = category_list.GetRowByID(row_id);

        if( BoardDAL.DeleteAllByParentID( row_id ) )
        {
            row.Delete();
            da.Update(category_list.DataSet.Tables[table_name].GetChanges());
        }
    }


    public static void MoveToTop(int primary_id)
    {
        if (!CategoryDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to change the position of a category that does not exist.");

        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);
        category_list.MoveToTop(primary_id);

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


    public static void MoveToBottom(int primary_id)
    {
        if (!CategoryDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to change the position of a category that does not exist.");

        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);
        category_list.MoveToBottom(primary_id);

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


    public static void MoveUp(int primary_id)
    {
        if (!CategoryDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to change the position of a category that does not exist.");

        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);
        category_list.MoveUp(primary_id);

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


    public static void MoveDown(int primary_id)
    {
        if (!CategoryDAL.Exists(primary_id))
            throw new System.ArgumentException("Attempt to change the position of a category that does not exist.");

        ActAsList category_list = new ActAsList(CategoryDAL.DataSet(), table_name);
        category_list.MoveDown(primary_id);

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


    // ///////////////////////// PRIVATE FUNCTIONS ///////////////////////


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

    private static DbConnection get_connection()
    {
        String conn_string = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Access"].ConnectionString;
        return new DbConnection(conn_string);
    }
}