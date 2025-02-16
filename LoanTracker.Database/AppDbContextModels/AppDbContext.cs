using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblCustomerLoan> TblCustomerLoans { get; set; }

    public virtual DbSet<TblCustomerPayment> TblCustomerPayments { get; set; }

    public virtual DbSet<TblLateFee> TblLateFees { get; set; }

    public virtual DbSet<TblLoanType> TblLoanTypes { get; set; }

    public virtual DbSet<TblLoanTypeBurmese> TblLoanTypeBurmeses { get; set; }

    public virtual DbSet<TblPaymentSchedule> TblPaymentSchedules { get; set; }

    public virtual DbSet<VwCustomerLoan> VwCustomerLoans { get; set; }

    public virtual DbSet<VwLoanType> VwLoanTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Tbl_Cust__A4AE64B8E7A2F009");

            entity.ToTable("Tbl_Customer");

            entity.HasIndex(e => e.Nrc, "UQ__Tbl_Cust__C7DEEA5A5FB49C09").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.MobileNo).HasMaxLength(20);
            entity.Property(e => e.Nrc)
                .HasMaxLength(50)
                .HasColumnName("NRC");
        });

        modelBuilder.Entity<TblCustomerLoan>(entity =>
        {
            entity.HasKey(e => e.LoanId).HasName("PK__Tbl_Cust__4F5AD43727D2E688");

            entity.ToTable("Tbl_CustomerLoan");

            entity.Property(e => e.LoanId).HasColumnName("LoanID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LoanTypeId).HasColumnName("LoanTypeID");
            entity.Property(e => e.PrincipalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RepaymentFrequency)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.TblCustomerLoans)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Custo__Custo__46E78A0C");

            entity.HasOne(d => d.LoanType).WithMany(p => p.TblCustomerLoans)
                .HasForeignKey(d => d.LoanTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Custo__LoanT__47DBAE45");
        });

        modelBuilder.Entity<TblCustomerPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Tbl_Cust__9B556A5856C41495");

            entity.ToTable("Tbl_CustomerPayment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LateFee)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Schedule).WithMany(p => p.TblCustomerPayments)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Custo__Sched__5070F446");
        });

        modelBuilder.Entity<TblLateFee>(entity =>
        {
            entity.HasKey(e => e.LateFeeId).HasName("PK__Tbl_Late__724D096E8449FE4A");

            entity.ToTable("Tbl_LateFee");

            entity.Property(e => e.LateFeeId).HasColumnName("LateFeeID");
            entity.Property(e => e.LateFeeAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

            entity.HasOne(d => d.Schedule).WithMany(p => p.TblLateFees)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_LateF__Sched__534D60F1");
        });

        modelBuilder.Entity<TblLoanType>(entity =>
        {
            entity.HasKey(e => e.LoanTypeId).HasName("PK__Tbl_Loan__19466B4FFCEA649C");

            entity.ToTable("Tbl_LoanType");

            entity.Property(e => e.LoanTypeId).HasColumnName("LoanTypeID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.LoanTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblLoanTypeBurmese>(entity =>
        {
            entity.HasKey(e => e.LoanTypeId).HasName("PK__Tbl_Loan__19466B4F5477F0B9");

            entity.ToTable("Tbl_LoanTypeBurmese");

            entity.Property(e => e.LoanTypeId).HasColumnName("LoanTypeID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.EnglishLoanTypeId).HasColumnName("EnglishLoanTypeID");
            entity.Property(e => e.LoanTypeName).HasMaxLength(100);

            entity.HasOne(d => d.EnglishLoanType).WithMany(p => p.TblLoanTypeBurmeses)
                .HasForeignKey(d => d.EnglishLoanTypeId)
                .HasConstraintName("FK_EnglishLoanType");
        });

        modelBuilder.Entity<TblPaymentSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Tbl_Paym__9C8A5B69D21FDEC2");

            entity.ToTable("Tbl_PaymentSchedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.InstallmentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InterestComponent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoanId).HasColumnName("LoanID");
            entity.Property(e => e.PrincipalComponent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RemainingBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Loan).WithMany(p => p.TblPaymentSchedules)
                .HasForeignKey(d => d.LoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Payme__LoanI__4BAC3F29");
        });

        modelBuilder.Entity<VwCustomerLoan>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_CustomerLoan");

            entity.Property(e => e.BurmeseLoanType).HasMaxLength(100);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.EnglishLoanType).HasMaxLength(100);
        });

        modelBuilder.Entity<VwLoanType>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vw_LoanType");

            entity.Property(e => e.BurmeseDescription).HasMaxLength(255);
            entity.Property(e => e.BurmeseLoanTypeName).HasMaxLength(100);
            entity.Property(e => e.EnglishDescription).HasMaxLength(255);
            entity.Property(e => e.EnglishLoanTypeId).HasColumnName("EnglishLoanTypeID");
            entity.Property(e => e.EnglishLoanTypeName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
