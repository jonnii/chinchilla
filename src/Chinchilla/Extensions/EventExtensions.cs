using System;

namespace Chinchilla.Extensions
{
    public static class EventExtensions
    {
        public static void Raise(this EventHandler<EventArgs> handler, object sender)
        {
            var copy = handler;
            if (copy != null)
            {
                copy(sender, EventArgs.Empty);
            }
        }
    }
}
