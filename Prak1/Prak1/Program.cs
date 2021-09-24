using System;
using System.Numerics;

namespace Prak1
{
    class Program
    {
        static void Main(string[] args)
        {
            // test start
            Console.WriteLine("*Test №1*");
            V2DataArray MyArray = new V2DataArray("Arr", new DateTime(1812, 09, 07), 3, 2,
                new Vector2(0.7f, 1.2f), MyStaticClass.Way2ToGet);
            Console.WriteLine($"{MyArray.ToLongString("F3")}Count = {MyArray.Count}; MinDistance = {MyArray.MinDistance}");
            V2DataList Converted = (V2DataList)MyArray;
            Console.WriteLine($"{Converted.ToLongString("F1")}Count = {Converted.Count}; MinDistance = {Converted.MinDistance}");
            // test 2 start
            Console.WriteLine("\n*Test №2*");
            V2DataList List1 = new V2DataList("1st List", DateTime.Now);
            List1.AddDefaults(5, MyStaticClass.JustDoubled);
            V2DataList List2 = new V2DataList("2nd List", DateTime.UtcNow); // empty list
            V2DataArray Array1 = new V2DataArray("1st Array", DateTime.Now, 2, 3, new Vector2(0.5f, 0.6f), MyStaticClass.JustRandom_0_1000);
            //V2DataArray Array2 = new V2DataArray("2nd Array", new DateTime(2000, 01, 01), 2, 2, new Vector2(0.2f, 0.5f), MyStaticClass.JustRandom_0_1000);
            V2DataArray Array2 = new V2DataArray("2nd Array", new DateTime(2000, 01, 01)); // empty array
            V2MainCollection MyCol = new V2MainCollection();
            MyCol.Add(List1); MyCol.Add(List2);
            MyCol.Add(Array1); MyCol.Add(Array2);
            Console.WriteLine(MyCol.ToLongString("F2"));
            //test 3 start
            Console.WriteLine("*Test №3*\nFrom MyCol:\n");
            for (int i = 0; i < MyCol.Count; i++)
            {
                Console.WriteLine($"(Collection[{i}]) Count = {MyCol[i].Count}, MinDistance = {MyCol[i].MinDistance}");
            }
        }
    }
}
