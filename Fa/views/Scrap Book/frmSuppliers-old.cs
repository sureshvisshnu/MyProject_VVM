using fa.libraries.utils;
using fa.context;
using fa.model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Accounting.Masters;


namespace fa.views.account.masters
{
    public partial class frmSuppliers : Form
    {
        private List<String> keys = new List<String>();
        public frmSuppliers()
        {
            InitializeComponent();
        }

        private void listBoxSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillForm();
        }

        private void loadSuppliers()
        {
            listBoxSuppliers.Items.Clear();
            using (AccountMasterContext context = new AccountMasterContext())
            {
                IList Suppliers = context.Companies.ToList();
                foreach (Supplier Supplier in Suppliers)
                {
                    listBoxSuppliers.Items.Add(Supplier);
                }

                if (listBoxSuppliers.Items.Count > 0)
                {
                    listBoxSuppliers.SelectedIndex = 0;
                }
            }
        }

        private void fillForm()
        {
            int index = listBoxSuppliers.SelectedIndex;
            resetForm();
            Supplier Supp = (Supplier)listBoxSuppliers.Items[index];
            if (Supp != null)
            {
                using (AccountMasterContext context = new AccountMasterContext())
                {
                    Supplier Supp1 = null; // context.Suppliers.Find(Supp.SupplierId);
                    textBoxAccountName.Text = Supp1.Name;
                    textBoxDisplayName.Text = Supp1.DisplayAs;
                    //textBoxSupplierId.Text = Supp1.SupplierId;
                    ////textBoxAddressId.Text = Supp1.AddressId;
                    //textBoxFax.Text = Supp1.Fax;
                    //textBoxPhone.Text = Supp1.Phone;
                    //textBoxMobile.Text = Supp1.Mobile;
                    //textBoxOpenBal.Text = System.Convert.ToString(Supp1.OpeningBalance);
                    //textBoxAccountNo.Text = Supp1.AccountNumber;
                    //dateTimePickerBalanceAsOf.Text = System.Convert.ToString(Supp1.OpeningBalanceAsOf);
                    //String AddressId = Supp1.AddressId;
                    /*
                    if(String.IsNullOrEmpty(AddressId))
                    {
                        Address SupplierAddress = context.Addresses.Find(AddressId);
                        if (SupplierAddress != null)
                        {
                            textBoxAddress1.Text = SupplierAddress.Address1;
                            textBoxAddress2.Text = SupplierAddress.Address2;
                            textBoxCity.Text = SupplierAddress.CityOrTown;
                            comboBoxState.SelectedValue = SupplierAddress.State;
                            textBoxDisctrict.Text = SupplierAddress.District;
                            textBoxPincode.Text = SupplierAddress.pinCode;
                        }
                    }*/
                }
            }
        }

        private void lblAccountName_Click(object sender, EventArgs e)
        {

        }

        private void btnExitSuppliers_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnSupplierCancel_Click(object sender, EventArgs e)
        {
            enableForm(false);
            fillForm();
            btnSupplierEdit.Enabled = true;
            if(btnNewSuppliers.Enabled==false)
            {
                btnNewSuppliers.Enabled = true;
                btnDeleteSuppliers.Enabled = true;
            }
        }

        private void btnSupplierEdit_Click(object sender, EventArgs e)
        {
            if(listBoxSuppliers.SelectedIndex>=0)
            {
                enableForm(true);
                textBoxAccountName.Focus();
                btnSupplierEdit.Enabled = false;
            }
        }

        private void dateTimePickerBalanceAsOf_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dateTimePickerBalanceAsOf_Enter(object sender, EventArgs e)
        {
            dateTimePickerBalanceAsOf.Show();
        }

