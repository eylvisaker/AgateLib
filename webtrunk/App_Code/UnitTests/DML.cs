using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace DML.UnitTests
{
    using testHelper = DAL.UnitTests.testHelper;
    using ConnectionManager = DAL.ConnectionManager;

    public class DMLForumPostTests
    {
        public DMLForumPostTests()
        {
            ConnectionManager.change_default_connection_string("AccessTest");
            ConnectionManager.change_default_connection_string("Access");
        }



    }
}
