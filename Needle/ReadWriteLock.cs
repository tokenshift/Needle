using System;

namespace Needle {
    /// <summary>
    /// Reader-writer lock using only System.Threading.Monitor.
    /// </summary>
    internal class ReadWriteLock {
        private readonly Mutex _readLock = new Mutex();
        private readonly Mutex _writeLock = new Mutex();

        private int _readers;
        private bool _writing;

        /// <summary>
        /// Enters the lock for reading.
        /// </summary>
        /// <remarks>
        /// Spin-waits on any writers.
        /// </remarks>
        public IDisposable Read() {
            while (true) {
                using (_readLock.Lock()) {
                    if (!_writing) {
                        ++_readers;
                        return new ReadLock(this);
                    }
                }
            }
        }

        /// <summary>
        /// Enters the lock for writing.
        /// </summary>
        /// <remarks>
        /// Spin-waits on any other readers or writers.
        /// </remarks>
        public IDisposable Write() {
            _writeLock.Enter();

            using (_readLock.Lock()) {
                _writing = true;
            }

            while (true) {
                using (_readLock.Lock()) {
                    if (_readers == 0) {
                        break;
                    }
                }
            }

            return new WriteLock(this);
        }

        #region Locks

        private class ReadLock : IDisposable {
            private readonly ReadWriteLock _owner;

            public void Dispose() {
                using(_owner._readLock.Lock()) {
                    --_owner._readers;
                }
            }

            public ReadLock(ReadWriteLock owner) {
                _owner = owner;
            }
        }

        private class WriteLock : IDisposable {
            private readonly ReadWriteLock _owner;

            public void Dispose() {
                using (_owner._readLock.Lock()) {
                    _owner._writing = false;
                }
                _owner._writeLock.Exit();
            }

            public WriteLock(ReadWriteLock owner) {
                _owner = owner;
            }
        }

        #endregion
    }
}