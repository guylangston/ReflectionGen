using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ReflectionGen.CodeGen
{
    public static class StringHelper
    {
        public static string Concat(IEnumerable list, string sep=", ", int maxLine=80)
        {
            if (list == null) return null;
            var all = new StringBuilder();
            var sb = new StringBuilder();
           
            bool sepNeeded = false;
            foreach (var obj in list)
            {
                if (sepNeeded)
                {
                    sb.Append(sep);
                }

                if (sb.Length > maxLine)
                {
                    if (all.Length > 0) all.AppendLine();
                    all.Append(sb);
                    sb.Clear();
                }
                
                if (obj != null)
                {
                    sb.Append(obj.ToString());
                    sepNeeded = true;
                }
            }
            if (all.Length > 0) all.AppendLine();
            all.Append(sb);
            return all.ToString();
        }

        public static string Concat<T>(IEnumerable<T> list, Func<T, object> select, string sep = ", ", int maxLine = 80)
        {
            if (list == null) return null;
            var all = new StringBuilder();
            var sb = new StringBuilder();

            bool sepNeeded = false;
            foreach (var obj in list)
            {
                if (sepNeeded)
                {
                    sb.Append(sep);
                }

                if (sb.Length > maxLine)
                {
                    if (all.Length > 0) all.AppendLine();
                    all.Append(sb);
                    sb.Clear();
                }

                var val = select(obj);
                if (val != null)
                {
                    sb.Append(val);
                    sepNeeded = true;
                }
            }
            if (all.Length > 0) all.AppendLine();
            all.Append(sb);
            return all.ToString();
        }
    }
}