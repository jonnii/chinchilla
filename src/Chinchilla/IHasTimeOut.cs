using System;

namespace Chinchilla
{
    /// <summary>
    /// Indicates that this message supports a timeout
    /// </summary>
    public interface IHasTimeOut
    {
        /// <summary>
        /// A timespan which indicates how long this message should remain unacked for
        /// until it times out.
        /// </summary>
        TimeSpan Timeout { get; }
    }
}
