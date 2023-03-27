using Swivel.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Swivel.Data.Identity
{

    public class DataContext : IdentityDbContext<User, IdentityRole, string, IdentityUserLogin
        , IdentityUserRole, IdentityUserClaim>
    {
        public DataContext() : base("name=aspnetrun_cs")
        {
            Database.SetInitializer(new AuthContextInitializer());
        }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Media> Medias { get; set; }
        public static DataContext Create()
        {
            return new DataContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>()
                        .HasRequired<User>(s => s.User)
                        .WithMany(g => g.Jobs)
                        .HasForeignKey(s => s.UserId)
                        .WillCascadeOnDelete();

            modelBuilder.Entity<Media>()
                        .HasRequired<Job>(s => s.Job)
                        .WithMany(g => g.Medias)
                        .HasForeignKey(s => s.JobId)
                        .WillCascadeOnDelete();

            #region table naming conventions
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UsersInRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            #endregion
        }

        public override async Task<int> SaveChangesAsync()
        {
            UpdateAuditEntities();
            return await base.SaveChangesAsync();
        }
        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = (BaseEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = entity?.CreatedBy;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
            }
        }

    }


    public class AuthContextInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            base.Seed(context);

            Identity.Seed.Init(context);
        }
    }
}