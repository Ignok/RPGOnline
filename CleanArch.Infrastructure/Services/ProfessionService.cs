using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Profession;
using RPGOnline.Application.DTOs.Responses.Asset.Item;
using RPGOnline.Application.DTOs.Responses.Asset.Profession;
using RPGOnline.Application.DTOs.Responses.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Enums;
using RPGOnline.Domain.Models;

namespace RPGOnline.Infrastructure.Services
{
    public class ProfessionService : IProfession
    {
        private readonly IApplicationDbContext _dbContext;
        private ILogger<ProfessionService> _logger;

        public ProfessionService(IApplicationDbContext dbContext, ILogger<ProfessionService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly int professionsOnPageAmount = 10;

        public async Task<ICollection<GetProfessionSimplifiedResponse>> GetProfessionsForCharacter(int uId, GetAssetForCharacterRequest getProfessionRequest)
        {
            var result = await (from asset in _dbContext.Assets
                                join profession in _dbContext.Professions on asset.AssetId equals profession.AssetId
                                where ((object.Equals(profession.KeyAttribute, getProfessionRequest.KeyValueName))
                                    || (object.Equals(profession.KeyAttribute, null)))
                                where getProfessionRequest.PrefferedLanguage.Contains(asset.Language)
                                where asset.IsPublic || asset.AuthorId == uId
                                orderby profession.Name ascending
                                select new GetProfessionSimplifiedResponse()
                                {
                                    ProfessionId = profession.ProfessionId,
                                    AssetId = asset.AssetId,
                                    Name = profession.Name,
                                    Description = profession.Description,
                                    Talent = profession.Talent,
                                    HiddenTalent = profession.HiddenTalent,
                                    KeyAttribute = profession.KeyAttribute,
                                    WeaponMod = profession.WeaponMod,
                                    ArmorMod = profession.ArmorMod,
                                    GadgetMod = profession.GadgetMod,
                                    CompanionMod = profession.CompanionMod,
                                    PsycheMod = profession.PsycheMod
                                })
                                .ToListAsync();
            return result;
        }

        public async Task<(ICollection<GetProfessionResponse>, int pageCount)> GetProfessions(SearchAssetRequest searchProfessionRequest, int userId, CancellationToken cancellationToken)
        {
            try
            {

                var page = searchProfessionRequest.Page;
                if (searchProfessionRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));



                var result = _dbContext.Professions.Include(p => p.Asset)
                                                .Include(p => p.Asset.UserSavedAssets)
                                                .Include(p => p.Asset.Author)
                                                .Include(p => p.ProfessionStartingItems).ThenInclude(p => p.Item).ThenInclude(p => p.Asset)
                                                .Include(p => p.Spells).ThenInclude(p => p.Asset)
                                                .AsParallel().WithCancellation(cancellationToken)
                    .Where(p => p.Asset.IsPublic || p.Asset.Author.UId == userId)
                    .Where(p => String.IsNullOrEmpty(searchProfessionRequest.KeyValueName)
                                || object.Equals(p.KeyAttribute, searchProfessionRequest.KeyValueName)
                                || object.Equals(p.KeyAttribute, null)
                            )
                    .Where(p => String.IsNullOrEmpty(searchProfessionRequest.Search)
                                || (p.Name.Contains(searchProfessionRequest.Search, StringComparison.OrdinalIgnoreCase))
                                || (p.Description.Contains(searchProfessionRequest.Search, StringComparison.OrdinalIgnoreCase))
                            )
                    .Where(p => searchProfessionRequest.PrefferedLanguage.Contains(p.Asset.Language))
                    .Select(p => new GetProfessionResponse()
                    {
                        AssetId = p.Asset.AssetId,
                        CreationDate = p.Asset.CreationDate,
                        TimesSaved = p.Asset.UserSavedAssets.Count,
                        ProfessionId = p.ProfessionId,
                        Name = p.Name,
                        Description = p.Description,
                        KeyAttribute = p.KeyAttribute,
                        Talent = p.Talent,
                        HiddenTalent = p.HiddenTalent,
                        WeaponMod = p.WeaponMod,
                        ArmorMod = p.ArmorMod,
                        GadgetMod = p.GadgetMod,
                        CompanionMod = p.CompanionMod,
                        PsycheMod = p.PsycheMod,
                        IsSaved = p.Asset.UserSavedAssets.Any(usa => usa.UId == userId),
                        PrefferedLanguage = p.Asset.Language,
                        CreatorNavigation = new UserSimplifiedResponse()
                        {
                            UId = p.Asset.Author.UId,
                            Username = p.Asset.Author.Username,
                            Picture = p.Asset.Author.Picture,
                        },
                        SpellList = p.Spells.Select(s =>
                            new GetSpellSimplifiedResponse()
                            {
                                AssetId = s.AssetId,
                                SpellId = s.SpellId,
                                Name = s.Name,
                                Description = s.Description,
                                KeyAttribute = s.KeyAttribute,
                                Effects = s.Effects,
                                PrefferedLanguage = s.Asset.Language,
                            }
                        ).ToList(),
                        ItemList = p.ProfessionStartingItems.Select(i =>
                            new GetItemSimplifiedResponse()
                            {
                                AssetId = i.Item.AssetId,
                                ItemId = i.Item.ItemId,
                                Name = i.Item.Name,
                                Description = i.Item.Description,
                                KeySkill = i.Item.KeySkill,
                                PrefferedLanguage = i.Item.Asset.Language,
                            }
                        ).ToList(),
                    })
                    .ToList();


                int pageCount = (int)Math.Ceiling((double)result.Count / professionsOnPageAmount);

                result = result
                    .Skip(professionsOnPageAmount * (page - 1))
                    .Take(professionsOnPageAmount)
                    .ToList();

                await Task.Delay(500, cancellationToken);

                return (result, pageCount);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("=========== I WAS CANCELLED ==========");
                throw ex;
            }
        }

