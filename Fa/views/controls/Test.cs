using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.views.controls.ComboTreeView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.controls
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {

            TreeNode treeNode = new TreeNode();
            treeNode.Text = "apple";
            treeNode.Name = "1";

            TreeNode ltreeNode = new TreeNode();
            ltreeNode.Text = "green";
            ltreeNode.Name = "2";
            treeNode.Nodes.Add(ltreeNode);

            ltreeNode = new TreeNode();
            ltreeNode.Text = "Blue";
            ltreeNode.Name = "3";
            treeNode.Nodes.Add(ltreeNode);

            ltreeNode = new TreeNode();
            ltreeNode.Text = "yello";
            ltreeNode.Name = "4";
            treeNode.Nodes.Add(ltreeNode);

            //checkedComboBox1.TreeNode = treeNode;



            treeNode = new TreeNode();
            treeNode.Text = "orange";
            treeNode.Name = "5";

            ltreeNode = new TreeNode();
            ltreeNode.Text = "green";
            ltreeNode.Name = "6";
            treeNode.Nodes.Add(ltreeNode);

            ltreeNode = new TreeNode();
            ltreeNode.Text = "Blue";
            ltreeNode.Name = "7";
            treeNode.Nodes.Add(ltreeNode);

            ltreeNode = new TreeNode();
            ltreeNode.Text = "yello";
            ltreeNode.Name = "8";
            treeNode.Nodes.Add(ltreeNode);

            //checkedComboBox1.TreeNode = treeNode;

            //string ss = "";
            //for(int i=1;i<52;i++)
            //{
            //    ss = ss + ",f" + i;
            //}
            //Console.WriteLine(ss);
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //AccountManager AccountManager = AccountManager.Instance;
            //IList<Account> Account = AccountManager.GetAllAccountsByCompanyId(Global.Company.CompanyId).ToArray<Account>();
            //checkedTreeComboBox1.Nodes.Clear();
            //if (Account.Count > 0)
            //{
            //    ComboTreeNode All = new ComboTreeNode();
            //    All.Name = "All";
            //    All.Text = "All";
            //    checkedTreeComboBox1.TreeNodes = All;
            //}
            //foreach (var lAccount in Account)
            //{
            //    if (lAccount.ParentAccountId == null)
            //    {
            //        ComboTreeNode parent = new ComboTreeNode();
            //        parent.Name = lAccount.Id.ToString();
            //        parent.Text = lAccount.Name;
            //        checkedTreeComboBox1.TreeNodes = parent;
            //        foreach (var llAccount in Account.Where(x => x.ParentAccountId == lAccount.Id))
            //        {
            //            ComboTreeNode child = new ComboTreeNode();
            //            child.Name = llAccount.Id.ToString();
            //            child.Text = llAccount.Name;
            //            parent.Nodes.Add(child);
            //        }
            //    }
            //}


            //toolstripCheckedTreeComboBox1.Nodes.Clear();
            //if (Account.Count > 0)
            //{
            //    ComboTreeNode All = new ComboTreeNode();
            //    All.Name = "All";
            //    All.Text = "All";
            //    toolstripCheckedTreeComboBox1.TreeNodes = All;
            //}
            //foreach (var lAccount in Account)
            //{
            //    if (lAccount.ParentAccountId == null)
            //    {
            //        ComboTreeNode parent = new ComboTreeNode();
            //        parent.Name = lAccount.Id.ToString();
            //        parent.Text = lAccount.Name;
            //        toolstripCheckedTreeComboBox1.TreeNodes = parent;
            //        foreach (var llAccount in Account.Where(x => x.ParentAccountId == lAccount.Id))
            //        {
            //            ComboTreeNode child = new ComboTreeNode();
            //            child.Name = llAccount.Id.ToString();
            //            child.Text = llAccount.Name;
            //            parent.Nodes.Add(child);
            //        }
            //    }
            //}







        }



        private void button2_Click(object sender, EventArgs e)
        {
            string output = "";
            if (checkedTreeComboBox1.CheckedNodes != null)
            {
                foreach (ComboTreeNode node in checkedTreeComboBox1.CheckedNodes)
                {
                    output = output + (string.IsNullOrEmpty(output) ? "" : ", ") + node.Text;
                }
            }
            MessageBox.Show(output);






            //string output=string.Empty;
            //IList<AccountGroup> AccountGroup = AccountGroupManager.Instance.GetAllAccountGroup();
            //foreach(AccountGroup ag in AccountGroup)
            //{
            //    Console.WriteLine("var AG"+ ag.Id+ " = context.AccountGroups.Find(" + ag.Id + ")");
            //    output = output + "AG" + ag.Id + ",";
            //}
            //Console.WriteLine(output);
        }

        private void checkedTreeComboBox1_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {

        }

        private void toolstripCheckedTreeComboBox1_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {

        }

        private void delayedTextChangeTextBox1_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void toolstripDelayedTextBox1_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show(toolstripDelayedTextBox1.Text);

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripCheckBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripCheckBox2_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripCheckBox2.Checked) { }
        }

        private void toolStripCheckBox2_CheckStateChanged(object sender, EventArgs e)
        {

        }
    }
}
