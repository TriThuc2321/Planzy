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


        public static bool CheckBookingSticketID(string test)
        {
            const string Element = "ABCDEFGHIJKLMNOPRQSTUVWYZXabcdefghijklmnopqrstuvwzxy0123456789_-";
            for (int i = 0; i < test.Length; i++)
            {
                int j;
                for (j = 0; j < Element.Length; j++)
                {
                    if (Element[j] == test[i])
                    {
                        break;
                    }
                }
                if (j == Element.Length)
                    return false;
            }
            return true;
        }

        public static bool CheckAddress(string test)
        {
            const string ElementAddress = "ABCDEFGHIJKLMNOPRQSTUVWYZXabcdefghijklmnopqrstuvwzxy0123456789_- ";
            for (int i = 0; i < test.Length; i++)
            {
                int j;
                for (j = 0; j < ElementAddress.Length; j++)
                {
                    if (ElementAddress[j] == test[i])
                    {
                        break;
                    }
                }
                if (j == ElementAddress.Length)
                    return false;
            }
            return true;
        }



        public static bool KiemTraChuoiSoNguyen(string test)
        {
            if (test == null)
                return false;
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
            if (test != "" && Convert.ToInt64(test) > 0)
                return true;
            else
                return false;
        }
    }
}

