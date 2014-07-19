using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace NotebookLib.ListBookNavigator
{
    [ToolboxItem(false)]
    public partial class ListBookNavigator : ListBox, INavigator 
    {
        Notebook owner;

        public ListBookNavigator(Notebook owner)
        {
            this.owner = owner;

            this.DrawMode = DrawMode.OwnerDrawVariable;
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            lstBox_MeasureItem(this, e);

            base.OnMeasureItem(e);
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            lstBox_DrawItem(this, e);

            base.OnDrawItem(e);
        }
        
        private void lstBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            int fontHeight = Font.Height;

            int imageHeight = 0;

            NotebookPage p = Items[e.Index] as NotebookPage;

            if (p.Image != null)
            {
                imageHeight = p.Image.Height;
            }

            e.ItemHeight = Math.Max(fontHeight, imageHeight) + 2;
            e.ItemWidth = Width;

            
        }
        private void lstBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                NotebookPage p = Items[e.Index] as NotebookPage;
                string text = p.Text;
                Image image = p.Image;

                if (image == null)
                {
                    if (string.IsNullOrEmpty(text))
                        text = "Untitled Page";

                    e.Graphics.DrawString(text, Font, new SolidBrush(e.ForeColor), e.Bounds.Location);
                }
                else
                {
                    StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
                    format.LineAlignment = StringAlignment.Center;

                    Point location = e.Bounds.Location;
                    location.Y = (e.Bounds.Height - image.Height) / 2 + e.Bounds.Y;

                    e.Graphics.DrawImage(image, new Rectangle(location, image.Size));

                    e.Graphics.DrawString(text, Font, new SolidBrush(e.ForeColor), 
                        new Rectangle(e.Bounds.Left + image.Width + 5, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height),
                        format);
                }
            }
            

            e.DrawFocusRectangle();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            owner.OnSelectedPageChanged();
        }

        public override int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                if (value < 0) return;
                if (value >= base.Items.Count) return;


                if (base.SelectedIndex != value)
                {
                    base.SelectedIndex = value;
                }
            }
        }

        public void RefreshAllPages()
        {
            int oldSelectedIndex = SelectedIndex;
            NotebookPage oldSelectedPage = (NotebookPage)SelectedItem;

            Items.Clear();

            foreach (NotebookPage p in owner.NotebookPages)
            {
                if (p.ShowPage || DesignMode || owner.DesignMode)
                    Items.Add(p);
            }

            if (oldSelectedPage != null && Items.Contains(oldSelectedPage))
            {
                SelectedItem = oldSelectedPage;
            }
            else
            {
                SelectedIndex = Math.Min(oldSelectedIndex, Items.Count - 1);
            }

        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int index = IndexFromPoint(e.Location);

            SelectedIndex = index;

            base.OnMouseClick(e);
        }

        public NavigatorProperties CreateProperties()
        {
            return new ListBookProperties(owner, this);
        }


        public Size NavMinSize
        {
            get { return new Size(75, 75); }
        }

        public Size NavMaxSize
        {
            get { return new Size(200, 200); }
        }

        public void OnUpdateLayout()
        {
            switch(owner.Navigator.Location)
            {
                case NavigatorLocation.Top:
                case NavigatorLocation.Bottom:
                    this.MultiColumn = true;
                    break;

                case NavigatorLocation.Left:
                case NavigatorLocation.Right:
                    this.MultiColumn = false;
                    break;

            }
        }

    }
}
