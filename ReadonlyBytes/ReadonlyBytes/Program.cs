using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] data;
        private int? cachedHashCode;

        // Основной конструктор
        public ReadonlyBytes(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            this.data = new byte[data.Length];
            Array.Copy(data, this.data, data.Length);
        }

        // Дополнительные конструкторы
        public ReadonlyBytes(byte b1, byte b2, byte b3)
            : this(new byte[] { b1, b2, b3 }) { }

        public ReadonlyBytes(byte b1, byte b2)
            : this(new byte[] { b1, b2 }) { }

        public ReadonlyBytes(byte b1)
            : this(new byte[] { b1 }) { }

        public ReadonlyBytes()
            : this(Array.Empty<byte>()) { }

        public ReadonlyBytes(byte b1, byte b2, byte b3, byte b4)
            : this(new byte[] { b1, b2, b3, b4 }) { }

        // Доступ к элементу по индексу
        public byte this[int index] => data[index];

        // Свойство длины
        public int Length => data.Length;

        // Реализация IEnumerable<byte>
        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var b in data)
                yield return b;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || obj.GetType() != typeof(ReadonlyBytes)) return false;

            var other = (ReadonlyBytes)obj;
            if (data.Length != other.data.Length) return false;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != other.data[i]) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            if (cachedHashCode.HasValue)
                return cachedHashCode.Value;

            unchecked
            {
                const uint fnvPrime = 16777619;
                uint hash = 2166136261; // FNV offset basis

                foreach (byte b in data)
                {
                    hash = (hash ^ b) * fnvPrime;
                }

                cachedHashCode = (int)hash;
                return cachedHashCode.Value;
            }
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", data)}]";
        }
    }
}