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

using System.Collections.Generic;

// TODO: update all methods to protect against bad inputs

namespace DAL
{
    /// <remarks>
    ///  Carrier for the Forum Data
    /// </remarks>
    public class ForumPostData
    {
        private int _id;

        // the id is immutable
        public ForumPostData(int primary_key)
        {
            _id = primary_key;
        }

        public int id
        {
            get { return _id; }
        }

        public int user_id;
        public int thread_id;
        public DateTime created_on;
        public DateTime edited_on;
        public int rank;
        public string body;
        public bool is_deleted;
    }

    /// <remarks>
    /// Data Access Layer for Forum Posts
    /// </remarks>
    public static class ForumPost
    {
        public const string table_name = "ForumPosts";

        public const int invalid_post_id = -1;

        // used for retrieving multiple posts at a time
        public const bool DESCENDING_RANK = true;
        public const bool ASCENDING_RANK = false;

        // retrieve all posts in the paginate methods
        public const int ALL_POSTS = -1;

        /// <summary>
        /// create a new post and give it the highest current rank
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="thread_id">parent thread id</param>
        /// <param name="body"></param>
        /// <returns></returns>
        /// Create a new post, assign it the default rank of invalid, increment the current max rank
        /// and attempt to update the recently created post with its actual rank.  If this fails
        /// attempt to decrement the current max rank and then delete the post (whether the increment
        /// was successful or not)
        public static int create(int user_id, int thread_id, string body)
        {
            int post_id = real_create(user_id, thread_id, body);
            int max_rank = DAL.RankManager.increment_max_rank(table_name, thread_id.ToString());

            if ( max_rank < 0 || !update_rank(post_id, max_rank, thread_id))
            {
                // attempt to decrement value then really delete the post that was just created.
                // if decrement fails we'll simply have a gap in our ranks.
                // decrement_max_rank will fail if someone else has already incremented the max rank between
                // the time we incremented it and the time we attempted to decrement it.
                DAL.RankManager.decrement_max_rank(table_name, thread_id.ToString(), max_rank);
                real_delete(post_id);
                return invalid_post_id;
            }

            return post_id;
        }

        /// <summary>
        /// retrieve a List<ForumPostData> with a single post.  Will filter out all
        /// deleted posts
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static List<ForumPostData> retrieve_filter_deleted(int post_id)
        {
            if (!is_deleted(post_id))
                return retrieve_no_filter_deleted(post_id);
            else
                return new List<ForumPostData>();
        }

        /// <summary>
        /// retrieve a List<ForumPostData> with a single post.  Will NOT filter out
        /// deleted posts
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static List<ForumPostData> retrieve_no_filter_deleted(int post_id)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                // we avoid SELECT * so that we have control over the exact order that the attributes are returned in.
                // Note that fill_list_from_reader depends on this order
                string query = "SELECT [id],[user_id],[thread_id],[created_on],[edited_on],[rank],[body],[is_deleted] FROM " + table_name + " " +
                    "WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();

                return fill_list_from_reader(reader);
            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// retrieve all posts with the specified parent thread that aren't deleted
        /// </summary>
        /// <param name="thread_id"></param>
        /// <param name="is_descending"></param>
        /// <returns></returns>
        public static List<DAL.ForumPostData> retrieve_by_thread_filter_deleted(int thread_id, bool is_descending)
        {
            try
            {
                return paginate_filter_deleted(thread_id, ALL_POSTS, 0, is_descending);
            }
            finally { }
        }

        /// <summary>
        /// /// retrieve all posts with the specified parent thread even if they're deleted
        /// </summary>
        /// <param name="thread_id"></param>
        /// <param name="is_descending"></param>
        /// <returns></returns>
        public static List<DAL.ForumPostData> retrieve_by_thread_no_filter_deleted(int thread_id, bool is_descending)
        {
            try
            {
                return paginate_no_filter_deleted(thread_id, ALL_POSTS, 0, is_descending);
            }
            finally { }
        }

        /// <summary>
        /// update the post with the deleted flag
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static bool delete(int post_id)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "UPDATE " + table_name +
                    " SET [is_deleted] = @is_deleted " +
                    "WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@is_deleted", DbType.Boolean).Value = true;

                cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();
                return (cmd.ExecuteNonQuery() > 0);
            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// update the post by removing the deleted flag
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static bool undelete(int post_id)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "UPDATE " + table_name +
                    " SET [is_deleted] = @is_deleted " +
                    "WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@is_deleted", DbType.Boolean).Value = false;

                cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();
                return (cmd.ExecuteNonQuery() > 0);
            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// is the post deleted?
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static bool is_deleted(int post_id)
        {
            if( !exists(post_id) )
                throw new System.ArgumentException("Attempt to delete non-existent post with id: " + post_id.ToString());


            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "SELECT [is_deleted] FROM " + table_name +
                    " WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();

                //string str = cmd.ExecuteScalar().ToString();
                //throw new System.Exception(cmd.ExecuteScalar().ToString());

                bool is_deleted = Convert.ToBoolean(cmd.ExecuteScalar().ToString());

                return is_deleted;

            }
            finally { conn.Close(); }
        }

        /// <summary>
        /// Does the post with post_id exist?
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        public static bool exists(int post_id)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "SELECT [rank] FROM " + table_name + " " +
                    "WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();

                return (reader.HasRows);
            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// Update the general information of the post.  Does *not* update the thread_id or the rank
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="user_id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool update_gen_info(int post_id, int user_id, string body)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "UPDATE " + table_name +
                    " SET [user_id] = @user_id, [edited_on] = @edited_on, [body] = @body " +
                    "WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                    cmd.Parameters.Add("@user_id", DbType.Integer).Value = user_id;
                    cmd.Parameters.Add("@edited_on", DbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("@body", DbType.Char).Value = body;

                    cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();
                return (cmd.ExecuteNonQuery() > 0); // returns true if a post was actually updated
            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// update the thread_id and rank of the post
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="rank"></param>
        /// <param name="thread_id"></param>
        /// <returns></returns>
        public static bool update_rank(int post_id, int rank, int thread_id)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "UPDATE " + table_name +
                    " SET [rank] = @rank, [thread_id] = @thread_id " +
                    "WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@rank",DbType.Integer).Value = rank;
                cmd.Parameters.Add("@thread_id", DbType.Integer).Value = thread_id;
                cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();
                return (cmd.ExecuteNonQuery() > 0);
            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// paginate all posts with the given thread_id, filter out all deleted posts
        /// </summary>
        /// <param name="thread_id"></param>
        /// <param name="per_page"></param>
        /// <param name="page_number"></param>
        /// <param name="is_descending"></param>
        /// <returns></returns>
        public static List<DAL.ForumPostData> paginate_filter_deleted(int thread_id, int per_page, int page_number, bool is_descending)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                // ============= construct query ===============
                string query = "SELECT [id],[user_id],[thread_id],[created_on],[edited_on],[rank],[body],[is_deleted] FROM " + table_name + " " +
                  "WHERE [thread_id] = @thread_id AND [is_deleted] = @is_deleted";

                // we only paginate if per_page and page_number is valid
                if (per_page >= 0 && page_number >= 0)
                {
                    query += " AND [rank] >= @first_rank AND [rank] < @last_rank";
                }
                // construct the ORDER BY clause
                query += " ORDER BY [rank]";

                if (is_descending)
                {
                    query += " DESC";
                }
                else
                {
                    query += " ASC";
                }
                // ==========================================

                int first_rank = per_page * page_number;
                int last_rank = first_rank + per_page;

                // We must be very careful here.  OleDb doesn't actually understand named parameters so
                // we must be careful to add the parameters in the order that they're defined in the query
                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@thread_id", DbType.Integer).Value = thread_id;
                cmd.Parameters.Add("@is_deleted", DbType.Boolean).Value = false;

                // we only add these if we're paginating
                if (per_page >= 0 && page_number >= 0)
                {
                    cmd.Parameters.Add("@first_rank", DbType.Integer).Value = first_rank;
                    cmd.Parameters.Add("@last_rank", DbType.Integer).Value = last_rank;
                }

                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();

                return fill_list_from_reader(reader);
            }
            finally { conn.Close(); }
        }

        /// <summary>
        /// paginate all posts with the given thread_id, do *NOT* filter out deleted posts
        /// </summary>
        /// <param name="thread_id"></param>
        /// <param name="per_page"></param>
        /// <param name="page_number"></param>
        /// <param name="is_descending"></param>
        /// <returns></returns>
        public static List<DAL.ForumPostData> paginate_no_filter_deleted(int thread_id, int per_page, int page_number, bool is_descending)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                // ============= construct query ===============
                string query = "SELECT [id],[user_id],[thread_id],[created_on],[edited_on],[rank],[body],[is_deleted] FROM " + table_name + " " +
                  "WHERE [thread_id] = @thread_id";

                // we only paginate if per_page and page_number is valid
                if (per_page >= 0 && page_number >= 0)
                {
                    query +=  " AND [rank] >= @first_rank AND [rank] < @last_rank"; 
                }
                // construct the ORDER BY clause
                query += " ORDER BY [rank]";

                if (is_descending)
                {
                    query += " DESC";
                }
                else
                {
                    query += " ASC";
                }
                // ==========================================

                int first_rank = per_page * page_number;
                int last_rank = first_rank + per_page;

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@thread_id", DbType.Integer).Value = thread_id;

                // we only add these if we're paginating
                if (per_page >= 0 && page_number >= 0)
                {
                    cmd.Parameters.Add("@first_rank", DbType.Integer).Value = first_rank;
                    cmd.Parameters.Add("@last_rank", DbType.Integer).Value = last_rank;
                }

                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();

                return fill_list_from_reader(reader);
            }
            finally { conn.Close(); }
        }

        public static DateTime get_edited_on(int post_id)
        {
            if (!exists(post_id))
                throw new System.ArgumentException("Attempt to delete non-existent post with id: " + post_id.ToString());

            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "SELECT [edited_on] FROM " + table_name +
                    " WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();

                DateTime edited_on = Convert.ToDateTime(cmd.ExecuteScalar().ToString());

                return edited_on;
            }
            finally { conn.Close(); }
        }

        /// <summary>
        /// fills a List<ForumPostData> object with all objects returned by the given reader
        /// NOTE:  Depends on the order: id,user_id,thread_id,created_on,edited_on,rank,body,is_deleted
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<ForumPostData> fill_list_from_reader(DbDataReader reader)
        {
            try{
            List<ForumPostData> list = new List<ForumPostData>();

                if (!reader.HasRows)
                    return list;

                int temp_id;
                while (reader.Read())
                {
                    temp_id = reader.GetInt32(0);
                    ForumPostData data = new ForumPostData(temp_id);

                    data.user_id = reader.GetInt32(1);
                    data.thread_id = reader.GetInt32(2);
                    data.created_on = reader.GetDateTime(3);
                    data.edited_on = reader.GetDateTime(4);
                    data.rank = reader.GetInt32(5);
                    data.body = reader.GetString(6).Trim();
                    data.is_deleted = reader.GetBoolean(7);

                    list.Add(data);
                }
                return list;
            }
            finally{}

        }

        /// <summary>
        /// does the actual creation of the post in the DB
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="thread_id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private static int real_create(int user_id, int thread_id, string body)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "INSERT INTO " + table_name +
                    "([user_id],[thread_id],[created_on],[edited_on],[rank],[body]) " +
                        "VALUES(@user_id, @thread_id, @created_on, @edited_on, @rank, @body)";

                DateTime now = new DateTime(DateTime.Now.Ticks); // allows us to give "created_on" and "edited_on" the exact same value;

                DbCommand cmd = new DbCommand(query, conn);
                    cmd.Parameters.Add("@user_id", DbType.Integer).Value = user_id;
                    cmd.Parameters.Add("@thread_id", DbType.Integer).Value = thread_id;
                    cmd.Parameters.Add("@created_on", DbType.Date).Value = now;
                    cmd.Parameters.Add("@edited_on", DbType.Date).Value = now;
                    cmd.Parameters.Add("@rank", DbType.Integer).Value = invalid_post_id;
                    cmd.Parameters.Add("@body", DbType.Char).Value = body;

                conn.Open();

                // lets retrieve the primary key
                if (cmd.ExecuteNonQuery() > 0)
                {
                    DbCommand pk_cmd = new DbCommand("SELECT @@Identity", conn);

                    // we want to be noisy so we'll use Parse instead of TryParse.  Theoretically this should never throw...
                    return int.Parse(pk_cmd.ExecuteScalar().ToString());
                }
                return invalid_post_id;
            }
            catch { throw; }
            finally { conn.Close(); }
        }

        /// <summary>
        /// Does the *real* deletion of the post in the DB, rather than simply updating a flag
        /// </summary>
        /// <param name="post_id"></param>
        /// <returns></returns>
        private static bool real_delete(int post_id)
        {
            DbConnection conn = ConnectionManager.get_default_connection();

            try
            {
                string query = "DELETE FROM " + table_name +
                    " WHERE [id] = @id";

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@id", DbType.Integer).Value = post_id;

                conn.Open();
                return (cmd.ExecuteNonQuery() > 0);
            }
            catch { throw; }
            finally { conn.Close(); }
        }
    }
}


