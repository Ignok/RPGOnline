using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.Asset.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Interfaces
{
    public interface ISpell
    {
        Task<ICollection<GetSpellSimplifiedResponse>> GetSpellsForCharacter(int uId, GetAssetForCharacterRequest getSpellRequest);
        Task<(ICollection<GetSpellResponse>, int pageCount)> GetSpells(SearchAssetRequest searchSpellRequest, CancellationToken cancellationToken, int userId);
        Task<GetSpellResponse> PostSpell(PostSpellRequest postSpellRequest);
    }
}
