using CivitaBack.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CivitaBack.Data.EF.Configurations
{
    public class AuditableConfiguration : IEntityTypeConfiguration<Auditable>
    {
        public void Configure(EntityTypeBuilder<Auditable> builder)
        {
            builder.OwnsOne(a => a); // marca la clase como owned
        }
    }
}
