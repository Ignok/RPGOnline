using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Domain.Models;

namespace RPGOnline.Infrastructure.Models
{
    public partial class RPGOnlineDbContext : DbContext, IApplicationDbContext
    {
        public RPGOnlineDbContext()
        {
        }

        public RPGOnlineDbContext(DbContextOptions<RPGOnlineDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Achievement> Achievements { get; set; } = null!;
        public virtual DbSet<Asset> Assets { get; set; } = null!;
        public virtual DbSet<Character> Characters { get; set; } = null!;
        public virtual DbSet<CharacterItem> CharacterItems { get; set; } = null!;
        public virtual DbSet<Characteristic> Characteristics { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Friendship> Friendships { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameMap> GameMaps { get; set; } = null!;
        public virtual DbSet<GameParticipant> GameParticipants { get; set; } = null!;
        public virtual DbSet<GameParticipantItem> GameParticipantItems { get; set; } = null!;
        public virtual DbSet<GameParticipantNote> GameParticipantNotes { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Map> Maps { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Motivation> Motivations { get; set; } = null!;
        public virtual DbSet<Note> Notes { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Profession> Professions { get; set; } = null!;
        public virtual DbSet<ProfessionStartingItem> ProfessionStartingItems { get; set; } = null!;
        public virtual DbSet<Race> Races { get; set; } = null!;
        public virtual DbSet<Spell> Spells { get; set; } = null!;
        public virtual DbSet<TokenOnMap> TokenOnMaps { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserAchievement> UserAchievements { get; set; } = null!;
        public virtual DbSet<UserLikedPost> UserLikedPosts { get; set; } = null!;
        public virtual DbSet<UserSavedAsset> UserSavedAssets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:rpg-online-server.database.windows.net,1433;Initial Catalog=DBRPGOnline;Persist Security Info=False;User ID=NiceDiceGigaChad;Password=ImmortalBoczek!23;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.ToTable("Achievement");

                entity.Property(e => e.AchievementId)
                    .ValueGeneratedNever()
                    .HasColumnName("achievement_id");

                entity.Property(e => e.AchievementName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("achievement_name");

                entity.Property(e => e.Description)
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Picture)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("picture");
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("Asset");

                entity.Property(e => e.AssetId)
                    .ValueGeneratedNever()
                    .HasColumnName("asset_id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.IsPublic).HasColumnName("is_public");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Language)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("language");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Asset_User");
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("Character");

                entity.Property(e => e.CharacterId)
                    .ValueGeneratedNever()
                    .HasColumnName("character_id");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("avatar");

                entity.Property(e => e.CharacterName)
                    .HasMaxLength(80)
                    .HasColumnName("character_name");

                entity.Property(e => e.CharacteristicsJson)
                    .HasMaxLength(280)
                    .HasColumnName("characteristics_json");

                entity.Property(e => e.Gold).HasColumnName("gold");

                entity.Property(e => e.MotivationJson)
                    .HasMaxLength(280)
                    .HasColumnName("motivation_json");

                entity.Property(e => e.ProfessionId).HasColumnName("profession_id");

                entity.Property(e => e.ProficiencyJson)
                    .HasMaxLength(280)
                    .HasColumnName("proficiency_json");

                entity.Property(e => e.RaceId).HasColumnName("race_id");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(280)
                    .HasColumnName("remarks");

                entity.Property(e => e.Kind)
                    .HasMaxLength(40)
                    .HasColumnName("kind");

                entity.Property(e => e.SkillsetJson)
                    .HasMaxLength(280)
                    .HasColumnName("skillset_json");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Character_Asset");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.ProfessionId)
                    .HasConstraintName("Character_Profession");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.RaceId)
                    .HasConstraintName("Character_Race");

                entity.HasMany(d => d.Spells)
                    .WithMany(p => p.Characters)
                    .UsingEntity<Dictionary<string, object>>(
                        "CharacterSpell",
                        l => l.HasOne<Spell>().WithMany().HasForeignKey("SpellId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Character_spells_Spell"),
                        r => r.HasOne<Character>().WithMany().HasForeignKey("CharacterId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Character_spells_Character"),
                        j =>
                        {
                            j.HasKey("CharacterId", "SpellId").HasName("Character_spells_pk");

                            j.ToTable("Character_spells");

                            j.IndexerProperty<int>("CharacterId").HasColumnName("character_id");

                            j.IndexerProperty<int>("SpellId").HasColumnName("spell_id");
                        });
            });

            modelBuilder.Entity<CharacterItem>(entity =>
            {
                entity.HasKey(e => e.CharacterItemsId)
                    .HasName("Character_items_pk");

                entity.ToTable("Character_items");

                entity.Property(e => e.CharacterItemsId)
                    .ValueGeneratedNever()
                    .HasColumnName("character_items_id");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterItems)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Character_items_Character");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.CharacterItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Character_items_Item");
            });

            modelBuilder.Entity<Characteristic>(entity =>
            {
                entity.HasKey(e => e.CharacteristicsId)
                    .HasName("Characteristics_pk");

                entity.Property(e => e.CharacteristicsId)
                    .ValueGeneratedNever()
                    .HasColumnName("characteristics_id");

                entity.Property(e => e.Beliefs)
                    .HasMaxLength(140)
                    .HasColumnName("beliefs");

                entity.Property(e => e.Face)
                    .HasMaxLength(140)
                    .HasColumnName("face");

                entity.Property(e => e.Origins)
                    .HasMaxLength(140)
                    .HasColumnName("origins");

                entity.Property(e => e.Posture)
                    .HasMaxLength(140)
                    .HasColumnName("posture");

                entity.Property(e => e.Temperament)
                    .HasMaxLength(140)
                    .HasColumnName("temperament");

                entity.Property(e => e.Voice)
                    .HasMaxLength(80)
                    .HasColumnName("voice");

                entity.Property(e => e.Language)
                    .HasMaxLength(2)
                    .HasColumnName("language");
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("Chat_message_pk");

                entity.ToTable("Chat_message");

                entity.Property(e => e.MessageId)
                    .ValueGeneratedNever()
                    .HasColumnName("message_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(280)
                    .HasColumnName("content");

                entity.Property(e => e.GameParticipantId).HasColumnName("game_participant_id");

                entity.HasOne(d => d.GameParticipant)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.GameParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Chat_message_Game_participant");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId)
                    .ValueGeneratedNever()
                    .HasColumnName("comment_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(280)
                    .HasColumnName("content");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.ResponseCommentId).HasColumnName("response_comment_id");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Comment_Post");

                entity.HasOne(d => d.ResponseComment)
                    .WithMany(p => p.InverseResponseComment)
                    .HasForeignKey(d => d.ResponseCommentId)
                    .HasConstraintName("Comment_Comment");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Comment_User");
            });

            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.FriendUId })
                    .HasName("Friendship_pk");

                entity.ToTable("Friendship");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.FriendUId).HasColumnName("friend_u_id");

