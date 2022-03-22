namespace Js.Snippets.CSharp.Collections.Concurrent
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Represents a thread-safe set of values.
    /// </summary>
    /// <typeparam name="T">The type of items in the set.</typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class ConcurrentHashSet<T> : IReadOnlyCollection<T>
    {
        private readonly ConcurrentDictionary<T, T> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentHashSet{T}"/> class.
        /// </summary>
        public ConcurrentHashSet()
            : this(EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentHashSet{T}"/> class
        /// with the specified <see cref="IEqualityComparer{T}"/> to determine equality of
        /// items in the collection.
        /// </summary>
        /// <param name="comparer">The equality comparison to use when comparing items.</param>
        public ConcurrentHashSet(IEqualityComparer<T> comparer)
        {
            items = new ConcurrentDictionary<T, T>(comparer);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ConcurrentHashSet{T}" /> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int Count => items.Count;

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Attempts to add the specified item to the <see cref="ConcurrentHashSet{T}"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>True, if the item was successfully added; false, if the item already exists in the  <see cref="ConcurrentHashSet{T}"/>.</returns>
        public bool Add(T item)
        {
            return items.TryAdd(item, item);
        }

        /// <summary>
        /// Removes all items from the <see cref="ConcurrentHashSet{T}" />.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="ConcurrentHashSet{T}" /> contains the specified item.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True, if the <see cref="ConcurrentHashSet{T}" /> contains the specified item; false, otherwise.</returns>
        public bool Contains(T item)
        {
            return items.ContainsKey(item);
        }

        /// <summary>
        /// Attempts to remove the specified item from the <see cref="ConcurrentHashSet{T}"/>.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True, if the item was removed successfully; false, otherwise.</returns>
        public bool Remove(T item)
        {
            return items.TryRemove(item, out _);
        }
    }
}

