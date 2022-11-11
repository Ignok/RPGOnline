using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Services.Profile;
using RPGOnline.Domain.Entities;
using RPGOnline.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<RPGOnline.Domain.Models.Avatar> Avatars { get; set; }
        public DbSet<BookRule> BookRules { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterDescription> CharacterDescriptions { get; set; }
        public DbSet<CharacterItem> CharacterItems { get; set; }
        public DbSet<CharacterSkill> CharacterSkills { get; set; }
        public DbSet<CharacterSpell> CharacterSpells { get; set; }
        public DbSet<CharacterTrait> CharacterTraits { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<FeatureGlossary> FeatureGlossaries { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameMap> GameMaps { get; set; }
        public DbSet<GameParticipant> GameParticipants { get; set; }
        public DbSet<GameParticipantItem> GameParticipantItems { get; set; }
        public DbSet<GameParticipantNote> GameParticipantNotes { get; set; }
        public DbSet<GameParticipantSpell> GameParticipantSpells { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemSkillModification> ItemSkillModifications { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<TokenOnMap> TokenOnMaps { get; set; }
        public DbSet<Trait> Traits { get; set; }
        public DbSet<TraitSkillModification> TraitSkillModifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
