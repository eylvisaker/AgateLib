using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Wiki_edit : System.Web.UI.Page
{
    string pageName ;
        
    protected void Page_Load(object sender, EventArgs e)
    {
        int id;

        pageName = Request.QueryString["page"];

        if (string.IsNullOrEmpty(pageName))
        {
            pageName = "Default";
        }

        // see if we've already loaded the page
        if (string.IsNullOrEmpty(editContent.Text) == false)
            return;

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
                pageContentData.SelectCommand = WikiRenderer.SelectPageCommand(pageName);
                view = (DataView)pageContentData.Select(new DataSourceSelectArguments());

            }

            pageNameLabel.Text = pageName;
            editContent.Text = view.Table.Rows[0]["Content"].ToString();

            viewPageLink.NavigateUrl = "wiki.aspx?page=" + pageName;
        }
        catch
        {
            // occurs if the page was not found, I hope.
            editPretext.Visible = true;
            editPretext.Text = "The page " + pageName + " was not found.  You can create it below.";

        }
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        /*
        OleDbConnection conn = new OleDbConnection(pageContentData.ConnectionString);
        OleDbDataAdapter adapter = new OleDbDataAdapter(WikiRenderer.SelectCommand(pageName), conn);
        DataSet set = new DataSet();

        adapter.Fill(set);

        DataTable table = set.Tables[0];
        DataRow row = table.NewRow();

        table.Rows.Add(row);

        row["PageName"] = pageName;
        row["Content"] = editContent.Text;

        OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
        adapter.Update(set);
        */

        OleDbConnection conn = new OleDbConnection(pageContentData.ConnectionString);
        OleDbCommand cmd = new OleDbCommand("Insert Into WikiPages " +
                " (PageName, Content) Values " +
                " ('" + pageName + "', '" + editContent.Text.Replace("'", "''") + "') ",
                conn);

        conn.Open();

        try
        {
            cmd.ExecuteNonQuery();

            Response.Redirect("wiki.aspx?page=" + pageName);
        }
        finally
        {
            conn.Close();
        }

    }
}