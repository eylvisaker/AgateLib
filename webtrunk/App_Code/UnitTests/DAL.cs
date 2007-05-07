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

using System.Collections.Generic;

namespace DAL.UnitTests
{
    // utils.cs
    // DAL.ConnectionManager
    public class DALConnectionManagerTests
    {
        public DALConnectionManagerTests()
        {
            get_default_connection_test();
            get_testing_connection_test();
        }


        public void get_default_connection_test()
        {
            DbConnection conn = DAL.ConnectionManager.get_default_connection();

            conn.Open();
            conn.Close();
        }

        public void get_testing_connection_test()
        {
            DbConnection conn = DAL.ConnectionManager.get_testing_connection();

            conn.Open();
            conn.Close();
        }
    }


    // utils.cs
    // DAL.RankManager
    public class DALMaxRankManagerTests
    {
        public DALMaxRankManagerTests()
        {
            DAL.ConnectionManager.change_default_connection_string("AccessTest");
                create_rank_test();
                current_max_rank_test();
                exists_test();
                increment_test();
                decrement_test();
            DAL.ConnectionManager.change_default_connection_string("Access");
            
        }


        private void create_rank_test()
        {
            string table_name = DAL.RankManager.table_name;
            DbConnection conn = DAL.ConnectionManager.get_testing_connection();

            try
            {
                testHelper.clear_table(table_name);

                string query = "SELECT COUNT( * ) FROM " + table_name;
                DbCommand cmd = new DbCommand(query, conn);

                DAL.RankManager.create(table_name, "test");

                conn.Open();
                if (cmd.ExecuteScalar().ToString() != Convert.ToString(1))
                    throw new System.Exception("create failed to create exactly 1 record");


                // make sure the new record is created with an "invalid" rank
                cmd.CommandText = "SELECT([max_rank]) FROM `" + table_name + "`" +
                    " WHERE [table] = @table AND [ident] = @ident";

                cmd.Parameters.Add("@table", DbType.Char, 255).Value = table_name;
                cmd.Parameters.Add("@ident", DbType.Char, 20).Value = "test";

                if (cmd.ExecuteScalar().ToString() != Convert.ToString(DAL.RankManager.invalid_rank))
                    throw new System.Exception("create failed to create a record with a default max_rank that is invalid");
            }
            finally { conn.Close(); }
        }

