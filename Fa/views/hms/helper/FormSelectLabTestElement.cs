using fa;
using fa.model.Hms.Master;
using fa.views.hms;
using fa.views.hms.helper;
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
using VisioForge.Libs.NDI;

namespace Fa.views.hms.helper
{
    public partial class FormSelectLabTestElement : FormPatientBase
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
        FormPatientBase parent = null!;

        public FormSelectLabTestElement(Object Sender)
        {
            if (Sender is FormSelectLabTestElement)
            {
                parent = (FormSelectLabTestElement)Sender;
            }
            InitializeComponent();
        }

        private void ResetForm()
        {
            TextBoxLabTestElementSearch.ResetText();
            TextBoxLabTestElementSearch.ResetText();
        }

        private void FormSelectLabTestElement_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            //LoadLabTestIds();
            LoadLabTestElementIds();
            //LoadLabTestSelectedLabTest();
            Cursor.Current = Cursors.Default;
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

        private void LoadLabTestSelectedLabTestElementsOnly()
        {
            // Clear previous nodes
            TreeViewLabTestElement.Nodes.Clear();
            TreeViewSelectedLabTestElement.Nodes.Clear();

            // SelectedLabTests and SelectedElementss can be cleared if needed
            SelectedLabTests = new List<MedicalTest>();
            SelectedElementss = new List<MedicalTestElement>();

            // Retrieve all active MedicalTestElements for the selected company
            IList<MedicalTestElement> LabTestElements = MedicalTestManager.Instance.ListMedicalTestElementByCompanyId(Global.Company.CompanyId);

            // Ensure there are MedicalTestElements to load
            if (LabTestElements != null && LabTestElements.Count > 0)
            {
                // Iterate over the MedicalTestElements and add them to the TreeView
                foreach (var testElement in LabTestElements)
                {
                    string testElementName = testElement.Name;
                    string filterString = TextBoxLabTestElementSearch.Text.Trim();

                    // Apply the filter if there's a search query, otherwise add all
                    if (string.IsNullOrEmpty(filterString) || testElementName.IndexOf(filterString, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        TreeNode treeNode = new TreeNode
                        {
                            Text = testElementName,
                            Name = testElement.Id.ToString()
                        };

                        // Add the MedicalTestElement node to the TreeView
                        TreeViewLabTestElement.Nodes.Add(treeNode);
                    }
                }
            }

            // Optionally, populate the TreeViewSelectedLabTest based on some other conditions
            // Or keep it empty if it's not needed in this method
        }

    }
}
