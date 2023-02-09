using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.Common;
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
using System.Diagnostics;

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

            var keyValues = getProfessionRequest.KeyValueName.Split('-');

            var result = await (from asset in _dbContext.Assets
                                join profession in _dbContext.Professions on asset.AssetId equals profession.AssetId
                                where keyValues.Contains(profession.KeyAttribute) || object.Equals(profession.KeyAttribute, null)
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
                                    PsycheMod = profession.PsycheMod,
                                    Language = asset.Language,
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
                    .Where(p => p.Asset.IsPublic || p.Asset.AuthorId == userId)
                    .Where(i => !searchProfessionRequest.IfOnlyMyAssets.GetValueOrDefault() || i.Asset.AuthorId == userId)
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
                                CreatorNavigation = new UserSimplifiedResponse()
                                {
                                    UId = s.Asset.Author.UId,
                                    Username = s.Asset.Author.Username,
                                    Picture = s.Asset.Author.Picture,
                                },
                                IsPublic = s.Asset.IsPublic,
                            }
                        ).Where(r => r.CreatorNavigation.UId == userId || r.IsPublic)
                        .ToList(),
                        ItemList = p.ProfessionStartingItems.Select(i =>
                            new GetItemSimplifiedResponse()
                            {
                                AssetId = i.Item.AssetId,
                                ItemId = i.Item.ItemId,
                                Name = i.Item.Name,
                                Description = i.Item.Description,
                                KeySkill = i.Item.KeySkill,
                                CreatorNavigation = new UserSimplifiedResponse()
                                {
                                    UId = i.Item.Asset.AuthorId,
                                    Username = i.Item.Asset.Author.Username,
                                    Picture = i.Item.Asset.Author.Picture,
                                },
                                IsPublic = i.Item.Asset.IsPublic,
                            }
                        ).Where(r => r.CreatorNavigation.UId == userId || r.IsPublic)
                        .ToList(),
                    })
                    .OrderByDescending(p => p.CreationDate)
                    .ToList();


                if (searchProfessionRequest.SortingByDate ?? false)
                {
                    result = result.OrderByDescending(i => i.CreationDate).ToList();
                }
                else if (searchProfessionRequest.SortingByLikes ?? false)
                {
                    result = result.OrderByDescending(i => i.TimesSaved).ToList();
                }

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

            if(postProfessionRequest.Spells != null)
            {
                var spells = await _dbContext.Spells
                                    .Where(s => postProfessionRequest.Spells.Contains(s.SpellId))
                                    .ToListAsync();
                profession.Spells.AddRange(spells);
            }

            if(postProfessionRequest.Items != null)
            {
                var items = await _dbContext.Items.Where(i => postProfessionRequest.Items.Contains(i.ItemId)).ToListAsync();
                var startingItemsId = (_dbContext.ProfessionStartingItems.Max(i => (int)i.ProfessionStartingItemsId) + 1);
                var startingItems = new List<ProfessionStartingItem>();
                foreach (Item i in items)
                {
                    var startingItem = new ProfessionStartingItem()
                    {
                        ProfessionStartingItemsId = startingItemsId++,
                        ProfessionId = profession.ProfessionId,
                        ItemId = i.ItemId,
                        Item = i,
                        Profession = profession,
                    };
                    startingItems.Add(startingItem);
                }
                profession.ProfessionStartingItems.AddRange(startingItems);
                //_dbContext.ProfessionStartingItems.Add(startingItems);
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

        private bool HasBlockedMe(int myId, int targetId)
        {
            if (myId == targetId) return false;
            return myId == targetId || _dbContext.Friendships
                .Where(f => f.UId == targetId && f.FriendUId == myId)
                .Where(f => f.IsBlocked).Any();
        }
    }
}
