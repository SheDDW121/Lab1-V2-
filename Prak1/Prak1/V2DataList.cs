﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Prak1
{
    class V2DataList: V2Data
    {
        List<DataItem> ListData { get; }
        public V2DataList(string st, DateTime DT): base(st, DT)
        {
            ListData = new List<DataItem>(100);
        }
        public bool Add(DataItem newitem)
        {
            for (int i = 0; i < ListData.Count; i++)
            {
                if (newitem.Pos == ListData[i].Pos)
                {
                    return false;
                }
            }
            ListData.Add(newitem);
            return true;
        }
        public int AddDefaults (int nitems, Fv2Complex F)
        {
            int Count = 0;
            Random rnd = new Random();
            for (int i = 0; i < nitems; i++)
            {
                float genX = (float)rnd.NextDouble() * 100;
                float genY = (float)rnd.NextDouble() * 100;
                Vector2 v2 = new Vector2(genX, genY);
                Complex C = F(v2);
                DataItem D_I = new DataItem(v2, C);
                if(Add(D_I) == true)
                {
                    Count++;
                }
            }
            return Count;
        }
        public override int Count { get { return ListData.Count; } }
        public override float MinDistance { get
            {
                float MinDist = float.MaxValue;
                for (int i = 0; i < ListData.Count; i++)
                {
                    for (int j = i + 1; j < ListData.Count; j++)
                    {
                        if (ListData[i].Pos == ListData[j].Pos)
                        {
                            continue;
                        }
                        float tmp = (float)Math.Sqrt((float)(Math.Pow(ListData[i].Pos.X - ListData[j].Pos.X, 2) +
                                    Math.Pow(ListData[i].Pos.Y - ListData[j].Pos.Y, 2)));
                        if (tmp < MinDist)
                        {
                            MinDist = tmp;
                        }
                    }
                }
                return MinDist == float.MaxValue ? 0f : MinDist;
            } 
        }
        public override string ToString()
        {
            return $"\n(type - V2DataList) - from base class (V2Data): {base.ToString()}. " +
                   $"From this class: length of ListData =  {ListData.Count}\n";
        }
        public override string ToLongString(string format)
        {
            string st = this.ToString();
            for (int i = 0; i < ListData.Count; i++)
            {
                st += $" i = {i} : " + ListData[i].ToLongString(format);
            }
            return st;
        }

    }
}
