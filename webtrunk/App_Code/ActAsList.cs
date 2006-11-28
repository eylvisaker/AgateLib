using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/*
 * The following class is *VERY* fragile.  There are many assumptions/requirements with regards to the data
 *  that it handles.  If any of these are broken, or changed, it *will* result in unexpected behavior.
 * 
 * DATASET TABLE LAYOUT
 *  This requires a primary key column of "id" & a position identifier column of "Position", both of which are type int.
 *   These can be generalized to get around this requirement, but for the sake of brevity, it has not been done.
 * 
 * POSITION COLUMN CONTAINS STRICTLY CONTIGUOUS INTEGERS
 *   This is very very important.  Every movement operation we do works under the assumption that everything in the 
 *   position column data is contiguous.
 *   
 */
public class ActAsList
{
    private DataSet data_set;
    private String table_name;


    public ActAsList(DataSet ds, String tbl_name)
    {
        data_set = ds;
        table_name = tbl_name;
    }


    // ////////////// PROPERTIES ///////////////////
    public DataSet DataSet
    {
        get { return data_set; }
    }



    // ////////////  METHODS /////////////////////////


    // move the row to the top of the list
    public void MoveToTop(int row_id)
    {
        if (!this.Exists(row_id))
            throw new System.ArgumentException(" Row to be moved does not exist in Data Set ");


        DataRow row_to_be_moved = this.GetRowByID(row_id);

        int current_position = (int)row_to_be_moved["Position"];

        // move up until on top
        for (int i = current_position; i > 0; i--)
            this.MoveUp(row_id);

    }


    // move the row to the bottom of the list
    public void MoveToBottom(int row_id)
    {
        if (!this.Exists(row_id))
            throw new System.ArgumentException(" Row to be moved does not exist in Data Set ");


        DataRow row_to_be_moved = this.GetRowByID(row_id);

        int current_position = (int)row_to_be_moved["Position"];
        int max_position = data_set.Tables[table_name].Rows.Count - 1;

        // move down until in back
        for (int i = current_position; i < max_position; i++)
            this.MoveDown(row_id);
    }


    // move the row up N positions
    public void MoveUpN(int row_id, int n)
    {
        // move up until on top
        for (int i = 0; i < n; i++)
            this.MoveUp(row_id);
    }

    // move the row down N positions
    public void MoveDownN(int row_id, int n)
    {
        // move up until on top
        for (int i = 0; i < n; i++)
            this.MoveDown(row_id);
    }


    // Move the row up 1 position
    public void MoveUp(int row_id)
    {
        DataRow current_row = this.GetRowByID(row_id);



        if (current_row != null)
        {
            /*
             * if current position is 0, return. if current_position > 0,
             *  then get row with position of current_position - 1 (directly above it in the list) & swap their positions.
             */

            int current_position = (int)current_row["Position"];

            if (current_position == 0)
                return;

            else if (current_position > 0)
            {
                DataRow upper_row = get_row_with_position((int)current_row["Position"] - 1);

                if (upper_row != null)
                {
                    upper_row["Position"] = current_position;
                    current_row["Position"] = current_position - 1;
                }
                else
                    throw new System.ApplicationException("DataSet contains positions that are not contiguous");
            }
            else
                throw new System.ApplicationException("DataSet contains a negative position");
        }
    }



    // move the row down 1 position
    public void MoveDown(int row_id)
    {
        DataRow current_row = this.GetRowByID(row_id);
        int max_position = data_set.Tables[table_name].Rows.Count - 1;

        if (current_row != null)
        {
            /*
             * if current position is max_position, return. if current_position < max_position,
             *  then get row with position of current_position + 1 (directly below it in the list) & swap their positions.
             */

            int current_position = (int)current_row["Position"];

            if (current_position == max_position)
                return;


            else if (current_position < max_position)
            {
                DataRow lower_row = get_row_with_position((int)current_row["Position"] + 1);

                if (lower_row != null)
                {
                    lower_row["Position"] = current_position;
                    current_row["Position"] = current_position + 1;
                }
                else
                    throw new System.ApplicationException("DataSet contains positions that are not contiguous");
            }
            else
                throw new System.ApplicationException("DataSet contains positions that are not contiguous");
        }
    }


    public DataRow GetRowByID(int row_id)
    {
        DataRow[] rows_with_id = data_set.Tables[table_name].Select("id = '" + row_id.ToString() + "'");

        if (rows_with_id.Length == 1)
        {
            return rows_with_id[0];

        }
        else if (rows_with_id.Length > 1)
        {
            throw new System.ApplicationException("DataSet is showing duplicated primary keys");
        }
        else
            return null;
    }

    public bool Exists(int row_id)
    {
        return (GetRowByID(row_id) != null);
    }


    // ///////////////////////// PRIVATE FUNCTIONS ///////////////////////

    public bool RowExists(int row_id)
    {
        return (GetRowByID(row_id) != null);
    }

    private DataRow get_row_with_position(int position)
    {
        DataRow[] rows_with_position = data_set.Tables[table_name].Select("Position = '" + position.ToString() + "'");

        if (rows_with_position.Length == 1)
        {
            return rows_with_position[0];

        }
        else if (rows_with_position.Length > 1)
        {
            throw new System.ApplicationException("DataSet is showing multiple entries for the same position");
        }
        else
            return null;
    }

}
