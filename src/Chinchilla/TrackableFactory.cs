using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Chinchilla
{
    public abstract class TrackableFactory<TTrackable> : IDisposable
        where TTrackable : Trackable
    {
        #region DisposableReaderWriterLockSlim

        private struct DisposableReaderWriterLockSlim : IDisposable
        {
            private readonly ReaderWriterLockSlim readerWriterLock;
            private bool isReadLock;

            private DisposableReaderWriterLockSlim(ReaderWriterLockSlim readerWriterLock, bool isReadLock)
            {
                this.readerWriterLock = readerWriterLock;
                this.isReadLock = isReadLock;
            }

            public static IDisposable EnterReadLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                readerWriterLockSlim.EnterReadLock();
                return new DisposableReaderWriterLockSlim(readerWriterLockSlim, true);
            }

            public static IDisposable EnterWriteLock(ReaderWriterLockSlim readerWriterLockSlim)
            {
                readerWriterLockSlim.EnterWriteLock();
                return new DisposableReaderWriterLockSlim(readerWriterLockSlim, false);
            }

            public void Dispose()
            {
                if (isReadLock)
                {
                    this.readerWriterLock.ExitReadLock();
                }
                else
                {
                    this.readerWriterLock.ExitWriteLock();
                }
            }
        }

        #endregion DisposableReaderWriterLockSlim

        private readonly ReaderWriterLockSlim padLock = new ReaderWriterLockSlim();
        private readonly List<TTrackable> trackables = new List<TTrackable>();

        public IEnumerable<TTrackable> Tracked
        {
            get
            {
                using (DisposableReaderWriterLockSlim.EnterReadLock(padLock))
                {
                    return trackables.ToArray();
                }
            }
        }

        public bool IsTracking(TTrackable trackable)
        {
            using (DisposableReaderWriterLockSlim.EnterReadLock(padLock))
            {
                return trackables.Contains(trackable);
            }
        }

        protected void Track(TTrackable trackable)
        {
            trackable.Disposed += Untrack;
            using (DisposableReaderWriterLockSlim.EnterWriteLock(padLock))
            {
                trackables.Add(trackable);
            }
        }

        private void Untrack(TTrackable trackable)
        {
            using (DisposableReaderWriterLockSlim.EnterWriteLock(padLock))
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
            using (DisposableReaderWriterLockSlim.EnterWriteLock(padLock))
            {
                var activeTrackables = Tracked.ToArray();

                foreach (var trackable in activeTrackables)
                {
                    trackable.Dispose();
                }
            }
        }
    }
}