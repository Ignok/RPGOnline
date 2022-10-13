using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RPGOnline.Application.Common.Interfaces;

namespace RPGOnline.Application.Samples.Queries.GetSamples
{
    public class GetSampleQuery : IRequest<GetSamplesResponseDto>
    {
        public string JustSomeSampleThing { get; set; }
    }


    public class GetSamplesQueryHandler : IRequestHandler<GetSampleQuery, GetSamplesResponseDto>
    {
        private IApplicationDbContext _context;
        public GetSamplesQueryHandler(IApplicationDbContext context) //reprezentuje bazę danych
        {
            _context = context;
        }

        public async Task<GetSamplesResponseDto> Handle(GetSampleQuery request, CancellationToken cancellationToken)
        {
            //... Logika

            return new GetSamplesResponseDto
            {
                Samples = await _context.Samples
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .Select(s => new SampleDto
                {
                    Name = s.Name
                })
                .ToListAsync()
            };
        }
    }
}
