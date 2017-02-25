using System;
using System.Collections.Generic;

namespace DiaShowWpf
{
    public class ListyList<T> : List<T>
    {
        private int _index;

        public ListyList()
        {
        }

        public ListyList(IEnumerable<T> list)
        {
            AddRange(list);
        }

        // TODO: FIX "return this[_index];" & "return this[index];", when list empty

        public T Previous()
        {
            if (_index - 1 > -1)
            {
                _index = _index - 1;
            }
            else
            {
                _index = Count - 1;
            }
            return this[_index];
        }

        public T Next()
        {
            if (_index + 1 < Count)
            {
                _index = _index + 1;
            }
            else
            {
                _index = 0;
            }
            return this[_index];
        }

        public T Next(int steps)
        {
            int index;
            if (steps < 0)
            {
                if (_index + steps > -1)
                {
                    index = _index + steps;
                }
                else
                {
                    index = Count + (steps + _index);
                }
            }
            else
            {
                if (_index + steps < Count)
                {
                    index = _index + steps;
                }
                else
                {
                    index = steps - (Count - _index);
                }
            }
            return this[index];
        }

        public void Shuffle()
        {
            _index = 0;
            var rng = new Random();
            int n = Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = this[k];
                this[k] = this[n];
                this[n] = value;
            }
        }
    }
}