        private void frmSuppliers_Load(object sender, EventArgs e)
        {
            loadSuppliers();
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void enableForm(Boolean enable)
        {
            textBoxAccountName.Enabled = enable;
            textBoxDisplayName.Enabled = enable;
            textBoxAddress1.Enabled = enable;
            textBoxAddress2.Enabled = enable;
            textBoxCity.Enabled = enable;
            comboBoxState.Enabled = enable;
            textBoxDisctrict.Enabled = enable;
            textBoxPincode.Enabled = enable;
            textBoxFax.Enabled = enable;
            textBoxPhone.Enabled = enable;
            textBoxMobile.Enabled = enable;
            textBoxOpenBal.Enabled = enable;
            textBoxAccountNo.Enabled = enable;
            dateTimePickerBalanceAsOf.Enabled = enable;
            maskedTextBoxOpenBalAsOf.Enabled = enable;
            btnSupplierSave.Enabled = enable;
            btnSupplierCancel.Enabled = enable;
            listBoxSuppliers.Enabled = !enable;
            txtBoxSupplierSearch.Enabled = !enable;
        }

        private void btnNewSuppliers_Click(object sender, EventArgs e)
        {
            btnNewSuppliers.Enabled = false;
            btnDeleteSuppliers.Enabled = false;
            newForm();
        }

        private void newForm()
        {
            btnSupplierEdit.Enabled = false;
            enableForm(true);
            resetForm();
            dateTimePickerBalanceAsOf.ResetText();
        }

        /// <summary>
        /// Reseting the current for for enteringa new value
        /// </summary>
        private void resetForm()
        {
            textBoxAccountName.ResetText();
            textBoxDisplayName.ResetText();
            textBoxAddress1.ResetText();
            textBoxAddress2.ResetText();
            textBoxCity.ResetText();
            comboBoxState.ResetText();
            textBoxDisctrict.ResetText();
            textBoxPincode.ResetText();
            textBoxFax.ResetText();
            textBoxPhone.ResetText();
            textBoxMobile.ResetText();
            textBoxOpenBal.ResetText();
            dateTimePickerBalanceAsOf.ResetText();
            textBoxAccountNo.ResetText();
        }

        private void btnSupplierSave_Click(object sender, EventArgs e)
        {
            Supplier SupplierToSave = getSupplierFromForm();
            if(groupBoxSuuplierInfo !=null)
            {
                using (AccountMasterContext context = new AccountMasterContext())
                {
            //        if (String.IsNullOrEmpty(SupplierToSave.SupplierId))
            //        {
            //            try
            //            {
            //                //New Supplier
            //               // SupplierToSave.SupplierId = IDGenerator.generateID("SP");
            //               // SupplierToSave.Address.AddressId = IDGenerator.generateID("AD");
            //                //SupplierToSave.AddressId = SupplierToSave.Address.AddressId;
            //                //context.Suppliers.Add(SupplierToSave);
            //                context.Addresses.Add(SupplierToSave.Address);
            //                context.SaveChanges();
            //            }
            //            catch(Exception)
            //            {
            //                MessageBox.Show("Error adding a new supplier!", "Error", MessageBoxButtons.OK);
            //            }

            //        }
            //        else
            //        {
            //            //updating Supplier
            //            //var ExistingSupplier = context.Companies.Find(SupplierToSave.SupplierId);
            //            //if(ExistingSupplier!=null)
            //            //{
            //            //    //context.Entry(ExistingSupplier).CurrentValues.SetValues(SupplierToSave);
            //            //    //context.SaveChanges();
            //            //    context.Companies.Remove(ExistingSupplier);

            //            //}



            //            //update Supplier
            //            /*
            //            try
            //            {
            //                var ExistingSupplier = context.Suppliers.Find(SupplierToSave.SupplierId);
            //                if(ExistingSupplier != null)
            //                {
            //                    ExistingSupplier.SupplierId = SupplierToSave.SupplierId;
            //                    ExistingSupplier.Name = SupplierToSave.Name;
            //                    ExistingSupplier.DisplayAs = SupplierToSave.DisplayAs;
            //                    ExistingSupplier.Fax = SupplierToSave.Fax;
            //                    ExistingSupplier.Phone = SupplierToSave.Phone;
            //                    ExistingSupplier.Mobile = SupplierToSave.Mobile;
            //                    ExistingSupplier.OpeningBalance = SupplierToSave.OpeningBalance;
            //                    ExistingSupplier.AccountNumber = SupplierToSave.AccountNumber;
            //                    ExistingSupplier.OpeningBalanceAsOf = SupplierToSave.OpeningBalanceAsOf;
            //                    ExistingSupplier.AddressId = SupplierToSave.AddressId;
            //                    Boolean AddressFound = false;
            //                    if (!String.IsNullOrEmpty(ExistingSupplier.AddressId))
            //                    {
            //                        Address ExistingSupplierAddress = context.Addresses.Find(SupplierToSave.AddressId);
            //                        if(ExistingSupplierAddress != null)
            //                        {
            //                            AddressFound = true;
            //                            ExistingSupplierAddress.Address1 = SupplierToSave.Address.Address1;
            //                            ExistingSupplierAddress.Address2 = SupplierToSave.Address.Address2;
            //                            ExistingSupplierAddress.CityOrTown = SupplierToSave.Address.CityOrTown;
            //                            ExistingSupplierAddress.State = SupplierToSave.Address.State;
            //                            ExistingSupplierAddress.District = SupplierToSave.Address.District;
            //                            ExistingSupplierAddress.pinCode = SupplierToSave.Address.pinCode;
            //                        }
            //                    }

            //                    if(!AddressFound)
            //                    {
            //                        Address NewAddress = new Address();
            //                        NewAddress.AddressId = IDGenerator.generateID("AD");
            //                        NewAddress.Address1 = SupplierToSave.Address.Address1;
            //                        NewAddress.Address2 = SupplierToSave.Address.Address2;
            //                        NewAddress.CityOrTown = SupplierToSave.Address.CityOrTown;
            //                        NewAddress.State = SupplierToSave.Address.State;
            //                        NewAddress.District = SupplierToSave.Address.District;
            //                        NewAddress.pinCode = SupplierToSave.Address.pinCode;
            //                        context.Addresses.Add(NewAddress);
            //                        ExistingSupplier.AddressId = NewAddress.AddressId;
            //                        ExistingSupplier.Address = NewAddress;
            //                    }
            //                    context.SaveChanges();
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex.Message);
            //                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            //            }*/
                  }

                }
                

            //}
            //enableForm(false);
            //loadSuppliers();
            //if (btnNewSuppliers.Enabled)
            //{
            //    btnNewSuppliers.Enabled = true;
            //    btnDeleteSuppliers.Enabled = true;
            //    btnSupplierEdit.Enabled = true;
            //}
            //else
            //{
            //    btnSupplierEdit.Enabled = true;
            //}

        }

