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

public partial class Wiki_Discussion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
