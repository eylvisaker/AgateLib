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
        DataRow row;

        // hack to do late binding, because C# doesn't support it like VB does.
        string pageName = (string)Master.GetType().InvokeMember("PageName",
            System.Reflection.BindingFlags.GetProperty, null, Master, null);
        
        try
        {
            row = (DataRow)Master.GetType().InvokeMember("GetPageData",
                System.Reflection.BindingFlags.InvokeMethod, null, Master, new object[] { });
        }
        catch
        {
            // occurs if the page was not found, I hope.
            Response.Redirect("notfound.aspx?page=" + pageName);
            return;
        }

        content.Text = "<h1 class=\"Wiki\">" + pageName.Replace('_', ' ') + "</h1>";
        content.Text += WikiRenderer.RenderWikiText(row["Content"].ToString());

        Page.Title = pageName.Replace('_', ' ');

        editPageLink.NavigateUrl = "edit.aspx?page=" + pageName;

    }

}