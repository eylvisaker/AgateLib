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

    // TODO: check contents of cat_details
    // TODO: find new id and return cat_details
    // TODO: check that cat_name is < 50 characters

    /*
     * Create connection, query for the max position, update db with new category of position + 1
     */
    public void InsertCategory( String cat_name )
    {
        DbConnection conn = new DbConnection(conn_string);

        // Position is a Jet Reserved Word
        DbCommand max_position_cmd = new DbCommand("SELECT MAX([Position]) FROM ForumCategories", conn);

        String query_string = "INSERT INTO ForumCategories "+
            " (Name, [Position], CreationDate)" +
            " Values(?,?,?)";

        DbDataReader reader = null;

        try
        {

            conn.Open();
            reader = max_position_cmd.ExecuteReader();

            // If there are no Categories in the table, this will be incremented to 0.
            int max_position = -1;

            if( reader.Read() && !reader.IsDBNull(0) )
            {
                max_position = reader.GetInt32(0);
            }

            DbCommand cmd = new DbCommand(query_string, conn);
                cmd.Parameters.Add("@Name", DbType.WChar, 50).Value = cat_name;
                cmd.Parameters.Add("@Position", DbType.Integer).Value = max_position + 1;
                cmd.Parameters.Add("@CreationDate", DbType.Date).Value = DateTime.Now.ToString();
            cmd.ExecuteNonQuery();
        }
        catch (DbException e)
        {
            throw e;
        }
        finally
        {
            conn.Close();
        }

    }


    public void DeleteCategory(CategoryDetails cat_details)
    {

    }

}
