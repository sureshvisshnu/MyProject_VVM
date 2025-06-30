using fa.api.Hms;
using fa.model.hms.common;
using fa.model.Hms.Master;
using fa.views.hms.Masters;
using Fa.api.Hms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.hms.helper
{
    public partial class FormSelectLabTest : FormPatientBase
    {
        List<MedicalTest> SelectedLabTests = new List<MedicalTest>();
        List<MedicalTestElement> SelectedElementss = new List<MedicalTestElement>();

        List<long> LabTestsids = new List<long>();
        List<long> LabTestsElementids = new List<long>();
        public List<long> Oldids = new List<long>();
        public List<long> Newids = new List<long>();
        public bool isDirty = false;

        public DataGridViewComboBoxCell SelectedLabTestsids;
        public DataGridViewComboBoxCell SelectedLabTestsElementids;
        public DataGridViewComboBoxCell SelectedLabTestsFees;
        public DataGridViewComboBoxCell SelectedLabTestsDisc;
        public string SelectedLabTestsNames;
        FormPatientBase parent = null;
        public FormSelectLabTest(Object Sender)
        {
            if (Sender is FormSelectLabTest)
            {
                parent = (FormSelectLabTest)Sender;
            }
            InitializeComponent();
        }
        private void FormSelectLabTest_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadLabTestIds();
            LoadLabTestElementIds();
            LoadLabTestSelectedLabTest();
            Cursor.Current = Cursors.Default;
        }
        private void LoadLabTestIds()
        {
            Oldids = new List<long>();
            Newids = new List<long>();
            LabTestsids = new List<long>();
            if (SelectedLabTestsids != null && SelectedLabTestsids.Items.Count > 0)
            {
                foreach (var Id in SelectedLabTestsids.Items)
                {
                    Oldids.Add(long.Parse(Id.ToString()!));
                    Newids.Add(long.Parse(Id.ToString()!));
                    LabTestsids.Add(long.Parse(Id.ToString()!));
                }
            }
        }
        private void LoadLabTestElementIds()
        {
            LabTestsElementids = new List<long>();
            if (SelectedLabTestsElementids != null && SelectedLabTestsElementids.Items.Count > 0)
            {
                foreach (var Id in SelectedLabTestsElementids.Items)
                {
                    LabTestsElementids.Add(long.Parse(Id.ToString()!));
                }
            }
        }
        private void LoadLabTestSelectedLabTest()
        {
            string FilterString = TextBoxLabtestSearch.Text.Trim();
            string SelectedFilterString = TextBoxSelectedLabtestSearch.Text.Trim();

            TreeViewLabtest.Nodes.Clear();
            TreeViewSelectedLabTest.Nodes.Clear();
            SelectedLabTests = new List<MedicalTest>();
            SelectedElementss = new List<MedicalTestElement>();

            IList<MedicalTest> LabTest = MedicalTestManager.Instance.ListActiveMedicalTestByCompanyId(Global.Company.CompanyId);
            if (LabTest.Count > 0)
            {
                foreach (var lLabTest in LabTest.OrderBy(l => l.Name))
                {
                    if (!LabTestsids.Contains(lLabTest.Id))
                    {
                        if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lLabTest.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            if (!string.IsNullOrEmpty(lLabTest.Name))
                            {
                                TreeNode treeNode = new TreeNode();
                                treeNode.Text = lLabTest.Name;
                                treeNode.Name = lLabTest.Id.ToString();
                                TreeViewLabtest.Nodes.Add(treeNode);

                                if (lLabTest.HasElement && lLabTest.TestElements.Count > 0)
                                {
                                    LoadChildNodesDistinct(TreeViewSelectedLabTest, treeNode, lLabTest.TestElements.ToList());
                                }
                            }
                        }
                    }
                }
            }
            if (LabTestsids.Count > 0)
            {
                IList<MedicalTest> MedicalTest = MedicalTestManager.Instance.ListMedicalTestByCompanyId(Global.Company.CompanyId).OrderBy(x => x.Name).ToList();
                foreach (var lLabTest in LabTestsids)
                {
                    var lMedicalTest = MedicalTest.FirstOrDefault(x => x.Id == lLabTest);
                    if (lMedicalTest != null)
                    {
                        if (SelectedFilterString == null || string.IsNullOrEmpty(SelectedFilterString.Trim()) || lMedicalTest.Name.IndexOf(SelectedFilterString, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            SelectedLabTests.Add(lMedicalTest);
                            if (!string.IsNullOrEmpty(lMedicalTest.Name))
                            {
                                TreeNode treeNode = new TreeNode();
                                treeNode.Text = lMedicalTest.Name;
                                treeNode.Name = lMedicalTest.Id.ToString() + (lMedicalTest.IsActive ? "" : "@");
                                TreeViewSelectedLabTest.Nodes.Add(treeNode);
                                if (lMedicalTest.HasElement && lMedicalTest.TestElements.Count > 0)
                                {
                                    LoadChildNodesDistinct(TreeViewLabtest, treeNode, lMedicalTest.TestElements.ToList());
                                }
                            }
                        }
                    }
                }
            }

        }
        private void LoadChildNodesDistinct(TreeView TreeView, TreeNode ParentNode, IList<MedicalTestElement> MedicalTestElements)
        {
            HashSet<string> uniqueElementNames = new HashSet<string>();

            foreach (var Element in MedicalTestElements.GroupBy(e => e.Name).Select(group => group.First()))
            {
                bool ConditionChecking = TreeView == TreeViewLabtest ? true : false;

                if (LabTestsElementids.Contains(Element.Id) == ConditionChecking)
                {
                    TreeNode ChildNode = new TreeNode
                    {
                        Text = Element.Name,
                        Name = Element.Id.ToString()
                    };

                    ParentNode.Nodes.Add(ChildNode);
                }
                else
                {
                    TreeNode lParentNode = TreeView.Nodes.Find(ParentNode.Name, false).FirstOrDefault()!;

                    if (lParentNode == null)
                    {
                        lParentNode = new TreeNode
                        {
                            Name = ParentNode.Name,
                            Text = ParentNode.Text
                        };
                        TreeView.Nodes.Add(lParentNode);
                    }

                    SelectedElementss.Add(Element);

                    TreeNode ChildNode = new TreeNode
                    {
                        Text = Element.Name,
                        Name = Element.Id.ToString()
                    };

                    lParentNode.Nodes.Add(ChildNode);
                }
            }
        }

        private void LoadChildNodes(TreeView TreeView, TreeNode ParentNode, IList<MedicalTestElement> MedicalTestElements)
        {
            foreach (var Element in MedicalTestElements)
            {
                bool ConditionChecking = TreeView == TreeViewLabtest ? true : false;
                if (LabTestsElementids.Contains(Element.Id) == ConditionChecking)
                {
                    TreeNode ChildNode = new TreeNode();
                    ChildNode.Text = Element.Name;
                    ChildNode.Name = Element.Id.ToString();

                    ParentNode.Nodes.Add(ChildNode);
                }
                else
                {
                    TreeNode lParentNode = null;
                    if (TreeView.Nodes.Find(ParentNode.Name, false).LongLength > 0)
                    {
                        lParentNode = new TreeNode();
                        lParentNode = TreeView.Nodes.Find(ParentNode.Name, false).First();
                    }
                    else
                    {
                        lParentNode = new TreeNode();
                        lParentNode.Name = ParentNode.Name;
                        lParentNode.Text = ParentNode.Text;
                        TreeView.Nodes.Add(lParentNode);
                    }
                    SelectedElementss.Add(Element);

                    TreeNode ChildNode = new TreeNode();
                    ChildNode.Text = Element.Name;
                    ChildNode.Name = Element.Id.ToString();

                    lParentNode.Nodes.Add(ChildNode);
                }
            }
        }
        private void TreeViewLabtest_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
        }

        private void TreeViewSelectedLabTest_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
        }

        private void SelectedNode(TreeNode _SelectedNode)
        {
            if (_SelectedNode != null)
            {
                TreeNode tn = _SelectedNode;

                TreeNode lParentNode = null;
                if (_SelectedNode.Parent != null)
                {
                    if (TreeViewSelectedLabTest.Nodes.Find(_SelectedNode.Parent.Name, false).LongLength > 0)
                    {
                        lParentNode = new TreeNode();
                        lParentNode = TreeViewSelectedLabTest.Nodes.Find(_SelectedNode.Parent.Name, false).First();

                        TreeNode lTreeNode = new TreeNode();
                        lTreeNode.Name = tn.Name;
                        lTreeNode.Text = tn.Text;
                        TreeViewSelectedLabTest.Nodes[lParentNode.Index].Nodes.Add(lTreeNode);
                    }
                    else
                    {
                        lParentNode = new TreeNode();
                        lParentNode.Name = _SelectedNode.Parent.Name;
                        lParentNode.Text = _SelectedNode.Parent.Text;
                        TreeViewSelectedLabTest.Nodes.Add(lParentNode);

                        TreeNode lTreeNode = new TreeNode();
                        lTreeNode.Name = tn.Name;
                        lTreeNode.Text = tn.Text;

                        lParentNode.Nodes.Add(lTreeNode);
                        if (SelectedLabTests.SingleOrDefault(x => x.Id == long.Parse(lParentNode.Name)) == null)
                        {
                            SelectedLabTests.Add(MedicalTestManager.Instance.GetMedicalTestById(long.Parse(lParentNode.Name)));
                        }
                    }
                    SelectedElementss.Add(MedicalTestManager.Instance.GetMedicalTestElementById(long.Parse(tn.Name), Global.Company.CompanyId));
                    TreeViewLabtest.Nodes[_SelectedNode.Parent.Index].Nodes.Remove(_SelectedNode);
                }
                else
                {
                    TreeViewLabtest.Nodes.Remove(_SelectedNode);
                    if (!(TreeViewSelectedLabTest.Nodes.Find(tn.Name, false).LongLength > 0))
                    {
                        TreeViewSelectedLabTest.Nodes.Add(tn);
                    }
                    if (SelectedLabTests.SingleOrDefault(x => x.Id == long.Parse(tn.Name)) == null)
                    {
                        SelectedLabTests.Add(MedicalTestManager.Instance.GetMedicalTestById(long.Parse(tn.Name)));
                    }
                }

            }
        }

        private void BtnSelectLabtest_Click(object sender, EventArgs e)
        {
            TextBoxSelectedLabtestSearch.ResetText();
            int PCount = TreeViewLabtest.Nodes.Count;
            for (int i = (PCount - 1); i > -1; i--)
            {
                if (TreeViewLabtest.Nodes[i] != null && TreeViewLabtest.Nodes[i].Nodes != null && TreeViewLabtest.Nodes[i].Nodes.Count > 0)
                {
                    int Count = TreeViewLabtest.Nodes[i].Nodes.Count;
                    for (int j = (Count - 1); j > -1; j--)
                    {
                        if (TreeViewLabtest.Nodes[i].Nodes[j] != null && TreeViewLabtest.Nodes[i].Nodes[j].Checked)
                        {
                            SelectedNode(TreeViewLabtest.Nodes[i].Nodes[j]);
                        }
                    }
                }
                if (TreeViewLabtest.Nodes[i] != null && TreeViewLabtest.Nodes[i].Checked)
                {
                    SelectedNode(TreeViewLabtest.Nodes[i]);
                }
            }
        }
        private void RemovedNode(TreeNode _SelectedNode)
        {
            if (_SelectedNode != null)
            {
                TreeNode tn = _SelectedNode;

                TreeNode lParentNode = null;
                if (_SelectedNode.Parent != null)
                {
                    if (TreeViewLabtest.Nodes.Find(_SelectedNode.Parent.Name, false).LongLength > 0)
                    {
                        lParentNode = new TreeNode();
                        lParentNode = TreeViewLabtest.Nodes.Find(_SelectedNode.Parent.Name, false).First();

                        TreeNode lTreeNode = new TreeNode();
                        lTreeNode.Name = tn.Name;
                        lTreeNode.Text = tn.Text;
                        TreeViewLabtest.Nodes[lParentNode.Index].Nodes.Add(lTreeNode);
                    }
                    else
                    {
                        lParentNode = new TreeNode();
                        lParentNode.Name = _SelectedNode.Parent.Name;
                        lParentNode.Text = _SelectedNode.Parent.Text;
                        if (!_SelectedNode.Parent.Name.Contains("@"))
                        {
                            TreeViewLabtest.Nodes.Add(lParentNode);

                            TreeNode lTreeNode = new TreeNode();
                            lTreeNode.Name = tn.Name;
                            lTreeNode.Text = tn.Text;

                            lParentNode.Nodes.Add(lTreeNode);
                        }
                        var item = SelectedLabTests.FirstOrDefault(x => x.Id == long.Parse(lParentNode.Name.Contains("@") ? lParentNode.Name.Replace("@", "") : lParentNode.Name));
                        SelectedLabTests.Remove(item);
                    }
                    var litem = SelectedElementss.SingleOrDefault(x => x.Id == long.Parse(tn.Name));
                    SelectedElementss.Remove(litem);

                    TreeViewSelectedLabTest.Nodes[_SelectedNode.Parent.Index].Nodes.Remove(_SelectedNode);

                }
                else
                {

                    TreeViewSelectedLabTest.Nodes.Remove(_SelectedNode);
                    var item = SelectedLabTests.SingleOrDefault(x => x.Id == long.Parse(tn.Name.Contains("@") ? tn.Name.Replace("@", "") : tn.Name));
                    SelectedLabTests.Remove(item);

                    if (!tn.Name.Contains("@") && !(TreeViewLabtest.Nodes.Find(tn.Name, false).LongLength > 0))
                    {
                        TreeViewLabtest.Nodes.Add(tn);
                    }
                }
            }
        }
        private void BtnRemoveLabtest_Click(object sender, EventArgs e)
        {
            TextBoxLabtestSearch.ResetText();
            int PCount = TreeViewSelectedLabTest.Nodes.Count;
            for (int i = (PCount - 1); i > -1; i--)
            {
                if (TreeViewSelectedLabTest.Nodes[i] != null && TreeViewSelectedLabTest.Nodes[i].Nodes != null && TreeViewSelectedLabTest.Nodes[i].Nodes.Count > 0)
                {
                    int Count = TreeViewSelectedLabTest.Nodes[i].Nodes.Count;
                    for (int j = (Count - 1); j > -1; j--)
                    {
                        if (TreeViewSelectedLabTest.Nodes[i].Nodes[j] != null && TreeViewSelectedLabTest.Nodes[i].Nodes[j].Checked)
                        {
                            RemovedNode(TreeViewSelectedLabTest.Nodes[i].Nodes[j]);
                        }
                    }
                }
                if (TreeViewSelectedLabTest.Nodes[i] != null && TreeViewSelectedLabTest.Nodes[i].Checked)
                {
                    RemovedNode(TreeViewSelectedLabTest.Nodes[i]);
                }
            }


        }
        private void TextBoxLabtestSearch_TextChanged(object sender, EventArgs e)
        {
            string FilterString = TextBoxLabtestSearch.Text.Trim();
            TreeViewLabtest.Nodes.Clear();

            IList<MedicalTest> LabTest = MedicalTestManager.Instance.ListActiveMedicalTestByCompanyId(Global.Company.CompanyId);
            if (LabTest != null && LabTest.Count > 0)
            {
                foreach (var lLabTest in LabTest.OrderBy(l => l.Name))
                {
                    if (SelectedLabTests.Any(x => x.Id == lLabTest.Id)) { continue; }
                    if (string.IsNullOrEmpty(FilterString) ||
                        (lLabTest.Name != null && lLabTest.Name.Contains(FilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (lLabTest.TestCode != null && lLabTest.TestCode.Contains(FilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (lLabTest.TestShortName != null && lLabTest.TestShortName.Contains(FilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (lLabTest.Keywords != null && lLabTest.Keywords.Any(k => k.Text != null && k.Text.Contains(FilterString, StringComparison.OrdinalIgnoreCase))) ||
                        (lLabTest.TestElements != null && lLabTest.TestElements.Any(e =>
                        (e.Name != null && e.Name.Contains(FilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (e.ElementCode != null && e.ElementCode.Contains(FilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (e.ElementShortName != null && e.ElementShortName.Contains(FilterString, StringComparison.OrdinalIgnoreCase)))))
                    {
                        if (!string.IsNullOrEmpty(lLabTest.Name))
                        {
                            TreeNode treeNode = new TreeNode
                            {
                                Text = lLabTest.Name,
                                Name = lLabTest.Id.ToString()
                            };
                            TreeViewLabtest.Nodes.Add(treeNode);

                            if (lLabTest.TestElements != null && lLabTest.TestElements.Count > 0)
                            {
                                HashSet<string> uniqueElementNames = new HashSet<string>();

                                foreach (var Element in lLabTest.TestElements)
                                {
                                    if (uniqueElementNames.Add(Element.Name) && SelectedElementss.FirstOrDefault(x => x.Id == Element.Id) == null)
                                    {
                                        TreeNode childtreeNode = new TreeNode
                                        {
                                            Text = Element.Name,
                                            Name = Element.Id.ToString()
                                        };
                                        treeNode.Nodes.Add(childtreeNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void TextBoxSelectedLabtestSearch_TextChanged(object sender, EventArgs e)
        {
            string SelectedFilterString = TextBoxSelectedLabtestSearch.Text.Trim();
            TreeViewSelectedLabTest.Nodes.Clear();
            if (SelectedLabTests.Count > 0)
            {
                foreach (var lLabTest in SelectedLabTests.OrderBy(x => x.Name))
                {
                    if (string.IsNullOrEmpty(SelectedFilterString) ||
                        (lLabTest.Name != null && lLabTest.Name.Contains(SelectedFilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (lLabTest.TestCode != null && lLabTest.TestCode.Contains(SelectedFilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (lLabTest.TestShortName != null && lLabTest.TestShortName.Contains(SelectedFilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (lLabTest.Keywords != null && lLabTest.Keywords.Any(k => k.Text != null && k.Text.Contains(SelectedFilterString, StringComparison.OrdinalIgnoreCase))) ||
                        (lLabTest.TestElements != null && lLabTest.TestElements.Any(e =>
                        (e.Name != null && e.Name.Contains(SelectedFilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (e.ElementCode != null && e.ElementCode.Contains(SelectedFilterString, StringComparison.OrdinalIgnoreCase)) ||
                        (e.ElementShortName != null && e.ElementShortName.Contains(SelectedFilterString, StringComparison.OrdinalIgnoreCase)))))
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = lLabTest.IsActive ? lLabTest.Name + "@" : lLabTest.Name;
                        treeNode.Name = lLabTest.Id.ToString();
                        TreeViewSelectedLabTest.Nodes.Add(treeNode);

                        IList<MedicalTestElement> MedicalTestElement = MedicalTestManager.Instance.ListMedicalTestElementByMedicalTestId(lLabTest.Id);
                        if (MedicalTestElement != null && MedicalTestElement.Count > 0)
                        {
                            foreach (var Element in MedicalTestElement)
                            {
                                if (SelectedElementss.FirstOrDefault(x => x.Id == Element.Id) != null)
                                {
                                    TreeNode childtreeNode = new TreeNode();
                                    childtreeNode.Text = Element.Name;
                                    childtreeNode.Name = Element.Id.ToString();
                                    treeNode.Nodes.Add(childtreeNode);
                                }
                            }
                        }
                    }
                }
            }
        }



        private void BtnLabtestDone_Click(object sender, EventArgs e)
        {
            /*
            // Creating new instances of DataGridViewComboBoxCell for each field
            SelectedLabTestsids = new DataGridViewComboBoxCell();
            SelectedLabTestsElementids = new DataGridViewComboBoxCell();
            SelectedLabTestsDisc = new DataGridViewComboBoxCell();
            SelectedLabTestsFees = new DataGridViewComboBoxCell();
            SelectedLabTestsNames = string.Empty;
            Newids = new List<long>();

            if (TreeViewSelectedLabTest.Nodes.Count > 0)
            {
                foreach (TreeNode Node in TreeViewSelectedLabTest.Nodes)
                {
                    // Extract Name
                    string Name = Node.Name.Contains("@") ? Node.Name.Replace("@", "") : Node.Name;
                    SelectedLabTestsids.Value = Name; // You should use Value instead of Items.Add
                    Newids.Add(long.Parse(Name));

                    // Fetch Medical Test
                    MedicalTest Test = MedicalTestManager.Instance.GetMedicalTestById(long.Parse(Name));
                    if (Test != null)
                    {
                        // Add description
                        SelectedLabTestsDisc.Value = Test.Description;

                        // Get fee from TestElements
                        double fee = Test.TestElements?.FirstOrDefault()?.Fee ?? 0;

                        // Add fee value to the ComboBox cell
                        SelectedLabTestsFees.Value = fee;

                        // Update SelectedLabTestsNames
                        SelectedLabTestsNames = string.IsNullOrEmpty(SelectedLabTestsNames) ? Test.Name : SelectedLabTestsNames + ",\n " + Test.Name;
                    }

                    // Handling child nodes
                    if (Node != null && Node.Nodes != null && Node.Nodes.Count > 0)
                    {
                        foreach (TreeNode ChildNode in Node.Nodes)
                        {
                            string CName = ChildNode.Name.Contains("@") ? ChildNode.Name.Replace("@", "") : ChildNode.Name;
                            SelectedLabTestsElementids.Value = CName; // Use Value property instead of Items.Add
                        }
                    }
                }
            }

            this.Close();
            */
            SelectedLabTestsids = new DataGridViewComboBoxCell();
            SelectedLabTestsElementids = new DataGridViewComboBoxCell();
            SelectedLabTestsDisc = new DataGridViewComboBoxCell();
            SelectedLabTestsFees = new DataGridViewComboBoxCell();
            SelectedLabTestsNames = string.Empty;
            Newids = new List<long>();
            if (TreeViewSelectedLabTest.Nodes.Count > 0)
            {
                foreach (TreeNode Node in TreeViewSelectedLabTest.Nodes)
                {
                    string Name = Node.Name.Contains("@") ? Node.Name.Replace("@", "") : Node.Name;
                    SelectedLabTestsids.Items.Add(Name);
                    Newids.Add(long.Parse(Name));
                    MedicalTest Test = MedicalTestManager.Instance.GetMedicalTestById(long.Parse(Name));
                    if (Test != null)
                    {
                        SelectedLabTestsDisc.Items.Add(Test.Description);
                        double fee = Test.TestElements?.FirstOrDefault()?.Fee ?? 0; 
                        SelectedLabTestsFees.Items.Add(fee);
                        // SelectedLabTestsFees.Items.Add(Test.Fee);
                        SelectedLabTestsNames = string.IsNullOrEmpty(SelectedLabTestsNames) ? Test.Name : SelectedLabTestsNames + ",\n " + Test.Name;
                    }
                    if (Node != null && Node.Nodes != null && Node.Nodes.Count > 0)
                    {
                        foreach (TreeNode ChildNode in Node.Nodes)
                        {
                            string CName = ChildNode.Name.Contains("@") ? ChildNode.Name.Replace("@", "") : ChildNode.Name;
                            SelectedLabTestsElementids.Items.Add(ChildNode.Name);
                        }
                    }
                }
            }
            this.Close();
            // */
        }

        private void BtnLabtestCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadLabTestSelectedLabTest();
        }
        private void ResetForm()
        {
            TextBoxSelectedLabtestSearch.ResetText();
            TextBoxLabtestSearch.ResetText();
        }
        bool EnableAfterCheck = true;
        private void TreeViewLabtest_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (EnableAfterCheck)
            {
                EnableAfterCheck = false;
                if (e.Node.Parent != null)
                {
                    TreeNode[] node = TreeViewLabtest.Nodes.Find(e.Node.Parent.Name, true);
                    bool ParentCheckedStatus = true;
                    foreach (TreeNode chnode in node[0].Nodes)
                    {
                        if (chnode.Checked != e.Node.Checked)
                        {
                            ParentCheckedStatus = false;
                        }
                    }
                    if ((ParentCheckedStatus && e.Node.Checked) || (!ParentCheckedStatus && !e.Node.Checked))
                    {
                        node[0].Checked = e.Node.Checked;
                    }
                }
                TreeNode[] tns = TreeViewLabtest.Nodes.Find(e.Node.Name, true);
                if (e.Node.Parent == null && tns[0].Nodes.Count > 0)
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
        bool lEnableAfterCheck = true;
        private void TreeViewSelectedLabTest_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (lEnableAfterCheck)
            {
                //if(e.Node!=null && e.Node.Name.Contains("@"))
                //{
                //    return;
                //}
                lEnableAfterCheck = false;
                if (e.Node.Parent != null)
                {
                    TreeNode[] node = TreeViewSelectedLabTest.Nodes.Find(e.Node.Parent.Name, true);
                    bool ParentCheckedStatus = true;
                    foreach (TreeNode chnode in node[0].Nodes)
                    {
                        if (chnode.Checked != e.Node.Checked)
                        {
                            ParentCheckedStatus = false;
                        }
                    }
                    if ((ParentCheckedStatus && e.Node.Checked) || (!ParentCheckedStatus && !e.Node.Checked))
                    {
                        node[0].Checked = e.Node.Checked;
                    }
                }
                TreeNode[] tns = TreeViewSelectedLabTest.Nodes.Find(e.Node.Name, true);
                if (e.Node.Parent == null && tns[0].Nodes.Count > 0)
                {
                    CheckedChange(e, tns[0]);
                }
                lEnableAfterCheck = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                BtnLabtestCancel.PerformClick();
            }
            if (keyData == Keys.F8)
            {
                BtnLabtestDone.PerformClick();
            }
            if (keyData == Keys.F3)
            {
                BtnNewLabTest.PerformClick();
            }
            if (keyData == Keys.Tab && ActiveControl == TreeViewLabtest)
            {
                BtnSelectLabtest.Focus();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnSelectLabtest)
            {
                BtnLabtestDone.Focus();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnLabtestDone)
            {
                TextBoxLabtestSearch.Focus();
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == BtnLabtestDone)
            {
                if (TreeViewSelectedLabTest.Nodes.Count > 0)
                {
                    TreeViewSelectedLabTest.Focus();
                    return true;
                }
                else
                {
                    BtnSelectLabtest.Focus();
                    return true;
                }
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == BtnSelectLabtest)
            {
                TreeViewLabtest.Focus();
                if (TreeViewLabtest.Nodes.Count > 0)
                {
                    TreeViewLabtest.SelectedImageIndex = 0;
                }
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == TextBoxLabtestSearch)
            {
                BtnLabtestDone.Focus();
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == TreeViewSelectedLabTest)
            {
                BtnRemoveLabtest.Focus();
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == BtnRemoveLabtest)
            {
                TreeViewLabtest.Focus();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnRemoveLabtest)
            {
                if (TreeViewSelectedLabTest.Nodes.Count > 0)
                {
                    TreeViewSelectedLabTest.Focus();
                    return true;
                }
                else
                {
                    BtnLabtestDone.Focus();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public void RefershLabTest()
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadLabTestIds();
            LoadLabTestElementIds();
            LoadLabTestSelectedLabTest();
            Cursor.Current = Cursors.Default;
        }

        private void BtnNewLabTest_Click(object sender, EventArgs e)
        {
            FormMedicalTest FormMedicalTest = new FormMedicalTest();
            FormMedicalTest.CreateMedicalTestOnLoad = true;
            FormMedicalTest.ShowDialog(this);
            RefershLabTest();
        }
    }
}
