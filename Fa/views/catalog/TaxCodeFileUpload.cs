using Cairo;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using ExcelDataReader;
using fa;
using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.Hms.Master;
using fa.views.catalog;
using fa.views.controls;
using fa.views.hms.masters;
using Fa.api.catalog;
using Fa.api.Hms;
using FADataAccessLibrary.Model.Common;
using Google.Protobuf;
using Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Libs.DirectShowLib;
using VisioForge.Libs.ZXing;

namespace Fa.views.catalog
{
    public partial class TaxCodeFileUpload : Form
    {
        public bool FileUploadComplete;
        public bool PauseProcessing;
        public bool IsBreak = false;

        public TaxCodeFileUpload()
        {
            InitializeComponent();
        }
        private void ExitFileUploadProcess()
        {
            if (fileUpload1.WorkFlow == true)
            {
                fileUpload1.EnableExitButton(false);
                PauseProcessing = true;
            }
            else
            {
                this.Close();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F10))
            {
                ExitFileUploadProcess();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void fileUpload1_OnClickUpload(object sender, EventArgs e)
        {
            string FileName = fileUpload1.FileName();
            bool HasHeader = fileUpload1.HasHeader();
            bool IsColumnValid = true;
            string[] TaxCodeHeader = new string[] { "Schedule", "Sl. No.", "TaxCode", "Description", "IGST", "CGST","SGST / UGST", "EffectiveFromDate", "EffectiveToDate" };
            Stopwatch stopw = new Stopwatch();
            using (var stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                   FileUploadComplete = true;
                    int Totalrowcount = reader.RowCount;
                    string Description = "";                    
                    int taxcoloumn = 0;                   
                    if (Global.Company.SalesTaxAccountMaps.Count > 0)
                    {
                        taxcoloumn = Global.Company.SalesTaxAccountMaps.Count;
                    }
                    float[] TaxfloatArray = new float[taxcoloumn];
                    stopw.Start();
                    fileUpload1.SetProgressMax(Totalrowcount);
                    int row = 1;
                    do
                    {
                        while (reader.Read())
                        {
                            if (PauseProcessing == true)
                            {
                                fileUpload1.AddAccessLog("Paused");
                                string message = "Do you want to exit from FileUpload Processing?";
                                string title = "Exit FileUpload";
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    fileUpload1.StopProcessing = true;
                                    this.Invoke((new Action(() => this.Close())));
                                }
                                else
                                {
                                    fileUpload1.AddAccessLog("Continuing");
                                    PauseProcessing = false;
                                    fileUpload1.StopProcessing = false;
                                    fileUpload1.EnableExitButton(true);
                                    continue;
                                }
                            }
                            if (fileUpload1.StopProcessing == false)
                            {                                
                                string TaxCode = "";
                                int fc = 0;
                                try
                                {
                                    if (TaxCodeHeader.Length == reader.FieldCount)
                                    {
                                        if (row == 1 && HasHeader)
                                        {
                                            foreach (string sh in TaxCodeHeader)
                                            {
                                                if (sh != reader.GetValue(fc).ToString())
                                                {
                                                    IsColumnValid = false;
                                                }
                                                fc++;
                                            }
                                            row++;
                                            continue;
                                        }
                                        if (IsColumnValid)
                                        {
                                            fileUpload1.IncrementProgress();
                                            TaxCode = reader.GetValue(2) != null ? reader.GetValue(2).ToString().Trim() : string.Empty;
                                            if (reader.GetValue(3) != null)
                                            {
                                                Description = reader.GetString(3);
                                            }

                                            int taxcoloumnStart = 0;

                                            for (int taxpercentagecount = 0; taxpercentagecount < taxcoloumn - 1; taxpercentagecount++)
                                            {
                                                int progress = (int)Math.Ceiling((decimal)reader.Depth / (decimal)reader.RowCount * (decimal)100);
                                                if (reader.GetValue(taxcoloumnStart + 4) != null)
                                                {
                                                    var TaxPercentages = reader.GetValue(taxcoloumnStart + 4);
                                                    var formattedValueTaxPercentages = (double)TaxPercentages; // * 100;
                                                    TaxfloatArray[taxpercentagecount] = (float)(double)formattedValueTaxPercentages;
                                                }
                                                taxcoloumnStart++;
                                            }
                                            var EFrom = reader != null && reader.GetValue(7) != null ? reader.GetValue(7).ToString().Trim() : string.Empty;
                                            var ETo = reader != null && reader.GetValue(8) != null ? reader.GetValue(8).ToString().Trim() : string.Empty;
                                            DateTime EffectiveStartDate = new DateTime(2017, 07, 01);
                                            DateTime EffectiveEndDate = new DateTime(2400, 12, 31);
                                            if (EFrom != null && DateTime.TryParse(EFrom.ToString(), out DateTime ESDresult))
                                            {
                                                var dateTime = DateUtils.ToDate(ESDresult.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern), CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);
                                                if (dateTime != null)
                                                {
                                                    EffectiveStartDate = (DateTime)dateTime;
                                                }
                                            }
                                            if (ETo != null && DateTime.TryParse(ETo.ToString(), out DateTime EEDresult))
                                            {
                                                var dateTime = DateUtils.ToDate(EEDresult.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern), CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);
                                                if (dateTime != null)
                                                {
                                                    EffectiveEndDate = (DateTime)dateTime;
                                                }
                                            }
                                            string formattedEFromDate = EffectiveStartDate.ToString(Global.Company.DateFormat);
                                            string formattedEToDate = EffectiveEndDate.ToString(Global.Company.DateFormat);
                                                                            
                                            bool newRow = false;
                                            if (!string.IsNullOrEmpty(TaxCode))
                                            {
                                                string TaxCodes = TaxCode;
                                                string[] stringSeparators = new string[] { "AND", "and", "or", "OR", "to", "To", " ", ",", ";", "&&", "&", "-", "_", ".", "{", "}", "[", "]", "@", "!", "$", "%", "^", "*", "+", "=" };
                                                string[] TaxCodeValues = TaxCodes.Split(stringSeparators, StringSplitOptions.None);
                                                double TaxCodeval = 0;
                                                for (int i = 0; i < TaxCodeValues.Length; i++)
                                                {
                                                    TaxCodeValues[i] = TaxCodeValues[i].Trim();
                                                    bool checkResult = int.TryParse(TaxCodeValues[i], out int TaxCheck);
                                                    if (checkResult == true)
                                                    {
                                                        if (TaxCodeValues[i] != "" || TaxCodeValues[i] != string.Empty)
                                                        {
                                                            TaxCode = TaxCodeValues[i];
                                                            ItemTax TC = new ItemTax();
                                                            TC.Id = 0L;
                                                            TC.CompanyId = Global.Company.CompanyId;
                                                            TC.Code = TaxCode;
                                                            TC.Description = Description;
                                                            List<CompanySalesTaxAccountMap> MapTax = Global.Company.SalesTaxAccountMaps.ToList();
                                                            if (MapTax.Count > 0)
                                                            {
                                                                List<CountrySaleTax> MapCountryTax = CountryManager.Instance.ListCountryTaxByCountryId((long)Global.Company.CountryId);
                                                                if (MapCountryTax.Count > 0)
                                                                {
                                                                    if (MapCountryTax.Any())
                                                                    {
                                                                        int TotalColoumn = MapCountryTax.Count;
                                                                        for (int ColoumnCount = 0; ColoumnCount < TotalColoumn; ColoumnCount++)
                                                                        {
                                                                            double percentagenumber = 0;
                                                                            ItemSalesTaxMap ItemSalesTaxMap = new ItemSalesTaxMap();
                                                                            CompanySalesTaxAccountMap companySalesTaxAccountMap = MapTax.FirstOrDefault(x => x.CountrySaleTaxId == MapCountryTax[ColoumnCount].Id);
                                                                            if (companySalesTaxAccountMap != null)
                                                                            {
                                                                                ItemSalesTaxMap.SalesTaxMapId = companySalesTaxAccountMap.MapId;
                                                                                if (companySalesTaxAccountMap.CountrySaleTaxId == 1)
                                                                                {
                                                                                    ItemSalesTaxMap.TaxPercentage = (float)(double)TaxfloatArray[ColoumnCount];
                                                                                }
                                                                                if (companySalesTaxAccountMap.CountrySaleTaxId == 2)
                                                                                {
                                                                                    ItemSalesTaxMap.TaxPercentage = (float)(double)TaxfloatArray[ColoumnCount];
                                                                                }
                                                                                if (companySalesTaxAccountMap.CountrySaleTaxId == 3)
                                                                                {
                                                                                    ItemSalesTaxMap.TaxPercentage = (float)(double)TaxfloatArray[ColoumnCount];
                                                                                }
                                                                                else if (companySalesTaxAccountMap.CountrySaleTaxId > 3)
                                                                                {
                                                                                    ItemSalesTaxMap.TaxPercentage = (float)(double)TaxfloatArray[ColoumnCount];
                                                                                }
                                                                                string format = Global.Company.DateFormat.ToString().Trim();
                                                                                bool CheckforDate = DateUtils.TryParseDate(formattedEFromDate, out DateTime EFNewdate, format);
                                                                                if (CheckforDate)
                                                                                {
                                                                                    ItemSalesTaxMap.EffectiveFromDate = DateTime.Parse(EffectiveStartDate.ToString());
                                                                                }
                                                                                else
                                                                                {
                                                                                    ItemSalesTaxMap.EffectiveFromDate = EFNewdate;
                                                                                }

                                                                                CheckforDate = DateUtils.TryParseDate(formattedEToDate, out DateTime ETNewdate, format);
                                                                                if (CheckforDate)
                                                                                {
                                                                                    ItemSalesTaxMap.EffectiveToDate = DateTime.Parse(EffectiveEndDate.ToString());
                                                                                }
                                                                                else
                                                                                {
                                                                                    ItemSalesTaxMap.EffectiveToDate = ETNewdate;
                                                                                }
                                                                                TC.SalesTaxMapLocal.Add(ItemSalesTaxMap);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (TC.Id == 0)
                                                            {
                                                                ItemTax TCById = ItemTaxManager.Instance.GetItemTaxCodeByCode(TC.Code, Global.Company.CompanyId);
                                                                if (TCById == null)
                                                                {

                                                                    ItemTaxManager.Instance.AddItemTax(TC);
                                                                }
                                                                else
                                                                {
                                                                    TC.Id = TCById.Id;
                                                                    TCById = ItemTaxManager.Instance.UpdateItemTaxFromExcel(TC);
                                                                }
                                                            }
                                                            if (HasHeader)
                                                            {
                                                                fileUpload1.AddAccessLog("Processing Row No : " + (row - 1) + " - " + Name);
                                                                fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                            }
                                                            else
                                                            {
                                                                fileUpload1.AddAccessLog("Processing Row No : " + row + " - " + Name);
                                                                fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        IsBreak = true;
                                                        fileUpload1.AddErrorLog("Row No - " + row + " Not a valued Tax Code");
                                                        fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (HasHeader)
                                                {
                                                    fileUpload1.AddErrorLog("Row No - " + (row - 1) + " Empty Name");
                                                    fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                }
                                                else
                                                {
                                                    fileUpload1.AddErrorLog("Row No - " + row + " Empty Name");
                                                    fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            FileUploadComplete = false;
                                            IsBreak = true;
                                            fileUpload1.AddAccessLog("Selected File for uploading is mismatched with coloumn header");
                                            Cursor.Current = Cursors.Default;
                                            this.UseWaitCursor = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        FileUploadComplete = false;
                                        IsBreak = true;
                                        fileUpload1.AddAccessLog("Selected File for uploading is mismatched with number of coloumn");
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;
                                        break;
                                    }
                                    
                                }
                                catch (Exception ex)
                                {
                                    if (ex.HResult.ToString() == "-2146233080")
                                    {
                                        FileUploadComplete = false;
                                        fileUpload1.AddAccessLog("Cancelled");
                                        Cursor.Current = Cursors.Default;
                                        throw new IndexOutOfRangeException("-2146233080", ex);
                                    }
                                    else
                                    {
                                        if (HasHeader)
                                        {
                                            if (ex.InnerException != null)
                                            {
                                                fileUpload1.AddErrorLog("Row No : " + (row - 1) + " - " + Name + " : " + ex.InnerException.Message);
                                            }
                                            else
                                            {
                                                fileUpload1.AddErrorLog("Row No : " + (row - 1) + " - " + Name + " : " + ex.Message);
                                            }
                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                        else
                                        {
                                            if (ex.InnerException != null)
                                            {
                                                fileUpload1.AddErrorLog("Row No : " + row + " - " + Name + " : " + ex.InnerException.Message);
                                            }
                                            else
                                            {
                                                fileUpload1.AddErrorLog("Row No : " + row + " - " + Name + " : " + ex.Message);
                                            }
                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                    }
                                }
                                row++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    } while (reader.NextResult());
                    if (IsBreak == false) { fileUpload1.CompleteIncrementProgress(row); } else { fileUpload1.CompleteIncrementProgress(0); }
                    fileUpload1.CompleteIncrementProgress(row);
                    fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                    stopw.Stop();
                    if (fileUpload1.StopProcessing == true)
                    {
                        fileUpload1.AddAccessLog("Cancelled");
                    }
                    else
                    {
                        fileUpload1.AddAccessLog("Finished");
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void fileUpload1_OnClickExit(object sender, EventArgs e)
        {
            ExitFileUploadProcess();
        }

        private void TaxCodeFileUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileUpload1.WorkFlow == true)
            {
                fileUpload1.ExitFileUpload();
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
