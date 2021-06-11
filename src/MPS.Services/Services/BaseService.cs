using System;
using AutoMapper;
using Moba.Data.EF.Interfaces.UOW;

namespace Moba.Services.Services
{
    public class BaseService : IDisposable
    {
        
        internal readonly IUnitOfWork _db;
        internal readonly IMapper _mapper;
        
        public BaseService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
        }
        
        #region dispose

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseService()
        {
            Dispose(false);
        }

        #endregion
    }
}