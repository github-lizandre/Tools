using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class NumberHelper
    {
        //function to calculate the smallest multiple of x closest to a given number
        public static int GetNearestMultiple(int qte, int multiple)
        {
            if (multiple > 0)
            {
                if (multiple > qte)
                    return multiple;
                qte = qte + (multiple / 2);
                qte = qte - (qte % multiple);
                return qte;
            }
            return 0;
        }
    }
}
