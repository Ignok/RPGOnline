using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface ICharacter
    {
        public Task<string> GetMotivation();

        public Task<string> GetCharacteristics();

    }
}
