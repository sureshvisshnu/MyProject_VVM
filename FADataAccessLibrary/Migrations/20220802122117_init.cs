using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "accountgroupclassifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DebitMultiplier = table.Column<long>(type: "bigint", nullable: false),
                    CreditMultiplier = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountgroupclassifications", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "accountgroupforhelps",
                columns: table => new
                {
                    AccountGroupForHelpId = table.Column<long>(type: "bigint", nullable: false),
                    HelpGroupDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountgroupforhelps", x => x.AccountGroupForHelpId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountingMethods",
                columns: table => new
                {
                    AccountingMethodId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingMethods", x => x.AccountingMethodId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddressLine1 = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressLine2 = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CityOrTown = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    District = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PinCode = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StateName = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanyPurchaseSetups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPurchaseSetups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanySalesSetups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IncludingTax = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DefaultSalesType = table.Column<int>(type: "int", nullable: false),
                    CombineItem = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsReceivePayment = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDelivery = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsNegativeStockAllowed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsBankDetailDisplayOnInvoice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeclarationDisplayOnInvoice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BankDetails = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Declarations = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PriceType = table.Column<int>(type: "int", nullable: false),
                    SaleTaxType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySalesSetups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanyStockMovementSetups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStockMovementSetups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanyTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "contactinfoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mobile = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fax = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WebSite = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactinfoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyCodeISO = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoundingPrecision = table.Column<int>(type: "int", nullable: false),
                    CurrencyFormat = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IdSpaceEntryTypeDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntryType = table.Column<int>(type: "int", nullable: false),
                    IsDotMatrix = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsResetDaily = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RoundOff = table.Column<double>(type: "double", nullable: false),
                    HasPrinterSetup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasRoundOff = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasDotMatrix = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Prefix = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdSpaceEntryTypeDetails", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientHistoryQuestionGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ParentGroupId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientHistoryQuestionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientHistoryQuestionGroups_PatientHistoryQuestionGroups_Pa~",
                        column: x => x.ParentGroupId,
                        principalTable: "PatientHistoryQuestionGroups",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientIds",
                columns: table => new
                {
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientIds", x => new { x.CompanyId, x.Date });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrintPaperFormats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintPaperFormats", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SeedDataHistories",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateExecuted = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeedDataHistories", x => x.Key);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SystemFunctions",
                columns: table => new
                {
                    FunctionId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuViewId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemFunctions", x => x.FunctionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "taxdocumenttypes",
                columns: table => new
                {
                    TaxTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taxdocumenttypes", x => x.TaxTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "taxinfoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PAN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CST = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GST = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIN1 = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taxinfoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => new { x.CompanyId, x.Date });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccountGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSubType = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentAccountGroupId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpenBalance = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TrackDepriciation = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccountClassificationId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountGroups_accountgroupclassifications_AccountClassificat~",
                        column: x => x.AccountClassificationId,
                        principalTable: "accountgroupclassifications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountGroups_AccountGroups_ParentAccountGroupId",
                        column: x => x.ParentAccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISOCode2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISOCode3 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultCurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultDateFormat = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultPhoneFormat = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DefaultMobileFormat = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccountingMethodId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyTypeId = table.Column<long>(type: "bigint", nullable: true),
                    AccountingStartDate = table.Column<int>(type: "int", nullable: false),
                    IncomeTaxStartDate = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_AccountingMethods_AccountingMethodId",
                        column: x => x.AccountingMethodId,
                        principalTable: "AccountingMethods",
                        principalColumn: "AccountingMethodId");
                    table.ForeignKey(
                        name: "FK_Countries_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Countries_Currencies_DefaultCurrencyId",
                        column: x => x.DefaultCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientHistoryQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    ValueType = table.Column<int>(type: "int", nullable: false),
                    AdditionalNotes = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AdditionalNotesCaption = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientHistoryQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientHistoryQuestions_PatientHistoryQuestionGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "PatientHistoryQuestionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rolesystemfunction",
                columns: table => new
                {
                    FunctionId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolesystemfunction", x => new { x.FunctionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_rolesystemfunction_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rolesystemfunction_SystemFunctions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "SystemFunctions",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "accountgroupandaccountgrouphelp",
                columns: table => new
                {
                    AccountGroupHelpId = table.Column<long>(type: "bigint", nullable: false),
                    AccountGroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountgroupandaccountgrouphelp", x => new { x.AccountGroupHelpId, x.AccountGroupId });
                    table.ForeignKey(
                        name: "FK_accountgroupandaccountgrouphelp_accountgroupforhelps_Account~",
                        column: x => x.AccountGroupHelpId,
                        principalTable: "accountgroupforhelps",
                        principalColumn: "AccountGroupForHelpId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_accountgroupandaccountgrouphelp_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "countrytax",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TaxTypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countrytax", x => new { x.Id, x.TaxTypeId });
                    table.ForeignKey(
                        name: "FK_countrytax_Countries_Id",
                        column: x => x.Id,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_countrytax_taxdocumenttypes_TaxTypeId",
                        column: x => x.TaxTypeId,
                        principalTable: "taxdocumenttypes",
                        principalColumn: "TaxTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISOCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accesses",
                columns: table => new
                {
                    AccessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesses", x => x.AccessId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSubAccount = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentAccountId = table.Column<long>(type: "bigint", nullable: true),
                    AccountGroupId = table.Column<long>(type: "bigint", nullable: true),
                    Balance = table.Column<float>(type: "float", nullable: false),
                    BalanceAsOf = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_ParentAccountId",
                        column: x => x.ParentAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanyAdditionalTransactionSetups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TransactionAction = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAdditionalTransactionSetups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAdditionalTransactionSetups_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    AddressId = table.Column<long>(type: "bigint", nullable: true),
                    ContactInfoId = table.Column<long>(type: "bigint", nullable: true),
                    TaxInfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suppliers_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Suppliers_contactinfoes_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "contactinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Suppliers_taxinfoes_TaxInfoId",
                        column: x => x.TaxInfoId,
                        principalTable: "taxinfoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "purchaseadditionaltransaction",
                columns: table => new
                {
                    AddTransactionSetupId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseSetupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchaseadditionaltransaction", x => new { x.AddTransactionSetupId, x.PurchaseSetupId });
                    table.ForeignKey(
                        name: "FK_purchaseadditionaltransaction_CompanyAdditionalTransactionSe~",
                        column: x => x.AddTransactionSetupId,
                        principalTable: "CompanyAdditionalTransactionSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_purchaseadditionaltransaction_CompanyPurchaseSetups_Purchase~",
                        column: x => x.PurchaseSetupId,
                        principalTable: "CompanyPurchaseSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "salesadditionaltransaction",
                columns: table => new
                {
                    AddTransactionSetupId = table.Column<long>(type: "bigint", nullable: false),
                    SalesSetupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesadditionaltransaction", x => new { x.AddTransactionSetupId, x.SalesSetupId });
                    table.ForeignKey(
                        name: "FK_salesadditionaltransaction_CompanyAdditionalTransactionSetup~",
                        column: x => x.AddTransactionSetupId,
                        principalTable: "CompanyAdditionalTransactionSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_salesadditionaltransaction_CompanySalesSetups_SalesSetupId",
                        column: x => x.SalesSetupId,
                        principalTable: "CompanySalesSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdditionalDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    SourceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Detail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalDetails", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdditionalTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "double", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PurchaseEntryId = table.Column<long>(type: "bigint", nullable: true),
                    SaleEntryId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalTransactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Beds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WardId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BedTypeId = table.Column<long>(type: "bigint", nullable: true),
                    BedStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beds", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BedTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BillAttachments",
                columns: table => new
                {
                    BillAttachementId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BillId = table.Column<long>(type: "bigint", nullable: false),
                    Attachment = table.Column<byte[]>(type: "longblob", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillAttachments", x => x.BillAttachementId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    BillDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BillId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => x.BillDetailId);
                    table.ForeignKey(
                        name: "FK_BillDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TermId = table.Column<long>(type: "bigint", nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    Memo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InternalMemo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Total = table.Column<float>(type: "float", nullable: false),
                    Paid = table.Column<float>(type: "float", nullable: false),
                    Balance = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bills_Accounts_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CatalogItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    _shouldMaintainInventory = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    _isInventoryAtBatch = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    DefaultDiscountLocal = table.Column<float>(type: "float", nullable: true),
                    SalesAccountLocalId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseAccountLocalId = table.Column<long>(type: "bigint", nullable: true),
                    DiscountAccountLocalId = table.Column<long>(type: "bigint", nullable: true),
                    InventoryAccountLocalId = table.Column<long>(type: "bigint", nullable: true),
                    Manufacturer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    SupplierName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItems_Accounts_DiscountAccountLocalId",
                        column: x => x.DiscountAccountLocalId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogItems_Accounts_InventoryAccountLocalId",
                        column: x => x.InventoryAccountLocalId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogItems_Accounts_PurchaseAccountLocalId",
                        column: x => x.PurchaseAccountLocalId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogItems_Accounts_SalesAccountLocalId",
                        column: x => x.SalesAccountLocalId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CatalogItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogItems_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_CatalogItems_Id",
                        column: x => x.Id,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductFamilies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFamilies_CatalogItems_Id",
                        column: x => x.Id,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ProductFamilyId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialId = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HSNCode = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UOM = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RetailUOM = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RetailXFactor = table.Column<int>(type: "int", nullable: false),
                    WholesaleUOM = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WholesaleXFactor = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<float>(type: "float", nullable: false),
                    CostPrice = table.Column<float>(type: "float", nullable: false),
                    RetailPrice = table.Column<float>(type: "float", nullable: false),
                    WholdSalePrice = table.Column<float>(type: "float", nullable: false),
                    Msrp = table.Column<float>(type: "float", nullable: false),
                    QuantityOnHand = table.Column<double>(type: "double", nullable: false),
                    UseHsnTax = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_CatalogItems_Id",
                        column: x => x.Id,
                        principalTable: "CatalogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductFamilies_ProductFamilyId",
                        column: x => x.ProductFamilyId,
                        principalTable: "ProductFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CatalogItemSalesTaxMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CatalogItemId = table.Column<long>(type: "bigint", nullable: true),
                    SalesTaxMapId = table.Column<long>(type: "bigint", nullable: true),
                    TaxPercentage = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItemSalesTaxMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogItemSalesTaxMaps_CatalogItems_CatalogItemId",
                        column: x => x.CatalogItemId,
                        principalTable: "CatalogItems",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    IsBranch = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentCompanyId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyTypeId = table.Column<long>(type: "bigint", nullable: true),
                    AccountingMethodId = table.Column<long>(type: "bigint", nullable: true),
                    AddressId = table.Column<long>(type: "bigint", nullable: true),
                    ContactInfoId = table.Column<long>(type: "bigint", nullable: true),
                    TaxInfoId = table.Column<long>(type: "bigint", nullable: true),
                    AccountingStartDate = table.Column<int>(type: "int", nullable: false),
                    IncomeTaxStartDate = table.Column<int>(type: "int", nullable: false),
                    DateFormat = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneFormat = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MobileFormat = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrimaryCurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    Logo = table.Column<byte[]>(type: "longblob", nullable: true),
                    CashOnHandAccountId = table.Column<long>(type: "bigint", nullable: true),
                    UndepositedFundAccountId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseAccountId = table.Column<long>(type: "bigint", nullable: true),
                    SalesAccountId = table.Column<long>(type: "bigint", nullable: true),
                    SalesReturnFeeAccountId = table.Column<long>(type: "bigint", nullable: true),
                    AccountRecivableId = table.Column<long>(type: "bigint", nullable: true),
                    AccountPayableId = table.Column<long>(type: "bigint", nullable: true),
                    IncomceAccountId = table.Column<long>(type: "bigint", nullable: true),
                    ExpenseAccountId = table.Column<long>(type: "bigint", nullable: true),
                    HasProductCatalog = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QuantityPricision = table.Column<int>(type: "int", nullable: false),
                    CompanyPurchaseSetupId = table.Column<long>(type: "bigint", nullable: true),
                    CompanySalesSetupId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyStockMovementSetupId = table.Column<long>(type: "bigint", nullable: true),
                    DayBookType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BusinessType = table.Column<int>(type: "int", nullable: false),
                    AllowWholSale = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PatientPurchaseAccountId = table.Column<long>(type: "bigint", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Companies_AccountingMethods_AccountingMethodId",
                        column: x => x.AccountingMethodId,
                        principalTable: "AccountingMethods",
                        principalColumn: "AccountingMethodId");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_AccountPayableId",
                        column: x => x.AccountPayableId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_AccountRecivableId",
                        column: x => x.AccountRecivableId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_CashOnHandAccountId",
                        column: x => x.CashOnHandAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_ExpenseAccountId",
                        column: x => x.ExpenseAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_IncomceAccountId",
                        column: x => x.IncomceAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_PurchaseAccountId",
                        column: x => x.PurchaseAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_SalesAccountId",
                        column: x => x.SalesAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_SalesReturnFeeAccountId",
                        column: x => x.SalesReturnFeeAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Accounts_UndepositedFundAccountId",
                        column: x => x.UndepositedFundAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Companies_Companies_ParentCompanyId",
                        column: x => x.ParentCompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_Companies_CompanyPurchaseSetups_CompanyPurchaseSetupId",
                        column: x => x.CompanyPurchaseSetupId,
                        principalTable: "CompanyPurchaseSetups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_CompanySalesSetups_CompanySalesSetupId",
                        column: x => x.CompanySalesSetupId,
                        principalTable: "CompanySalesSetups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_CompanyStockMovementSetups_CompanyStockMovementSet~",
                        column: x => x.CompanyStockMovementSetupId,
                        principalTable: "CompanyStockMovementSetups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_contactinfoes_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "contactinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_Currencies_PrimaryCurrencyId",
                        column: x => x.PrimaryCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId");
                    table.ForeignKey(
                        name: "FK_Companies_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_taxinfoes_TaxInfoId",
                        column: x => x.TaxInfoId,
                        principalTable: "taxinfoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanyFinancialPeriods",
                columns: table => new
                {
                    PeriodsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Begin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    End = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Closed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyFinancialPeriods", x => x.PeriodsId);
                    table.ForeignKey(
                        name: "FK_CompanyFinancialPeriods_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanySalesTaxAccountMaps",
                columns: table => new
                {
                    MapId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySalesTaxAccountMaps", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_CompanySalesTaxAccountMaps_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanySalesTaxAccountMaps_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fee = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CostCenters",
                columns: table => new
                {
                    CostCenterId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenters", x => x.CostCenterId);
                    table.ForeignKey(
                        name: "FK_CostCenters_Companies_ParentCompanyId",
                        column: x => x.ParentCompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSubDepartment = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentDepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentCategories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentCategories_DocumentCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DocumentCategories",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HospitalConfigurations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DefaultOPConsultingFee = table.Column<double>(type: "double", nullable: false),
                    OPRegistrationFeeAccountId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultIPConsultingFee = table.Column<double>(type: "double", nullable: false),
                    IPRegistrationFeeAccountId = table.Column<long>(type: "bigint", nullable: true),
                    HasInvoicingGroup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HospitalConfigurations_Accounts_IPRegistrationFeeAccountId",
                        column: x => x.IPRegistrationFeeAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HospitalConfigurations_Accounts_OPRegistrationFeeAccountId",
                        column: x => x.OPRegistrationFeeAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HospitalConfigurations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IdSpaces",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntryType = table.Column<int>(type: "int", nullable: false),
                    PrintPaperFormat_Id = table.Column<long>(type: "bigint", nullable: true),
                    IsDotMatrix = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsResetDaily = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RoundOff = table.Column<double>(type: "double", nullable: false),
                    Seed = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Prefix = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    YearStartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    YearEndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RunningSeed = table.Column<long>(type: "bigint", nullable: false),
                    HasPrinterSetup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasRoundOff = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasDotMatrix = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdSpaces_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdSpaces_PrintPaperFormats_PrintPaperFormat_Id",
                        column: x => x.PrintPaperFormat_Id,
                        principalTable: "PrintPaperFormats",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "itemtaxes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemtaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_itemtaxes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "licenseinfoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncludeInInvoice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IncludeInReport = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Required = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Required1 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_licenseinfoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_licenseinfoes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicalProcedures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasElement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Fee = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalProcedures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalProcedures_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicalTests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasElement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalTests_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicalTestUOMs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTestUOMs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalTestUOMs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Narrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Narrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Narrations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientLedgerTransactionTypeGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLedgerTransactionTypeGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientLedgerTransactionTypeGroups_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreditCard = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PaymentTerms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FixedDays = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NoOfDays = table.Column<int>(type: "int", nullable: false),
                    DueOnDate = table.Column<int>(type: "int", nullable: false),
                    LeadPeriodToDue = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTerms_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MiddleInitial = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    TaxId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Occupation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Income = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Employer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AddressId = table.Column<long>(type: "bigint", nullable: true),
                    ContactInfoId = table.Column<long>(type: "bigint", nullable: true),
                    Photo = table.Column<byte[]>(type: "longblob", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Persons_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persons_contactinfoes_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "contactinfoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Refereds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refereds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refereds_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BedTypeId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    RentPeriod = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rents_BedTypes_BedTypeId",
                        column: x => x.BedTypeId,
                        principalTable: "BedTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rents_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codes = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Class = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReasonForInactive = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Symptoms_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Titles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "daybooks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    ReferenceTrasnactionId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LedgerOnly = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daybooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_daybooks_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_daybooks_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_daybooks_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DebitNotes",
                columns: table => new
                {
                    DebitNoteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReferenceNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InternalNote = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    Paid = table.Column<float>(type: "float", nullable: false),
                    Balance = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebitNotes", x => x.DebitNoteId);
                    table.ForeignKey(
                        name: "FK_DebitNotes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DebitNotes_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_DebitNotes_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Reference = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayeeId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TransctionType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    Memo = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankTransferId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CashAccountId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true),
                    CCAccountId = table.Column<long>(type: "bigint", nullable: true),
                    CCTransactionNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CCTransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_BankTransferId",
                        column: x => x.BankTransferId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_CashAccountId",
                        column: x => x.CashAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_CCAccountId",
                        column: x => x.CCAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Expenses_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inventorylocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventorylocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventorylocations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventorylocations_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Journals",
                columns: table => new
                {
                    JournalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReferenceNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Memo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journals", x => x.JournalId);
                    table.ForeignKey(
                        name: "FK_Journals_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Journals_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reference = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    TransctionType = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankTransferId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DepositedIntoId = table.Column<long>(type: "bigint", nullable: true),
                    DepositedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CCAccountId = table.Column<long>(type: "bigint", nullable: true),
                    CCTransactionNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CCTransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_Accounts_BankTransferId",
                        column: x => x.BankTransferId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_Accounts_CCAccountId",
                        column: x => x.CCAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_Accounts_DepositedIntoId",
                        column: x => x.DepositedIntoId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reference = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankTransferId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstitutionName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DepositedIntoId = table.Column<long>(type: "bigint", nullable: true),
                    DepositedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CCAccountId = table.Column<long>(type: "bigint", nullable: true),
                    CCTransactionNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CCTransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK_Receipts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Receipts_Accounts_BankTransferId",
                        column: x => x.BankTransferId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Receipts_Accounts_CCAccountId",
                        column: x => x.CCAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Receipts_Accounts_DepositedIntoId",
                        column: x => x.DepositedIntoId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Receipts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Receipts_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItemSalesTaxMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemTaxId = table.Column<long>(type: "bigint", nullable: true),
                    SalesTaxMapId = table.Column<long>(type: "bigint", nullable: true),
                    TaxPercentage = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSalesTaxMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSalesTaxMaps_CompanySalesTaxAccountMaps_SalesTaxMapId",
                        column: x => x.SalesTaxMapId,
                        principalTable: "CompanySalesTaxAccountMaps",
                        principalColumn: "MapId");
                    table.ForeignKey(
                        name: "FK_ItemSalesTaxMaps_itemtaxes_ItemTaxId",
                        column: x => x.ItemTaxId,
                        principalTable: "itemtaxes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SupplierLicenceDetails",
                columns: table => new
                {
                    SupplierLicenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanySupplierLicenseMasterId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierLicenceDetails", x => x.SupplierLicenceId);
                    table.ForeignKey(
                        name: "FK_SupplierLicenceDetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierLicenceDetails_licenseinfoes_CompanySupplierLicenseM~",
                        column: x => x.CompanySupplierLicenseMasterId,
                        principalTable: "licenseinfoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierLicenceDetails_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicalProcedureElements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MedicalProcedureId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fee = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalProcedureElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalProcedureElements_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalProcedureElements_MedicalProcedures_MedicalProcedureId",
                        column: x => x.MedicalProcedureId,
                        principalTable: "MedicalProcedures",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicalTestElements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MedicalTestId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HiAbsolute = table.Column<double>(type: "double", nullable: false),
                    HiNormal = table.Column<double>(type: "double", nullable: false),
                    HiCritical = table.Column<double>(type: "double", nullable: false),
                    LowAbsolute = table.Column<double>(type: "double", nullable: false),
                    LowNormal = table.Column<double>(type: "double", nullable: false),
                    LowCritical = table.Column<double>(type: "double", nullable: false),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTestElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalTestElements_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalTestElements_MedicalTests_MedicalTestId",
                        column: x => x.MedicalTestId,
                        principalTable: "MedicalTests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalTestElements_MedicalTestUOMs_UomId",
                        column: x => x.UomId,
                        principalTable: "MedicalTestUOMs",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientLedgerTransactionTypeGroupMappings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientLedgerTransactionTypeGroupId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLedgerTransactionTypeGroupMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientLedgerTransactionTypeGroupMappings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientLedgerTransactionTypeGroupMappings_PatientLedgerTrans~",
                        column: x => x.PatientLedgerTransactionTypeGroupId,
                        principalTable: "PatientLedgerTransactionTypeGroups",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BillingAddressId = table.Column<long>(type: "bigint", nullable: true),
                    ShippingAddressId = table.Column<long>(type: "bigint", nullable: true),
                    ContactInfoId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayNameOnCheck = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentTermId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_Addresses_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Customers_Addresses_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Customers_contactinfoes_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "contactinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_PaymentTerms_PaymentTermId",
                        column: x => x.PaymentTermId,
                        principalTable: "PaymentTerms",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TermId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    Memo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InternalMemo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Total = table.Column<float>(type: "float", nullable: false),
                    Paid = table.Column<float>(type: "float", nullable: false),
                    Balance = table.Column<float>(type: "float", nullable: false),
                    Discount = table.Column<float>(type: "float", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Accounts_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_Invoices_PaymentTerms_TermId",
                        column: x => x.TermId,
                        principalTable: "PaymentTerms",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PatientNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResponsiblePartyId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeceased = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Persons_Id",
                        column: x => x.Id,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_Persons_ResponsiblePartyId",
                        column: x => x.ResponsiblePartyId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedicalProcedureId = table.Column<long>(type: "bigint", nullable: true),
                    MedicalTestId = table.Column<long>(type: "bigint", nullable: true),
                    SymptomId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keywords_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Keywords_MedicalProcedures_MedicalProcedureId",
                        column: x => x.MedicalProcedureId,
                        principalTable: "MedicalProcedures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Keywords_MedicalTests_MedicalTestId",
                        column: x => x.MedicalTestId,
                        principalTable: "MedicalTests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Keywords_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    TitleId = table.Column<long>(type: "bigint", nullable: true),
                    AddressId = table.Column<long>(type: "bigint", nullable: true),
                    ContactInfoId = table.Column<long>(type: "bigint", nullable: true),
                    TaxInfoId = table.Column<long>(type: "bigint", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_employees_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employees_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_employees_contactinfoes_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "contactinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_employees_taxinfoes_TaxInfoId",
                        column: x => x.TaxInfoId,
                        principalTable: "taxinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_employees_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DebitNoteDetails",
                columns: table => new
                {
                    DebitNoteDetailsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DebitNoteId = table.Column<long>(type: "bigint", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebitNoteDetails", x => x.DebitNoteDetailsId);
                    table.ForeignKey(
                        name: "FK_DebitNoteDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DebitNoteDetails_DebitNotes_DebitNoteId",
                        column: x => x.DebitNoteId,
                        principalTable: "DebitNotes",
                        principalColumn: "DebitNoteId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExpenseDetails",
                columns: table => new
                {
                    ExpenseDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExpenseId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseDetails", x => x.ExpenseDetailId);
                    table.ForeignKey(
                        name: "FK_ExpenseDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseDetails_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "ExpenseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Purchased = table.Column<double>(type: "double", nullable: false),
                    Sold = table.Column<double>(type: "double", nullable: false),
                    OpeningStock = table.Column<double>(type: "double", nullable: false),
                    In = table.Column<double>(type: "double", nullable: false),
                    Out = table.Column<double>(type: "double", nullable: false),
                    Damage = table.Column<double>(type: "double", nullable: false),
                    ToPatient = table.Column<double>(type: "double", nullable: false),
                    Adjust = table.Column<double>(type: "double", nullable: false),
                    StockUOM = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InventoryLocationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_Inventories_inventorylocations_InventoryLocationId",
                        column: x => x.InventoryLocationId,
                        principalTable: "inventorylocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "purchaseentries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    SupplierName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupplierAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PurchaseMethod = table.Column<int>(type: "int", nullable: false),
                    RefDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PurchaseInvNumber = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PurchaseInvDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<double>(type: "double", nullable: false),
                    NetAmount = table.Column<double>(type: "double", nullable: false),
                    TaxAmount = table.Column<double>(type: "double", nullable: false),
                    DisAmount = table.Column<double>(type: "double", nullable: false),
                    RoundOff = table.Column<double>(type: "double", nullable: false),
                    Paid = table.Column<double>(type: "double", nullable: false),
                    Balance = table.Column<double>(type: "double", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InventoryLocationId = table.Column<long>(type: "bigint", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PurchaseEntryId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseEntrytype = table.Column<int>(type: "int", nullable: false),
                    isPurchaseEntryLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchaseentries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchaseentries_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_purchaseentries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_purchaseentries_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_purchaseentries_inventorylocations_InventoryLocationId",
                        column: x => x.InventoryLocationId,
                        principalTable: "inventorylocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_purchaseentries_purchaseentries_PurchaseEntryId",
                        column: x => x.PurchaseEntryId,
                        principalTable: "purchaseentries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaintainInventory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InventoryLocationId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wards_inventorylocations_InventoryLocationId",
                        column: x => x.InventoryLocationId,
                        principalTable: "inventorylocations",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JournalDetails",
                columns: table => new
                {
                    JournalDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JournalId = table.Column<long>(type: "bigint", nullable: true),
                    ToAccountId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    ReferenceAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalDetails", x => x.JournalDetailId);
                    table.ForeignKey(
                        name: "FK_JournalDetails_Accounts_ReferenceAccountId",
                        column: x => x.ReferenceAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JournalDetails_Accounts_ToAccountId",
                        column: x => x.ToAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JournalDetails_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "JournalId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    PaymentDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PaymentId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    InvoiceType = table.Column<int>(type: "int", nullable: false),
                    ReferenceTrasnactionId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.PaymentDetailId);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "saleentries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountsId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaleMethod = table.Column<int>(type: "int", nullable: false),
                    SaleType = table.Column<int>(type: "int", nullable: false),
                    SaleTaxType = table.Column<int>(type: "int", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<double>(type: "double", nullable: false),
                    NetAmount = table.Column<double>(type: "double", nullable: false),
                    TaxAmount = table.Column<double>(type: "double", nullable: false),
                    DisAmount = table.Column<double>(type: "double", nullable: false),
                    RoundOff = table.Column<double>(type: "double", nullable: false),
                    Paid = table.Column<double>(type: "double", nullable: false),
                    Balance = table.Column<double>(type: "double", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Memo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntryType = table.Column<int>(type: "int", nullable: false),
                    QuotaionExpireAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaymentId = table.Column<long>(type: "bigint", nullable: true),
                    isPaymentReceived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    hasDelivered = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    isSaleLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SaleEntryId = table.Column<long>(type: "bigint", nullable: true),
                    SoldById = table.Column<long>(type: "bigint", nullable: true),
                    ReferedById = table.Column<long>(type: "bigint", nullable: true),
                    InventoryLocationId = table.Column<long>(type: "bigint", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true),
                    WorkStationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkStationName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saleentries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_saleentries_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_saleentries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saleentries_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_saleentries_inventorylocations_InventoryLocationId",
                        column: x => x.InventoryLocationId,
                        principalTable: "inventorylocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_saleentries_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId");
                    table.ForeignKey(
                        name: "FK_saleentries_Refereds_ReferedById",
                        column: x => x.ReferedById,
                        principalTable: "Refereds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_saleentries_Refereds_SoldById",
                        column: x => x.SoldById,
                        principalTable: "Refereds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_saleentries_saleentries_SaleEntryId",
                        column: x => x.SaleEntryId,
                        principalTable: "saleentries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_saleentries_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReceiptDetails",
                columns: table => new
                {
                    ReceiptDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReceiptId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    InvoiceType = table.Column<int>(type: "int", nullable: false),
                    ReferenceTrasnactionId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptDetails", x => x.ReceiptDetailId);
                    table.ForeignKey(
                        name: "FK_ReceiptDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptDetails_Receipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalTable: "Receipts",
                        principalColumn: "ReceiptId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreditNotes",
                columns: table => new
                {
                    CreditNoteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReferenceNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InternalNote = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    Paid = table.Column<float>(type: "float", nullable: false),
                    Balance = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditNotes", x => x.CreditNoteId);
                    table.ForeignKey(
                        name: "FK_CreditNotes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditNotes_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_CreditNotes_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerLicenceDetails",
                columns: table => new
                {
                    CustomerLicenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyCustomerLicenseMasterId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLicenceDetails", x => x.CustomerLicenceId);
                    table.ForeignKey(
                        name: "FK_CustomerLicenceDetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerLicenceDetails_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerLicenceDetails_licenseinfoes_CompanyCustomerLicenseM~",
                        column: x => x.CompanyCustomerLicenseMasterId,
                        principalTable: "licenseinfoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    InvoiceDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    SalesAccountId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<float>(type: "float", nullable: false),
                    Total = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.InvoiceDetailId);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Accounts_SalesAccountId",
                        column: x => x.SalesAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmergencyContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RelationShip = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyContacts_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmergencyContacts_Persons_Id",
                        column: x => x.Id,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Guardians",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RelationShip = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guardians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guardians_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Guardians_Persons_Id",
                        column: x => x.Id,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "insuranceinfoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InsuranceHolderId = table.Column<long>(type: "bigint", nullable: false),
                    InsuranceHolderRelationShip = table.Column<int>(type: "int", nullable: false),
                    PolicyNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InsuranceName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployerName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployerAddressId = table.Column<long>(type: "bigint", nullable: true),
                    EmployerContactInfoId = table.Column<long>(type: "bigint", nullable: true),
                    EmployerPhone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdditionalInformation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPrimary = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    IPInsuranceCoverage = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    OPInsuranceCoverage = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insuranceinfoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_insuranceinfoes_Addresses_EmployerAddressId",
                        column: x => x.EmployerAddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_insuranceinfoes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_insuranceinfoes_contactinfoes_EmployerContactInfoId",
                        column: x => x.EmployerContactInfoId,
                        principalTable: "contactinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_insuranceinfoes_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_insuranceinfoes_Persons_InsuranceHolderId",
                        column: x => x.InsuranceHolderId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    PatientDocumentCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    File = table.Column<byte[]>(type: "longblob", nullable: true),
                    FileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_DocumentCategories_PatientDocumentCategoryId",
                        column: x => x.PatientDocumentCategoryId,
                        principalTable: "DocumentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientInvoices",
                columns: table => new
                {
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Total = table.Column<double>(type: "double", nullable: false),
                    Paid = table.Column<double>(type: "double", nullable: false),
                    Balance = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInvoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_PatientInvoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientInvoices_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientPreMedicalHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HistoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    HistoryItemValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdditionalValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPreMedicalHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientPreMedicalHistories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPreMedicalHistories_PatientHistoryQuestions_HistoryIt~",
                        column: x => x.HistoryItemId,
                        principalTable: "PatientHistoryQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPreMedicalHistories_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Dosage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Days = table.Column<int>(type: "int", nullable: false),
                    TakeDosage = table.Column<int>(type: "int", nullable: false),
                    Hours = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Morning = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Afternoon = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Evening = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Night = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PrescriptionNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdditionalNotes = table.Column<string>(type: "varchar(3072)", maxLength: 3072, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrescribedByDoctorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescriptions_employees_PrescribedByDoctorId",
                        column: x => x.PrescribedByDoctorId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prescriptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Login = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsResetPassword = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AddressId = table.Column<long>(type: "bigint", nullable: true),
                    ContactInfoId = table.Column<long>(type: "bigint", nullable: true),
                    TaxInfoId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Users_contactinfoes_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "contactinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_taxinfoes_TaxInfoId",
                        column: x => x.TaxInfoId,
                        principalTable: "taxinfoes",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InventoryBatches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InventoryId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    BatchNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Purchased = table.Column<double>(type: "double", nullable: false),
                    Sold = table.Column<double>(type: "double", nullable: false),
                    OpeningStock = table.Column<double>(type: "double", nullable: false),
                    In = table.Column<double>(type: "double", nullable: false),
                    Out = table.Column<double>(type: "double", nullable: false),
                    Damage = table.Column<double>(type: "double", nullable: false),
                    ToPatient = table.Column<double>(type: "double", nullable: false),
                    Adjust = table.Column<double>(type: "double", nullable: false),
                    RetailUOM = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RetailXFactor = table.Column<int>(type: "int", nullable: false),
                    WholesaleUOM = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WholesaleXFactor = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<float>(type: "float", nullable: false),
                    Cost = table.Column<float>(type: "float", nullable: false),
                    RetailSalePrice = table.Column<float>(type: "float", nullable: false),
                    WholeSalePrice = table.Column<float>(type: "float", nullable: false),
                    MaxRetailPrice = table.Column<float>(type: "float", nullable: false),
                    StockUOM = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryBatches_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_InventoryBatches_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryBatches_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchaseAttachments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PurchaseEntryId = table.Column<long>(type: "bigint", nullable: true),
                    Attachment = table.Column<byte[]>(type: "longblob", nullable: true),
                    FileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseAttachments_purchaseentries_PurchaseEntryId",
                        column: x => x.PurchaseEntryId,
                        principalTable: "purchaseentries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PurchaseDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PurchaseEntryId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<double>(type: "double", nullable: false),
                    FreeQuantity = table.Column<double>(type: "double", nullable: false),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    isFree = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PurchasePrice = table.Column<float>(type: "float", nullable: false),
                    PurchaseCost = table.Column<float>(type: "float", nullable: false),
                    MaterialId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isBatch = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BatchNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PurchaseDetailsId = table.Column<long>(type: "bigint", nullable: true),
                    ReturnFee = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_PurchaseDetails_PurchaseDetailsId",
                        column: x => x.PurchaseDetailsId,
                        principalTable: "PurchaseDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_purchaseentries_PurchaseEntryId",
                        column: x => x.PurchaseEntryId,
                        principalTable: "purchaseentries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "saledetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaleId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<double>(type: "double", nullable: false),
                    FreeQuantity = table.Column<double>(type: "double", nullable: false),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    isFree = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Price = table.Column<float>(type: "float", nullable: false),
                    Msrp = table.Column<float>(type: "float", nullable: false),
                    OverridePrice = table.Column<float>(type: "float", nullable: false),
                    ReturnFee = table.Column<float>(type: "float", nullable: false),
                    OverrideenBy = table.Column<float>(type: "float", nullable: false),
                    MaterialId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isBatch = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BatchNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SaleDetailId = table.Column<long>(type: "bigint", nullable: true),
                    Uom = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true),
                    WorkStationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkStationName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saledetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_saledetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saledetails_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_saledetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_saledetails_saledetails_SaleDetailId",
                        column: x => x.SaleDetailId,
                        principalTable: "saledetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_saledetails_saleentries_SaleId",
                        column: x => x.SaleId,
                        principalTable: "saleentries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stockmovements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MovementDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    InventoryStockLocationId = table.Column<long>(type: "bigint", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InventoryLocationFromId = table.Column<long>(type: "bigint", nullable: true),
                    StockMovementOutId = table.Column<long>(type: "bigint", nullable: true),
                    InventoryLocationToId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseEntryId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseReturnEntryId = table.Column<long>(type: "bigint", nullable: true),
                    RequestInventoryLocationId = table.Column<long>(type: "bigint", nullable: true),
                    HasRequestCompleted = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    IsResponsed = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    StockOutId = table.Column<long>(type: "bigint", nullable: true),
                    SaleId = table.Column<long>(type: "bigint", nullable: true),
                    SaleReturnId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true),
                    WorkStationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkStationName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockmovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stockmovements_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stockmovements_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_stockmovements_inventorylocations_InventoryLocationFromId",
                        column: x => x.InventoryLocationFromId,
                        principalTable: "inventorylocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stockmovements_inventorylocations_InventoryLocationToId",
                        column: x => x.InventoryLocationToId,
                        principalTable: "inventorylocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stockmovements_inventorylocations_InventoryStockLocationId",
                        column: x => x.InventoryStockLocationId,
                        principalTable: "inventorylocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stockmovements_inventorylocations_RequestInventoryLocationId",
                        column: x => x.RequestInventoryLocationId,
                        principalTable: "inventorylocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stockmovements_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stockmovements_purchaseentries_PurchaseEntryId",
                        column: x => x.PurchaseEntryId,
                        principalTable: "purchaseentries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stockmovements_purchaseentries_PurchaseReturnEntryId",
                        column: x => x.PurchaseReturnEntryId,
                        principalTable: "purchaseentries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stockmovements_saleentries_SaleId",
                        column: x => x.SaleId,
                        principalTable: "saleentries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stockmovements_saleentries_SaleReturnId",
                        column: x => x.SaleReturnId,
                        principalTable: "saleentries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stockmovements_stockmovements_StockMovementOutId",
                        column: x => x.StockMovementOutId,
                        principalTable: "stockmovements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stockmovements_stockmovements_StockOutId",
                        column: x => x.StockOutId,
                        principalTable: "stockmovements",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreditNoteDetails",
                columns: table => new
                {
                    CreditNoteDetailsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreditNoteId = table.Column<long>(type: "bigint", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditNoteDetails", x => x.CreditNoteDetailsId);
                    table.ForeignKey(
                        name: "FK_CreditNoteDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CreditNoteDetails_CreditNotes_CreditNoteId",
                        column: x => x.CreditNoteId,
                        principalTable: "CreditNotes",
                        principalColumn: "CreditNoteId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "registrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    DateOfRegistration = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReasonForTheVisit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TockenNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestedDoctorId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    HasConsulted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsNurseActivitiesCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsTechnicianActivitiesCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsFeePaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsFeeInsurance = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasRegistrationFeePaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RegistrationFee = table.Column<double>(type: "double", nullable: false),
                    IsBillToInsurance = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InsuranceInfoId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_registrations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_registrations_employees_RequestedDoctorId",
                        column: x => x.RequestedDoctorId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_registrations_insuranceinfoes_InsuranceInfoId",
                        column: x => x.InsuranceInfoId,
                        principalTable: "insuranceinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_registrations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultedDoctorConsultationFees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsultantId = table.Column<long>(type: "bigint", nullable: true),
                    ConsultationId = table.Column<long>(type: "bigint", nullable: true),
                    Fee = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultedDoctorConsultationFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultedDoctorConsultationFees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedDoctorConsultationFees_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultedDoctorConsultationFees_Users_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userrole",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userrole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_userrole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userrole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DiscountDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiscountSequence = table.Column<int>(type: "int", nullable: false),
                    DisccountType = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<float>(type: "float", nullable: false),
                    DiscountAmount = table.Column<float>(type: "float", nullable: false),
                    DiscountDescription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaleDetailsId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseDetailsId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseEntryId = table.Column<long>(type: "bigint", nullable: true),
                    SaleId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountDetails_PurchaseDetails_PurchaseDetailsId",
                        column: x => x.PurchaseDetailsId,
                        principalTable: "PurchaseDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountDetails_purchaseentries_PurchaseEntryId",
                        column: x => x.PurchaseEntryId,
                        principalTable: "purchaseentries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountDetails_saledetails_SaleDetailsId",
                        column: x => x.SaleDetailsId,
                        principalTable: "saledetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountDetails_saleentries_SaleId",
                        column: x => x.SaleId,
                        principalTable: "saleentries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TaxDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaxSequence = table.Column<int>(type: "int", nullable: false),
                    TaxAccountId = table.Column<long>(type: "bigint", nullable: true),
                    TaxRate = table.Column<float>(type: "float", nullable: false),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaleDetailsId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseDetailsId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseEntryId = table.Column<long>(type: "bigint", nullable: true),
                    SaleId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxDetails_Accounts_TaxAccountId",
                        column: x => x.TaxAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaxDetails_PurchaseDetails_PurchaseDetailsId",
                        column: x => x.PurchaseDetailsId,
                        principalTable: "PurchaseDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaxDetails_purchaseentries_PurchaseEntryId",
                        column: x => x.PurchaseEntryId,
                        principalTable: "purchaseentries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaxDetails_saledetails_SaleDetailsId",
                        column: x => x.SaleDetailsId,
                        principalTable: "saledetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaxDetails_saleentries_SaleId",
                        column: x => x.SaleId,
                        principalTable: "saleentries",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stockmovementdetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StockMovementId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<double>(type: "double", nullable: false),
                    isFree = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FreeQuantity = table.Column<double>(type: "double", nullable: false),
                    MaterialId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isBatch = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BatchNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Uom = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true),
                    WorkStationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkStationName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockmovementdetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stockmovementdetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stockmovementdetails_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "CostCenterId");
                    table.ForeignKey(
                        name: "FK_stockmovementdetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_stockmovementdetails_stockmovements_StockMovementId",
                        column: x => x.StockMovementId,
                        principalTable: "stockmovements",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InPatientAdmissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    DateOfAdmission = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AdmissionNote = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OpRegistrationId = table.Column<long>(type: "bigint", nullable: false),
                    IsBillToInsurance = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InsuranceInfoId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InPatientAdmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InPatientAdmissions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InPatientAdmissions_insuranceinfoes_InsuranceInfoId",
                        column: x => x.InsuranceInfoId,
                        principalTable: "insuranceinfoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InPatientAdmissions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InPatientAdmissions_registrations_OpRegistrationId",
                        column: x => x.OpRegistrationId,
                        principalTable: "registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultationNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConsultantId = table.Column<long>(type: "bigint", nullable: false),
                    IsPrescriptionDispatchedForMedical = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPrescriptionDone = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SaleEntryId = table.Column<long>(type: "bigint", nullable: true),
                    OpRegistrationId = table.Column<long>(type: "bigint", nullable: true),
                    InPatientAdmissionId = table.Column<long>(type: "bigint", nullable: true),
                    Fees = table.Column<double>(type: "double", nullable: false),
                    IsDischarged = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsInvoiced = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultationNotes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultationNotes_InPatientAdmissions_InPatientAdmissionId",
                        column: x => x.InPatientAdmissionId,
                        principalTable: "InPatientAdmissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultationNotes_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultationNotes_registrations_OpRegistrationId",
                        column: x => x.OpRegistrationId,
                        principalTable: "registrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultationNotes_saleentries_SaleEntryId",
                        column: x => x.SaleEntryId,
                        principalTable: "saleentries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultationNotes_Users_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DischargeNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    InPatientAdmissionId = table.Column<long>(type: "bigint", nullable: false),
                    DiagnosisSummary = table.Column<string>(type: "varchar(3072)", maxLength: 3072, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TreatmentSummary = table.Column<string>(type: "varchar(3072)", maxLength: 3072, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DischargeSummary = table.Column<string>(type: "varchar(3072)", maxLength: 3072, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DischargeOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NextFollowUp = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Waive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeceased = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DischargeNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DischargeNotes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DischargeNotes_employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DischargeNotes_InPatientAdmissions_InPatientAdmissionId",
                        column: x => x.InPatientAdmissionId,
                        principalTable: "InPatientAdmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DischargeNotes_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InPatientLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdmissionId = table.Column<long>(type: "bigint", nullable: false),
                    DateMovedIn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateMovedOut = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WardId = table.Column<long>(type: "bigint", nullable: true),
                    BedId = table.Column<long>(type: "bigint", nullable: true),
                    AuthorizedByDoctorId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InPatientLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InPatientLocations_Beds_BedId",
                        column: x => x.BedId,
                        principalTable: "Beds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InPatientLocations_employees_AuthorizedByDoctorId",
                        column: x => x.AuthorizedByDoctorId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InPatientLocations_InPatientAdmissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "InPatientAdmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InPatientLocations_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicalTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdmissionId = table.Column<long>(type: "bigint", nullable: false),
                    PrimaryDoctorId = table.Column<long>(type: "bigint", nullable: true),
                    SecondaryDoctorId = table.Column<long>(type: "bigint", nullable: true),
                    PrimaryCareGiverId = table.Column<long>(type: "bigint", nullable: true),
                    SecondaryCareGiverId = table.Column<long>(type: "bigint", nullable: true),
                    From = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    To = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AuthorizedByDoctorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalTeams_employees_AuthorizedByDoctorId",
                        column: x => x.AuthorizedByDoctorId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalTeams_employees_PrimaryCareGiverId",
                        column: x => x.PrimaryCareGiverId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalTeams_employees_PrimaryDoctorId",
                        column: x => x.PrimaryDoctorId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalTeams_employees_SecondaryCareGiverId",
                        column: x => x.SecondaryCareGiverId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalTeams_employees_SecondaryDoctorId",
                        column: x => x.SecondaryDoctorId,
                        principalTable: "employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalTeams_InPatientAdmissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "InPatientAdmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vitals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    BMI = table.Column<float>(type: "float", nullable: false),
                    Temperature = table.Column<float>(type: "float", nullable: false),
                    Pulse = table.Column<int>(type: "int", nullable: false),
                    RespRate = table.Column<int>(type: "int", nullable: false),
                    BPressure = table.Column<int>(type: "int", nullable: false),
                    BPressureOver = table.Column<int>(type: "int", nullable: false),
                    BOxyLevel = table.Column<int>(type: "int", nullable: false),
                    OpRegistrationId = table.Column<long>(type: "bigint", nullable: true),
                    InPatientAdmissionId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vitals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vitals_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vitals_InPatientAdmissions_InPatientAdmissionId",
                        column: x => x.InPatientAdmissionId,
                        principalTable: "InPatientAdmissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vitals_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vitals_registrations_OpRegistrationId",
                        column: x => x.OpRegistrationId,
                        principalTable: "registrations",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultedConsultationFees",
                columns: table => new
                {
                    ConsultedConsultationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsultationNoteId = table.Column<long>(type: "bigint", nullable: false),
                    ConsultationId = table.Column<long>(type: "bigint", nullable: false),
                    ConsultantId = table.Column<long>(type: "bigint", nullable: true),
                    IsOverrideFee = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultedConsultationFees", x => x.ConsultedConsultationId);
                    table.ForeignKey(
                        name: "FK_ConsultedConsultationFees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedConsultationFees_ConsultationNotes_ConsultationNote~",
                        column: x => x.ConsultationNoteId,
                        principalTable: "ConsultationNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedConsultationFees_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedConsultationFees_Users_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultedLabTests",
                columns: table => new
                {
                    ConsultedLabTestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsultationNoteId = table.Column<long>(type: "bigint", nullable: false),
                    MedicalTestId = table.Column<long>(type: "bigint", nullable: false),
                    HasElement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultedLabTests", x => x.ConsultedLabTestId);
                    table.ForeignKey(
                        name: "FK_ConsultedLabTests_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedLabTests_ConsultationNotes_ConsultationNoteId",
                        column: x => x.ConsultationNoteId,
                        principalTable: "ConsultationNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedLabTests_MedicalTests_MedicalTestId",
                        column: x => x.MedicalTestId,
                        principalTable: "MedicalTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultedPrescriptions",
                columns: table => new
                {
                    ConsultedPrescriptionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsultationNoteId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultedPrescriptions", x => x.ConsultedPrescriptionId);
                    table.ForeignKey(
                        name: "FK_ConsultedPrescriptions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedPrescriptions_ConsultationNotes_ConsultationNoteId",
                        column: x => x.ConsultationNoteId,
                        principalTable: "ConsultationNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedPrescriptions_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultedProcedures",
                columns: table => new
                {
                    ConsultedProcedureId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsultationNoteId = table.Column<long>(type: "bigint", nullable: false),
                    MedicalProcedureId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProStatus = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fees = table.Column<double>(type: "double", nullable: false),
                    PerformOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PerformedById = table.Column<long>(type: "bigint", nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RequestedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultedProcedures", x => x.ConsultedProcedureId);
                    table.ForeignKey(
                        name: "FK_ConsultedProcedures_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedProcedures_ConsultationNotes_ConsultationNoteId",
                        column: x => x.ConsultationNoteId,
                        principalTable: "ConsultationNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedProcedures_MedicalProcedures_MedicalProcedureId",
                        column: x => x.MedicalProcedureId,
                        principalTable: "MedicalProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedProcedures_Users_PerformedById",
                        column: x => x.PerformedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_ConsultedProcedures_Users_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultedSymptoms",
                columns: table => new
                {
                    ConsultedSymptomId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsultationNoteId = table.Column<long>(type: "bigint", nullable: false),
                    SymptomId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultedSymptoms", x => x.ConsultedSymptomId);
                    table.ForeignKey(
                        name: "FK_ConsultedSymptoms_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedSymptoms_ConsultationNotes_ConsultationNoteId",
                        column: x => x.ConsultationNoteId,
                        principalTable: "ConsultationNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedSymptoms_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DischargePrescriptions",
                columns: table => new
                {
                    DischargePrescriptionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DischargeNoteId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DischargePrescriptions", x => x.DischargePrescriptionId);
                    table.ForeignKey(
                        name: "FK_DischargePrescriptions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DischargePrescriptions_DischargeNotes_DischargeNoteId",
                        column: x => x.DischargeNoteId,
                        principalTable: "DischargeNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DischargePrescriptions_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConsultedLabTestElements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsLabTestId = table.Column<long>(type: "bigint", nullable: false),
                    MedicalTestElementId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HiAbsolute = table.Column<double>(type: "double", nullable: false),
                    HiNormal = table.Column<double>(type: "double", nullable: false),
                    HiCritical = table.Column<double>(type: "double", nullable: false),
                    ObservedHi = table.Column<double>(type: "double", nullable: false),
                    LowAbsolute = table.Column<double>(type: "double", nullable: false),
                    LowNormal = table.Column<double>(type: "double", nullable: false),
                    LowCritical = table.Column<double>(type: "double", nullable: false),
                    ObservedLow = table.Column<double>(type: "double", nullable: false),
                    UomId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultedLabTestElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultedLabTestElements_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedLabTestElements_ConsultedLabTests_ConsLabTestId",
                        column: x => x.ConsLabTestId,
                        principalTable: "ConsultedLabTests",
                        principalColumn: "ConsultedLabTestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedLabTestElements_MedicalTestElements_MedicalTestElem~",
                        column: x => x.MedicalTestElementId,
                        principalTable: "MedicalTestElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedLabTestElements_MedicalTestUOMs_UomId",
                        column: x => x.UomId,
                        principalTable: "MedicalTestUOMs",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LabTestAttachments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsLabTestId = table.Column<long>(type: "bigint", nullable: false),
                    Attachment = table.Column<byte[]>(type: "longblob", nullable: true),
                    FileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabTestAttachments_ConsultedLabTests_ConsLabTestId",
                        column: x => x.ConsLabTestId,
                        principalTable: "ConsultedLabTests",
                        principalColumn: "ConsultedLabTestId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "consultedprocedurehistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsultedProcedureId = table.Column<long>(type: "bigint", nullable: false),
                    ProStatus = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PerformOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PerformedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultedprocedurehistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_consultedprocedurehistories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_consultedprocedurehistories_ConsultedProcedures_ConsultedPro~",
                        column: x => x.ConsultedProcedureId,
                        principalTable: "ConsultedProcedures",
                        principalColumn: "ConsultedProcedureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_consultedprocedurehistories_Users_PerformedById",
                        column: x => x.PerformedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientLedgers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    RefNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ConsultantId = table.Column<long>(type: "bigint", nullable: true),
                    OpRegistrationId = table.Column<long>(type: "bigint", nullable: true),
                    InPatientAdmissionId = table.Column<long>(type: "bigint", nullable: true),
                    ConsultationNoteId = table.Column<long>(type: "bigint", nullable: true),
                    ConsultedConsultationId = table.Column<long>(type: "bigint", nullable: true),
                    ConsultedProcedureId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Invoiced = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PatientInvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientLedgers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientLedgers_ConsultationNotes_ConsultationNoteId",
                        column: x => x.ConsultationNoteId,
                        principalTable: "ConsultationNotes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgers_ConsultedConsultationFees_ConsultedConsultati~",
                        column: x => x.ConsultedConsultationId,
                        principalTable: "ConsultedConsultationFees",
                        principalColumn: "ConsultedConsultationId");
                    table.ForeignKey(
                        name: "FK_PatientLedgers_ConsultedProcedures_ConsultedProcedureId",
                        column: x => x.ConsultedProcedureId,
                        principalTable: "ConsultedProcedures",
                        principalColumn: "ConsultedProcedureId");
                    table.ForeignKey(
                        name: "FK_PatientLedgers_InPatientAdmissions_InPatientAdmissionId",
                        column: x => x.InPatientAdmissionId,
                        principalTable: "InPatientAdmissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgers_PatientInvoices_PatientInvoiceId",
                        column: x => x.PatientInvoiceId,
                        principalTable: "PatientInvoices",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_PatientLedgers_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientLedgers_registrations_OpRegistrationId",
                        column: x => x.OpRegistrationId,
                        principalTable: "registrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgers_Users_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientInvoicePayments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    LedgerId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInvoicePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientInvoicePayments_PatientInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "PatientInvoices",
                        principalColumn: "InvoiceId");
                    table.ForeignKey(
                        name: "FK_PatientInvoicePayments_PatientLedgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "PatientLedgers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientPaymentDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientLedgerId = table.Column<long>(type: "bigint", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    DocumentNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientPaymentDetails_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPaymentDetails_PatientLedgers_PatientLedgerId",
                        column: x => x.PatientLedgerId,
                        principalTable: "PatientLedgers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AccountingMethods",
                columns: new[] { "AccountingMethodId", "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1L, null, null, null, null, "Cash" },
                    { 2L, null, null, null, null, "Accrual" }
                });

            migrationBuilder.InsertData(
                table: "CompanyTypes",
                columns: new[] { "Id", "Description", "DisplayAs", "Name" },
                values: new object[,]
                {
                    { 1L, "Sole Proprietorship company", "Proprietorship", "Proprietorship" },
                    { 2L, "One or more partnership company", "Partnership", "Partnership" },
                    { 3L, "Limited Company", "Limited Company", "Limited" },
                    { 4L, "Private Limited Company", "Prinvate Limited Company", "Private Limited" }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "CurrencyId", "CreatedBy", "CreatedDate", "CurrencyCodeISO", "CurrencyFormat", "DisplayAs", "LastModifiedBy", "LastModifiedDate", "Name", "RoundingPrecision" },
                values: new object[,]
                {
                    { 1L, null, null, "BDT", "", "BDT", null, null, "Taka", 2 },
                    { 2L, null, null, "LKR", "", "LKR", null, null, "Sri Lanka Rupee", 2 },
                    { 3L, null, null, "PKR", "", "PKRLKR", null, null, "Pakistan Rupee", 2 },
                    { 165L, null, null, "INR", "#,##,##,##0.00", "INR", null, null, "Indian Rupee", 2 }
                });

            migrationBuilder.InsertData(
                table: "IdSpaceEntryTypeDetails",
                columns: new[] { "Id", "EntryType", "HasDotMatrix", "HasPrinterSetup", "HasRoundOff", "IsDotMatrix", "IsResetDaily", "Prefix", "RoundOff" },
                values: new object[,]
                {
                    { 1L, 0, true, true, true, false, false, "", 0.0 },
                    { 2L, 1, true, true, true, false, false, "", 0.0 },
                    { 3L, 2, true, true, true, false, false, "", 0.0 },
                    { 4L, 3, true, true, true, false, false, "", 0.0 },
                    { 5L, 5, true, true, true, false, false, "", 0.0 },
                    { 6L, 4, true, true, true, false, false, "", 0.0 },
                    { 7L, 6, true, true, true, false, false, "", 0.0 },
                    { 8L, 7, true, true, true, false, false, "", 0.0 },
                    { 9L, 11, true, true, true, false, false, "", 0.0 },
                    { 10L, 12, true, true, true, false, false, "", 0.0 },
                    { 11L, 13, true, true, true, false, false, "", 0.0 },
                    { 12L, 8, false, false, true, false, false, "", 0.0 },
                    { 13L, 10, false, false, true, false, false, "", 0.0 },
                    { 14L, 9, false, false, true, false, false, "", 0.0 },
                    { 15L, 14, false, true, false, false, false, "", 0.0 },
                    { 16L, 15, false, true, false, false, false, "", 0.0 },
                    { 17L, 20, false, false, false, false, false, "", 0.0 },
                    { 18L, 21, false, false, false, false, false, "", 0.0 },
                    { 19L, 23, false, false, false, false, false, "", 0.0 },
                    { 20L, 22, false, false, false, false, false, "", 0.0 },
                    { 21L, 24, false, false, false, false, false, "", 0.0 },
                    { 22L, 18, false, true, false, false, false, "", 0.0 },
                    { 23L, 16, false, true, true, false, false, "", 0.0 },
                    { 24L, 19, false, true, true, false, false, "", 0.0 },
                    { 25L, 17, false, true, false, false, false, "", 0.0 },
                    { 26L, 25, false, true, false, false, false, "", 0.0 },
                    { 27L, 26, false, true, false, false, false, "", 0.0 },
                    { 28L, 27, false, true, false, false, false, "", 0.0 }
                });

            migrationBuilder.InsertData(
                table: "PatientHistoryQuestionGroups",
                columns: new[] { "Id", "Name", "Order", "ParentGroupId" },
                values: new object[,]
                {
                    { 1L, "Medical History (Do you have now or before?)", 1, null },
                    { 2L, "System Review", 2, null }
                });

            migrationBuilder.InsertData(
                table: "PrintPaperFormats",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "105 MM ROLL" },
                    { 2L, "A4 PORTRAIT" },
                    { 3L, "A4 LANDSCAPE" },
                    { 4L, "A5 PORTRAIT" },
                    { 5L, "A5 LANDSCAPE" },
                    { 6L, "80 MM ROLL" }
                });


            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AddressId", "ContactInfoId", "CreatedBy", "CreatedDate", "EmployeeId", "FirstName", "IsLocked", "IsResetPassword", "IsSuperAdmin", "LastModifiedBy", "LastModifiedDate", "LastName", "Login", "Password", "TaxInfoId" },
                values: new object[] { 1L, null, null, null, null, null, "Admin", false, true, true, null, null, "User", "admin", "5wiGU7luIvOPsyEa58V7+A==", null });

            migrationBuilder.InsertData(
                table: "accountgroupclassifications",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "CreditMultiplier", "DebitMultiplier", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1L, null, null, 1L, -1L, null, null, "Income" },
                    { 2L, null, null, -1L, 1L, null, null, "Expense" },
                    { 3L, null, null, -1L, 1L, null, null, "Asset" },
                    { 4L, null, null, 1L, -1L, null, null, "Liability" }
                });

            migrationBuilder.InsertData(
                table: "accountgroupforhelps",
                columns: new[] { "AccountGroupForHelpId", "HelpGroupDescription" },
                values: new object[,]
                {
                    { 1L, "ExcludedAccountInReceipt" },
                    { 2L, "BankAccountInReceipt" },
                    { 3L, "IncludedAccountCreditNoteService" },
                    { 4L, "IncludedAccountDebitNoteService" },
                    { 5L, "IncludedAccountInvoiceService" },
                    { 6L, "IncludedAccountBillService" },
                    { 7L, "IncludedAccountCompanySalesTaxReceivable" }
                });

            migrationBuilder.InsertData(
                table: "taxdocumenttypes",
                columns: new[] { "TaxTypeId", "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1L, null, null, null, null, "PAN" },
                    { 2L, null, null, null, null, "CST" },
                    { 3L, null, null, null, null, "GST" },
                    { 4L, null, null, null, null, "TIN" }
                });

            migrationBuilder.InsertData(
                table: "AccountGroups",
                columns: new[] { "Id", "AccountClassificationId", "CreatedBy", "CreatedDate", "Description", "IsSubType", "LastModifiedBy", "LastModifiedDate", "Name", "OpenBalance", "ParentAccountGroupId", "TrackDepriciation" },
                values: new object[,]
                {
                    { 1L, 3L, null, null, null, false, null, null, "Account Receivables", false, null, false },
                    { 2L, 3L, null, null, null, false, null, null, "Other Current Assets", false, null, false },
                    { 3L, 3L, null, null, null, false, null, null, "Bank", false, null, false },
                    { 4L, 3L, null, null, null, false, null, null, "Fixed Assets", false, null, false },
                    { 5L, 3L, null, null, null, false, null, null, "Other Assets", false, null, false },
                    { 6L, 4L, null, null, null, false, null, null, "Accounts Payable (A/P)", false, null, false },
                    { 7L, 4L, null, null, null, false, null, null, "Credit Card", false, null, false },
                    { 8L, 4L, null, null, null, false, null, null, "Other Current Liabilities", false, null, false },
                    { 9L, 4L, null, null, null, false, null, null, "Long Term Liabilities", false, null, false },
                    { 10L, 3L, null, null, null, false, null, null, "Equity", false, null, false },
                    { 11L, 1L, null, null, null, false, null, null, "Income", false, null, false },
                    { 12L, 1L, null, null, null, false, null, null, "Other Income", false, null, false },
                    { 13L, 1L, null, null, null, false, null, null, "Cost of Good Sold", false, null, false },
                    { 14L, 2L, null, null, null, false, null, null, "Expenses", false, null, false },
                    { 15L, 2L, null, null, null, false, null, null, "Other Expenses", false, null, false }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "AccountingMethodId", "AccountingStartDate", "Active", "CompanyTypeId", "CreatedBy", "CreatedDate", "DefaultCurrencyId", "DefaultDateFormat", "DefaultMobileFormat", "DefaultPhoneFormat", "ISOCode2", "ISOCode3", "IncomeTaxStartDate", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AF", "AFG", 0, null, null, "Afghanistan" },
                    { 2L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AL", "ALB", 0, null, null, "Albania" },
                    { 3L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "DZ", "DZA", 0, null, null, "Algeria" },
                    { 4L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AS", "ASM", 0, null, null, "American Samoa" },
                    { 5L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AD", "AND", 0, null, null, "Andorra" },
                    { 6L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AO", "AGO", 0, null, null, "Angola" },
                    { 7L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AI", "AIA", 0, null, null, "Anguilla" },
                    { 8L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AQ", "ATA", 0, null, null, "Antarctica" },
                    { 9L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AG", "ATG", 0, null, null, "Antigua and Barbuda" },
                    { 10L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AR", "ARG", 0, null, null, "Argentina" },
                    { 11L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AM", "ARM", 0, null, null, "Armenia" },
                    { 12L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AW", "ABW", 0, null, null, "Aruba" },
                    { 13L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AU", "AUS", 0, null, null, "Australia" },
                    { 14L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AT", "AUT", 0, null, null, "Austria" },
                    { 15L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AZ", "AZE", 0, null, null, "Azerbaijan" },
                    { 16L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BS", "BHS", 0, null, null, "Bahamas" },
                    { 17L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BH", "BHR", 0, null, null, "Bahrain" },
                    { 18L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BD", "BGD", 0, null, null, "Bangladesh" },
                    { 19L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BB", "BRB", 0, null, null, "Barbados" },
                    { 20L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BY", "BLR", 0, null, null, "Belarus" },
                    { 21L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BE", "BEL", 0, null, null, "Belgium" },
                    { 22L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BZ", "BLZ", 0, null, null, "Belize" },
                    { 23L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BJ", "BEN", 0, null, null, "Benin" },
                    { 24L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BM", "BMU", 0, null, null, "Bermuda" },
                    { 25L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BT", "BTN", 0, null, null, "Bhutan" },
                    { 26L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BO", "BOL", 0, null, null, "Bolivia" },
                    { 27L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BA", "BIH", 0, null, null, "Bosnia and Herzegowina" },
                    { 28L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BW", "BWA", 0, null, null, "Botswana" },
                    { 29L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BV", "BVT", 0, null, null, "Bouvet Island" },
                    { 30L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BR", "BRA", 0, null, null, "Brazil" },
                    { 31L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "IO", "IOT", 0, null, null, "British Indian Ocean Territory" },
                    { 32L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BN", "BRN", 0, null, null, "Brunei Darussalam" },
                    { 33L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BG", "BGR", 0, null, null, "Bulgaria" },
                    { 34L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BF", "BFA", 0, null, null, "Burkina Faso" },
                    { 35L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "BI", "BDI", 0, null, null, "Burundi" },
                    { 36L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KH", "KHM", 0, null, null, "Cambodia" },
                    { 37L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CM", "CMR", 0, null, null, "Cameroon" },
                    { 38L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CA", "CAN", 0, null, null, "Canada" },
                    { 39L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CV", "CPV", 0, null, null, "Cape Verde" },
                    { 40L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KY", "CYM", 0, null, null, "Cayman Islands" },
                    { 41L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CF", "CAF", 0, null, null, "Central African Republic" },
                    { 42L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TD", "TCD", 0, null, null, "Chad" },
                    { 43L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CL", "CHL", 0, null, null, "Chile" },
                    { 44L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CN", "CHN", 0, null, null, "China" },
                    { 45L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CX", "CXR", 0, null, null, "Christmas Island" },
                    { 46L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CC", "CCK", 0, null, null, "Cocos (Keeling) Islands" },
                    { 47L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CO", "COL", 0, null, null, "Colombia" },
                    { 48L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KM", "COM", 0, null, null, "Comoros" },
                    { 49L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CG", "COG", 0, null, null, "Congo" },
                    { 50L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CK", "COK", 0, null, null, "Cook Islands" },
                    { 51L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CR", "CRI", 0, null, null, "Costa Rica" },
                    { 52L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CI", "CIV", 0, null, null, "Cote D Ivoire" },
                    { 53L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "HR", "HRV", 0, null, null, "Croatia" },
                    { 54L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CU", "CUB", 0, null, null, "Cuba" },
                    { 55L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CY", "CYP", 0, null, null, "Cyprus" },
                    { 56L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CZ", "CZE", 0, null, null, "Czech Republic" },
                    { 57L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "DK", "DNK", 0, null, null, "Denmark" },
                    { 58L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "DJ", "DJI", 0, null, null, "Djibouti" },
                    { 59L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "DM", "DMA", 0, null, null, "Dominica" },
                    { 60L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "DO", "DOM", 0, null, null, "Dominican Republic" },
                    { 61L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TP", "TMP", 0, null, null, "East Timor" },
                    { 62L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "EC", "ECU", 0, null, null, "Ecuador" },
                    { 63L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "EG", "EGY", 0, null, null, "Egypt" },
                    { 64L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SV", "SLV", 0, null, null, "El Salvador" },
                    { 65L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GQ", "GNQ", 0, null, null, "Equatorial Guinea" },
                    { 66L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ER", "ERI", 0, null, null, "Eritrea" },
                    { 67L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "EE", "EST", 0, null, null, "Estonia" },
                    { 68L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ET", "ETH", 0, null, null, "Ethiopia" },
                    { 69L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "FK", "FLK", 0, null, null, "Falkland Islands (Malvinas)" },
                    { 70L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "FO", "FRO", 0, null, null, "Faroe Islands" },
                    { 71L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "FJ", "FJI", 0, null, null, "Fiji" },
                    { 72L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "FI", "FIN", 0, null, null, "Finland" },
                    { 73L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "FR", "FRA", 0, null, null, "France" },
                    { 74L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, " ", " ", 0, null, null, "France" },
                    { 75L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GF", "GUF", 0, null, null, "French Guiana" },
                    { 76L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PF", "PYF", 0, null, null, "French Polynesia" },
                    { 77L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TF", "ATF", 0, null, null, "French Southern Territories" },
                    { 78L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GA", "GAB", 0, null, null, "Gabon" },
                    { 79L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GM", "GMB", 0, null, null, "Gambia" },
                    { 80L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GE", "GEO", 0, null, null, "Georgia" },
                    { 81L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "DE", "DEU", 0, null, null, "Germany" },
                    { 82L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GH", "GHA", 0, null, null, "Ghana" },
                    { 83L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GI", "GIB", 0, null, null, "Gibraltar" },
                    { 84L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GR", "GRC", 0, null, null, "Greece" },
                    { 85L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GL", "GRL", 0, null, null, "Greenland" },
                    { 86L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GD", "GRD", 0, null, null, "Grenada" },
                    { 87L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GP", "GLP", 0, null, null, "Guadeloupe" },
                    { 88L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GU", "GUM", 0, null, null, "Guam" },
                    { 89L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GT", "GTM", 0, null, null, "Guatemala" },
                    { 90L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GN", "GIN", 0, null, null, "Guinea" },
                    { 91L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GW", "GNB", 0, null, null, "Guinea-bissau" },
                    { 92L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GY", "GUY", 0, null, null, "Guyana" },
                    { 93L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "HT", "HTI", 0, null, null, "Haiti" },
                    { 94L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "HM", "HMD", 0, null, null, "Heard and Mc Donald Islands" },
                    { 95L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "HN", "HND", 0, null, null, "Honduras" },
                    { 96L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "HK", "HKG", 0, null, null, "Hong Kong" },
                    { 97L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "HU", "HUN", 0, null, null, "Hungary" },
                    { 98L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "IS", "ISL", 0, null, null, "Iceland" },
                    { 99L, null, 0, true, null, null, null, 165L, "MM/dd/yyyy", null, null, "IN", "IND", 0, null, null, "India" },
                    { 100L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ID", "IDN", 0, null, null, "Indonesia" },
                    { 101L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "IR", "IRN", 0, null, null, "Iran (Islamic Republic of)" },
                    { 102L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "IQ", "IRQ", 0, null, null, "Iraq" },
                    { 103L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "IE", "IRL", 0, null, null, "Ireland" },
                    { 104L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "IL", "ISR", 0, null, null, "Israel" },
                    { 105L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "IT", "ITA", 0, null, null, "Italy" },
                    { 106L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "JM", "JAM", 0, null, null, "Jamaica" },
                    { 107L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "JP", "JPN", 0, null, null, "Japan" },
                    { 108L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "JO", "JOR", 0, null, null, "Jordan" },
                    { 109L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KZ", "KAZ", 0, null, null, "Kazakhstan" },
                    { 110L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KE", "KEN", 0, null, null, "Kenya" },
                    { 111L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KI", "KIR", 0, null, null, "Kiribati" },
                    { 112L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, " ", " ", 0, null, null, "Korea" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "AccountingMethodId", "AccountingStartDate", "Active", "CompanyTypeId", "CreatedBy", "CreatedDate", "DefaultCurrencyId", "DefaultDateFormat", "DefaultMobileFormat", "DefaultPhoneFormat", "ISOCode2", "ISOCode3", "IncomeTaxStartDate", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 113L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, " ", " ", 0, null, null, "Korea" },
                    { 114L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KW", "KWT", 0, null, null, "Kuwait" },
                    { 115L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KG", "KGZ", 0, null, null, "Kyrgyzstan" },
                    { 116L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LA", "LAO", 0, null, null, "Lao Peoples Democratic Republic" },
                    { 117L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LV", "LVA", 0, null, null, "Latvia" },
                    { 118L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LB", "LBN", 0, null, null, "Lebanon" },
                    { 119L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LS", "LSO", 0, null, null, "Lesotho" },
                    { 120L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LR", "LBR", 0, null, null, "Liberia" },
                    { 121L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LY", "LBY", 0, null, null, "Libyan Arab Jamahiriya" },
                    { 122L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LI", "LIE", 0, null, null, "Liechtenstein" },
                    { 123L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LT", "LTU", 0, null, null, "Lithuania" },
                    { 124L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LU", "LUX", 0, null, null, "Luxembourg" },
                    { 125L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MO", "MAC", 0, null, null, "Macau" },
                    { 126L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, " ", " ", 0, null, null, "Macedonia" },
                    { 127L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MG", "MDG", 0, null, null, "Madagascar" },
                    { 128L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MW", "MWI", 0, null, null, "Malawi" },
                    { 129L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MY", "MYS", 0, null, null, "Malaysia" },
                    { 130L, null, 0, true, null, null, null, 165L, "MM/dd/yyyy", null, null, "MV", "MDV", 0, null, null, "Maldives" },
                    { 131L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ML", "MLI", 0, null, null, "Mali" },
                    { 132L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MT", "MLT", 0, null, null, "Malta" },
                    { 133L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MH", "MHL", 0, null, null, "Marshall Islands" },
                    { 134L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MQ", "MTQ", 0, null, null, "Martinique" },
                    { 135L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MR", "MRT", 0, null, null, "Mauritania" },
                    { 136L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MU", "MUS", 0, null, null, "Mauritius" },
                    { 137L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "YT", "MYT", 0, null, null, "Mayotte" },
                    { 138L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MX", "MEX", 0, null, null, "Mexico" },
                    { 139L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, " ", " ", 0, null, null, "Micronesia" },
                    { 140L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, " ", " ", 0, null, null, "Moldova" },
                    { 141L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MC", "MCO", 0, null, null, "Monaco" },
                    { 142L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MN", "MNG", 0, null, null, "Mongolia" },
                    { 143L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MS", "MSR", 0, null, null, "Montserrat" },
                    { 144L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MA", "MAR", 0, null, null, "Morocco" },
                    { 145L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MZ", "MOZ", 0, null, null, "Mozambique" },
                    { 146L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MM", "MMR", 0, null, null, "Myanmar" },
                    { 147L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NA", "NAM", 0, null, null, "Namibia" },
                    { 148L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NR", "NRU", 0, null, null, "Nauru" },
                    { 149L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NP", "NPL", 0, null, null, "Nepal" },
                    { 150L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NL", "NLD", 0, null, null, "Netherlands" },
                    { 151L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AN", "ANT", 0, null, null, "Netherlands Antilles" },
                    { 152L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NC", "NCL", 0, null, null, "New Caledonia" },
                    { 153L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NZ", "NZL", 0, null, null, "New Zealand" },
                    { 154L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NI", "NIC", 0, null, null, "Nicaragua" },
                    { 155L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NE", "NER", 0, null, null, "Niger" },
                    { 156L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NG", "NGA", 0, null, null, "Nigeria" },
                    { 157L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NU", "NIU", 0, null, null, "Niue" },
                    { 158L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NF", "NFK", 0, null, null, "Norfolk Island" },
                    { 159L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "MP", "MNP", 0, null, null, "Northern Mariana Islands" },
                    { 160L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "NO", "NOR", 0, null, null, "Norway" },
                    { 161L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "OM", "OMN", 0, null, null, "Oman" },
                    { 162L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PK", "PAK", 0, null, null, "Pakistan" },
                    { 163L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PW", "PLW", 0, null, null, "Palau" },
                    { 164L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PA", "PAN", 0, null, null, "Panama" },
                    { 165L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PG", "PNG", 0, null, null, "Papua New Guinea" },
                    { 166L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PY", "PRY", 0, null, null, "Paraguay" },
                    { 167L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PE", "PER", 0, null, null, "Peru" },
                    { 168L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PH", "PHL", 0, null, null, "Philippines" },
                    { 169L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PN", "PCN", 0, null, null, "Pitcairn" },
                    { 170L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PL", "POL", 0, null, null, "Poland" },
                    { 171L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PT", "PRT", 0, null, null, "Portugal" },
                    { 172L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PR", "PRI", 0, null, null, "Puerto Rico" },
                    { 173L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "QA", "QAT", 0, null, null, "Qatar" },
                    { 174L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "RE", "REU", 0, null, null, "Reunion" },
                    { 175L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "RO", "ROM", 0, null, null, "Romania" },
                    { 176L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "RU", "RUS", 0, null, null, "Russian Federation" },
                    { 177L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "RW", "RWA", 0, null, null, "Rwanda" },
                    { 178L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "KN", "KNA", 0, null, null, "Saint Kitts and Nevis" },
                    { 179L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LC", "LCA", 0, null, null, "Saint Lucia" },
                    { 180L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "VC", "VCT", 0, null, null, "Saint Vincent and the Grenadines" },
                    { 181L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "WS", "WSM", 0, null, null, "Samoa" },
                    { 182L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SM", "SMR", 0, null, null, "San Marino" },
                    { 183L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ST", "STP", 0, null, null, "Sao Tome and Principe" },
                    { 184L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SA", "SAU", 0, null, null, "Saudi Arabia" },
                    { 185L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SN", "SEN", 0, null, null, "Senegal" },
                    { 186L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SC", "SYC", 0, null, null, "Seychelles" },
                    { 187L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SL", "SLE", 0, null, null, "Sierra Leone" },
                    { 188L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SG", "SGP", 0, null, null, "Singapore" },
                    { 189L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SK", "SVK", 0, null, null, "Slovakia (Slovak Republic)" },
                    { 190L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SI", "SVN", 0, null, null, "Slovenia" },
                    { 191L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SB", "SLB", 0, null, null, "Solomon Islands" },
                    { 192L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SO", "SOM", 0, null, null, "Somalia" },
                    { 193L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ZA", "ZAF", 0, null, null, "south Africa" },
                    { 194L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GS", "SGS", 0, null, null, "South Georgia and the South Sandwich Islands" },
                    { 195L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ES", "ESP", 0, null, null, "Spain" },
                    { 196L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "LK", "LKA", 0, null, null, "Sri Lanka" },
                    { 197L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SH", "SHN", 0, null, null, "St. Helena" },
                    { 198L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "PM", "SPM", 0, null, null, "St. Pierre and Miquelon" },
                    { 199L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SD", "SDN", 0, null, null, "Sudan" },
                    { 200L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SR", "SUR", 0, null, null, "Suriname" },
                    { 201L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SJ", "SJM", 0, null, null, "Svalbard and Jan Mayen Islands" },
                    { 202L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SZ", "SWZ", 0, null, null, "Swaziland" },
                    { 203L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SE", "SWE", 0, null, null, "Sweden" },
                    { 204L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "CH", "CHE", 0, null, null, "Switzerland" },
                    { 205L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "SY", "SYR", 0, null, null, "Syrian Arab Republic" },
                    { 206L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, " ", " ", 0, null, null, "Taiwan" },
                    { 207L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TJ", "TJK", 0, null, null, "Tajikistan" },
                    { 208L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, " ", " ", 0, null, null, "Tanzania" },
                    { 209L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TH", "THA", 0, null, null, "Thailand" },
                    { 210L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TG", "TGO", 0, null, null, "Togo" },
                    { 211L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TK", "TKL", 0, null, null, "Tokelau" },
                    { 212L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TO", "TON", 0, null, null, "Tonga" },
                    { 213L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TT", "TTO", 0, null, null, "Trinidad and Tobago" },
                    { 214L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TN", "TUN", 0, null, null, "Tunisia" },
                    { 215L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TR", "TUR", 0, null, null, "Turkey" },
                    { 216L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TM", "TKM", 0, null, null, "Turkmenistan" },
                    { 217L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TC", "TCA", 0, null, null, "Turks and Caicos Islands" },
                    { 218L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "TV", "TUV", 0, null, null, "Tuvalu" },
                    { 219L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "UG", "UGA", 0, null, null, "Uganda" },
                    { 220L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "UA", "UKR", 0, null, null, "Ukraine" },
                    { 221L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "AE", "ARE", 0, null, null, "United Arab Emirates" },
                    { 222L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "GB", "GBR", 0, null, null, "United Kingdom" },
                    { 223L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "US", "USA", 0, null, null, "United States" },
                    { 224L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "UM", "UMI", 0, null, null, "United States Minor Outlying Islands" },
                    { 225L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "UY", "URY", 0, null, null, "Uruguay" },
                    { 226L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "UZ", "UZB", 0, null, null, "Uzbekistan" },
                    { 227L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "VU", "VUT", 0, null, null, "Vanuatu" },
                    { 228L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "VA", "VAT", 0, null, null, "Vatican City State (Holy See)" },
                    { 229L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "VE", "VEN", 0, null, null, "Venezuela" },
                    { 230L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "VN", "VNM", 0, null, null, "Viet Nam" },
                    { 231L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "VG", "VGB", 0, null, null, "Virgin Islands (British)" },
                    { 232L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "VI", "VIR", 0, null, null, "Virgin Islands (U.S.)" },
                    { 233L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "WF", "WLF", 0, null, null, "Wallis and Futuna Islands" },
                    { 234L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "EH", "ESH", 0, null, null, "Western Sahara" },
                    { 235L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "YE", "YEM", 0, null, null, "Yemen" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "AccountingMethodId", "AccountingStartDate", "Active", "CompanyTypeId", "CreatedBy", "CreatedDate", "DefaultCurrencyId", "DefaultDateFormat", "DefaultMobileFormat", "DefaultPhoneFormat", "ISOCode2", "ISOCode3", "IncomeTaxStartDate", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 236L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "YU", "YUG", 0, null, null, "Yugoslavia" },
                    { 237L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ZR", "ZAR", 0, null, null, "Zaire" },
                    { 238L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ZM", "ZMB", 0, null, null, "Zambia" },
                    { 239L, null, 0, false, null, null, null, 165L, "MM/dd/yyyy", null, null, "ZW", "ZWE", 0, null, null, "Zimbabwe" }
                });

            migrationBuilder.InsertData(
                table: "PatientHistoryQuestionGroups",
                columns: new[] { "Id", "Name", "Order", "ParentGroupId" },
                values: new object[,]
                {
                    { 3L, "General", 1, 2L },
                    { 4L, "Muscle/joint/Bones", 2, 2L },
                    { 5L, "Stomach and Intestines", 3, 2L },
                    { 6L, "Ears", 4, 2L },
                    { 7L, "Nervous System", 5, 2L }
                });

            migrationBuilder.InsertData(
                table: "PatientHistoryQuestions",
                columns: new[] { "Id", "AdditionalNotes", "AdditionalNotesCaption", "GroupId", "Name", "Order", "ValueType" },
                values: new object[,]
                {
                    { 1L, false, null, 1L, "Diabetes", 1, 4 },
                    { 2L, false, null, 1L, "High Blood Sugar", 2, 4 },
                    { 3L, false, null, 1L, "High Cholesterol", 3, 4 },
                    { 4L, false, null, 1L, "Hypothyrodisim", 4, 4 },
                    { 5L, false, null, 1L, "Goiter", 5, 4 },
                    { 6L, true, "Type?", 1L, "Cancer", 6, 4 },
                    { 7L, false, null, 1L, "Other Medical Condition", 7, 3 }
                });

            migrationBuilder.InsertData(
                table: "AccountGroups",
                columns: new[] { "Id", "AccountClassificationId", "CreatedBy", "CreatedDate", "Description", "IsSubType", "LastModifiedBy", "LastModifiedDate", "Name", "OpenBalance", "ParentAccountGroupId", "TrackDepriciation" },
                values: new object[,]
                {
                    { 101L, 3L, null, null, "Accounts receivable (also called A/R, Debtors, or Trade and other receivables) tracks money that customers owe you for products or services, and payments customers make. System automatically creates one Accounts receivable account for you. Most businesses need only one. Each customer has a register, which functions like an Accounts receivable account for each customer.", true, null, null, "Account Receivables", false, 1L, false },
                    { 201L, 3L, null, null, "Use Allowance for bad debts to estimate the part of Accounts receivable you think you might not collect. Use this only if you are keeping your books on the accrual basis.", true, null, null, "Allowance of Bad Debts", false, 2L, false },
                    { 202L, 3L, null, null, "Use Development costs to track amounts you deposit or set aside to arrange for financing, such as an SBA loan, or for deposits in anticipation of the purchase of property or other assets. When the deposit is refunded, or the purchase takes place, remove the amount from this account.", true, null, null, "Development Cost", false, 2L, false },
                    { 203L, 3L, null, null, "Use Employee cash advances to track employee wages and salary you issue to an employee early, or other non-salary money given to employees. If you make a loan to an employee, use the Other current asset account type called Loans to others, instead.", true, null, null, "Employee Cash Advances", false, 2L, false },
                    { 204L, 3L, null, null, "Use Inventory to track the cost of goods your business purchases for resale. When the goods are sold, assign the sale to a Cost of goods sold account.", true, null, null, "Inventory", false, 2L, false },
                    { 205L, 3L, null, null, "Use Investments - Mortgage/real estate loans to show the balances of any mortgage or real estate loans your business has made or purchased.", true, null, null, "Investment - Mortgage/Real Estate Loans", false, 2L, false },
                    { 206L, 3L, null, null, "Use Investments - Tax-exempt securities for investments in state and local bonds, or mutual funds that invest in state and local bonds.", true, null, null, "Investment - Tax-Exempt Securities", false, 2L, false },
                    { 207L, 3L, null, null, "Use Investments - U.S. government obligations for bonds issued by the U.S. government.", true, null, null, "Investment - Government Obligations", false, 2L, false },
                    { 208L, 3L, null, null, "Use Investments - Other to track the value of investments not covered by other investment account types. Examples include publicly-traded stocks, coins, or gold.", true, null, null, "Investments - Other", false, 2L, false },
                    { 209L, 3L, null, null, "If you operate your business as a Limited Company, use Loans to officers to track money loaned to officers of your business.", true, null, null, "Loans To Officers", false, 2L, false },
                    { 210L, 3L, null, null, "Use Loans to others to track money your business loans to other people or businesses.This type of account is also referred to as Notes Receivable.For early salary payments to employees, use Employee cash advances, instead.", true, null, null, "Loans to Others", false, 2L, false },
                    { 211L, 3L, null, null, "If you operate your business as a Limited, use Loans to stockholders to track money your business loans to its stockholders.", true, null, null, "Loans to Stockholders", false, 2L, false },
                    { 212L, 3L, null, null, "Use Other current assets for current assets not covered by the other types. Current assets are likely to be converted to cash or used up in a year.", true, null, null, "Other Current Assets", false, 2L, false },
                    { 213L, 3L, null, null, "Use Prepaid expenses to track payments for expenses that you won’t recognize until your next accounting period. When you recognize the expense, make a journal entry to transfer money from this account to the expense account.", true, null, null, "Prepaid Expenses", false, 2L, false },
                    { 214L, 3L, null, null, "Use Retainage if your customers regularly hold back a portion of a contract amount until you have completed a project. This type of account is often used in the construction industry, and only if you record income on an accrual basis.", true, null, null, "Retainage", false, 2L, false },
                    { 215L, 3L, null, null, "Use Undeposited funds for cash or checks from sales that haven’t been deposited yet. For petty cash, use Cash on hand, instead.", true, null, null, "Undeposited Funds", false, 2L, false },
                    { 301L, 3L, null, null, "Use a Cash on hand account to track cash your company keeps for occasional expenses, also called petty cash. To track cash from sales that have not been deposited yet, use a pre-created account called Undeposited funds, instead.", true, null, null, "Cash on hand", false, 3L, false },
                    { 302L, 3L, null, null, "Use Checking accounts to track all your checking activity, including debit card transactions. Each checking account your company has at a bank or other financial institution should have its own Checking type account in system.", true, null, null, "Checking", false, 3L, false },
                    { 303L, 3L, null, null, "Use Money market to track amounts in money market accounts. For investments, see Other Current Assets, instead.", true, null, null, "Money Market", false, 3L, false },
                    { 304L, 3L, null, null, "Use Rents held in trust to track deposits and rent held on behalf of the property owners. Typically only property managers use this type of account.", true, null, null, "Rents Held in Trust", false, 3L, false },
                    { 305L, 3L, null, null, "Use Savings accounts to track your savings and CD activity. Each savings account your company has at a bank or other financial institution should have its own Savings type account. For investments, see Other Current Assets, instead.", true, null, null, "Savings", false, 3L, false },
                    { 306L, 3L, null, null, "Use Trust accounts for money held by you for the benefit of someone else. For example, trust accounts are often used by attorneys to keep track of expense money their customers have given them. Often, to keep the amount in a trust account from looking like it’s yours, the amount is offset in a 'contra' liability account (a Current Liability).", true, null, null, "Trust account", false, 3L, false },
                    { 307L, 3L, null, null, "Undeposited Funds, keep all undeposited cheques, drafts and bank documents", true, null, null, "Undeposited Fund", false, 3L, false },
                    { 401L, 3L, null, null, "Use Accumulated amortization to track how much you amortize intangible assets.", true, null, null, "Accumulated Amortization", false, 4L, false },
                    { 402L, 3L, null, null, "Use Accumulated depletion to track how much you deplete a natural resource.", true, null, null, "Accumulated Depletion", false, 4L, false },
                    { 403L, 3L, null, null, "Use Accumulated depreciation to track how much you depreciate a fixed asset (a physical asset you do not expect to convert to cash during one year of normal operations).", true, null, null, "Accumulated Depreciation", false, 4L, false },
                    { 404L, 3L, null, null, "Use Buildings to track the cost of structures you own and use for your business. If you have a business in your home, consult your accountant or IRS Publication 587. Use a Land account for the land portion of any real property you own, splitting the cost of the property between land and building in a logical method. A common method is to mimic the land-to-building ratio on the property tax statement.", true, null, null, "Buildings", false, 4L, false },
                    { 405L, 3L, null, null, "Use Depletable assets to track natural resources, such as timberlands, oil wells, and mineral deposits.", true, null, null, "Depletable Assets", false, 4L, false },
                    { 406L, 3L, null, null, "Use Furniture & fixtures to track any furniture and fixtures your business owns and uses, like a dental chair or sales booth.", true, null, null, "Furniture & Fixtures", false, 4L, false },
                    { 407L, 3L, null, null, "Use Intangible assets to track intangible assets that you plan to amortize. Examples include franchises, customer lists, copyrights, and patents.", true, null, null, "Intangible Assets", false, 4L, false },
                    { 408L, 3L, null, null, "Use Land for land or property you don’t depreciate.If land and building were acquired together, split the cost between the two in a logical way. One common method is to use the land-to-building ratio on the property tax statement. For land you use as a natural resource, use a Depletable assets account, instead.", true, null, null, "Land", false, 4L, false },
                    { 409L, 3L, null, null, "Use Leasehold improvements to track improvements to a leased asset that increases the asset’s value. For example, if you carpet a leased office space and are not reimbursed, that’s a leasehold improvement.", true, null, null, "Leasehold Improvements", false, 4L, false },
                    { 410L, 3L, null, null, "Use Machinery & equipment to track computer hardware, as well as any other non-furniture fixtures or devices owned and used for your business. This includes equipment that you ride, like tractors and lawn mowers. Cars and trucks, however, should be tracked with Vehicle accounts, instead.", true, null, null, "Machinery & Equipment", false, 4L, false },
                    { 411L, 3L, null, null, "Use Other fixed asset for fixed assets that are not covered by other asset types. Fixed assets are physical property that you use in your business and that you do not expect to convert to cash or be used up during one year of normal operations.", true, null, null, "Other fixed assets", false, 4L, false },
                    { 412L, 3L, null, null, "Use Vehicles to track the value of vehicles your business owns and uses for business. This includes off-road vehicles, air planes, helicopters, and boats. If you use a vehicle for both business and personal use, consult your accountant or the IRS to see how you should track its value.", true, null, null, "Vehicles", false, 4L, false },
                    { 501L, 3L, null, null, "Use Accumulated amortization of other assets to track how much you’ve amortized asset whose type is Other Asset.", true, null, null, "Accumulated Amortization of Other Assets", false, 5L, false },
                    { 502L, 3L, null, null, "Use Goodwill only if you have acquired another company. It represents the intangible assets of the acquired company which gave it an advantage, such as favorable government relations, business name, outstanding credit ratings, location, superior management, customer lists, product quality, or good labor relations.", true, null, null, "Goodwill", false, 5L, false },
                    { 503L, 3L, null, null, "Use Lease buyout to track lease payments to be applied toward the purchase of a leased asset. You don’t track the leased asset itself until you purchase it.", true, null, null, "Lease Buyout", false, 5L, false },
                    { 504L, 3L, null, null, "Use Licenses to track non-professional licenses for permission to engage in an activity, like selling alcohol or radio broadcasting. For fees associated with professional licenses granted to individuals, use a Legal & professional fees expense account, instead.", true, null, null, "Licenses", false, 5L, false },
                    { 505L, 3L, null, null, "Use Organizational costs to track costs incurred when forming a partnership or corporation. The costs include the legal and accounting costs necessary to organize the company, facilitate the filings of the legal documents, and other paperwork.", true, null, null, "Organizational Costs", false, 5L, false },
                    { 506L, 3L, null, null, "Use Other long-term assets to track assets not covered by other types. Long-term assets are expected to provide value for more than one year.", true, null, null, "Other Long-term Assets", false, 5L, false },
                    { 507L, 3L, null, null, "Use Security deposits to track funds you’ve paid to cover any potential costs incurred by damage, loss, or theft. The funds should be returned to you at the end of the contract. If you collect deposits, use an Other current liabilities account type (an Other current liability account).", true, null, null, "Security Deposits", false, 5L, false },
                    { 601L, 4L, null, null, "Accounts payable (also called A/P) tracks amounts you owe to your vendors and suppliers. Systen automatically creates one Accounts Payable account for you. Most businesses need only one.", true, null, null, "Accounts Payable (A/P)", false, 6L, false },
                    { 602L, 4L, null, null, "Accounts payable tracks amounts you owe to your Employees.", true, null, null, "Employees", false, 6L, false },
                    { 701L, 4L, null, null, "Credit card accounts track the balance due on your business credit cards. Create one Credit card account for each credit card account your business uses", true, null, null, "Credit Card", false, 7L, false },
                    { 801L, 4L, null, null, "Use Federal Income Tax Payable if your business is a corporation, S corporation, or limited partnership keeping records on the accrual basis.This account tracks income tax liabilities in the year the income is earned.", true, null, null, "Federal Income Tax Payable", false, 8L, false },
                    { 802L, 4L, null, null, "Use Insurance payable to keep track of insurance amounts due. This account is most useful for businesses with monthly recurring insurance expenses such as Workers’ Compensation.", true, null, null, "Insurance Payable", false, 8L, false },
                    { 803L, 4L, null, null, "Use Line of credit to track the balance due on any lines of credit your business has. Each line of credit your business has should have its own Line of credit account.", true, null, null, "Line of Credit", false, 8L, false },
                    { 804L, 4L, null, null, "Use Loan payable to track loans your business owes which are payable within the next twelve months. For longer-term loans, use the Long-term liability called Notes payable, instead.", true, null, null, "Loan Payable", false, 8L, false },
                    { 805L, 4L, null, null, "Use Other current liabilities to track liabilities due within the next twelve months that do not fit the Other current liability account types.", true, null, null, "Other Current Liabilities", false, 8L, false },
                    { 806L, 4L, null, null, "Use Payroll clearing to keep track of any non-tax amounts that you have deducted from employee paychecks or that you owe as a result of doing payroll. When you forward money to the appropriate vendors, deduct the amount from the balance of this account. Do not use this account for tax amounts you have withheld or owe from paying employee wages. For those amounts, use the Payroll tax payable account instead.", true, null, null, "Payroll Clearing", false, 8L, false },
                    { 807L, 4L, null, null, "Use Payroll tax payable to keep track of tax amounts that you owe to Federal, State, and Local government agencies as a result of paying wages and taxes you have withheld from employee paychecks. When you forward the money to the government agency, deduct the amount from the balance of this account.", true, null, null, "Payroll Tax Payable", false, 8L, false },
                    { 808L, 4L, null, null, "Use Prepaid expenses payable to track items such as property taxes that are due, but not yet deductible as an expense because the period they cover has not yet passed.", true, null, null, "Prepaid Expenses Payable", false, 8L, false },
                    { 809L, 4L, null, null, "Use Rents in trust - liability to offset the Rents in trust amount in assets. Amounts in these accounts are held by your business on behalf of others. They do not belong to your business, so should not appear to be yours on your balance sheet. This 'contra' account takes care of that, as long as the two balances match.", true, null, null, "Rents in trust - Liability", false, 8L, false },
                    { 810L, 4L, null, null, "Use Sales tax payable to track sales tax you have collected, but not yet remitted to the IRS.", true, null, null, "Sales Tax Payable", false, 8L, false },
                    { 811L, 4L, null, null, "Use State/local income tax payable if your business is a corporation, S corporation, or limited partnership keeping records on the accrual basis. This account tracks income tax liabilities in the year the income is earned", true, null, null, "State/Local Income Tax Payable", false, 8L, false },
                    { 812L, 4L, null, null, "Use Trust accounts - liabilities to offset Trust accounts in assets.Amounts in these accounts are held by your business on behalf of others. They do not belong to your business, so should not appear to be yours on your balance sheet. This 'contra' account takes care of that, as long as the two balances match.", true, null, null, "Trust Accounts - Liabilities", false, 8L, false },
                    { 813L, 4L, null, null, "Use Integrated income tax payable if your business is a corporation, S corporation, or limited partnership keeping records on the accrual basis. This account tracks income tax liabilities in the year the income is earned", true, null, null, "Integrated Income Tax Payable", false, 8L, false },
                    { 901L, 4L, null, null, "Use Notes payable to track the amounts your business owes in long-term (over twelve months) loans. For shorter loans, use the Other current liability account type called Loan payable, instead.", true, null, null, "Notes Payable", false, 9L, false },
                    { 902L, 4L, null, null, "Use Other long term liabilities to track liabilities due in more than twelve months that don’t fit the other Long-term liability account types.", true, null, null, "Other Long Term Liabilities", false, 9L, false },
                    { 903L, 4L, null, null, "Use Shareholder notes payable to track long-term loan balances your business owes its shareholders.", true, null, null, "Shareholder Notes Payable", false, 9L, false },
                    { 1001L, 3L, null, null, "S corporations use this account to track adjustments to owner’s equity that are not attributable to net income.", true, null, null, "Accumulated Adjustment", false, 10L, false },
                    { 1002L, 3L, null, null, "Corporations use Common stock to track shares of its common stock in the hands of shareholders. The amount in this account should be the stated (or par) value of the stock.", true, null, null, "Common Stock", false, 10L, false },
                    { 1003L, 3L, null, null, "System creates this account the first time you enter an opening balance for a balance sheet account. As you enter opening balances, System records the amounts in Opening balance equity. This ensures that you have a correct balance sheet for your company, even before you’ve finished entering all your company’s assets and liabilities.", true, null, null, "Opening Balance Equity", false, 10L, false },
                    { 1004L, 3L, null, null, "S corporations use Owner’s equity to show the cumulative net income or loss of their business as of the beginning of the fiscal year. ", true, null, null, "Owner's Equity", false, 10L, false },
                    { 1005L, 3L, null, null, "Corporations use Paid-in capital to track amounts received from shareholders in exchange for stock that are over and above the stock’s stated (or par) value.", true, null, null, "Paid-In Capital or Surplus", false, 10L, false },
                    { 1006L, 3L, null, null, "Partnerships use Partner contributions to track amounts partners contribute to the partnership during the year.", true, null, null, "Partner Contributions", false, 10L, false },
                    { 1007L, 3L, null, null, "Partnerships use Partner distributions to track amounts distributed by the partnership to its partners during the year. Don’t use this for regular payments to partners for interest or service. For regular payments, use a Guaranteed payments account (a Expense account in Payroll expenses), instead. ", true, null, null, "Partner Distributions", false, 10L, false },
                    { 1008L, 3L, null, null, "Partnerships use Partner’s equity to show the income remaining in the partnership for each partner as of the end of the prior year.", true, null, null, "Partner's Equity", false, 10L, false },
                    { 1009L, 3L, null, null, "Corporations use this account to track shares of its preferred stock in the hands of shareholders. The amount in this account should be the stated (or par) value of the stock.", true, null, null, "Preferred Stock", false, 10L, false },
                    { 1010L, 3L, null, null, "System adds this account when you create your company. Retained earnings tracks net income from previous fiscal years. System automatically transfers your profit (or loss) to Retained earnings at the end of each fiscal year.", true, null, null, "Retained Earnings", false, 10L, false },
                    { 1011L, 3L, null, null, "Corporations use Treasury stock to track amounts paid by the corporation to buy its own stock back from shareholders.", true, null, null, "Treasury Stock", false, 10L, false },
                    { 1101L, 1L, null, null, "Use Discounts/refunds given to track discounts you give to customers. This account typically has a negative balance so it offsets other income. For discounts from vendors, use an expense account, instead.", true, null, null, "Discounts/Refunds Given", false, 11L, false },
                    { 1102L, 1L, null, null, "Use Non-profit income to track money coming in if you are a non-profit organization.", true, null, null, "Non-Profit Income", false, 11L, false },
                    { 1103L, 1L, null, null, "Use Other primary income to track income from normal business operations that doesn’t fall into another Income type.", true, null, null, "Other Primary Income", false, 11L, false },
                    { 1104L, 1L, null, null, "Use Sales of product income to track income from selling products. This can include all kinds of products, like crops and livestock, rental fees, performances, and food served.", true, null, null, "Sales of Product Income", false, 11L, false },
                    { 1105L, 1L, null, null, "Use Service/fee income to track income from services you perform or ordinary usage fees you charge. For fees customers pay you for late payments or other uncommon situations, use an Other Income account type called Other miscellaneous income, instead.", true, null, null, "Service/Fee Income", false, 11L, false },
                    { 1106L, 1L, null, null, "Unapplied Cash Payment Income reports the Cash Basis income from customers payments you’ve received but not applied to invoices or charges. In general, you would never use this directly on a purchase or sale transaction. The IRS calls this 'Constructive Receipt Income.' See Publication 538.", true, null, null, "Unapplied Cash Payment Income", false, 11L, false },
                    { 1201L, 1L, null, null, "Use Dividend income to track taxable dividends from investments.", true, null, null, "Dividend Income", false, 12L, false },
                    { 1202L, 1L, null, null, "Use Interest earned to track interest from bank or savings accounts, investments, or interest payments to you on loans your business made.", true, null, null, "Interest Earned", false, 12L, false },
                    { 1203L, 1L, null, null, "Use Other investment income to track other types of investment income that isn’t from dividends or interest.", true, null, null, "Other Investment Income", false, 12L, false },
                    { 1204L, 1L, null, null, "Use Other miscellaneous income to track income that isn’t from normal business operations, and doesn’t fall into another Other Income type.", true, null, null, "Other Miscellaneous Income", false, 12L, false },
                    { 1205L, 1L, null, null, "Use Tax-exempt interest to record interest that isn’t taxable, such as interest on money in tax-exempt retirement accounts, or interest from tax-exempt bonds.", true, null, null, "Tax-Exempt Interest", false, 12L, false },
                    { 1301L, 1L, null, null, "Use Cost of labor - COS to track the cost of paying employees to produce products or supply services. It includes all employment costs, including food and transportation, if applicable.", true, null, null, "Cost of labor - COS", false, 13L, false },
                    { 1302L, 1L, null, null, "Use Equipment rental - COS to track the cost of renting equipment to produce products or services. If you purchase equipment, use a Fixed Asset account type called Machinery and equipment.", true, null, null, "Equipment Rental - COS", false, 13L, false },
                    { 1303L, 1L, null, null, "Use Other costs of service - COS to track costs related to services you provide that don’t fall into another Cost of Goods Sold type.", true, null, null, "Other Costs of Services - COS", false, 13L, false },
                    { 1304L, 1L, null, null, "Use Shipping, freight & delivery - COGS to track the cost of shipping products to customers or distributors.", true, null, null, "Shipping, Freight & Delivery - COS", false, 13L, false },
                    { 1305L, 1L, null, null, "Use Supplies & materials - COGS to track the cost of raw goods and parts used or consumed when producing a product or providing a service.", true, null, null, "Supplies & Materials - COGS", false, 13L, false },
                    { 1401L, 2L, null, null, "Use Advertising/promotional to track money spent promoting your company. You may want different accounts of this type to track different promotional efforts (Yellow Pages, newspaper, radio, flyers, events, and so on). If the promotion effort is a meal, use Promotional meals instead.", true, null, null, "Advertising/Promotional", false, 14L, false },
                    { 1402L, 2L, null, null, "Use Auto to track costs associated with vehicles. You may want different accounts of this type to track gasoline, repairs, and maintenance. If your business owns a car or truck, you may want to track its value as a Fixed Asset, in addition to tracking its expenses.", true, null, null, "Auto", false, 14L, false },
                    { 1403L, 2L, null, null, "Use Bad debt to track debt you have written off.", true, null, null, "Bad Debts", false, 14L, false },
                    { 1404L, 2L, null, null, "Use Bank charges for any fees you pay to financial institutions.", true, null, null, "Bank Charges", false, 14L, false },
                    { 1405L, 2L, null, null, "Use Charitable contributions to track gifts to charity.", true, null, null, "Charitable Contributions", false, 14L, false },
                    { 1406L, 2L, null, null, "Use Cost of labor to track the cost of paying employees to produce products or supply services. It includes all employment costs, including food and transportation, if applicable. This account is also available as a Cost of Goods Sold (COGS) account.", true, null, null, "Cost of Labor", false, 14L, false },
                    { 1407L, 2L, null, null, "Use Dues & subscriptions to track dues & subscriptions related to running your business. You may want different accounts of this type for professional dues, fees for licenses that can’t be transferred, magazines, newspapers, industry publications, or service subscriptions.", true, null, null, "Dues & subscriptions", false, 14L, false },
                    { 1408L, 2L, null, null, "Use Entertainment to track events to entertain employees. If the event is a meal, use Entertainment meals, instead.", true, null, null, "Entertainment", false, 14L, false },
                    { 1409L, 2L, null, null, "Use Entertainment meals to track how much you spend on dining with your employees to promote morale. If you dine with a customer to promote your business, use a Promotional meals account, instead. Be sure to include who you ate with and the purpose of the meal when you enter the transaction.", true, null, null, "Entertainment Meals", false, 14L, false },
                    { 1410L, 2L, null, null, "Use Equipment rental to track the cost of renting equipment to produce products or services. This account is also available as a Cost of Goods (COGS) account. If you purchase equipment, use a Fixed Asset account type called Machinery and equipment.", true, null, null, "Equipment Rental", false, 14L, false },
                    { 1411L, 2L, null, null, "", true, null, null, "Finance costs", false, 14L, false },
                    { 1412L, 2L, null, null, "Use Insurance to track insurance payments. You may want different accounts of this type for different types of insurance (auto, general liability, and so on).", true, null, null, "Insurance", false, 14L, false },
                    { 1413L, 2L, null, null, "Use Interest paid for all types of interest you pay, including mortgage interest, finance charges on credit cards, or interest on loans.", true, null, null, "Interest Paid", false, 14L, false },
                    { 1414L, 2L, null, null, "Use Legal & professional fees to track money to pay to professionals to help you run your business. You may want different accounts of this type for payments to your accountant, lawyer, or other consultants.", true, null, null, "Legal & Professional Fees", false, 14L, false },
                    { 1415L, 2L, null, null, "Use Office/general administrative expenses to track all types of general or office-related expenses.", true, null, null, "Office/General Administrative Expenses", false, 14L, false },
                    { 1416L, 2L, null, null, "Use Other miscellaneous service cost to track costs related to providing services that don’t fall into another Expense type. his account is also available as a Cost of Goods Sold (COGS) account.", true, null, null, "Other Miscellaneous Service Cost", false, 14L, false },
                    { 1417L, 2L, null, null, "Use Payroll expenses to track payroll expenses. You may want different accounts of this type for things like: Compensation of officers Guaranteed payments Workers compensation Salaries and wages Payroll taxes", true, null, null, "Payroll Expenses", false, 14L, false },
                    { 1418L, 2L, null, null, "Use Promotional meals to track how much you spend dining with a customer to promote your business. Be sure to include who you ate with and the purpose of the meal when you enter the transaction.", true, null, null, "Promotional Meals", false, 14L, false },
                    { 1419L, 2L, null, null, "Use Rent or lease of buildings to track rent payments you make.", true, null, null, "Rent or Lease of Buildings", false, 14L, false },
                    { 1420L, 2L, null, null, "Use Repair & maintenance to track any repairs and periodic maintenance fees. You may want different accounts of this type to track different types repair & maintenance expenses (auto, equipment, landscape, and so on).", true, null, null, "Repair & Maintenance", false, 14L, false },
                    { 1421L, 2L, null, null, "Use Shipping, freight & delivery to track the cost of shipping products to customers or distributors. You might use this type of account for incidental shipping expenses, and the Cost of Goods Sold type of Shipping, freight & delivery account for direct costs.", true, null, null, "Shipping, Freight & Delivery", false, 14L, false },
                    { 1422L, 2L, null, null, "Use Supplies & materials to track the cost of raw goods and parts used or consumed when producing a product or providing a service. This account is also available as a Cost of Goods Sold (COGS) account.", true, null, null, "Supplies & Materials", false, 14L, false },
                    { 1423L, 2L, null, null, "Use Taxes paid to track taxes you pay. You may want different accounts of this type for payments to different tax agencies (sales tax, state tax, federal tax).", true, null, null, "Taxes Paid", false, 14L, false },
                    { 1424L, 2L, null, null, "Use Travel to track travel costs. For food you eat while traveling, use Travel meals, instead.", true, null, null, "Travel", false, 14L, false },
                    { 1425L, 2L, null, null, "Use Travel meals to track how much you spend on food while traveling. If you dine with a customer to promote your business, use a Promotional meals account, instead. If you dine with your employees to promote morale, use Entertainment meals, instead.", true, null, null, "Travel Meals", false, 14L, false },
                    { 1426L, 2L, null, null, "Unapplied Cash Bill Payment Expense reports the Cash Basis expense from vendor payment checks you’ve sent but not yet applied to vendor bills. In general, you would never use this directly on a purchase or sale transaction. See IRS Publication 538.", true, null, null, "Unapplied Cash Bill Payment Expense", false, 14L, false },
                    { 1427L, 2L, null, null, "Use Utilities to track utility payments. You may want different accounts of this type to track different types of utility payments (gas and electric, telephone, water, and so on).", true, null, null, "Utilities", false, 14L, false },
                    { 1428L, 2L, null, null, "To track research and development expenses.", true, null, null, "Research and Development", false, 14L, false },
                    { 1429L, 2L, null, null, "To track sales and marketing expenses.", true, null, null, "Sales/Marketing Expenses", false, 14L, false },
                    { 1501L, 2L, null, null, "Use Amortization to track amortization of intangible assets. Amortization is spreading the cost of an intangible asset over its useful life, like depreciation of fixed assets. You may want an amortization account for each intangible asset you have.", true, null, null, "Amortization", false, 15L, false },
                    { 1502L, 2L, null, null, "Use Depreciation to track how much you depreciate fixed assets. You may want a depreciation account for each fixed asset you have.", true, null, null, "Depreciation", false, 15L, false },
                    { 1503L, 2L, null, null, "Use Exchange Gain or Loss to track gains or losses that occur as a result of exchange rate fluctuations.", true, null, null, "Exchange Gain or Loss", false, 15L, false },
                    { 1504L, 2L, null, null, "Use Other miscellaneous expense to track unusual or infrequent expenses that don’t fall into another Other Expense type. If an expense is directly related to providing a service, use an Expense type (not an Other Expense type) account called Other miscellaneous service cost.", true, null, null, "Other Miscellaneous Expense", false, 15L, false },
                    { 1505L, 2L, null, null, "Use Penalties & settlements to track money you pay for violating laws or regulations, settling lawsuits, or other penalties.", true, null, null, "Penalties & Settlements", false, 15L, false }
                });

            migrationBuilder.InsertData(
                table: "PatientHistoryQuestions",
                columns: new[] { "Id", "AdditionalNotes", "AdditionalNotesCaption", "GroupId", "Name", "Order", "ValueType" },
                values: new object[,]
                {
                    { 8L, true, "How much?", 3L, "Recent weight gain", 1, 4 },
                    { 9L, true, "How much?", 3L, "Recent weight loss", 2, 4 },
                    { 10L, false, null, 3L, "Fatique", 3, 4 },
                    { 11L, false, null, 3L, "Weaknes", 4, 4 },
                    { 12L, false, null, 3L, "Fever", 5, 4 },
                    { 13L, false, null, 3L, "Night Sweads", 6, 4 }
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Code", "CountryId", "CreatedBy", "CreatedDate", "DisplayAs", "ISOCode", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1L, "28", 99L, null, null, "AP", "IN-AP", null, null, "Andhra Pradesh" },
                    { 2L, "12", 99L, null, null, "AR", "IN-AR", null, null, "Arunachal Pradesh" },
                    { 3L, "18", 99L, null, null, "AS", "IN-AS", null, null, "Assam" },
                    { 4L, "10", 99L, null, null, "BR", "IN-BR", null, null, "Bihar" },
                    { 5L, "22", 99L, null, null, "CT", "IN-CT", null, null, "Chhattisgarh" },
                    { 6L, "30", 99L, null, null, "GA", "IN-GA", null, null, "Goa" },
                    { 7L, "24", 99L, null, null, "GJ", "IN-GJ", null, null, "Gujarat" },
                    { 8L, "6", 99L, null, null, "HR", "IN-HR", null, null, "Haryana" },
                    { 9L, "2", 99L, null, null, "HP", "IN-HP", null, null, "Himachal Pradesh" },
                    { 10L, "1", 99L, null, null, "JK", "IN-JK", null, null, "Jammu and Kashmir" },
                    { 11L, "20", 99L, null, null, "JH", "IN-JH", null, null, "Jharkhand" },
                    { 12L, "29", 99L, null, null, "KA", "IN-KA", null, null, "Karnataka" },
                    { 13L, "32", 99L, null, null, "KL", "IN-KL", null, null, "Kerala" },
                    { 14L, "23", 99L, null, null, "MP", "IN-MP", null, null, "Madhya Pradesh" },
                    { 15L, "27", 99L, null, null, "MH", "IN-MH", null, null, "Maharashtra" },
                    { 16L, "14", 99L, null, null, "MN", "IN-MN", null, null, "Manipur" },
                    { 17L, "17", 99L, null, null, "ML", "IN-ML", null, null, "Meghalaya" },
                    { 18L, "15", 99L, null, null, "MZ", "IN-MZ", null, null, "Mizoram" },
                    { 19L, "13", 99L, null, null, "NL", "IN-NL", null, null, "Nagaland" },
                    { 20L, "21", 99L, null, null, "OR", "IN-OR", null, null, "Odisha (Orissa,99)" },
                    { 21L, "3", 99L, null, null, "PB", "IN-PB", null, null, "Punjab" },
                    { 22L, "8", 99L, null, null, "RJ", "IN-RJ", null, null, "Rajasthan" },
                    { 23L, "11", 99L, null, null, "SK", "IN-SK", null, null, "Sikkim" },
                    { 24L, "33", 99L, null, null, "TN", "IN-TN", null, null, "Tamil Nadu" },
                    { 25L, "16", 99L, null, null, "TR", "IN-TR", null, null, "Tripura" },
                    { 26L, "9", 99L, null, null, "UP", "IN-UP", null, null, "Uttar Pradesh" },
                    { 27L, "5", 99L, null, null, "UT", "IN-UT", null, null, "Uttarakhand" },
                    { 28L, "19", 99L, null, null, "WB", "IN-WB", null, null, "West Bengal" },
                    { 31L, "35", 99L, null, null, "AN", "IN-AN", null, null, "Andaman and Nicobar Islands" },
                    { 32L, "4", 99L, null, null, "CH", "IN-CH", null, null, "Chandigarh" },
                    { 33L, "26", 99L, null, null, "DN", "IN-DN", null, null, "Dadra and Nagar Haveli" },
                    { 34L, "36", 99L, null, null, "CT", "IN-CT", null, null, "Telangana" },
                    { 35L, "31", 99L, null, null, "LD", "IN-LD", null, null, "Lakshadweep" },
                    { 36L, "7", 99L, null, null, "DL", "IN-DL", null, null, "National Capital Territory of Delhi" },
                    { 37L, "34", 99L, null, null, "PY", "IN-PY", null, null, "Puducherry" },
                    { 38L, "37", 99L, null, null, "HY", "IN-HY", null, null, "Hyderabad" },
                    { 39L, "38", 99L, null, null, "LH", "IN-LH", null, null, "Ladakh" },
                    { 40L, "97", 99L, null, null, "OT", "IN-OT", null, null, "Other Territory" },
                    { 41L, "99", 99L, null, null, "CJ", "IN-CJ", null, null, "Center Jurisdiction" }
                });

            migrationBuilder.InsertData(
                table: "accountgroupandaccountgrouphelp",
                columns: new[] { "AccountGroupHelpId", "AccountGroupId" },
                values: new object[,]
                {
                    { 1L, 301L },
                    { 1L, 302L },
                    { 1L, 303L },
                    { 1L, 304L },
                    { 1L, 305L },
                    { 1L, 306L },
                    { 2L, 302L },
                    { 2L, 303L },
                    { 2L, 304L },
                    { 2L, 305L },
                    { 2L, 306L },
                    { 2L, 307L },
                    { 3L, 1101L },
                    { 3L, 1102L },
                    { 3L, 1103L },
                    { 3L, 1104L },
                    { 3L, 1105L },
                    { 3L, 1106L },
                    { 3L, 1201L },
                    { 3L, 1202L },
                    { 3L, 1203L },
                    { 3L, 1204L },
                    { 3L, 1205L },
                    { 3L, 1301L },
                    { 3L, 1302L },
                    { 3L, 1303L },
                    { 3L, 1304L },
                    { 3L, 1305L },
                    { 4L, 1401L },
                    { 4L, 1402L },
                    { 4L, 1403L },
                    { 4L, 1404L },
                    { 4L, 1405L },
                    { 4L, 1406L },
                    { 4L, 1407L },
                    { 4L, 1408L },
                    { 4L, 1409L },
                    { 4L, 1410L },
                    { 4L, 1411L },
                    { 4L, 1412L },
                    { 4L, 1413L },
                    { 4L, 1414L },
                    { 4L, 1415L },
                    { 4L, 1416L },
                    { 4L, 1417L },
                    { 4L, 1418L },
                    { 4L, 1419L },
                    { 4L, 1420L },
                    { 4L, 1421L },
                    { 4L, 1422L },
                    { 4L, 1423L },
                    { 4L, 1424L },
                    { 4L, 1425L },
                    { 4L, 1426L },
                    { 4L, 1427L },
                    { 5L, 1101L },
                    { 5L, 1102L },
                    { 5L, 1103L },
                    { 5L, 1104L },
                    { 5L, 1105L },
                    { 5L, 1106L },
                    { 7L, 801L },
                    { 7L, 802L },
                    { 7L, 803L },
                    { 7L, 804L },
                    { 7L, 805L },
                    { 7L, 806L },
                    { 7L, 807L },
                    { 7L, 808L },
                    { 7L, 809L },
                    { 7L, 810L },
                    { 7L, 811L },
                    { 7L, 812L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accesses_UserId",
                table: "Accesses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_accountgroupandaccountgrouphelp_AccountGroupId",
                table: "accountgroupandaccountgrouphelp",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_AccountClassificationId",
                table: "AccountGroups",
                column: "AccountClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroups_ParentAccountGroupId",
                table: "AccountGroups",
                column: "ParentAccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountGroupId",
                table: "Accounts",
                column: "AccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CompanyId",
                table: "Accounts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentAccountId",
                table: "Accounts",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalDetails_CompanyId",
                table: "AdditionalDetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalDetails_CostCenterId",
                table: "AdditionalDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalTransactions_AccountId",
                table: "AdditionalTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalTransactions_PurchaseEntryId",
                table: "AdditionalTransactions",
                column: "PurchaseEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalTransactions_SaleEntryId",
                table: "AdditionalTransactions",
                column: "SaleEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_BedTypeId",
                table: "Beds",
                column: "BedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_CompanyId",
                table: "Beds",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ward_Bed_Unique",
                table: "Beds",
                columns: new[] { "WardId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_BedTypes_CompanyId",
                table: "BedTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BillAttachments_BillId",
                table: "BillAttachments",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_AccountId",
                table: "BillDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_BillId",
                table: "BillDetails",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_CompanyId",
                table: "Bills",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_CostCenterId",
                table: "Bills",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_TermId",
                table: "Bills",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_VendorId",
                table: "Bills",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CompanyId",
                table: "CatalogItems",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_DiscountAccountLocalId",
                table: "CatalogItems",
                column: "DiscountAccountLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_InventoryAccountLocalId",
                table: "CatalogItems",
                column: "InventoryAccountLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_ParentId",
                table: "CatalogItems",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_PurchaseAccountLocalId",
                table: "CatalogItems",
                column: "PurchaseAccountLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_SalesAccountLocalId",
                table: "CatalogItems",
                column: "SalesAccountLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_SupplierId",
                table: "CatalogItems",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemSalesTaxMaps_CatalogItemId",
                table: "CatalogItemSalesTaxMaps",
                column: "CatalogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemSalesTaxMaps_SalesTaxMapId",
                table: "CatalogItemSalesTaxMaps",
                column: "SalesTaxMapId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AccountingMethodId",
                table: "Companies",
                column: "AccountingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AccountPayableId",
                table: "Companies",
                column: "AccountPayableId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AccountRecivableId",
                table: "Companies",
                column: "AccountRecivableId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AddressId",
                table: "Companies",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CashOnHandAccountId",
                table: "Companies",
                column: "CashOnHandAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyPurchaseSetupId",
                table: "Companies",
                column: "CompanyPurchaseSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanySalesSetupId",
                table: "Companies",
                column: "CompanySalesSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyStockMovementSetupId",
                table: "Companies",
                column: "CompanyStockMovementSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyTypeId",
                table: "Companies",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ContactInfoId",
                table: "Companies",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CountryId",
                table: "Companies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ExpenseAccountId",
                table: "Companies",
                column: "ExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IncomceAccountId",
                table: "Companies",
                column: "IncomceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ParentCompanyId",
                table: "Companies",
                column: "ParentCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PatientPurchaseAccountId",
                table: "Companies",
                column: "PatientPurchaseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PrimaryCurrencyId",
                table: "Companies",
                column: "PrimaryCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PurchaseAccountId",
                table: "Companies",
                column: "PurchaseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_SalesAccountId",
                table: "Companies",
                column: "SalesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_SalesReturnFeeAccountId",
                table: "Companies",
                column: "SalesReturnFeeAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_StateId",
                table: "Companies",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TaxInfoId",
                table: "Companies",
                column: "TaxInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UndepositedFundAccountId",
                table: "Companies",
                column: "UndepositedFundAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAdditionalTransactionSetups_AccountId",
                table: "CompanyAdditionalTransactionSetups",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFinancialPeriods_CompanyId",
                table: "CompanyFinancialPeriods",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySalesTaxAccountMaps_AccountId",
                table: "CompanySalesTaxAccountMaps",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySalesTaxAccountMaps_CompanyId",
                table: "CompanySalesTaxAccountMaps",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationNotes_CompanyId",
                table: "ConsultationNotes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationNotes_ConsultantId",
                table: "ConsultationNotes",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationNotes_InPatientAdmissionId",
                table: "ConsultationNotes",
                column: "InPatientAdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationNotes_OpRegistrationId",
                table: "ConsultationNotes",
                column: "OpRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationNotes_PatientId",
                table: "ConsultationNotes",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationNotes_SaleEntryId",
                table: "ConsultationNotes",
                column: "SaleEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_CompanyId",
                table: "Consultations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedConsultationFees_CompanyId",
                table: "ConsultedConsultationFees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedConsultationFees_ConsultantId",
                table: "ConsultedConsultationFees",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedConsultationFees_ConsultationId",
                table: "ConsultedConsultationFees",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedConsultationFees_ConsultationNoteId",
                table: "ConsultedConsultationFees",
                column: "ConsultationNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedDoctorConsultationFees_CompanyId",
                table: "ConsultedDoctorConsultationFees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedDoctorConsultationFees_ConsultantId",
                table: "ConsultedDoctorConsultationFees",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedDoctorConsultationFees_ConsultationId",
                table: "ConsultedDoctorConsultationFees",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTestElements_CompanyId",
                table: "ConsultedLabTestElements",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTestElements_ConsLabTestId",
                table: "ConsultedLabTestElements",
                column: "ConsLabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTestElements_MedicalTestElementId",
                table: "ConsultedLabTestElements",
                column: "MedicalTestElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTestElements_UomId",
                table: "ConsultedLabTestElements",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTests_CompanyId",
                table: "ConsultedLabTests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTests_ConsultationNoteId",
                table: "ConsultedLabTests",
                column: "ConsultationNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTests_MedicalTestId",
                table: "ConsultedLabTests",
                column: "MedicalTestId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedPrescriptions_CompanyId",
                table: "ConsultedPrescriptions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedPrescriptions_ConsultationNoteId",
                table: "ConsultedPrescriptions",
                column: "ConsultationNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedPrescriptions_PrescriptionId",
                table: "ConsultedPrescriptions",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_consultedprocedurehistories_CompanyId",
                table: "consultedprocedurehistories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_consultedprocedurehistories_ConsultedProcedureId",
                table: "consultedprocedurehistories",
                column: "ConsultedProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_consultedprocedurehistories_PerformedById",
                table: "consultedprocedurehistories",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedProcedures_CompanyId",
                table: "ConsultedProcedures",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedProcedures_ConsultationNoteId",
                table: "ConsultedProcedures",
                column: "ConsultationNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedProcedures_MedicalProcedureId",
                table: "ConsultedProcedures",
                column: "MedicalProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedProcedures_PerformedById",
                table: "ConsultedProcedures",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedProcedures_RequestedById",
                table: "ConsultedProcedures",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedSymptoms_CompanyId",
                table: "ConsultedSymptoms",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedSymptoms_ConsultationNoteId",
                table: "ConsultedSymptoms",
                column: "ConsultationNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedSymptoms_SymptomId",
                table: "ConsultedSymptoms",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_ParentCompanyId",
                table: "CostCenters",
                column: "ParentCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_AccountingMethodId",
                table: "Countries",
                column: "AccountingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CompanyTypeId",
                table: "Countries",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_DefaultCurrencyId",
                table: "Countries",
                column: "DefaultCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_countrytax_TaxTypeId",
                table: "countrytax",
                column: "TaxTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteDetails_AccountId",
                table: "CreditNoteDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNoteDetails_CreditNoteId",
                table: "CreditNoteDetails",
                column: "CreditNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNotes_CompanyId",
                table: "CreditNotes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNotes_CostCenterId",
                table: "CreditNotes",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditNotes_CustomerId",
                table: "CreditNotes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLicenceDetails_CompanyCustomerLicenseMasterId",
                table: "CustomerLicenceDetails",
                column: "CompanyCustomerLicenseMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLicenceDetails_CompanyId",
                table: "CustomerLicenceDetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLicenceDetails_CustomerId",
                table: "CustomerLicenceDetails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BillingAddressId",
                table: "Customers",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ContactInfoId",
                table: "Customers",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PaymentMethodId",
                table: "Customers",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PaymentTermId",
                table: "Customers",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ShippingAddressId",
                table: "Customers",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_daybooks_AccountId",
                table: "daybooks",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_daybooks_CompanyId",
                table: "daybooks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_daybooks_CostCenterId",
                table: "daybooks",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteDetails_AccountId",
                table: "DebitNoteDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNoteDetails_DebitNoteId",
                table: "DebitNoteDetails",
                column: "DebitNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotes_CompanyId",
                table: "DebitNotes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotes_CostCenterId",
                table: "DebitNotes",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_DebitNotes_SupplierId",
                table: "DebitNotes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DischargeNotes_CompanyId",
                table: "DischargeNotes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DischargeNotes_EmployeeId",
                table: "DischargeNotes",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DischargeNotes_InPatientAdmissionId",
                table: "DischargeNotes",
                column: "InPatientAdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_DischargeNotes_PatientId",
                table: "DischargeNotes",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DischargePrescriptions_CompanyId",
                table: "DischargePrescriptions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DischargePrescriptions_DischargeNoteId",
                table: "DischargePrescriptions",
                column: "DischargeNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_DischargePrescriptions_PrescriptionId",
                table: "DischargePrescriptions",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountDetails_PurchaseDetailsId",
                table: "DiscountDetails",
                column: "PurchaseDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountDetails_PurchaseEntryId",
                table: "DiscountDetails",
                column: "PurchaseEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountDetails_SaleDetailsId",
                table: "DiscountDetails",
                column: "SaleDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountDetails_SaleId",
                table: "DiscountDetails",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCategories_CompanyId",
                table: "DocumentCategories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCategories_ParentId",
                table: "DocumentCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_PatientId",
                table: "EmergencyContacts",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_AddressId",
                table: "employees",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_ContactInfoId",
                table: "employees",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_DepartmentId",
                table: "employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_TaxInfoId",
                table: "employees",
                column: "TaxInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_TitleId",
                table: "employees",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseDetails_AccountId",
                table: "ExpenseDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseDetails_ExpenseId",
                table: "ExpenseDetails",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BankAccountId",
                table: "Expenses",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BankTransferId",
                table: "Expenses",
                column: "BankTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CashAccountId",
                table: "Expenses",
                column: "CashAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CCAccountId",
                table: "Expenses",
                column: "CCAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CompanyId",
                table: "Expenses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CostCenterId",
                table: "Expenses",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PayeeId",
                table: "Expenses",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_PatientId",
                table: "Guardians",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalConfigurations_CompanyId",
                table: "HospitalConfigurations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalConfigurations_IPRegistrationFeeAccountId",
                table: "HospitalConfigurations",
                column: "IPRegistrationFeeAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalConfigurations_OPRegistrationFeeAccountId",
                table: "HospitalConfigurations",
                column: "OPRegistrationFeeAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_IdSpaces_CompanyId",
                table: "IdSpaces",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_IdSpaces_PrintPaperFormat_Id",
                table: "IdSpaces",
                column: "PrintPaperFormat_Id");

            migrationBuilder.CreateIndex(
                name: "IX_InPatientAdmissions_CompanyId",
                table: "InPatientAdmissions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InPatientAdmissions_InsuranceInfoId",
                table: "InPatientAdmissions",
                column: "InsuranceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_InPatientAdmissions_OpRegistrationId",
                table: "InPatientAdmissions",
                column: "OpRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_InPatientAdmissions_PatientId",
                table: "InPatientAdmissions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_InPatientLocations_AdmissionId",
                table: "InPatientLocations",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_InPatientLocations_AuthorizedByDoctorId",
                table: "InPatientLocations",
                column: "AuthorizedByDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InPatientLocations_BedId",
                table: "InPatientLocations",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_InPatientLocations_WardId",
                table: "InPatientLocations",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_insuranceinfoes_CompanyId",
                table: "insuranceinfoes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_insuranceinfoes_EmployerAddressId",
                table: "insuranceinfoes",
                column: "EmployerAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_insuranceinfoes_EmployerContactInfoId",
                table: "insuranceinfoes",
                column: "EmployerContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_insuranceinfoes_InsuranceHolderId",
                table: "insuranceinfoes",
                column: "InsuranceHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_insuranceinfoes_PatientId",
                table: "insuranceinfoes",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_CompanyId",
                table: "Inventories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_CostCenterId",
                table: "Inventories",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_InventoryLocationId",
                table: "Inventories",
                column: "InventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_CompanyId",
                table: "InventoryBatches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_CostCenterId",
                table: "InventoryBatches",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_InventoryId",
                table: "InventoryBatches",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBatches_ProductId",
                table: "InventoryBatches",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_inventorylocations_CompanyId",
                table: "inventorylocations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_inventorylocations_CostCenterId",
                table: "inventorylocations",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_SalesAccountId",
                table: "InvoiceDetails",
                column: "SalesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CompanyId",
                table: "Invoices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CostCenterId",
                table: "Invoices",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TermId",
                table: "Invoices",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSalesTaxMaps_ItemTaxId",
                table: "ItemSalesTaxMaps",
                column: "ItemTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSalesTaxMaps_SalesTaxMapId",
                table: "ItemSalesTaxMaps",
                column: "SalesTaxMapId");

            migrationBuilder.CreateIndex(
                name: "IDX_HsnCode",
                table: "itemtaxes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_itemtaxes_CompanyId",
                table: "itemtaxes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalDetails_JournalId",
                table: "JournalDetails",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalDetails_ReferenceAccountId",
                table: "JournalDetails",
                column: "ReferenceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalDetails_ToAccountId",
                table: "JournalDetails",
                column: "ToAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_CompanyId",
                table: "Journals",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_CostCenterId",
                table: "Journals",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_CompanyId",
                table: "Keywords",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_MedicalProcedureId",
                table: "Keywords",
                column: "MedicalProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_MedicalTestId",
                table: "Keywords",
                column: "MedicalTestId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_SymptomId",
                table: "Keywords",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTestAttachments_ConsLabTestId",
                table: "LabTestAttachments",
                column: "ConsLabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_licenseinfoes_CompanyId",
                table: "licenseinfoes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedureElements_CompanyId",
                table: "MedicalProcedureElements",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedureElements_MedicalProcedureId",
                table: "MedicalProcedureElements",
                column: "MedicalProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedures_CompanyId",
                table: "MedicalProcedures",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTeams_AdmissionId",
                table: "MedicalTeams",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTeams_AuthorizedByDoctorId",
                table: "MedicalTeams",
                column: "AuthorizedByDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTeams_PrimaryCareGiverId",
                table: "MedicalTeams",
                column: "PrimaryCareGiverId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTeams_PrimaryDoctorId",
                table: "MedicalTeams",
                column: "PrimaryDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTeams_SecondaryCareGiverId",
                table: "MedicalTeams",
                column: "SecondaryCareGiverId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTeams_SecondaryDoctorId",
                table: "MedicalTeams",
                column: "SecondaryDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestElements_CompanyId",
                table: "MedicalTestElements",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestElements_MedicalTestId",
                table: "MedicalTestElements",
                column: "MedicalTestId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestElements_UomId",
                table: "MedicalTestElements",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTests_CompanyId",
                table: "MedicalTests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestUOMs_CompanyId",
                table: "MedicalTestUOMs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Narrations_CompanyId",
                table: "Narrations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_CompanyId",
                table: "PatientDocuments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_PatientDocumentCategoryId",
                table: "PatientDocuments",
                column: "PatientDocumentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_PatientId",
                table: "PatientDocuments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientHistoryQuestionGroups_ParentGroupId",
                table: "PatientHistoryQuestionGroups",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientHistoryQuestions_GroupId",
                table: "PatientHistoryQuestions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInvoicePayments_InvoiceId",
                table: "PatientInvoicePayments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInvoicePayments_LedgerId",
                table: "PatientInvoicePayments",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInvoices_CompanyId",
                table: "PatientInvoices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInvoices_PatientId",
                table: "PatientInvoices",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_CompanyId",
                table: "PatientLedgers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_ConsultantId",
                table: "PatientLedgers",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_ConsultationNoteId",
                table: "PatientLedgers",
                column: "ConsultationNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_ConsultedConsultationId",
                table: "PatientLedgers",
                column: "ConsultedConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_ConsultedProcedureId",
                table: "PatientLedgers",
                column: "ConsultedProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_InPatientAdmissionId",
                table: "PatientLedgers",
                column: "InPatientAdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_OpRegistrationId",
                table: "PatientLedgers",
                column: "OpRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_PatientId",
                table: "PatientLedgers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_PatientInvoiceId",
                table: "PatientLedgers",
                column: "PatientInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerTransactionTypeGroupMappings_CompanyId",
                table: "PatientLedgerTransactionTypeGroupMappings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerTransactionTypeGroupMappings_PatientLedgerTrans~",
                table: "PatientLedgerTransactionTypeGroupMappings",
                column: "PatientLedgerTransactionTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerTransactionTypeGroups_CompanyId",
                table: "PatientLedgerTransactionTypeGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPaymentDetails_CompanyId",
                table: "PatientPaymentDetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPaymentDetails_PatientLedgerId",
                table: "PatientPaymentDetails",
                column: "PatientLedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPreMedicalHistories_CompanyId",
                table: "PatientPreMedicalHistories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPreMedicalHistories_HistoryItemId",
                table: "PatientPreMedicalHistories",
                column: "HistoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPreMedicalHistories_PatientId",
                table: "PatientPreMedicalHistories",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ResponsiblePartyId",
                table: "Patients",
                column: "ResponsiblePartyId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_AccountId",
                table: "PaymentDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_PaymentId",
                table: "PaymentDetails",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_AccountId",
                table: "PaymentMethods",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_CompanyId",
                table: "PaymentMethods",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AccountId",
                table: "Payments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BankTransferId",
                table: "Payments",
                column: "BankTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CCAccountId",
                table: "Payments",
                column: "CCAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CompanyId",
                table: "Payments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CostCenterId",
                table: "Payments",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DepositedIntoId",
                table: "Payments",
                column: "DepositedIntoId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerms_CompanyId",
                table: "PaymentTerms",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_AddressId",
                table: "Persons",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CompanyId",
                table: "Persons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ContactInfoId",
                table: "Persons",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PrescribedByDoctorId",
                table: "Prescriptions",
                column: "PrescribedByDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_ProductId",
                table: "Prescriptions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductFamilyId",
                table: "Products",
                column: "ProductFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseadditionaltransaction_PurchaseSetupId",
                table: "purchaseadditionaltransaction",
                column: "PurchaseSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAttachments_PurchaseEntryId",
                table: "PurchaseAttachments",
                column: "PurchaseEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_CompanyId",
                table: "PurchaseDetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_CostCenterId",
                table: "PurchaseDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_ProductId",
                table: "PurchaseDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_PurchaseDetailsId",
                table: "PurchaseDetails",
                column: "PurchaseDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_PurchaseEntryId",
                table: "PurchaseDetails",
                column: "PurchaseEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseentries_AccountId",
                table: "purchaseentries",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseentries_CompanyId",
                table: "purchaseentries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseentries_CostCenterId",
                table: "purchaseentries",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseentries_InventoryLocationId",
                table: "purchaseentries",
                column: "InventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseentries_PurchaseEntryId",
                table: "purchaseentries",
                column: "PurchaseEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptDetails_AccountId",
                table: "ReceiptDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptDetails_ReceiptId",
                table: "ReceiptDetails",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_AccountId",
                table: "Receipts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_BankTransferId",
                table: "Receipts",
                column: "BankTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CCAccountId",
                table: "Receipts",
                column: "CCAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CompanyId",
                table: "Receipts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CostCenterId",
                table: "Receipts",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_DepositedIntoId",
                table: "Receipts",
                column: "DepositedIntoId");

            migrationBuilder.CreateIndex(
                name: "IX_Refereds_CompanyId",
                table: "Refereds",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_CompanyId",
                table: "registrations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_InsuranceInfoId",
                table: "registrations",
                column: "InsuranceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_PatientId",
                table: "registrations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_RequestedDoctorId",
                table: "registrations",
                column: "RequestedDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_BedTypeId",
                table: "Rents",
                column: "BedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_CompanyId",
                table: "Rents",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_rolesystemfunction_RoleId",
                table: "rolesystemfunction",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_saledetails_CompanyId",
                table: "saledetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_saledetails_CostCenterId",
                table: "saledetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_saledetails_ProductId",
                table: "saledetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_saledetails_SaleDetailId",
                table: "saledetails",
                column: "SaleDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_saledetails_SaleId",
                table: "saledetails",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_AccountsId",
                table: "saleentries",
                column: "AccountsId");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_CompanyId",
                table: "saleentries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_CostCenterId",
                table: "saleentries",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_InventoryLocationId",
                table: "saleentries",
                column: "InventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_PaymentId",
                table: "saleentries",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_ReferedById",
                table: "saleentries",
                column: "ReferedById");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_SaleEntryId",
                table: "saleentries",
                column: "SaleEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_SoldById",
                table: "saleentries",
                column: "SoldById");

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_StateId",
                table: "saleentries",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_salesadditionaltransaction_SalesSetupId",
                table: "salesadditionaltransaction",
                column: "SalesSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovementdetails_CompanyId",
                table: "stockmovementdetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovementdetails_CostCenterId",
                table: "stockmovementdetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovementdetails_ProductId",
                table: "stockmovementdetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovementdetails_StockMovementId",
                table: "stockmovementdetails",
                column: "StockMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_CompanyId",
                table: "stockmovements",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_CostCenterId",
                table: "stockmovements",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_InventoryLocationFromId",
                table: "stockmovements",
                column: "InventoryLocationFromId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_InventoryLocationToId",
                table: "stockmovements",
                column: "InventoryLocationToId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_InventoryStockLocationId",
                table: "stockmovements",
                column: "InventoryStockLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_PatientId",
                table: "stockmovements",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_PurchaseEntryId",
                table: "stockmovements",
                column: "PurchaseEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_PurchaseReturnEntryId",
                table: "stockmovements",
                column: "PurchaseReturnEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_RequestInventoryLocationId",
                table: "stockmovements",
                column: "RequestInventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_SaleId",
                table: "stockmovements",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_SaleReturnId",
                table: "stockmovements",
                column: "SaleReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_StockMovementOutId",
                table: "stockmovements",
                column: "StockMovementOutId");

            migrationBuilder.CreateIndex(
                name: "IX_stockmovements_StockOutId",
                table: "stockmovements",
                column: "StockOutId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierLicenceDetails_CompanyId",
                table: "SupplierLicenceDetails",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierLicenceDetails_CompanySupplierLicenseMasterId",
                table: "SupplierLicenceDetails",
                column: "CompanySupplierLicenseMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierLicenceDetails_SupplierId",
                table: "SupplierLicenceDetails",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_AddressId",
                table: "Suppliers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ContactInfoId",
                table: "Suppliers",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TaxInfoId",
                table: "Suppliers",
                column: "TaxInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Symptoms_CompanyId",
                table: "Symptoms",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_PurchaseDetailsId",
                table: "TaxDetails",
                column: "PurchaseDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_PurchaseEntryId",
                table: "TaxDetails",
                column: "PurchaseEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_SaleDetailsId",
                table: "TaxDetails",
                column: "SaleDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_SaleId",
                table: "TaxDetails",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_TaxAccountId",
                table: "TaxDetails",
                column: "TaxAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_CompanyId",
                table: "Titles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_userrole_UserId",
                table: "userrole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ContactInfoId",
                table: "Users",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TaxInfoId",
                table: "Users",
                column: "TaxInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_CompanyId",
                table: "Vitals",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_InPatientAdmissionId",
                table: "Vitals",
                column: "InPatientAdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_OpRegistrationId",
                table: "Vitals",
                column: "OpRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_PatientId",
                table: "Vitals",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_CompanyId",
                table: "Wards",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_InventoryLocationId",
                table: "Wards",
                column: "InventoryLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accesses_Users_UserId",
                table: "Accesses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Companies_CompanyId",
                table: "Accounts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalDetails_Companies_CompanyId",
                table: "AdditionalDetails",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalDetails_CostCenters_CostCenterId",
                table: "AdditionalDetails",
                column: "CostCenterId",
                principalTable: "CostCenters",
                principalColumn: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalTransactions_purchaseentries_PurchaseEntryId",
                table: "AdditionalTransactions",
                column: "PurchaseEntryId",
                principalTable: "purchaseentries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalTransactions_saleentries_SaleEntryId",
                table: "AdditionalTransactions",
                column: "SaleEntryId",
                principalTable: "saleentries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_BedTypes_BedTypeId",
                table: "Beds",
                column: "BedTypeId",
                principalTable: "BedTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Companies_CompanyId",
                table: "Beds",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Wards_WardId",
                table: "Beds",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BedTypes_Companies_CompanyId",
                table: "BedTypes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillAttachments_Bills_BillId",
                table: "BillAttachments",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "BillId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Bills_BillId",
                table: "BillDetails",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "BillId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Companies_CompanyId",
                table: "Bills",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_CostCenters_CostCenterId",
                table: "Bills",
                column: "CostCenterId",
                principalTable: "CostCenters",
                principalColumn: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_PaymentTerms_TermId",
                table: "Bills",
                column: "TermId",
                principalTable: "PaymentTerms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItems_Companies_CompanyId",
                table: "CatalogItems",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogItemSalesTaxMaps_CompanySalesTaxAccountMaps_SalesTaxM~",
                table: "CatalogItemSalesTaxMaps",
                column: "SalesTaxMapId",
                principalTable: "CompanySalesTaxAccountMaps",
                principalColumn: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Customers_PatientPurchaseAccountId",
                table: "Companies",
                column: "PatientPurchaseAccountId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountGroups_AccountGroupId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Companies_CompanyId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Companies_CompanyId",
                table: "PaymentMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerms_Companies_CompanyId",
                table: "PaymentTerms");

            migrationBuilder.DropTable(
                name: "Accesses");

            migrationBuilder.DropTable(
                name: "accountgroupandaccountgrouphelp");

            migrationBuilder.DropTable(
                name: "AdditionalDetails");

            migrationBuilder.DropTable(
                name: "AdditionalTransactions");

            migrationBuilder.DropTable(
                name: "BillAttachments");

            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DropTable(
                name: "CatalogItemSalesTaxMaps");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CompanyFinancialPeriods");

            migrationBuilder.DropTable(
                name: "ConsultedDoctorConsultationFees");

            migrationBuilder.DropTable(
                name: "ConsultedLabTestElements");

            migrationBuilder.DropTable(
                name: "ConsultedPrescriptions");

            migrationBuilder.DropTable(
                name: "consultedprocedurehistories");

            migrationBuilder.DropTable(
                name: "ConsultedSymptoms");

            migrationBuilder.DropTable(
                name: "countrytax");

            migrationBuilder.DropTable(
                name: "CreditNoteDetails");

            migrationBuilder.DropTable(
                name: "CustomerLicenceDetails");

            migrationBuilder.DropTable(
                name: "daybooks");

            migrationBuilder.DropTable(
                name: "DebitNoteDetails");

            migrationBuilder.DropTable(
                name: "DischargePrescriptions");

            migrationBuilder.DropTable(
                name: "DiscountDetails");

            migrationBuilder.DropTable(
                name: "EmergencyContacts");

            migrationBuilder.DropTable(
                name: "ExpenseDetails");

            migrationBuilder.DropTable(
                name: "Guardians");

            migrationBuilder.DropTable(
                name: "HospitalConfigurations");

            migrationBuilder.DropTable(
                name: "IdSpaceEntryTypeDetails");

            migrationBuilder.DropTable(
                name: "IdSpaces");

            migrationBuilder.DropTable(
                name: "InPatientLocations");

            migrationBuilder.DropTable(
                name: "InventoryBatches");

            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "ItemSalesTaxMaps");

            migrationBuilder.DropTable(
                name: "JournalDetails");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "LabTestAttachments");

            migrationBuilder.DropTable(
                name: "MedicalProcedureElements");

            migrationBuilder.DropTable(
                name: "MedicalTeams");

            migrationBuilder.DropTable(
                name: "Narrations");

            migrationBuilder.DropTable(
                name: "PatientDocuments");

            migrationBuilder.DropTable(
                name: "PatientIds");

            migrationBuilder.DropTable(
                name: "PatientInvoicePayments");

            migrationBuilder.DropTable(
                name: "PatientLedgerTransactionTypeGroupMappings");

            migrationBuilder.DropTable(
                name: "PatientPaymentDetails");

            migrationBuilder.DropTable(
                name: "PatientPreMedicalHistories");

            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "purchaseadditionaltransaction");

            migrationBuilder.DropTable(
                name: "PurchaseAttachments");

            migrationBuilder.DropTable(
                name: "ReceiptDetails");

            migrationBuilder.DropTable(
                name: "Rents");

            migrationBuilder.DropTable(
                name: "rolesystemfunction");

            migrationBuilder.DropTable(
                name: "salesadditionaltransaction");

            migrationBuilder.DropTable(
                name: "SeedDataHistories");

            migrationBuilder.DropTable(
                name: "stockmovementdetails");

            migrationBuilder.DropTable(
                name: "SupplierLicenceDetails");

            migrationBuilder.DropTable(
                name: "TaxDetails");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "userrole");

            migrationBuilder.DropTable(
                name: "Vitals");

            migrationBuilder.DropTable(
                name: "accountgroupforhelps");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "MedicalTestElements");

            migrationBuilder.DropTable(
                name: "taxdocumenttypes");

            migrationBuilder.DropTable(
                name: "CreditNotes");

            migrationBuilder.DropTable(
                name: "DebitNotes");

            migrationBuilder.DropTable(
                name: "DischargeNotes");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "PrintPaperFormats");

            migrationBuilder.DropTable(
                name: "Beds");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "CompanySalesTaxAccountMaps");

            migrationBuilder.DropTable(
                name: "itemtaxes");

            migrationBuilder.DropTable(
                name: "Journals");

            migrationBuilder.DropTable(
                name: "Symptoms");

            migrationBuilder.DropTable(
                name: "ConsultedLabTests");

            migrationBuilder.DropTable(
                name: "DocumentCategories");

            migrationBuilder.DropTable(
                name: "PatientLedgerTransactionTypeGroups");

            migrationBuilder.DropTable(
                name: "PatientLedgers");

            migrationBuilder.DropTable(
                name: "PatientHistoryQuestions");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropTable(
                name: "SystemFunctions");

            migrationBuilder.DropTable(
                name: "CompanyAdditionalTransactionSetups");

            migrationBuilder.DropTable(
                name: "stockmovements");

            migrationBuilder.DropTable(
                name: "licenseinfoes");

            migrationBuilder.DropTable(
                name: "PurchaseDetails");

            migrationBuilder.DropTable(
                name: "saledetails");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "MedicalTestUOMs");

            migrationBuilder.DropTable(
                name: "BedTypes");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "MedicalTests");

            migrationBuilder.DropTable(
                name: "ConsultedConsultationFees");

            migrationBuilder.DropTable(
                name: "ConsultedProcedures");

            migrationBuilder.DropTable(
                name: "PatientInvoices");

            migrationBuilder.DropTable(
                name: "PatientHistoryQuestionGroups");

            migrationBuilder.DropTable(
                name: "purchaseentries");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "ConsultationNotes");

            migrationBuilder.DropTable(
                name: "MedicalProcedures");

            migrationBuilder.DropTable(
                name: "ProductFamilies");

            migrationBuilder.DropTable(
                name: "InPatientAdmissions");

            migrationBuilder.DropTable(
                name: "saleentries");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CatalogItems");

            migrationBuilder.DropTable(
                name: "registrations");

            migrationBuilder.DropTable(
                name: "inventorylocations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Refereds");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "insuranceinfoes");

            migrationBuilder.DropTable(
                name: "CostCenters");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "AccountGroups");

            migrationBuilder.DropTable(
                name: "accountgroupclassifications");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyPurchaseSetups");

            migrationBuilder.DropTable(
                name: "CompanySalesSetups");

            migrationBuilder.DropTable(
                name: "CompanyStockMovementSetups");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "taxinfoes");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "contactinfoes");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "PaymentTerms");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountingMethods");

            migrationBuilder.DropTable(
                name: "CompanyTypes");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
