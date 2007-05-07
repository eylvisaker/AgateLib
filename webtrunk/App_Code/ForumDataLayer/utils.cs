using System;
using System.Data;
using System.Configuration;
using System.Web;


using System.Collections.Generic;

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



    public class rank_tuple
    {
        public int id;
        public int rank;

        public static int CompareById(rank_tuple l, rank_tuple r)
        {
            if (l.id < r.id)
                return -1;
            else if (l.id > r.id)
                return 1;
            else
                return 0;
        }
    }



    /// <remarks>
    /// Manages a "simulated autonumber column".
    /// 
    /// Uses optimistic concurrency to ensure uniqueness of increment/decrements
    /// </remarks>
    public static class RankManager
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



    class RankManipulator
    {
        private string _table;
        private string _ident;
        private int _id;
        private string _parent_id_column_name;
        private int _parent_id;

        public RankManipulator(string table, string ident, int id, string parent_id_col, int parent_id)
        {
            _table = table;
            _ident = ident;
            _id = id;
            _parent_id_column_name = parent_id_col;
            _parent_id = parent_id;
        }


        public bool increment_rank()
        {
            return true;
        }

        public bool decrement_rank()
        {
            return true;
        }

        public bool move_to_largest_rank()
        {
            return change_rank(RankManager.current_max_rank(_table,_ident));
        }

        public bool move_to_smallest_rank()
        {
            return change_rank(0);
        }


        private bool change_rank(int new_rank)
        {
            int curr_max_rank = RankManager.current_max_rank(_table, _ident);
            List<rank_tuple> list = fill_list_from_reader( get_reader() );

            if (list.Count <= 0)
                return false;

            list.Sort(rank_tuple.CompareById);

            rank_tuple id_tuple = find_id_tuple(list, _id);

            if (id_tuple == null)
                return false;

            if (new_rank > curr_max_rank)
                throw new System.ArgumentException("The new rank being assigned is too large");
            if (new_rank < 0)
                throw new System.ArgumentException("The new rank being assigned is < 0 ");
            if (!rank_exists(list, new_rank))
                throw new System.ArgumentException("The new rank does not exist in the list.  Verify no gaps in ranks, et al ");

            int index = list.IndexOf(id_tuple);

            // decide which direction to go
            if (id_tuple.rank > new_rank)
                return change_rank_decrement(ref list, index, new_rank, 0);
            else if (id_tuple.rank < new_rank)
                return change_rank_increment(ref list, index, new_rank, curr_max_rank);

            update_from_list(list);
            return true;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region change rank implementation
        /*
         * Algorithm:
         *  Take the tuple who's rank is being changed(id_tuple) and swap it's current rank with the tuple that's "next in line".
         *  Continue to do this until the id_tuple's rank is what we want it to be.
         * 
         * The reason we're using this algorithm rather than simply incrementing/decrementing all the relevant ranks is because the
         * ranks may have gaps in them and we want to leave the gaps as they are.  Swapping them will result in leaving the gaps.
         * 
         * ie, we have ranks {1,2,4,5} so we're missing rank 3.  When we finish we want to still be missing rank 3 but we want the
         * actual tuples that correspond to those ranks to have changed.
         * 
         */

        private bool change_rank_increment(ref List<rank_tuple> list, int index, int new_rank, int curr_max_rank)
        {
            int curr_index = index;
            int tmp_rank;
            rank_tuple id_tuple = list[index];
            while (id_tuple.rank < new_rank)
            {
                curr_index++;
                if (curr_index <= curr_max_rank)
                {
                    // manual swap of list[index] && list[current_index]
                    // C# makes creating a swap method for container elements a ridiculous PITA
                    tmp_rank = list[index].rank;
                    list[index].rank = list[curr_index].rank;
                    list[curr_index].rank = tmp_rank;
                }
                else
                    break;
            }

            return true;
        }

        private bool change_rank_decrement(ref List<rank_tuple> list, int index, int new_rank, int floor)
        {
            int curr_index = index;
            int tmp_rank;
            rank_tuple id_tuple = list[index];
            while (id_tuple.rank > new_rank)
            {
                curr_index--;
                if (curr_index >= floor)
                {
                    // manual swap of list[index] & list[current_index]
                    // C# makes creating a swap method for container elements a ridiculous PITA
                    tmp_rank = list[index].rank;
                    list[index].rank = list[curr_index].rank;
                    list[curr_index].rank = tmp_rank;
                }
                else
                    break;
            }
            return true;
        }
        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private DbDataReader get_reader()
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                // we avoid SELECT * so that we have control over the exact order that the attributes are returned in.
                // Note that fill_list_from_reader depends on this order
                string query = "SELECT [id],[rank] FROM " + _table + " " +
                    "WHERE [id] = @id AND [" + _parent_id_column_name + "] = @parent_id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@id", DbType.Integer).Value = _id;
                cmd.Parameters.Add("@parent_id", DbType.Integer).Value = _parent_id;

                conn.Open();
                return cmd.ExecuteReader();
            }
            catch { throw; }
            finally { conn.Close(); }
        }


        private List<rank_tuple> fill_list_from_reader(DbDataReader reader)
        {
            try
            {
                List<rank_tuple> list = new List<rank_tuple>();

                if (!reader.HasRows)
                    return list;

                while (reader.Read())
                {
                    rank_tuple tuple = new rank_tuple();

                    tuple.id = reader.GetInt32(0);
                    tuple.rank = reader.GetInt32(1);

                    list.Add(tuple);
                }
                return list;
            }
            finally { }

        }

        private void update_from_list(List<rank_tuple> list)
        {
        }


        private static rank_tuple find_id_tuple(List<rank_tuple> list, int id)
        {
            rank_tuple id_tuple = null;
            foreach (rank_tuple curr_tuple in list)
            {
                if (curr_tuple.id == id)
                {
                    id_tuple = curr_tuple;
                    break;
                }
            }

            return id_tuple;
        }

        private static bool rank_exists(List<rank_tuple> list, int new_rank)
        {
            foreach (rank_tuple curr_tuple in list)
            {
                if (curr_tuple.rank == new_rank)
                    return true;
            }
            return false;
        }

    }
}