        public async Task<GetProfessionResponse> PostProfession(PostProfessionRequest postProfessionRequest)
        {
            if (postProfessionRequest == null) throw new ArgumentNullException(nameof(postProfessionRequest));

            //if user exists
            if (!_dbContext.Users.Where(u => u.UId == postProfessionRequest.UId).ToList().Any())
                throw new ArgumentException($"User with id: {postProfessionRequest.UId} does not exist");

            //if language exists
            if (!Enum.IsDefined(typeof(Language), postProfessionRequest.Language))
                throw new InvalidDataException($"Language '{postProfessionRequest.Language}' is not supported");

            //if attribute exists, or is null
            if (!Enum.IsDefined(typeof(Attributes), postProfessionRequest.KeyAttribute ?? "none"))
                throw new InvalidDataException($"Attribute '{postProfessionRequest?.KeyAttribute}' does not exist");


            var asset = new Asset()
            {
                AssetId = (_dbContext.Assets.Max(a => (int)a.AssetId) + 1),
                AuthorId = postProfessionRequest.UId,
                IsPublic = postProfessionRequest.IsPublic,
                Language = postProfessionRequest.Language,
                CreationDate = DateTime.Now,
                Author = await _dbContext.Users.Where(u => u.UId == postProfessionRequest.UId).SingleAsync(),
            };

            var profession = new Profession()
            {
                ProfessionId = (_dbContext.Professions.Max(r => (int)r.ProfessionId) + 1),
                AssetId = asset.AssetId,
                Name = postProfessionRequest.Name,
                Description = postProfessionRequest.Description,
                Talent = postProfessionRequest.Talent,
                HiddenTalent = postProfessionRequest.HiddenTalent,
                KeyAttribute = postProfessionRequest.KeyAttribute.Equals("none") ? null : postProfessionRequest.KeyAttribute,
                WeaponMod = postProfessionRequest.WeaponMod,
                ArmorMod = postProfessionRequest.ArmorMod,
                GadgetMod = postProfessionRequest.GadgetMod,
                CompanionMod = postProfessionRequest.CompanionMod,
                PsycheMod = postProfessionRequest.PsycheMod,
                Asset = asset,
            };

            asset.Professions.Add(profession);

            var spell = await _dbContext.Spells.Where(s => s.SpellId == postProfessionRequest.SpellId).FirstOrDefaultAsync();
            if (spell != null)
            {
                profession.Spells.Add(spell);
            }

            var item = await _dbContext.Items.Where(s => s.ItemId == postProfessionRequest.ItemId).FirstOrDefaultAsync();
            if (item != null)
            {
                var startingItem = new ProfessionStartingItem()
                {
                    ProfessionStartingItemsId = (_dbContext.ProfessionStartingItems.Max(i => (int)i.ProfessionStartingItemsId) + 1),
                    ProfessionId = profession.ProfessionId,
                    ItemId = item.ItemId,
                    Item = item,
                    Profession = profession,
                };

                profession.ProfessionStartingItems.Add(startingItem);
                _dbContext.ProfessionStartingItems.Add(startingItem);
            }

            _dbContext.Assets.Add(asset);
            _dbContext.Professions.Add(profession);
            _dbContext.SaveChanges();

            return new GetProfessionResponse()
            {
                AssetId = profession.Asset.AssetId,
                CreationDate = profession.Asset.CreationDate,
                TimesSaved = profession.Asset.UserSavedAssets.Count,
                ProfessionId = profession.ProfessionId,
                Name = profession.Name,
                Description = profession.Description,
                KeyAttribute = profession.KeyAttribute,
                Talent = profession.Talent,
                HiddenTalent = profession.HiddenTalent,
                WeaponMod = profession.WeaponMod,
                ArmorMod = profession.ArmorMod,
                GadgetMod = profession.GadgetMod,
                CompanionMod = profession.CompanionMod,
                PsycheMod = profession.PsycheMod,
                PrefferedLanguage = asset.Language,
                CreatorNavigation = new UserSimplifiedResponse()
                {
                    UId = asset.Author.UId,
                    Username = asset.Author.Username,
                    Picture = asset.Author.Picture,
                }
            };
        }
    }
}
