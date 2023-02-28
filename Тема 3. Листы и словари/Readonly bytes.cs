using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;
using static hashes.ReadonlyBytesTests;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] values;
        private int hash;
        public int Length => values.Length;

        public override int GetHashCode()
        {
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(ReadonlyBytes))
                return false;
            return Equals((ReadonlyBytes)obj);
        }

        private bool Equals(ReadonlyBytes obj)
        {
            if (obj.Length != Length)
                return false;
            for (var i = 0; i < Length; i++)
            {
                if (obj[i] != values[i])
                     return false;
            }

            return true;
        }

        public ReadonlyBytes(params byte[] values)
        {
            if (values == null)
                throw new ArgumentNullException();
            this.values = values;
            FNVHash();
        }

        private void FNVHash()
        {
            const int fnvPrime = 4999;
            foreach (var value in values)
            {
                unchecked
                {
                    hash *= fnvPrime;
                    hash ^= value;
                }
            }
        }

        public IEnumerator<byte> GetEnumerator() 
        {
            return ((IEnumerable<byte>)values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public byte this[int index] 
        {
            get
            {
                if (index >= values.Length)
                    throw new IndexOutOfRangeException();
                return values[index];
            }
        }

        public override string ToString() => "[" + string.Join(", ", values) + "]";
    }
}