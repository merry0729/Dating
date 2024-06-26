﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TemplateTable
{
    public class TemplateTable<TKey, TValue> : ITemplateTable<TKey>, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : IComparable
        where TValue : class, new()
    {
        public class ValueData
        {
            public TValue Value;
            public Func<TKey, TValue> LazyLoader;
        }

        private ConcurrentDictionary<TKey, ValueData> _table =
            new ConcurrentDictionary<TKey, ValueData>();

        private ConcurrentDictionary<TKey, TValue> _ghostTable =
            new ConcurrentDictionary<TKey, TValue>();

        public Func<TKey, TValue> GhostValueFactory = null;

        public Type KeyType => typeof(TKey);

        public Type ValueType => typeof(TValue);

        public int Count => _table.Count;

        public TValue TryGet(TKey id)
        {
            ValueData data;
            if (_table.TryGetValue(id, out data) == false)
                return default(TValue);

            return data.Value ?? LoadLazyValue(id, data);
        }

        public Func<TValue> TryGetFunc(TKey id)
        {
            ValueData data;
            if (_table.TryGetValue(id, out data) == false)
                return null;

            if (data.Value != null)
            {
                var value = data.Value;
                return () => value;
            }
            else
            {
                var lazyLoader = data.LazyLoader;
                return () => lazyLoader(id);
            }
        }

        private TValue LoadLazyValue(TKey id, ValueData data)
        {
            var lazyLoader = data.LazyLoader;
            if (lazyLoader != null)
            {
                data.Value = lazyLoader(id);
                data.LazyLoader = null;
            }
            return data.Value;
        }

        public object TryGetValue(TKey id)
        {
            return TryGet(id);
        }

        public bool ContainsKey(TKey id)
        {
            return _table.ContainsKey(id);
        }

        public TValue this[TKey id]
        {
            get
            {
                var value = TryGet(id);
                if (value != null)
                    return value;

                if (GhostValueFactory == null)
                    throw new KeyNotFoundException("Key: " + id);

                return _ghostTable.GetOrAdd(id, GhostValueFactory);
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (var i in _table)
                {
                    yield return i.Key;
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var i in _table)
                {
                    yield return i.Value.Value ?? LoadLazyValue(i.Key, i.Value);
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var i in _table)
            {
                yield return new KeyValuePair<TKey, TValue>(
                    i.Key,
                    i.Value.Value ?? LoadLazyValue(i.Key, i.Value));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var i in _table)
            {
                yield return i;
            }
        }

        public void Load(ITemplateTableLoader<TKey, TValue> loader)
        {
            var table = new ConcurrentDictionary<TKey, ValueData>();

            foreach (var i in loader.Load())
            {
                bool added;

                if (i.Value.Value != null)
                    added = table.TryAdd(i.Key, new ValueData { Value = i.Value.Value });
                else if (i.Value.LazyLoader != null)
                    added = table.TryAdd(i.Key, new ValueData { LazyLoader = i.Value.LazyLoader });
                else
                    throw new InvalidOperationException("Empty:" + i.Key + " ValueType : " + this.ValueType);

                if (added == false)
                    throw new InvalidOperationException("Duplicate:" + i.Key + " ValueType : " + this.ValueType);
            }

            _table = table;
            _ghostTable = new ConcurrentDictionary<TKey, TValue>();
        }

        public void Update(ITemplateTableLoader<TKey, TValue> loader)
        {
            foreach (var i in loader.Load())
            {
                if (i.Value.Value != null)
                {
                    _table[i.Key] = new ValueData { Value = i.Value.Value };
                }
                else if (i.Value.LazyLoader != null)
                {
                    _table[i.Key] = new ValueData { LazyLoader = i.Value.LazyLoader };
                }
                else
                {
                    ValueData data;
                    _table.TryRemove(i.Key, out data);
                }
            }
        }

        public void Release()
		{
            if (null != _table)
                _table.Clear();

            if (null != _ghostTable)
                _ghostTable.Clear();

            _table = null;
            _ghostTable = null;
		}
    }
}
