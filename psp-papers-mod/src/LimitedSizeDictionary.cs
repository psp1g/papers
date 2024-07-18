using System;
using System.Collections.Generic;

namespace psp_papers_mod;

public class LimitedSizeDictionary<TKey, TValue>(int maxSize) {
    private readonly LinkedList<TKey> order = [];
    private readonly Dictionary<TKey, TValue> dictionary = new();

    public void Add(TKey key, TValue value) {
        if (this.dictionary.ContainsKey(key)) {
            this.order.Remove(key);
        } else if (this.dictionary.Count >= maxSize) {
            TKey oldestKey = this.order.First!.Value;
            this.order.RemoveFirst();
            this.dictionary.Remove(oldestKey);
        }

        this.order.AddLast(key);
        this.dictionary[key] = value;
    }

    public bool TryGetValue(TKey key, out TValue value) {
        return this.dictionary.TryGetValue(key, out value);
    }

    public int Count => this.dictionary.Count;

    public void Clear() {
        this.order.Clear();
        this.dictionary.Clear();
    }
}