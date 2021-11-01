using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Prak1
{
    class Program
    {
        static void Main(string[] args)
        {
            LinqTests();                                                                    
            //Files_Tests();
        }
        static void Files_Tests()
        {
            Console.WriteLine("1) Создаем объект V2DataArray. 2) Выводим его. 3) Сохраняем его в файл \"MyArray.txt\" (текстовый файл). 4. Далее восстанавливаем объект из этого файла и выводим этот сохраненный объект");
            V2DataArray Array1 = new V2DataArray("1st Array", DateTime.Now, 2, 2, new Vector2(0.5f, 0.6f), MyStaticClass.JustRandom_0_1000);
            Console.WriteLine("______________Start of 1st test______________\nInput Array: (this is ToLongString method)\n" + Array1.ToLongString("F3"));
            Array1.SaveAsText("MyArray.txt");                  //файл будет находиться в папке: bin/Debug

            V2DataArray RecoveredArray = new V2DataArray("whatever", DateTime.UtcNow);
            V2DataArray.LoadText("MyArray.txt", ref RecoveredArray);
            Console.WriteLine("Array from .txt file: (this is also ToLongString method)\n" + RecoveredArray.ToLongString("F3"));
            Console.WriteLine("==============End of 1st test==============\n");

            Console.WriteLine("1) Создаем объект V2DataList. 2) Выводим его. 3) Сохраняем его в файле \"MyBinaryList\" (бинарный файл). 4. Далее восстанавливаем объект из файла и выводим этот сохраненный объект");
            V2DataList List1 = new V2DataList("1st List", DateTime.Now);
            List1.AddDefaults(3, MyStaticClass.JustDoubled);
            Console.WriteLine("______________Start of 2nd test______________\nInput List: (this is ToLongString method)\n" + List1.ToLongString("F3"));
            List1.SaveBinary("MyBinaryList");
            V2DataList RecoveredList = new V2DataList("whatever", DateTime.UtcNow);
            V2DataList.LoadBinary("MyBinaryList", ref RecoveredList);
            Console.WriteLine("List from Binary file: (this is also ToLongString method)\n" + RecoveredList.ToLongString("F3"));
            Console.WriteLine("==============End of 2nd test==============\n");
        }
        static void LinqTests()
        {
            Console.WriteLine("Сначала создаем все необходимые элементы коллекции (создаются 5 элементов: 2 массива с количеством элементов: 4, 2 ; 1 пустой массив ; 1 лист с 2 элементами ; 1 пустой лист)\n" +
                "Затем, соответственно, запускаем 1-е - 3-е LINQ-свойства. (Начало вывода свойства отмечено нижними подчеркиваниями \"_\", конец - символами \"=\")");
            V2DataList List1 = new V2DataList("1st List", DateTime.Now);
            List1.AddDefaults(2, MyStaticClass.JustDoubled);                                                                                              //2 elements in List1
            V2DataList Empty_List = new V2DataList("Empty List", DateTime.UtcNow);                                                                          //0 elements in Empty_list
            V2DataArray Array1 = new V2DataArray("1st Array", DateTime.Now, 2, 2, new Vector2(0.5f, 0.6f), MyStaticClass.JustRandom_0_1000);              //4 elements in Array1
            V2DataArray Array2 = new V2DataArray("Empty Array", new DateTime(2000, 01, 01), 2, 1, new Vector2(0.2f, 0.5f), MyStaticClass.JustRandom_0_1000);//2 elements in Array2
            V2DataArray Empty_Array = new V2DataArray("2nd Array", new DateTime(2000, 01, 01));                                                           //0 elements in Empty_array
            V2DataList Converted_List = (V2DataList)Array2;
            Converted_List.AddDefaults(1, MyStaticClass.JustDoubled); //для проверки 2-го LINQ свойства, из Converted List'а не должна попасть ни одна точка, кроме этого одного добавленного элемента
            V2MainCollection MyCol = new V2MainCollection();
            MyCol.Add(List1); MyCol.Add(Empty_List); MyCol.Add(Converted_List);
            MyCol.Add(Array1); MyCol.Add(Array2); MyCol.Add(Empty_Array);

            Console.Write("___________________________________All elements in MyCol:___________________________________\n");
            Console.WriteLine(MyCol.ToLongString("F2"));
            for (int i = 0; i < MyCol.Count; i++)
            {
                Console.WriteLine($"(Collection[{i}]) Count = {MyCol[i].Count}");
            }
            Console.WriteLine("================================End elements in MyCol================================\n");


            Console.Write("__________________1st LINQ property, DataItem element with maximum module from all the elements in MyCollection__________________:\n");
            if (MyCol.MaxModule != null)
                Console.Write("\t" + MyCol.MaxModule.Value.ToLongString("F2"));
            else
                Console.WriteLine("\tThe property is NULL for some reason");
            Console.WriteLine("==========================End of 1st property==========================\n");


            Console.WriteLine("__________________2nd LINQ property, All points (Vector2) in MyCollection, that do exist in V2DataList-type, but do not in V2DataArray__________________:");
            if (MyCol.All_Points != null)
                foreach (var item in MyCol.All_Points)
                    Console.WriteLine("\t" + item.ToString("F2"));
            else
                Console.WriteLine("\tThe property is NULL for some reason");
            Console.WriteLine("==========================End of 2nd property==========================\n");


            Console.WriteLine("__________________3rd LINQ property, get grouped <V2Data> elements in MyCollection depending on Count of Field's measurements__________________:\n");
            IEnumerable<IGrouping<int, V2Data>> grouped = MyCol.Group_Elements;
            if (grouped == null)
                Console.WriteLine("\tThe property is NULL for some reason");
            else
                foreach (IGrouping<int, V2Data> surface in grouped)
                {
                    Console.Write("Count of elements " + surface.Key + " (total " + surface.Count() + " element/s):\n");
                    foreach (V2Data item in surface)
                        Console.WriteLine(item.ToLongString("F3", true));
                }
            Console.WriteLine("==========================End of 3rd property==========================\n");
        }
    }
}
