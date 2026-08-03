using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ICV.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ICV.Domain.Common;



namespace ICV.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {





        }

    }
}
