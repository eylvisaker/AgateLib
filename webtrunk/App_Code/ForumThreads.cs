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
///  Static Data Access Layer for the ForumThreads table
/// </summary>
public static class ThreadDAL
{
    private static int max_type_length = 3;
    private static int max_priority_length = 3;
    private static string table_name = "ForumThreads";

    /// <summary>
    /// Insert a new thread
    /// </summary>
    /// <param name="parent_id"></param>
    /// <param name="user_id"></param>
    /// <param name="icon_index"></param>
    /// <param name="priority"></param>
    /// <param name="type"></param>
    public static void Insert(int parent_id, int user_id, int icon_index, String priority, String type)
    {
        priority = priority.ToUpper();
        type = type.ToUpper();

        if (!validate_parent_id(parent_id))
            throw new System.ArgumentException(parent_id.ToString() + " is an invalid Board ID");

        if( !validate_user_id(user_id))
            throw new System.ArgumentException(user_id.ToString() + " is an invalid User ID");

        if (!validate_icon_index(icon_index))
            throw new System.ArgumentException(icon_index.ToString() + " is an invalid icon index");

        if (!validate_thread_priority_string(priority))
            throw new System.ArgumentException(priority + "is an invalid priority string");

        if (!validate_thread_type_string(type))
            throw new System.ArgumentException(type + "is an invalid type string");
        /////////////////////////////////////////////////////////////////////////////
        


        DbConnection connection = get_connection();

        String insert_query_string = "INSERT INTO " + table_name +
            " (parent_id, user_id, icon_index, creation_date, priority, type)" +
            " Values(?,?,?,?,?,?)";

        //DbDataReader reader = null;

        try
        {
            connection.Open();

            DbCommand cmd = new DbCommand(insert_query_string, connection);

            cmd.Parameters.Add("@parent_id", DbType.Integer).Value = parent_id;
            cmd.Parameters.Add("@user_id", DbType.Integer).Value = user_id;
            cmd.Parameters.Add("@icon_index", DbType.Integer).Value = icon_index;
            cmd.Parameters.Add("@creation_date", DbType.Date).Value = DateTime.Now.ToString();
            cmd.Parameters.Add("@priority", DbType.WChar, max_priority_length).Value = priority;
            cmd.Parameters.Add("@type", DbType.WChar, max_type_length).Value = type;

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
    ///  Delete a single thread from the table
    /// </summary>
    /// <param name="thread_id">primary key for the thread</param>
    public static void Delete(int thread_id)
    {
        if (!validate_thread_id(thread_id))
            throw new System.ArgumentException(thread_id.ToString() + " is an Invalid Thread ID");
        /////////////////////////////////////////////////////////////////////////////////////////

        /* - create delete command, set command text, bind parameters, & set connection
         * - open connection, execute query, close connection
         */


        DbCommand delete_cmd = new DbCommand();
        delete_cmd.CommandText = 
            "DELETE FROM " + table_name +
            " WHERE id = ?";

        delete_cmd.Parameters.Add("@id", DbType.Integer).Value = thread_id;

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
    ///  Delete all children of the parent Board
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

        DbCommand cmd = new DbCommand();
        cmd.CommandText =
            "DELETE FROM " + table_name +
            " WHERE parent_id = ?";

        cmd.Parameters.Add("@id", DbType.Integer).Value = parent_id;

        cmd.Connection = get_connection();

        try
        {
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();

            return true;
        }
        catch (DbException e)
        {
            throw;
        }
        finally
        {
            cmd.Connection.Close();
        }
    }


    /// <summary>
    /// Update a thread
    /// </summary>
    /// <param name="thread_id"></param>
    /// <param name="parent_id"></param>
    /// <param name="user_id"></param>
    /// <param name="icon_index"></param>
    /// <param name="priority"></param>
    /// <param name="type"></param>
    public static void Update(int thread_id, int parent_id, int user_id, int icon_index, String priority, String type)
    {
        priority = priority.ToUpper();
        type = type.ToUpper();

        if (!validate_parent_id(parent_id))
            throw new System.ArgumentException(parent_id.ToString() + " is an invalid Board ID");

        if (!validate_user_id(user_id))
            throw new System.ArgumentException(user_id.ToString() + " is an invalid User ID");

        if (!validate_icon_index(icon_index))
            throw new System.ArgumentException(icon_index.ToString() + " is an invalid icon index");

        if (!validate_thread_priority_string(priority))
            throw new System.ArgumentException(priority + "is an invalid priority string");

        if (!validate_thread_type_string(type))
            throw new System.ArgumentException(type + "is an invalid type string");
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
            " SET parent_id = ?, user_id = ?, icon_index = ?, priority = ?, type = ?" +
            " WHERE id = ?";

        // SET parent_id = ?, user_id = ?, icon_index = ?, priority = ?, type = ?
        update_cmd.Parameters.Add("@parent_id", DbType.Integer).Value = parent_id;
        update_cmd.Parameters.Add("@user_id", DbType.Integer).Value = user_id;
        update_cmd.Parameters.Add("@icon_index", DbType.Integer).Value = icon_index;
        update_cmd.Parameters.Add("@priority", DbType.WChar, max_priority_length).Value = priority;
        update_cmd.Parameters.Add("@type", DbType.WChar, max_type_length).Value = type;

        // WHERE id = 
        update_cmd.Parameters.Add("@id", DbType.Integer, 0).Value = thread_id;

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
    ///  Retrieves data set of all threads with the given parent id
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
    /// Tests to see if a thread with the id exists
    /// </summary>
    /// <param name="thread_id"></param>
    /// <returns></returns>
    public static bool Exists(int thread_id)
    {
        DbConnection connection = get_connection();

        String sql = "SELECT id FROM " + table_name + " WHERE id = " + thread_id.ToString();
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
    /// retrieve the parent id of the given thread
    /// </summary>
    /// <param name="thread_id"></param>
    /// <returns></returns>
    /// we don't worry about verifything thread_id.  If it's not valid, we return -1 which means
    /// 
    private static int GetParentID(int thread_id)
    {
        if (!validate_thread_id(thread_id))
            throw new System.ArgumentException(thread_id.ToString() + " is an invalid ID");
        ///////////////////////////////////////////////////////////////////////////////

        DbConnection connection = get_connection();

        String sql = "SELECT parent_id FROM " + table_name + " WHERE id = " + thread_id.ToString();
        DbCommand cmd = new DbCommand(sql, connection);

        /*
         * - open connection, execute reader
         * - retrieve parent_id, check parent_id, then return parent_id
         */
        try
        {
            connection.Open();
            DbDataReader reader = cmd.ExecuteReader();

            int parent_id;
            if (reader.Read() && !reader.IsDBNull(0))
            {
                //TODO: verify actual bit size of OleDb.Integer
                parent_id = reader.GetInt32(0);

                return parent_id;
            }
            else
                throw new System.ApplicationException("thread with id: " + thread_id.ToString() + " has no parent ID value");
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
    /// checks to see if thread id is valid
    /// </summary>
    /// <remarks>
    /// thread_id must be >= 0 and it must exist in the ForumThreads table
    /// </remarks>
    /// <param name="thread_id"></param>
    /// <returns></returns>
    private static bool validate_thread_id(int thread_id)
    {
        if (thread_id < 0 || !ThreadDAL.Exists(thread_id))
            return false;

        return true;
    }


    /// <summary>
    /// validates the parent board ID
    /// </summary>
    /// <param name="parent_id"></param>
    /// <returns>Returns true on successful validation</returns>
    private static bool validate_parent_id(int parent_id)
    {
        if (parent_id < 0 || !BoardDAL.Exists(parent_id))
            return false;

        return true;
    }


    

    /// <summary>
    ///  validates the user ID
    /// </summary>
    /// <param name="user_id"></param>
    /// <returns>Returns true on successful validation</returns>
    ///TODO: this is a bad way to validate and will not be used in the end
    private static bool validate_user_id(int user_id)
    {
        return true;
    }


    /// <summary>
    /// Validates the icon index
    /// </summary>
    /// <param name="icon_index"></param>
    /// <returns>Returns true on successful validation</returns>
    /// TODO: No idea how this will ultimately be implemented
    private static bool validate_icon_index( int icon_index)
    {
        return true;
    }


    /// <summary>
    /// Validates the priority string
    /// </summary>
    /// <remarks>
    /// Possible values are:
    /// "NML" -> "Normal"
    /// "STK" -> "Sticky"
    /// "ANN" -> "Announcement"
    /// 
    /// Anything else should probably default to normal, but we fail fast & often so as to catch
    /// as many bugs as we can
    /// </remarks>
    /// <param name="priority_string"></param>
    /// <returns>Returns true on validation</returns>
    private static bool validate_thread_priority_string( String priority_string)
    {
        String upper = priority_string.ToUpper();
        // NML is Normal, STK is Sticky, ANN is Announcement
        if (upper == "NML" || upper == "STK" || upper == "ANN")
            return true;

        return false;
    }


    /// <summary>
    /// Validates the type string
    /// </summary>
    /// <remarks>
    /// "NML" -> "Normal"
    /// "PLL" -> "Poll"
    /// "SPR" -> "Spoiler"
    /// </remarks>
    /// <param name="type_string"></param>
    /// <returns>Returns true on validation</returns>
    private static bool validate_thread_type_string( String type_string)
    {
        String upper = type_string.ToUpper();
        // NML is Normal, PLL is Poll, SPR is Spoiler
        if (upper == "NML" || upper == "PLL" || upper == "SPR")
            return true;

        return false;
    }
}
