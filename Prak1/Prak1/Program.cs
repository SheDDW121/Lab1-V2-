using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Prak1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Для элементов сетки используется функция CubicPol из класса "MyStaticClass"
            Test1(true);  //true указывает, что будут выводиться коэффициенты полученного сплайна для каждого элемента векторной функции 
                          //Test1 - матрица 4х1, шаг - {2, 1}
            //Test2(true);    // матрица 3х4, шаг - {2.5, 1.5}
        }
        static void Test1(bool ShowCoeff = false)
        {
            V2DataArray Array1 = new V2DataArray("1st Array", DateTime.Now, 4, 1, new Vector2(2f, 1f), MyStaticClass.CubicPol);
            Console.WriteLine(Array1.ToLongString("F3"));
            Array1.FirstDerivative(ShowCoeff);
        }
        static void Test2(bool ShowCoeff = false)
        {
            V2DataArray Array2 = new V2DataArray("2nd Array", DateTime.Now, 3, 4, new Vector2(2.5f, 1.5f), MyStaticClass.CubicPol);
            Console.WriteLine(Array2.ToLongString("F3"));
            Array2.FirstDerivative(ShowCoeff);
        }
    }
}