        private void current_max_rank_test()
        {
            string table_name = DAL.RankManager.table_name;
            DbConnection conn = DAL.ConnectionManager.get_testing_connection();

            try
            {
                testHelper.clear_table(table_name);
                DAL.RankManager.create(table_name, "test");

                string select_query = "SELECT([max_rank]) FROM `" + table_name + "`" +
                    " WHERE [table] = @table AND [ident] = @ident";

                DbCommand select_cmd = new DbCommand(select_query, conn);
                    select_cmd.Parameters.Add("@table", DbType.Char, 255).Value = table_name;
                    select_cmd.Parameters.Add("@ident", DbType.Char, 20).Value = "test";

                string update_query = "UPDATE " + table_name +
                    " SET [max_rank] = @new_max_rank" +
                    " WHERE [table]=@table AND [ident] = @ident";

                DbCommand update_cmd = new DbCommand(update_query, conn);
                    update_cmd.Parameters.Add("@new_max_rank", DbType.Integer).Value = 10;
                    update_cmd.Parameters.Add("@table", DbType.Char, 255).Value = table_name;
                    update_cmd.Parameters.Add("@ident", DbType.Char, 20).Value = "test";

                conn.Open();

                if (select_cmd.ExecuteScalar().ToString() != DAL.RankManager.current_max_rank(table_name,"test").ToString())
                    throw new System.Exception("current_max_rank failed");

                

                if ( update_cmd.ExecuteNonQuery() != 1 )
                    throw new System.Exception("An unexpected error occured when manually updating the max_rank");

                int current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (select_cmd.ExecuteScalar().ToString() != current_max_rank.ToString() && current_max_rank == 10)
                    throw new System.Exception("current_max_rank failed");

                /* The OleDB Command object doesn't actually understand named parameters such as @new_max_rank, only positional parameters.
                 * As a result we can't simply change the @new_max_rank parameter for each query.  Instead we have to recreate the entire
                 * object every time
                 */
                update_cmd = new DbCommand(update_query, conn);
                    update_cmd.Parameters.Add("@new_max_rank", DbType.Integer).Value = 50;
                    update_cmd.Parameters.Add("@table", DbType.Char, 255).Value = table_name;
                    update_cmd.Parameters.Add("@ident", DbType.Char, 20).Value = "test";

                if (update_cmd.ExecuteNonQuery() != 1)
                    throw new System.Exception("An unexpected error occured when manually updating the max_rank");

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (select_cmd.ExecuteScalar().ToString() != current_max_rank.ToString() && current_max_rank == 50)
                    throw new System.Exception("current_max_rank failed");

                // ////////////////////////////////////////////////////////////
                update_cmd = new DbCommand(update_query, conn);
                    update_cmd.Parameters.Add("@new_max_rank", DbType.Integer).Value = 100;
                    update_cmd.Parameters.Add("@table", DbType.Char, 255).Value = table_name;
                    update_cmd.Parameters.Add("@ident", DbType.Char, 20).Value = "test";

                if (update_cmd.ExecuteNonQuery() != 1)
                    throw new System.Exception("An unexpected error occured when manually updating the max_rank");

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (select_cmd.ExecuteScalar().ToString() != current_max_rank.ToString() && current_max_rank == 100)
                    throw new System.Exception("current_max_rank failed");

                // ////////////////////////////////////////////////////////////
                update_cmd = new DbCommand(update_query, conn);
                    update_cmd.Parameters.Add("@new_max_rank", DbType.Integer).Value = 15000;
                    update_cmd.Parameters.Add("@table", DbType.Char, 255).Value = table_name;
                    update_cmd.Parameters.Add("@ident", DbType.Char, 20).Value = "test";
                if (update_cmd.ExecuteNonQuery() != 1)
                    throw new System.Exception("An unexpected error occured when manually updating the max_rank");

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (select_cmd.ExecuteScalar().ToString() != current_max_rank.ToString() && current_max_rank == 15000)
                    throw new System.Exception("current_max_rank failed");
            }
            finally { conn.Close(); }
        }

        private void exists_test()
        {
            string table_name = DAL.RankManager.table_name;

            try
            {
                testHelper.clear_table(table_name);

                // table should be empty
                if (DAL.RankManager.exists(table_name, "test"))
                    throw new System.Exception("exists found a non-existent record");

                DAL.RankManager.create(table_name, "test");

                // record should be found
                if (!DAL.RankManager.exists(table_name, "test"))
                    throw new System.Exception("exists failed to find an existing record");

                // other record is non-existent
                if (DAL.RankManager.exists(table_name, "test_other"))
                    throw new System.Exception("exists found a non-existent record with another record in the table");

                DAL.RankManager.create(table_name, "test_other");

                // other record should be found
                if (!DAL.RankManager.exists(table_name, "test_other"))
                    throw new System.Exception("exists failed to find an existing record with another record in the table");
            }
            finally { }
        }

        private void increment_test()
        {
            string table_name = DAL.RankManager.table_name;

            try
            {
                testHelper.clear_table(table_name);

                DAL.RankManager.create(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name,"test");

                int current_max_rank = DAL.RankManager.current_max_rank(table_name,"test");
                if (current_max_rank != 0)
                    throw new System.Exception("increment failed");

                DAL.RankManager.increment_max_rank(table_name, "test");

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (current_max_rank != 1)
                    throw new System.Exception("increment failed");


                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (current_max_rank != 6)
                    throw new System.Exception("increment failed");

                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (current_max_rank != 8)
                    throw new System.Exception("increment failed");

            }
            finally { }

        }

