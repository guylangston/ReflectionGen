using System;

namespace ReflectionGen.CodeGen
{
    public static class SQLHelper
    {

        public static string ToSQLType(Type type)
        {
            switch (TypeHelper.ToFriendlyCSharp(type))
            {
                    // Value
                case "bool": return "bit NOT NULL";
                case "int": return "int NOT NULL";
                case "float": return "float NOT NULL";
                case "double": return "double NOT NULL";
                case "decimal": return "numeric NOT NULL";
                case "DateTime": return "datestamp NOT NULL";
                case "TimeSpan": return "int NOT NULL";

                    // Nullable
                case "bool?": return "bit";
                case "int?": return "int";
                case "float?": return "float";
                case "double?": return "double";
                case "decimal?": return "numeric";
                case "DateTime?": return "datestamp";
                case "TimeSpan?": return "int";

                    // Simple Class
                case "string": return "nvarchar(500)";

                default:
                    return string.Format("??? {0}", TypeHelper.ToFriendlyCSharp(type));
            }

        }
    }
}