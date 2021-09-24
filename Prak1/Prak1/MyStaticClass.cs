using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prak1
{
    delegate Complex Fv2Complex(Vector2 v2);
    static class MyStaticClass
    {
        static Random rnd = new Random();
        public static Complex JustRandom_0_1000 (Vector2 v2)
        {
            return new Complex((float)rnd.NextDouble() * 1000, (float)rnd.NextDouble() * 1000);
        }
        public static Complex Way2ToGet (Vector2 v2)
        {
            return new Complex(Math.Abs(Math.Pow((v2.X + v2.Y), 1.3)), (v2.X + v2.Y) * (1.5 + 5));
        }
        public static Complex JustDoubled(Vector2 v2)
        {
            return new Complex(v2.X * 2, v2.Y * 2);
        }
    }
}
