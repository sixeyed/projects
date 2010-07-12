using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.Unity;

namespace Sixeyed.EC2Launcher
{
    /// <summary>
    /// Lifetime manager which uses an object for the life of the current call context
    /// </summary>
    /// <remarks>
    /// Typically used for objects which are to be created and used within the scope
    /// of a web request
    /// </remarks>
    public class PerCallContextLifeTimeManager : LifetimeManager
    {
        private string _key = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the object from <see cref="CallContext"/>
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return CallContext.GetData(_key);
        }

        /// <summary>
        /// Sets the object in <see cref="CallContext"/>
        /// </summary>
        /// <param name="newValue"></param>
        public override void SetValue(object newValue)
        {
            CallContext.SetData(_key, newValue);
        }

        /// <summary>
        /// Removes the object from <see cref="CallContext"/>
        /// </summary>
        public override void RemoveValue()
        {
            CallContext.FreeNamedDataSlot(_key);
        }
    }
}
