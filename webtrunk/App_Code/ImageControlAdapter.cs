using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls.Adapters;
using System.Web.UI.HtmlControls;

/// <summary>
/// The code here is adapted from:
/// http://www.codeproject.com/useritems/IePngControlAdapter.asp
/// 
/// Mainly the purpose is to allow IE to display png files with transparency.
/// </summary>
public class ImageControlAdapter : WebControlAdapter
{
    protected override void Render(HtmlTextWriter writer)
    {
        Image img = Control as Image;
        int n = img.ImageUrl.LastIndexOf('.');

        if (-1 == n || ".png" != img.ImageUrl.Substring(n)) // If extension is not PNG...
        {
            base.Render(writer);
            return;
        }

        if (!img.Width.IsEmpty)
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, img.Width.ToString());

        if (!img.Height.IsEmpty)
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, img.Height.ToString());

        writer.AddAttribute(HtmlTextWriterAttribute.Id, img.ClientID);
        writer.AddStyleAttribute(HtmlTextWriterStyle.Filter,
          "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" +
          Page.ResolveClientUrl(img.ImageUrl) + "')");


        writer.RenderBeginTag(HtmlTextWriterTag.Div);
        writer.RenderEndTag();
    }
}