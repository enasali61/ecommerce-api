using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Order_Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.Configuration
{
    internal class DeliveryMethodConfiguration :
        IEntityTypeConfiguration<DeliveryMethods>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethods> builder)
        {
            builder.Property(d=>d.Cost).HasColumnType("decimal(18,3)");
        }
    }
}