        private void decrement_test()
        {
            string table_name = DAL.RankManager.table_name;

            try
            {
                testHelper.clear_table(table_name);

                // check a bogus expected max rank
                DAL.RankManager.create(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");

                if (DAL.RankManager.decrement_max_rank(table_name, "test", 100) != DAL.RankManager.invalid_rank)
                    throw new System.Exception("decrement_max_rank decremented when the expected max_rank was wrong");
                
                // make sure a single decrement to an invalid rank works correctly
                DAL.RankManager.decrement_max_rank(table_name, "test", 0);
                int current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (current_max_rank != DAL.RankManager.invalid_rank)
                    throw new System.Exception("decrement failed");
                
                
                // check if a decrement to a valid rank works correctly
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.decrement_max_rank(table_name, "test",1);

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (current_max_rank != 0)
                    throw new System.Exception("increment failed");

                // another check
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.increment_max_rank(table_name, "test");
                DAL.RankManager.decrement_max_rank(table_name, "test",4);

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (current_max_rank != 3)
                    throw new System.Exception("increment failed");


                // yet another check
                DAL.RankManager.decrement_max_rank(table_name, "test",3);
                DAL.RankManager.decrement_max_rank(table_name, "test",2);

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (current_max_rank != 1)
                    throw new System.Exception("increment failed");

                // check to see if multiple decrements from an invalid state leave it in an invalid state
                DAL.RankManager.decrement_max_rank(table_name, "test",1);
                DAL.RankManager.decrement_max_rank(table_name, "test",0);
                DAL.RankManager.decrement_max_rank(table_name, "test",DAL.RankManager.invalid_rank);
                DAL.RankManager.decrement_max_rank(table_name, "test", DAL.RankManager.invalid_rank);
                DAL.RankManager.decrement_max_rank(table_name, "test", DAL.RankManager.invalid_rank);

                current_max_rank = DAL.RankManager.current_max_rank(table_name, "test");
                if (current_max_rank != DAL.RankManager.invalid_rank)
                    throw new System.Exception("increment failed");
            }
            finally { }
        }
    }

    // ForumPost.cs
    // DAL.ForumPost
    public class DALForumPostTests
    {
        public DALForumPostTests()
        {
            DAL.ConnectionManager.change_default_connection_string("AccessTest");
                exists_test();
                create_test();
                is_deleted_test();
                delete_undelete_test();

                retrieve_filter_deleted_test();
                retrieve_no_filter_deleted_test();
                retrieve_by_thread_filter_deleted_test1();
                retrieve_by_thread_filter_deleted_test2();
                retrieve_by_thread_filter_deleted_test3();
                retrieve_by_thread_no_filter_deleted_test1();
                retrieve_by_thread_no_filter_deleted_test2();
                update_gen_info_test();
                update_rank_test();
                paginate_filter_deleted_test1();
                paginate_filter_deleted_test2();
                paginate_no_filter_deleted_test1();
                paginate_no_filter_deleted_test2();
            DAL.ConnectionManager.change_default_connection_string("Access");
        }

        public void exists_test()
        {
            string table_name = DAL.ForumPost.table_name;
            DbConnection conn = DAL.ConnectionManager.get_testing_connection();

            try
            {
                testHelper.clear_table(table_name);

                string query = "INSERT INTO " + table_name +
                    "([user_id],[thread_id],[created_on],[edited_on],[rank],[body]) " +
                    "VALUES(@user_id, @thread_id, @created_on, @edited_on, @rank, @body)";

                DateTime now = new DateTime(DateTime.Now.Ticks); // allows us to give "created_on" and "edited_on" the exact same value;

                DbCommand cmd = new DbCommand(query, conn);
                cmd.Parameters.Add("@user_id", DbType.Integer).Value = 1;
                cmd.Parameters.Add("@thread_id", DbType.Integer).Value = 2;
                cmd.Parameters.Add("@created_on", DbType.Date).Value = now;
                cmd.Parameters.Add("@edited_on", DbType.Date).Value = now;
                cmd.Parameters.Add("@rank", DbType.Integer).Value = DAL.ForumPost.invalid_post_id;
                cmd.Parameters.Add("@body", DbType.Char).Value = "test";


                // make sure we don't find a non-existent record when no records exist
                if (DAL.ForumPost.exists(0))
                    throw new System.Exception("Found a non-existent record");


                conn.Open();
                // lets retrieve the primary key
                if (cmd.ExecuteNonQuery() != 1)
                    throw new System.Exception("An unexpected error occured while creating a new ForumPosts record");

                DbCommand pk_cmd = new DbCommand("SELECT @@Identity", conn);

                int id;
                int.TryParse(pk_cmd.ExecuteScalar().ToString(),out id);

                // check if we find an existent record when only that record exists
                if (!DAL.ForumPost.exists(id))
                    throw new System.Exception("Unable to find a record that exists");


                if (cmd.ExecuteNonQuery() != 1)
                    throw new System.Exception("An unexpected error occured while creating a new ForumPosts record");

                int.TryParse(pk_cmd.ExecuteScalar().ToString(), out id);


                // check if we find a non-existent record when another record exists
                if (DAL.ForumPost.exists(id + 1))
                    throw new System.Exception("Unable to find a record that exists");

                // check if we don't find a non-existent record when another record exists
                if (DAL.ForumPost.exists(id + 1))
                    throw new System.Exception("Unable to find a record that exists");


            }
            finally { conn.Close(); }

        }
     
