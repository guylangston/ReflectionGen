using System;
using System.Collections.Generic;

namespace ReflectionGen.CodeGen
{
    public class TestClass
    {
        // Value types
        public bool Boolean { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public int Integer { get; set; }
        public float Float { get; set; }
        public decimal Decimal { get; set; }

        // Nullable

        public bool? BooleanNullable { get; set; }
        public DateTime? DateTimeNullable { get; set; }
        public TimeSpan? TimeSpanNullable { get; set; }
        public int? IntegerNullable { get; set; }
        public float? FloatNullable{ get; set; }
        public decimal? DecimalNullable { get; set; }

        // Classes
        public string String { get; set; }

        // Lists
        public List<string> StringList { get; set; }
        public string[] StringArray { get; set; }

    }
}