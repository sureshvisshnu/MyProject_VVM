using fa.context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using Microsoft.EntityFrameworkCore;
using fa.api.OrderManagement;
using fa.model.OrderManagement;
using Fa.model.Purchase;
using DocumentFormat.OpenXml.Presentation;
using System.Diagnostics;

namespace Fa.views.hms.helper
{
    public partial class FormDataModification : Form
    {
        public FormDataModification()
        {
            InitializeComponent();
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            ExecuteDeleteStatements();
        }

        private void FormDataModification_Load(object sender, EventArgs e)
        {
            DisplayDate();
        }

        private void DisplayDate()
        {
            DateTime date = DateTime.Now;
            LblDateToday.Text = date.ToString();
        }

        //private void ExecuteDeleteStatements()
        //{
        //    string startDate = "2023-04-01 00:00:00";
        //    string endDate = "2024-03-31 23:59:59";

        //    try
        //    {
        //        Cursor.Current = Cursors.WaitCursor;
        //        using (AccountMasterContext context = new AccountMasterContext())
        //        {
        //            string connectionString = context.Database.GetDbConnection().ConnectionString;

        //            string tableQuery = $"SELECT table_name " +
        //                                "FROM information_schema.tables " +
        //                                "WHERE table_schema = 'vv-matrix' AND table_type = 'BASE TABLE';";

        //            using (MySqlConnection tableConnection = new MySqlConnection(connectionString))
        //            {
        //                tableConnection.Open();

        //                using (MySqlCommand tableCommand = new MySqlCommand(tableQuery, tableConnection))
        //                {
        //                    using (MySqlDataReader tableReader = tableCommand.ExecuteReader())
        //                    {
        //                        while (tableReader.Read())
        //                        {
        //                            string tableName = tableReader["table_name"].ToString()!;
        //                            if (CheckColumnExists(connectionString, tableName, "CreatedDate"))
        //                            {
        //                                string deleteStatement = $"DELETE FROM {tableName} WHERE CreatedDate < '{startDate}' OR CreatedDate >= '{endDate}';";
        //                                ExecuteNonQueryWithForeignKeyCheck(deleteStatement, connectionString);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        MessageBox.Show("Delete statements executed successfully.");
        //        Cursor.Current = Cursors.Default;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred: {ex.Message}");
        //        Cursor.Current = Cursors.Default;
        //    }
        //}

        // Second Set of Delete Data
        /*
        private void ExecuteDeleteStatements()
        {
            string startDate = "2023-04-01 00:00:00";
            string endDate = "2024-03-31 23:59:59";

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                using (AccountMasterContext context = new AccountMasterContext())
                {
                    string connectionString = context.Database.GetDbConnection().ConnectionString;

                    // First connection to get the list of tables
                    List<string> tableNames = new List<string>();
                    using (MySqlConnection tableConnection = new MySqlConnection(connectionString))
                    {
                        tableConnection.Open();

                        string tableQuery = "SELECT table_name " +
                                            "FROM information_schema.tables " +
                                            "WHERE table_schema = 'vv-matrix' AND table_type = 'BASE TABLE';";

                        using (MySqlCommand tableCommand = new MySqlCommand(tableQuery, tableConnection))
                        {
                            using (MySqlDataReader tableReader = tableCommand.ExecuteReader())
                            {
                                while (tableReader.Read())
                                {
                                    tableNames.Add(tableReader["table_name"].ToString()!);
                                }
                            }
                        }
                    }

                    // Second connection for executing delete statements with transaction
                    using (MySqlConnection deleteConnection = new MySqlConnection(connectionString))
                    {
                        deleteConnection.Open();

                        using (var transaction = deleteConnection.BeginTransaction())
                        {
                            try
                            {
                                foreach (string tableName in tableNames)
                                {
                                    if (CheckColumnExists(connectionString, tableName, "CreatedDate"))
                                    {
                                        string deleteStatement = $"DELETE FROM {tableName} WHERE CreatedDate < '{startDate}' OR CreatedDate >= '{endDate}';";
                                        ExecuteNonQueryWithForeignKeyCheck(deleteStatement, deleteConnection, transaction);
                                    }
                                }

                                // Commit transaction if all delete statements are successful
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                // Rollback transaction if any delete statement fails
                                transaction.Rollback();
                                throw new Exception($"An error occurred during delete operations: {ex.Message}", ex);
                            }
                        }
                    }
                }

                MessageBox.Show("Delete statements executed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool CheckColumnExists(string connectionString, string tableName, string columnName)
        {
            string query = $"SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = 'vv-matrix' AND table_name = '{tableName}' AND column_name = '{columnName}';";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        private void ExecuteNonQueryWithForeignKeyCheck(string query, MySqlConnection connection, MySqlTransaction transaction)
        {
            using (var command = new MySqlCommand(query, connection, transaction))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException ex) when (ex.Number == 1451 || ex.Number == 1452) // Foreign key constraint errors
                {
                    // Handle foreign key constraint errors
                    throw new Exception($"Foreign key constraint error: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error executing non-query: {ex.Message}", ex);
                }
            }
        }
        */

        // -----------------------------------------------------------

        // First  Set of Delete Data