        public void create_test()
        {
            string table_name = DAL.ForumPost.table_name;
            DbConnection conn = DAL.ConnectionManager.get_testing_connection();

            try
            {
                testHelper.clear_table(table_name);

                int id = DAL.ForumPost.create(1, 2, "test");

                if (id == DAL.ForumPost.invalid_post_id)
                    throw new System.Exception("create failed to create a new post");

                // test creation with no existing records
                if (!DAL.ForumPost.exists(id))
                    throw new System.Exception("create failed");

                id = DAL.ForumPost.create(3, 4, "test2");

                if (id == DAL.ForumPost.invalid_post_id)
                    throw new System.Exception("create failed to create a new post");

                // test creation with an existing record
                if (!DAL.ForumPost.exists(id))
                    throw new System.Exception("create failed");

                id = DAL.ForumPost.create(5, 6, "test3");

                if (id == DAL.ForumPost.invalid_post_id)
                    throw new System.Exception("create failed to create a new post");

                // test creation with multiple existing record
                if (!DAL.ForumPost.exists(id))
                    throw new System.Exception("create failed");

            }
            finally { conn.Close(); }
        }

        public void is_deleted_test()
        {
            string table_name = DAL.ForumPost.table_name;
            DbConnection conn = DAL.ConnectionManager.get_testing_connection();

            try
            {
                testHelper.clear_table(table_name);

                int id = DAL.ForumPost.create(1, 2, "test");

                // check if it's flagged as deleted 
                if (DAL.ForumPost.is_deleted(id))
                    throw new System.Exception("record flagged as deleted when it isn't");

                string update_query = "UPDATE " + table_name +
                    " SET [is_deleted]=@is_deleted " +
                    " WHERE [id] = @id";

                DbCommand cmd2 = new DbCommand(update_query, conn);
                cmd2.Parameters.Add("@is_deleted", DbType.Boolean).Value = true;
                cmd2.Parameters.Add("@id", DbType.Integer).Value = id;

                conn.Open();
                if (cmd2.ExecuteNonQuery() != 1)
                    throw new System.Exception("An unexpected error occured while creating a new ForumPosts record");

                conn.Close();

                if (!DAL.ForumPost.is_deleted(id))
                    throw new System.Exception("record flagged as not deleted when it is");

            }
            finally { conn.Close(); }
        }

        public void delete_undelete_test()
        {
            string table_name = DAL.ForumPost.table_name;

            try
            {
                testHelper.clear_table(table_name);

                int id = DAL.ForumPost.create(1, 2, "test");

                if (DAL.ForumPost.is_deleted(id))
                    throw new System.Exception("create created a post flagged as deleted");

                DAL.ForumPost.delete(id);

                if (!DAL.ForumPost.is_deleted(id))
                    throw new System.Exception("delete failed to properly delete a post");

                DAL.ForumPost.undelete(id);

                if (DAL.ForumPost.is_deleted(id))
                    throw new System.Exception("undelete failed to properly undelete a post");

                // NOTE: We aren't testing to see the behavior of deleting a post that is already deleted or
                //  undeleting a post that isn't deleted.  The default behavior of these methods will be to
                //  throw an argument exception error in these cases.

            }
            finally { }
        }

        public void retrieve_filter_deleted_test()
        {
            string table_name = DAL.ForumPost.table_name;

            try
            {
                testHelper.clear_table(table_name);

                int id = DAL.ForumPost.create(1, 2, "test");
                DAL.ForumPost.delete(id);

                if (DAL.ForumPost.retrieve_filter_deleted(id).Count != 0)
                    throw new System.Exception("retrieve_filter_deleted did not filter properly");

                DAL.ForumPost.undelete(id);

                if (DAL.ForumPost.retrieve_filter_deleted(id).Count == 0)
                    throw new System.Exception("retrieve_filter_deleted did not filter properly");
            }
            finally { }
        }

