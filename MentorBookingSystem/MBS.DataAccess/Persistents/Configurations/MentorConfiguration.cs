using MBS.BusinessObject.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Persistents.Configurations
{
    public class MentorConfiguration : IEntityTypeConfiguration<Mentor>
    {
        public void Configure(EntityTypeBuilder<Mentor> builder)
        {
            builder.HasKey(m => m.UserId);
            builder.HasOne(m => m.User)
                  .WithMany()
                  .HasForeignKey(m => m.UserId);

            builder.Property(m => m.Industry)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(m => m.ConsumePoint)
                    .HasDefaultValue(100)
                    .IsRequired();


        }
    }
}
