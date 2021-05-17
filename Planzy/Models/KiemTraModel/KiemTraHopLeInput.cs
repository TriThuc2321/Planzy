using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planzy.Models.KiemTraModel
    {
    public static class KiemTraHopLeInput
{
    const string MA = "ABCDEFGHIJKLMNOPRQSTUVWYZXabcdefghijklmnopqrstuvwzxy0123456789";
    const string SO = "0123456789";

    public static bool KiemTraMa(string test)
    {
        for (int i = 0; i < test.Length; i++)
        {
            int j;
            for (j = 0; j < MA.Length; j++)
            {
                if (MA[j] == test[i])
                {
                    break;
                }
            }
            if (j == MA.Length)
                return false;
        }
        return true;
    }
    public static bool KiemTraChuoiSoNguyen(string test)
    {
        for (int i = 0; i < test.Length; i++)
        {
            int j;
            for (j = 0; j < SO.Length; j++)
            {
                if (SO[j] == test[i])
                {
                    break;
                }
            }
            if (j == SO.Length)
                return false;
        }
        return true; ;
    }
}
}
