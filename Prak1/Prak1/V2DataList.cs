using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace Prak1
{
    class V2DataList: V2Data, IEnumerable<DataItem>
    {
        public List<DataItem> ListData { get; }
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
                        float TMP = Vector2.Distance(ListData[i].Pos, ListData[j].Pos);
                        float tmp = (float)Math.Sqrt((float)(Math.Pow(ListData[i].Pos.X - ListData[j].Pos.X, 2) +
                                    Math.Pow(ListData[i].Pos.Y - ListData[j].Pos.Y, 2)));
                        tmp = TMP;
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
            return $"(type - V2DataList) - from base class (V2Data): {base.ToString()}." +
                   $" From this class: length of ListData =  {ListData.Count}\n";
        }
        public override string ToLongString(string format, bool tabulation = false)
        {
            string st = tabulation == false ? this.ToString() : "    " + this.ToString();
            for (int i = 0; i < ListData.Count; i++)
            {
                if (tabulation)
                    st += "\t";
                st += $" i = {i} : " + ListData[i].ToLongString(format);
            }
            return st + "\n";
        }

        public bool SaveBinary(string filename) 
        {
            FileStream stream = null;
            BinaryWriter bw = null;
            try
            {
                stream = new FileStream(filename, FileMode.Create);
                bw = new BinaryWriter(stream);
                {
                    bw.Write(Ident); bw.Write(Date.ToString());
                    bw.Write(Count);                              //имеет тип "int32", в отличие от всех остальных записанных значений (конечно, и тут тоже можно было сделать bw.Write(Count.ToString()))
                    foreach (var item in ListData)
                    {
                        bw.Write(item.Pos.X.ToString() + " " + item.Pos.Y.ToString());
                        bw.Write(item.Val.Real.ToString() + " " + item.Val.Imaginary.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveBinary\n{ex.Message}");
                return false;
            }
            finally
            {
                if (stream != null & bw != null)
                {
                    stream.Close();
                    bw.Dispose();
                }
            }
            return true;
        }
        public static bool LoadBinary(string filename, ref V2DataList v2)
        {

            FileStream stream = null;
            BinaryReader br = null;
            try
            {
                stream = new FileStream(filename, FileMode.Open);
                br = new BinaryReader(stream);
                {
                    char[] separator = { '.', ' ', ':' };        //for Parse
                    v2.Ident = br.ReadString();

                    string[] st = br.ReadString().Split(separator);
                    v2.Date = new DateTime(int.Parse(st[2]), int.Parse(st[1]), int.Parse(st[0]), int.Parse(st[3]), int.Parse(st[4]), int.Parse(st[5]));

                    int count = br.ReadInt32(); 

                    for (int i = 0; i < count; i++)
                    {
                        st = br.ReadString().Split(separator);
                        string[] st1 = br.ReadString().Split(separator);
                        v2.Add(new DataItem(new Vector2(float.Parse(st[0]), float.Parse(st[1])), new Complex(float.Parse(st1[0]), float.Parse(st1[1]))));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadBinary\n{ex.Message}");
                return false;
            }
            finally
            {
                if (stream != null & br != null)
                {
                    stream.Close();
                    br.Dispose();
                }
            }
            return true;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < ListData.Count; i++)
                yield return ListData[i];
        }
    }
}
