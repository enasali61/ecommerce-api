using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.Configuration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.ProductBrand).WithMany().HasForeignKey(P=> P.BrandId);
            builder.HasOne(P => P.ProductType).WithMany().HasForeignKey(P => P.TypeId);
            builder.Property(P => P.Price).HasColumnType("decimal(8,3)");

        }
    }
}
