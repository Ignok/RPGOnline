using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Infrastructure.Persistence
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        // warstwa infrastruktury ma mieć referencje do warstwy domeny, jak tu Sample
        DbSet<Sample> IApplicationDbContext.Samples => throw new NotImplementedException();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
