using System;

namespace Needle {
    /// <summary>
    /// Simple mutex implemented using only System.Threading.Monitor.
    /// </summary>
    internal class Mutex {
        private bool _busy;

        /// <summary>
        /// Enters the mutex.
        /// </summary>
        /// <remarks>Spin-waits on the mutex.</remarks>
        public void Enter() {
            while (true) {
                lock (this) {
                    if (!_busy) {
                        _busy = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Leaves the mutex.
        /// </summary>
        public void Exit() {
            lock (this) {
                _busy = false;
            }
        }

        /// <summary>
        /// Enters the mutex.
        /// </summary>
        /// <returns>An object that can be disposed to exit the mutex.</returns>
        /// <remarks>Spin-waits on the mutex.</remarks>
        public IDisposable Lock() {
            Enter();
            return new Status(this);
        }

        private class Status : IDisposable {
            private readonly Mutex _owner;

            public void Dispose() {
                _owner.Exit();
            }

            public Status(Mutex owner) {
                _owner = owner;
            }
        }
    }
}