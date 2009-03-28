using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Tests
{
    public partial class frmLauncher : Form
    {
        Dictionary<string, List<MethodInfo>> tests = new Dictionary<string, List<MethodInfo>>();


        public frmLauncher()
        {
            InitializeComponent();

            Icon = AgateLib.WinForms.FormUtil.AgateLibIcon;

            LoadTests();
        }

        private void FillList()
        {
            foreach (string key in tests.Keys)
            {
                TreeNode n = new TreeNode { Text = key };
                int index = tree.Nodes.Add(n);

                AddChildren(tests[key], index);

                n.Expand();
            }

            tree.Sort();
        }

        private void AddChildren(List<MethodInfo> methods, int nodeIndex)
        {
            foreach (var method in methods)
            {
                AgateTestAttribute a = GetTestAttribute(method);

                tree.Nodes[nodeIndex].Nodes.Add(new TreeNode { Text = a.Name, Tag = method });
            }
        }

        private void LoadTests()
        {
            Assembly myass = Assembly.GetAssembly(typeof(frmLauncher));

            foreach (var t in myass.GetTypes())
            {
                MethodInfo main = t.GetMethod("Main", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (main == null)
                    continue;

                AddTest(main);
            }

            FillList();
        }


        private void AddTest(MethodInfo main)
        {
            AgateTestAttribute attrib = GetTestAttribute(main);
            if (attrib == null)
                return;

            if (tests.ContainsKey(attrib.Category) == false)
            {
                tests[attrib.Category] = new List<MethodInfo>();
            }

            tests[attrib.Category].Add(main);
        }

        private static AgateTestAttribute GetTestAttribute(MethodInfo main)
        {
            var all_attribs = main.GetCustomAttributes(typeof(AgateTestAttribute), false);
            if (all_attribs.Length == 0)
                return null;

            AgateTestAttribute attrib = (AgateTestAttribute)all_attribs[0];
            return attrib;
        }

        private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MethodInfo m = e.Node.Tag as MethodInfo ;
            LaunchMethod(e.Node.Text, m);
        }

        private void LaunchMethod(string name, MethodInfo m)
        {
            if (m == null)
                return;

            string[] args = { "--choose" };
            object[] parameters = new object[] { args };

            this.Hide();

            try
            {
                if (m.GetParameters().Length == 0)
                {
                    m.Invoke(null, null);
                }
                else
                {
                    m.Invoke(null, parameters);
                }
            }
            catch (TargetInvocationException e)
            {
                Exception ex_relevant = e.InnerException ?? e;
                string info = ex_relevant.Message;

                MessageBox.Show(
                    ex_relevant.GetType().Name + Environment.NewLine + info,
                    "AgateLib Test " + name + " threw an exception.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);

            }
            finally
            {
                this.Show();
                this.TopMost = true;
                this.TopMost = false;
                this.Activate();
            }
        }
    }
}