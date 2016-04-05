using System;
using System.Collections.Generic;
using System.Linq;

namespace Chinchilla
{
    public abstract class TrackableFactory<TTrackable> : IDisposable
        where TTrackable : Trackable
    {
        private readonly object trackLock = new object();

        private readonly List<TTrackable> trackables = new List<TTrackable>();

        public IEnumerable<TTrackable> Tracked
        {
            get
            {
                lock (trackLock)
                {
                    return trackables.ToArray();
                }
            }
        }

        public bool IsTracking(TTrackable trackable)
        {
            lock (trackLock)
            {
                return trackables.Contains(trackable);
            }
        }

        protected void Track(TTrackable trackable)
        {
            lock (trackLock)
            {
                trackables.Add(trackable);
            }

            trackable.Disposed += Untrack;
        }

        private void Untrack(TTrackable trackable)
        {
            lock (trackLock)
            {
                trackables.Remove(trackable);
            }
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