                entity.Property(e => e.IsFriend).HasColumnName("is_friend");

                entity.Property(e => e.IsFollowed).HasColumnName("is_followed");

                entity.Property(e => e.IsBlocked).HasColumnName("is_Blocked");

                entity.Property(e => e.IsRequestSent).HasColumnName("is_request_sent");

                entity.Property(e => e.IsRequestReceived).HasColumnName("is_request_received");
                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.HasOne(d => d.FriendU)
                    .WithMany(p => p.FriendshipFriendUs)
                    .HasForeignKey(d => d.FriendUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Friendship_Friend");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.FriendshipUIdNavigations)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Friendship_User");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameId)
                    .ValueGeneratedNever()
                    .HasColumnName("game_id");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("date")
                    .HasColumnName("creation_date");

                entity.Property(e => e.CreatorUId).HasColumnName("creator_u_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(280)
                    .HasColumnName("description");

                entity.Property(e => e.GameName)
                    .HasMaxLength(80)
                    .HasColumnName("game_name");

                entity.Property(e => e.GameStatus).HasColumnName("game_status");

                entity.Property(e => e.LastPlayed)
                    .HasColumnType("date")
                    .HasColumnName("last_played");

                entity.HasOne(d => d.CreatorU)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.CreatorUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_User");
            });

            modelBuilder.Entity<GameMap>(entity =>
            {
                entity.HasKey(e => e.GameMapsId)
                    .HasName("Game_maps_pk");

                entity.ToTable("Game_maps");

                entity.Property(e => e.GameMapsId)
                    .ValueGeneratedNever()
                    .HasColumnName("game_maps_id");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.HorizontalBox).HasColumnName("horizontal_box");

                entity.Property(e => e.MapId).HasColumnName("map_id");

                entity.Property(e => e.VerticalBox).HasColumnName("vertical_box");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameMaps)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_maps_Game");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.GameMaps)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_maps_Map");
            });

            modelBuilder.Entity<GameParticipant>(entity =>
            {
                entity.ToTable("Game_participant");

                entity.Property(e => e.GameParticipantId)
                    .ValueGeneratedNever()
                    .HasColumnName("game_participant_id");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.Colour).HasColumnName("colour");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.Gold).HasColumnName("gold");

                entity.Property(e => e.HealthPoints).HasColumnName("health_points");

                entity.Property(e => e.ManaPoints).HasColumnName("mana_points");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.GameParticipants)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("Game_participant_Character");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameParticipants)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_participant_Game");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.GameParticipants)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_participant_User");

                entity.HasMany(d => d.Spells)
                    .WithMany(p => p.GameParticipants)
                    .UsingEntity<Dictionary<string, object>>(
                        "GameParticipantSpell",
                        l => l.HasOne<Spell>().WithMany().HasForeignKey("SpellId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Game_participant_spells_Spell"),
                        r => r.HasOne<GameParticipant>().WithMany().HasForeignKey("GameParticipantId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Game_participant_spells_Game_participant"),
                        j =>
                        {
                            j.HasKey("GameParticipantId", "SpellId").HasName("Game_participant_spells_pk");

                            j.ToTable("Game_participant_spells");

                            j.IndexerProperty<int>("GameParticipantId").HasColumnName("game_participant_id");

                            j.IndexerProperty<int>("SpellId").HasColumnName("spell_id");
                        });
            });

            modelBuilder.Entity<GameParticipantItem>(entity =>
            {
                entity.HasKey(e => e.GameParticipantItemsId)
                    .HasName("Game_participant_items_pk");

                entity.ToTable("Game_participant_items");

                entity.Property(e => e.GameParticipantItemsId)
                    .ValueGeneratedNever()
                    .HasColumnName("game_participant_items_id");

                entity.Property(e => e.GameParticipantId).HasColumnName("game_participant_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.GameParticipant)
                    .WithMany(p => p.GameParticipantItems)
                    .HasForeignKey(d => d.GameParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_participant_items_Game_participant");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.GameParticipantItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_participant_items_Item");
            });

            modelBuilder.Entity<GameParticipantNote>(entity =>
            {
                entity.HasKey(e => e.GameParticipantNotesId)
                    .HasName("Game_participant_notes_pk");

                entity.ToTable("Game_participant_notes");

                entity.Property(e => e.GameParticipantNotesId)
                    .ValueGeneratedNever()
                    .HasColumnName("game_participant_notes_id");

                entity.Property(e => e.FolderName)
                    .HasMaxLength(20)
                    .HasColumnName("folder_name");

                entity.Property(e => e.GameParticipantId).HasColumnName("game_participant_id");

                entity.Property(e => e.NoteId).HasColumnName("note_id");

                entity.HasOne(d => d.GameParticipant)
                    .WithMany(p => p.GameParticipantNotes)
                    .HasForeignKey(d => d.GameParticipantId)
                    .HasConstraintName("Game_participant_notes_Game_participant");

                entity.HasOne(d => d.Note)
                    .WithMany(p => p.GameParticipantNotes)
                    .HasForeignKey(d => d.NoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_participant_notes_Note");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.ItemId)
                    .ValueGeneratedNever()
                    .HasColumnName("item_id");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(280)
                    .HasColumnName("description");

                entity.Property(e => e.GoldMultiplier).HasColumnName("gold_multiplier");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("item_name");

                entity.Property(e => e.KeySkill)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("key_skill");

                entity.Property(e => e.SkillMod).HasColumnName("skill_mod");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_Asset");
            });

            modelBuilder.Entity<Map>(entity =>
            {
                entity.ToTable("Map");

                entity.Property(e => e.MapId)
                    .ValueGeneratedNever()
                    .HasColumnName("map_id");

                entity.Property(e => e.AuthorUId).HasColumnName("author_u_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(280)
                    .HasColumnName("description");

                entity.Property(e => e.IsPublic).HasColumnName("is_public");

                entity.Property(e => e.MapName)
                    .HasMaxLength(40)
                    .HasColumnName("map_name");

                entity.Property(e => e.Picture)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("picture");

                entity.HasOne(d => d.AuthorU)
                    .WithMany(p => p.Maps)
                    .HasForeignKey(d => d.AuthorUId)
                    .HasConstraintName("Map_User");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.MessageId)
                    .ValueGeneratedNever()
                    .HasColumnName("message_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(280)
                    .HasColumnName("content");

                entity.Property(e => e.ReceiverUId).HasColumnName("receiver_u_id");

                entity.Property(e => e.SendDate)
                    .HasColumnType("datetime")
                    .HasColumnName("send_date");

                entity.Property(e => e.IsOpened).HasColumnName("is_opened");

                entity.Property(e => e.SenderUId).HasColumnName("sender_u_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.ReceiverU)
                    .WithMany(p => p.MessageReceiverUs)
                    .HasForeignKey(d => d.ReceiverUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Message_Receiver_User");

                entity.HasOne(d => d.SenderU)
                    .WithMany(p => p.MessageSenderUs)
                    .HasForeignKey(d => d.SenderUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Message_Sender_User");
            });

            modelBuilder.Entity<Motivation>(entity =>
            {
                entity.ToTable("Motivation");

                entity.Property(e => e.MotivationId)
                    .ValueGeneratedNever()
                    .HasColumnName("motivation_id");

                entity.Property(e => e.Destination)
                    .HasMaxLength(140)
                    .HasColumnName("destination");

                entity.Property(e => e.HowHappened)
                    .HasMaxLength(140)
                    .HasColumnName("how_happened");

                entity.Property(e => e.Objective)
                    .HasMaxLength(80)
                    .HasColumnName("objective");

                entity.Property(e => e.Subject)
                    .HasMaxLength(140)
                    .HasColumnName("subject");

                entity.Property(e => e.WhatHappened)
                    .HasMaxLength(140)
                    .HasColumnName("what_happened");

                entity.Property(e => e.WhereHappened)
                    .HasMaxLength(140)
                    .HasColumnName("where_happened");

                entity.Property(e => e.Language)
                    .HasMaxLength(2)
                    .HasColumnName("language");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("Note");

                entity.Property(e => e.NoteId)
                    .ValueGeneratedNever()
                    .HasColumnName("note_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(1080)
                    .HasColumnName("content");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.Picture)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("picture");

                entity.Property(e => e.Title)
                    .HasMaxLength(80)
                    .HasColumnName("title");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Note_Game");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.PostId)
                    .ValueGeneratedNever()
                    .HasColumnName("post_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(1080)
                    .HasColumnName("content");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Tag)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("tag");

                entity.Property(e => e.Picture)
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("picture");

                entity.Property(e => e.Title)
                    .HasMaxLength(40)
                    .HasColumnName("title");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_Post");
            });

            modelBuilder.Entity<Profession>(entity =>
            {
                entity.ToTable("Profession");

                entity.Property(e => e.ProfessionId)
                    .ValueGeneratedNever()
                    .HasColumnName("profession_id");

                entity.Property(e => e.ArmorMod).HasColumnName("armor_mod");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.CompanionMod).HasColumnName("companion_mod");

                entity.Property(e => e.Description)
                    .HasMaxLength(280)
                    .HasColumnName("description");

                entity.Property(e => e.GadgetMod).HasColumnName("gadget_mod");

                entity.Property(e => e.HiddenTalent)
                    .HasMaxLength(280)
                    .HasColumnName("hidden_talent");

                entity.Property(e => e.KeyAttribute)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("key_attribute");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .HasColumnName("profession_name");

                entity.Property(e => e.PsycheMod).HasColumnName("psyche_mod");

                entity.Property(e => e.Talent)
                    .HasMaxLength(280)
                    .HasColumnName("talent");

                entity.Property(e => e.WeaponMod).HasColumnName("weapon_mod");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Professions)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Profession_Asset");
            });

            modelBuilder.Entity<ProfessionStartingItem>(entity =>
            {
                entity.HasKey(e => e.ProfessionStartingItemsId)
                    .HasName("Profession_starting_items_pk");

                entity.ToTable("Profession_starting_items");

                entity.Property(e => e.ProfessionStartingItemsId)
                    .ValueGeneratedNever()
                    .HasColumnName("profession_starting_items_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.ProfessionId).HasColumnName("profession_id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ProfessionStartingItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Profession_starting_items_Item");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.ProfessionStartingItems)
                    .HasForeignKey(d => d.ProfessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Profession_starting_items_Profession");
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.ToTable("Race");

                entity.Property(e => e.RaceId)
                    .ValueGeneratedNever()
                    .HasColumnName("race_id");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(280)
                    .HasColumnName("description");

                entity.Property(e => e.HiddenTalent)
                    .HasMaxLength(280)
                    .HasColumnName("hidden_talent");

                entity.Property(e => e.KeyAttribute)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("key_attribute");

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .HasColumnName("race_name");

                entity.Property(e => e.Talent)
                    .HasMaxLength(280)
                    .HasColumnName("talent");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Races)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Race_Asset");
            });

            modelBuilder.Entity<Spell>(entity =>
            {
                entity.ToTable("Spell");

                entity.Property(e => e.SpellId)
                    .ValueGeneratedNever()
                    .HasColumnName("spell_id");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(280)
                    .HasColumnName("description");

                entity.Property(e => e.Effects)
                    .HasMaxLength(280)
                    .HasColumnName("effects");

                entity.Property(e => e.KeyAttribute)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("key_attribute");

                entity.Property(e => e.ManaCost).HasColumnName("mana_cost");

                entity.Property(e => e.MinValue).HasColumnName("min_value");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("spell_name");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.Spells)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Spell_Asset");

                entity.HasMany(d => d.Professions)
                    .WithMany(p => p.Spells)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProfessionStartingSpell",
                        l => l.HasOne<Profession>().WithMany().HasForeignKey("ProfessionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Profession_starting_spells_Profession"),
                        r => r.HasOne<Spell>().WithMany().HasForeignKey("SpellId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Profession_starting_spells_Spell"),
                        j =>
                        {
                            j.HasKey("SpellId", "ProfessionId").HasName("Profession_starting_spells_pk");

                            j.ToTable("Profession_starting_spells");

                            j.IndexerProperty<int>("SpellId").HasColumnName("spell_id");

                            j.IndexerProperty<int>("ProfessionId").HasColumnName("profession_id");
                        });
            });

            modelBuilder.Entity<TokenOnMap>(entity =>
            {
                entity.ToTable("Token_on_map");

                entity.Property(e => e.TokenOnMapId)
                    .ValueGeneratedNever()
                    .HasColumnName("token_on_map_id");

                entity.Property(e => e.GameMapId).HasColumnName("game_map_id");

                entity.Property(e => e.GameParticipantId).HasColumnName("game_participant_id");

                entity.Property(e => e.HorizontalPosition).HasColumnName("horizontal_position");

                entity.Property(e => e.VerticalPosition).HasColumnName("vertical_position");

                entity.HasOne(d => d.GameMap)
                    .WithMany(p => p.TokenOnMaps)
                    .HasForeignKey(d => d.GameMapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Token_on_map_Game_maps");

                entity.HasOne(d => d.GameParticipant)
                    .WithMany(p => p.TokenOnMaps)
                    .HasForeignKey(d => d.GameParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Token_on_map_Game_participant");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("User_pk");

                entity.ToTable("User");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.AboutMe)
                    .HasMaxLength(280)
                    .HasColumnName("about_me");

                entity.Property(e => e.Attitude)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("attitude");

                entity.Property(e => e.City)
                    .HasMaxLength(80)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("country");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("date")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Email)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Picture)
                    .HasColumnName("picture");

                entity.Property(e => e.Pswd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("pswd");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("refresh_token");

                entity.Property(e => e.RefreshTokenExp)
                    .HasColumnType("date")
                    .HasColumnName("refresh_token_exp");

                entity.Property(e => e.Salt)
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("salt");

                entity.Property(e => e.Username)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.AchievementId })
                    .HasName("User_achievement_pk");

                entity.ToTable("User_achievement");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.AchievementId).HasColumnName("achievement_id");

                entity.Property(e => e.CompletionDate)
                    .HasColumnType("date")
                    .HasColumnName("completion_date");

                entity.HasOne(d => d.Achievement)
                    .WithMany(p => p.UserAchievements)
                    .HasForeignKey(d => d.AchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_achievement_Achievement");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.UserAchievements)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_achievement_User");
            });

            modelBuilder.Entity<UserLikedPost>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.PostId })
                    .HasName("User_liked_post_pk");

                entity.ToTable("User_liked_post");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.LikeDate)
                    .HasColumnType("date")
                    .HasColumnName("like_date");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UserLikedPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_liked_post_Post");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.UserLikedPosts)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_likedPost_User");
            });

            modelBuilder.Entity<UserSavedAsset>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.AssetId })
                    .HasName("User_saved_asset_pk");

                entity.ToTable("User_saved_asset");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.AssetId).HasColumnName("asset_id");

                entity.Property(e => e.SaveDate)
                    .HasColumnType("date")
                    .HasColumnName("save_date");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.UserSavedAssets)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_saved_asset_Asset");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.UserSavedAssets)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_saved_asset_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
