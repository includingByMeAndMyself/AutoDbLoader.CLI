using AutoDbLoader.DAL.MSSQL.Entity;
using Microsoft.EntityFrameworkCore;

namespace AutoDbLoader.DAL.MSSQL.Context
{
    public partial class TerritoryPaymentContext : DbContext
    {
        public TerritoryPaymentContext()
        {
        }

        public TerritoryPaymentContext(DbContextOptions<TerritoryPaymentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TerritoryPayments> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("строка подключения");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<TerritoryPayments>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("territory");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .HasColumnName("address");

                entity.Property(e => e.Alias)
                    .HasMaxLength(100)
                    .HasColumnName("alias");

                entity.Property(e => e.Debt)
                    .HasMaxLength(21)
                    .HasColumnName("erc_balance");

                entity.Property(e => e.Payer)
                    .HasMaxLength(255)
                    .HasColumnName("fio");

                entity.Property(e => e.INN)
                    .HasMaxLength(20)
                    .HasColumnName("INN");

                entity.Property(e => e.PersonalAccount)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ls");

                entity.Property(e => e.PaymentAccount)
                    .HasMaxLength(50)
                    .HasColumnName("pay_account");

                entity.Property(e => e.IndicationPeriod)
                    .HasMaxLength(255)
                    .HasColumnName("period");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
