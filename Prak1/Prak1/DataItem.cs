using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prak1
{
    struct DataItem
    {
        public Vector2 Pos { get; set; }
        public Complex Val { get; set; }

        public DataItem(Vector2 Pos, Complex Val)
        {
            this.Pos = Pos; this.Val = Val;
        }
        public string ToLongString(string format)
        {
            return ($"X = {Pos.X.ToString(format)}, Y = {Pos.Y.ToString(format)}, Field = ({Val.Real.ToString(format)}" +
                $" + {Val.Imaginary.ToString(format)}i) (with module = " +
                $" {Val.Magnitude.ToString(format)})\n");
        }
        public override string ToString()
        {
            return $"X = {Pos.X}, Y = {Pos.Y} \nField = ({Val.Real} + {Val.Imaginary}i)";
        }
        //public static bool operator ==(DataItem DI_1, DataItem DI_2)
        //{
        //    return DI_1.Pos == DI_2.Pos ? DI_1.Val == DI_2.Val : false;  not need
        //}
        //public static bool operator !=(DataItem DI_1, DataItem DI_2)
        //{
        //    return DI_1.Pos == DI_2.Pos ? DI_1.Val != DI_2.Val : true;
        //}

    }
}
