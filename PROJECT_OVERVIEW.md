# LoanTracker Project Overview & Business Logic

## 1. Project Overview
**LoanTracker** is a comprehensive loan management system designed to streamline the operations of financial institutions or businesses that provide lending services. Built with **ASP.NET Core Web API (.NET 8)** and **Entity Framework Core**, it provides a robust backend for managing the entire loan lifecycle—from customer onboarding to final loan closure.

### Core Objectives
*   **Centralized Customer Data**: Maintain a single source of truth for customer information.
*   **Automated Financial Calculations**: Reduce human error by automating installment schedules and late fee calculations.
*   **Compliance & Tracking**: Monitor payment status and loan progress in real-time.

---

## 2. Business Logic & Workflow

### A. Customer Management
*   **Onboarding**: Captures essential details like Name, NRC (for identity), Mobile Number, and Address.
*   **Data Integrity**: Ensures uniqueness for NRC and Mobile Numbers to prevent duplicate profiles.
*   **Searchability**: Allows administrators to quickly locate records via Name or NRC.

### B. Loan Type Configuration
*   **Categorization**: Define different loan products (e.g., Personal Loan, Business Loan, Mortgage).
*   **Localized Support**: Built-in support for Burmese translations of loan type names to cater to local staff.

### C. Loan Lifecyle
1.  **Origination**: A loan is created with a Principal Amount, Interest Rate, Loan Term (years), and Repayment Frequency (e.g., Monthly).
2.  **Amortization**: Upon creation, the system automatically uses the loan amortization formula to generate an equal installment schedule.
    *   *Calculation*: `Monthly Payment = P * [r(1+r)^n] / [(1+r)^n - 1]` where `r` is monthly interest and `n` is total payments.
3.  **Active Status**: Newly created loans are set to "Active".
4.  **Completion**: When all associated payment schedules are marked as "Paid", the loan status automatically updates to "Completed".

### D. Repayment & Late Fees
*   **Payment Schedules**: Each loan has a series of `Pending` schedules with a `DueDate`.
*   **Late Fee Policy**: If a payment is recorded after the `DueDate`, the system automatically calculates a late fee of **1% per day** based on the installment amount.
*   **Status Indicators**:
    *   `On-Time`: Payment made on or before the due date.
    *   `Late`: Payment made after the due date (includes accrued fees).
    *   `Paid`: Installment fully covered.
    *   `Partial`: Amount paid is less than the required installment.

---

## 3. Suggestions for New Features

### 🚀 High Priority (Operational Efficiency)
1.  **User Authentication & Roles**: Implement JWT-based authentication with Role-Based Access Control (RBAC).
    *   *Admin*: Full access.
    *   *Agent/Cashier*: Can create customers and record payments but cannot delete loans.
2.  **Dashboard Analytics**: A visual summary showing:
    *   Total Outstanding Balance.
    *   Expected vs. Actual Collections for the current month.
    *   Heatmap of overdue payments.
3.  **Audit Logging**: Track every change made to a loan or payment (Who, When, What) to prevent fraud.

### 📈 Medium Priority (User Experience)
4.  **Automated Notifications**: Integrate SMS or Email gateways (e.g., Twilio, SendGrid) to send:
    *   Payment reminders (3 days before due).
    *   Overdue alerts.
    *   Success receipts.
5.  **Attachment Management**: Allow users to upload scanned documents (NRC photos, signed contracts, property deeds) directly to a customer's profile or loan record.
6.  **Multiple Payment Methods**: Track whether a payment was made via Cash, Bank Transfer, or Mobile Wallets (KBZPay, WaveMoney).

### 💎 Advanced Features (Growth)
7.  **Early Settlement Calculation**: A feature to calculate the total amount needed to close a loan early, potentially offering an interest rebate.
8.  **Credit Scoring Engine**: Automatically assign a credit score to customers based on their historical payment compliance within the system.
9.  **Guarantor Management**: Ability to link another customer as a guarantor for high-value loans.
