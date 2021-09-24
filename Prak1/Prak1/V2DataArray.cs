using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prak1
{
    class V2DataArray: V2Data
    {
        public int X_Nodes { get; }
        public int Y_Nodes { get; }
        public Vector2 Step { get; }
        public Complex[,] FieldValue { get;  }
        public V2DataArray (string st, DateTime DT): base(st, DT)
        {
            FieldValue = new Complex[0,0];
            Step = new Vector2(0, 0);
            X_Nodes = Y_Nodes = 0;
        }
        public V2DataArray(string st, DateTime DT, int X, int Y, Vector2 v2, Fv2Complex F) : base(st, DT)
        {
            try
            {
                if (X < 0 || Y < 0)
                {
                    throw (new Exception("X or Y nodes less than 0"));
                }
            }
            catch (Exception)
            {
                X = 0; Y = 0;
                Console.WriteLine("X or Y nodes less than 0 and count of nodes X and Y was automatically set to [0, 0]\n");
            }
            FieldValue = new Complex[X, Y];
            Step = v2;
            X_Nodes = X; Y_Nodes = Y;
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    FieldValue[i, j] = F(new Vector2(v2.X * i, v2.Y * j));
                }
            }
        }
        public override int Count
        {
            get
            {
                return X_Nodes * Y_Nodes;
            }
        }
        public override float MinDistance
        {
            get
            {
                if (Count > 1)
                {
                    if (X_Nodes > 1 && Y_Nodes > 1)
                    {
                        return Step.X < Step.Y ? Step.X : Step.Y;
                    }
                    else if (Y_Nodes == 1)
                    {
                        return Step.X;
                    }
                    else if (X_Nodes == 1)
                    {
                        return Step.Y;
                    }
                    else return -111;
                }
                else return 0;
            }
        }
        public override string ToString()
        {
            return $"\n(type = V2DataArray), from base class (V2Data) {base.ToString()} " +
                $". From this class: the grid has the following parametres:\nX_Nodes = {X_Nodes}, " +
                $"Y_Nodes = {Y_Nodes}, Step = {Step}\n";
        }
        public override string ToLongString(string format)
        {
            string st = this.ToString();
            for (int i = 0; i < X_Nodes; i++)
            {
                for (int j = 0; j < Y_Nodes; j++)
                {
                    st += $"(i = {i}, j = {j}), coordinates: X = {(i * Step.X).ToString(format)}, " +
                        $"Y = {(j * Step.Y).ToString(format)}, FieldValue = ({FieldValue[i, j].Real.ToString(format)} + " +
                        $" {FieldValue[i, j].Imaginary.ToString(format)}i) (with + module = " + 
                        $"{(Math.Sqrt(Math.Pow(FieldValue[i, j].Real, 2) + Math.Pow(FieldValue[i, j].Imaginary, 2))).ToString(format)})\n";
                }
            }
            return st;
        }
        public static explicit operator V2DataList(V2DataArray Arr)
        {
            V2DataList NewList = new V2DataList("LIST_ReceivedFrom "+ Arr.Ident, DateTime.Now);
            for (int i = 0; i < Arr.X_Nodes; i++)
            {
                for (int j = 0; j < Arr.Y_Nodes; j++)
                {
                    DataItem DI = new DataItem(new Vector2(Arr.Step.X * i, Arr.Step.Y * j),
                        new Complex(Arr.FieldValue[i, j].Real, Arr.FieldValue[i, j].Imaginary));
                    NewList.Add(DI);
                }
            }
            return NewList;
        }
    }
}
