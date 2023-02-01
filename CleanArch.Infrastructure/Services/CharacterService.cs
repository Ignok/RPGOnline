﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Common;
using Newtonsoft.Json;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Responses.Asset.Character.Character;
using RPGOnline.Application.DTOs.Responses.Asset.Item;
using RPGOnline.Application.DTOs.Responses.Asset.Profession;
using RPGOnline.Application.DTOs.Responses.Asset.Race;
using RPGOnline.Application.DTOs.Responses.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.Character;
using RPGOnline.Application.DTOs.Responses.User;
using RPGOnline.Application.Interfaces;
using RPGOnline.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Infrastructure.Services
{
    public class CharacterService : ICharacter
    {
        private readonly IApplicationDbContext _dbContext;
        private ILogger<CharacterService> _logger;

        public CharacterService(IApplicationDbContext dbContext, ILogger<CharacterService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private readonly int charactersOnPageAmount = 5;

        public async Task<(ICollection<CharacterResponse>, int pageCount)> GetCharacters(SearchAssetRequest searchAssetRequest, int userId, CancellationToken cancellationToken)
        {
            try
            {
                var page = searchAssetRequest.Page;
                if (searchAssetRequest.Page <= 0) throw new ArgumentOutOfRangeException(nameof(page));

                var result = await (from character in _dbContext.Characters
                                    join asset in _dbContext.Assets on character.AssetId equals asset.AssetId
                                    join p in _dbContext.Professions on character.ProfessionId equals p.ProfessionId into prof
                                    from profession in prof.DefaultIfEmpty()
                                    join r in _dbContext.Races on character.RaceId equals r.RaceId into rc
                                    from race in rc.DefaultIfEmpty()
                                    where (character.Asset.IsPublic || character.Asset.Author.UId == userId)
                                    //warunek na typ (NPC/ MONSTER/ PLAYABLE)
                                    //warunek na PrefferedLanguage
                                    where (String.IsNullOrEmpty(searchAssetRequest.Search)
                                            || character.CharacterName.Contains(searchAssetRequest.Search, StringComparison.OrdinalIgnoreCase)
                                            || character.Remarks.Contains(searchAssetRequest.Search, StringComparison.OrdinalIgnoreCase)
                                            )
                                    select new CharacterResponse()
                                    {
                                        CharacterId = character.CharacterId,
                                        AssetId = asset.AssetId,
                                        CreationDate = asset.CreationDate,
                                        CharacterName = character.CharacterName,
                                        Remarks = character.Remarks,
                                        Gold = character.Gold,
                                        Avatar = character.Avatar,
                                        JsonResponse = GetFromJsonResponse(character),
                                        Profession = profession != null ? new GetProfessionSimplifiedResponse()
                                        {
                                            AssetId = profession.AssetId,
                                            ProfessionId = profession.ProfessionId,
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
                                        } : null,
                                        Race = race != null ? new GetRaceSimplifiedResponse()
                                        {
                                            RaceId = race.RaceId,
                                            AssetId = race.AssetId,
                                            Name = race.Name,
                                            Description = race.Description,
                                            Talent = race.Talent,
                                            HiddenTalent = race.HiddenTalent,
                                            KeyAttribute = race.KeyAttribute,
                                        } : null,
                                        Items = character.CharacterItems != null ? (
                                        character.CharacterItems
                                            .Select(i =>
                                                new CharacterItemResponse()
                                                {
                                                    AssetId = i.Item.AssetId,
                                                    ItemId = i.Item.ItemId,
                                                    Name = i.Item.Name,
                                                    Description = i.Item.Description,
                                                    KeySkill = i.Item.KeySkill,
                                                    Quantity = i.Quantity,
                                                    AuthorId = i.Item.Asset.AuthorId,
                                                    AuthorUsername = i.Item.Asset.Author.Username,
                                                    IsPublic = i.Item.Asset.IsPublic,
                                                }
                                            )
                                            .ToList()) : null,
                                        Spells = character.Spells != null ? (
                                            character.Spells.Select(s =>
                                                new CharacterSpellResponse()
                                                {
                                                    SpellId = s.SpellId,
                                                    AssetId = s.AssetId,
                                                    Name = s.Name,
                                                    Description = s.Description,
                                                    KeyAttribute = s.KeyAttribute,
                                                    Effects = s.Effects,
                                                    AuthorId = s.Asset.AuthorId,
                                                    AuthorUsername = s.Asset.Author.Username,
                                                    IsPublic = s.Asset.IsPublic,
                                                }
                                            )
                                            .ToList()) : null,
                                        CreatorNavigation = new UserSimplifiedResponse()
                                        {
                                            UId = asset.AuthorId,
                                            Username = asset.Author.Username,
                                            Picture = asset.Author.Picture
                                        }

                                    })
                                .ToListAsync();

                int pageCount = (int)Math.Ceiling((double)result.Count / charactersOnPageAmount);

                result = result
                    .Skip(charactersOnPageAmount * (page - 1))
                    .Take(charactersOnPageAmount)
                    .ToList();

                return (result, pageCount);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("=========== I WAS CANCELLED ==========");
                throw ex;
            }
        }

        public async Task<CharacterResponse> GetCharacter(int characterId)
        {
            var chara = await _dbContext.Characters.Where(c => c.CharacterId == characterId).SingleAsync();

            FromJsonResponse jsonResponse = GetFromJsonResponse(chara);

            var result = await (from character in _dbContext.Characters
                                join asset in _dbContext.Assets on character.AssetId equals asset.AssetId
                                join p in _dbContext.Professions on character.ProfessionId equals p.ProfessionId into prof
                                    from profession in prof.DefaultIfEmpty()
                                join r in _dbContext.Races on character.RaceId equals r.RaceId into rc
                                    from race in rc.DefaultIfEmpty()
                                //join characterItems in _dbContext.CharacterItems on character.CharacterId equals characterItems.CharacterId
                                where (character.CharacterId == characterId)
                                select new CharacterResponse()
                                {
                                    CharacterId = characterId,
                                    AssetId = asset.AssetId,
                                    CreationDate = asset.CreationDate,
                                    CharacterName = character.CharacterName,
                                    Remarks = character.Remarks,
                                    Gold = character.Gold,
                                    Avatar = character.Avatar,
                                    JsonResponse = jsonResponse,
                                    Profession = profession != null ? new GetProfessionSimplifiedResponse()
                                    {
                                        AssetId = profession.AssetId,
                                        ProfessionId = profession.ProfessionId,
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
                                    } : null,
                                    Race = race != null ? new GetRaceSimplifiedResponse()
                                    {
                                        RaceId = race.RaceId,
                                        AssetId = race.AssetId,
                                        Name = race.Name,
                                        Description = race.Description,
                                        Talent = race.Talent,
                                        HiddenTalent = race.HiddenTalent,
                                        KeyAttribute = race.KeyAttribute,
                                    } : null,
                                    Items = character.CharacterItems != null ? (
                                    character.CharacterItems
                                        .Select(i =>
                                            new CharacterItemResponse()
                                            {
                                                AssetId = i.Item.AssetId,
                                                ItemId = i.Item.ItemId,
                                                Name = i.Item.Name,
                                                Description = i.Item.Description,
                                                KeySkill = i.Item.KeySkill,
                                                Quantity = i.Quantity,
                                                AuthorId = i.Item.Asset.AuthorId,
                                                AuthorUsername = i.Item.Asset.Author.Username,
                                                IsPublic = i.Item.Asset.IsPublic,
                                            }
                                        )
                                        .ToList()) : null,
                                    Spells = character.Spells != null ? (
                                        character.Spells.Select(s =>
                                            new CharacterSpellResponse()
                                            {
                                                SpellId = s.SpellId,
                                                AssetId = s.AssetId,
                                                Name = s.Name,
                                                Description = s.Description,
                                                KeyAttribute = s.KeyAttribute,
                                                Effects = s.Effects,
                                                AuthorId = s.Asset.AuthorId,
                                                AuthorUsername = s.Asset.Author.Username,
                                                IsPublic = s.Asset.IsPublic,
                                            }
                                        )
                                        .ToList()) : null,
                                    CreatorNavigation = new UserSimplifiedResponse()
                                    {
                                        UId = asset.AuthorId,
                                        Username = asset.Author.Username,
                                        Picture = asset.Author.Picture
                                    }

                                })
                                .SingleAsync();

            return result;
        }

        private static FromJsonResponse GetFromJsonResponse(Character character)
        {
            var motivation = JsonConvert.DeserializeObject<MotivationResponse>(character.MotivationJson);
            var characteristics = JsonConvert.DeserializeObject<CharacteristicsResponse>(character.CharacteristicsJson);
            var skillset = JsonConvert.DeserializeObject<SkillsetResponse>(character.SkillsetJson);
            var attributes = JsonConvert.DeserializeObject<AttributesResponse>(character.ProficiencyJson);

            var motivationResponse = new MotivationResponse();
            if (motivation != null)
            {
                motivationResponse = new MotivationResponse()
                {
                    Objective = motivation.Objective,
                    Subject = motivation.Subject,
                    What_Happened = motivation.What_Happened,
                    Where_Happened = motivation.Where_Happened,
                    How_Happened = motivation.How_Happened,
                    Destination = motivation.Destination,
                };
            }
            var characteristicsResponse = new CharacteristicsResponse();
            if (characteristics != null)
            {
                characteristicsResponse = new CharacteristicsResponse()
                {
                    Voice = characteristics.Voice,
                    Posture = characteristics.Posture,
                    Temperament = characteristics.Temperament,
                    Beliefs = characteristics.Beliefs,
                    Face = characteristics.Face,
                    Origins = characteristics.Origins,
                };
            }
            var skillsetResponse = new SkillsetResponse();
            if (skillset != null)
            {
                skillsetResponse = new SkillsetResponse()
                {
                    Weapon = skillset.Weapon,
                    Armor = skillset.Armor,
                    Gadget = skillset.Gadget,
                    Companion = skillset.Companion,
                    Psyche = skillset.Psyche,
                };
            }
            var attributesResponse = new AttributesResponse();
            if (attributes != null)
            {
                attributesResponse = new AttributesResponse()
                {
                    Strength = attributes.Strength,
                    Dexterity = attributes.Dexterity,
                    Intelligence = attributes.Intelligence,
                    Charisma = attributes.Charisma,
                    Health = attributes.Health,
                    Mana = attributes.Mana,
                };
            }

            return new FromJsonResponse()
            {
                Motivation = motivationResponse,
                Characteristics = characteristicsResponse,
                Skillset = skillsetResponse,
                Attributes = attributesResponse,
            };
        }

        public async Task<MotivationResponse> GetRandomMotivation()
        {
            var minId = 1;
            var maxId = await _dbContext.Motivations.CountAsync();

            var objective = _dbContext.Motivations
                .Where(m => m.MotivationId == GetRandom(minId, maxId)).Select( m => m.Objective).Single();

            var subject = _dbContext.Motivations
                .Where(m => m.MotivationId == GetRandom(minId, maxId)).Select(m => m.Subject).Single();

            var what = _dbContext.Motivations
                .Where(m => m.MotivationId == GetRandom(minId, maxId)).Select(m => m.WhatHappened).Single();

            var where = _dbContext.Motivations
                .Where(m => m.MotivationId == GetRandom(minId, maxId)).Select(m => m.WhereHappened).Single();

            var how = _dbContext.Motivations
                .Where(m => m.MotivationId == GetRandom(minId, maxId)).Select(m => m.HowHappened).Single();

            var destination = _dbContext.Motivations
                .Where(m => m.MotivationId == GetRandom(minId, maxId)).Select(m => m.Destination).Single();

            return new MotivationResponse()
            {
                Objective = objective,
                Subject = subject,
                What_Happened = what,
                Where_Happened = where,
                How_Happened = how,
                Destination = destination
            };
        }

        public async Task<CharacteristicsResponse> GetRandomCharacteristics()
        {
            var minId = 1;
            var maxId = await _dbContext.Characteristics.CountAsync();

            var voice = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == GetRandom(minId, maxId)).Select(c => c.Voice).Single();

            var posture = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == GetRandom(minId, maxId)).Select(c => c.Posture).Single();

            var temperament = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == GetRandom(minId, maxId)).Select(c => c.Temperament).Single();

            var beliefs = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == GetRandom(minId, maxId)).Select(c => c.Beliefs).Single();

            var face = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == GetRandom(minId, maxId)).Select(c => c.Face).Single();

            var origins = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == GetRandom(minId, maxId)).Select(c => c.Origins).Single();

            return new CharacteristicsResponse()
            {
                Voice = voice,
                Posture = posture,
                Temperament = temperament,
                Beliefs = beliefs,
                Face = face,
                Origins = origins
            };
        }

        public Task<AttributesResponse> GetRandomAttributes()
        {
            var min = 1;
            var max = 6;

            return Task.FromResult(new AttributesResponse()
            {
                Strength = GetRandom(min, max),
                Dexterity = GetRandom(min, max),
                Intelligence = GetRandom(min, max),
                Charisma = GetRandom(min, max),
                Health = GetRandom(min, max),
                Mana = GetRandom(min, max),
            });
        }

        private int GetRandom(int min, int max)
        {
            Random random = new();
            return random.Next(min, max);
        }

        private static async Task<string> FlattenMotivation(params string[] values)
        {
            return 
                $"You have to {(values[0].IsNullOrEmpty() ? "rescue" : values[0])} " +
                $"your {(values[1].IsNullOrEmpty() ? "companion" : values[1])} " +
                $"who was {(values[2].IsNullOrEmpty() ? "kidnapped" : values[2])} " +
                $"in the {(values[3].IsNullOrEmpty() ? "ancient forest" : values[3])} " +
                $"by {(values[4].IsNullOrEmpty() ? "dark elves" : values[4])}, " +
                $"now roaming in the {(values[5].IsNullOrEmpty() ? "Beastmen city" : values[5])}.";
        }

        private static async Task<string> FlattenCharacteristics(params string[] values)
        {
            return
                $"Your voice is {(values[0].IsNullOrEmpty() ? "loud" : values[0])}, " +
                $"and your posture is {(values[1].IsNullOrEmpty() ? "athletic" : values[1])}. " +
                $"You are considered to be {(values[2].IsNullOrEmpty() ? "brave" : values[2])}. " +
                $"You believe in the {(values[3].IsNullOrEmpty() ? "ancient forest" : values[3])}. " +
                $"Your face covered in {(values[4].IsNullOrEmpty() ? "dark elves" : values[4])} " +
                $"indicates your origins - the {(values[5].IsNullOrEmpty() ? "Beastmen city" : values[5])}.";
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
