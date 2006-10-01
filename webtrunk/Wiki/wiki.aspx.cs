using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Wiki_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string pageName = Request.QueryString["page"];
        int id;

        if (string.IsNullOrEmpty(pageName))
        {
            pageName = "Default";
        }

        try
        {
            DataView view;

            if (int.TryParse(pageName, out id))
            {
                pageContentData.SelectCommand = "Select * from pages where ID = " + id;
                view = (DataView)pageContentData.Select(new DataSourceSelectArguments());
                
                pageName = (string)view.Table.Rows[0]["PageName"];
            }
            else
            {
                pageContentData.SelectCommand =
                    "Select Top 1 * from WikiPages where PageName = '" + pageName + "' Order By Date Desc";
                view = (DataView)pageContentData.Select(new DataSourceSelectArguments());

            }

            content.Text = view.Table.Rows[0]["Content"].ToString();

        }
        catch
        {
            // occurs if the page was not found, I hope.
            Response.Redirect("notfound.aspx?page=" + pageName);
            return;
        }

        commentsData.SelectCommand =
            "Select WikiComments.*, UserName from WikiComments left join Users " +
            "On WikiComments.UserID = Users.UserID where PageName = '" + pageName + "' Order By Date";

        DataView commentsView = (DataView)commentsData.Select(new DataSourceSelectArguments());

        this.DataList1.DataSource = commentsView.Table.Rows;

        DataList1.DataBind();
    }
}