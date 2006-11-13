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
    public void InsertCategory( CategoryDetails cat_details )
    {
        DbConnection conn = new DbConnection(conn_string);

        String query_string = "INSERT INTO ForumCategories "+
            " (Name, [Position], CreationDate)" +
            " Values('" + cat_details.Name + "'," + cat_details.Position + ",'" + DateTime.Now.ToString() + "')";

        DbCommand cmd = new DbCommand(query_string, conn);


        try
        {
            conn.Open();

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


    public void UpdateCategory(CategoryDetails cat_details)
    {

    }

}
