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

public partial class WikiMasterPage : System.Web.UI.MasterPage
{
    private void Initialize()
    {
        WikiRenderer.SetConnectionString(ConfigurationManager.ConnectionStrings["Access"].ConnectionString);

        pageName = Request.QueryString["page"];

        if (string.IsNullOrEmpty(pageName))
        {
            pageName = "Main_Page";
        }
    }

    private string pageName;

    public string PageName
    {
        get
        {
            if (string.IsNullOrEmpty(pageName ))
                Initialize();

            return pageName;
        }
    }

    public DataRow GetPageData()
    {
        OleDbConnection conn = new OleDbConnection(WikiRenderer.ConnectionString);
        string command;
        int id;

        if (int.TryParse(PageName, out id))
        {
            command = WikiRenderer.SelectPageCommand(id);
        }
        else
        {
            command = WikiRenderer.SelectPageCommand(pageName);
        }

        OleDbDataAdapter adapter = new OleDbDataAdapter(command, conn);
        DataSet set = new DataSet();
        adapter.Fill(set);

        if (set.Tables[0].Rows.Count == 0)
            throw new Exception("Page not found.");

        return set.Tables[0].Rows[0];
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void searchButton_Click(object sender, EventArgs e)
    {
        string searchString = searchBox.Text;
        string pageSearch = searchString.Replace(' ', '_');

        if (WikiRenderer.HasPage(pageSearch))
        {
            Response.Redirect("wiki.aspx?page=" + pageSearch);
        }

    }
}
