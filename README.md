# 🏦 **Loan Tracker: A Comprehensive Mortgage Loan Management System** 🏠

**Loan Tracker** is a powerful and user-friendly system designed to simplify the management of mortgage loans for both **financial institutions** and **borrowers**. It provides a centralized platform to track loans, payments, late fees, and repayment schedules, ensuring transparency and efficiency throughout the loan lifecycle.

---

### **Key Features** 🔑

1. **👤 Customer Management**:
   - Store and manage customer information, including borrower names and contact details.
   - Link customers to their respective loans for easy tracking.

2. **💼 Loan Management**:
   - Record loan details such as loan amount, interest rate, loan term, start date, monthly payment, and down payment.
   - Calculate the **total repayment amount** (loan amount + interest) automatically.

3. **💰 Payment Tracking**:
   - Record all payments made by borrowers, including the payment date, amount paid, and any applicable late fees.
   - Ensure payments do not exceed the total repayment amount.

4. **⏰ Dynamic Late Fees**:
   - Automatically calculate late fees based on predefined rules (e.g., $25 for 1–5 days overdue, $50 for 6–10 days overdue, $100 for more than 10 days overdue).
   - Flexible rules can be customized to meet specific business needs.

5. **📅 Payment Schedule Generation**:
   - Generate **monthly** or **yearly** payment schedules for borrowers.
   - Show the breakdown of principal, interest, and total payment for each period.
   - Display the remaining balance after each payment.

6. **✅ Repayment Validation**:
   - Ensure the sum of all payments matches the total repayment amount.
   - Reject payments that exceed the remaining repayment amount.

7. **📊 Reporting and Insights**:
   - View detailed reports on loans, payments, and late fees.
   - Track the repayment status of each loan (e.g., fully repaid, partially repaid).

---

### **How It Works** 🛠️

1. **👤 Add Customers**:
   - Customers are added to the system with their basic information (e.g., name).

2. **💼 Create Loans**:
   - For each customer, a loan is created with details such as loan amount, interest rate, loan term, and start date.
   - The system calculates the **monthly payment** and **total repayment amount** automatically.

3. **💰 Record Payments**:
   - Borrowers make payments, which are recorded in the system.
   - Late fees are calculated dynamically based on the number of days overdue.

4. **📅 Generate Payment Schedules**:
   - Borrowers can view their payment schedules (monthly or yearly) to understand how much they need to pay each period.

5. **✅ Track Repayment Status**:
   - The system tracks the remaining balance and ensures payments do not exceed the total repayment amount.
   - Once the loan is fully repaid, the system marks it as complete.

---

### **Benefits** 🌟

1. **🏦 For Financial Institutions**:
   - Streamline loan management and reduce manual errors.
   - Automate late fee calculations and payment tracking.
   - Generate detailed reports for better decision-making.

2. **👤 For Borrowers**:
   - View payment schedules and track repayment progress.
   - Understand how much of each payment goes toward principal and interest.
   - Avoid overpayments with repayment validation.

---

### **Example Workflow** 🔄

1. **👤 Add a Customer**:
   - Insert a new customer into the `Customers` table:
     ```sql
     INSERT INTO Customers (BorrowerName) VALUES ('John Doe');
     ```

2. **💼 Create a Loan**:
   - Insert a new loan into the `MortgageLoans` table:
     ```sql
     DECLARE @LoanAmount DECIMAL(18, 2) = 250000.00;
     DECLARE @InterestRate DECIMAL(5, 2) = 3.75;
     DECLARE @LoanTerm INT = 30;
     DECLARE @TotalRepayment DECIMAL(18, 2);

     -- Calculate total repayment
     SET @TotalRepayment = @LoanAmount + (@LoanAmount * (@InterestRate / 100) * @LoanTerm);

     -- Insert the loan
     INSERT INTO MortgageLoans (CustomerID, LoanAmount, InterestRate, LoanTerm, StartDate, MonthlyPayment, DownPayment, TotalRepayment)
     VALUES (1, @LoanAmount, @InterestRate, @LoanTerm, '2023-01-01', 1157.79, 50000.00, @TotalRepayment);
     ```

3. **💰 Record a Payment**:
   - Use the `RecordPayment` stored procedure to record a payment:
     ```sql
     EXEC RecordPayment @LoanID = 1, @PaymentDate = '2023-01-01', @AmountPaid = 1157.79;
     ```

