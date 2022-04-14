using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RideshareApp.Entities;
using System.Security.Claims;

namespace RideshareApp.DataAccess.EFCore
{
    public partial class DataContext : DbContext
    {
        public const string AnonymousUser = "anonymous";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _currentUserName = AnonymousUser;
        private int? _currentUserId = null;

        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentUserName
        {
            get
            {
                if (_currentUserName == AnonymousUser)
                {
                    var identity = _httpContextAccessor.HttpContext?.User?.Identity;
                    if (identity != null)
                    {
                        var claims = _httpContextAccessor.HttpContext?.User?.Claims;
                        if (claims == null)
                        {
                            _currentUserName = AnonymousUser;
                        }
                        else
                        {
                            var claimUsername = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                            if (claimUsername == null || string.IsNullOrEmpty(claimUsername.Value))
                            {
                                _currentUserName = AnonymousUser;
                            }
                            else
                            {
                                _currentUserName = claimUsername.Value;
                            }
                        }
                    }
                    else
                    {
                        _currentUserName = AnonymousUser;
                    }
                }

                return _currentUserName;
            }
        }

        public int? CurrentUserId
        {
            get
            {
                if (_currentUserId == null)
                {
                    var identity = _httpContextAccessor.HttpContext?.User?.Identity;
                    if (identity != null)
                    {
                        if (identity == null)
                        {
                            _currentUserId = null;
                        }
                        else
                        {
                            var claims = _httpContextAccessor.HttpContext?.User?.Claims;
                            var claimId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                            if (claimId == null || string.IsNullOrEmpty(claimId.Value))
                            {
                                _currentUserId = null;
                            }
                            else
                            {
                                _currentUserId = Convert.ToInt32(claimId.Value);
                            }
                        }
                    }
                    else
                    {
                        _currentUserId = null;
                    }
                }

                return _currentUserId;
            }
        }

        /// <summary>
        /// Override the SaveChanges method to automatically add audit information
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            this.SimpleAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.SimpleAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SimpleAudit()
        {
            // Add audit info for create statements
            foreach (var entity in this.ChangeTracker.Entries()
                     .Where(t => t.State == EntityState.Added))
            {
                entity.Property(nameof(BaseEntity.SysInsDate)).CurrentValue = DateTime.Now;
            }

            //Add audit info for update statements
            foreach (var entity in this.ChangeTracker.Entries()
                     .Where(t => t.State == EntityState.Modified))
            {
                entity.Property(nameof(BaseEntity.SysUpdDate)).CurrentValue = DateTime.Now;
            }
        }
    }
}
