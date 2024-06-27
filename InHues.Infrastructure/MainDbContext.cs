using InHues.Application.Common.Interfaces;
using InHues.Domain.BaseModels;
using InHues.Domain.Entities;
using InHues.Domain.Persistence;
using InHues.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


//Scaffold Script
//Scaffold-DbContext "Server=localhost;Database=InHues;Trusted_Connection=true;" Microsoft.EntityFrameworkCore.SqlServer -Tables "Color" -OutputDir Scaffold

namespace InHues.Infrastructure
{
    public class MainDbContext: IdentityDbContext<ApplicationUser>, IMainContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public MainDbContext(
            DbContextOptions<MainDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;

            ChangeTracker.LazyLoadingEnabled = false;
        }

        #region Entities
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<CustomerFile> CustomerFiles { get; set; }
        #endregion

        public DatabaseFacade GetDatabase => Database;
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedBy = _currentUserService.UserId;
                            entry.Entity.CreatedOn = _dateTime.Now;
                            entry.Entity.IsDeleted = false;
                            if (string.IsNullOrEmpty(entry.Entity.CustomerId)) { 
                                entry.Entity.CustomerId = _currentUserService.UserId;
                            }
                            break;

                        case EntityState.Modified:
                            if (entry.Entity.IsDeleted)
                            {
                                entry.Entity.DeletedBy = _currentUserService.UserId;
                                entry.Entity.DeletedOn = _dateTime.Now;
                                break;
                            }
                            entry.Entity.LastModifiedBy = _currentUserService.UserId;
                            entry.Entity.LastModifiedOn = _dateTime.Now;

                            //skip modification for created columns
                            entry.Property(x => x.CreatedBy).IsModified = false;
                            entry.Property(x => x.CreatedOn).IsModified = false;
                            break;

                    }
                }

                var result = await base.SaveChangesAsync(cancellationToken);

                return result;
            }catch(Exception e) {
                var error = e.Message;
                return 0;
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            //Filter to show only data which aren't soft deleted
            //var entityTypes = builder.Model.GetEntityTypes();
            //foreach (var entityType in entityTypes)
            //{
            //    // Check if the entity has the IsDeleted property
            //    var isDeletedProperty = entityType.FindProperty("IsDeleted");
            //    if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
            //    {
            //        // Apply the global query filter to entities with IsDeleted = false
            //        var parameter = Expression.Parameter(entityType.ClrType);
            //        var body = Expression.Equal(
            //            Expression.Property(parameter, isDeletedProperty.PropertyInfo),
            //            Expression.Constant(false, typeof(bool)));

            //        var lambda = Expression.Lambda(body, parameter);
            //        builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            //    }
            //}

        }
    }
}
