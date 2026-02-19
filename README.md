# LoanTracker (ASP.NET Core .NET 8)

**LoanTracker** is a comprehensive loan management system designed to streamline the operations of financial institutions or businesses that provide lending services. Built with **ASP.NET Core (.NET 8)**, it provides a robust ecosystem for managing the entire loan lifecycle—from customer onboarding and localized product management to automated repayment schedules and real-time analytics.

---

## 🏛 Architecture Overview

The project follows a decoupled, multi-layered architecture to ensure scalability and maintainability:

- **LoanTracker.Shared**: The central contract library containing all Request/Response DTOs and the unified `Result<T>` wrapper. This ensures type safety and consistency across all layers.
- **LoanTracker.Domain**: The business logic layer. It contains services that handle complex operations, validation, and error handling, returning wrapped results.
- **LoanTracker.Database**: Data access layer using **Entity Framework Core**. It manages the SQL Server schema, entities, and database-first mappings.
- **LoanTracker.Api**: A secure RESTful API layer that exposes endpoints for mobile or external integration. It implements **Basic Authentication** and uses extension methods for consistent HTTP response mapping.
- **LoanTracker.Mvc**: The modern frontend dashboard built with ASP.NET Core MVC and **Tailwind CSS**. It consumes the API via a centralized `HttpClientService` and provides a premium, responsive user experience.

---

## ⚙ Business Logic & Key Modules

### 1. Customer Management
- **Smart Onboarding**: Capture essential details (Name, NRC, Mobile, Address) with built-in data integrity checks for unique identity documents.
- **Quick Lookup**: Search and locate customers instantly via Name or NRC.

### 2. Loan Product Configuration (Loan Types)
- **Localized Support**: Define loan products with dual-language support (English and Burmese) for names and descriptions, catering to local operational needs.
- **Full Lifecycle Mastery**: Manage everything from personal loans to complex business growth capital.

### 3. Automated Loan Portfolio
- **Intelligent Amortization**: Automatically generates equal installment schedules upon loan creation using standard financial formulas.
- **Portfolio Tracking**: Monitor loan status from "Active" to "Completed" in real-time.
- **Repayment Frequency**: Flexible scheduling including monthly installments.

### 4. Advanced Payment Management
- **Automated Late Fees**: System automatically enforces a **1% per day** late fee policy for payments made after the `DueDate`.
- **Compliance Monitoring**: Tracks payment status as "On-Time", "Late", or "Paid" to provide deep insights into borrower behavior.

### 5. Real-Time Dashboard Analytics
- **Portfolio Metrics**: Instant visibility into Total Portfolio Value, Active Loan Counts, and Late Payment alerts.
- **Dynamic Activity Feed**: A live stream of recent system events, including successful payment postings and new loan originations.

---

## 🚀 Tech Stack

- **Core**: .NET 8 (LTS)
- **ORM**: Entity Framework Core 8
- **Database**: SQL Server
- **UI Architecture**: ASP.NET Core MVC + Tailwind CSS
- **Serialization**: Newtonsoft.Json (Customized for consistent DTO handling)
- **Security**: Basic Authentication for API protection
- **Design**: Premium Modern Aesthetics with Glassmorphism and Micro-animations

---

## 🛠 Getting Started

1. **Database Setup**: Execute the `LoanTracker.sql` script in your SQL Server instance to initialize the schema and seed data.
2. **API Configuration**: Update the `ConnectionStrings` in `LoanTracker.Api/appsettings.json`.
3. **Run**: Launch the `LoanTracker.Api` project first, followed by the `LoanTracker.Mvc` project to access the management dashboard.
