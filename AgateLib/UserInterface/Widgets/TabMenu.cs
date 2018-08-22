//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

//using System;
//using System.Collections.Generic;
//using System.Text;
//using AgateLib.Mathematics.Geometry;
//using Microsoft.Xna.Framework;
//using AgateLib.UserInterface.Layout;

//namespace AgateLib.UserInterface
//{
//    public class TabMenu : RenderWidget
//    {
//        private List<TabPage> tabs = new List<TabPage>();
//        private Menu menu = new Menu();
//        private SingleColumnLayout menuLayout;
//        private SingleRowLayout tabLayout;

//        public TabMenu()
//        {
//            menuLayout = new SingleColumnLayout();
//            tabLayout = new SingleRowLayout();

//            menu.Layout = menuLayout;
//        }

//        private void InitializeMenuItems()
//        {
//            menuLayout.Clear();

//            foreach (var tab in tabs)
//            {
//                menuLayout.Add(new Label() { Text = tab.Text });
//            }
//        }

//        public event Action Exit;
        
//        public override Size ComputeIdealSize(IWidgetRenderContext renderContext, Size maxSize)
//        {
//            return tabLayout.RecalculateSize(this, renderContext, maxSize);
//        }

//        public override void Draw(IWidgetRenderContext renderContext, Point offset)
//        {
//            throw new NotImplementedException();
//        }

//        public void Add(TabPage page)
//        {
//            tabs.Add(page);

//            InitializeMenuItems();
//        }

//        public void Add(params TabPage[] pages)
//        {
//            foreach (var page in pages)
//            {
//                Add(page);
//            }
//        }

//        public override void Update(IWidgetRenderContext renderContext)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class TabPage
//    {
//        private string text;

//        public TabPage(string text)
//        {
//            this.Text = text;
//        }

//        public string Text { get; set; }

//        public IRenderWidget Child { get; set; }
//    }
//}
