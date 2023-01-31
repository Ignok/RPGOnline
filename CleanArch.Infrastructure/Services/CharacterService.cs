using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Common;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<string> GetMotivation()
        {
            var minId = 1;
            var maxId = await _dbContext.Motivations.CountAsync();

            var objective = _dbContext.Motivations
                .Where(m => m.MotivationId == getRandom(minId, maxId)).Select( m => m.Objective).Single();

            var subject = _dbContext.Motivations
                .Where(m => m.MotivationId == getRandom(minId, maxId)).Select(m => m.Subject).Single();

            var what = _dbContext.Motivations
                .Where(m => m.MotivationId == getRandom(minId, maxId)).Select(m => m.WhatHappened).Single();

            var where = _dbContext.Motivations
                .Where(m => m.MotivationId == getRandom(minId, maxId)).Select(m => m.WhereHappened).Single();

            var how = _dbContext.Motivations
                .Where(m => m.MotivationId == getRandom(minId, maxId)).Select(m => m.HowHappened).Single();

            var destination = _dbContext.Motivations
                .Where(m => m.MotivationId == getRandom(minId, maxId)).Select(m => m.Destination).Single();

            return await FlattenMotivation(objective, subject, what, where, how, destination);
        }

        public async Task<string> GetCharacteristics()
        {
            var minId = 1;
            var maxId = await _dbContext.Characteristics.CountAsync();

            var voice = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == getRandom(minId, maxId)).Select(c => c.Voice).Single();

            var posture = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == getRandom(minId, maxId)).Select(c => c.Posture).Single();

            var temperament = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == getRandom(minId, maxId)).Select(c => c.Temperament).Single();

            var beliefs = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == getRandom(minId, maxId)).Select(c => c.Beliefs).Single();

            var face = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == getRandom(minId, maxId)).Select(c => c.Face).Single();

            var origins = _dbContext.Characteristics
                .Where(c => c.CharacteristicsId == getRandom(minId, maxId)).Select(c => c.Origins).Single();

            return await FlattenCharacteristics(voice, posture, temperament, beliefs, face, origins);
        }

        private int getRandom(int min, int max)
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
