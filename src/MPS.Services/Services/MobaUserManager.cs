using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moba.Data.EF.Interfaces.UOW;
using Moba.Domain.Entities.Security;
using Moba.Services.Interfaces.EntityServices.Security;
using System;
using System.Collections.Generic;

namespace Moba.Services.Services
{
    public class MobaUserManager : UserManager<User>
    {
        private readonly IUnitOfWork _db;
        public readonly IAuthService AuthService;
        public MobaUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer
            keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger, IUnitOfWork db, IAuthService authService)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _db = db;
            AuthService = authService;
        }
    }
}
