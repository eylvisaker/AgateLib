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

        private ForumPost()
        {
            //
            // TODO: Add constructor logic here
            //
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
            return post;
        }

        public int id
        {
            get { return post_data.id; }
        }

        public MembershipUser user
        {
            get { return Membership.GetUser(post_data.user_id); }
            set { post_data.user_id = Convert.ToInt32(value.ProviderUserKey); }
        }

        public int parent_thread_id
        {
            get { return post_data.thread_id; }
            set { post_data.thread_id = value; }
        }

        public DateTime creation_date
        {
            get { return post_data.created_on; }
            set { post_data.created_on = value; }
        }

        public DateTime last_edited_date
        {
            get { return post_data.edited_on; }
        }

        public int rank
        {
            get { return post_data.rank; }
            set { post_data.rank = value; }
        }

        public string body
        {
            get { return post_data.body; }
            set { post_data.body = value; }
        }

        public bool is_deleted()
        {
            return post_data.is_deleted;
        }

        // need to find a method for getting the edited DateTime back
        public void save_general_info()
        {
            DAL.ForumPost.update_gen_info(post_data.id, post_data.user_id, post_data.body);
            post_data.edited_on = DAL.ForumPost.get_edited_on(post_data.id);
        }

        public void save_rank_info()
        {
            DAL.ForumPost.update_rank(post_data.id, post_data.rank, post_data.thread_id);
            // update_rank doesn't currently update the edited_on field, but it may in the future
            post_data.edited_on = DAL.ForumPost.get_edited_on(post_data.id);
        }

        public void delete()
        {
            post_data.is_deleted = true;
            DAL.ForumPost.delete(post_data.id);
        }

        public void undelete()
        {
            post_data.is_deleted = false;
            DAL.ForumPost.undelete(post_data.id);
        }
    }
}
