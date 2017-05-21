//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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

using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Layout;

namespace AgateLib.Platform.WinForms.GuiDebug
{
	public partial class frmGuiDebug : Form
	{
		Dictionary<AgateLib.UserInterface.Widgets.Widget, TreeNode> mNodes = new Dictionary<UserInterface.Widgets.Widget, TreeNode>();
		Dictionary<AgateLib.UserInterface.Widgets.Widget, AgateLib.UserInterface.Widgets.FacetScene> mGuiMap = new Dictionary<UserInterface.Widgets.Widget, UserInterface.Widgets.FacetScene>();

		TreeNode RootNode { get { return tvWidgets.Nodes[0]; } }

		public frmGuiDebug()
		{
			InitializeComponent();
		}

		//protected override CreateParams CreateParams
		//{
		//	get
		//	{
		//		const int WS_EX_NOACTIVATE = 0x08000000;

		//		CreateParams cp = base.CreateParams;
		//		cp.ExStyle |= WS_EX_NOACTIVATE;
		//		return cp;
		//	}
		//}

		private void frmGuiDebug_Load(object sender, EventArgs e)
		{
			var screens = Screen.AllScreens;
			var targetScreen = Screen.PrimaryScreen;

			Top = targetScreen.WorkingArea.Top;
			Left = targetScreen.WorkingArea.Left;
			Height = targetScreen.WorkingArea.Height;

			MarkTypesExpandable();

			AgateLib.UserInterface.GuiStack.GuiEvent += GuiStack_GuiEvent;
		}

		void GuiStack_GuiEvent(object sender, InputLib.AgateInputEventArgs e)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new EventHandler<InputLib.AgateInputEventArgs>(GuiStack_GuiEvent), sender, e);
				return;
			}

			StringBuilder b = new StringBuilder();

			b.AppendLine(sender.ToString());
			b.AppendLine(e.InputEventType.ToString());
			b.AppendLine("Mouse: " + e.MousePosition.ToString());

			txtEvent.Text = b.ToString();

			if (chkSelect.Checked && e.InputEventType == InputLib.InputEventType.MouseDown)
			{
				var widget = (AgateLib.UserInterface.Widgets.Widget)sender;
				if (widget is Desktop)
					return;

				EnsureInTree(widget);

				tvWidgets.SelectedNode = mNodes[widget];
			}
		}

		private void EnsureInTree(UserInterface.Widgets.Widget widget)
		{
			if (IsInTree(widget) == false && widget is UserInterface.Widgets.Desktop == false)
				EnsureInTree(widget.Parent);

			TreeViewAddCheck(widget);
		}

		private bool IsInTree(UserInterface.Widgets.Widget widget)
		{
			return mNodes.ContainsKey(widget);
		}

		private void MarkTypesExpandable()
		{
			var types = typeof(AgateLayoutEngine).Assembly.DefinedTypes;

			foreach (var type in types)
			{
				if (type.Namespace == null)
					continue;

				if (type.Namespace == "AgateLib.Geometry" ||
					type.Namespace.StartsWith("AgateLib.UserInterface"))
				{
					MarkTypeExpandable(type);
				}
			}
		}

		private void MarkTypeExpandable(System.Reflection.TypeInfo type)
		{
			var attr = new TypeConverterAttribute(typeof(ExpandableObjectConverter));
			TypeDescriptor.AddAttributes(type.AsType(), attr);
		}


		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateTreeView();
		}

		List<AgateLib.UserInterface.Widgets.Widget> itemsToRemove = new List<AgateLib.UserInterface.Widgets.Widget>();
		AgateLib.UserInterface.Widgets.FacetScene currentFacetScene;

		private void UpdateTreeView()
		{
			var list = new List<FacetScene>(AgateLib.UserInterface.GuiStack.Items.Count());
			list.AddRange(AgateLib.UserInterface.GuiStack.Items);

			foreach (var gui in list)
			{
				var adapter = GetAdapter(gui);

				if (adapter == null)
					continue;

				currentFacetScene = gui;

				TreeViewAddCheck(gui.Desktop);
			}

			itemsToRemove.Clear();

			foreach (var item in mNodes.Keys)
			{
				if (item is AgateLib.UserInterface.Widgets.Desktop)
				{
					if (list.All(x => x.Desktop != item))
						itemsToRemove.Add(item);
				}
				else
				{
					if (item.Parent == null)
						itemsToRemove.Add(item);
				}
			}

			foreach (var item in itemsToRemove)
			{
				mNodes[item].Remove();
				mNodes.Remove(item);
			}

			UpdateAnimatorProperties();
		}

		private void TreeViewAddCheck(AgateLib.UserInterface.Widgets.Widget widget)
		{
			if (mNodes.ContainsKey(widget))
				return;
			if (currentFacetScene == null)
				return;

			mNodes[widget] = new TreeNode(widget.ToString()) { Tag = widget };
			mGuiMap[widget] = currentFacetScene;

			if (widget.Parent == null)
			{
				RootNode.Nodes.Add(mNodes[widget]);
			}
			else
			{
				var parentNode = mNodes[widget.Parent];

				int index = widget.Parent.LayoutChildren.ToList().IndexOf(widget);

				if (index < mNodes[widget.Parent].Nodes.Count)
					parentNode.Nodes.Insert(index, mNodes[widget]);
				else
					parentNode.Nodes.Add(mNodes[widget]);
			}

			var container = widget as AgateLib.UserInterface.Widgets.Container;
			if (container != null)
			{
				List<Widget> children = new List<Widget>();
				children.AddRange(container.Children);

				foreach (var w in children)
				{
					TreeViewAddCheck(w);
				}
			}
		}

		AgateLib.UserInterface.Widgets.Widget SelectedWidget
		{
			get { return tvWidgets.SelectedNode.Tag as AgateLib.UserInterface.Widgets.Widget; }
		}

		private void tvWidgets_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag == null)
				return;

			var widget = (AgateLib.UserInterface.Widgets.Widget)e.Node.Tag;
			var adapter = GetAdapter(widget);
			var style = adapter.StyleOf(widget);
			var renderer = (AgateUserInterfaceRenderer)widget.MyFacetScene.Renderer;

			pgWidget.SelectedObject = widget;
			pgStyle.SelectedObject = style;
			pgAnimator.SelectedObject = renderer.GetAnimator(widget);
		}

		private IWidgetAdapter GetAdapter(UserInterface.Widgets.Widget widget)
		{
			if (mGuiMap.ContainsKey(widget) == false)
				return null;

			var gui = mGuiMap[widget];
			return GetAdapter(gui);
		}

		private static IWidgetAdapter GetAdapter(UserInterface.Widgets.FacetScene facetScene)
		{
			try
			{
				var renderer = facetScene.Renderer;

				if (renderer == null)
					return null;

				var adapter = renderer.Adapter;
				return adapter;
			}
			catch
			{
				return null;
			}
		}

		private void UpdateAnimatorProperties()
		{
			if (SelectedWidget != null)
			{
				var adapter = GetAdapter(SelectedWidget);

				adapter.StyleOf(SelectedWidget);
				pgStyle.Refresh();
			}
			pgAnimator.Refresh();
		}

		private void chkSelect_CheckedChanged(object sender, EventArgs e)
		{

		}

	}
}
