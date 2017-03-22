using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _1CIntegrationDB 
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

        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || obj == DBNull.Value || obj.ToString() == "";
        }

        public static double AsDouble(this object obj)
        {
            try
            {
                return Convert.ToDouble(obj.ToString());
            }
            catch (Exception e)
            {
                return 0.0;
            }

        }

        public static double AsDouble(this string str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception e)
            {
                return 0.0;
            }

        }

        public static int AsInteger(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj.ToString());
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

        public static object ToList(this DataTable dt)
        {
            try
            {
                return
                    (from DataRow dr in dt.Rows
                        select dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]))
                        .ToList();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static IList<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            IList<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        public static IList<T> ToList<T>(this DataTable table, Dictionary<string, string> mappings) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            IList<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties, mappings);
                result.Add(item);
            }

            return result;
        }
        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                property.SetValue(item, row[property.Name], null);
            }
            return item;
        }

        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties,
            Dictionary<string, string> mappings) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (mappings.ContainsKey(property.Name))
                    property.SetValue(item, row[mappings[property.Name]], null);
            }
            return item;
        }

        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType),
                                null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static Stream ToStream(this Image image, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        public static Stream ToStream(this Bitmap image, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        public static Bitmap ResizeBitmapUpto(this Image b, int nWidth, int nHeight, InterpolationMode interpolationMode)
        {
            var box = PlaceInside(b.Width, b.Height, nWidth, nHeight);
            var actualNewWidth = (int)Math.Ceiling(box.Width);
            var actualNewHeight = (int)Math.Ceiling(box.Height);
            var result = new Bitmap(actualNewWidth, actualNewHeight);
            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = interpolationMode;
                g.DrawImage(b, 0, 0, actualNewWidth, actualNewHeight);
            }
            return result;
        }

        static RectangleF PlaceInside(int oldWidth, int oldHeight, int newWidth, int newHeight)
        {
            if (oldWidth <= 0 || oldHeight <= 0 || newWidth <= 0 || newHeight <= 0)
                return new RectangleF(oldWidth, oldHeight, newWidth, newHeight);
            var widthFactor = newWidth / (float)oldWidth;
            var heightFactor = newHeight / (float)oldHeight;
            if (widthFactor < heightFactor)
            {
                var scaledHeight = widthFactor * oldHeight;
                return new RectangleF(0, (newHeight - scaledHeight) / 2.0f, newWidth, scaledHeight);
            }
            var scaledWidth = heightFactor * oldWidth;
            return new RectangleF((newWidth - scaledWidth) / 2.0f, 0, scaledWidth, newHeight);
        }

        public static void ToFileSave(this Bitmap b, string fullPath, string name)
        {
            try
            {
                b.Save(fullPath.Replace(".jpg","_min.jpg"), ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}