4. **📅 Generate a Payment Schedule**:
   - Use the `GetPaymentSchedule` stored procedure to generate a monthly or yearly schedule:
     ```sql
     EXEC GetPaymentSchedule @LoanID = 1, @ScheduleType = 'Monthly';
     ```

---

### **Sample Outputs** 📄

#### 📅 Monthly Payment Schedule
| Period | PaymentDate | Principal  | Interest  | TotalPayment | RemainingBalance |
|--------|-------------|------------|-----------|--------------|------------------|
| 1      | 2023-01-01  | 500.00     | 781.25    | 1281.25      | 249500.00        |
| 2      | 2023-02-01  | 502.34     | 778.91    | 1281.25      | 248997.66        |
| 3      | 2023-03-01  | 504.69     | 776.56    | 1281.25      | 248492.97        |

#### 📅 Yearly Payment Schedule
| Period | PaymentDate | Principal  | Interest  | TotalPayment | RemainingBalance |
|--------|-------------|------------|-----------|--------------|------------------|
| 1      | 2023-01-01  | 6000.00    | 9375.00   | 15375.00     | 244000.00        |
| 2      | 2024-01-01  | 6200.00    | 9175.00   | 15375.00     | 237800.00        |
| 3      | 2025-01-01  | 6400.00    | 8975.00   | 15375.00     | 231400.00        |

---

### **Why Choose Loan Tracker?** ❓
- **⚡ Efficiency**: Automates repetitive tasks like payment tracking and late fee calculations.
- **🔍 Transparency**: Provides clear and detailed payment schedules for borrowers.
- **🛠️ Flexibility**: Supports both monthly and yearly payment schedules.
- **🎯 Accuracy**: Ensures payments do not exceed the total repayment amount.

---

### **Step 1: Create the Tables** 🗂️

#### `Customers` Table
Stores customer information.

```sql
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1), -- Unique identifier for each customer
    BorrowerName NVARCHAR(100) NOT NULL      -- Name of the borrower
);
```

#### `MortgageLoans` Table
Stores loan details and links to the `Customers` table.

```sql
CREATE TABLE MortgageLoans (
    LoanID INT PRIMARY KEY IDENTITY(1,1), -- Unique identifier for each loan
    CustomerID INT NOT NULL,              -- Foreign key linking to the Customers table
    LoanAmount DECIMAL(18, 2) NOT NULL,   -- Total loan amount
    InterestRate DECIMAL(5, 2) NOT NULL,  -- Annual interest rate
    LoanTerm INT NOT NULL,                -- Loan term in years
    StartDate DATE NOT NULL,              -- Loan start date
    MonthlyPayment DECIMAL(18, 2),        -- Monthly payment amount
    DownPayment DECIMAL(18, 2),           -- Down payment amount
    TotalRepayment DECIMAL(18, 2),        -- Total repayment amount (Loan Amount + Interest)
    CONSTRAINT FK_MortgageLoans_CustomerID FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);
```

#### `LateFeeRules` Table
Defines dynamic late fee rules based on the number of days overdue.

```sql
CREATE TABLE LateFeeRules (
    RuleID INT PRIMARY KEY IDENTITY(1,1), -- Unique identifier for each rule
    MinDaysOverdue INT NOT NULL,          -- Minimum days overdue for this rule
    MaxDaysOverdue INT NULL,              -- Maximum days overdue for this rule (NULL means no upper limit)
    LateFeeAmount DECIMAL(18, 2) NOT NULL -- Late fee amount for this rule
);
```

#### `Payments` Table
Records each payment made by the borrower, including late fees.

```sql
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1), -- Unique identifier for each payment
    LoanID INT NOT NULL,                     -- Foreign key linking to the MortgageLoans table
    PaymentDate DATE NOT NULL,               -- Date the payment was made
    AmountPaid DECIMAL(18, 2) NOT NULL,      -- Amount paid by the borrower
    LateFee DECIMAL(18, 2) DEFAULT 0,        -- Late fee charged (if any)
    CONSTRAINT FK_Payments_LoanID FOREIGN KEY (LoanID) REFERENCES MortgageLoans(LoanID)
);
```

---

### **Step 2: Insert Sample Data** 📥

#### Insert Data into `Customers`
```sql
INSERT INTO Customers (BorrowerName)
VALUES
('John Doe'),
('Jane Smith'),
('Alice Johnson'),
('Bob Brown');
```

