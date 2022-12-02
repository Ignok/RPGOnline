using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface IUser
    {
        Task<UserAboutmeResponse> GetAboutMe(int id);

        Task<PutUserResponse> PutUser(int id, UserRequest userRequest);
    }
}
