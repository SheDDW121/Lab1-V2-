using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Prak1
{
    class V2DataArray: V2Data, IEnumerable<DataItem>
    {
        public int X_Nodes { get; protected set; }
        public int Y_Nodes { get; protected set; }
        public Vector2 Step { get; protected set; }
        public Complex[,] FieldValue { get; protected set; }
        public Complex[,] Derivative { get; }

        [DllImport("..\\..\\..\\x64\\DEBUG\\Dll_Lib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        bool GetMKL(int nx, float[] x, int ny, float[] y, float[] derivative, ref int error, float[] coeff);
        //передаю также параметр coeff - для полученных коэффициентов кубических сплайнов (нужен был при отладке и проверке, решил оставить)
        public bool FirstDerivative(bool ShowCoeff = false)
        {
            /* для дальнейшей работы в Dll_Lib со сплайнами нужно определить параметры:
            1) nx = X_Nodes - число узлов сплайна
            2) Собственно, так как у нас сетка равномерная, то нужно передать начальное и конечные значения узлов сплайна
            3) ny = Y_Nodes * 2 - размерность векторной функции 
            4) Массив y - значения векторной функции (введем значения в цикле)
            */
            int nx, ny, err = 0;
            nx = X_Nodes;
            float[] x = new float[2] { 0, (nx - 1) * Step.X };
            ny = Y_Nodes * 2;
            float[] y = new float[nx * ny];
            int i = 0;
            float[] der = new float[nx * ny];
            float[] coeff = new float[ny * 4 * (nx - 1)];
            for (int j = 0; j < Y_Nodes; j++)  //формируем одномерный массив y - значений векторной функции
            {
                for (i = 0; i < nx; i++)
                {
                    y[2 * j * nx + i] = (float)FieldValue[i, j].Real;
                }
                for (i = 0; i < nx; i++)
                {
                    y[(2 * j + 1) * nx + i] = (float)FieldValue[i, j].Imaginary;
                }
            }
            GetMKL(nx, x, ny, y, der, ref err, coeff); //используем PInvoke, в der - массив производных, err - код ошибки
            if (err != 0)
            {
                Console.WriteLine("ERROR in MKL = " + err);
                return false;
            }
            for (int j = 0; j < Y_Nodes; j++) // полученный массив производных в узлах преобразовываем в двумерный массив
            {
                for (i = 0; i < nx; i++)
                {
                    Derivative[i, j] = new Complex(der[2 * nx * j + i], der[2 * nx * j + nx + i]);
                }
            }
            Console.WriteLine(this.DerToString("F3"));
            if (ShowCoeff == true)
            {
                i = 0;
                Console.WriteLine("Коэффициенты, начиная от свободного до при ^3 в многочлене для узлов x в промежутках:");
                Console.Write("   ( x равномерно разбивается соответственно на ");
                for (float k = x[0]; k < x[1]; k += Step.X)
                {
                    Console.Write($"[{k} , {k + Step.X}] ");
                }
                Console.WriteLine($") (в самих куб. многочленах х принадлежит [{x[0]} , {x[0] + Step.X}])\n\n");
                Console.Write("\tC\tC1*x\t C2*x^2\t   C3*x^3");
                foreach (var item in coeff)
                {
                    if (i % 4 == 0)
                        Console.WriteLine();
                    if (i % ((nx - 1) * 4) == 0)
                        Console.WriteLine("Новый элемент векторной функции");
                    Console.Write("{0,10:F2}", item);
                    i++;
                }
                Console.WriteLine();
            }
            return true;
        }
        public Complex? FirstDerivativeSplineAt (int jx, int jy)
        {
            if ((jx >= 0 && jx < X_Nodes && jy >= 0 && jy <= Y_Nodes) == true)
                return Derivative[jx, jy];
            return null;
        }
        public Complex? FirstDerivativeLeftAt(int jx, int jy)
        {
            if ((jx >= 1 && jx < X_Nodes && jy >= 0 && jy <= Y_Nodes) == true)
                return (FieldValue[jx, jy] - FieldValue[jx - 1, jy]) / Step.X;
            return null;
        }
        public Complex? FirstDerivativeRightAt(int jx, int jy)
        {
            if ((jx >= 0 && jx + 1 < X_Nodes && jy >= 0 && jy <= Y_Nodes) == true)
                return (FieldValue[jx+1, jy] - FieldValue[jx, jy]) / Step.X;
            return null;
        }
        public V2DataArray (string st, DateTime DT): base(st, DT)
        {
            FieldValue = new Complex[0,0];
            Derivative = new Complex[0,0];
            Step = new Vector2(0, 0);
            X_Nodes = Y_Nodes = 0;
        }
        public V2DataArray(string st, DateTime DT, int X, int Y, Vector2 v2, Fv2Complex F) : base(st, DT)
        {
            if (X < 0 || Y < 0) { 
                X = 0; Y = 0;
                Console.WriteLine("X or Y nodes less than 0 and count of nodes X and Y was automatically set to [0, 0]\n");
            }
            FieldValue = new Complex[X, Y];
            Derivative = new Complex[X, Y];
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
                    else
                    {
                        return Step.Y;
                    }
                }
                else return 0;
            }
        }
        public override string ToString()
        {
            return $"(type = V2DataArray), from base class (V2Data) {base.ToString()}" +
                $". The grid has the following parametres:X_Nodes = {X_Nodes}, " +
                $"Y_Nodes = {Y_Nodes}, Step = {Step}\n";
        }
        public override string ToLongString(string format, bool tabulation = false)
        {
            string st = tabulation == false ? this.ToString() : "    " + this.ToString();
            for (int i = 0; i < X_Nodes; i++)
            {
                for (int j = 0; j < Y_Nodes; j++)
                {
                    if (tabulation)
                        st += "\t";
                    st += $"(i = {i}, j = {j}), coordinates: X = {(i * Step.X).ToString(format)}, " +
                        $"Y = {(j * Step.Y).ToString(format)}, FieldValue = ({FieldValue[i, j].Real.ToString(format)} + " +
                        $" {FieldValue[i, j].Imaginary.ToString(format)}i) (with + module = " + 
                        $"{FieldValue[i, j].Magnitude.ToString(format)})\n";
                }
            }
            return st + "\n";
        }
        public string DerToString(string format) // выводим значения производной, посчитанной с помощью сплайн-интерполяции
        // и приближенные значения левой и правой производной
        {
            string st = "";
            for (int i = 0; i < X_Nodes; i++)
            {
                for (int j = 0; j < Y_Nodes; j++)
                {
                    st += $"(i = {i}, j = {j}), coordinates: X = {(i * Step.X).ToString(format)}, " +
                        $"Y = {(j * Step.Y).ToString(format)}, Derivative = ({Derivative[i, j].Real.ToString(format)} + " +
                        $" {Derivative[i, j].Imaginary.ToString(format)}i); ";
                    if (FirstDerivativeLeftAt(i, j) == null)
                        st += "LeftDer = null; ";
                    else
                        st += ($"LeftDer = { FirstDerivativeLeftAt(i, j)}; ");
                    if (FirstDerivativeRightAt(i, j) == null)
                        st += "RightDer = null\n";
                    else
                        st += ($"RightDer = { FirstDerivativeRightAt(i, j)}\n");
                }
            }
            return st + "\n";
        }
        public bool SaveAsText(string filename)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(filename);
                {
                    sw.WriteLine(Ident); sw.WriteLine(Date);                 
                    sw.WriteLine(X_Nodes + " " + Y_Nodes);    
                    sw.WriteLine(Step.X + " " + Step.Y);
                    foreach (var item in FieldValue)
                        sw.WriteLine(item.Real + " " + item.Imaginary);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error in SaveAsText\n{ex.Message}");
                return false;
            }
            finally
            {
                if (sw != null)
                    sw.Dispose();
            }
            return true;
        }
        public static bool LoadText(string filename, ref V2DataArray v2)
        {
            StreamReader sr = null;
            try
            {
                
                sr = new StreamReader(filename);
                {
                    char[] separator = { '.', ' ', ':' };        //for Parse

                    v2.Ident = sr.ReadLine();                    

                    string [] st = sr.ReadLine().Split(separator);
                    v2.Date = new DateTime(int.Parse(st[2]), int.Parse(st[1]), int.Parse(st[0]), int.Parse(st[3]), int.Parse(st[4]), int.Parse(st[5]));   

                    st = sr.ReadLine().Split(separator);
                    v2.X_Nodes = int.Parse(st[0]) ; v2.Y_Nodes = int.Parse(st[1]);

                    st = sr.ReadLine().Split(separator);
                    v2.Step = new Vector2(float.Parse(st[0]), float.Parse(st[1]));
                    v2.FieldValue = new Complex[v2.X_Nodes, v2.Y_Nodes];
                    for (int i = 0; i < v2.X_Nodes; i++)
                    {
                        for (int j = 0; j < v2.Y_Nodes; j++)
                        {
                            st = sr.ReadLine().Split(separator);
                            v2.FieldValue[i, j] = new Complex(float.Parse(st[0]), float.Parse(st[1]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadAsText\n{ex.Message}");
                return false;
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }
            return true;
        }
        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < X_Nodes; i++)
            {
                for (int j = 0; j < Y_Nodes; j++)
                {
                    float X = i * Step.X;
                    float Y = j * Step.Y;
                    yield return new DataItem(new Vector2(X, Y), FieldValue[i, j]);
                }
            }
        }

        public static explicit operator V2DataList(V2DataArray Arr)
        {
            V2DataList NewList = new V2DataList("LIST_ReceivedFrom "+ Arr.Ident, DateTime.Now);
            for (int i = 0; i < Arr.X_Nodes; i++)
            {
                for (int j = 0; j < Arr.Y_Nodes; j++)
                {
                    DataItem DI = new DataItem(new Vector2(Arr.Step.X * i, Arr.Step.Y * j), Arr.FieldValue[i, j]);
                    NewList.Add(DI);
                }
            }
            return NewList;
        }
    }
}
