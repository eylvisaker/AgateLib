using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace DML
{

    /// <summary>
    /// Summary description for ForumPost
    /// </summary>
    public class ForumPost
    {
        private DAL.ForumPostData post_data;
        private bool dirty_rank;
        private bool dirty_gen_info;

        private ForumPost()
        {
            dirty_rank = true;
            dirty_gen_info = true;
        }

        public static DML.ForumPost factory()
        {
            DAL.ForumPostData data = new DAL.ForumPostData(DAL.ForumPost.invalid_post_id);
            return factory(data);
        }

        public static DML.ForumPost factory(DAL.ForumPostData param_post_data)
        {
            DML.ForumPost post = new DML.ForumPost();
            post.post_data = param_post_data;

            post.dirty_gen_info = false;

            if (post.post_data.id == DAL.ForumPost.invalid_post_id)
                post.dirty_gen_info = true;
            return post;
        }

        public int id
        {
            get { return post_data.id; }
        }

        public MembershipUser user
        {
            get { return Membership.GetUser(post_data.user_id); }
            set
            {
                if (value.ProviderUserKey.ToString().Trim() != post_data.user_id.ToString())
                {
                    post_data.user_id = Convert.ToInt32(value.ProviderUserKey);
                    dirty_gen_info = true;
                }
            }
        }

        public int parent_thread_id
        {
            get { return post_data.thread_id; }
        }

        public DateTime creation_date
        {
            get { return post_data.created_on; }
        }

        public DateTime last_edited_date
        {
            get { return post_data.edited_on; }
        }

        public int rank
        {
            get { return post_data.rank; }
            set
            {
                if (post_data.rank != value)
                {
                    post_data.rank = value;
                    dirty_rank = true;
                }
            }
        }

        public string body
        {
            get { return post_data.body; }
            set
            {
                if (post_data.body != value)
                {
                    post_data.body = value;
                    dirty_gen_info = true;
                }
            }
        }

        public bool is_deleted()
        {
            return post_data.is_deleted;
        }

        public void save()
        {
        }



        public void delete()
        {
            if (!is_valid_post())
                throw new System.Exception("Attempt to delete a post that hasn't been saved to the DB");

            DAL.ForumPost.delete(post_data.id);
            post_data.is_deleted = true;
        }

        public void undelete()
        {
            if (!is_valid_post())
                throw new System.Exception("Attempt to undelete a post that hasn't been saved to the DB");

            DAL.ForumPost.undelete(post_data.id);
            post_data.is_deleted = false;
        }


        public void increment_rank()
        {
        }

        public void decrement_rank()
        {
        }

        public void change_parent_thread()
        {
        }

        private bool is_valid_post()
        {
            return (post_data.id != DAL.ForumPost.invalid_post_id);
        }
    }
}
