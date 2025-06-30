using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using fa.model.Hms.Master;
using Fa.api.Hms;
using Microsoft.Office.Interop.Excel;

namespace fa.views.hms.masters.upload
{
    public class PrcedureUploadProcessor : UploadProcessor
    {
        
        public void ProcessData(string FileName, bool HasHeader)
        {
            using (var stream = File.Open(FileName, FileMode.Open))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    int row = 1;
                    do
                    {
                        while (reader.Read())
                        {
                            string Name="";
                            try
                            {
                                if (row == 1 && HasHeader)
                                {
                                    row++;
                                    continue;
                                }
                                    
                                Name = reader.GetString(0);
                                string Description = reader.GetString(1);
                                string DisplayName = reader.GetString(2);
                                bool IsActive = reader.GetBoolean(3);
                                string Reason = reader.GetString(4);
                                double Fee = reader.GetDouble(5);
                                string KeywordsInString = reader.GetString(6);


                                MedicalProcedure MP = new model.Hms.Master.MedicalProcedure
                                {
                                    CompanyId = Global.Company.CompanyId,
                                    Name = Name,
                                    Description = Description,
                                    DisplayAs = DisplayName,
                                    IsActive = IsActive,
                                    Reason = Reason,
                                    Fee = Fee
                                };

                                if (MedicalProcedureManager.Instance.MedicalProcedureNameUniqueById(MP))
                                {
                                    if (!String.IsNullOrEmpty(KeywordsInString))
                                    {
                                        String[] KeywordsArray = KeywordsInString.Split(' ');
                                        foreach (string keyword in KeywordsArray)
                                        {
                                            if (String.IsNullOrEmpty(keyword))
                                            {
                                                continue;
                                            }
                                            MedicalProcedureKeyword keyWord = new MedicalProcedureKeyword
                                            {
                                                Text = keyword,
                                                CompanyId = Global.Company.CompanyId,
                                                MedicalProcedure = MP
                                            };
                                            if (MP.Keywords == null)
                                            {
                                                MP.Keywords = new List<MedicalProcedureKeyword>();
                                            }
                                            MP.Keywords.Add(keyWord);
                                        }
                                    } 
                                     MedicalProcedure MPById = MedicalProcedureManager.Instance.AddMedicalProcedure(MP);
                                }
                                else
                                {

                                }

                                //AccessLog += "\nProcedure : " + Name;
                                //Console.WriteLine("\nProcedure : " + Name);
                            }
                            catch(Exception e)
                            {
                                //fileUplod1.("Procedure : " + Name + ":" + e.InnerException.Message);                                
                            }
                            row++;
                        }                        
                    } while (reader.NextResult());
                }
            }
        }
    }
}
