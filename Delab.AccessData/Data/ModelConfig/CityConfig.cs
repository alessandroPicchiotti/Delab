using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.AccessData.Data.ModelConfig;

public class CityConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(x => x.CityId);   
        builder.HasIndex(x => new{x.Name,x.StateId }).IsUnique();
        builder.HasOne( x => x.State)
            .WithMany(x => x.Cities)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