        public void retrieve_no_filter_deleted_test()
        {
            string table_name = DAL.ForumPost.table_name;

            try
            {
                testHelper.clear_table(table_name);

                int id = DAL.ForumPost.create(1, 2, "test");
                DAL.ForumPost.delete(id);

                if (DAL.ForumPost.retrieve_no_filter_deleted(id).Count == 0)
                    throw new System.Exception("retrieve_no_filter_deleted filtered out a deleted post");

                DAL.ForumPost.undelete(id);

                if (DAL.ForumPost.retrieve_no_filter_deleted(id).Count == 0)
                    throw new System.Exception("retrieve_no_filter_deleted filtered out an undeleted post");
            }
            finally { }
        }

        // verify its returning the correct number of rows
        public void retrieve_by_thread_filter_deleted_test1()
        {
            string table_name = DAL.ForumPost.table_name;

            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);

                int id1 = DAL.ForumPost.create(1, 2, "test1");
                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(2,DESCENDING).Count != 1)
                    throw new System.Exception("retrieve_by_thread_filter_deleted failed");

                int id2 = DAL.ForumPost.create(1, 3, "test1");
                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(2,DESCENDING).Count != 1)
                    throw new System.Exception("retrieve_by_thread_filter_deleted failed");

                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(3,DESCENDING).Count != 1)
                    throw new System.Exception("retrieve_by_thread_filter_deleted failed");

                int id3 = DAL.ForumPost.create(1, 2, "test1");
                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(2,DESCENDING).Count != 2)
                    throw new System.Exception("retrieve_by_thread_filter_deleted failed");

                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(3,DESCENDING).Count != 1)
                    throw new System.Exception("retrieve_by_thread_filter_deleted failed");
            }
            finally { }
        }

        // verify it's filtering correctly
        public void retrieve_by_thread_filter_deleted_test2()
        {
            string table_name = DAL.ForumPost.table_name;

            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);
                System.Collections.Generic.IList<DAL.ForumPostData> list;

                list = DAL.ForumPost.retrieve_by_thread_filter_deleted(2, DESCENDING);

                if (list.Count != 0)
                    throw new System.Exception("retrieve_by_thread_filter_deleted returned a row that hadn't been created yet");

                int id = DAL.ForumPost.create(1, 2, "test1");
                DAL.ForumPost.undelete(id);


                list = DAL.ForumPost.retrieve_by_thread_filter_deleted(2, DESCENDING);


                // check if it filtered out an undeleted row
                if (list.Count != 1)
                    throw new System.Exception("retrieve_by_thread_filter_deleted did not return the correct number of rows");

                // make sure it returned the correct row
                if (list[0].id.ToString() != id.ToString())
                    throw new System.Exception("retrieve_by_thread_filter_deleted returned the wrong row");


                // make sure it's filtering out deleted rows
                DAL.ForumPost.delete(id);

                list = DAL.ForumPost.retrieve_by_thread_filter_deleted(2, DESCENDING);

                if (list.Count != 0)
                    throw new System.Exception("retrieve_by_thread_filter_deleted did not filter deleted");
            }
            finally { }
        }

        // verify it's returning the correct rows & filtering at the same time
        public void retrieve_by_thread_filter_deleted_test3()
        {
            string table_name = DAL.ForumPost.table_name;

            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);

                int id1 = DAL.ForumPost.create(1, 2, "test1");
                int id2 = DAL.ForumPost.create(2, 2, "test2");
                int id3 = DAL.ForumPost.create(2, 3, "test2");

                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(2, DESCENDING).Count != 2)
                    throw new System.Exception("retrieved an incorrect number of rows");

                DAL.ForumPost.delete(id1);

                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(2, DESCENDING).Count != 1)
                    throw new System.Exception("retrieved an incorrect number of rows");

                DAL.ForumPost.delete(id3);

                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(3, DESCENDING).Count != 0)
                    throw new System.Exception("retrieved an incorrect number of rows");

                DAL.ForumPost.delete(id2);

                if (DAL.ForumPost.retrieve_by_thread_filter_deleted(2, DESCENDING).Count != 0)
                    throw new System.Exception("retrieved an incorrect number of rows");
            }
            finally { }
        }

        // verify its returning the correct number of rows
        public void retrieve_by_thread_no_filter_deleted_test1()
        {
            string table_name = DAL.ForumPost.table_name;
            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);

                int id1 = DAL.ForumPost.create(1, 2, "test1");
                if (DAL.ForumPost.retrieve_by_thread_no_filter_deleted(2, DESCENDING).Count != 1)
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted failed");

                int id2 = DAL.ForumPost.create(1, 3, "test1");
                if (DAL.ForumPost.retrieve_by_thread_no_filter_deleted(2, DESCENDING).Count != 1)
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted failed");

                if (DAL.ForumPost.retrieve_by_thread_no_filter_deleted(3, DESCENDING).Count != 1)
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted failed");

                int id3 = DAL.ForumPost.create(1, 2, "test1");
                if (DAL.ForumPost.retrieve_by_thread_no_filter_deleted(2, DESCENDING).Count != 2)
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted failed");

                if (DAL.ForumPost.retrieve_by_thread_no_filter_deleted(3, DESCENDING).Count != 1)
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted failed");
            }
            finally { }
        }

        // verify it isn't filtering deleted
        public void retrieve_by_thread_no_filter_deleted_test2()
        {
            string table_name = DAL.ForumPost.table_name;
            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);
                System.Collections.Generic.IList<DAL.ForumPostData> list;

                list = DAL.ForumPost.retrieve_by_thread_no_filter_deleted(2,DESCENDING);

                if (list.Count != 0)
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted returned a row that hadn't been created yet");

                int id = DAL.ForumPost.create(1, 2, "test1");
                DAL.ForumPost.undelete(id);



                list = DAL.ForumPost.retrieve_by_thread_no_filter_deleted(2,DESCENDING);

                // check if it filtered out an undeleted row
                if (list.Count != 1)
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted did not return the correct row");

                // make sure it returned the correct row
                if (list[0].id.ToString() != id.ToString())
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted returned the wrong row");

                // make sure it isn't filtering out deleted rows
                DAL.ForumPost.delete(id);

                list = DAL.ForumPost.retrieve_by_thread_no_filter_deleted(2,DESCENDING);

                if (list.Count != 1)
                    throw new System.Exception("retrieve_by_thread_no_filter_deleted did not filter deleted");
            }
            finally { }
        }

        public void update_gen_info_test()
        {
            string table_name = DAL.ForumPost.table_name;
            DbConnection conn = DAL.ConnectionManager.get_testing_connection();

            try
            {
                testHelper.clear_table(table_name);

                int id = DAL.ForumPost.create(1, 2, "test1");
                DAL.ForumPost.update_gen_info(id, 3, "updated_test1");

                List<DAL.ForumPostData> list = DAL.ForumPost.retrieve_no_filter_deleted(id);

                if (list.Count == 0)
                    throw new System.Exception("unexpected error during retrieval of record");

                ForumPostData data = list[0];
                if (data.user_id != 3)
                    throw new System.Exception("user_id not updated properly");


                if (data.body != "updated_test1")
                    throw new System.Exception("body was not updated correctly");


                // test when there are other rows
                id = DAL.ForumPost.create(4, 5, "test2");
                DAL.ForumPost.update_gen_info(id, 6, "updated_test2");

                list = DAL.ForumPost.retrieve_no_filter_deleted(id);

                if (list.Count == 0)
                    throw new System.Exception("unexpected error during retrieval of record");

                data = list[0];
                if (data.user_id != 6)
                    throw new System.Exception("user_id not updated properly");

                if (data.body != "updated_test2")
                    throw new System.Exception("body was not updated correctly");
            }
            finally { conn.Close(); }
        }

        public void update_rank_test()
        {
            string table_name = DAL.ForumPost.table_name;
            DbConnection conn = DAL.ConnectionManager.get_testing_connection();

            try
            {
                testHelper.clear_table(table_name);

                int id = DAL.ForumPost.create(1, 2, "test1");
                DAL.ForumPost.update_rank(id, 100, 5);

                List<DAL.ForumPostData> list = DAL.ForumPost.retrieve_no_filter_deleted(id);

                if (list.Count == 0)
                    throw new System.Exception("unexpected error during retrieval of record");

                ForumPostData data = list[0];
                if (data.rank != 100)
                    throw new System.Exception("rank did not updated properly");

                if (data.thread_id != 5)
                    throw new System.Exception("thread_id was not updated correctly");


                // test when there are other rows
                id = DAL.ForumPost.create(4, 5, "test2");
                DAL.ForumPost.update_rank(id, 200, 11);

                list = DAL.ForumPost.retrieve_no_filter_deleted(id);

                if (list.Count == 0)
                    throw new System.Exception("unexpected error during retrieval of record");

                data = list[0];
                if (data.rank != 200)
                    throw new System.Exception("rank did not updated properly");

                if (data.thread_id != 11)
                    throw new System.Exception("thread_id was not updated correctly");
            }
            finally { conn.Close(); }
        }

        public void paginate_filter_deleted_test1()
        {
            string table_name = DAL.ForumPost.table_name;
            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);
                testHelper.clear_table(DAL.RankManager.table_name);

                List<DAL.ForumPostData> list;

                list = DAL.ForumPost.paginate_filter_deleted(0,1,1,DESCENDING);

                // make sure it returns nothing if there's nothing there
                if( list.Count != 0)
                    throw new System.Exception("paginate_filter_deleted returned a record when none existed");

                int id = DAL.ForumPost.create(1, 2, "test");
                // create records with different thread_id's
                DAL.ForumPost.create(1, 3, "test");
                DAL.ForumPost.create(1, 3, "test");
                DAL.ForumPost.create(1, 4, "test");
                DAL.ForumPost.undelete(id);

                // make sure it's returning a single row correctly
                list = DAL.ForumPost.paginate_filter_deleted(2, 1, 0, DESCENDING);
                if (list.Count != 1)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                id = DAL.ForumPost.create(3, 2, "test2");
                DAL.ForumPost.undelete(id);

                // check if the first page is returned correctly when there's multiple pages
                list = DAL.ForumPost.paginate_filter_deleted(2, 1, 0, DESCENDING);
                if (list.Count != 1)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                // if there's two rows and we're accessing the last row, make sure everything is returned as expected
                list = DAL.ForumPost.paginate_filter_deleted(2, 1, 1, DESCENDING);
                if (list.Count != 1)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                // make sure it's the one we expected
                 if(list[0].id != id)
                     throw new System.Exception("paginate_filter_deleted returned the wrong record");


                 list = DAL.ForumPost.paginate_filter_deleted(2, 2, 0, DESCENDING);

                // make sure we properly return a page with more than 1 record in it
                if (list.Count != 2)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");
            }
            finally { }
        }

        public void paginate_filter_deleted_test2()
        {
            string table_name = DAL.ForumPost.table_name;
            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);
                testHelper.clear_table(DAL.RankManager.table_name);

                int id1 = DAL.ForumPost.create(1, 2, "test");
                // create records with different thread_id's
                DAL.ForumPost.create(1, 3, "test");
                DAL.ForumPost.create(1, 3, "test");
                DAL.ForumPost.create(1, 4, "test");
                DAL.ForumPost.delete(id1);

                // make sure it's returning a single row correctly
                List<DAL.ForumPostData> list = DAL.ForumPost.paginate_filter_deleted(2, 1, 0, DESCENDING);
                if (list.Count != 0)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                int id2 = DAL.ForumPost.create(1, 2, "test");
                DAL.ForumPost.delete(id2);

                list = DAL.ForumPost.paginate_filter_deleted(2, 1, 1, DESCENDING);
                if (list.Count != 0)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                DAL.ForumPost.undelete(id2);
                list = DAL.ForumPost.paginate_filter_deleted(2, 1, 0, DESCENDING);

                if (list.Count != 0)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                list = DAL.ForumPost.paginate_filter_deleted(2, 1, 1, DESCENDING);

                if (list.Count != 1)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                list = DAL.ForumPost.paginate_filter_deleted(2, 2, 0, DESCENDING);

                if (list.Count != 1)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");
            }
            finally { }
        }

        // make sure it's returning rows correctly
        public void paginate_no_filter_deleted_test1()
        {
            string table_name = DAL.ForumPost.table_name;
            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);
                testHelper.clear_table(DAL.RankManager.table_name);

                List<DAL.ForumPostData> list;

                list = DAL.ForumPost.paginate_no_filter_deleted(0, 1, 0, DESCENDING);
                
                // make sure it returns nothing if there's nothing there
                if (list.Count != 0)
                    throw new System.Exception("paginate_filter_deleted returned a record when none existed");

                int id = DAL.ForumPost.create(1, 2, "test");
                // create records with different thread_id's
                DAL.ForumPost.create(1, 3, "test");
                DAL.ForumPost.create(1, 3, "test");
                DAL.ForumPost.create(1, 4, "test");
                DAL.ForumPost.undelete(id);

                // make sure it's returning a single row correctly
                list = DAL.ForumPost.paginate_no_filter_deleted(2, 1, 0, DESCENDING);
                if (list.Count != 1)
                    throw new System.Exception("paginate_no_filter_deleted returned the wrong number of records");

                id = DAL.ForumPost.create(3, 2, "test2");
                DAL.ForumPost.undelete(id);

                // check if the first page is returned correctly when there's multiple pages
                list = DAL.ForumPost.paginate_no_filter_deleted(2, 1, 0, DESCENDING);
                if (list.Count != 1)
                    throw new System.Exception("paginate_no_filter_deleted returned the wrong number of records");

                // if there's two rows and we're accessing the last row, make sure everything is returned as expected
                list = DAL.ForumPost.paginate_no_filter_deleted(2, 1, 1, DESCENDING);

                if (list.Count != 1)
                    throw new System.Exception("paginate_no_filter_deleted returned the wrong number of records");

                // make sure it's the one we expected
                if (list[0].id != id)
                    throw new System.Exception("paginate_no_filter_deleted returned the wrong record");


                list = DAL.ForumPost.paginate_no_filter_deleted(2, 2, 0, DESCENDING);

                // make sure we properly return a page with more than 1 record in it
                if (list.Count != 2)
                    throw new System.Exception("paginate_no_filter_deleted returned the wrong number of records");
            }
            finally { }
        }

        // make sure it isn't filtering
        public void paginate_no_filter_deleted_test2()
        {
            string table_name = DAL.ForumPost.table_name;
            bool DESCENDING = DAL.ForumPost.DESCENDING_RANK;

            try
            {
                testHelper.clear_table(table_name);
                testHelper.clear_table(DAL.RankManager.table_name);

                int id1 = DAL.ForumPost.create(1, 2, "test");
                // create records with different thread_id's
                DAL.ForumPost.create(1, 3, "test");
                DAL.ForumPost.create(1, 3, "test");
                DAL.ForumPost.create(1, 4, "test");
                DAL.ForumPost.delete(id1);

                // make sure it's returning a single row correctly
                List<DAL.ForumPostData> list = DAL.ForumPost.paginate_no_filter_deleted(2, 1, 1, DESCENDING);
                if (list.Count != 0)
                    throw new System.Exception("paginate_no_filter_deleted returned the wrong number of records");

                int id2 = DAL.ForumPost.create(1, 2, "test");
                DAL.ForumPost.delete(id2);

                list = DAL.ForumPost.paginate_no_filter_deleted(2, 1, 1, DESCENDING);
                if (list.Count != 1)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                DAL.ForumPost.undelete(id2);
                list = DAL.ForumPost.paginate_no_filter_deleted(2, 1, 0, DESCENDING);

                if (list.Count != 1)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                list = DAL.ForumPost.paginate_no_filter_deleted(2, 1, 1, DESCENDING);

                if (list.Count != 1)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                list = DAL.ForumPost.paginate_no_filter_deleted(2, 2, 0, DESCENDING);

                if (list.Count != 2)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");

                list = DAL.ForumPost.paginate_no_filter_deleted(2, DAL.ForumPost.ALL_POSTS, 0, DESCENDING);

                if (list.Count != 2)
                    throw new System.Exception("paginate_filter_deleted returned the wrong number of records");
            }
            finally { }
        }
    }




    public static class testHelper
    {
        public static void clear_table(string table)
        {
            string query = "DELETE * FROM " + table;

            DbConnection conn = DAL.ConnectionManager.get_testing_connection();
            DbCommand cmd = new DbCommand(query,conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally { conn.Close(); }
        }
    }
}
