using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prak1
{
    class V2MainCollection
    {
        private List<V2Data> Collection = new List<V2Data> ();
        public int Count
        {
            get
            {
                return Collection.Count;
            }
        }
        public V2Data this[int index]
        {
            get
            {
                return Collection[index];
            }
        }
        public bool Contains (string ID)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Collection[i].Ident == ID)
                    return true;
            }
            return false;
        }
        public bool Add (V2Data v2Data)
        {
            if (Contains(v2Data.Ident))
                return false;
            Collection.Add(v2Data);
            return true;
        }
        public string ToLongString (string format)
        {
            string st = "";
            foreach (V2Data i in Collection)
            {
                st += i.ToLongString(format);
            }
            return st;
        }
        public override string ToString()
        {
            string st = "";
            foreach (V2Data i in Collection)
            {
                st += i.ToString();
            }
            return st;
        }
    }
}
