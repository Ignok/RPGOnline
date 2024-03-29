﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RPGOnline.Domain.Models;

namespace RPGOnline.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterItem> CharacterItems { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameMap> GameMaps { get; set; }
        public DbSet<GameParticipant> GameParticipants { get; set; }
        public DbSet<GameParticipantItem> GameParticipantItems { get; set; }
        public DbSet<GameParticipantNote> GameParticipantNotes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Motivation> Motivations { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ProfessionStartingItem> ProfessionStartingItems { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<TokenOnMap> TokenOnMaps { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<UserLikedPost> UserLikedPosts { get; set; }
        public DbSet<UserSavedAsset> UserSavedAssets { get; set; }
        public int SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public EntityEntry Entry(object entity);
    }
}
