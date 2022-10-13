using RPGOnline.Domain.Common;
using RPGOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Domain.Events
{
    public class SampleResignedEvent : DomainEvent
    {
        public Sample SampleClass { get; }

        public SampleResignedEvent(Sample sampleClass)
        {
            SampleClass = sampleClass;
        }
    }
}
