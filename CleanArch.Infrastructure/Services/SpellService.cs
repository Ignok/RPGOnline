﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Enums;
using RPGOnline.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPGOnline.Infrastructure.Services
{
    public class SpellService : ISpell
    {
        private readonly IApplicationDbContext _dbContext;
        private ILogger<SpellService> _logger;

        public SpellService(IApplicationDbContext dbContext, ILogger<SpellService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly int spellsOnPageAmount = 10;

        public async Task<(ICollection<GetSpellResponse>, int pageCount)> GetSpells(SearchAssetRequest searchSpellRequest, int userId, CancellationToken cancellationToken)
        {
            try
            {

                var page = searchSpellRequest.Page;
                if (searchSpellRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));



                var result = _dbContext.Spells.Include(s => s.Asset)
                                                .Include(s => s.Asset.Author)
                                                .Include(s => s.Asset.UserSavedAssets)
                                                .AsParallel().WithCancellation(cancellationToken)
                    .Where(s => s.Asset.IsPublic)
                    .Where(s => String.IsNullOrEmpty(searchSpellRequest.KeyValueName)
                                || object.Equals(s.KeyAttribute, searchSpellRequest.KeyValueName)
                            )
                    .Where(s => String.IsNullOrEmpty(searchSpellRequest.Search)
                                || (s.Name.Contains(searchSpellRequest.Search, StringComparison.OrdinalIgnoreCase))
                                || (s.Description.Contains(searchSpellRequest.Search, StringComparison.OrdinalIgnoreCase))
                            )
                    .Where(s => searchSpellRequest.PrefferedLanguage.Contains(s.Asset.Language))
                    .Select(s => new GetSpellResponse()
                    {
                        AssetId = s.Asset.AssetId,
                        CreationDate = s.Asset.CreationDate,
                        TimesSaved = s.Asset.UserSavedAssets.Count,
                        SpellId = s.SpellId,
                        Name = s.Name,
                        Description = s.Description,
                        KeyAttribute = s.KeyAttribute,
                        MinValue = s.MinValue,
                        ManaCost = s.ManaCost,
                        Effects = s.Effects,
                        IsSaved = s.Asset.UserSavedAssets.Any(usa => usa.UId == userId),
                        PrefferedLanguage = s.Asset.Language,
                        CreatorNavigation = new UserSimplifiedResponse()
                        {
                            UId = s.Asset.Author.UId,
                            Username = s.Asset.Author.Username,
                            Picture = s.Asset.Author.Picture,
                        }
                    })
                    //.Where(p => p...)  <- kategoria
                    .OrderByDescending(p => p.CreationDate)
                    .ToList();


                int pageCount = (int)Math.Ceiling((double)result.Count / spellsOnPageAmount);

                result = result
                    .Skip(spellsOnPageAmount * (page - 1))
                    .Take(spellsOnPageAmount)
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

        public async Task<ICollection<GetSpellSimplifiedResponse>> GetSpellsForCharacter(int uId)
        {
            var result = await _dbContext.Spells.Include(s => s.Asset)
                                                .ThenInclude(s => s.UserSavedAssets.Where(u => u.UId.Equals(uId)))
                                               
                    .Where(s => s.Asset.IsPublic || s.Asset.AuthorId == uId)

                    .Select(s => new GetSpellSimplifiedResponse()
                    {
                        AssetId = s.Asset.AssetId,
                        SpellId = s.SpellId,
                        Name = s.Name,
                        Description = s.Description,
                        KeyAttribute = s.KeyAttribute,
                        MinValue = s.MinValue,
                        ManaCost = s.ManaCost,
                        Effects = s.Effects,
                        IsSaved = s.Asset.UserSavedAssets.Any(usa => usa.UId == uId),
                        PrefferedLanguage = s.Asset.Language,
                    })
                    .ToListAsync();


            return result;
        }

        public async Task<GetSpellResponse> PostSpell(PostSpellRequest postSpellRequest)
        {
            if (postSpellRequest == null) throw new ArgumentNullException(nameof(postSpellRequest));

            //if user exists
            if (!_dbContext.Users.Where(u => u.UId == postSpellRequest.UId).ToList().Any())
                throw new ArgumentException($"User with id: {postSpellRequest.UId} does not exist");

            //if language exists
            if (!Enum.IsDefined(typeof(Language), postSpellRequest.Language))
                throw new InvalidDataException($"Language '{postSpellRequest.Language}' is not supported");

            //if attribute exists, or is null
            if (!Enum.IsDefined(typeof(Attributes), postSpellRequest.KeyAttribute ?? "none"))
                throw new InvalidDataException($"Attribute '{postSpellRequest?.KeyAttribute}' does not exist");


            var asset = new Asset()
            {
                AssetId = (_dbContext.Assets.Max(a => (int)a.AssetId) + 1),
                AuthorId = postSpellRequest.UId,
                IsPublic = postSpellRequest.IsPublic,
                Language = postSpellRequest.Language,
                CreationDate = DateTime.Now,
                Author = await _dbContext.Users.Where(u => u.UId == postSpellRequest.UId).SingleAsync(),
            };

            var spell = new Spell()
            {
                SpellId = (_dbContext.Spells.Max(s => (int)s.AssetId) + 1),
                AssetId = asset.AssetId,
                Name = postSpellRequest.Name,
                Description = postSpellRequest.Description,
                KeyAttribute = postSpellRequest.KeyAttribute,
                MinValue = postSpellRequest.MinValue,
                ManaCost = postSpellRequest.ManaCost,
                Effects = postSpellRequest.Effects,
                Asset = asset,
            };

            asset.Spells.Add(spell);

            _dbContext.Assets.Add(asset);
            _dbContext.Spells.Add(spell);
            _dbContext.SaveChanges();

            return new GetSpellResponse()
            {
                AssetId = asset.AssetId,
                CreationDate = asset.CreationDate,
                TimesSaved = asset.UserSavedAssets.Count,
                SpellId = spell.SpellId,
                Name = spell.Name,
                Description = spell.Description,
                KeyAttribute = spell.KeyAttribute,
                MinValue = spell.MinValue,
                ManaCost = spell.ManaCost,
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
