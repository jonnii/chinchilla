using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Chinchilla
{
    public abstract class TrackableFactory<TTrackable> : IDisposable
        where TTrackable : Trackable
    {
        private readonly ConcurrentDictionary<int, TTrackable> trackables = new ConcurrentDictionary<int, TTrackable>();

        public IEnumerable<TTrackable> Tracked
        {
            get { return trackables.Values; }
        }

        public bool IsTracking(TTrackable trackable)
        {
            return trackables.ContainsKey(trackable.GetHashCode());
        }

        protected void Track(TTrackable trackable)
        {
            trackable.Disposed += Untrack;
            trackables.TryAdd(trackable.GetHashCode(), trackable);
        }

        private void Untrack(TTrackable trackable)
        {
            trackables.TryRemove(trackable.GetHashCode(), out trackable);
        }

        private void Untrack(object sender, EventArgs eventArgs)
        {
            var trackable = (TTrackable)sender;

            trackable.Disposed -= Untrack;
            Untrack(trackable);
        }

        public virtual void Dispose()
        {
            var activeTrackables = Tracked.ToArray();

            foreach (var trackable in activeTrackables)
            {
                trackable.Dispose();
            }
        }
    }
}