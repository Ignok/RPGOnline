using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Character;
using RPGOnline.Application.DTOs.Responses;
using RPGOnline.Application.DTOs.Responses.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface IRace
    {
        Task<ICollection<RaceResponse>> GetRacesForCharacter(int uId, GetRaceRequest getRaceRequest);
    }
}
