using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Documents;

namespace ReflectionGen.CodeGen
{
    public interface ICodeGenFromType
    {
        string Generate(Type type);
    }


    public class CodeGenSQL_CREATETABLE : ICodeGenFromType
    {
        public string Generate(Type type)
        {
            var props = type.GetProperties();
            var sb = new StringBuilder();
            sb.Append("@\"");
            sb.AppendFormat("INSERT INTO [{0}]", type.Name);
            sb.AppendLine();
            sb.Append("(");
            foreach (var prop in props)
            {
                sb.Append(prop.Name);
                sb.Append(", ");
            }
            sb.AppendLine(") VALUES ");
            sb.Append("(");
            int cc = 0;
            foreach (var prop in props)
            {
                sb.Append("\t{");
                sb.Append(cc++);
                sb.Append("},   -- ");
                sb.AppendLine(prop.Name);
            }
            sb.AppendLine(");");
            sb.AppendLine("SELECT CAST(@@IDENTITY AS int);\",");
            sb.Append(StringHelper.Concat(props.Select(x => string.Format("item.{0}", x.Name))));

            return sb.ToString();
        }
    }



    public class CodeGenSQL_INSERT : ICodeGenFromType
    {
        public string Generate(Type type)
        {
            var props = type.GetProperties();
            var sb = new StringBuilder();
            sb.Append("@\"");
            sb.AppendFormat("INSERT INTO [{0}]", type.Name);
            sb.AppendLine();
            sb.Append("(");
            sb.Append(StringHelper.Concat(props, x => x.Name));
            sb.AppendLine(") VALUES ");
            sb.Append("(");
            int cc = 0;
            foreach (var prop in props)
            {
                sb.Append("\t{");
                sb.Append(cc++);
                sb.Append("}");
                if (prop != props.Last())
                {
                    sb.Append(",");    
                }
                sb.Append("\t\t-- ");
                sb.AppendLine(prop.Name);
            }
            sb.AppendLine(");");
            sb.AppendLine("SELECT CAST(@@IDENTITY AS int);\",");
            sb.Append(StringHelper.Concat(props.Select(x => string.Format("item.{0}",x.Name))));
            
            return sb.ToString();
        }


    }

    public class CodeGenSQL_UPDATE : ICodeGenFromType
    {
        public string Generate(Type type)
        {
            var props = type.GetProperties();
            var sb = new StringBuilder();
            sb.AppendFormat("@\"UPDATE [{0}] SET ", type.Name);
            sb.AppendLine();
            
            int cc = 0;
            foreach (var prop in props)
            {
                sb.Append("\t[");
                sb.Append(prop.Name);
                sb.Append("]={");
                sb.Append(cc++);
                sb.Append("}");
                if (prop != props.Last())
                {
                    sb.Append(",");
                }
                sb.Append("\t\t-- ");
                sb.AppendLine(SQLHelper.ToSQLType(prop.PropertyType));
            }
            sb.AppendLine();
            sb.AppendLine("WHERE Id={0}\",");
            sb.Append(StringHelper.Concat(props.Select(x => "item."+x.Name)));

            return sb.ToString();
        }
    }

    public class CodeGenSQL_SELECT : ICodeGenFromType
    {
        public string Generate(Type type)
        {
            var props = type.GetProperties();
            var sb = new StringBuilder();
            sb.Append("@\"SELECT ");
            sb.AppendLine(StringHelper.Concat(props.Select(x => x.Name)));
            sb.AppendFormat(" FROM [{0}]", type.Name);
            sb.AppendLine();
            sb.Append("WHERE {0}\",");
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine("// C# Binding");
            foreach (var prop in props)
            {
                sb.AppendFormat("\titem.{1} = {0};", GetAccessorFor(prop), prop.Name);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private string GetAccessorFor(PropertyInfo prop)
        {
            var spec = TypeHelper.ToSpecification(prop.PropertyType);
            if (spec.IsCollection || spec.IsComplex)
            {
                return string.Format("({0})binder.GetObject(row, \"{1}\", typeof({2}))", spec.Name, prop.Name, spec.Name);
            }
            var name = FirstUpper(spec.Name ?? prop.PropertyType.Name);
            if (name.EndsWith("?"))
            {
                name = name.Replace("?", "Nullable");
            }
            if (name.EndsWith("?"))
            {
                name = name.Replace("[]", "Array");
            }
            return string.Format("binder.Get{0}{2}(row, \"{1}\")", name, prop.Name, spec.IsNullable ? "Nullable" : "");
        }


        string FirstUpper(string s)
        {
            var sb = new StringBuilder(s);
            sb[0] = char.ToUpperInvariant(sb[0]);
            return sb.ToString();
        }

    }

}