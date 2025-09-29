using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistence.Entities;

namespace TodoApp.Infrastructure.Persistence;

public sealed class TodoAppDbContext : DbContext
{
    public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<TodoListEntity> TodoLists => Set<TodoListEntity>();
    public DbSet<TodoItemEntity> TodoItems => Set<TodoItemEntity>();
    public DbSet<SharedListEntity> SharedLists => Set<SharedListEntity>();
    public DbSet<FriendshipEntity> Friendships => Set<FriendshipEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(builder =>
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(320);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<TodoListEntity>(builder =>
        {
            builder.ToTable("TodoLists");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.OwnerId).IsRequired();
            builder.HasOne(x => x.Owner)
                .WithMany(x => x.TodoLists)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TodoItemEntity>(builder =>
        {
            builder.ToTable("TodoItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.HasOne(x => x.TodoList)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SharedListEntity>(builder =>
        {
            builder.ToTable("SharedLists");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Permission).IsRequired().HasMaxLength(32);
            builder.Property(x => x.GrantedAt).IsRequired();
            builder.HasOne(x => x.TodoList)
                .WithMany(x => x.SharedWith)
                .HasForeignKey(x => x.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => new { x.TodoListId, x.SharedWithUserId }).IsUnique();
        });

        modelBuilder.Entity<FriendshipEntity>(builder =>
        {
            builder.ToTable("Friendships");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.HasOne(x => x.Requester)
                .WithMany(x => x.FriendshipsRequested)
                .HasForeignKey(x => x.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Addressee)
                .WithMany(x => x.FriendshipsReceived)
                .HasForeignKey(x => x.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new { x.RequesterId, x.AddresseeId }).IsUnique();
        });
    }
}