        private void ExecuteDeleteStatements()
        {
            string startDate = "2023-04-01 00:00:00";
            string endDate = "2024-03-31 23:59:59";

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (AccountMasterContext context = new AccountMasterContext())
                {
                    string connectionString = context.Database.GetDbConnection().ConnectionString;

                    string tableQuery = $"SELECT table_name " +
                                        "FROM information_schema.tables " +
                                        "WHERE table_schema = 'vv-matrix' AND table_type = 'BASE TABLE';";

                    using (MySqlConnection tableConnection = new MySqlConnection(connectionString))
                    {
                        tableConnection.Open();

                        using (MySqlCommand tableCommand = new MySqlCommand(tableQuery, tableConnection))
                        {
                            using (MySqlDataReader tableReader = tableCommand.ExecuteReader())
                            {
                                while (tableReader.Read())
                                {
                                    string tableName = tableReader["table_name"].ToString()!;
                                    if (CheckColumnExists(connectionString, tableName, "CreatedDate"))
                                    {
                                        string deleteStatement = $"DELETE FROM {tableName} WHERE CreatedDate < '{startDate}' OR CreatedDate >= '{endDate}';";
                                        ExecuteNonQueryWithForeignKeyCheck(deleteStatement, connectionString);
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Delete statements executed successfully.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                Cursor.Current = Cursors.Default;
            }
        }
        private bool CheckColumnExists(string connectionString, string tableName, string columnName)
        {
            string columnQuery = $"SHOW COLUMNS FROM {tableName} LIKE '{columnName}';";
            using (MySqlConnection columnConnection = new MySqlConnection(connectionString))
            {
                columnConnection.Open();
                using (MySqlCommand columnCommand = new MySqlCommand(columnQuery, columnConnection))
                {
                    using (MySqlDataReader columnReader = columnCommand.ExecuteReader())
                    {
                        return columnReader.HasRows;
                    }
                }
            }
        }

        private void ExecuteNonQueryWithForeignKeyCheck(string sqlQuery, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand disableFkCheckCommand = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 0;", connection, transaction))
                        {
                            disableFkCheckCommand.ExecuteNonQuery();
                        }

                        using (MySqlCommand command = new MySqlCommand(sqlQuery, connection, transaction))
                        {
                            command.ExecuteNonQuery();
                        }

                        using (MySqlCommand enableFkCheckCommand = new MySqlCommand("SET FOREIGN_KEY_CHECKS = 1;", connection, transaction))
                        {
                            enableFkCheckCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        // - For Sales Tabel Deletion

        private void ExecuteDeleteSalesStatements()
        {
            string startDate = "2023-04-01 00:00:00";
            string endDate = "2024-03-31 23:59:59";

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                using (AccountMasterContext context = new AccountMasterContext())
                {
                    string connectionString = context.Database.GetDbConnection().ConnectionString;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                // First, delete related records from saledetails
                                string deleteSaleDetailsQuery = @"
                                    DELETE SD 
                                    FROM saledetails SD
                                    INNER JOIN saleentries SE ON SD.SaleId = SE.Id
                                    WHERE SE.SaleDate < @StartDate OR SE.SaleDate > @EndDate;";

                                using (var deleteSaleDetailsCommand = new MySqlCommand(deleteSaleDetailsQuery, connection, transaction))
                                {
                                    deleteSaleDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteSaleDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteSaleDetailsCommand.ExecuteNonQuery();
                                }

                                // Finally, delete records from saleentries
                                string deleteSaleEntriesQuery = @"
                                    DELETE FROM saleentries 
                                    WHERE SaleDate < @StartDate OR SaleDate > @EndDate;";

                                using (var deleteSaleEntriesCommand = new MySqlCommand(deleteSaleEntriesQuery, connection, transaction))
                                {
                                    deleteSaleEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteSaleEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteSaleEntriesCommand.ExecuteNonQuery();
                                }

                                // Commit the transaction
                                transaction.Commit();

                                MessageBox.Show("Delete statements executed successfully.");
                            }
                            catch (Exception ex)
                            {
                                // Rollback the transaction if any command fails
                                transaction.Rollback();
                                throw new Exception($"An error occurred during delete operations: {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /*
        // - For Purchase and Sales Tabel Deletion
        private void ExecuteDeletePurchaseAndSalesStatements()
        {
            string startDate = "2023-04-01 00:00:00";
            string endDate = "2024-03-31 23:59:59";

            //try
            //{
                Cursor.Current = Cursors.WaitCursor;

                using (AccountMasterContext context = new AccountMasterContext())
                {
                    string connectionString = context.Database.GetDbConnection().ConnectionString;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                        //try
                        //{
                        // Delete related records from stockmovementdetails
                        string deleteStockMovementDetailsQuery = @"
                            DELETE SMD 
                            FROM stockmovementdetails SMD
                            INNER JOIN stockmovements SM ON SMD.StockMovementId = SM.Id
                            INNER JOIN purchaseentries PE ON SM.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                        using (var deleteStockMovementDetailsCommand = new MySqlCommand(deleteStockMovementDetailsQuery, connection, transaction))
                        {
                            deleteStockMovementDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                            deleteStockMovementDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                            deleteStockMovementDetailsCommand.ExecuteNonQuery();
                        }


                        // Delete related records from taxdetails for purchase entries
                        string deleteTaxDetailsPurchaseQuery = @"
                            DELETE TD 
                            FROM taxdetails TD
                            INNER JOIN purchasedetails PD ON TD.PurchaseDetailsId = PD.Id
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deleteTaxDetailsPurchaseCommand = new MySqlCommand(deleteTaxDetailsPurchaseQuery, connection, transaction))
                                {
                                    deleteTaxDetailsPurchaseCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsPurchaseCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsPurchaseCommand.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for purchase entries directly
                                string deleteTaxDetailsDirectPurchaseQuery = @"
                            DELETE FROM taxdetails
                            WHERE PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                                using (var deleteTaxDetailsDirectPurchaseCommand = new MySqlCommand(deleteTaxDetailsDirectPurchaseQuery, connection, transaction))
                                {
                                    deleteTaxDetailsDirectPurchaseCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsDirectPurchaseCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsDirectPurchaseCommand.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for sale entries
                                string deleteTaxDetailsSaleQuery = @"
                            DELETE TD 
                            FROM taxdetails TD
                            INNER JOIN saledetails SD ON TD.SaleDetailsId = SD.Id
                            INNER JOIN saleentries SE ON SD.SaleId = SE.Id
                            WHERE SE.SaleDate < @StartDate OR SE.SaleDate > @EndDate;
                        ";

                                using (var deleteTaxDetailsSaleCommand = new MySqlCommand(deleteTaxDetailsSaleQuery, connection, transaction))
                                {
                                    deleteTaxDetailsSaleCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsSaleCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsSaleCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for purchase details
                                string deleteDiscountDetailsPurchaseDetailsQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            INNER JOIN purchasedetails PD ON DD.PurchaseDetailsId = PD.Id
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deleteDiscountDetailsPurchaseDetailsCommand = new MySqlCommand(deleteDiscountDetailsPurchaseDetailsQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsPurchaseDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsPurchaseDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsPurchaseDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for sale details
                                string deleteDiscountDetailsSaleDetailsQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            INNER JOIN saledetails SD ON DD.SaleDetailsId = SD.Id
                            INNER JOIN saleentries SE ON SD.SaleId = SE.Id
                            WHERE SE.SaleDate < @StartDate OR SE.SaleDate > @EndDate;
                        ";

                                using (var deleteDiscountDetailsSaleDetailsCommand = new MySqlCommand(deleteDiscountDetailsSaleDetailsQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsSaleDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsSaleDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsSaleDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for purchase entries
                                string deleteDiscountDetailsPurchaseEntriesQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            WHERE DD.PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                                using (var deleteDiscountDetailsPurchaseEntriesCommand = new MySqlCommand(deleteDiscountDetailsPurchaseEntriesQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsPurchaseEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsPurchaseEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsPurchaseEntriesCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for sale entries
                                string deleteDiscountDetailsSaleEntriesQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            WHERE DD.SaleId IN (
                                SELECT Id
                                FROM saleentries
                                WHERE SaleDate < @StartDate OR SaleDate > @EndDate
                            );
                        ";

                                using (var deleteDiscountDetailsSaleEntriesCommand = new MySqlCommand(deleteDiscountDetailsSaleEntriesQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsSaleEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsSaleEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsSaleEntriesCommand.ExecuteNonQuery();
                                }

                                // Delete related records from purchasedetails
                                string deletePurchaseDetailsQuery = @"
                            DELETE PD 
                            FROM purchasedetails PD
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deletePurchaseDetailsCommand = new MySqlCommand(deletePurchaseDetailsQuery, connection, transaction))
                                {
                                    deletePurchaseDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deletePurchaseDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deletePurchaseDetailsCommand.ExecuteNonQuery();
                                }

                        // Delete related records from stockmovements for purchase entries
                        string deleteStockMovementsQuery = @"
                            DELETE FROM stockmovements 
                            WHERE PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                        using (var deleteStockMovementsCommand = new MySqlCommand(deleteStockMovementsQuery, connection, transaction))
                        {
                            deleteStockMovementsCommand.Parameters.AddWithValue("@StartDate", startDate);
                            deleteStockMovementsCommand.Parameters.AddWithValue("@EndDate", endDate);
                            deleteStockMovementsCommand.ExecuteNonQuery();
                        }

                        // Delete related records from saledetails for sale entries
                        string deleteSaleDetailsQuery = @"
                            SELECT Id
                            FROM saleentries
                            WHERE SaleDate < @StartDate OR SaleDate > @EndDate;
                        ";

                        using (var deleteSaleDetailsCommand = new MySqlCommand(deleteSaleDetailsQuery, connection, transaction))
                        {
                            deleteSaleDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                            deleteSaleDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);

                            using (var reader = deleteSaleDetailsCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    long saleEntryId = reader.GetInt64("Id");

                                    // Close the data reader before reusing the connection
                                    reader.Close();

                                    // Delete sale details using a new connection object
                                    using (var deleteSaleDetailsForEntryConnection = new MySqlConnection(connectionString))
                                    {
                                        deleteSaleDetailsForEntryConnection.Open();

                                        string deleteSaleDetailsForEntryQuery = @"
        DELETE FROM saledetails 
        WHERE SaleId = @SaleId;
    ";

                                        using (var deleteSaleDetailsForEntryCommand = new MySqlCommand(deleteSaleDetailsForEntryQuery, deleteSaleDetailsForEntryConnection))
                                        {
                                            // Set the transaction for the command
                                            deleteSaleDetailsForEntryCommand.Transaction = transaction;

                                            // Check if the transaction is active for the connection
                                            if (deleteSaleDetailsForEntryCommand.Transaction != null &&
                                                deleteSaleDetailsForEntryCommand.Transaction.Connection == deleteSaleDetailsForEntryConnection &&
                                                deleteSaleDetailsForEntryCommand.Transaction.Connection.State == ConnectionState.Open)
                                            {
                                                deleteSaleDetailsForEntryCommand.Parameters.AddWithValue("@SaleId", saleEntryId);
                                                deleteSaleDetailsForEntryCommand.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                // Log detailed information to identify the issue
                                                if (deleteSaleDetailsForEntryCommand.Transaction == null)
                                                {
                                                    throw new InvalidOperationException("The transaction associated with this command is null.");
                                                }
                                                else if (deleteSaleDetailsForEntryCommand.Transaction.Connection != deleteSaleDetailsForEntryConnection)
                                                {
                                                    throw new InvalidOperationException("The transaction associated with this command is not for this connection.");
                                                }
                                                else if (deleteSaleDetailsForEntryCommand.Transaction.Connection.State != ConnectionState.Open)
                                                {
                                                    throw new InvalidOperationException("The connection associated with the transaction is not open.");
                                                }
                                                else
                                                {
                                                    throw new InvalidOperationException("The transaction associated with this command is not active for the connection.");
                                                }
                                            }
                                        }
                                    }
                                }
                            } // Data reader will be disposed automatically after use
                        }

                        // Delete records from saleentries
                        string deleteSaleEntriesQuery = @"
                            DELETE FROM saleentries 
                            WHERE SaleDate < @StartDate OR SaleDate > @EndDate;
                        ";

                                using (var deleteSaleEntriesCommand = new MySqlCommand(deleteSaleEntriesQuery, connection, transaction))
                                {
                                    deleteSaleEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteSaleEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteSaleEntriesCommand.ExecuteNonQuery();
                                }

                                // Commit the transaction
                                transaction.Commit();

                                MessageBox.Show("Delete statements executed successfully.");
                            //}
                            //catch (Exception ex)
                            //{
                            //    // Rollback the transaction if any command fails
                            //    transaction.Rollback();
                            //    throw new Exception($"An error occurred during delete operations: {ex.Message}", ex);
                            //}
                        }
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //}
            //finally
            //{
            //    Cursor.Current = Cursors.Default;
            //}
        }
        */
        /*
        // For Sales and Purchase Deletion on given date
        private void ExecuteDeletePurchaseAndSalesStatements()
        {
            string startDate = "2023-04-01 00:00:00";
            string endDate = "2024-03-31 23:59:59";

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                using (AccountMasterContext context = new AccountMasterContext())
                {
                    string connectionString = context.Database.GetDbConnection().ConnectionString;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                // Delete related records from stockmovementdetails
                                string deleteStockMovementDetailsQuery = @"
                            DELETE SMD 
                            FROM stockmovementdetails SMD
                            INNER JOIN stockmovements SM ON SMD.StockMovementId = SM.Id
                            INNER JOIN purchaseentries PE ON SM.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deleteStockMovementDetailsCommand = new MySqlCommand(deleteStockMovementDetailsQuery, connection, transaction))
                                {
                                    deleteStockMovementDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteStockMovementDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteStockMovementDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for purchase entries
                                string deleteTaxDetailsPurchaseQuery = @"
                            DELETE TD 
                            FROM taxdetails TD
                            INNER JOIN purchasedetails PD ON TD.PurchaseDetailsId = PD.Id
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deleteTaxDetailsPurchaseCommand = new MySqlCommand(deleteTaxDetailsPurchaseQuery, connection, transaction))
                                {
                                    deleteTaxDetailsPurchaseCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsPurchaseCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsPurchaseCommand.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for purchase entries directly
                                string deleteTaxDetailsDirectPurchaseQuery = @"
                            DELETE FROM taxdetails
                            WHERE PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                                using (var deleteTaxDetailsDirectPurchaseCommand = new MySqlCommand(deleteTaxDetailsDirectPurchaseQuery, connection, transaction))
                                {
                                    deleteTaxDetailsDirectPurchaseCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsDirectPurchaseCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsDirectPurchaseCommand.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for sale entries
                                string deleteTaxDetailsSaleQuery = @"
                            DELETE TD 
                            FROM taxdetails TD
                            INNER JOIN saledetails SD ON TD.SaleDetailsId = SD.Id
                            INNER JOIN saleentries SE ON SD.SaleId = SE.Id
                            WHERE SE.SaleDate < @StartDate OR SE.SaleDate > @EndDate;
                        ";

                                using (var deleteTaxDetailsSaleCommand = new MySqlCommand(deleteTaxDetailsSaleQuery, connection, transaction))
                                {
                                    deleteTaxDetailsSaleCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsSaleCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsSaleCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for purchase details
                                string deleteDiscountDetailsPurchaseDetailsQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            INNER JOIN purchasedetails PD ON DD.PurchaseDetailsId = PD.Id
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deleteDiscountDetailsPurchaseDetailsCommand = new MySqlCommand(deleteDiscountDetailsPurchaseDetailsQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsPurchaseDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsPurchaseDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsPurchaseDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for sale details
                                string deleteDiscountDetailsSaleDetailsQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            INNER JOIN saledetails SD ON DD.SaleDetailsId = SD.Id
                            INNER JOIN saleentries SE ON SD.SaleId = SE.Id
                            WHERE SE.SaleDate < @StartDate OR SE.SaleDate > @EndDate;
                        ";

                                using (var deleteDiscountDetailsSaleDetailsCommand = new MySqlCommand(deleteDiscountDetailsSaleDetailsQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsSaleDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsSaleDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsSaleDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for purchase entries
                                string deleteDiscountDetailsPurchaseEntriesQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            WHERE DD.PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                                using (var deleteDiscountDetailsPurchaseEntriesCommand = new MySqlCommand(deleteDiscountDetailsPurchaseEntriesQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsPurchaseEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsPurchaseEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsPurchaseEntriesCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for sale entries
                                string deleteDiscountDetailsSaleEntriesQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            WHERE DD.SaleId IN (
                                SELECT Id
                                FROM saleentries
                                WHERE SaleDate < @StartDate OR SaleDate > @EndDate
                            );
                        ";

                                using (var deleteDiscountDetailsSaleEntriesCommand = new MySqlCommand(deleteDiscountDetailsSaleEntriesQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsSaleEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsSaleEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsSaleEntriesCommand.ExecuteNonQuery();
                                }

                                // Delete related records from purchasedetails
                                string deletePurchaseDetailsQuery = @"
                            DELETE PD 
                            FROM purchasedetails PD
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deletePurchaseDetailsCommand = new MySqlCommand(deletePurchaseDetailsQuery, connection, transaction))
                                {
                                    deletePurchaseDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deletePurchaseDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deletePurchaseDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from stockmovements for purchase entries
                                string deleteStockMovementsQuery = @"
                            DELETE FROM stockmovements 
                            WHERE PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                                using (var deleteStockMovementsCommand = new MySqlCommand(deleteStockMovementsQuery, connection, transaction))
                                {
                                    deleteStockMovementsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteStockMovementsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteStockMovementsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from saledetails for sale entries
                                string deleteSaleDetailsQuery = @"
                            SELECT Id
                            FROM saleentries
                            WHERE SaleDate < @StartDate OR SaleDate > @EndDate;
                        ";

                                using (var deleteSaleDetailsCommand = new MySqlCommand(deleteSaleDetailsQuery, connection, transaction))
                                {
                                    deleteSaleDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteSaleDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);

                                    using (var reader = deleteSaleDetailsCommand.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            long saleEntryId = reader.GetInt64("Id");

                                            // Delete sale details using the same connection and transaction
                                            string deleteSaleDetailsForEntryQuery = @"
                                        DELETE FROM saledetails 
                                        WHERE SaleId = @SaleId;
                                    ";

                                            using (var deleteSaleDetailsForEntryCommand = new MySqlCommand(deleteSaleDetailsForEntryQuery, connection, transaction))
                                            {
                                                deleteSaleDetailsForEntryCommand.Parameters.AddWithValue("@SaleId", saleEntryId);
                                                deleteSaleDetailsForEntryCommand.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }

                                // Delete records from saleentries
                                string deleteSaleEntriesQuery = @"
                            DELETE FROM saleentries 
                            WHERE SaleDate < @StartDate OR SaleDate > @EndDate;
                        ";

                                using (var deleteSaleEntriesCommand = new MySqlCommand(deleteSaleEntriesQuery, connection, transaction))
                                {
                                    deleteSaleEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteSaleEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteSaleEntriesCommand.ExecuteNonQuery();
                                }

                                // Delete records from purchaseentries
                                string deletePurchaseEntriesQuery = @"
                            DELETE FROM purchaseentries 
                            WHERE RefDate < @StartDate OR RefDate > @EndDate;
                        ";

                                using (var deletePurchaseEntriesCommand = new MySqlCommand(deletePurchaseEntriesQuery, connection, transaction))
                                {
                                    deletePurchaseEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deletePurchaseEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deletePurchaseEntriesCommand.ExecuteNonQuery();
                                }

                                // Commit the transaction
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                // Rollback the transaction if any error occurs
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        */
        // Re-Coded for Purchase
        private void ExecuteDeletePurchaseRecords()
        {
            string startDate = "2023-04-01 00:00:00";
            string endDate = "2024-03-31 23:59:59";

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                using (AccountMasterContext context = new AccountMasterContext())
                {
                    string connectionString = context.Database.GetDbConnection().ConnectionString;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                // Delete related records from stockmovementdetails
                                string deleteStockMovementDetailsQuery = @"
                            DELETE SMD 
                            FROM stockmovementdetails SMD
                            INNER JOIN stockmovements SM ON SMD.StockMovementId = SM.Id
                            INNER JOIN purchaseentries PE ON SM.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deleteStockMovementDetailsCommand = new MySqlCommand(deleteStockMovementDetailsQuery, connection, transaction))
                                {
                                    deleteStockMovementDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteStockMovementDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteStockMovementDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for purchase entries
                                string deleteTaxDetailsPurchaseQuery = @"
                            DELETE TD 
                            FROM taxdetails TD
                            INNER JOIN purchasedetails PD ON TD.PurchaseDetailsId = PD.Id
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deleteTaxDetailsPurchaseCommand = new MySqlCommand(deleteTaxDetailsPurchaseQuery, connection, transaction))
                                {
                                    deleteTaxDetailsPurchaseCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsPurchaseCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsPurchaseCommand.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for purchase entries directly
                                string deleteTaxDetailsDirectPurchaseQuery = @"
                            DELETE FROM taxdetails
                            WHERE PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                                using (var deleteTaxDetailsDirectPurchaseCommand = new MySqlCommand(deleteTaxDetailsDirectPurchaseQuery, connection, transaction))
                                {
                                    deleteTaxDetailsDirectPurchaseCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsDirectPurchaseCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsDirectPurchaseCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for purchase details
                                string deleteDiscountDetailsPurchaseDetailsQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            INNER JOIN purchasedetails PD ON DD.PurchaseDetailsId = PD.Id
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deleteDiscountDetailsPurchaseDetailsCommand = new MySqlCommand(deleteDiscountDetailsPurchaseDetailsQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsPurchaseDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsPurchaseDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsPurchaseDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for purchase entries
                                string deleteDiscountDetailsPurchaseEntriesQuery = @"
                            DELETE DD 
                            FROM discountdetails DD
                            WHERE DD.PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                                using (var deleteDiscountDetailsPurchaseEntriesCommand = new MySqlCommand(deleteDiscountDetailsPurchaseEntriesQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsPurchaseEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsPurchaseEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsPurchaseEntriesCommand.ExecuteNonQuery();
                                }

                                // Delete related records from purchasedetails
                                string deletePurchaseDetailsQuery = @"
                            DELETE PD 
                            FROM purchasedetails PD
                            INNER JOIN purchaseentries PE ON PD.PurchaseEntryId = PE.Id
                            WHERE PE.RefDate < @StartDate OR PE.RefDate > @EndDate;
                        ";

                                using (var deletePurchaseDetailsCommand = new MySqlCommand(deletePurchaseDetailsQuery, connection, transaction))
                                {
                                    deletePurchaseDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deletePurchaseDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deletePurchaseDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from stockmovements for purchase entries
                                string deleteStockMovementsQuery = @"
                            DELETE FROM stockmovements 
                            WHERE PurchaseEntryId IN (
                                SELECT Id
                                FROM purchaseentries
                                WHERE RefDate < @StartDate OR RefDate > @EndDate
                            );
                        ";

                                using (var deleteStockMovementsCommand = new MySqlCommand(deleteStockMovementsQuery, connection, transaction))
                                {
                                    deleteStockMovementsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteStockMovementsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteStockMovementsCommand.ExecuteNonQuery();
                                }

                                // Delete records from purchaseentries
                                string deletePurchaseEntriesQuery = @"
                            DELETE FROM purchaseentries 
                            WHERE RefDate < @StartDate OR RefDate > @EndDate;
                        ";

                                using (var deletePurchaseEntriesCommand = new MySqlCommand(deletePurchaseEntriesQuery, connection, transaction))
                                {
                                    deletePurchaseEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deletePurchaseEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deletePurchaseEntriesCommand.ExecuteNonQuery();
                                }

                                // Commit the transaction
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                // Rollback the transaction if any error occurs
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        // Re-Coded for Sales
        private void ExecuteDeleteSalesRecords()
        {
            string startDate = "2023-04-01 00:00:00";
            string endDate = "2024-03-31 23:59:59";

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                using (AccountMasterContext context = new AccountMasterContext())
                {
                    string connectionString = context.Database.GetDbConnection().ConnectionString;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string deleteDependentRecordsQuery1 = @"
                            DELETE FROM payments 
                            WHERE Reference IN (
                                SELECT Id
                                FROM saleentries
                                WHERE CreatedDate < @StartDate OR CreatedDate > @EndDate
                            );
";
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                using (var deleteDependentRecordsCommand1 = new MySqlCommand(deleteDependentRecordsQuery1, connection, transaction))
                                {
                                    deleteDependentRecordsCommand1.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDependentRecordsCommand1.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDependentRecordsCommand1.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for sale entries
                                string deleteTaxDetailsSaleQuery = @"
                                    DELETE TD 
                                    FROM taxdetails TD
                                    INNER JOIN saledetails SD ON TD.SaleDetailsId = SD.Id
                                    INNER JOIN saleentries SE ON SD.SaleId = SE.Id
                                    WHERE SE.CreatedDate < @StartDate OR SE.CreatedDate > @EndDate;
                                ";

                                using (var deleteTaxDetailsSaleCommand = new MySqlCommand(deleteTaxDetailsSaleQuery, connection, transaction))
                                {
                                    deleteTaxDetailsSaleCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsSaleCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsSaleCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for sale details
                                string deleteDiscountDetailsSaleDetailsQuery = @"
                                    DELETE DD 
                                    FROM discountdetails DD
                                    INNER JOIN saledetails SD ON DD.SaleDetailsId = SD.Id
                                    INNER JOIN saleentries SE ON SD.SaleId = SE.Id
                                    WHERE SE.CreatedDate < @StartDate OR SE.CreatedDate > @EndDate;
                                ";

                                using (var deleteDiscountDetailsSaleDetailsCommand = new MySqlCommand(deleteDiscountDetailsSaleDetailsQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsSaleDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsSaleDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsSaleDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from discountdetails for sale entries
                                string deleteDiscountDetailsSaleEntriesQuery = @"
                                    DELETE DD 
                                    FROM discountdetails DD
                                    WHERE DD.SaleId IN (
                                        SELECT Id
                                        FROM saleentries
                                        WHERE CreatedDate < @StartDate OR CreatedDate > @EndDate
                                    );
                                ";

                                using (var deleteDiscountDetailsSaleEntriesCommand = new MySqlCommand(deleteDiscountDetailsSaleEntriesQuery, connection, transaction))
                                {
                                    deleteDiscountDetailsSaleEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteDiscountDetailsSaleEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteDiscountDetailsSaleEntriesCommand.ExecuteNonQuery();
                                }

                                // Create a temporary table to store the IDs of the child sale details to be deleted
                                string createTempTableQuery = @"
                                    CREATE TEMPORARY TABLE TempSaleDetailsToDelete (Id BIGINT);
                                ";
                                using (var createTempTableCommand = new MySqlCommand(createTempTableQuery, connection, transaction))
                                {
                                    createTempTableCommand.ExecuteNonQuery();
                                }

                                // Insert the IDs of the child sale details to be deleted into the temporary table
                                string insertTempTableQuery = @"
                                    INSERT INTO TempSaleDetailsToDelete (Id)
                                    SELECT SD1.Id
                                    FROM saledetails SD1
                                    WHERE SD1.SaleDetailId IN (
                                        SELECT SD2.Id
                                        FROM saledetails SD2
                                        INNER JOIN saleentries SE ON SD2.SaleId = SE.Id
                                        WHERE SE.CreatedDate < @StartDate OR SE.CreatedDate > @EndDate
                                    );
                                ";

                                using (var insertTempTableCommand = new MySqlCommand(insertTempTableQuery, connection, transaction))
                                {
                                    insertTempTableCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    insertTempTableCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    insertTempTableCommand.ExecuteNonQuery();
                                }

                                // Delete the child sale details using the temporary table
                                string deleteChildSaleDetailsQuery = @"
                                    DELETE FROM saledetails 
                                    WHERE Id IN (SELECT Id FROM TempSaleDetailsToDelete);
                                ";

                                using (var deleteChildSaleDetailsCommand = new MySqlCommand(deleteChildSaleDetailsQuery, connection, transaction))
                                {
                                    deleteChildSaleDetailsCommand.ExecuteNonQuery();
                                }

                                // Drop the temporary table
                                string dropTempTableQuery = "DROP TEMPORARY TABLE IF EXISTS TempSaleDetailsToDelete;";
                                using (var dropTempTableCommand = new MySqlCommand(dropTempTableQuery, connection, transaction))
                                {
                                    dropTempTableCommand.ExecuteNonQuery();
                                }

                                // Delete related records from stockmovementdetails
                                string deleteStockMovementDetailsQuery = @"
                                    DELETE SMD 
                                    FROM stockmovementdetails SMD
                                    INNER JOIN stockmovements SM ON SMD.StockMovementId = SM.Id
                                    WHERE SM.CreatedDate < @StartDate OR SM.CreatedDate > @EndDate;
                                ";

                                using (var deleteStockMovementDetailsCommand = new MySqlCommand(deleteStockMovementDetailsQuery, connection, transaction))
                                {
                                    deleteStockMovementDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteStockMovementDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteStockMovementDetailsCommand.ExecuteNonQuery();
                                }

                                // Now you can safely delete related records from stockmovements
                                string deleteStockMovementsQuery = @"
                                    DELETE FROM stockmovements 
                                    WHERE CreatedDate < @StartDate OR CreatedDate > @EndDate;
                                ";

                                using (var deleteStockMovementsCommand = new MySqlCommand(deleteStockMovementsQuery, connection, transaction))
                                {
                                    deleteStockMovementsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteStockMovementsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteStockMovementsCommand.ExecuteNonQuery();
                                }


                                // Step 2: Delete parent records in saledetails
                                string deleteSaleDetailsQuery = @"
                                    DELETE SD
                                    FROM saledetails SD
                                    WHERE SD.SaleId IN (
                                        SELECT Id
                                        FROM saleentries
                                        WHERE CreatedDate < @StartDate OR CreatedDate > @EndDate
                                    );
                                ";

                                using (var deleteSaleDetailsCommand = new MySqlCommand(deleteSaleDetailsQuery, connection, transaction))
                                {
                                    deleteSaleDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteSaleDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteSaleDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete related records from taxdetails for sale entries
                                string deleteTaxDetailsQuery = @"
                                    DELETE TD 
                                    FROM taxdetails TD
                                    INNER JOIN saleentries SE ON TD.SaleId = SE.Id
                                    WHERE SE.CreatedDate < @StartDate OR SE.CreatedDate > @EndDate;
                                ";

                                using (var deleteTaxDetailsCommand = new MySqlCommand(deleteTaxDetailsQuery, connection, transaction))
                                {
                                    deleteTaxDetailsCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteTaxDetailsCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteTaxDetailsCommand.ExecuteNonQuery();
                                }

                                // Delete records from saleentries
                                string deleteSaleEntriesQuery = @"
                                    DELETE FROM saleentries 
                                    WHERE CreatedDate < @StartDate OR CreatedDate > @EndDate;
                                ";

                                using (var deleteSaleEntriesCommand = new MySqlCommand(deleteSaleEntriesQuery, connection, transaction))
                                {
                                    deleteSaleEntriesCommand.Parameters.AddWithValue("@StartDate", startDate);
                                    deleteSaleEntriesCommand.Parameters.AddWithValue("@EndDate", endDate);
                                    deleteSaleEntriesCommand.ExecuteNonQuery();
                                }


                                // Commit the transaction
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                // Rollback the transaction if any error occurs
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private void ExecuteNonQuery(string sqlQuery, string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void BtnElemenateSales_Click(object sender, EventArgs e)
        {
            //try
            //{
            ExecuteDeleteSalesRecords();
            MessageBox.Show("Data modification completed successfully.");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //}
        }

        private void BtnDeletePurchase_Click(object sender, EventArgs e)
        {
            try
            {
                ExecuteDeletePurchaseRecords();
                MessageBox.Show("Data modification completed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void BtnPurchaseDelete_Click(object sender, EventArgs e)
        {
            // LblTimeRecord.Text = "Time Taken for purchase deletion : ";
            LblTotRec.Text = "Total Purchase Record : ";
            var sw = new Stopwatch();
            Cursor.Current = Cursors.WaitCursor;
            sw.Start();
            GetAllPurchaseEntryId(1);
            LblTotRecCount.Text = purchaseEntryIds.Length.ToString();
            await DeleteAllPurchaseAsync();
            sw.Stop();
            LblTimeTaken.Text = $"Elapsed Time: {sw.Elapsed.ToString(@"hh\:mm\:ss\.fff")}";
            Cursor.Current = Cursors.Default;

        }
        private long[] purchaseEntryIds;
        private long[] SalesEntyIds;
        public void GetAllPurchaseEntryId(long SelectedCompanyId)
        {
            Cursor.Current = Cursors.WaitCursor;
            DateTime startFromDate = new DateTime(2023, 4, 1, 0, 0, 0);
            DateTime endToDate = new DateTime(2024, 3, 31, 23, 59, 59);

            IList<PurchaseEntry> purchaseIdDetails = PurchaseEntryManager.Instance.ListPurchasePurchaseIdByCreateDate(startFromDate, endToDate, SelectedCompanyId);

            purchaseEntryIds = new long[purchaseIdDetails.Count];
            int i = 0;

            foreach (var purchaseDetail in purchaseIdDetails)
            {
                purchaseEntryIds[i++] = purchaseDetail.Id;
            }
            Cursor.Current = Cursors.Default;
            // You now have an array of purchase entry IDs in purchaseEntryIds.
        }
        public void GetAllSalesEntryId(long SelectedCompanyId)
        {
            Cursor.Current = Cursors.WaitCursor;
            DateTime startFromDate = new DateTime(2023, 4, 1, 0, 0, 0);
            DateTime endToDate = new DateTime(2024, 3, 31, 23, 59, 59);

            IList<SaleEntry> SalseIdDetails = SalesManager.Instance.ListSaleEntryIdByCreateDate(startFromDate, endToDate, SelectedCompanyId);

            SalesEntyIds = new long[SalseIdDetails.Count];
            int i = 0;

            foreach (var SalesDetail in SalseIdDetails)
            {
                SalesEntyIds[i++] = SalesDetail.Id;
            }
            Cursor.Current = Cursors.Default;
            // You now have an array of purchase entry IDs in purchaseEntryIds.
        }
        public int DeletePurCount = 0;
        public int DeleteSalCount = 0;
        public async Task DeleteAllPurchaseAsync()
        {
            LblRecordCount.Text = "Deleting Purchase Record #  :";
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < purchaseEntryIds.Length; i++)
            {
                var purchaseID = purchaseEntryIds[i];
                TxtBoxRecordCount.Text = (DeletePurCount + 1).ToString();

                // Update UI
                await Task.Delay(1);

                bool deleteResult = await Task.Run(() => PurchaseEntryManager.Instance.DeletePurchaseEntry(purchaseID));
                DeletePurCount++;
                if (!deleteResult)
                {
                    // Handle the case where the deletion fails, if necessary
                    Console.WriteLine($"Failed to delete purchase entry with ID: {purchaseID}");
                    MessageBox.Show($"Failed to delete purchase entry with ID: {purchaseID}");
                }
            }
            MessageBox.Show("Deleted purchase entry Successfully");
            Cursor.Current = Cursors.Default;
        }
        public async Task DeleteAllSalesAsync()
        {
            LblRecordCount.Text = "Deleting Sales Record #  :";
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < SalesEntyIds.Length; i++)
            {
                var SalseID = SalesEntyIds[i];
                TxtBoxRecordCount.Text = (DeleteSalCount + 1).ToString();

                // Update UI
                await Task.Delay(1);

                bool deleteResult = await Task.Run(() => SalesManager.Instance.DeleteSaleEntry(SalseID));
                DeleteSalCount++;
                if (!deleteResult)
                {
                    // Handle the case where the deletion fails, if necessary
                    Console.WriteLine($"Failed to delete sales entry with ID: {SalseID}");
                    MessageBox.Show($"Failed to delete sales entry with ID: {SalseID}");
                }
            }
            MessageBox.Show("Deleted sales entry Successfully");
            Cursor.Current = Cursors.Default;
        }
        private async Task DeleteAllRemainingSalesAsync()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                using (AccountMasterContext context = new AccountMasterContext())
                {
                    string connectionString = context.Database.GetDbConnection().ConnectionString;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        using (var transaction = await connection.BeginTransactionAsync())
                        {
                            try
                            {
                                // Convert SalesEntyIds to a comma-separated string
                                string saleEntryIdsCsv = string.Join(",", SalesEntyIds);

                                // Step 1: Delete child records from taxdetails
                                string deleteTaxDetailsQuery = $@"
                            DELETE FROM taxdetails 
                            WHERE SaleDetailsId IN (SELECT Id FROM saledetails WHERE SaleId IN ({saleEntryIdsCsv}))";

                                using (var deleteTaxDetailsCommand = new MySqlCommand(deleteTaxDetailsQuery, connection, transaction))
                                {
                                    await deleteTaxDetailsCommand.ExecuteNonQueryAsync();
                                }

                                // Step 2: Delete child records from saledetails
                                string deleteChildSaleDetailsQuery = $@"
                            DELETE FROM saledetails 
                            WHERE SaleId IN ({saleEntryIdsCsv})";

                                using (var deleteChildSaleDetailsCommand = new MySqlCommand(deleteChildSaleDetailsQuery, connection, transaction))
                                {
                                    await deleteChildSaleDetailsCommand.ExecuteNonQueryAsync();
                                }

                                // Step 3: Delete parent records from saleentries
                                string deleteParentSaleEntriesQuery = $@"
                            DELETE FROM saleentries 
                            WHERE Id IN ({saleEntryIdsCsv})";

                                using (var deleteParentSaleEntriesCommand = new MySqlCommand(deleteParentSaleEntriesQuery, connection, transaction))
                                {
                                    await deleteParentSaleEntriesCommand.ExecuteNonQueryAsync();
                                }

                                // Commit the transaction
                                await transaction.CommitAsync();

                                MessageBox.Show("Delete statements executed successfully.");
                            }
                            catch (Exception ex)
                            {
                                // Rollback the transaction if any command fails
                                await transaction.RollbackAsync();
                                throw new Exception($"An error occurred during delete operations: {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        public void DeleteAllPurchase()
        {

            Cursor.Current = Cursors.WaitCursor;
            foreach (var purchaseID in purchaseEntryIds)
            {
                TxtBoxRecordCount.Text = (DeletePurCount + 1).ToString();
                bool deleteResult = PurchaseEntryManager.Instance.DeletePurchaseEntry(purchaseID);
                DeletePurCount++;
                if (!deleteResult)
                {
                    // Handle the case where the deletion fails, if necessary
                    Console.WriteLine($"Failed to delete purchase entry with ID: {purchaseID}");
                    MessageBox.Show("Failed to delete purchase entry with ID: {purchaseID}");
                }
            }
            MessageBox.Show("Deleted purchase entry Successfully");
            Cursor.Current = Cursors.Default;
        }

        private async void BtnSalesDelete_Click(object sender, EventArgs e)
        {
            // LblTimeRecord.Text = "Time Taken for salse deletion  : ";
            LblTotRec.Text = "Total Sales Record : ";
            var sw = new Stopwatch();
            Cursor.Current = Cursors.WaitCursor;
            sw.Start();
            GetAllSalesEntryId(1);
            LblTotRecCount.Text = SalesEntyIds.Length.ToString();
            await DeleteAllSalesAsync();
            sw.Stop();
            LblTimeTaken.Text = $"Elapsed Time: {sw.Elapsed.ToString(@"hh\:mm\:ss\.fff")}";
            Cursor.Current = Cursors.Default;
        }

        private async void BtnDelRemaingSales_Click(object sender, EventArgs e)
        {
            LblTotRec.Text = "Total Sales Record Bal : ";
            var sw = new Stopwatch();
            Cursor.Current = Cursors.WaitCursor;
            sw.Start();
            GetAllSalesEntryId(1);
            LblTotRecCount.Text = SalesEntyIds.Length.ToString();
            await DeleteAllRemainingSalesAsync();
            sw.Stop();
            LblTimeTaken.Text = $"Elapsed Time: {sw.Elapsed.ToString(@"hh\:mm\:ss\.fff")}";
            Cursor.Current = Cursors.Default;
        }
    }
}
