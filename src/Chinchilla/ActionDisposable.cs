using System;

namespace Chinchilla
{
    public class ActionDisposable : IDisposable
    {
        private readonly Action action;

        public ActionDisposable(Action action)
        {
            this.action = action;
        }

        public void Dispose()
        {
            action();
        }
    }
}