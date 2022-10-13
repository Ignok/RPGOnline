using RPGOnline.Domain.Common;
using RPGOnline.Domain.Enums;
using RPGOnline.Domain.Events;
using RPGOnline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//duża szansa że w bazie ten obiekt to kilka różnych tabel,
// po prostu w ogóle nie myślimy o bazie

namespace RPGOnline.Domain.Entities
{
    public class Sample
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get;}

        //public SampleValueObject? SampleValueObject { get; }
       
        //private SampleStatus _sampleStatus;

        /*
        public SampleStatus SampleStatus
        {
            get => _sampleStatus;
            set
            {
                if(value == SampleStatus.Resigned)
                {
                    DomainEvents.Add(new SampleResignedEvent(this));
                }

                _sampleStatus = value;
            }
        }
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
        */
    }
}
