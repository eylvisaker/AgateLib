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

/*
 * The following class if *VERY* fragile.  There are many assumptions/requirements with regards to the data
 *  that it handles.  If any of these are broken, or changed, it *will* result in unexpected behavior.
 * 
 * DATASET TABLE LAYOUT
 *  This requires a primary key column of "id" & a position identifier column of "Position", both of which are type int.
 *   These can be generalized to get around this requirement, but for the sake of brevity, it has not been done.
 * 
 *  POSITION COLUMN CONTAINS STRICTLY CONTIGUOUS INTEGERS
 *   This is very very important.  Every movement operation we do works under the assumption that everything the 
 *   position column data is contiguous.
 *   
 */
public class TreatAsListHelper
{
    private DataSet data_set;
    private String table_name;


    public TreatAsListHelper( DataSet ds, String tbl_name )
    {
        data_set = ds;
        table_name = tbl_name;
    }

    public DataSet GetUnderlyingDataSet()
    {
        return data_set;
    }



    public void MoveToFirst(int row_id)
    {
        DataRow[] rows_with_id = data_set.Tables[table_name].Select("id = '" + row_id.ToString() + "'" );

        if (rows_with_id.Length == 1)
        {
            DataRow row_to_be_moved = rows_with_id[0];

            int current_position = (int)row_to_be_moved["Position"];

            if ( current_position > 0)
            {
                String select_string = "Position < '" + current_position.ToString() + "'";
                DataRow[] rows = data_set.Tables[table_name].Select( select_string );

                foreach (DataRow row in rows)
                    row["Position"] = (int)row["Position"] + 1;

                row_to_be_moved["Position"] = 0;
            }
        }
        else if (rows_with_id.Length > 1)
        {
            throw new System.ApplicationException("DataSet is showing duplicated primary keys");
        }
    }



    public void MoveToLast(int row_id)
    {
        DataRow[] rows_with_id = data_set.Tables[table_name].Select("id = '" + row_id.ToString() + "'");

        if (rows_with_id.Length == 1)
        {
            DataRow row_to_be_moved = rows_with_id[0];

            int current_position = (int)row_to_be_moved["Position"];

            // super ugly hack to get the number of rows in the table
            if (current_position < data_set.Tables[table_name].Rows.Count - 1  )
            {
                String select_string = "Position > '" + current_position.ToString() + "'";
                DataRow[] rows = data_set.Tables[table_name].Select(select_string);

                foreach (DataRow row in rows)
                    row["Position"] = (int)row["Position"] - 1;

                row_to_be_moved["Position"] = data_set.Tables[table_name].Rows.Count - 1;
            }
        }
        else if (rows_with_id.Length > 1)
        {
            throw new System.ApplicationException("DataSet is showing duplicated primary keys");
        }
    }


    public DataSet MoveUp(int row_id)
    {
        return data_set;
    }



    public DataSet MoveDown(int row_id)
    {
        return data_set;
    }
}





public static class CategoryDB
{
    public static int max_category_name_length = 50;

    private static string conn_string;
    private static DbConnection connection;

    

    static CategoryDB()
    {
        
        conn_string = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Access"].ConnectionString;
        connection = new DbConnection(conn_string);

    }


    //TODO: Implement roles based authority
    //PRECONDITIONS:
    //  cat_name is <= max_category_name_length
    //
    public static bool InsertCategory( String cat_name )
    {
        if (cat_name.Length >= max_category_name_length )
        {
            throw new System.ArgumentException("category name length must be less than "
                + cat_name.ToString(), "cat_name");
        }

 
        DbCommand max_position_cmd = new DbCommand("SELECT MAX([Position]) FROM ForumCategories", connection);

        String insert_query_string = "INSERT INTO ForumCategories " +
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

            if( reader.Read() && !reader.IsDBNull(0) )
            {
                //TODO: verify actual bit size of OleDb.Integer
                max_position = reader.GetInt32(0);
            }

            DbCommand cmd = new DbCommand(insert_query_string, connection);
            cmd.Parameters.Add("@Name", DbType.WChar, 50).Value = cat_name;
            cmd.Parameters.Add("@Position", DbType.Integer).Value = max_position + 1;
            cmd.Parameters.Add("@CreationDate", DbType.Date).Value = DateTime.Now.ToString();
            cmd.ExecuteNonQuery();

            return true;
        }
        catch (DbException )
        {
            throw;
        }
        finally
        {
            connection.Close();
        }
    }


    public static DataSet GetAllCategories()
    {
        String sql = "SELECT * from ForumCategories";
        DbDataAdapter da = new DbDataAdapter(sql, connection);

        DataSet ds = new DataSet();
        try
        {
            connection.Open();
            da.Fill(ds, "ForumCategories");

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



    /*
     * Algorithm:
     *  Read in dataset, remove category, decrement all categories with positions > category just removed,
     *  write back out to database.
     */
    public static void DeleteCategory(String cat_name)
    {
        DataSet ds = CategoryDB.GetAllCategories();
        DataRow[] dr_collection = ds.Tables[0].Select("Name = '"+cat_name+"'");

        // Must not have more than 1 row with the category name
        if (dr_collection.Length > 1)
        {
            throw new System.ApplicationException("Multiple categories in the DB with the same name");
        }

        if (dr_collection.Length == 1)
        {
            DataRow dr = dr_collection[0];

            int curr_position = -1;

            if ((String)dr["Name"] == cat_name)
            {
                curr_position = (int)dr["Position"];
            }


            // Create an explicit UPDATE command
            DbDataAdapter da = new DbDataAdapter();
            da.UpdateCommand = connection.CreateCommand();


            da.UpdateCommand.CommandText =
                "UPDATE ForumCategories " +
                "SET [Position] = ?" + 
                "WHERE id = ?";

            // Bind parameters to appropriate columns for UPDATE command
            da.UpdateCommand.Parameters.Add("@Position", DbType.Integer, 0, "Position");
            da.UpdateCommand.Parameters.Add("@id", DbType.Integer, 0, "id");

            
            // Create an explicit DELETE command
            da.DeleteCommand = connection.CreateCommand();
            da.DeleteCommand.CommandText =
                "DELETE FROM ForumCategories " +
                "WHERE id = ?";


            // Bind parameters to appropriate columns for DELETE command
            da.DeleteCommand.Parameters.Add("@id", DbType.Integer, 0, "id");

            dr.Delete();

            da.Update(ds.Tables[0].GetChanges());
            ds.AcceptChanges();

            ds = GetAllCategories();

            
            // not sure if this works
            foreach (DataRow row in ds.Tables[0].Select("[Position] > '" + curr_position.ToString() + "'" ) )
            {
                if ( curr_position != -1 )
                {
                    int tmp = (int)row["Position"];
                    row["Position"] = (tmp - 1).ToString();

                    
                }
            }

            DataTable changes = ds.Tables[0].GetChanges();

            if (changes != null)
            {

                da.Update(ds.Tables[0].GetChanges());
                ds.AcceptChanges();
            }
            

        }
        // if category was not 
    }
}
