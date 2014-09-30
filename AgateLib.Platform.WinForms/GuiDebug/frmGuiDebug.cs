﻿using AgateLib.UserInterface.Css.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgateLib.Platform.WinForms.GuiDebug
{
	public partial class frmGuiDebug : Form
	{
		Dictionary<AgateLib.UserInterface.Widgets.Widget, TreeNode> mNodes = new Dictionary<UserInterface.Widgets.Widget, TreeNode>();
		Dictionary<AgateLib.UserInterface.Widgets.Widget, AgateLib.UserInterface.Widgets.Gui> mGuiMap = new Dictionary<UserInterface.Widgets.Widget, UserInterface.Widgets.Gui>();

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
			var types = typeof(CssLayoutEngine).Assembly.DefinedTypes;

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
		AgateLib.UserInterface.Widgets.Gui currentGui;

		private void UpdateTreeView()
		{
			foreach (var gui in AgateLib.UserInterface.GuiStack.Items.ToList())
			{
				var adapter = GetAdapter(gui);

				if (adapter == null)
					continue;

				currentGui = gui;

				TreeViewAddCheck(gui.Desktop);
			}

			itemsToRemove.Clear();

			foreach (var item in mNodes.Keys)
			{
				if (item is AgateLib.UserInterface.Widgets.Desktop)
				{
					if (AgateLib.UserInterface.GuiStack.Items.All(x => x.Desktop != item))
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
			if (currentGui == null)
				return;

			mNodes[widget] = new TreeNode(widget.ToString()) { Tag = widget };
			mGuiMap[widget] = currentGui;

			if (widget.Parent == null)
			{
				RootNode.Nodes.Add(mNodes[widget]);
			}
			else
			{
				var parentNode = mNodes[widget.Parent];

				int index = widget.Parent.Children.IndexOf(widget);

				if (index < mNodes[widget.Parent].Nodes.Count)
					parentNode.Nodes.Insert(index, mNodes[widget]);
				else
					parentNode.Nodes.Add(mNodes[widget]);
			}

			if (widget is AgateLib.UserInterface.Widgets.Container)
			{
				foreach (var w in ((AgateLib.UserInterface.Widgets.Container)widget).Children)
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
			var style = adapter.GetStyle(widget);
			var renderer = (AgateLib.UserInterface.Css.Rendering.CssRenderer)widget.MyGui.Renderer;

			pgWidget.SelectedObject = widget;
			pgStyle.SelectedObject = style;
			pgAnimator.SelectedObject = renderer.GetAnimator(widget);
		}

		private UserInterface.Css.CssAdapter GetAdapter(UserInterface.Widgets.Widget widget)
		{
			if (mGuiMap.ContainsKey(widget) == false)
				return null;

			var gui = mGuiMap[widget];
			return GetAdapter(gui);
		}

		private static UserInterface.Css.CssAdapter GetAdapter(UserInterface.Widgets.Gui gui)
		{
			var layout = (CssLayoutEngine)gui.LayoutEngine;
			var adapter = layout.Adapter;
			return adapter;
		}

		private void UpdateAnimatorProperties()
		{
			if (SelectedWidget != null)
			{
				var adapter = GetAdapter(SelectedWidget);

				adapter.GetStyle(SelectedWidget);
				pgStyle.Refresh();
			}
			pgAnimator.Refresh();
		}

		private void chkSelect_CheckedChanged(object sender, EventArgs e)
		{

		}

	}
}