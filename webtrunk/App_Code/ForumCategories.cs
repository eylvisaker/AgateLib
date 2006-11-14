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
 * This is a POD.  All actual constraints will be checked in CategoryDB 
 * 
 */

public class CategoryDetails
{
    
    public String Name;
    public int Position;
    public DateTime CreationDate;

    internal int pID;

    public CategoryDetails()
    {
        // signifies that it hasn't been set yet
        pID = -1;
    }

    public int ID
    {
        get { return pID; }
    }
}



public class CategoryDB
{
    public static int max_category_name_length = 50;

    private string conn_string;

    

    public CategoryDB()
    {
        
        conn_string = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Access"].ConnectionString;

    }

    public CategoryDetails GetCategory( String cat_name )
    {
        CategoryDetails tmpDetails = new CategoryDetails();


        return tmpDetails;
    }


    //TODO: Implement roles based authority
    //PRECONDITIONS:
    //  cat_name is <= max_category_name_length
    //
    public bool InsertCategory( String cat_name )
    {
        if (cat_name.Length >= max_category_name_length )
        {
            throw new System.ArgumentException("category name length must be less than "
                + cat_name.ToString(), "cat_name");
        }

        
        DbConnection conn = new DbConnection(conn_string);
        DbCommand max_position_cmd = new DbCommand("SELECT MAX([Position]) FROM ForumCategories", conn);

        String insert_query_string = "INSERT INTO ForumCategories " +
            " (Name, [Position], CreationDate)" +
            " Values(?,?,?)";

        DbDataReader reader = null;

        // Open connection, query for the max position, update db with new category of position + 1
        try
        {
            conn.Open();
            reader = max_position_cmd.ExecuteReader();

            // If there are no Categories in the table, this will be incremented to 0.
            int max_position = -1;

            if( reader.Read() && !reader.IsDBNull(0) )
            {
                //TODO: verify actual bit size of OleDb.Integer
                max_position = reader.GetInt32(0);
            }

            DbCommand cmd = new DbCommand(insert_query_string, conn);
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
            conn.Close();
        }
    }

    public DataSet GetAllCategories()
    {
        DbConnection con = new DbConnection(conn_string);
        String sql = "SELECT * from ForumCategories";
        DbDataAdapter da = new DbDataAdapter(sql, con);

        DataSet ds = new DataSet();
        try
        {
            con.Open();
            da.Fill(ds, "ForumCategories");

            return ds;
        }
        catch (DbException e)
        {
            throw;
        }
        finally
        {
            con.Close();
        }
    }

    /*
     * Algorithm:
     *  Read in dataset, remove category, decrement all categories with positions > category just removed,
     *  write back out to database.
     */
    public void DeleteCategory(String cat_name)
    {
        DataSet ds = this.GetAllCategories();
        DataRow[] dr_collection = ds.Tables[0].Select("Name = '"+cat_name+"'");

        // Must not have more than 1 row with the category name
        if (dr_collection.Length > 1)
        {
            throw new System.ApplicationException("Multiple categories in the DB with the same name");
        }

        if (dr_collection.Length == 1)
        {
            DataRow dr = dr_collection[0];

            int curr_position;

            if ((String)dr["Name"] == cat_name)
            {
                curr_position = (int)dr["Position"];
            }
        }
    }
}
