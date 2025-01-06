using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDB.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AppDB.AppDbContext
{
    public class AppDbContext : DbContext
    {
        private readonly SqlConnectionStringBuilder _connBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "LoanTracker",
            UserID = "sa",
            Password = "Kyawzin@123",
            TrustServerCertificate = true
        };

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connBuilder.ConnectionString);
            }
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<LoanDetails> LoanDetails { get; set; }
        public DbSet<PaymentRecord> PaymentRecords { get; set; }
    }
}
