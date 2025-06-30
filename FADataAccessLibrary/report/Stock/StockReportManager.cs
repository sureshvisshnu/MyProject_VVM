using fa.api.catalog;
using fa.context;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FaData.Utils;
using fa.model.Catalog;
using fa.model.OrderManagement;
using Fa.model.Purchase;
using NPOI.SS.Formula.Functions;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using MathNet.Numerics.RootFinding;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using fa.model.Hms.common;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System.Diagnostics;
using FADataAccessLibrary.Migrations;
using NPOI.SS.Formula.Atp;
using Org.BouncyCastle.Tls.Crypto;
using static NPOI.HSSF.Util.HSSFColor;
using System.ComponentModel.Design;

namespace FADataAccessLibrary.report.Stock
{
    public class StockReportManager
    {
        private static volatile StockReportManager instance;
        private static object syncRoot = new Object();

        StockReportManager()
        {

        }
        public static StockReportManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new StockReportManager();
                    }
                }
                return instance;
            }
        }

        public AccountType AccountingType { get; set; }
        public static DateTime FromDate { get; set; }
        public static DateTime ToDate { get; set; }
        // Stock Report Testing
        public bool IsStoredProcedureExists(string storedProcedureName)
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            string schemaName = "";
            if (ConString != null)
            {
                var builder = new DbConnectionStringBuilder { ConnectionString = ConString };

                if (builder.TryGetValue("Database", out var database))
                {
                    schemaName = database?.ToString();
                }
            }
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SHOW PROCEDURE STATUS WHERE Db = @databaseName AND Name = @procedureName", connection);
                command.Parameters.AddWithValue("@databaseName", schemaName);
                command.Parameters.AddWithValue("@procedureName", storedProcedureName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }
        public void CreateSPForStockReportByLoctn()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                    CREATE PROCEDURE StockReportGeneratorByMultiLocation(
                    IN p_CompanyId BIGINT, 
                    IN p_InventoryLocationIds VARCHAR(255)
                        )
                        BEGIN
                            SET @row_number := 0;

                            DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                            CREATE TEMPORARY TABLE TempStockReport (
                                SlNo INT,
                                ProductCode VARCHAR(25),
                                Name VARCHAR(50),
                                UOM VARCHAR(14),
                                RackNo VARCHAR(255),
                                `Batch / Expiry` VARCHAR(255),
                                `Purchase Price` FLOAT,
                                `Opening Stock` DOUBLE,
                                `Closing Stock` DOUBLE,
                                Purchase DOUBLE,
                                `Purchase Return` DOUBLE,
                                Sales DOUBLE,
                                `Sales Return` DOUBLE,
                                `Stock In` DOUBLE,
                                `Stock Out` DOUBLE,
                                `To Patient` DOUBLE,
                                Damage DOUBLE,
                                Adjust DOUBLE,
                                `Last Month Sale` DOUBLE,
                                `Group Head` VARCHAR(255)
                            );

                            INSERT INTO TempStockReport
                            SELECT 
                                @row_number := @row_number + 1 AS SlNo,
                                ci.MaterialId AS ProductCode,
                                ci.Name,
                                ci.UOM,
                                ci.RackNumber AS RackNo,
                                CONCAT(IFNULL(ib.BatchNo, ''), ' / ', IFNULL(DATE_FORMAT(ib.ExpDate, '%Y-%m-%d'), '')) AS `Batch / Expiry`,
                                IFNULL(ib.PurchasePrice, 0) AS `Purchase Price`,
                                IFNULL(ib.OpeningStock, 0) AS `Opening Stock`,
                                IFNULL(ib.OpeningStock, 0) 
                                    + IFNULL(purchaseTotal.TotalPurchase, 0) 
                                    - IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) 
                                    - IFNULL(saleTotal.TotalSale, 0) 
                                    + IFNULL(saleReturnTotal.TotalSalesReturn, 0)
                                    + IFNULL(stockInTotal.TotalIn, 0) 
                                    - IFNULL(stockOutTotal.TotalOut, 0) 
                                    - IFNULL(toPatientTotal.TotalToPatient, 0) 
                                    - IFNULL(damageTotal.TotalDamage, 0) 
                                    + IFNULL(adjustTotal.TotalAdjust, 0) AS `Closing Stock`,
                                IFNULL(purchaseTotal.TotalPurchase, 0) AS Purchase,
                                IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) AS `Purchase Return`,
                                IFNULL(saleTotal.TotalSale, 0) AS Sales,
                                IFNULL(saleReturnTotal.TotalSalesReturn, 0) AS `Sales Return`,
                                IFNULL(stockInTotal.TotalIn, 0) AS `Stock In`,
                                IFNULL(stockOutTotal.TotalOut, 0) AS `Stock Out`,
                                IFNULL(toPatientTotal.TotalToPatient, 0) AS `To Patient`,
                                IFNULL(damageTotal.TotalDamage, 0) AS Damage,
                                IFNULL(adjustTotal.TotalAdjust, 0) AS Adjust,
                                IFNULL(lastMonthSale.LastMonthSale, 0) AS `Last Month Sale`,
                                il.Name AS `Group Head`
                            FROM 
                                (SELECT * FROM catalogitems WHERE CompanyId = p_CompanyId) ci
                            LEFT JOIN (
                                SELECT 
                                    i.ProductId,
                                    ib.BatchNo,
                                    ib.ExpDate,
                                    ib.PurchasePrice,
                                    SUM(ib.OpeningStock) AS OpeningStock,
                                    i.InventoryLocationId
                                FROM 
                                    inventories i
                                LEFT JOIN inventorybatches ib ON i.Id = ib.InventoryId AND ib.CompanyId = p_CompanyId
                                WHERE i.CompanyId = p_CompanyId 
                                AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                                AND i.Id = ib.InventoryId
                                GROUP BY i.ProductId, ib.BatchNo, ib.ExpDate, ib.PurchasePrice, i.InventoryLocationId
                            ) ib ON ci.Id = ib.ProductId
                            LEFT JOIN inventorylocations il ON ib.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalPurchase
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 5 -- PURCHASE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) purchaseTotal ON ci.Id = purchaseTotal.ProductId AND ib.BatchNo = purchaseTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalPurchaseReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 7 -- PURCHASE_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) purchaseReturnTotal ON ci.Id = purchaseReturnTotal.ProductId AND ib.BatchNo = purchaseReturnTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) saleTotal ON ci.Id = saleTotal.ProductId AND ib.BatchNo = saleTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalSalesReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 8 -- SALES_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) saleReturnTotal ON ci.Id = saleReturnTotal.ProductId AND ib.BatchNo = saleReturnTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalIn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 1 -- STOCK_IN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) stockInTotal ON ci.Id = stockInTotal.ProductId AND ib.BatchNo = stockInTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalOut
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 2 -- STOCK_OUT
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) stockOutTotal ON ci.Id = stockOutTotal.ProductId AND ib.BatchNo = stockOutTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalToPatient
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 3 -- PATIENT_USE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) toPatientTotal ON ci.Id = toPatientTotal.ProductId AND ib.BatchNo = toPatientTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalDamage
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 4 -- DAMAGED
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) damageTotal ON ci.Id = damageTotal.ProductId AND ib.BatchNo = damageTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalAdjust
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 9 -- ADJUSTMENT
                                AND sm.CompanyId = p_CompanyId
                                AND (FIND_IN_SET(sm.InventoryLocationFromId, p_InventoryLocationIds) OR FIND_IN_SET(sm.InventoryLocationToId, p_InventoryLocationIds))
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) adjustTotal ON ci.Id = adjustTotal.ProductId AND ib.BatchNo = adjustTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS LastMonthSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.MovementDate BETWEEN DATE_FORMAT(NOW() - INTERVAL 1 MONTH, '%Y-%m-01') AND LAST_DAY(NOW() - INTERVAL 1 MONTH)
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryLocationFromId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) lastMonthSale ON ci.Id = lastMonthSale.ProductId AND ib.BatchNo = lastMonthSale.BatchNo;

                            SELECT * 
                            FROM TempStockReport
                            WHERE `Opening Stock` <> 0 OR `Closing Stock` <> 0;

                            DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                        END";
                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }
        public void CreateSPForStockReportWithoutBatchByLoctn()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                                CREATE PROCEDURE StockReportGeneratorByDateAndMultiLocationWithoutBatch(
                    IN p_CompanyId BIGINT,
                    IN p_InventoryLocationIds VARCHAR(255),
                    IN FromDate DATE,
                    IN ToDate DATE
                )
                BEGIN
                    DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                                CREATE TEMPORARY TABLE TempStockReport(
                                    SlNo INT,
                                    ProductCode VARCHAR(25),
                        Name VARCHAR(50),
                        UOM VARCHAR(14),
                        RackNo VARCHAR(255),
                        `Opening Stock` DOUBLE,
                        `Closing Stock` DOUBLE,
                        Purchase DOUBLE,
                        `Purchase Return` DOUBLE,
                        Sales DOUBLE,
                        `Sales Return` DOUBLE,
                        `Stock In` DOUBLE,
                        `Stock Out` DOUBLE,
                        `To Patient` DOUBLE,
                        Damage DOUBLE,
                        Adjust DOUBLE,
                        `Last Month Sale` DOUBLE,
                        `Group Head` VARCHAR(255)
                    );

                    SET @row_number := 0;

                                INSERT INTO TempStockReport
                    SELECT
                        @row_number := @row_number + 1 AS SlNo,
                        ci.MaterialId AS ProductCode,
                        ci.Name,
                        ci.UOM,
                        ci.RackNumber AS RackNo,
                        IFNULL(SUM(i.OpeningStock), 0) AS `Opening Stock`,
                        IFNULL(SUM(i.OpeningStock), 0) 
                            +IFNULL(SUM(purchaseTotal.TotalPurchase), 0)
                            - IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0)
                            - IFNULL(SUM(saleTotal.TotalSale), 0)
                            + IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0)
                                + IFNULL(SUM(stockInTotal.TotalIn), 0)
                            - IFNULL(SUM(stockOutTotal.TotalOut), 0)
                            - IFNULL(SUM(toPatientTotal.TotalToPatient), 0)
                            - IFNULL(SUM(damageTotal.TotalDamage), 0)
                            + IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS `Closing Stock`,
                        IFNULL(SUM(purchaseTotal.TotalPurchase), 0) AS Purchase,
                        IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) AS `Purchase Return`,
                        IFNULL(SUM(saleTotal.TotalSale), 0) AS Sales,
                        IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0) AS `Sales Return`,
                        IFNULL(SUM(stockInTotal.TotalIn), 0) AS `Stock In`,
                        IFNULL(SUM(stockOutTotal.TotalOut), 0) AS `Stock Out`,
                        IFNULL(SUM(toPatientTotal.TotalToPatient), 0) AS `To Patient`,
                        IFNULL(SUM(damageTotal.TotalDamage), 0) AS Damage,
                        IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS Adjust,
                        IFNULL(SUM(lastMonthSale.LastMonthSale), 0) AS `Last Month Sale`,
                        il.Name AS `Group Head`
                    FROM
                        (SELECT * FROM catalogitems WHERE CompanyId = p_CompanyId) ci
                    LEFT JOIN(
                        SELECT
                            i.ProductId,
                            i.InventoryLocationId,
                            SUM(i.OpeningStock) AS OpeningStock
                        FROM
                            inventories i
                        WHERE i.CompanyId = p_CompanyId
                        AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                        GROUP BY i.ProductId, i.InventoryLocationId
                    ) i ON ci.Id = i.ProductId
                    LEFT JOIN inventorylocations il ON i.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                    LEFT JOIN(
                        SELECT
                                smd.ProductId,
                            sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalPurchase
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 5-- PURCHASE
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) purchaseTotal ON ci.Id = purchaseTotal.ProductId AND i.InventoryLocationId = purchaseTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                            sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalPurchaseReturn
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 7-- PURCHASE_RETURN
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) purchaseReturnTotal ON ci.Id = purchaseReturnTotal.ProductId AND i.InventoryLocationId = purchaseReturnTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                             sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalSale
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 6-- SALES
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) saleTotal ON ci.Id = saleTotal.ProductId AND i.InventoryLocationId = saleTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                            sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalSalesReturn
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 8-- SALES_RETURN
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) saleReturnTotal ON ci.Id = saleReturnTotal.ProductId AND i.InventoryLocationId = saleReturnTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                            sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalIn
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 1-- STOCK_IN
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) stockInTotal ON ci.Id = stockInTotal.ProductId AND i.InventoryLocationId = stockInTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                            sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalOut
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 2-- STOCK_OUT
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) stockOutTotal ON ci.Id = stockOutTotal.ProductId AND i.InventoryLocationId = stockOutTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                            sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalToPatient
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 3-- PATIENT_USE
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) toPatientTotal ON ci.Id = toPatientTotal.ProductId AND i.InventoryLocationId = toPatientTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                            sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalDamage
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 4-- DAMAGED
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) damageTotal ON ci.Id = damageTotal.ProductId AND i.InventoryLocationId = damageTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                            sm.InventoryStockLocationId,
                            SUM(smd.Quantity) AS TotalAdjust
                        FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 9-- ADJUSTMENT
                        AND sm.CompanyId = p_CompanyId
                        AND (FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds) OR FIND_IN_SET(sm.InventoryLocationToId, p_InventoryLocationIds))
                        AND sm.MovementDate BETWEEN FromDate AND ToDate
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) adjustTotal ON ci.Id = adjustTotal.ProductId AND i.InventoryLocationId = adjustTotal.InventoryStockLocationId
                    LEFT JOIN(
                        SELECT
                            smd.ProductId,
                            sm.InventoryStockLocationId,
                                SUM(smd.Quantity) AS LastMonthSale
                                FROM
                            stockmovementdetails smd
                        JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                        WHERE sm.Type = 6-- SALES
                        AND sm.MovementDate BETWEEN DATE_FORMAT(NOW() -INTERVAL 1 MONTH, '%Y-%m-01') AND LAST_DAY(NOW() -INTERVAL 1 MONTH)
                        AND sm.CompanyId = p_CompanyId
                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                        GROUP BY smd.ProductId, sm.InventoryStockLocationId
                    ) lastMonthSale ON ci.Id = lastMonthSale.ProductId AND i.InventoryLocationId = lastMonthSale.InventoryStockLocationId
                    GROUP BY ci.Id, il.Name;

                    SELECT*
                    FROM TempStockReport
                    WHERE `Opening Stock` <> 0 OR `Closing Stock` <> 0
                    ORDER BY `Group Head`;

                    DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                    END";
                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }
        public void CreateSPForStockReportWithBatchByLoctn()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                                CREATE PROCEDURE StockReportGeneratorByDateAndMultiLocationWithBatch(
                        IN p_CompanyId BIGINT, 
                        IN p_InventoryLocationIds VARCHAR(255),
                        IN FromDate DATE,
                        IN ToDate DATE
                    )
                    BEGIN
                        SET @row_number := 0;

                        DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                        CREATE TEMPORARY TABLE TempStockReport (
                            SlNo INT,
                            ProductCode VARCHAR(25),
                            Name VARCHAR(50),
                            UOM VARCHAR(14),
                            RackNo VARCHAR(255),
                            `Batch / Expiry` VARCHAR(255),
                            `Opening Stock` DOUBLE,
                            `Closing Stock` DOUBLE,
                            Purchase DOUBLE,
                            `Purchase Return` DOUBLE,
                            Sales DOUBLE,
                            `Sales Return` DOUBLE,
                            `Stock In` DOUBLE,
                            `Stock Out` DOUBLE,
                            `To Patient` DOUBLE,
                            Damage DOUBLE,
                            Adjust DOUBLE,
                            `Last Month Sale` DOUBLE,
                            `Group Head` VARCHAR(255)
                        );

                        INSERT INTO TempStockReport
                        SELECT 
                            @row_number := @row_number + 1 AS SlNo,
                            ci.Id AS ProductCode,
                            ci.Name,
                            ci.UOM,
                            ci.RackNumber AS RackNo,
                            CONCAT(IFNULL(ib.BatchNo, ''), ' / ', IFNULL(DATE_FORMAT(ib.ExpDate, '%Y-%m-%d'), '')) AS `Batch / Expiry`,
                            IFNULL(SUM(ib.OpeningStock), 0) AS `Opening Stock`,
                            IFNULL(SUM(ib.OpeningStock), 0) 
                                + IFNULL(SUM(purchaseTotal.TotalPurchase), 0) 
                                - IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) 
                                - IFNULL(SUM(saleTotal.TotalSale), 0) 
                                + IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0)
                                + IFNULL(SUM(stockInTotal.TotalIn), 0) 
                                - IFNULL(SUM(stockOutTotal.TotalOut), 0) 
                                - IFNULL(SUM(toPatientTotal.TotalToPatient), 0) 
                                - IFNULL(SUM(damageTotal.TotalDamage), 0) 
                                + IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS `Closing Stock`,
                            IFNULL(SUM(purchaseTotal.TotalPurchase), 0) AS Purchase,
                            IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) AS `Purchase Return`,
                            IFNULL(SUM(saleTotal.TotalSale), 0) AS Sales,
                            IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0) AS `Sales Return`,
                            IFNULL(SUM(stockInTotal.TotalIn), 0) AS `Stock In`,
                            IFNULL(SUM(stockOutTotal.TotalOut), 0) AS `Stock Out`,
                            IFNULL(SUM(toPatientTotal.TotalToPatient), 0) AS `To Patient`,
                            IFNULL(SUM(damageTotal.TotalDamage), 0) AS Damage,
                            IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS Adjust,
                            IFNULL(SUM(lastMonthSale.LastMonthSale), 0) AS `Last Month Sale`,
                            il.Name AS `Group Head`
                        FROM 
                            catalogitems ci
                        LEFT JOIN (
                            SELECT 
                                i.ProductId,
                                ib.BatchNo,
                                ib.ExpDate,
                                i.InventoryLocationId,
                                SUM(ib.OpeningStock) AS OpeningStock
                            FROM 
                                inventories i
                            LEFT JOIN inventorybatches ib ON i.Id = ib.InventoryId AND ib.CompanyId = p_CompanyId
                            WHERE i.CompanyId = p_CompanyId 
                            AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                            GROUP BY i.ProductId, ib.BatchNo, ib.ExpDate, i.InventoryLocationId
                        ) ib ON ci.Id = ib.ProductId
                        LEFT JOIN inventorylocations il ON ib.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                sm.InventoryStockLocationId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalPurchase
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 5 -- PURCHASE
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, sm.InventoryStockLocationId, smd.BatchNo
                        ) purchaseTotal ON ci.Id = purchaseTotal.ProductId AND ib.BatchNo = purchaseTotal.BatchNo AND ib.InventoryLocationId = purchaseTotal.InventoryStockLocationId
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalPurchaseReturn
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 7 -- PURCHASE_RETURN
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) purchaseReturnTotal ON ci.Id = purchaseReturnTotal.ProductId AND ib.BatchNo = purchaseReturnTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalSale
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 6 -- SALES
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) saleTotal ON ci.Id = saleTotal.ProductId AND ib.BatchNo = saleTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalSalesReturn
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 8 -- SALES_RETURN
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) saleReturnTotal ON ci.Id = saleReturnTotal.ProductId AND ib.BatchNo = saleReturnTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalIn
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 1 -- STOCK_IN
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) stockInTotal ON ci.Id = stockInTotal.ProductId AND ib.BatchNo = stockInTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalOut
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 2 -- STOCK_OUT
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) stockOutTotal ON ci.Id = stockOutTotal.ProductId AND ib.BatchNo = stockOutTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalToPatient
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 3 -- PATIENT_USE
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) toPatientTotal ON ci.Id = toPatientTotal.ProductId AND ib.BatchNo = toPatientTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalDamage
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 4 -- DAMAGED
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) damageTotal ON ci.Id = damageTotal.ProductId AND ib.BatchNo = damageTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalAdjust
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 9 -- ADJUSTMENT
                            AND sm.CompanyId = p_CompanyId
                            AND (FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds) OR FIND_IN_SET(sm.InventoryLocationToId, p_InventoryLocationIds))
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) adjustTotal ON ci.Id = adjustTotal.ProductId AND ib.BatchNo = adjustTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS LastMonthSale
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 6 -- SALES
                            AND sm.MovementDate BETWEEN DATE_FORMAT(NOW() - INTERVAL 1 MONTH, '%Y-%m-01') AND LAST_DAY(NOW() - INTERVAL 1 MONTH)
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) lastMonthSale ON ci.Id = lastMonthSale.ProductId AND ib.BatchNo = lastMonthSale.BatchNo
                        GROUP BY ci.Id, ib.BatchNo, ib.ExpDate, ib.InventoryLocationId, il.Name;

                        SELECT * 
                        FROM TempStockReport
                        WHERE `Opening Stock` <> 0 OR `Closing Stock` <> 0
                        ORDER BY `Group Head`;

                        DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                    END";
                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }
        public void CreateSPForStockReportWithoutBatchByCatLoctn()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                        CREATE PROCEDURE StockReportGeneratorByDateAndMultiLocationWithoutBatchForCat(
                            IN p_CompanyId BIGINT, 
                            IN p_InventoryLocationIds VARCHAR(255),
                            IN FromDate DATE,
                            IN ToDate DATE,
                            IN p_CategoryIds TEXT
                        )
                        BEGIN
                            SET @row_number := 0;

                            DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                            CREATE TEMPORARY TABLE TempStockReport (
                                SlNo INT,
                                ProductCode VARCHAR(25),
                                Name VARCHAR(50),
                                UOM VARCHAR(14),
                                RackNo VARCHAR(255),
                                `Opening Stock` DOUBLE,
                                `Closing Stock` DOUBLE,
                                Purchase DOUBLE,
                                `Purchase Return` DOUBLE,
                                Sales DOUBLE,
                                `Sales Return` DOUBLE,
                                `Stock In` DOUBLE,
                                `Stock Out` DOUBLE,
                                `To Patient` DOUBLE,
                                Damage DOUBLE,
                                Adjust DOUBLE,
                                `Last Month Sale` DOUBLE,
                                `Group Head` VARCHAR(255),
                                `Category Name` VARCHAR(50)
                            );

                            -- Fetch all products under the given categories
                            INSERT INTO TempStockReport
                            SELECT 
                                @row_number := @row_number + 1 AS SlNo,
                                ci.Id AS ProductCode,
                                ci.Name,
                                ci.UOM,
                                ci.RackNumber AS RackNo,
                                IFNULL(SUM(ib.OpeningStock), 0) AS `Opening Stock`,
                                IFNULL(SUM(ib.OpeningStock), 0) 
                                    + IFNULL(SUM(purchaseTotal.TotalPurchase), 0) 
                                    - IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) 
                                    - IFNULL(SUM(saleTotal.TotalSale), 0) 
                                    + IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0)
                                    + IFNULL(SUM(stockInTotal.TotalIn), 0) 
                                    - IFNULL(SUM(stockOutTotal.TotalOut), 0) 
                                    - IFNULL(SUM(toPatientTotal.TotalToPatient), 0) 
                                    - IFNULL(SUM(damageTotal.TotalDamage), 0) 
                                    + IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS `Closing Stock`,
                                IFNULL(SUM(purchaseTotal.TotalPurchase), 0) AS Purchase,
                                IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) AS `Purchase Return`,
                                IFNULL(SUM(saleTotal.TotalSale), 0) AS Sales,
                                IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0) AS `Sales Return`,
                                IFNULL(SUM(stockInTotal.TotalIn), 0) AS `Stock In`,
                                IFNULL(SUM(stockOutTotal.TotalOut), 0) AS `Stock Out`,
                                IFNULL(SUM(toPatientTotal.TotalToPatient), 0) AS `To Patient`,
                                IFNULL(SUM(damageTotal.TotalDamage), 0) AS Damage,
                                IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS Adjust,
                                IFNULL(SUM(lastMonthSale.LastMonthSale), 0) AS `Last Month Sale`,
                                il.Name AS `Group Head`,
                                cat.Name AS `Category Name`
                            FROM 
                                catalogitems ci
                            LEFT JOIN (
                                SELECT 
                                    i.ProductId,
                                    i.InventoryLocationId,
                                    SUM(i.OpeningStock) AS OpeningStock
                                FROM 
                                    inventories i
                                WHERE i.CompanyId = p_CompanyId 
                                AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                                GROUP BY i.ProductId, i.InventoryLocationId
                            ) ib ON ci.Id = ib.ProductId
                            LEFT JOIN inventorylocations il ON ib.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    sm.InventoryStockLocationId,
                                    SUM(smd.Quantity) AS TotalPurchase
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 5 -- PURCHASE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, sm.InventoryStockLocationId
                            ) purchaseTotal ON ci.Id = purchaseTotal.ProductId AND ib.InventoryLocationId = purchaseTotal.InventoryStockLocationId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS TotalPurchaseReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 7 -- PURCHASE_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId
                            ) purchaseReturnTotal ON ci.Id = purchaseReturnTotal.ProductId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS TotalSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId
                            ) saleTotal ON ci.Id = saleTotal.ProductId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS TotalSalesReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 8 -- SALES_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId
                            ) saleReturnTotal ON ci.Id = saleReturnTotal.ProductId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS TotalIn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 1 -- STOCK_IN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId
                            ) stockInTotal ON ci.Id = stockInTotal.ProductId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS TotalOut
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 2 -- STOCK_OUT
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId
                            ) stockOutTotal ON ci.Id = stockOutTotal.ProductId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS TotalToPatient
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 3 -- PATIENT_USE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId
                            ) toPatientTotal ON ci.Id = toPatientTotal.ProductId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS TotalDamage
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 4 -- DAMAGED
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId
                            ) damageTotal ON ci.Id = damageTotal.ProductId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS TotalAdjust
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 9 -- ADJUSTMENT
                                AND sm.CompanyId = p_CompanyId
                                AND (FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds) OR FIND_IN_SET(sm.InventoryLocationToId, p_InventoryLocationIds))
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId
                            ) adjustTotal ON ci.Id = adjustTotal.ProductId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    SUM(smd.Quantity) AS LastMonthSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN DATE_SUB(FromDate, INTERVAL 1 MONTH) AND FromDate
                                GROUP BY smd.ProductId
                            ) lastMonthSale ON ci.Id = lastMonthSale.ProductId
                            LEFT JOIN `vv-matrix`.catalogitems cat ON cat.Id = ci.ParentId
                            LEFT JOIN `vv-matrix`.catalogitems pf ON pf.Id = ci.ParentId
                            WHERE 
                                pf.ParentId IN (SELECT CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(p_CategoryIds, ',', numbers.n), ',', -1) AS UNSIGNED) FROM 
                                (SELECT @rownum := @rownum + 1 AS n FROM (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7 UNION SELECT 8 UNION SELECT 9 UNION SELECT 10) numbers INNER JOIN 
                                (SELECT @rownum := 0) r ) numbers 
                                WHERE numbers.n <= 1 + LENGTH(p_CategoryIds) - LENGTH(REPLACE(p_CategoryIds, ',', '')))
                            AND pf.CompanyId = p_CompanyId
                            GROUP BY ci.Id, ib.InventoryLocationId
                            HAVING `Opening Stock` <> 0 OR `Closing Stock` <> 0
                            ORDER BY `Category Name`, `Group Head`;

                            SELECT * FROM TempStockReport;

                            DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                        END";

                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }
        public void CreateSPForStockReportWithBatchByCatLoctn()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                        CREATE PROCEDURE StockReportGeneratorByDateAndMultiLocationWithBatchForCat(
                            IN p_CompanyId BIGINT, 
                            IN p_InventoryLocationIds VARCHAR(255),
                            IN FromDate DATE,
                            IN ToDate DATE,
                            IN p_CategoryIds TEXT
                        )
                        BEGIN
                            SET @row_number := 0;

                            DROP TEMPORARY TABLE IF EXISTS TempProductIds;
                            CREATE TEMPORARY TABLE TempProductIds (
                                ProductId BIGINT
                            );

                            SET @sql = CONCAT('INSERT INTO TempProductIds (ProductId) ',
                                              'SELECT ci.Id AS ProductId ',
                                              'FROM `vv-matrix`.catalogitems ci ',
                                              'JOIN `vv-matrix`.catalogitems pf ON ci.ParentId = pf.Id ',
                                              'WHERE pf.ParentId IN (', p_CategoryIds, ')');

                            PREPARE stmt FROM @sql;
                            EXECUTE stmt;
                            DEALLOCATE PREPARE stmt;

                            DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                            CREATE TEMPORARY TABLE TempStockReport (
                                SlNo INT,
                                ProductCode VARCHAR(25),
                                Name VARCHAR(50),
                                UOM VARCHAR(14),
                                RackNo VARCHAR(255),
                                `Batch / Expiry` VARCHAR(255),
                                `Opening Stock` DOUBLE,
                                `Closing Stock` DOUBLE,
                                Purchase DOUBLE,
                                `Purchase Return` DOUBLE,
                                Sales DOUBLE,
                                `Sales Return` DOUBLE,
                                `Stock In` DOUBLE,
                                `Stock Out` DOUBLE,
                                `To Patient` DOUBLE,
                                Damage DOUBLE,
                                Adjust DOUBLE,
                                `Last Month Sale` DOUBLE,
                                `Group Head` VARCHAR(255),
                                `Category Name` VARCHAR(50)
                            );

                            INSERT INTO TempStockReport
                            SELECT 
                                @row_number := @row_number + 1 AS SlNo,
                                ci.Id AS ProductCode,
                                ci.Name,
                                ci.UOM,
                                ci.RackNumber AS RackNo,
                                CONCAT(IFNULL(ib.BatchNo, ''), ' / ', IFNULL(DATE_FORMAT(ib.ExpDate, '%Y-%m-%d'), '')) AS `Batch / Expiry`,
                                IFNULL(SUM(ib.OpeningStock), 0) AS `Opening Stock`,
                                IFNULL(SUM(ib.OpeningStock), 0) 
                                    + IFNULL(SUM(purchaseTotal.TotalPurchase), 0) 
                                    - IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) 
                                    - IFNULL(SUM(saleTotal.TotalSale), 0) 
                                    + IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0)
                                    + IFNULL(SUM(stockInTotal.TotalIn), 0) 
                                    - IFNULL(SUM(stockOutTotal.TotalOut), 0) 
                                    - IFNULL(SUM(toPatientTotal.TotalToPatient), 0) 
                                    - IFNULL(SUM(damageTotal.TotalDamage), 0) 
                                    + IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS `Closing Stock`,
                                IFNULL(SUM(purchaseTotal.TotalPurchase), 0) AS Purchase,
                                IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) AS `Purchase Return`,
                                IFNULL(SUM(saleTotal.TotalSale), 0) AS Sales,
                                IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0) AS `Sales Return`,
                                IFNULL(SUM(stockInTotal.TotalIn), 0) AS `Stock In`,
                                IFNULL(SUM(stockOutTotal.TotalOut), 0) AS `Stock Out`,
                                IFNULL(SUM(toPatientTotal.TotalToPatient), 0) AS `To Patient`,
                                IFNULL(SUM(damageTotal.TotalDamage), 0) AS Damage,
                                IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS Adjust,
                                IFNULL(SUM(lastMonthSale.LastMonthSale), 0) AS `Last Month Sale`,
                                il.Name AS `Group Head`,
                                cat.Name AS `Category Name`
                            FROM 
                                catalogitems ci
                            LEFT JOIN (
                                SELECT 
                                    i.ProductId,
                                    ib.BatchNo,
                                    ib.ExpDate,
                                    i.InventoryLocationId,
                                    SUM(ib.OpeningStock) AS OpeningStock
                                FROM 
                                    inventories i
                                LEFT JOIN inventorybatches ib ON i.Id = ib.InventoryId AND ib.CompanyId = p_CompanyId
                                WHERE i.CompanyId = p_CompanyId 
                                AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                                GROUP BY i.ProductId, ib.BatchNo, ib.ExpDate, i.InventoryLocationId
                            ) ib ON ci.Id = ib.ProductId
                            LEFT JOIN inventorylocations il ON ib.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    sm.InventoryStockLocationId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalPurchase
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 5 -- PURCHASE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, sm.InventoryStockLocationId, smd.BatchNo
                            ) purchaseTotal ON ci.Id = purchaseTotal.ProductId AND ib.BatchNo = purchaseTotal.BatchNo AND ib.InventoryLocationId = purchaseTotal.InventoryStockLocationId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalPurchaseReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 7 -- PURCHASE_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) purchaseReturnTotal ON ci.Id = purchaseReturnTotal.ProductId AND ib.BatchNo = purchaseReturnTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) saleTotal ON ci.Id = saleTotal.ProductId AND ib.BatchNo = saleTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalSalesReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 8 -- SALES_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) saleReturnTotal ON ci.Id = saleReturnTotal.ProductId AND ib.BatchNo = saleReturnTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalIn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 1 -- STOCK_IN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) stockInTotal ON ci.Id = stockInTotal.ProductId AND ib.BatchNo = stockInTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalOut
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 2 -- STOCK_OUT
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) stockOutTotal ON ci.Id = stockOutTotal.ProductId AND ib.BatchNo = stockOutTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalToPatient
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 3 -- TO_PATIENT
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) toPatientTotal ON ci.Id = toPatientTotal.ProductId AND ib.BatchNo = toPatientTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalDamage
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 4 -- DAMAGE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) damageTotal ON ci.Id = damageTotal.ProductId AND ib.BatchNo = damageTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalAdjust
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 9 -- ADJUST
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN FromDate AND ToDate
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) adjustTotal ON ci.Id = adjustTotal.ProductId AND ib.BatchNo = adjustTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    sm.InventoryStockLocationId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS LastMonthSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                AND sm.MovementDate BETWEEN DATE_SUB(FromDate, INTERVAL 1 MONTH) AND DATE_SUB(ToDate, INTERVAL 1 MONTH)
                                GROUP BY smd.ProductId, sm.InventoryStockLocationId, smd.BatchNo
                            ) lastMonthSale ON ci.Id = lastMonthSale.ProductId AND ib.BatchNo = lastMonthSale.BatchNo
                            LEFT JOIN catalogitems cat ON ci.ParentId = cat.Id
                            WHERE 
                                ci.Id IN (SELECT ProductId FROM TempProductIds) -- Only include products in the given category IDs
                            GROUP BY 
                                ci.Id, ci.Name, ci.UOM, ci.RackNumber, ib.BatchNo, ib.ExpDate, il.Name, cat.Name
                            HAVING 
                                `Opening Stock` <> 0 OR `Closing Stock` <> 0; -- Filter out rows where both opening and closing stock are zero

                            SELECT * FROM TempStockReport;

                            DROP TEMPORARY TABLE IF EXISTS TempProductIds;
                            DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                        END";

                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }
        public void CreateSPForStockReportByDateAndLoctn()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                                    CREATE PROCEDURE StockReportGeneratorByDateAndMultiLocation(
                        IN p_CompanyId BIGINT, 
                        IN p_InventoryLocationIds VARCHAR(255),
                        IN FromDate DATE,
                        IN ToDate DATE
                    )
                    BEGIN
                        SET @row_number := 0;

                        DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                        CREATE TEMPORARY TABLE TempStockReport (
                            SlNo INT,
                            ProductCode VARCHAR(25),
                            Name VARCHAR(50),
                            UOM VARCHAR(14),
                            RackNo VARCHAR(255),
                            `Batch / Expiry` VARCHAR(255),
                            `Opening Stock` DOUBLE,
                            `Closing Stock` DOUBLE,
                            Purchase DOUBLE,
                            `Purchase Return` DOUBLE,
                            Sales DOUBLE,
                            `Sales Return` DOUBLE,
                            `Stock In` DOUBLE,
                            `Stock Out` DOUBLE,
                            `To Patient` DOUBLE,
                            Damage DOUBLE,
                            Adjust DOUBLE,
                            `Last Month Sale` DOUBLE,
                            `Group Head` VARCHAR(255)
                        );

                        INSERT INTO TempStockReport
                        SELECT 
                            @row_number := @row_number + 1 AS SlNo,
                            ci.MaterialId AS ProductCode,
                            ci.Name,
                            ci.UOM,
                            ci.RackNumber AS RackNo,
                            CONCAT(IFNULL(ib.BatchNo, ''), ' / ', IFNULL(DATE_FORMAT(ib.ExpDate, '%Y-%m-%d'), '')) AS `Batch / Expiry`,
                            IFNULL(SUM(ib.OpeningStock), 0) AS `Opening Stock`,
                            IFNULL(SUM(ib.OpeningStock), 0) 
                                + IFNULL(SUM(purchaseTotal.TotalPurchase), 0) 
                                - IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) 
                                - IFNULL(SUM(saleTotal.TotalSale), 0) 
                                + IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0)
                                + IFNULL(SUM(stockInTotal.TotalIn), 0) 
                                - IFNULL(SUM(stockOutTotal.TotalOut), 0) 
                                - IFNULL(SUM(toPatientTotal.TotalToPatient), 0) 
                                - IFNULL(SUM(damageTotal.TotalDamage), 0) 
                                + IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS `Closing Stock`,
                            IFNULL(SUM(purchaseTotal.TotalPurchase), 0) AS Purchase,
                            IFNULL(SUM(purchaseReturnTotal.TotalPurchaseReturn), 0) AS `Purchase Return`,
                            IFNULL(SUM(saleTotal.TotalSale), 0) AS Sales,
                            IFNULL(SUM(saleReturnTotal.TotalSalesReturn), 0) AS `Sales Return`,
                            IFNULL(SUM(stockInTotal.TotalIn), 0) AS `Stock In`,
                            IFNULL(SUM(stockOutTotal.TotalOut), 0) AS `Stock Out`,
                            IFNULL(SUM(toPatientTotal.TotalToPatient), 0) AS `To Patient`,
                            IFNULL(SUM(damageTotal.TotalDamage), 0) AS Damage,
                            IFNULL(SUM(adjustTotal.TotalAdjust), 0) AS Adjust,
                            IFNULL(SUM(lastMonthSale.LastMonthSale), 0) AS `Last Month Sale`,
                            il.Name AS `Group Head`
                        FROM 
                            (SELECT * FROM catalogitems WHERE CompanyId = p_CompanyId) ci
                        LEFT JOIN (
                            SELECT 
                                i.ProductId,
                                ib.BatchNo,
                                ib.ExpDate,
                                i.InventoryLocationId,
                                SUM(ib.OpeningStock) AS OpeningStock
                            FROM 
                                inventories i
                            LEFT JOIN inventorybatches ib ON i.Id = ib.InventoryId AND ib.CompanyId = p_CompanyId
                            WHERE i.CompanyId = p_CompanyId 
                            AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                            GROUP BY i.ProductId, ib.BatchNo, ib.ExpDate, i.InventoryLocationId
                        ) ib ON ci.Id = ib.ProductId
                        LEFT JOIN inventorylocations il ON ib.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalPurchase
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 5 -- PURCHASE
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) purchaseTotal ON ci.Id = purchaseTotal.ProductId AND ib.BatchNo = purchaseTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalPurchaseReturn
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 7 -- PURCHASE_RETURN
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) purchaseReturnTotal ON ci.Id = purchaseReturnTotal.ProductId AND ib.BatchNo = purchaseReturnTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalSale
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 6 -- SALES
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) saleTotal ON ci.Id = saleTotal.ProductId AND ib.BatchNo = saleTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalSalesReturn
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 8 -- SALES_RETURN
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) saleReturnTotal ON ci.Id = saleReturnTotal.ProductId AND ib.BatchNo = saleReturnTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalIn
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 1 -- STOCK_IN
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) stockInTotal ON ci.Id = stockInTotal.ProductId AND ib.BatchNo = stockInTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalOut
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 2 -- STOCK_OUT
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) stockOutTotal ON ci.Id = stockOutTotal.ProductId AND ib.BatchNo = stockOutTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalToPatient
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 3 -- PATIENT_USE
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) toPatientTotal ON ci.Id = toPatientTotal.ProductId AND ib.BatchNo = toPatientTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalDamage
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 4 -- DAMAGED
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) damageTotal ON ci.Id = damageTotal.ProductId AND ib.BatchNo = damageTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS TotalAdjust
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 9 -- ADJUSTMENT
                            AND sm.CompanyId = p_CompanyId
                            AND (FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds) OR FIND_IN_SET(sm.InventoryLocationToId, p_InventoryLocationIds))
                            AND sm.MovementDate BETWEEN FromDate AND ToDate
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) adjustTotal ON ci.Id = adjustTotal.ProductId AND ib.BatchNo = adjustTotal.BatchNo
                        LEFT JOIN (
                            SELECT 
                                smd.ProductId,
                                smd.BatchNo,
                                SUM(smd.Quantity) AS LastMonthSale
                            FROM 
                                stockmovementdetails smd
                            JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                            WHERE sm.Type = 6 -- SALES
                            AND sm.MovementDate BETWEEN DATE_FORMAT(NOW() - INTERVAL 1 MONTH, '%Y-%m-01') AND LAST_DAY(NOW() - INTERVAL 1 MONTH)
                            AND sm.CompanyId = p_CompanyId
                            AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                            GROUP BY smd.ProductId, smd.BatchNo
                        ) lastMonthSale ON ci.Id = lastMonthSale.ProductId AND ib.BatchNo = lastMonthSale.BatchNo
                        GROUP BY ci.Id, ib.BatchNo, ib.ExpDate, ib.InventoryLocationId, il.Name;

                        SELECT * 
                        FROM TempStockReport
                        WHERE `Opening Stock` <> 0 OR `Closing Stock` <> 0;

                        DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                    END";
                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }

        public void CreateSPForClosingStockReportValueByLoctn()
        {
            // ClosingStockReportValueGeneratorByMultiLocation
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                            CREATE PROCEDURE ClosingStockReportValueGeneratorByMultiLocation(
                        IN p_CompanyId BIGINT, 
                        IN p_InventoryLocationIds VARCHAR(255)
                    )
                    BEGIN
                        SET @row_number := 0;

                        DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                        CREATE TEMPORARY TABLE TempStockReport (
                            SlNo INT,
                            ProductCode VARCHAR(25),
                            Name VARCHAR(50),
                            UOM VARCHAR(14),
                            RackNo VARCHAR(255),
                            `Batch / Expiry` VARCHAR(255),
                            `Opening Stock` DOUBLE,
                            `Closing Stock` DOUBLE,
                            `Purchase Price` FLOAT,
                            `Closing Stock Value` FLOAT,
                            Purchase DOUBLE,
                            `Purchase Return` DOUBLE,
                            Sales DOUBLE,
                            `Sales Return` DOUBLE,
                            `Stock In` DOUBLE,
                            `Stock Out` DOUBLE,
                            `To Patient` DOUBLE,
                            Damage DOUBLE,
                            Adjust DOUBLE,
                            `Last Month Sale` DOUBLE,
                            `Group Head` VARCHAR(255)
                        );

                        INSERT INTO TempStockReport
                        SELECT 
                            @row_number := @row_number + 1 AS SlNo,
                            ci.MaterialId AS ProductCode,
                            ci.Name,
                            ci.UOM,
                            ci.RackNumber AS RackNo,
                            CONCAT(IFNULL(ib.BatchNo, ''), ' / ', IFNULL(DATE_FORMAT(ib.ExpDate, '%Y-%m-%d'), '')) AS `Batch / Expiry`,
                            IFNULL(ib.OpeningStock, 0) AS `Opening Stock`,
                            IFNULL(ib.OpeningStock, 0) 
                                + IFNULL(purchaseTotal.TotalPurchase, 0) 
                                - IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) 
                                - IFNULL(saleTotal.TotalSale, 0) 
                                + IFNULL(saleReturnTotal.TotalSalesReturn, 0)
                                + IFNULL(stockInTotal.TotalIn, 0) 
                                - IFNULL(stockOutTotal.TotalOut, 0) 
                                - IFNULL(toPatientTotal.TotalToPatient, 0) 
                                - IFNULL(damageTotal.TotalDamage, 0) 
                                + IFNULL(adjustTotal.TotalAdjust, 0) AS `Closing Stock`,
                            ib.PurchasePrice AS `Purchase Price`,
                            IFNULL(ib.PurchasePrice, 0) * (
                                IFNULL(ib.OpeningStock, 0) 
                                + IFNULL(purchaseTotal.TotalPurchase, 0) 
                                - IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) 
                                - IFNULL(saleTotal.TotalSale, 0) 
                                + IFNULL(saleReturnTotal.TotalSalesReturn, 0)
                                + IFNULL(stockInTotal.TotalIn, 0) 
                                - IFNULL(stockOutTotal.TotalOut, 0) 
                                - IFNULL(toPatientTotal.TotalToPatient, 0) 
                                - IFNULL(damageTotal.TotalDamage, 0) 
                                + IFNULL(adjustTotal.TotalAdjust, 0)
                            ) AS `Closing Stock Value`,
                            IFNULL(purchaseTotal.TotalPurchase, 0) AS Purchase,
                            IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) AS `Purchase Return`,
                            IFNULL(saleTotal.TotalSale, 0) AS Sales,
                            IFNULL(saleReturnTotal.TotalSalesReturn, 0) AS `Sales Return`,
                            IFNULL(stockInTotal.TotalIn, 0) AS `Stock In`,
                            IFNULL(stockOutTotal.TotalOut, 0) AS `Stock Out`,
                            IFNULL(toPatientTotal.TotalToPatient, 0) AS `To Patient`,
                            IFNULL(damageTotal.TotalDamage, 0) AS Damage,
                            IFNULL(adjustTotal.TotalAdjust, 0) AS Adjust,
                            IFNULL(lastMonthSale.LastMonthSale, 0) AS `Last Month Sale`,
                            il.Name AS `Group Head`
                        FROM 
                            (SELECT * FROM catalogitems WHERE CompanyId = p_CompanyId) ci
                        LEFT JOIN (
                            SELECT 
                                i.ProductId,
                                ib.BatchNo,
                                ib.ExpDate,
                                ib.PurchasePrice,
                                SUM(ib.OpeningStock) AS OpeningStock,
                                i.InventoryLocationId
                            FROM 
                                inventories i
                            LEFT JOIN inventorybatches ib ON i.Id = ib.InventoryId AND ib.CompanyId = p_CompanyId
                            WHERE i.CompanyId = p_CompanyId 
                            AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                            AND i.Id = ib.InventoryId
                            GROUP BY i.ProductId, ib.BatchNo, ib.ExpDate, i.InventoryLocationId
                        ) ib ON ci.Id = ib.ProductId
                        LEFT JOIN inventorylocations il ON ib.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                        -- (Other joins for purchaseTotal, purchaseReturnTotal, saleTotal, etc., remain unchanged)
                        ;

                        SELECT * 
                        FROM TempStockReport
                        WHERE `Opening Stock` <> 0 OR `Closing Stock` <> 0;

                        DROP TEMPORARY TABLE IF EXISTS TempStockReport;
                    END";

                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }
        public DataTable ExecuteDTStoredProcedure(long CompanyId, long[] LocationIds)
        {
            DataTable dataTable = new DataTable();

            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                // Execute the stored procedure and fill the result into a DataTable 
                using (MySqlDataAdapter adapter = new MySqlDataAdapter("StockReportGeneratorByMultiLocation", connection))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.AddWithValue("@p_CompanyId", CompanyId);
                    adapter.SelectCommand.Parameters.AddWithValue("@p_InventoryLocationIds", string.Join(",", LocationIds));
                    adapter.Fill(dataTable);
                }

                // Drop the stored procedure after execution
                using (MySqlCommand dropCommand = new MySqlCommand("DROP PROCEDURE IF EXISTS StockReportGeneratorByMultiLocation", connection))
                {
                    dropCommand.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Execution completed and stored procedure dropped successfully.");
            return dataTable;
        }

        
        public void CreateSPForStockReportPriceByLoctn()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                    CREATE PROCEDURE StockReportGeneratorClosingPriceByMultiLocation(
                    IN p_CompanyId BIGINT, 
                    IN p_InventoryLocationIds VARCHAR(255)
                        )
                        BEGIN
                            SET @row_number := 0;

                            DROP TEMPORARY TABLE IF EXISTS TempClosingStockValue;
                            CREATE TEMPORARY TABLE TempClosingStockValue (
                                Name VARCHAR(50),
                                ClosingStockValue DOUBLE
                            );

                            DROP TEMPORARY TABLE IF EXISTS TempClosingStockReport;
                            CREATE TEMPORARY TABLE TempClosingStockReport (
                                SlNo INT,
                                ProductCode VARCHAR(25),
                                Name VARCHAR(50),
                                UOM VARCHAR(14),
                                RackNo VARCHAR(255),
                                `Batch / Expiry` VARCHAR(255),
                                `Purchase Price` FLOAT,
                                `Opening Stock` DOUBLE,
                                `Closing Stock` DOUBLE,
                                Purchase DOUBLE,
                                `Purchase Return` DOUBLE,
                                Sales DOUBLE,
                                `Sales Return` DOUBLE,
                                `Stock In` DOUBLE,
                                `Stock Out` DOUBLE,
                                `To Patient` DOUBLE,
                                Damage DOUBLE,
                                Adjust DOUBLE,
                                `Last Month Sale` DOUBLE,
                                `Group Head` VARCHAR(255),
                                `ClosingStockValue` DOUBLE
                            );

                            INSERT INTO TempClosingStockReport
                            SELECT 
                                @row_number := @row_number + 1 AS SlNo,
                                ci.MaterialId AS ProductCode,
                                ci.Name,
                                ci.UOM,
                                ci.RackNumber AS RackNo,
                                CONCAT(IFNULL(ib.BatchNo, ''), ' / ', IFNULL(DATE_FORMAT(ib.ExpDate, '%Y-%m-%d'), '')) AS `Batch / Expiry`,
                                IFNULL(ib.PurchasePrice, 0) AS `Purchase Price`,
                                IFNULL(ib.OpeningStock, 0) AS `Opening Stock`,
                                IFNULL(ib.OpeningStock, 0) 
                                    + IFNULL(purchaseTotal.TotalPurchase, 0) 
                                    - IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) 
                                    - IFNULL(saleTotal.TotalSale, 0) 
                                    + IFNULL(saleReturnTotal.TotalSalesReturn, 0)
                                    + IFNULL(stockInTotal.TotalIn, 0) 
                                    - IFNULL(stockOutTotal.TotalOut, 0) 
                                    - IFNULL(toPatientTotal.TotalToPatient, 0) 
                                    - IFNULL(damageTotal.TotalDamage, 0) 
                                    + IFNULL(adjustTotal.TotalAdjust, 0) AS `Closing Stock`,
                                IFNULL(purchaseTotal.TotalPurchase, 0) AS Purchase,
                                IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) AS `Purchase Return`,
                                IFNULL(saleTotal.TotalSale, 0) AS Sales,
                                IFNULL(saleReturnTotal.TotalSalesReturn, 0) AS `Sales Return`,
                                IFNULL(stockInTotal.TotalIn, 0) AS `Stock In`,
                                IFNULL(stockOutTotal.TotalOut, 0) AS `Stock Out`,
                                IFNULL(toPatientTotal.TotalToPatient, 0) AS `To Patient`,
                                IFNULL(damageTotal.TotalDamage, 0) AS Damage,
                                IFNULL(adjustTotal.TotalAdjust, 0) AS Adjust,
                                IFNULL(lastMonthSale.LastMonthSale, 0) AS `Last Month Sale`,
                                il.Name AS `Group Head`,
                                IFNULL(ib.PurchasePrice, 0) * (
                                            IFNULL(ib.OpeningStock, 0) 
                                            + IFNULL(purchaseTotal.TotalPurchase, 0) 
                                            - IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) 
                                            - IFNULL(saleTotal.TotalSale, 0) 
                                            + IFNULL(saleReturnTotal.TotalSalesReturn, 0)
                                            + IFNULL(stockInTotal.TotalIn, 0) 
                                            - IFNULL(stockOutTotal.TotalOut, 0) 
                                            - IFNULL(toPatientTotal.TotalToPatient, 0) 
                                            - IFNULL(damageTotal.TotalDamage, 0) 
                                            + IFNULL(adjustTotal.TotalAdjust, 0)
                                        ) AS `ClosingStockValue`
                            FROM 
                                (SELECT * FROM catalogitems WHERE CompanyId = p_CompanyId) ci
                            LEFT JOIN (
                                SELECT 
                                    i.ProductId,
                                    ib.BatchNo,
                                    ib.ExpDate,
                                    ib.PurchasePrice,
                                    SUM(ib.OpeningStock) AS OpeningStock,
                                    i.InventoryLocationId
                                FROM 
                                    inventories i
                                LEFT JOIN inventorybatches ib ON i.Id = ib.InventoryId AND ib.CompanyId = p_CompanyId
                                WHERE i.CompanyId = p_CompanyId 
                                AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                                AND i.Id = ib.InventoryId
                                GROUP BY i.ProductId, ib.BatchNo, ib.ExpDate, ib.PurchasePrice, i.InventoryLocationId
                            ) ib ON ci.Id = ib.ProductId
                            LEFT JOIN inventorylocations il ON ib.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalPurchase
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 5 -- PURCHASE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) purchaseTotal ON ci.Id = purchaseTotal.ProductId AND ib.BatchNo = purchaseTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalPurchaseReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 7 -- PURCHASE_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) purchaseReturnTotal ON ci.Id = purchaseReturnTotal.ProductId AND ib.BatchNo = purchaseReturnTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) saleTotal ON ci.Id = saleTotal.ProductId AND ib.BatchNo = saleTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalSalesReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 8 -- SALES_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) saleReturnTotal ON ci.Id = saleReturnTotal.ProductId AND ib.BatchNo = saleReturnTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalIn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 1 -- STOCK_IN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) stockInTotal ON ci.Id = stockInTotal.ProductId AND ib.BatchNo = stockInTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalOut
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 2 -- STOCK_OUT
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) stockOutTotal ON ci.Id = stockOutTotal.ProductId AND ib.BatchNo = stockOutTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalToPatient
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 3 -- PATIENT_USE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) toPatientTotal ON ci.Id = toPatientTotal.ProductId AND ib.BatchNo = toPatientTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalDamage
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 4 -- DAMAGED
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) damageTotal ON ci.Id = damageTotal.ProductId AND ib.BatchNo = damageTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalAdjust
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 9 -- ADJUSTMENT
                                AND sm.CompanyId = p_CompanyId
                                AND (FIND_IN_SET(sm.InventoryLocationFromId, p_InventoryLocationIds) OR FIND_IN_SET(sm.InventoryLocationToId, p_InventoryLocationIds))
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) adjustTotal ON ci.Id = adjustTotal.ProductId AND ib.BatchNo = adjustTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS LastMonthSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.MovementDate BETWEEN DATE_FORMAT(NOW() - INTERVAL 1 MONTH, '%Y-%m-01') AND LAST_DAY(NOW() - INTERVAL 1 MONTH)
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryLocationFromId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) lastMonthSale ON ci.Id = lastMonthSale.ProductId AND ib.BatchNo = lastMonthSale.BatchNo;

                            -- SELECT * 
                            -- FROM TempClosingStockReport
                            -- WHERE `Opening Stock` <> 0 OR `Closing Stock` <> 0;

                            INSERT INTO TempClosingStockValue
                            SELECT ""Closing stock value"" AS Name, SUM(`ClosingStockValue`) AS ClosingStockValue
                            FROM TempClosingStockReport;

                            SELECT * FROM TempClosingStockValue;

                            DROP TEMPORARY TABLE IF EXISTS TempClosingStockReport;
                            DROP TEMPORARY TABLE IF EXISTS TempClosingStockValue;

                        END";

                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }

        
        private decimal GetClosingStockValue(MySqlConnection connection, int companyId, string startDate, string endDate)
        {
            decimal closingStockValue = 0;

            using (MySqlCommand command = new MySqlCommand("GetClosingStockValue", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters to the stored procedure
                command.Parameters.AddWithValue("@CompanyId", companyId);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                // Execute the stored procedure
                object result = command.ExecuteScalar();
                if (result != null && decimal.TryParse(result.ToString(), out decimal value))
                {
                    closingStockValue = value;
                }
            }

            return closingStockValue;
        }

        public DataTable ExecuteStoredProcedure(string StoredProcedureName, long CompanyId, long[] LocationIds, long[] CategoryIds, int QueryQ)
        {
            DataTable dataTable = new DataTable();

            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                // Execute the stored procedure and fill the result into a DataTable
                // "StockReportGeneratorByMultiLocation"
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(StoredProcedureName, connection))
                {
                    if (QueryQ == 0)
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@p_CompanyId", CompanyId);
                        adapter.SelectCommand.Parameters.AddWithValue("@p_InventoryLocationIds", string.Join(",", LocationIds));
                        adapter.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate);
                        adapter.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate);
                    }
                    else if (QueryQ == 1)
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@p_CompanyId", CompanyId);
                        adapter.SelectCommand.Parameters.AddWithValue("@p_InventoryLocationIds", string.Join(",", LocationIds));
                        adapter.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate);
                        adapter.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate);
                        adapter.SelectCommand.Parameters.AddWithValue("@p_CategoryIds", string.Join(",", CategoryIds));
                    }
                    else if(QueryQ == 2)
                    {
                        
                    }
                    
                    adapter.Fill(dataTable);

                    string DropProcedureQuery = $"DROP PROCEDURE IF EXISTS {StoredProcedureName}";

                    using (MySqlCommand dropCommand = new MySqlCommand(DropProcedureQuery, connection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }
                }
            }
            Console.WriteLine("Execution completed successfully.");
            return dataTable;
        }
        public decimal? GetFinalClosingStockValue(string StoredProcedureName, long CompanyId, long[] LocationIds, long[] CategoryIds, int QueryQ, DateTime FromDate, DateTime ToDate)
        {
            decimal? finalClosingStockValue = null;

            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                // Execute the stored procedure and fill the result into a DataTable
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(StoredProcedureName, connection))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    adapter.SelectCommand.Parameters.AddWithValue("@p_CompanyId", CompanyId);
                    adapter.SelectCommand.Parameters.AddWithValue("@p_InventoryLocationIds", string.Join(",", LocationIds));
                    adapter.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate);

                    if (QueryQ == 1)
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@p_CategoryIds", string.Join(",", CategoryIds));
                    }

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Extract FinalClosingStockValue if available
                    if (dataTable.Rows.Count > 0 && dataTable.Columns.Contains("FinalClosingStockValue"))
                    {
                        var value = dataTable.Rows[0]["FinalClosingStockValue"];
                        finalClosingStockValue = value != DBNull.Value ? Convert.ToDecimal(value) : (decimal?)null;
                    }

                    // Drop the stored procedure
                    string DropProcedureQuery = $"DROP PROCEDURE IF EXISTS {StoredProcedureName}";

                    using (MySqlCommand dropCommand = new MySqlCommand(DropProcedureQuery, connection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }
                }
            }

            Console.WriteLine("Execution completed successfully.");
            return finalClosingStockValue;
        }

        // Tryed for RptTrialBalace

        public long[] LocationIds { get; set; }
        double TotalOpeningStockValue { get; set; }
        double TotalClosingStockValue { get; set; }
        double TotalStockMovementValue { get; set; }

        public void PopulateLocationIds(long companyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<InventoryLocation> locationInfo = Context.InventoryLocation
                    .Where(x => x.CompanyId == companyId)
                    .OrderBy(x => x.Name)
                    .ToList();

                LocationIds = locationInfo.Select(x => x.Id).ToArray();
            }
        }

        public long[] CategoryIds { get; set; }

        public void PopulateCategoryIds(long companyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<CatalogItem> productCategoryInfo = Context.CatalogItems
                    .Where(x => x.CompanyId == companyId
                                && x.Type == CatalogItemType.CATEGORY
                                && x.ParentId == null)
                    .OrderBy(x => x.Name)
                    .ToList();

                CategoryIds = productCategoryInfo.Select(x => x.Id).ToArray();
            }
        }
        public void GetClosingStockValue()
        {
            long CompanyId = 0 ;
            DataTable cbdt = new DataTable();
            PopulateCategoryIds(CompanyId); // PopulateCategoryIds(Company.CompanyId);
            if (!IsStockReportPriceFileExists("StockReportGeneratorClosingPriceByMultiLocation"))
            {
                StockReportPriceByLoctn();
            }

            cbdt = ExecuteClosingStockReportPrice("StockReportGeneratorClosingPriceByMultiLocation", CompanyId, LocationIds, CategoryIds, null, 1);
            double StockMovementValue = 0;

            if (cbdt != null && cbdt.Rows.Count > 0)
            {
                if (cbdt.Rows[0][1] != DBNull.Value && cbdt.Rows[0][1] != null)
                {
                    StockMovementValue = Convert.ToDouble(cbdt.Rows[0][1]);
                    TotalStockMovementValue = StockMovementValue;
                    TotalClosingStockValue = TotalOpeningStockValue + TotalStockMovementValue;
                }
            }
        }
        public DataTable ExecuteClosingStockReportPrice(string StoredProcedureName, long CompanyId, long[] LocationIds, long[] CategoryIds, long[] CostcenterIds, int QueryQ)
        {
            DataTable dataTable = new DataTable();

            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(StoredProcedureName, connection))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    adapter.SelectCommand.Parameters.AddWithValue("@p_CompanyId", CompanyId);
                    adapter.SelectCommand.Parameters.AddWithValue("@p_InventoryLocationIds", string.Join(",", LocationIds));
                    adapter.SelectCommand.Parameters.AddWithValue("@FromDate", FromDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@ToDate", ToDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@CostCenterId", CostcenterIds);


                    if (QueryQ == 1)
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@p_CategoryIds", string.Join(",", CategoryIds));
                    }

                    adapter.Fill(dataTable);

                    string DropProcedureQuery = $"DROP PROCEDURE IF EXISTS {StoredProcedureName}";

                    using (MySqlCommand dropCommand = new MySqlCommand(DropProcedureQuery, connection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }
                }
            }
            Console.WriteLine("Execution completed successfully.");
            return dataTable;
        }

        public bool IsStockReportPriceFileExists(string StockReportPriceFileName)
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            string schemaName = "";
            if (ConString != null)
            {
                var builder = new DbConnectionStringBuilder { ConnectionString = ConString };

                if (builder.TryGetValue("Database", out var database))
                {
                    schemaName = database?.ToString();
                }
            }
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SHOW PROCEDURE STATUS WHERE Db = @databaseName AND Name = @procedureName", connection);
                command.Parameters.AddWithValue("@databaseName", schemaName);
                command.Parameters.AddWithValue("@procedureName", StockReportPriceFileName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }
        public void StockReportPriceByLoctn()
        {
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                string createProcedureScript = @"
                    CREATE PROCEDURE StockReportGeneratorClosingPriceByMultiLocation(
                    IN p_CompanyId BIGINT, 
                    IN p_InventoryLocationIds VARCHAR(255)
                        )
                        BEGIN
                            SET @row_number := 0;

                            DROP TEMPORARY TABLE IF EXISTS TempClosingStockValue;
                            CREATE TEMPORARY TABLE TempClosingStockValue (
                                Name VARCHAR(50),
                                ClosingStockValue DOUBLE
                            );

                            DROP TEMPORARY TABLE IF EXISTS TempClosingStockReport;
                            CREATE TEMPORARY TABLE TempClosingStockReport (
                                SlNo INT,
                                ProductCode VARCHAR(25),
                                Name VARCHAR(50),
                                UOM VARCHAR(14),
                                RackNo VARCHAR(255),
                                `Batch / Expiry` VARCHAR(255),
                                `Purchase Price` FLOAT,
                                `Opening Stock` DOUBLE,
                                `Closing Stock` DOUBLE,
                                Purchase DOUBLE,
                                `Purchase Return` DOUBLE,
                                Sales DOUBLE,
                                `Sales Return` DOUBLE,
                                `Stock In` DOUBLE,
                                `Stock Out` DOUBLE,
                                `To Patient` DOUBLE,
                                Damage DOUBLE,
                                Adjust DOUBLE,
                                `Last Month Sale` DOUBLE,
                                `Group Head` VARCHAR(255),
                                `ClosingStockValue` DOUBLE
                            );

                            INSERT INTO TempClosingStockReport
                            SELECT 
                                @row_number := @row_number + 1 AS SlNo,
                                ci.MaterialId AS ProductCode,
                                ci.Name,
                                ci.UOM,
                                ci.RackNumber AS RackNo,
                                CONCAT(IFNULL(ib.BatchNo, ''), ' / ', IFNULL(DATE_FORMAT(ib.ExpDate, '%Y-%m-%d'), '')) AS `Batch / Expiry`,
                                IFNULL(ib.PurchasePrice, 0) AS `Purchase Price`,
                                IFNULL(ib.OpeningStock, 0) AS `Opening Stock`,
                                IFNULL(ib.OpeningStock, 0) 
                                    + IFNULL(purchaseTotal.TotalPurchase, 0) 
                                    - IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) 
                                    - IFNULL(saleTotal.TotalSale, 0) 
                                    + IFNULL(saleReturnTotal.TotalSalesReturn, 0)
                                    + IFNULL(stockInTotal.TotalIn, 0) 
                                    - IFNULL(stockOutTotal.TotalOut, 0) 
                                    - IFNULL(toPatientTotal.TotalToPatient, 0) 
                                    - IFNULL(damageTotal.TotalDamage, 0) 
                                    + IFNULL(adjustTotal.TotalAdjust, 0) AS `Closing Stock`,
                                IFNULL(purchaseTotal.TotalPurchase, 0) AS Purchase,
                                IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) AS `Purchase Return`,
                                IFNULL(saleTotal.TotalSale, 0) AS Sales,
                                IFNULL(saleReturnTotal.TotalSalesReturn, 0) AS `Sales Return`,
                                IFNULL(stockInTotal.TotalIn, 0) AS `Stock In`,
                                IFNULL(stockOutTotal.TotalOut, 0) AS `Stock Out`,
                                IFNULL(toPatientTotal.TotalToPatient, 0) AS `To Patient`,
                                IFNULL(damageTotal.TotalDamage, 0) AS Damage,
                                IFNULL(adjustTotal.TotalAdjust, 0) AS Adjust,
                                IFNULL(lastMonthSale.LastMonthSale, 0) AS `Last Month Sale`,
                                il.Name AS `Group Head`,
                                IFNULL(ib.PurchasePrice, 0) * (
                                            IFNULL(ib.OpeningStock, 0) 
                                            + IFNULL(purchaseTotal.TotalPurchase, 0) 
                                            - IFNULL(purchaseReturnTotal.TotalPurchaseReturn, 0) 
                                            - IFNULL(saleTotal.TotalSale, 0) 
                                            + IFNULL(saleReturnTotal.TotalSalesReturn, 0)
                                            + IFNULL(stockInTotal.TotalIn, 0) 
                                            - IFNULL(stockOutTotal.TotalOut, 0) 
                                            - IFNULL(toPatientTotal.TotalToPatient, 0) 
                                            - IFNULL(damageTotal.TotalDamage, 0) 
                                            + IFNULL(adjustTotal.TotalAdjust, 0)
                                        ) AS `ClosingStockValue`
                            FROM 
                                (SELECT * FROM catalogitems WHERE CompanyId = p_CompanyId) ci
                            LEFT JOIN (
                                SELECT 
                                    i.ProductId,
                                    ib.BatchNo,
                                    ib.ExpDate,
                                    ib.PurchasePrice,
                                    SUM(ib.OpeningStock) AS OpeningStock,
                                    i.InventoryLocationId
                                FROM 
                                    inventories i
                                LEFT JOIN inventorybatches ib ON i.Id = ib.InventoryId AND ib.CompanyId = p_CompanyId
                                WHERE i.CompanyId = p_CompanyId 
                                AND FIND_IN_SET(i.InventoryLocationId, p_InventoryLocationIds)
                                AND i.Id = ib.InventoryId
                                GROUP BY i.ProductId, ib.BatchNo, ib.ExpDate, ib.PurchasePrice, i.InventoryLocationId
                            ) ib ON ci.Id = ib.ProductId
                            LEFT JOIN inventorylocations il ON ib.InventoryLocationId = il.Id AND il.CompanyId = p_CompanyId
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalPurchase
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 5 -- PURCHASE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) purchaseTotal ON ci.Id = purchaseTotal.ProductId AND ib.BatchNo = purchaseTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalPurchaseReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 7 -- PURCHASE_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) purchaseReturnTotal ON ci.Id = purchaseReturnTotal.ProductId AND ib.BatchNo = purchaseReturnTotal.BatchNo
                            LEFT JOIN (
                                 SELECT 
                                        smd.ProductId, smd.BatchNo, SUM(smd.Quantity) AS TotalSale
                                    FROM
                                        stockmovementdetails smd
                                            JOIN
                                        stockmovements sm ON smd.StockMovementId = sm.Id
                                    WHERE
                                        sm.Type = 6 AND sm.CompanyId = p_CompanyId
                                        AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                    GROUP BY smd.ProductId , smd.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalSalesReturn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 8 -- SALES_RETURN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) saleReturnTotal ON ci.Id = saleReturnTotal.ProductId AND ib.BatchNo = saleReturnTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalIn
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 1 -- STOCK_IN
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) stockInTotal ON ci.Id = stockInTotal.ProductId AND ib.BatchNo = stockInTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalOut
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 2 -- STOCK_OUT
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) stockOutTotal ON ci.Id = stockOutTotal.ProductId AND ib.BatchNo = stockOutTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalToPatient
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 3 -- PATIENT_USE
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) toPatientTotal ON ci.Id = toPatientTotal.ProductId AND ib.BatchNo = toPatientTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalDamage
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 4 -- DAMAGED
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) damageTotal ON ci.Id = damageTotal.ProductId AND ib.BatchNo = damageTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS TotalAdjust
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 9 -- ADJUSTMENT
                                AND sm.CompanyId = p_CompanyId
                                AND (FIND_IN_SET(sm.InventoryLocationFromId, p_InventoryLocationIds) OR FIND_IN_SET(sm.InventoryLocationToId, p_InventoryLocationIds))
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) adjustTotal ON ci.Id = adjustTotal.ProductId AND ib.BatchNo = adjustTotal.BatchNo
                            LEFT JOIN (
                                SELECT 
                                    smd.ProductId,
                                    smd.BatchNo,
                                    SUM(smd.Quantity) AS LastMonthSale
                                FROM 
                                    stockmovementdetails smd
                                JOIN stockmovements sm ON smd.StockMovementId = sm.Id
                                WHERE sm.Type = 6 -- SALES
                                AND sm.MovementDate BETWEEN DATE_FORMAT(NOW() - INTERVAL 1 MONTH, '%Y-%m-01') AND LAST_DAY(NOW() - INTERVAL 1 MONTH)
                                AND sm.CompanyId = p_CompanyId
                                AND FIND_IN_SET(sm.InventoryStockLocationId, p_InventoryLocationIds)
                                GROUP BY smd.ProductId, smd.BatchNo
                            ) lastMonthSale ON ci.Id = lastMonthSale.ProductId AND ib.BatchNo = lastMonthSale.BatchNo;


                            -- SELECT * 
                            -- FROM TempClosingStockReport
                            -- WHERE `Opening Stock` <> 0 OR `Closing Stock` <> 0;

                            INSERT INTO TempClosingStockValue
                            SELECT ""Closing stock value"" AS Name, SUM(`ClosingStockValue`) AS ClosingStockValue
                            FROM TempClosingStockReport;

                            SELECT * FROM TempClosingStockValue;

                            DROP TEMPORARY TABLE IF EXISTS TempClosingStockReport;
                            DROP TEMPORARY TABLE IF EXISTS TempClosingStockValue;

                        END";

                using (MySqlCommand cmd = new MySqlCommand(createProcedureScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Stored procedure created successfully.");
            }
        }

    }
}
