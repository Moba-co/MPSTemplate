using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moba.Data.EF.Context;
using Moba.Data.EF.Interfaces.Repositories;
using Moba.Data.EF.Interfaces.UOW;
using Moba.Data.EF.Services.Repositories;
using Moba.Domain.Entities.Security;
using Moba.Domain.Entities.Setting;
// ReSharper disable InconsistentNaming

namespace Moba.Data.EF.Services.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        #region ctor
        private readonly IMapper _mapper;
        private readonly MobaDbContext _context;
        public UnitOfWork(MobaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region AuthEntity
        private IGenericRepository<User> _userRepository { get; set; }
        public IGenericRepository<User> UserRepository => _userRepository ??= new GenericRepository<User>(_context, _mapper);
         private IGenericRepository<Role> _roleRepository { get; set; }
         public IGenericRepository<Role> RoleRepository => _roleRepository ??= new GenericRepository<Role>(_context , _mapper);
        private IGenericRepository<UserRole> _userRoleRepository { get; set; }
        public IGenericRepository<UserRole> UserRoleRepository => _userRoleRepository ??= new GenericRepository<UserRole>(_context, _mapper);
        private IGenericRepository<UserClaim> _userClaimRepository { get; set; }
        public IGenericRepository<UserClaim> UserClaimRepository => _userClaimRepository ??= new GenericRepository<UserClaim>(_context, _mapper);
        private IGenericRepository<RoleClaim> _roleClaimRepository { get; set; }
        public IGenericRepository<RoleClaim> RoleClaimRepository => _roleClaimRepository ??= new GenericRepository<RoleClaim>(_context, _mapper);

        private IGenericRepository<UserLogin> _userLoginRepository { get; set; }
        public IGenericRepository<UserLogin> UserLoginRepository => _userLoginRepository ??= new GenericRepository<UserLogin>(_context, _mapper);
        private IGenericRepository<UserToken> _userTokenRepository { get; set; }
        public IGenericRepository<UserToken> UserTokenRepository => _userTokenRepository ??= new GenericRepository<UserToken>(_context, _mapper);

        #endregion

        #region SettingEntity
        private IGenericRepository<SettingRoleManager> _settingRoleManagerRepository { get; set; }
        public IGenericRepository<SettingRoleManager> SettingRoleManagerRepository => _settingRoleManagerRepository ??= new GenericRepository<SettingRoleManager>(_context, _mapper);
        #endregion
       
        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _context.Dispose();
            }

            _disposedValue = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #region  SaveChange
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
                DetachAll();
            }
            catch
            {
                DetachAll();
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                DetachAll();
            }
            catch (Exception)
            {
                DetachAll();
                throw;
            }
        }
        private void DetachAll()
        {
            var entityEntries = _context.ChangeTracker.Entries().ToArray();

            foreach (var entityEntry in entityEntries)
            {
                entityEntry.State = EntityState.Detached;
            }
        }
        #endregion
    }
}
