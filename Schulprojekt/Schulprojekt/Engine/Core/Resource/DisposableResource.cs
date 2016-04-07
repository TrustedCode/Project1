using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Core.Resource
{
    public abstract class DisposableResource : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                DisposeManaged();
            }
            DisposeUnmanaged();

            _disposed = true;
        }

        protected abstract void DisposeManaged();
        protected abstract void DisposeUnmanaged();

        ~DisposableResource()
        {
            Dispose(false);
        }
    }
}
