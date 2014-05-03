using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ReflectionGen.CodeGen
{
    public class TypeHelper
    {

        public class SimpleSpecification
        {
            public bool IsAtomic { get; set; }
            public bool IsValueType { get; set; }
            public bool IsNullable { get; set; }

            public string Name { get; set; }
            public Type Type { get; set; }
            public bool IsComplex { get; set; }
            public bool IsCollection { get; set; }
        }

        

        public static string ToFriendlyCSharp(Type type)
        {
            var spec = ToSpecification(type);
            if (spec != null) return spec.Name;
            return type.Name;
        }

        public static SimpleSpecification ToSpecification(Type type)
        {
            // Value
            if (type == typeof(bool)) return new SimpleSpecification() { Name = "bool", IsValueType = true};
            if (type == typeof(int)) return new SimpleSpecification() { Name = "int", IsValueType = true };
            if (type == typeof(long)) return new SimpleSpecification() { Name = "long", IsValueType = true };
            if (type == typeof(byte)) return new SimpleSpecification() { Name = "byte", IsValueType = true };
            if (type == typeof(float)) return new SimpleSpecification() { Name = "float", IsValueType = true };
            if (type == typeof(double)) return new SimpleSpecification() { Name = "double", IsValueType = true };
            if (type == typeof(decimal)) return new SimpleSpecification() { Name = "decimal", IsValueType = true };
            if (type == typeof(DateTime)) return new SimpleSpecification() { Name = "DateTime", IsValueType = true };
            if (type == typeof(TimeSpan)) return new SimpleSpecification() { Name = "TimeSpan", IsValueType = true };

            // NUllable
            if (type.Name.StartsWith("Nullable`1"))
            {
                var inner = ToSpecification(type.GetGenericArguments().First());
                if (inner == null) return null;
                inner.IsNullable = true;
                inner.Type = type;
                return inner;
            }

            // Common
            if (type == typeof(string)) return new SimpleSpecification() { Name = "string"};
            if (type.IsArray)
            {
                return new SimpleSpecification() { Name = type.Name, IsCollection = true, Type = type};
            }
            if (type.IsGenericType)
            {
                return new SimpleSpecification()
                {
                    Name = string.Format("{0}<{1}>", GetGenericName(type), StringHelper.Concat(type.GenericTypeArguments, ToFriendlyCSharp)),
                    Type = type,
                    IsCollection = true
                };
            }
           

            return new SimpleSpecification()
            {
                Type = type,
                IsComplex = true,
                Name = type.Name
            };
        }

        private static string GetGenericName(Type type)
        {
            if (!type.IsGenericType) return null;
            var idx = type.Name.IndexOf('`');
            return type.Name.Substring(0, idx);
        }
    }
}