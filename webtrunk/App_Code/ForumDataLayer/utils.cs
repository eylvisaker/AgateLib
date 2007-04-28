using System;
using System.Data;
using System.Configuration;
using System.Web;

using DbConnection = System.Data.OleDb.OleDbConnection;
using DbCommand = System.Data.OleDb.OleDbCommand;
using DbType = System.Data.OleDb.OleDbType;
using DbDataReader = System.Data.OleDb.OleDbDataReader;
using DbParameter = System.Data.OleDb.OleDbParameter;
using DbTransaction = System.Data.OleDb.OleDbTransaction;
using DbException = System.Data.OleDb.OleDbException;
using DbDataAdapter = System.Data.OleDb.OleDbDataAdapter;


/// <summary>
/// Summary description for utils
/// </summary>
/// 

namespace DAL
{

    /// <remarks>
    /// Holds all connection related utilities
    /// </remarks>
    public static class ConnectionManager
    {

        private static string connection_string_name = "Access";

        /// <summary>
        ///  returns the default connection
        /// </summary>
        /// <returns>a newly instantiated connection that has not been opened</returns>
        public static DbConnection get_default_connection()
        {
            String conn_string = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[connection_string_name].ConnectionString;
            return new DbConnection(conn_string);
        }

        /// <summary>
        ///  returns the default connection
        /// </summary>
        /// <returns>a newly instantiated connection that has not been opened</returns>
        public static DbConnection get_testing_connection()
        {
            String conn_string = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["AccessTest"].ConnectionString;
            return new DbConnection(conn_string);
        }

        public static void change_default_connection_string(string new_string)
        {
            connection_string_name = new_string;
        }
    }




    /// <remarks>
    /// Manages a "simulated autonumber column".
    /// 
    /// Uses optimistic concurrency to ensure uniqueness of increment/decrements
    /// </remarks>
    public static class MaxRankManager
    {
        public const string table_name = "TrackMaxRanks";
        private static int max_tries = 10;

        public const int invalid_rank = -1;


        /// <summary>
        /// Increments the max rank
        /// </summary>
        /// <param name="table">column 1 of primary key</param>
        /// <param name="ident">column 2 of primary key</param>
        /// <returns>returns the new max rank</returns>
        public static int increment_max_rank( string table, string ident)
        {
            if (!exists(table, ident))
                create(table, ident);

            try
            {
                int prev_rank;
                int new_rank;
                int tries = 0;

                // If we can't get through in max_tries we give up
                while (tries < max_tries)
                {
                    prev_rank = current_max_rank(table,ident);
                    new_rank = prev_rank+1;

                    

                    if (save_new_rank(table, ident, prev_rank, new_rank))
                        return new_rank;

                    ++tries;
                }
                return invalid_rank;
            }
            catch { throw; }
        }

        /// <summary>
        /// decrements the specified max rank
        /// </summary>
        /// <param name="table">column 1 of primary key</param>
        /// <param name="ident">column 2 of primary key</param>
        /// <returns>returns the new max rank after decrement</returns>
        public static int decrement_max_rank(string table, string ident, int current_expected_rank)
        {
            try
            {
                if (!exists(table, ident))
                    create(table, ident);

                if (current_max_rank(table, ident) < 0)
                    return invalid_rank;

                int new_rank = current_expected_rank - 1;

                if (save_new_rank(table, ident, current_expected_rank, new_rank))
                    return new_rank;

                return invalid_rank;
            }
            catch { throw; }
        }

        /// <summary/>
        /// <param name="table">column 2 of primary key</param>
        /// <param name="ident">column 2 of primary key</param>
        /// <returns>specified current max rank</returns>
        public static int current_max_rank(string table, string ident)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            if (!exists(table, ident))
                return invalid_rank;

            try
            {
                conn.Open();

                string query = "SELECT([max_rank]) FROM `" + table_name + "`" +
                    " WHERE [table] = @table AND [ident] = @ident";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@table", DbType.Char,255).Value = table;
                cmd.Parameters.Add("@ident", DbType.Char,20).Value = ident;

                DbDataReader reader = cmd.ExecuteReader();

                // theoretically this check is redundant, but we're making it anyway :)
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                    return invalid_rank;

            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// creates a new rank with a unique table/ident combo
        /// </summary>
        /// <param name="table">column 1 of primary key</param>
        /// <param name="ident">column 2 of primary key</param>
        public static void create(string table, string ident)
        {
            if( !exists(table,ident))
            {
                DbConnection conn = ConnectionManager.get_default_connection();

                try
                {
                    conn.Open();

                    string insert_query = "INSERT INTO " + table_name + "([table],[ident],[max_rank]) " +
                        "VALUES(@table,@ident,@max_rank)";

                    DbCommand cmd = new DbCommand(insert_query, conn);
                    cmd.Parameters.Add("@table", DbType.Char, 255).Value = table;
                    cmd.Parameters.Add("@ident", DbType.Char, 20).Value = ident;
                    cmd.Parameters.Add("@max_rank", DbType.Integer).Value = invalid_rank;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                        // if the row exists that means another process created the row between the time we
                        // originally checked for the existence in this method and the time we actually created it.
                        // We don't care who creates the record, the important point is that it now exists and can
                        // be incremented/decremented as needed.  We only throw if the exception wasn't caused by
                        // non-unique primary key's.
                    catch (DbException)
                    {
                        if (!exists(table, ident))
                            throw;
                    }
                }
                catch { throw; }
                finally { conn.Close(); }
            }
        }

        /// <summary>
        /// checks for the existence of the table/ident combo
        /// </summary>
        /// <param name="table">column 1 of primary key</param>
        /// <param name="ident">column 2 of primary key</param>
        /// <returns></returns>
        public static bool exists(string table, string ident)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                conn.Open();

                string query = "SELECT * FROM " + table_name + 
                    " WHERE [table] = @table AND [ident] = @ident";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@table", DbType.Char,255).Value = table;
                cmd.Parameters.Add("@ident", DbType.Char,20).Value = ident;

                DbDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    return true;
                return false;
            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// Uses Optimistic Locking to save the new rank value
        /// </summary>
        /// <param name="table">column 1 of primary key</param>
        /// <param name="ident">column 2 of primary key</param>
        /// <param name="old_rank">previous rank expected</param>
        /// <param name="new_rank">new rank to save</param>
        /// <returns>returns true on success</returns>
        private static bool save_new_rank(string table, string ident, int old_rank, int new_rank)
        {


            if (!exists(table, ident))
                return false;

            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                conn.Open();

                string update_query = "UPDATE " + table_name +
                    " SET [max_rank] = @new_max_rank" +
                    " WHERE [table]=@table AND [ident] = @ident AND [max_rank] = @curr_max_rank";

                DbCommand cmd = new DbCommand(update_query, conn);
                cmd.Parameters.Add("@new_max_rank", DbType.Integer).Value = new_rank;

                cmd.Parameters.Add("@table", DbType.Char, 255).Value = table;
                cmd.Parameters.Add("@ident", DbType.Char, 20).Value = ident;
                cmd.Parameters.Add("@curr_max_rank", DbType.Integer).Value = old_rank;

                return (cmd.ExecuteNonQuery() > 0);
            }
            catch { throw; }
            finally { conn.Close(); }
        }

    }
}