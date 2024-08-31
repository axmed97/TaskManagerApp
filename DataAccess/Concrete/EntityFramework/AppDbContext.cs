using Core.Configuration;
using Core.Entities.Concrete;
using Entities.Common;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server = ASUS; Database = SolutionArchDb; Trusted_Connection = True; MultipleActiveResultSets = True; TrustServerCertificate = True;");
            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        public DbSet<FileEntity> FileEntities { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TaskEntity> TaskEntities { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<TaskEntity>()
                .HasOne(x => x.CreateBy)
                .WithMany(x => x.TaskEntities)
                .HasForeignKey(x => x.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TaskEntity>()
                .HasOne(x => x.UpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.UpdatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TaskEntity>()
                .HasOne(x => x.DeletedBy)
                .WithMany()
                .HasForeignKey(x => x.DeletedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<GroupUser>()
            //    .HasKey(x => new { x.UserId, x.GroupId });

            builder.Entity<GroupUser>()
                .HasOne(x => x.Group)
                .WithMany(x => x.GroupUsers)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GroupUser>()
                .HasOne(x => x.User)
                .WithMany(x => x.GroupUsers)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Group>()
                .HasOne(x => x.User)
                .WithMany(x => x.Groups)
                .HasForeignKey(x => x.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
                .HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
                .HasOne(x => x.To)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.ToId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<AppRole>().ToTable("Roles");
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var datas = ChangeTracker
                 .Entries<BaseEntity>();

            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow.AddHours(4),
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow.AddHours(4),
                    _ => DateTime.UtcNow
                };
            }
        }
    }
}
