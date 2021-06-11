using Microsoft.AspNetCore.Http;
using MPS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPS.Services.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SessionService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool Change(string key, string newValue)
        {
            try
            {
                Delete(key);
                Set(key, newValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Clear()
        {
            try
            {
                _contextAccessor.HttpContext.Session.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _contextAccessor.HttpContext.Session.CommitAsync();
        }

        public bool Delete(string key)
        {
            try
            {
                _contextAccessor.HttpContext.Session.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Get(string key)
        {
            return _contextAccessor.HttpContext.Session.GetString(key);
        }

        public bool KeyExist(string key)
        {
            return _contextAccessor.HttpContext.Session.Keys.Any(a => a == key);
        }

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
            await _contextAccessor.HttpContext.Session.LoadAsync();
        }

        public bool Set(string key, string value)
        {
            try
            {
                _contextAccessor.HttpContext.Session.SetString(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
