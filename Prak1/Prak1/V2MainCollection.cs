using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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
        public DataItem? MaxModule 
        {
            get
            {
                var query1 =
                from Item in Collection
                where Item.Count != 0
                select Item.Max(x => x.Val.Magnitude);

                if (query1.Count() == 0)
                    return null;

                var query_Data_Items =
                from Item in Collection
                from dataItem in Item
                select dataItem;

                return query_Data_Items.FirstOrDefault(x => x.Val.Magnitude == query1.Max());
            }
        }
        public IEnumerable<Vector2> All_Points
        { 
            get
            {
                if (Collection.Count == 0)
                    return null;
                var query_Points_In_V2_Array =
                    from Item in Collection
                    where Item is V2DataArray
                    from dataItem in Item
                    select dataItem.Pos;

                var query = 
                    from Item in Collection
                    where Item is V2DataList
                    from dataItem in Item
                    where !query_Points_In_V2_Array.Any(x => x == dataItem.Pos)
                    select dataItem.Pos;
                return query.Count() == 0 ? null : query;
            }
        }
        public IEnumerable<V2Data> Mean
        {
            get
            {
                if (Collection.Count == 0)
                    return null;
                var query2 =
                    from Item in Collection
                    where Item is V2DataList & Item.Count != 0
                    orderby Item.Average(x => x.Val.Magnitude) descending
                    select Item;

                return (IEnumerable<V2Data>) query2;
            }
        }
        public IEnumerable <IGrouping<int, V2Data>> Group_Elements
        {
            get => Collection.Count == 0 ? null: Collection.GroupBy(x => x.Count);
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
