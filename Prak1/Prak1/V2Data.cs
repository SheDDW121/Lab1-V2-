using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prak1
{
    abstract class V2Data : IEnumerable <DataItem>
    {
        public string Ident { get; protected set; }
        public DateTime Date { get; protected set; }
        public V2Data (string Ident, DateTime Date)
        {
            this.Ident = Ident;
            this.Date = Date;
        }
        public abstract int Count { get; }
        public abstract float MinDistance { get; }
        public abstract string ToLongString(string format, bool tabulation = false);  //добавил параметр по умолчанию для более "красивого" вывода (нужно, в основном, для 3-го LINQ-свойства)
        public override string ToString()
        {
            return $"Date = {Date}, Ident = \"{Ident}\"";
        }

        public abstract IEnumerator<DataItem> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
