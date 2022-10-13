using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Domain.ValueObjects
{
    // ValueObject reprezentuje nie pełnoprawny obiekt, tylko nieco bardziej złożoną strukturę danych, np. Adres (u nas może karta postaci podczas grania?)
    public record SampleValueObject
    {
        public SampleValueObject(string name, string value)
        {
            //walidacja itd.
            Name = name;
            Value = value;
        }

        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
