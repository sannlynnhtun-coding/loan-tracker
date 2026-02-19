USE [master]
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'LoanTracker')
BEGIN
    DROP DATABASE [LoanTracker];
END
GO

CREATE DATABASE [LoanTracker];
GO

USE [LoanTracker];
GO

-- 1. Table [Tbl_Customer]
CREATE TABLE [Tbl_Customer] (
    [CustomerId] INT IDENTITY(1,1) PRIMARY KEY,
    [CustomerName] NVARCHAR(100) NOT NULL,
    [Nrc] NVARCHAR(50) NOT NULL,
    [MobileNo] NVARCHAR(20) NOT NULL,
    [Address] NVARCHAR(MAX),
    [CreatedDate] DATETIME DEFAULT GETDATE()
);

-- 2. Table [Tbl_LoanType]
CREATE TABLE [Tbl_LoanType] (
    [LoanTypeId] INT IDENTITY(1,1) PRIMARY KEY,
    [LoanTypeName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX),
    [CreatedDate] DATETIME DEFAULT GETDATE()
);

-- 3. Table [Tbl_LoanTypeBurmese]
CREATE TABLE [Tbl_LoanTypeBurmese] (
    [LoanTypeId] INT IDENTITY(1,1) PRIMARY KEY,
    [LoanTypeName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX),
    [CreatedDate] DATETIME DEFAULT GETDATE(),
    [EnglishLoanTypeId] INT NULL,
    CONSTRAINT [FK_Tbl_LoanTypeBurmese_Tbl_LoanType] FOREIGN KEY ([EnglishLoanTypeId]) REFERENCES [Tbl_LoanType]([LoanTypeId])
);

-- 4. Table [Tbl_CustomerLoan]
CREATE TABLE [Tbl_CustomerLoan] (
    [LoanId] INT IDENTITY(1,1) PRIMARY KEY,
    [CustomerId] INT NOT NULL,
    [LoanTypeId] INT NOT NULL,
    [TotalAmount] DECIMAL(18,2) NOT NULL,
    [PrincipalAmount] DECIMAL(18,2) NOT NULL,
    [InterestRate] DECIMAL(18,2) NOT NULL,
    [LoanTerm] INT NOT NULL,
    [LoanStartDate] DATE NOT NULL,
    [RepaymentFrequency] NVARCHAR(50),
    [Status] NVARCHAR(50),
    [CreatedDate] DATETIME DEFAULT GETDATE(),
    CONSTRAINT [FK_Tbl_CustomerLoan_Tbl_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Tbl_Customer]([CustomerId]),
    CONSTRAINT [FK_Tbl_CustomerLoan_Tbl_LoanType] FOREIGN KEY ([LoanTypeId]) REFERENCES [Tbl_LoanType]([LoanTypeId])
);

-- 5. Table [Tbl_PaymentSchedule]
CREATE TABLE [Tbl_PaymentSchedule] (
    [ScheduleId] INT IDENTITY(1,1) PRIMARY KEY,
    [LoanId] INT NOT NULL,
    [DueDate] DATE NOT NULL,
    [InstallmentAmount] DECIMAL(18,2) NOT NULL,
    [PrincipalComponent] DECIMAL(18,2) NOT NULL,
    [InterestComponent] DECIMAL(18,2) NOT NULL,
    [RemainingBalance] DECIMAL(18,2) NOT NULL,
    [Status] NVARCHAR(50),
    CONSTRAINT [FK_Tbl_PaymentSchedule_Tbl_CustomerLoan] FOREIGN KEY ([LoanId]) REFERENCES [Tbl_CustomerLoan]([LoanId])
);

-- 6. Table [Tbl_CustomerPayment]
CREATE TABLE [Tbl_CustomerPayment] (
    [PaymentId] INT IDENTITY(1,1) PRIMARY KEY,
    [ScheduleId] INT NOT NULL,
    [PaymentDate] DATE NOT NULL,
    [AmountPaid] DECIMAL(18,2) NOT NULL,
    [LateFee] DECIMAL(18,2) NULL,
    [Status] NVARCHAR(50),
    CONSTRAINT [FK_Tbl_CustomerPayment_Tbl_PaymentSchedule] FOREIGN KEY ([ScheduleId]) REFERENCES [Tbl_PaymentSchedule]([ScheduleId])
);

