using RPGOnline.Application.DTOs.Responses.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface ICharacter
    {
        public Task<MotivationResponse> GetMotivation();

        public Task<CharacteristicsResponse> GetCharacteristics();

        public Task<CharacterResponse> GetCharacterInfo(int characterId);

    }
}
