using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Domain.Common
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }

    public abstract class DomainEvent
    {
        public DomainEvent()
        {

        }

        public bool IsPublished { get; set; }
        public DateTimeOffset DateOccured { get; protected set; } = DateTimeOffset.UtcNow;
    }
}
