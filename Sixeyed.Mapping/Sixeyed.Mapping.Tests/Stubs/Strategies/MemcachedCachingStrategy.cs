using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.Tests.Stubs.Strategies
{
    public class MemcachedCachingStrategy : ICachingStrategy
    {
        #region ICachingStrategy Members

        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
