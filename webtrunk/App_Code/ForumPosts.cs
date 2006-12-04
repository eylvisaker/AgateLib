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
/// Summary description for ForumPosts
/// </summary>
public static class PostDAL
{
    private static int max_body_length = 1024;

    private static String table_name = "ForumPosts";

    /// <summary>
    /// Insert a new post
    /// </summary>
    /// <param name="parent_id"></param>
    /// <param name="user_id"></param>
    /// <param name="body">text of post</param>
    public static void Insert(int parent_id, int user_id, String body)
    {
        if (!validate_parent_id(parent_id))
            throw new System.ArgumentException(parent_id.ToString() + " is not a valid parent id");

        if (!validate_user_id(user_id))
            throw new System.ArgumentException(user_id.ToString() + " is not a valid user id");

        if (!validate_body(body))
            throw new System.ArgumentException("The post body is invalid");
        ///////////////////////////////////////////////////////////////////

        DbConnection connection = get_connection();

        String insert_query_string = "INSERT INTO " + table_name +
            " (parent_id, user_id, creation_date, body)" +
            " Values(?,?,?,?)";

        //DbDataReader reader = null;

        try
        {
            connection.Open();

            DbCommand cmd = new DbCommand(insert_query_string, connection);

            cmd.Parameters.Add("@parent_id", DbType.Integer).Value = parent_id;
            cmd.Parameters.Add("@user_id", DbType.Integer).Value = user_id;
            cmd.Parameters.Add("@creation_date", DbType.Date).Value = DateTime.Now.ToString();
            cmd.Parameters.Add("@body", DbType.Char, max_body_length).Value = body;

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
    ///  Delete a single post from the table
    /// </summary>
    /// <param name="post_id">primary key for the post</param>
    public static void Delete(int post_id)
    {
        if (!validate_post_id(post_id))
            throw new System.ArgumentException(post_id.ToString() + " is an Invalid Post ID");
        /////////////////////////////////////////////////////////////////////////////////////////

        /* - create delete command, set command text, bind parameters, & set connection
         * - open connection, execute query, close connection
         */


        DbCommand delete_cmd = new DbCommand();
        delete_cmd.CommandText =
            "DELETE FROM " + table_name +
            " WHERE id = ?";


        delete_cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;
        delete_cmd.Connection = get_connection();

        try
        {
            delete_cmd.Connection.Open();
            delete_cmd.ExecuteNonQuery();
        }
        catch (DbException e)
        { throw; }
        finally
        { delete_cmd.Connection.Close(); }
    }


    /// <summary>
    ///  Delete all children of the parent Thread
    /// </summary>
    /// <param name="parent_id"> primary key for the parent</param>
    /// <returns>returns true on success</returns>
    public static bool DeleteAllByParentID(int parent_id)
    {
        if (!validate_parent_id(parent_id))
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
    ///  Update a threads name, parent id, & description
    /// </summary>
    /// <param name="post_id"></param>
    /// <param name="body"></param>
    public static void Update(int post_id, String body)
    {
        if (!validate_body(body))
            throw new System.ArgumentException("the body is invalid");
        /////////////////////////////////////////////////////////////////////////////

        /*
         * - create command, set command text for query
         * - bind parameter values
         * - set command connection
         * - open connection, execute query, close connection
         */

        DbCommand update_cmd = new DbCommand();
        update_cmd.CommandText =
            "UPDATE " + table_name +
            " SET body = ?" +
            " WHERE id = ?";

        // SET body = ?
        update_cmd.Parameters.Add("@body", DbType.Char, max_body_length).Value = body;

        // WHERE id = 
        update_cmd.Parameters.Add("@id", DbType.Integer, 0).Value = post_id;

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
    ///  Retrieves data set of all posts with the given parent id
    /// </summary>
    /// <param name="parent_id"></param>
    /// <returns></returns>
    public static DataSet DataSet(int parent_id)
    {
        if (!validate_parent_id(parent_id))
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
    /// Tests to see if a post with the id exists
    /// </summary>
    /// <param name="post_id"></param>
    /// <returns></returns>
    public static bool Exists(int post_id)
    {
        DbConnection connection = get_connection();

        String sql = "SELECT id FROM " + table_name + " WHERE id = " + post_id.ToString();
        DbCommand cmd = new DbCommand(sql, connection);

        /*
         * - open connection, execute reader
         * - if reader successfully executed, return true;
         */
 
        try
        {
            connection.Open();
            DbDataReader reader = cmd.ExecuteReader();

            if (reader.Read() && !reader.IsDBNull(0))
                return true;

            return false;
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
    ///  returns the connection for the table
    /// </summary>
    /// <returns></returns>
    private static DbConnection get_connection()
    {
        String conn_string = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Access"].ConnectionString;
        return new DbConnection(conn_string);
    }


    /// <summary>
    /// validates the parent post ID
    /// </summary>
    /// <param name="parent_id"></param>
    /// <returns>Returns true on successful validation</returns>
    private static bool validate_parent_id(int parent_id)
    {
        if (parent_id < 0 || !ThreadDAL.Exists(parent_id))
            return false;

        return true;
    }



    /// <summary>
    /// checks to see if post id is valid
    /// </summary>
    /// <remarks>
    /// post_id must be >= 0 and it must exist in the ForumPosts table
    /// </remarks>
    /// <param name="post_id"></param>
    /// <returns></returns>
    private static bool validate_post_id(int post_id)
    {
        if (post_id < 0 || !PostDAL.Exists(post_id))
            return false;

        return true;
    }



    /// <summary>
    ///  validates the user ID
    /// </summary>
    /// <param name="user_id"></param>
    /// <returns>Returns true on successful validation</returns>
    /// 
    ///TODO: this is a bad way to validate and will not be used in the end
    private static bool validate_user_id(int user_id)
    {
        return true;
    }


    /// <summary>
    /// validates the body of the post
    /// </summary>
    /// <param name="body"></param>
    /// <returns>returns true on successful validation</returns>
    private static bool validate_body(String body)
    {
        if (body == "" || body == null)
            return false;

        return true;
    }
}
