using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Animation_Engine.Engine.Core.Resource.Cache
{
    public sealed class ThreadCache
    {
        public const int SIZE_ADDER = 1000;
        private int _index = 0;
        private int _caches = 0;

        private readonly Dictionary<Int32, Int32> _threadAssignment = new Dictionary<int, int>(); 

        private CacheObject[] _data = new CacheObject[SIZE_ADDER];

        public ThreadCache(int caches)
        {
            _caches = caches;
        }

        public int LinkThread()
        {
            if (_threadAssignment.ContainsKey(Thread.CurrentThread.ManagedThreadId))
            {
                return -1;
            }
            for (int i = 0; i < _caches; i++)
            {
                if (!_threadAssignment.ContainsValue(i))
                {
                    _threadAssignment.Add(Thread.CurrentThread.ManagedThreadId, i);
                    return i;
                }   
            }
            throw new IndexOutOfRangeException("Cannot link more Threads than " + _caches +
                                               ". Use UnlinkThread() to free a Thread!");
        }

        public bool UnlinkThread()
        {
            return _threadAssignment.Remove(Thread.CurrentThread.ManagedThreadId);
        }
        
        public CacheObject Alloc()
        {
            if (!(_data.Length > _index))
            {
                CacheObject[] tmp = new CacheObject[_data.Length * 2];
                Array.Copy(_data, tmp, _data.Length);
                _data = tmp;
            }
            CacheObject obj = new CacheObject();
            _data[_index] = obj;
            obj.Alloc(_index, _caches);
            _index++;
            return obj;
        }
        
        public void Clean()
        {
            int index = 0;
            CacheObject[] _tmp = new CacheObject[_data.Length]; // Store new array
            bool[] _flagL = new bool[_data.Length]; // Find redundant pointers
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[i] != null)
                {
                    if (_data[i].Ptr > -1 && !_flagL[_data[i].Ptr])
                    {
                        _tmp[index] = _data[i];
                        _data[i].Ptr = index;
                        _flagL[index] = true;
                        index++;
                    }
                }
            }
            _data = _tmp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Object GetValue(int ptr)
        {
            return _data[ptr].Objects[_threadAssignment[Thread.CurrentThread.ManagedThreadId]];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Object GetValue(int ptr, int cache)
        {
            return _data[ptr].Objects[cache];
        }

        public void CopyCacheToAllOthers(int cache)
        {
            Parallel.ForEach(_data, (range, loopState) =>
            {
                ICopyable c = range.Objects[cache];
                for (int i = 0; i < _caches; i++)
                {
                    if (i != cache)
                    {
                        range.Objects[i].GetCopy(c);
                    }
                }
            });
        }

        public void CopyCacheData(int srcCache, int dstCache)
        {
            Parallel.ForEach(_data, (range, loopState) =>
            {
                range.Objects[dstCache].GetCopy(range.Objects[srcCache]);
            });
        }

        public string GetDebugString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Linked Threads:");
            foreach (KeyValuePair<int, int> kvp in _threadAssignment)
            {
                builder.Append(String.Format("\n\t[Thread: {0}; Cache: {1}]", kvp.Key, kvp.Value));
            }
            builder.Append(String.Format("\nAllocated objects: {0} of {1}", _index, _data.Length));
            builder.Append(String.Format("\nCache absulte object count[{0} pages per object]: {1}\n", _caches, _data.Length * _caches));
            return builder.ToString();
        }
    }

    public class CacheObject
    {
        public int Ptr;
        public ICopyable[] Objects;

        public void Alloc(int ptr, int caches)
        {
            Ptr = ptr;
            Objects = new ICopyable[caches];
        }   
    }


}
