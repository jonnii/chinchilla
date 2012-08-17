using System;
using System.Collections.Generic;
using System.Linq;

namespace Chinchilla
{
    public abstract class TrackableFactory<TTrackable> : IDisposable
        where TTrackable : Trackable
    {
        private readonly List<TTrackable> trackables = new List<TTrackable>();

        public IEnumerable<TTrackable> Tracked
        {
            get { return trackables; }
        }

        public bool IsTracking(TTrackable trackable)
        {
            return trackables.Contains(trackable);
        }

        protected void Track(TTrackable trackable)
        {
            trackable.Disposed += Untrack;
            trackables.Add(trackable);
        }

        private void Untrack(TTrackable trackable)
        {
            trackables.Remove(trackable);
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