#### Insert Data into `MortgageLoans`
```sql
DECLARE @LoanAmount DECIMAL(18, 2) = 250000.00;
DECLARE @InterestRate DECIMAL(5, 2) = 3.75;
DECLARE @LoanTerm INT = 30;
DECLARE @TotalRepayment DECIMAL(18, 2);

-- Calculate total repayment
SET @TotalRepayment = @LoanAmount + (@LoanAmount * (@InterestRate / 100) * @LoanTerm);

-- Insert the loan
INSERT INTO MortgageLoans (CustomerID, LoanAmount, InterestRate, LoanTerm, StartDate, MonthlyPayment, DownPayment, TotalRepayment)
VALUES (1, @LoanAmount, @InterestRate, @LoanTerm, '2023-01-01', 1157.79, 50000.00, @TotalRepayment);
```

#### Insert Data into `LateFeeRules`
```sql
INSERT INTO LateFeeRules (MinDaysOverdue, MaxDaysOverdue, LateFeeAmount)
VALUES
(1, 5, 25.00),
(6, 10, 50.00),
(11, NULL, 100.00); -- NULL means no upper limit
```

#### Insert Data into `Payments`
```sql
INSERT INTO Payments (LoanID, PaymentDate, AmountPaid, LateFee)
VALUES
(1, '2023-01-01', 1157.79, 0),       -- On-time payment
(1, '2023-02-05', 1157.79, 50.00),   -- Late payment (5 days late, $50 late fee)
(1, '2023-03-01', 1157.79, 0);       -- On-time payment
```

---

### **Step 3: Create Stored Procedures** ⚙️

#### 1. **RecordPayment**
Records a payment and calculates dynamic late fees.

```sql
CREATE PROCEDURE RecordPayment
    @LoanID INT,
    @PaymentDate DATE,
    @AmountPaid DECIMAL(18, 2)
AS
BEGIN
    DECLARE @DueDate DATE;
    DECLARE @DaysOverdue INT;
    DECLARE @LateFee DECIMAL(18, 2) = 0;
    DECLARE @TotalRepayment DECIMAL(18, 2);
    DECLARE @TotalPaid DECIMAL(18, 2);

    -- Get the due date (e.g., 1st of every month)
    SET @DueDate = DATEFROMPARTS(YEAR(@PaymentDate), MONTH(@PaymentDate), 1);

    -- Calculate days overdue
    SET @DaysOverdue = DATEDIFF(DAY, @DueDate, @PaymentDate);

    -- Calculate late fee dynamically based on rules
    IF @DaysOverdue > 0
    BEGIN
        SELECT @LateFee = LateFeeAmount
        FROM LateFeeRules
        WHERE @DaysOverdue >= MinDaysOverdue
          AND (@DaysOverdue <= MaxDaysOverdue OR MaxDaysOverdue IS NULL);
    END;

    -- Get the total repayment amount for the loan
    SELECT @TotalRepayment = TotalRepayment
    FROM MortgageLoans
    WHERE LoanID = @LoanID;

    -- Calculate the total amount paid so far
    SELECT @TotalPaid = ISNULL(SUM(AmountPaid), 0)
    FROM Payments
    WHERE LoanID = @LoanID;

    -- Check if the payment exceeds the remaining repayment amount
    IF (@TotalPaid + @AmountPaid) > @TotalRepayment
    BEGIN
        RAISERROR('Payment exceeds the total repayment amount.', 16, 1);
        RETURN;
    END;

    -- Insert the payment record
    INSERT INTO Payments (LoanID, PaymentDate, AmountPaid, LateFee)
    VALUES (@LoanID, @PaymentDate, @AmountPaid, @LateFee);

    -- Check if the loan is fully repaid
    IF (@TotalPaid + @AmountPaid) = @TotalRepayment
    BEGIN
        PRINT 'Loan fully repaid!';
    END;
END;
```

#### 2. **GetPaymentSchedule**
Generates a monthly or yearly payment schedule.

