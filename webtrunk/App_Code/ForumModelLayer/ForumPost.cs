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
        { return new DML.ForumPost(); }

        public static DML.ForumPost factory(DAL.ForumPostData param_post_data)
        { return new DML.ForumPost(); }

        public int id
        {
            get { return post_data.id; }
        }

        public MembershipUser user
        {
            get { return Membership.GetUser(); }
            set { }
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
        { return true; }

        public void save_general_info()
        { }

        public void save_rank_info()
        { }

        public void save_all()
        { }

        public void delete()
        { }
    }
}
