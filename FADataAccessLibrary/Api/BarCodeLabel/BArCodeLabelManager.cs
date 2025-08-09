using fa.context;
using fa.model.Catalog;
using FADataAccessLibrary.Model.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.BarCodeLabel
{
    public class BarCodeLabelManager
    {
        private static BarCodeLabelManager _instance;
        private static object syncRoot = new Object();
        public static BarCodeLabelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BarCodeLabelManager();
                }
                return _instance;
            }
        }
        BarCodeLabelManager()
        {
            // Initialize any resources or settings here if needed
        }
        // Add methods to manage bar code labels, e.g., create, update, delete, retrieve labels

        public LabelStockMaster AddBarCodeLabel(LabelStockMaster labelStock)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Ensure no related entities cause unintended inserts
                        context.LabelStockMasters.Add(labelStock);
                        context.SaveChanges();

                        transaction.Commit();
                        return labelStock; // return the saved entity
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }

}
