using fa.views.utils.Hms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.hms.patient
{
    public partial class FormChartDetails : Form
    {
        public static string SelectPrintingPageErrorMsg = "Please select printing pages.";
        public FormChartDetails()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadTreviewChecked(TreeNodeCollection Nodes)
        {
            foreach (TreeNode node in Nodes)
            {
                node.Checked = true;
                if (node.Nodes.Count > 0)
                {
                    LoadTreviewChecked(node.Nodes);
                }
            }
        }

        List<string> Nodes = new List<string>();
        private List<string> PrintRecursive(TreeNode Node)
        {
            if (Node.Checked == true)
            {
                Nodes.Add(Node.Name);
            }
            foreach (TreeNode tn in Node.Nodes)
            {
                PrintRecursive(tn);
            }
            return Nodes;
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ErrorMsg.Text = string.Empty;
            List<string> Pages = new List<string>();
            foreach (TreeNode ParenNodes in TreeViewPrintingPages.Nodes)
            {
                List<string> AllNodes = PrintRecursive(ParenNodes);
                foreach (var Nodes in AllNodes)
                {
                    if (Nodes == "Consultation") { continue; }
                    Pages.Add(Nodes);
                }
                AllNodes.Clear();
            }

            if (Pages.Count > 0)
            {
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                MemoryStream Stream = new MemoryStream();
                PictureBoxMedicalHis.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] CheckedImg = Stream.ToArray();

                Stream = new MemoryStream();
                PictureBoxMedHisUnChecked.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] UnCheckedImg = Stream.ToArray();

                PatientChartPrinting PatientChartPrinting = new PatientChartPrinting()
                {
                    PatientId = long.Parse(TextBoxPatientId.Text),
                    Pages = Pages,
                    CheckedImg = CheckedImg,
                    UnCheckedImg = UnCheckedImg,
                    IncludeFullChart = true
                };
                PatientChartPrinting.GenerateChart();
                System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
            else
            {
                ErrorMsg.Text = SelectPrintingPageErrorMsg;
                TreeViewPrintingPages.Select();
            }
            Cursor.Current = Cursors.Default;
            BtnPrint.Focus();
        }

        private void FormChartDetails_Load(object sender, EventArgs e)
        {
            ErrorMsg.Text = string.Empty;

            LoadTreviewChecked(TreeViewPrintingPages.Nodes);
            TreeViewPrintingPages.ExpandAll();
        }

        private void TreeViewPrintingPages_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        bool EnableAfterCheck = true;
        private void TreeViewPrintingPages_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (EnableAfterCheck)
            {
                EnableAfterCheck = false;
                if (e.Node.Parent != null)
                {
                    TreeNode[] node = TreeViewPrintingPages.Nodes.Find(e.Node.Parent.Name, true);
                    if (e.Node.Checked)
                    {
                        if (!node[0].Checked)
                        {
                            node[0].Checked = true;
                        }
                    }
                    else if (node[0].Nodes.Count > 1)
                    {
                        bool ParentChecked = false;
                        foreach (TreeNode Tnode in node[0].Nodes)
                        {
                            if (Tnode.Checked)
                            {
                                ParentChecked = true;
                            }
                        }
                        node[0].Checked = ParentChecked;
                    }
                    else
                    {
                        if (node[0].Checked)
                        {
                            node[0].Checked = false;
                        }
                    }
                }
                TreeNode[] tns = TreeViewPrintingPages.Nodes.Find(e.Node.Name, true);
                if (tns[0].Nodes.Count > 0)
                {
                    CheckedChange(e, tns[0]);
                }
                EnableAfterCheck = true;
            }
        }
        private void CheckedChange(TreeViewEventArgs Args, TreeNode TreeNode)
        {
            foreach (TreeNode node in TreeNode.Nodes)
            {
                node.Checked = Args.Node.Checked;
                if (node.Nodes.Count > 0)
                {
                    CheckedChange(Args, node);
                }
            }

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
            }
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