-- 7. Table [Tbl_LateFee]
CREATE TABLE [Tbl_LateFee] (
    [LateFeeId] INT IDENTITY(1,1) PRIMARY KEY,
    [ScheduleId] INT NOT NULL,
    [OverdueDays] INT NOT NULL,
    [LateFeeAmount] DECIMAL(18,2) NOT NULL,
    CONSTRAINT [FK_Tbl_LateFee_Tbl_PaymentSchedule] FOREIGN KEY ([ScheduleId]) REFERENCES [Tbl_PaymentSchedule]([ScheduleId])
);
GO

-- SEED DATA

-- Customers
INSERT INTO [Tbl_Customer] ([CustomerName], [Nrc], [MobileNo], [Address])
VALUES 
('Hsu Myat Noe', '12/DAGANA(N)123456', '09123456789', 'No. (123), Pyay Road, Yangon'),
('Kyaw Zayar Hein', '9/MAHAMA(N)654321', '09987654321', 'Mandalay Street, Mandalay'),
('Aye Aye Thant', '7/YAKANA(N)111222', '09456123789', 'Bago Township, Bago');

-- Loan Types
INSERT INTO [Tbl_LoanType] ([LoanTypeName], [Description])
VALUES 
('Personal Loan', 'Standard personal loan for general needs'),
('Business Growth Loan', 'Capital for small and medium enterprises'),
('Home Loan', 'Financing for real estate and home renovations');

-- English-Burmese Mappings
INSERT INTO [Tbl_LoanTypeBurmese] ([LoanTypeName], [Description], [EnglishLoanTypeId])
VALUES 
(N'ကိုယ်ရေးကိုယ်တာ ချေးငွေ', N'အထွေထွေ လိုအပ်ချက်များအတွက် ကိုယ်ရေးကိုယ်တာ ချေးငွေ', 1),
(N'လုပ်ငန်းတိုးချဲ့ရေး ချေးငွေ', N'အသေးစားနှင့် အလတ်စား စီးပွားရေးလုပ်ငန်းများအတွက် မတည်ရင်းနှီးငွေ', 2),
(N'အိမ်ရာချေးငွေ', N'အိမ်ခြံမြေနှင့် အိမ်ပြင်ဆင်ရေးအတွက် ငွေကြေးထောက်ပံ့မှု', 3);

-- Active Loans
-- For Hsu Myat Noe (ID: 1), Personal Loan (ID:1)
INSERT INTO [Tbl_CustomerLoan] ([CustomerId], [LoanTypeId], [PrincipalAmount], [InterestRate], [TotalAmount], [LoanTerm], [LoanStartDate], [RepaymentFrequency], [Status])
VALUES (1, 1, 10000.00, 5.0, 10500.00, 12, '2024-01-01', 'Monthly', 'Active');

-- For Kyaw Zayar Hein (ID: 2), Business Loan (ID:2)
INSERT INTO [Tbl_CustomerLoan] ([CustomerId], [LoanTypeId], [PrincipalAmount], [InterestRate], [TotalAmount], [LoanTerm], [LoanStartDate], [RepaymentFrequency], [Status])
VALUES (2, 2, 50000.00, 8.0, 54000.00, 24, '2024-02-15', 'Monthly', 'Active');

-- Payment Schedules for Loan 1 (Simplistic)
INSERT INTO [Tbl_PaymentSchedule] ([LoanId], [DueDate], [InstallmentAmount], [PrincipalComponent], [InterestComponent], [RemainingBalance], [Status])
VALUES 
(1, '2024-02-01', 875.00, 833.33, 41.67, 9625.00, 'Paid'),
(1, '2024-03-01', 875.00, 833.33, 41.67, 8750.00, 'Pending');

-- Payments for Loan 1
INSERT INTO [Tbl_CustomerPayment] ([ScheduleId], [PaymentDate], [AmountPaid], [LateFee], [Status])
VALUES (1, '2024-02-01', 875.00, 0.00, 'Completed');
GO
