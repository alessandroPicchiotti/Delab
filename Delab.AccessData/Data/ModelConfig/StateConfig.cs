using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.AccessData.Data.ModelConfig;

public class StateConfig : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.HasKey(x => x.StateId);   
        builder.HasIndex(x => new {x.Name,x.CountryId }).IsUnique();

        builder.HasOne( x => x.Country).WithMany( x => x.States).OnDelete(DeleteBehavior.Restrict);  
    }
}
