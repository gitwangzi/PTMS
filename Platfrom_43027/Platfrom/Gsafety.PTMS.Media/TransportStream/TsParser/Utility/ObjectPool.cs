using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Utility
{
    sealed class ObjectPool<T> : IDisposable
        where T : new()
    {
        readonly Stack<T> _pool = new Stack<T>();
#if OBJECT_POOL_STATISTICS
        int _allocations;
        int _deallocations;
        int _objectsCreated;
#if DEBUG
        readonly List<T> _allocationTracker = new List<T>(); 
#endif
#endif

        public T Allocate()
        {
            lock (_pool)
            {
#if OBJECT_POOL_STATISTICS
                ++_allocations;
#endif

                if (_pool.Count > 0)
                {
                    var poolObject = _pool.Pop();

                    return poolObject;
                }

#if OBJECT_POOL_STATISTICS
                ++_objectsCreated;
#endif
            }

            var t = new T();

#if OBJECT_POOL_STATISTICS && DEBUG
            lock (_pool)
            {
                _allocationTracker.Add(t);
            }
#endif

            return t;
        }

        public void Free(T poolObject)
        {
            lock (_pool)
            {
                _pool.Push(poolObject);

#if OBJECT_POOL_STATISTICS
                ++_deallocations;
                Debug.Assert(_deallocations <= _allocations,
                    string.Format("ObjectPool.Free() more deallocations than allocations ({0} > {1})", _deallocations, _allocations));
#endif
            }
        }

        public void Dispose()
        {
            Clear();
        }

        public void Clear()
        {
            lock (_pool)
            {
#if OBJECT_POOL_STATISTICS
                Debug.Assert(_allocations == _deallocations && _pool.Count == _objectsCreated,
                    string.Format("ObjectPool.Clear(): allocations {0} == deallocations {1} && _pool.Count {2} == _objectsCreated {3}",
                        _allocations, _deallocations, _pool.Count, _objectsCreated));
#endif

                _pool.Clear();

#if OBJECT_POOL_STATISTICS
                _allocations = 0;
                _deallocations = 0;
                _objectsCreated = 0;
#endif
            }
        }
    }
}
