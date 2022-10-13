using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Samples.Queries.GetSamples
{
    public class GetSamplesResponseDto
    {
        public ICollection<SampleDto> Samples { get; set; }
    }
}
