using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Survey_Basket.Extentions;

namespace Survey_Basket.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor) :
        IdentityDbContext<User,ApplicationRole,string>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Answer> Answers {  get; set; }
        public DbSet<Poll> Polls {  get; set; }
        public DbSet<Question>  Questions {  get; set; }
        public DbSet<Vote>   Votes {  get; set; }
        public DbSet<VoteAnswer>  VoteAnswers {  get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var cascadeFK = modelBuilder.Model
                                        .GetEntityTypes()
                                        .SelectMany(t=>t.GetForeignKeys())
                                        .Where(fk=>fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFK)
                fk.DeleteBehavior = DeleteBehavior.Restrict;



            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var Entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entityEntry in Entries)
            {
                if(entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId!;
                }
                if(entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x=>x.UpdatedById).CurrentValue = currentUserId!;
                    entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