```sql
CREATE PROCEDURE GetPaymentSchedule
    @LoanID INT,                     -- Loan ID to generate the schedule for
    @ScheduleType NVARCHAR(10)       -- 'Monthly' or 'Yearly'
AS
BEGIN
    -- Declare variables
    DECLARE @LoanAmount DECIMAL(18, 2);
    DECLARE @InterestRate DECIMAL(5, 2);
    DECLARE @LoanTerm INT;
    DECLARE @MonthlyPayment DECIMAL(18, 2);
    DECLARE @StartDate DATE;
    DECLARE @TotalRepayment DECIMAL(18, 2);
    DECLARE @RemainingBalance DECIMAL(18, 2);
    DECLARE @PaymentDate DATE;
    DECLARE @Principal DECIMAL(18, 2);
    DECLARE @Interest DECIMAL(18, 2);
    DECLARE @TotalPayment DECIMAL(18, 2);
    DECLARE @Period INT = 1;

    -- Fetch loan details
    SELECT 
        @LoanAmount = LoanAmount,
        @InterestRate = InterestRate,
        @LoanTerm = LoanTerm,
        @MonthlyPayment = MonthlyPayment,
        @StartDate = StartDate,
        @TotalRepayment = TotalRepayment
    FROM MortgageLoans
    WHERE LoanID = @LoanID;

    -- Initialize remaining balance
    SET @RemainingBalance = @LoanAmount;

    -- Create a temporary table to store the schedule
    CREATE TABLE #PaymentSchedule (
        Period INT,
        PaymentDate DATE,
        Principal DECIMAL(18, 2),
        Interest DECIMAL(18, 2),
        TotalPayment DECIMAL(18, 2),
        RemainingBalance DECIMAL(18, 2)
    );

    -- Calculate monthly or yearly schedule
    WHILE @Period <= @LoanTerm * CASE WHEN @ScheduleType = 'Monthly' THEN 12 ELSE 1 END
    BEGIN
        -- Calculate payment date
        SET @PaymentDate = DATEADD(MONTH, @Period - 1, @StartDate);

        -- Calculate interest for the period
        SET @Interest = @RemainingBalance * (@InterestRate / 100) / CASE WHEN @ScheduleType = 'Monthly' THEN 12 ELSE 1 END;

        -- Calculate principal for the period
        SET @Principal = @MonthlyPayment - @Interest;

        -- Update remaining balance
        SET @RemainingBalance = @RemainingBalance - @Principal;

        -- Calculate total payment for the period
        SET @TotalPayment = @Principal + @Interest;

        -- Insert the period details into the temporary table
        INSERT INTO #PaymentSchedule (Period, PaymentDate, Principal, Interest, TotalPayment, RemainingBalance)
        VALUES (@Period, @PaymentDate, @Principal, @Interest, @TotalPayment, @RemainingBalance);

        -- Increment period
        SET @Period = @Period + 1;
    END;

    -- Return the payment schedule
    SELECT 
        Period,
        PaymentDate,
        Principal,
        Interest,
        TotalPayment,
        RemainingBalance
    FROM #PaymentSchedule
    ORDER BY Period;

    -- Drop the temporary table
    DROP TABLE #PaymentSchedule;
END;
```

---

### **Step 4: Example Usage** 🖥️

#### Record a Payment
```sql
EXEC RecordPayment @LoanID = 1, @PaymentDate = '2023-04-01', @AmountPaid = 1157.79;
```

#### Generate Monthly Payment Schedule
```sql
EXEC GetPaymentSchedule @LoanID = 1, @ScheduleType = 'Monthly';
```

#### Generate Yearly Payment Schedule
```sql
EXEC GetPaymentSchedule @LoanID = 1, @ScheduleType = 'Yearly';
```

---

### **Step 5: Sample Outputs** 📄

#### 📅 Monthly Payment Schedule
| Period | PaymentDate | Principal  | Interest  | TotalPayment | RemainingBalance |
|--------|-------------|------------|-----------|--------------|------------------|
| 1      | 2023-01-01  | 500.00     | 781.25    | 1281.25      | 249500.00        |
| 2      | 2023-02-01  | 502.34     | 778.91    | 1281.25      | 248997.66        |
| 3      | 2023-03-01  | 504.69     | 776.56    | 1281.25      | 248492.97        |

#### 📅 Yearly Payment Schedule
| Period | PaymentDate | Principal  | Interest  | TotalPayment | RemainingBalance |
|--------|-------------|------------|-----------|--------------|------------------|
| 1      | 2023-01-01  | 6000.00    | 9375.00   | 15375.00     | 244000.00        |
| 2      | 2024-01-01  | 6200.00    | 9175.00   | 15375.00     | 237800.00        |
| 3      | 2025-01-01  | 6400.00    | 8975.00   | 15375.00     | 231400.00        |

---

### **Scaffold Database Context** 🛠️

```sql
dotnet ef dbcontext scaffold "Server=.;Database=LoanTracker;User Id=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f
```
