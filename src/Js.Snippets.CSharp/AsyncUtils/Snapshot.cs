namespace Js.Snippets.CSharp.AsyncUtils
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    /// <summary>
    /// Holds a snapshot, described by a query.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class Snapshot<T> : INotifyPropertyChanged
    {
        private readonly Func<Task<T>> query;
        private T value;
        private DateTime lastRefreshed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Snapshot{T}"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public Snapshot(Func<Task<T>> query)
            : this(query, default(T))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Snapshot{T}"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="initialValue">The initial value.</param>
        public Snapshot(Func<Task<T>> query, T initialValue)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            this.query = query;
            this.value = initialValue;
            this.lastRefreshed = DateTime.MinValue;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the cached value.
        /// </summary>
        public T Value
        {
            get { return this.value; }
            private set { SetField(ref this.value, value); }
        }

        /// <summary>
        /// Gets the timestamp indicating when the snapshot was last refreshed.
        /// </summary>
        public DateTime LastRefreshed
        {
            get { return this.lastRefreshed; }
            private set { SetField(ref this.lastRefreshed, value); }
        }

        /// <summary>
        /// Refreshes this snapshot instance asynchronously.
        /// </summary>
        /// <returns>A task for the snapshot refresh operation.</returns>
        public async Task Refresh()
        {
            Value = await this.query();
            LastRefreshed = DateTime.UtcNow;
        }

        /// <summary>
        /// Called when a property value is changed to notify registered event handlers.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field to the given value and notifies registered event handlers in case of a changed value.
        /// </summary>
        /// <typeparam name="S">The field type.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True, if the value of the field has changed.</returns>
        protected bool SetField<S>(ref S field, S value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<S>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
