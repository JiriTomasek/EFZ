
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using EFZ.Database.DbContext.EntityConfiguration;
using EFZ.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFZ.Database.DbContext
{

    public class EfzDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        #region  Properties

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<AttachmentComplete> AttachmentComplete { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Completion> Completion { get; set; }
        public virtual DbSet<Delivery> Delivery { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItem { get; set; }
        public virtual DbSet<InvoiceAddress> InvoiceAddress { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobScheduler> JobSchedulers { get; set; }
        public virtual DbSet<JobLog> JobLogs { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(@"RoleId", @"UserId");
            });
           
            modelBuilder.Entity<Completion>(b =>
                {
                    b.HasOne(t => t.Invoice).WithOne().OnDelete(DeleteBehavior.Restrict).HasForeignKey<Completion>(t=>t.InvoiceId);
                });

            modelBuilder.Entity<User>(u => u.HasOne(x => x.Customer).WithMany(t=>t.Users).HasForeignKey(x=>x.CustomerId).OnDelete(DeleteBehavior.SetNull));

            modelBuilder.Entity<AttachmentComplete>(b =>
            {
                b.HasKey(@"AttachmentId", @"CompletionId");
            });
            modelBuilder.Entity<Order>(o =>
                o.HasIndex(t=>t.OrderNumber).IsUnique()); 
            modelBuilder.Entity<Invoice>(o =>
                o.HasIndex(t => t.OrderNumber).IsUnique());
            modelBuilder.Entity<Delivery>(o =>
                o.HasIndex(t => t.OrderNumber).IsUnique(false));
            modelBuilder.Entity<Attachment>(o =>
                o.HasIndex(t => t.OrderNumber).IsUnique(false));
            modelBuilder.Entity<Completion>(o =>
                o.HasIndex(t => t.OrderNumber).IsUnique(false));

            modelBuilder.Entity<Order>(o =>
                o.HasMany(x => x.Deliveries).WithOne(t => t.Order).HasForeignKey(y => y.OrderId).OnDelete(DeleteBehavior.SetNull));
            modelBuilder.Entity<Order>(o =>
                o.HasMany(x => x.Deliveries).WithOne(t=>t.Order).HasForeignKey(y=>y.OrderId).OnDelete(DeleteBehavior.SetNull));
            modelBuilder.Entity<Invoice>(o =>
                o.HasOne(i=>i.Order).WithOne().HasForeignKey<Invoice>(x=>x.OrderId).OnDelete(DeleteBehavior.SetNull));
            modelBuilder.Entity<Delivery>(o =>
                o.HasMany(x => x.Attachments).WithOne(t=>t.Delivery).HasForeignKey(y=>y.DeliveryId).OnDelete(DeleteBehavior.SetNull));

           
            modelBuilder.Entity<JobLog>(o =>
                o.HasOne(i => i.InnerLog).WithOne().HasForeignKey<JobLog>(x => x.InnerLogId).OnDelete(DeleteBehavior.Restrict));

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        #region constructor
            public EfzDbContext()
        {
        }
        public EfzDbContext(DbContextOptions options) : base(options)
        {
        }
       


        #endregion
    }



}