using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests.Character;
using RPGOnline.Application.DTOs.Responses.Character;
using RPGOnline.Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace RPGOnline.Infrastructure.Services
{
    public class RaceService : IRace
    {
        private readonly IApplicationDbContext _dbContext;
        public RaceService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ICollection<RaceResponse>> GetRacesForCharacter(int uId, GetRaceRequest getRaceRequest)
        {
           // select* from asset a join[character] r on a.asset_id = r.asset_id where is_public = 1 or a.author_id = 3;
            var result = await (from asset in _dbContext.Assets
                                join race in _dbContext.Races on asset.AssetId equals race.AssetId
                                where ((object.Equals(race.KeyAttribute, getRaceRequest.Attribute))
                                    || (object.Equals(race.KeyAttribute, null)))
                                where getRaceRequest.PrefferedLanguage.Contains(asset.Language)
                                where asset.IsPublic || asset.AuthorId == uId
                                orderby race.RaceName ascending
                                select new RaceResponse()
                                {
                                     RaceId = race.RaceId,
                                     AssetId = asset.AssetId,
                                     RaceName = race.RaceName,
                                     Description = race.Description,
                                     Talent = race.Talent,
                                     HiddenTalent = race.HiddenTalent,
                                     KeyAttribute = race.KeyAttribute
                                })
                                .ToListAsync();
            return result;
        }
    }
}
