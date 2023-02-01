using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Common;
using Newtonsoft.Json;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.DTOs.Responses.Asset.Spell;
using RPGOnline.Application.DTOs.Responses.Character;
using RPGOnline.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<CharacterResponse> GetCharacterInfo(int characterId)
        {
            var motivationJson = await _dbContext.Characters.Where(c => c.CharacterId == characterId).Select(c => c.MotivationJson).SingleAsync();
            var motivation = JsonConvert.DeserializeObject<MotivationResponse>(motivationJson);
            
            var characteristicsJson = await _dbContext.Characters.Where(c => c.CharacterId == characterId).Select(c => c.CharacteristicsJson).SingleAsync();
            var characteristics = JsonConvert.DeserializeObject<CharacteristicsResponse>(characteristicsJson);

            if(motivation == null || characteristics == null)
            {
                throw new Exception("Motivation or characteristics are null!");
            }
            else
            {
                var result = await _dbContext.Characters
                .Where(c => c.CharacterId == characterId)
                .Select(c => new CharacterResponse
                {
                    Motivation = new MotivationResponse()
                    {
                        Objective = motivation.Objective,
                        Subject = motivation.Subject,
                        WhatHappened = motivation.WhatHappened,
                        WhereHappened = motivation.WhereHappened,
                        HowHappened = motivation.HowHappened,
                        Destination = motivation.Destination,
                    },
                    Characteristics = new CharacteristicsResponse()
                    {
                        Voice = characteristics.Voice,
                        Posture = characteristics.Posture,
                        Temperament = characteristics.Temperament,
                        Beliefs = characteristics.Beliefs,
                        Face = characteristics.Face,
                        Origins = characteristics.Origins,
                    }
                }).SingleAsync();

                return result;
            }
        }

        public async Task<MotivationResponse> GetMotivation()
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

            //return await FlattenMotivation(objective, subject, what, where, how, destination);

            return new MotivationResponse()
            {
                Objective = objective,
                Subject = subject,
                WhatHappened = what,
                WhereHappened = where,
                HowHappened = how,
                Destination = destination
            };
        }

        public async Task<CharacteristicsResponse> GetCharacteristics()
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

            //return await FlattenCharacteristics(voice, posture, temperament, beliefs, face, origins);

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
    }
}
