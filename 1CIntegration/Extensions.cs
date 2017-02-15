using System;

namespace _1CIntegration
{
    public static class Extensions
    {
        public static bool IsNull(this object obj)
        {
            try
            {
                return obj == null;
            }
            catch (Exception e)
            {
                return true;
            }

        }

        public static int AsInteger(this object obj)
        {
            try
            {
                if (obj is Int32)
                {
                    return Convert.ToInt32(obj);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public static int AsInteger(this string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                return 0;
            }

        }
    }
}