using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MPS.Data.EF.Interfaces.UOW;
using MPS.Domain.Entities.Security;
using MPS.Services.Interfaces.EntityServices.Security;
using System;
using System.Collections.Generic;

namespace MPS.Services.Services
{
    public class MPSUserManager : UserManager<User>
    {
        private readonly IUnitOfWork _db;
        public readonly IAuthService AuthService;
        public MPSUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer
            keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger, IUnitOfWork db, IAuthService authService)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _db = db;
            AuthService = authService;
        }
    }
}
