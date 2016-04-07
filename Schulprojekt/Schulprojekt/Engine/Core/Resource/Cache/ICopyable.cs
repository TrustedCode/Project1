using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Core.Resource.Cache
{
    public abstract class ICopyable
    {
        public abstract void GetCopy(ICopyable copyable);
    }
}
