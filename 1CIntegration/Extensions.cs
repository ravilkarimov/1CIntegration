﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace _1CIntegration
{
    public static class Extensions
    {
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