using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for WikiRenderer
/// </summary>
public static class WikiRenderer
{
    class WikiRendererState
    {
        public string bulletState;
        public int bulletLevel;

        public string line;
        public string lastLine;
        public string nextLine;

        public bool italic;
        public bool bold;

        /// <summary>
        /// Dictionary containing available pages.
        /// Only the key is used, the value is just an int.
        /// </summary>
        public Dictionary<string, int> availablePages = new Dictionary<string, int>();
    }
    private static string connectionString;

	static WikiRenderer()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string ConnectionString
    {
        get { return connectionString; }
    }
    public static void SetConnectionString(string conn)
    {
        connectionString = conn;   
    }
    private static void FillAvailablePages(WikiRendererState rs)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Must set connection string before rendering wiki.");

        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbDataAdapter adapter = new OleDbDataAdapter(
            "Select PageName from WikiPages Group By PageName", conn);

        DataSet dataset = new DataSet();
        adapter.Fill(dataset);

        DataTable table = dataset.Tables[0];

        for (int i = 0; i < table.Rows.Count; i++)
        {
            if (rs.availablePages.ContainsKey(table.Rows[i]["PageName"].ToString()))
                continue;

            rs.availablePages.Add(table.Rows[i]["PageName"].ToString(), 1);
        }
    }

    public static bool HasPage(string page)
    {
        WikiRendererState rs = new WikiRendererState();

        FillAvailablePages(rs);

        foreach (string key in rs.availablePages.Keys)
        {
            if (key.ToLowerInvariant() == page.ToLowerInvariant())
                return true;
        }

        return false;
    }
    public static string SelectPageCommand(string pageName)
    {
        return "Select Top 1 * from WikiPages where PageName = '" + pageName + "' Order By Date Desc";
    }
    public static string SelectPageCommand(int pageID)
    {
        return "Select * from pages where ID = " + pageID;
    }

    public static string RenderWikiText(string text)
    {
        string[] lines = text.Trim().Split('\n');
        string[] retvalLines = new string[lines.Length];
        WikiRendererState renderState = new WikiRendererState();

        FillAvailablePages(renderState);

        for (int i = 0; i < lines.Length; i++)
        {
            renderState.line = lines[i].TrimEnd('\r');

            // fill previous line
            if (i > 0)
                renderState.lastLine = lines[i - 1].TrimEnd('\r');
            else
                renderState.lastLine = null;

            // fill next line
            if (i < lines.Length - 1)
                renderState.nextLine = lines[i + 1].TrimEnd('\r');
            else
                renderState.nextLine = null;

            retvalLines[i] = RenderWikiText(renderState);
        }

        return string.Join("\n", retvalLines);
    }

    private static string RenderWikiText(WikiRendererState rs)
    {

        if (string.IsNullOrEmpty(rs.line))
            return "<p>";

        if (CheckEnds(rs.line.TrimEnd(), "="))
            return DoHeader(rs);
        if (rs.line.StartsWith("*"))
            return DoBullet(rs);
        if (rs.line.StartsWith(" "))
            return DoPreformat(rs);

        string retval = "";
        retval = ParseLine(rs.line, rs);

        return retval;
    }



    private static string ParseLine(string line, WikiRendererState rs)
    {
        string retval = "";

        for (int i = 0; i < line.Length; i++)
        {
            string str = line.Substring(i);

            // Bold
            if (str.StartsWith("'''"))
            {
                if (rs.bold)
                {
                    retval += CloseTag("b");
                    rs.bold = false;
                }
                else
                {
                    retval += CreateTag("b");
                    rs.bold = true;
                }
                i += 2;
                continue;
            }
                // italics
            else if (str.StartsWith("''"))
            {
                if (rs.italic)
                {
                    retval += CloseTag("i");
                    rs.italic = false;
                }
                else
                {
                    retval += CreateTag("i");
                    rs.italic = true;
                }

                i += 1;
                continue;
            }
            // wiki link
            else if (str.StartsWith("[["))
            {
                int end = str.IndexOf("]]");
                int next = str.IndexOf("[[", 2);

                if (end > -1 && (next > end || next == -1))
                {
                    string innertext = str.Substring(2, end-2);
                    int pipe = innertext.IndexOf("|");
                    string wikipage;
                    string text;
                    string cssClass = "Wiki";

                    if (pipe == -1)
                    {
                        wikipage = innertext;
                        text = innertext;
                    }
                    else
                    {
                        wikipage = innertext.Substring(0, pipe);
                        text = innertext.Substring(pipe + 1);
                    }

                    if (rs.availablePages.ContainsKey(wikipage) == false)
                    {
                        cssClass = "WikiNew";
                    }

                    retval += CreateTag("a", "href=\"wiki.aspx?page=" + wikipage + "\"", cssClass);
                    retval += text;
                    retval += CloseTag("a");

                    i += end + 1;
                    continue;
                }
            }
            // external link
            else if (str.StartsWith("[http://"))
            {
                int end = str.IndexOf("]");
                int next = str.IndexOf("[", 2);

                if (end > -1 && (next > end || next == -1))
                {
                    string innertext = str.Substring(1, end - 1);
                    int space = innertext.IndexOf(" ");
                    string link;
                    string text;

                    if (space == -1)
                    {
                        link = innertext;
                        text = innertext;
                    }
                    else
                    {
                        link = innertext.Substring(0, space);
                        text = innertext.Substring(space).Trim();
                    }

                    retval += CreateTag("a", "href=\"" + link + "\"");
                    retval += text;
                    retval += CloseTag("a");

                    i += end;
                    continue;
                }
            }

            retval += line[i];
        }

        return retval;
    }

    private static string DoPreformat(WikiRendererState rs)
    {
        string retval = "";

        if (rs.lastLine.StartsWith(" ") == false)
            retval += CreateTag("pre");

        retval += rs.line.Substring(1);

        if (rs.nextLine.StartsWith(" ") == false)
            retval += CloseTag("pre");

        return retval;
    }

    private static string DoBullet(WikiRendererState rs)
    {
        string retval = "";
        bool closeListItem = true;

        if (rs.line.StartsWith("*") == false)
            throw new Exception("Shouldn't be here.");
        
        int listLevel = 0;

        for (int i = 0; rs.line[i] == '*'; i++)
            listLevel++;

        if (string.IsNullOrEmpty(rs.bulletState) || listLevel != rs.bulletLevel)
        {
            rs.bulletState = "ul";
            retval += CreateTag("ul") + "\r\n";
            closeListItem = false;

            rs.bulletLevel = listLevel;
        }


        if (closeListItem)
            retval += CloseTag("li");

        retval += CreateTag("li") + ParseLine(rs.line.Substring(rs.bulletLevel).Trim(), rs) + CloseTag("li");

        if (rs.nextLine == null || rs.nextLine.StartsWith("*") == false)
        {
            while (rs.bulletLevel > 0)
            {
                retval += "\r\n" + CloseTag(rs.bulletState);
                rs.bulletLevel--;

                if (rs.bulletLevel > 0)
                    retval += CloseTag("li");
            }

            rs.bulletState = "";
        }

        return retval;
    }

    private static string CreateTag(string tagname)
    {
        return CreateTag(tagname, "");
    }
    private static string CreateTag(string tagname, string attributes)
    {
        return CreateTag(tagname, attributes, "Wiki");
    }
    private static string CreateTag(string tagname, string attributes, string cssClass)
    {
        return "<" + tagname + " " + attributes + " class=\"" + cssClass + "\">";
    }
    private static string CloseTag(string tagname)
    {
        return "</" + tagname + ">";
    }
    private static string DoHeader(WikiRendererState rs)
    {
        string retval;
        rs.line = rs.line.TrimEnd();

        if (CheckEnds(rs.line, "===="))
            retval = CreateTag("h4") + 
                ParseLine(rs.line.Substring(4, rs.line.Length - 8), rs).Trim() + 
                CloseTag("h4");
        else if (CheckEnds(rs.line, "==="))
            retval = CreateTag("h3") + 
                ParseLine(rs.line.Substring(3, rs.line.Length - 6), rs).Trim() + 
                CloseTag("h3");
        else if (CheckEnds(rs.line, "=="))
            retval = CreateTag("h2") + 
                ParseLine(rs.line.Substring(2, rs.line.Length - 4), rs).Trim() + 
                CloseTag("h2");
        else if (CheckEnds(rs.line, "="))
            retval = CreateTag("h1") +
                ParseLine(rs.line.Substring(1, rs.line.Length - 2), rs).Trim() +
                CloseTag("h1");
        else
            throw new InvalidOperationException("Don't understand line.");
        
        return retval;
    }


    private static bool CheckEnds(string line, string p)
    {
        if (line.StartsWith(p) && line.EndsWith(p))
            return true;
        else
            return false;
    }
}
