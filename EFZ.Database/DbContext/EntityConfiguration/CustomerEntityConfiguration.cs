using EFZ.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFZ.Database.DbContext.EntityConfiguration
{
    public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
           
            builder.HasKey(@"Id");

            // configures relationships
            builder.HasMany(x => x.Users)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(@"CustomerId");
        }
    }
}
