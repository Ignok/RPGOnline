using Microsoft.EntityFrameworkCore;
using RPGOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        // nie wiem czy dobrze że dziedziczy po warstwie Domain?
        DbSet<Sample> Samples { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
