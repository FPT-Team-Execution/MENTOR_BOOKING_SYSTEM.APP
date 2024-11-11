using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MBS.BusinessObject.Entities;
using MBS.Externals.Services.Interfaces;
using MBS.BusinessObject.Commom;

namespace MBS.DataAccess
{
    public class MBSContext : IdentityDbContext
    {
        private readonly IClaimExternalService _claimExternalService;
        public MBSContext(DbContextOptions<MBSContext> options, IClaimExternalService claimExternalService) : base(options)
        {
            _claimExternalService = claimExternalService;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //seed data by builder
            MBSContextSeed.SeedPositionAsync(builder);
            //MBSContextSeed.SeedMajorAsync(builder);
            base.OnModelCreating(builder);
        }

        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<BusinessObject.Entities.Group> Groups { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingMember> MeetingMembers { get; set; }
        public DbSet<MentorMajor> MentorMajors { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<BusinessObject.Entities.Request> Requests { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _claimExternalService.GetUserId();
                        entry.Entity.CreatedOn = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _claimExternalService.GetUserId();
                        entry.Entity.UpdatedOn = DateTime.Now;
                        break;
                }

            return await base.SaveChangesAsync(cancellationToken);
        }
        public new int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _claimExternalService.GetUserId();
                        entry.Entity.CreatedOn = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _claimExternalService.GetUserId();
                        entry.Entity.UpdatedOn = DateTime.Now;
                        break;
                }

            return base.SaveChanges();
        }

    }
}
