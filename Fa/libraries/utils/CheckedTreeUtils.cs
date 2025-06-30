using fa.api.accounting.doubleentry;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.UserProfile;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Common;
using fa.model.Employee;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.views.controls;
using fa.views.controls.ComboListView;
using fa.views.controls.ComboTreeView;
using Fa.api.Hms;
using Standard;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using VisioForge.Libs.NDI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace fa.libraries.utils
{
    class CheckedTreeUtils
    {
        public static void NodesCheckingStatus(ComboTreeNodeEventArgs Args, ComboTreeNodeCollection ComboBoxTreeNode)
        {
            bool IsChecked= Args.Node.Checked;
            foreach (ComboTreeNode node in ComboBoxTreeNode.Where(x=>x.Name!= Args.Node.Name))
            {
                //if (node.Name == Args.Node.Name && node.Text == Args.Node.Text)
                //{
                //    continue;
                //}
                node.Checked = IsChecked;
                //if (node.Nodes != null && node.Nodes.Count > 0)
                //{
                //    NodesCheckingStatus(Args, node.Nodes);
                //}
            }
        }

        public static bool AllNodeCheckingStatus(ComboTreeNodeEventArgs Args, ComboTreeNodeCollection ComboBoxTreeNode, bool AllNodeCheckingStatusInnodeCheck)
        {
            if (!Args.Node.Checked)
            {
                foreach (ComboTreeNode node in ComboBoxTreeNode)
                {
                    if (node.Text == "All")
                    {
                        if (node.Checked)
                        {
                            AllNodeCheckingStatusInnodeCheck = Args.Node.Checked;
                        }
                        break;
                    }
                    if (node.Nodes!=null && node.Nodes.Count > 0)
                    {
                        AllNodeCheckingStatus(Args, node.Nodes, AllNodeCheckingStatusInnodeCheck);
                    }
                }
            }
            else
            {
                AllNodeCheckingStatusInnodeCheck = true;
                foreach (ComboTreeNode node in ComboBoxTreeNode)
                {
                    if (!node.Checked && node.Text != "All")
                    {
                        AllNodeCheckingStatusInnodeCheck = false;
                        break;
                    }
                    if (node.Nodes.Count > 0)
                    {
                        AllNodeCheckingStatus(Args, node.Nodes, AllNodeCheckingStatusInnodeCheck);
                    }
                }
            }
            return AllNodeCheckingStatusInnodeCheck;
        }
        public static List<string> SelectedNameNodes(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            List<string> Names = new List<string>();

            if (ComboTreeBox.CheckedNodes != null && ComboTreeBox.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboTreeBox.CheckedNodes)
                {
                    if (node.Name == "All")
                    {
                        if (ComboTreeBox.Name == "ComboBoxAccounts")
                        {
                            Names.AddRange(AccountManager.Instance.GetAllAccountsByCompanyId(Global.Company.CompanyId).Select(x => x.Name.ToString()));
                        }
                        else if (ComboTreeBox.Name == "CheckedTreeComboLocation" || ComboTreeBox.Name == "ComboExpiryReportLocation" || ComboTreeBox.Name == "ComboStockReportLocation")
                        {
                            Names.AddRange(HospitalInventoryManager.Instance.ListAllInventoryLocation(Global.Company.CompanyId).Select(x => x.Name.ToString()));
                        }
                        else if (ComboTreeBox.Name == "CheckedComboBoxFeeType")
                        {
                            Names.AddRange(ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId).Select(x => x.Name.ToString()));
                        }
                        else if (ComboTreeBox.Name == "CheckedTreeComboBoxArea")
                        {
                            Names.AddRange(AddressManager.Instance.ListAddress().ToArray<string>());
                        }
                        else if (ComboTreeBox.Name == "ComboBoxProductFamily")
                        {
                            Names.AddRange(CatalogProductFamilyManager.Instance.ListProductFamilyByCompanyId(Global.Company.CompanyId).Select(x => x.Name.ToString()));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxCustomerforReturn")
                        {
                            Names.AddRange(SalesManager.Instance.GetCustomerFromSaleEntry(Global.Company.CompanyId).ToArray<string>());
                        }
                        else if (ComboTreeBox.Name == "ComboBoxCategory" || ComboTreeBox.Name == "ComboBoxManufacturer" || ComboTreeBox.Name == "ComboBoxSupplier" || ComboTreeBox.Name == "ComboBoxRack")
                        {
                            Names.AddRange(CatalogProductFamilyManager.Instance.ListProductCategoryByCompanyId(Global.Company.CompanyId).Select(x => x.Name.ToString()));
                        }
                        else if (ComboTreeBox.Name == "CheckedTreeComboBoxType")
                        {
                            foreach (ComboTreeNode nodes in ComboTreeBox.CheckedNodes)
                            {
                                Names.Add(nodes.Text);
                            }
                        }
                        else if (ComboTreeBox.Name == "CheckedTreeComboBoxConsultations")
                        {
                            Names.AddRange(ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId).Select(x => x.Name.ToString()));
                            IList<MedicalProcedure> procedures = MedicalProcedureManager.Instance.ListMedicalProcedureByCompanyId(Global.Company.CompanyId);
                            if (procedures.Count > 0) { Names.Add("Procedure Fee"); }
                        }
                        break;
                    }
                    Names.Add(node.Text);
                }
            }
            return Names;
        }

        public static List<long> SelectedNodes(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            int i = 0;
            List<long> Ids = new List<long>();
            if (ComboTreeBox.CheckedNodes!=null && ComboTreeBox.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboTreeBox.CheckedNodes)
                {
                    if (node.Name == "All")
                    {
                        if (ComboTreeBox.Name == "ComboBoxAccounts")
                        {
                            Ids.AddRange((AccountManager.Instance.GetAllAccountsByCompanyId(Global.Company.CompanyId).ToList<Account>()).Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "AdjustmentEntryComboxLocation" ||ComboTreeBox.Name == "CheckedTreeComboLocation" || ComboTreeBox.Name == "ComboExpiryReportLocation" || ComboTreeBox.Name == "ComboStockReportLocation" || ComboTreeBox.Name == "CheckedTreeComboBoxLocation")
                        {
                            Ids.AddRange((HospitalInventoryManager.Instance.ListAllInventoryLocation(Global.Company.CompanyId).ToArray<InventoryLocation>()).Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxItem")
                        {
                            Ids.AddRange(CatalogItemManager.Instance.ListItemsByCompanyId(Global.Company.CompanyId).ToArray<CatalogItem>().Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxVendors")
                        {
                            Ids.AddRange(AccountHelper.GetHelpData(0, true, true, false, false, "", Global.Company).ToArray<AccountHelperData>().Select(x => x.Id));
                        }
                        else if ( ComboTreeBox.Name == "CheckedComboBoxFeeType")
                        {
                            Ids.AddRange(ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId).Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxProductFamily")
                        {
                           Ids.AddRange(CatalogProductFamilyManager.Instance.ListProductFamilyByCompanyId(Global.Company.CompanyId).ToList<ProductFamily>().Select(x => x.Id));
                        }
                        else if(ComboTreeBox.Name == "ComboBoxCategory" || ComboTreeBox.Name == "ComboBoxManufacturer" || ComboTreeBox.Name == "ComboBoxSupplier" || ComboTreeBox.Name == "ComboBoxRack")
                        {
                            Ids.AddRange(CatalogProductFamilyManager.Instance.ListProductCategoryByCompanyId(Global.Company.CompanyId).ToList<CatalogItem>().Select(x => x.Id));
                        }
                        else if(ComboTreeBox.Name == "ComboBoxCustomer")
                        {
                            Ids.AddRange(CustomerManager.Instance.GetAllCustomer(Global.Company.CompanyId).ToList<Customer>().Select(x => x.Id));
                        }
                        else if(ComboTreeBox.Name == "TreeComboBoxSupplier")
                        {
                            Ids.AddRange(SupplierManager.Instance.ListSupplierByCompanyId(Global.Company.CompanyId).ToList<Supplier>().Select(x => x.Id));
                            Ids.AddRange(CustomerManager.Instance.ListCustomerByCompanyId(Global.Company.CompanyId).ToList<Customer>().Select(x => x.Id));
                        }
                        else if(ComboTreeBox.Name == "ComboBoxReferer")
                        {
                            Ids.AddRange(ReferedManager.Instance.ListAllRefered(Global.Company.CompanyId).ToList<Refered>().Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "CheckedTreeComboBoxPatient")
                        {
                            Ids.AddRange(PatientManager.Instance.ListAllPatient(Global.Company.CompanyId).Select(x => x.Id));
                        }    
                        else if (ComboTreeBox.Name == "CheckedTreeComboBoxConsulted" || ComboTreeBox.Name == "CheckedTreeComboBoxConsultant" || ComboTreeBox.Name == "CheckedComboBoxUser")
                        {
                            Ids.AddRange(UserManager.Instance.ListAllUserForCounslting(Global.Company.BusinessType).Select(x => x.UserId));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxConsultant" || ComboTreeBox.Name == "ComboBoxDoctor")
                        {
                            Ids.AddRange(EmployeeManager.Instance.ListEmployeeByCompanyIdTitle(Global.Company.CompanyId, "Doctor").Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxNurse" )
                        {
                            Ids.AddRange(EmployeeManager.Instance.ListEmployeeByCompanyIdTitle(Global.Company.CompanyId, "Nurse").Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxTechnician")
                        {
                            Ids.AddRange(EmployeeManager.Instance.ListEmployeeByCompanyIdTitle(Global.Company.CompanyId, "Technician").Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxWard" || ComboTreeBox.Name == "CheckedTreeComboBoxWard")
                        {
                            Ids.AddRange(WardManager.Instance.ListWardByCompanyId(Global.Company.CompanyId).Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxInsurance")
                        {
                            Ids.AddRange(InsuranceInfoManager.Instance.ListAllActiveInsuranceInfoByCompanyId(Global.Company.CompanyId).Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "ComboBoxDepartment" || ComboTreeBox.Name == "CheckedTreeComboBoxDept")
                        {
                            Ids.AddRange(DepartmentManager.Instance.ListDepartmentByCompanyId(Global.Company.CompanyId).Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "CheckedTreeComboBoxLabTestName")
                        {
                            Ids.AddRange(MedicalTestManager.Instance.GetMedicalTestsByCompanyId(Global.Company.CompanyId).Select(x => x.Id));
                        }
                        else if (ComboTreeBox.Name == "CheckedTreeComboBoxDiag")
                        {
                            Ids.AddRange(SymptomsManager.Instance.ListSymptomByCompanyId(Global.Company.CompanyId).Select(x => x.Id));
                        }
                        break;
                    }                    
                    Ids.Add(long.Parse(node.Name));
                }
            }
            return Ids;
        }
        public static List<long> SelectedItems(ToolstripCheckedComboBox ComboBox)
        {
            int i = 0;
            List<long> Ids = new List<long>();
            if (ComboBox.CheckedItems.Count > 0)
            {
                foreach (CCBoxItem Item in ComboBox.CheckedItems)
                {                   
                    Ids.Add(Item.Value);
                }
            }
            return Ids;
        }

    }
}
