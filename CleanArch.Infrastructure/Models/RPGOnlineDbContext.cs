using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RPGOnline.Infrastructure.Models
{
    public partial class RPGOnlineDbContext : DbContext
    {
        public RPGOnlineDbContext()
        {
        }

        public RPGOnlineDbContext(DbContextOptions<RPGOnlineDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Achievement> Achievements { get; set; } = null!;
        public virtual DbSet<Avatar> Avatars { get; set; } = null!;
        public virtual DbSet<BookRule> BookRules { get; set; } = null!;
        public virtual DbSet<Character> Characters { get; set; } = null!;
        public virtual DbSet<CharacterDescription> CharacterDescriptions { get; set; } = null!;
        public virtual DbSet<CharacterItem> CharacterItems { get; set; } = null!;
        public virtual DbSet<CharacterSkill> CharacterSkills { get; set; } = null!;
        public virtual DbSet<CharacterSpell> CharacterSpells { get; set; } = null!;
        public virtual DbSet<CharacterTrait> CharacterTraits { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Description> Descriptions { get; set; } = null!;
        public virtual DbSet<FeatureGlossary> FeatureGlossaries { get; set; } = null!;
        public virtual DbSet<Friendship> Friendships { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameMap> GameMaps { get; set; } = null!;
        public virtual DbSet<GameParticipant> GameParticipants { get; set; } = null!;
        public virtual DbSet<GameParticipantItem> GameParticipantItems { get; set; } = null!;
        public virtual DbSet<GameParticipantNote> GameParticipantNotes { get; set; } = null!;
        public virtual DbSet<GameParticipantSpell> GameParticipantSpells { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<ItemSkillModification> ItemSkillModifications { get; set; } = null!;
        public virtual DbSet<Map> Maps { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Note> Notes { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Spell> Spells { get; set; } = null!;
        public virtual DbSet<TokenOnMap> TokenOnMaps { get; set; } = null!;
        public virtual DbSet<Trait> Traits { get; set; } = null!;
        public virtual DbSet<TraitSkillModification> TraitSkillModifications { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserAchievement> UserAchievements { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:rpg-online-server.database.windows.net,1433;Initial Catalog=DBRPGOnline;Persist Security Info=False;User ID=NiceDiceGigaChad;Password=ImmortalBoczek!23;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        public Task<List<Post>> AllPosts(int postId) =>
            Posts.FromSqlRaw(
                @"WITH peepeepoopoo (post_id, u_id, username, title, content, picture, creation_date, likes, comments) AS (
                    SELECT p.post_id, u.u_id, u.username, p.title, p.content, p.picture, p.creation_date, COUNT(ulp.post_id), COUNT(c.comment_id)
                    FROM Post p
                    JOIN User_liked_post ulp ON ulp.post_id = p.post_id
                    JOIN [User] u ON u.u_id = p.u_id
                    LEFT JOIN Comment c ON c.post_id = p.post_id
                    GROUP BY p.post_id, u.u_id, u.username, p.title, p.content, p.picture, p.creation_date
                )
                SELECT * FROM peepeepoopoo", postId)
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

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

                entity.Property(e => e.Commentary)
                    .HasMaxLength(280)
                    .IsUnicode(false)
                    .HasColumnName("commentary");

                entity.Property(e => e.Picture)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("picture");
            });

            modelBuilder.Entity<Avatar>(entity =>
            {
                entity.ToTable("Avatar");

                entity.Property(e => e.AvatarId)
                    .ValueGeneratedNever()
                    .HasColumnName("avatar_id");

                entity.Property(e => e.AuthorUId).HasColumnName("author_u_id");

                entity.Property(e => e.AvatarName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("avatar_name");

                entity.Property(e => e.Picture)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("picture");

                entity.HasOne(d => d.AuthorU)
                    .WithMany(p => p.Avatars)
                    .HasForeignKey(d => d.AuthorUId)
                    .HasConstraintName("Token_User");
            });

            modelBuilder.Entity<BookRule>(entity =>
            {
                entity.HasKey(e => e.BookRulesId)
                    .HasName("Book_rules_pk");

                entity.ToTable("Book_rules");

                entity.Property(e => e.BookRulesId)
                    .ValueGeneratedNever()
                    .HasColumnName("book_rules_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(1080)
                    .HasColumnName("content");

                entity.Property(e => e.Title)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("Character");

                entity.Property(e => e.CharacterId)
                    .ValueGeneratedNever()
                    .HasColumnName("character_id");

                entity.Property(e => e.AvatarId).HasColumnName("avatar_id");

                entity.Property(e => e.CharacterName)
                    .HasMaxLength(80)
                    .HasColumnName("character_name");

                entity.Property(e => e.Gold).HasColumnName("gold");

                entity.Property(e => e.IsPublic).HasColumnName("is_public");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(280)
                    .HasColumnName("remarks");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.HasOne(d => d.Avatar)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.AvatarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Character_Token");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.UId)
                    .HasConstraintName("Character_User");
            });

            modelBuilder.Entity<CharacterDescription>(entity =>
            {
                entity.HasKey(e => e.CharacterDescriptionsId)
                    .HasName("Character_descriptions_pk");

                entity.ToTable("Character_descriptions");

                entity.Property(e => e.CharacterDescriptionsId)
                    .ValueGeneratedNever()
                    .HasColumnName("character_descriptions_id");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.DescriptionId).HasColumnName("description_id");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterDescriptions)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Copy_of_Characters_description_Copy_of_Copy_of_Character");

                entity.HasOne(d => d.Description)
                    .WithMany(p => p.CharacterDescriptions)
                    .HasForeignKey(d => d.DescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Copy_of_Characters_description_Description");
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

            modelBuilder.Entity<CharacterSkill>(entity =>
            {
                entity.HasKey(e => new { e.FeatureId, e.CharacterId })
                    .HasName("Character_skills_pk");

                entity.ToTable("Character_skills");

                entity.Property(e => e.FeatureId).HasColumnName("feature_id");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterSkills)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_60_Character");

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.CharacterSkills)
                    .HasForeignKey(d => d.FeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_60_Feature_glossary");
            });

            modelBuilder.Entity<CharacterSpell>(entity =>
            {
                entity.HasKey(e => e.CharacterSpellsId)
                    .HasName("Character_spells_pk");

                entity.ToTable("Character_spells");

                entity.Property(e => e.CharacterSpellsId)
                    .ValueGeneratedNever()
                    .HasColumnName("character_spells_id");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.SpellId).HasColumnName("spell_id");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterSpells)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Character_spells_Character");

                entity.HasOne(d => d.Spell)
                    .WithMany(p => p.CharacterSpells)
                    .HasForeignKey(d => d.SpellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Character_spells_Spell");
            });

            modelBuilder.Entity<CharacterTrait>(entity =>
            {
                entity.HasKey(e => e.CharacterTraitsId)
                    .HasName("Character_traits_pk");

                entity.ToTable("Character_traits");

                entity.Property(e => e.CharacterTraitsId)
                    .ValueGeneratedNever()
                    .HasColumnName("character_traits_id");

                entity.Property(e => e.CharacterId).HasColumnName("character_id");

                entity.Property(e => e.TraitId).HasColumnName("trait_id");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.CharacterTraits)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_59_Character");

                entity.HasOne(d => d.Trait)
                    .WithMany(p => p.CharacterTraits)
                    .HasForeignKey(d => d.TraitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_59_Trait");
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

            modelBuilder.Entity<Description>(entity =>
            {
                entity.ToTable("Description");

                entity.Property(e => e.DescriptionId)
                    .ValueGeneratedNever()
                    .HasColumnName("description_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.DescriptionFeatureId).HasColumnName("description_feature_id");

                entity.HasOne(d => d.DescriptionFeature)
                    .WithMany(p => p.Descriptions)
                    .HasForeignKey(d => d.DescriptionFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Copy_of_Lineament_Feature_glossary");
            });

            modelBuilder.Entity<FeatureGlossary>(entity =>
            {
                entity.HasKey(e => e.FeatureId)
                    .HasName("Feature_glossary_pk");

                entity.ToTable("Feature_glossary");

                entity.Property(e => e.FeatureId)
                    .ValueGeneratedNever()
                    .HasColumnName("feature_id");

                entity.Property(e => e.FeatureGlossaryName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("feature_glossary_name");
            });

            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.FriendUId })
                    .HasName("Friendship_pk");

                entity.ToTable("Friendship");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.FriendUId).HasColumnName("friend_u_id");

                entity.Property(e => e.FollowStatus).HasColumnName("follow_status");

                entity.Property(e => e.FriendshipStatus).HasColumnName("friendship_status");

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

                entity.Property(e => e.Commentary)
                    .HasMaxLength(280)
                    .HasColumnName("commentary");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("date")
                    .HasColumnName("creation_date");

                entity.Property(e => e.CreatorUId).HasColumnName("creator_u_id");

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

            modelBuilder.Entity<GameParticipantSpell>(entity =>
            {
                entity.HasKey(e => e.GameParticipantSpellsId)
                    .HasName("Game_participant_spells_pk");

                entity.ToTable("Game_participant_spells");

                entity.Property(e => e.GameParticipantSpellsId)
                    .ValueGeneratedNever()
                    .HasColumnName("game_participant_spells_id");

                entity.Property(e => e.GameParticipantId).HasColumnName("game_participant_id");

                entity.Property(e => e.SpellId).HasColumnName("spell_id");

                entity.HasOne(d => d.GameParticipant)
                    .WithMany(p => p.GameParticipantSpells)
                    .HasForeignKey(d => d.GameParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_participant_spells_Game_participant");

                entity.HasOne(d => d.Spell)
                    .WithMany(p => p.GameParticipantSpells)
                    .HasForeignKey(d => d.SpellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Game_participant_spells_Spell");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.ItemId)
                    .ValueGeneratedNever()
                    .HasColumnName("item_id");

                entity.Property(e => e.AuthorUId).HasColumnName("author_u_id");

                entity.Property(e => e.Commentary)
                    .HasMaxLength(280)
                    .HasColumnName("commentary");

                entity.Property(e => e.GoldMultiplier).HasColumnName("gold_multiplier");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(40)
                    .HasColumnName("item_name");

                entity.Property(e => e.KeySkill).HasColumnName("key_skill");

                entity.Property(e => e.MinValue).HasColumnName("min_value");

                entity.HasOne(d => d.AuthorU)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.AuthorUId)
                    .HasConstraintName("Item_User");
            });

            modelBuilder.Entity<ItemSkillModification>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.SkillFeatureId })
                    .HasName("Item_skill_modification_pk");

                entity.ToTable("Item_skill_modification");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.SkillFeatureId).HasColumnName("skill_feature_id");

                entity.Property(e => e.Modifier).HasColumnName("modifier");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemSkillModifications)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_skill_modification_Item");

                entity.HasOne(d => d.SkillFeature)
                    .WithMany(p => p.ItemSkillModifications)
                    .HasForeignKey(d => d.SkillFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Item_skill_modification_Feature_glossary");
            });

            modelBuilder.Entity<Map>(entity =>
            {
                entity.ToTable("Map");

                entity.Property(e => e.MapId)
                    .ValueGeneratedNever()
                    .HasColumnName("map_id");

                entity.Property(e => e.AuthorUId).HasColumnName("author_u_id");

                entity.Property(e => e.Commentary)
                    .HasMaxLength(280)
                    .HasColumnName("commentary");

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

                entity.Property(e => e.Picture)
                    .HasMaxLength(60)
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

            modelBuilder.Entity<Spell>(entity =>
            {
                entity.ToTable("Spell");

                entity.Property(e => e.SpellId)
                    .ValueGeneratedNever()
                    .HasColumnName("spell_id");

                entity.Property(e => e.AuthorUId).HasColumnName("author_u_id");

                entity.Property(e => e.Commentary)
                    .HasMaxLength(280)
                    .HasColumnName("commentary");

                entity.Property(e => e.Effects)
                    .HasMaxLength(280)
                    .HasColumnName("effects");

                entity.Property(e => e.KeySkill).HasColumnName("key_skill");

                entity.Property(e => e.ManaCost).HasColumnName("mana_cost");

                entity.Property(e => e.RequiredValue).HasColumnName("required_value");

                entity.Property(e => e.SpellName)
                    .HasMaxLength(40)
                    .HasColumnName("spell_name");

                entity.HasOne(d => d.AuthorU)
                    .WithMany(p => p.Spells)
                    .HasForeignKey(d => d.AuthorUId)
                    .HasConstraintName("Spell_User");
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

            modelBuilder.Entity<Trait>(entity =>
            {
                entity.ToTable("Trait");

                entity.Property(e => e.TraitId)
                    .ValueGeneratedNever()
                    .HasColumnName("trait_id");

                entity.Property(e => e.AuthorUId).HasColumnName("author_u_id");

                entity.Property(e => e.Commentary)
                    .HasMaxLength(280)
                    .HasColumnName("commentary");

                entity.Property(e => e.HiddenTalent)
                    .HasMaxLength(280)
                    .HasColumnName("hidden_talent");

                entity.Property(e => e.IsRace).HasColumnName("is_race");

                entity.Property(e => e.KeySkill).HasColumnName("key_skill");

                entity.Property(e => e.Talent)
                    .HasMaxLength(280)
                    .HasColumnName("talent");

                entity.Property(e => e.TraitName)
                    .HasMaxLength(80)
                    .HasColumnName("trait_name");

                entity.HasOne(d => d.AuthorU)
                    .WithMany(p => p.Traits)
                    .HasForeignKey(d => d.AuthorUId)
                    .HasConstraintName("Trait_User");
            });

            modelBuilder.Entity<TraitSkillModification>(entity =>
            {
                entity.HasKey(e => new { e.SkillFeatureId, e.TraitId })
                    .HasName("Trait_skill_modification_pk");

                entity.ToTable("Trait_skill_modification");

                entity.Property(e => e.SkillFeatureId).HasColumnName("skill_feature_id");

                entity.Property(e => e.TraitId).HasColumnName("trait_id");

                entity.Property(e => e.Modifier).HasColumnName("modifier");

                entity.HasOne(d => d.SkillFeature)
                    .WithMany(p => p.TraitSkillModifications)
                    .HasForeignKey(d => d.SkillFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Copy_of_Copy_of_Skill_Skill_glossary");

                entity.HasOne(d => d.Trait)
                    .WithMany(p => p.TraitSkillModifications)
                    .HasForeignKey(d => d.TraitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Skill_modification_Trait");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("User_pk");

                entity.ToTable("User");

                entity.Property(e => e.UId)
                    .ValueGeneratedNever()
                    .HasColumnName("u_id");

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
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("picture");

                entity.Property(e => e.Pswd)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("pswd");

                entity.Property(e => e.Username)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasMany(d => d.PostsNavigation)
                    .WithMany(p => p.UIds)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserLikedPost",
                        l => l.HasOne<Post>().WithMany().HasForeignKey("PostId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("User_liked_post_Post"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("User_likedPost_User"),
                        j =>
                        {
                            j.HasKey("UId", "PostId").HasName("User_liked_post_pk");

                            j.ToTable("User_liked_post");

                            j.IndexerProperty<int>("UId").HasColumnName("u_id");

                            j.IndexerProperty<int>("PostId").HasColumnName("post_id");
                        });
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
