# LoanTracker (ASP.NET Core Web API - .NET 8)

> LoanTracker is a loan management system designed to help financial institutions or businesses manage customer loans, payments, and loan types efficiently.

---

#### **1. Customer Management**
- **As a user**, I can add, update, delete, and retrieve customer details (e.g., name, NRC, mobile number, address) so that I can manage customer information effectively.
- **As a user**, I can search for customers by name or NRC so that I can quickly find specific customer records.

---

#### **2. Loan Management**
- **As a user**, I can create, update, delete, and retrieve loan details (e.g., loan type, principal amount, interest rate, loan term) so that I can manage loans for customers.
- **As a user**, I can generate payment schedules automatically when a loan is created so that I can track repayment details.
- **As a user**, I can update the loan status to "Completed" when all payments are made so that I can track the loan lifecycle.

---

#### **3. Payment Management**
- **As a user**, I can add payments for a loan so that I can track repayments.
- **As a user**, I can calculate and apply late fees (1% per day) for overdue payments so that I can enforce repayment rules.
- **As a user**, I can update payment status (e.g., "On-Time", "Late", "Paid") so that I can monitor payment compliance.

---

#### **4. Loan Type Management**
- **As a user**, I can add, update, delete, and retrieve loan types (e.g., personal loan, business loan) so that I can manage different loan products.
- **As a user**, I can add Burmese translations for loan types so that I can support multilingual loan type names.

---

### Key Features
- **RESTful API**: Built with ASP.NET Core Web API (.NET 8) for seamless integration with front-end applications.
- **Database**: Uses Entity Framework Core for database operations with SQL Server.
- **Validation**: Ensures data integrity with validation checks (e.g., unique NRC, mobile number).
- **Automation**: Automatically generates payment schedules and calculates late fees.