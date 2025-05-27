using System;
using System.Collections.Generic;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private readonly List<T> items = new();
        private readonly object locker = new();

        public T this[int index]
        {
            get
            {
                lock (locker)
                {
                    if (index < 0 || index >= items.Count)
                        return null;
                    return items[index];
                }
            }
            set
            {
                lock (locker)
                {
                    if (index < 0) return;

                    if (index < items.Count)
                    {
                        // Заменяем элемент и удаляем все элементы после
                        items[index] = value;
                        items.RemoveRange(index + 1, items.Count - (index + 1));
                    }
                    else if (index == items.Count)
                    {
                        // Просто добавляем (Append)
                        items.Add(value);
                    }
                }
            }
        }

        public T LastItem()
        {
            lock (locker)
            {
                if (items.Count == 0)
                    return null;
                return items[^1]; // Последний элемент
            }
        }

        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (locker)
            {
                var last = LastItem();
                if (last == knownLastItem)
                    items.Add(item);
            }
        }

        public int Count
        {
            get
            {
                lock (locker)
                {
                    return items.Count;
                }
            }
        }
    }
}
