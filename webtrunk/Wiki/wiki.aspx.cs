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
        
        WikiRenderer.SetConnectionString(pageContentData.ConnectionString);

        if (string.IsNullOrEmpty(pageName))
        {
            pageName = "Default";
        }

        DataView view;
        try
        {

            if (int.TryParse(pageName, out id))
            {
                pageContentData.SelectCommand = "Select * from pages where ID = " + id;
                view = (DataView)pageContentData.Select(new DataSourceSelectArguments());

                pageName = (string)view.Table.Rows[0]["PageName"];
            }
            else
            {
                pageContentData.SelectCommand = WikiRenderer.SelectPageCommand(pageName);
                view = (DataView)pageContentData.Select(new DataSourceSelectArguments());

            }

            if (view.Table.Rows.Count == 0)
                throw new Exception("Page not found.");
        }
        catch
        {
            // occurs if the page was not found, I hope.
            Response.Redirect("notfound.aspx?page=" + pageName);
            return;
        }

        content.Text = WikiRenderer.RenderWikiText(view.Table.Rows[0]["Content"].ToString());

        editPageLink.NavigateUrl = "edit.aspx?page=" + pageName;


        BindComments(pageName);
    }

    private void BindComments(string pageName)
    {

        commentsData.SelectCommand =
            "Select WikiComments.*, UserName from WikiComments left join Users " +
            "On WikiComments.UserID = Users.UserID where PageName = '" + pageName + "' Order By Date";

        DataView commentsView = (DataView)commentsData.Select(new DataSourceSelectArguments());

        this.DataList1.DataSource = commentsView.Table.Rows;

        DataList1.DataBind();
    }
}