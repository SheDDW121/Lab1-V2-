using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prak1
{
    abstract class V2Data
    {
        public string Ident { get; }
        public DateTime Date { get;  }
        public V2Data (string Ident, DateTime Date)
        {
            this.Ident = Ident;
            this.Date = Date;
        }
        public abstract int Count { get; }
        public abstract float MinDistance { get; }
        public abstract string ToLongString(string format);
        public override string ToString()
        {
            return $"Date = {Date}, Ident = \"{Ident}\"";
        }
    }
}