        private void DeleteSupplier()
        {

        }

        private void UpdateSupplier()
        {
            Supplier SupplierToUpdate = new Supplier();
        }

        private Supplier getSupplierFromForm()
        {
            Supplier SupplierToUpdate = new Supplier();
           // SupplierToUpdate.SupplierId = textBoxSupplierId.Text;
            SupplierToUpdate.Name = textBoxAccountName.Text;
            SupplierToUpdate.DisplayAs = textBoxDisplayName.Text;
            ///SupplierToUpdate.Fax = textBoxFax.Text;
           // SupplierToUpdate.Phone = textBoxPhone.Text;
            //SupplierToUpdate.Mobile = textBoxMobile.Text;
            try
            {
                //SupplierToUpdate.OpeningBalance = Convert.ToDouble(textBoxOpenBal.Text);
            }
            catch(FormatException)
            {
                MessageBox.Show("Invalida Opening Balance, Please correct it", "Error", MessageBoxButtons.OK);
                return null;
            }
            //SupplierToUpdate.AccountNumber= textBoxAccountNo.Text;
            try
            {
              //  SupplierToUpdate.OpeningBalanceAsOf = Convert.ToDateTime(dateTimePickerBalanceAsOf.Text);
            }
            catch(FormatException)
            {
                MessageBox.Show("Invalida Date of Opening Balance as Of", "Error", MessageBoxButtons.OK);
                return null;
            }
            Address SupplierAddressToUpdate = new Address();
            //SupplierAddressToUpdate.AddressId = textBoxAddressId.Text;
            //SupplierAddressToUpdate.Address1 = textBoxAddress1.Text;
            //SupplierAddressToUpdate.Address2 = textBoxAddress2.Text;
            SupplierAddressToUpdate.CityOrTown = textBoxCity.Text;
            if(comboBoxState.SelectedIndex>-1)
            {
                //SupplierAddressToUpdate.State = (String)comboBoxState.Items[comboBoxState.SelectedIndex];
            }            
            SupplierAddressToUpdate.District = textBoxDisctrict.Text;
            //SupplierAddressToUpdate.pinCode = textBoxPincode.Text;
            return SupplierToUpdate;
        }

        private void groupBoxSuuplierInfo_Enter(object sender, EventArgs e)
        {

        }
    }

    //public class Supplier
    //{
        
    //    public String SupplierId { set; get; }
    //    public String Name { set; get; }
    //    public String DisplayAs { set; get; }
    //    public Address Address { set; get; }
    //    public double OpeningBalance { set; get; }
    //    public DateTime OpeningBalanceAsOf { set; get; }
    //    public String AccountNumber { set; get; }
    //    public String Phone { set; get; }
    //    public String Mobile { set; get; }
    //    public String Fax { set; get; }
    //    public override string ToString()
    //    {
    //        return string.Format("{0}", Name);
    //    }
    //}

}
