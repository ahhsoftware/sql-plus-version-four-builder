using SQLPLUS.Builder.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLPLUS.Builder.TemplateModels
{
    public class StaticCollection : ErrorBase
    {
        public string Name { set; get; }

        public List<Column> Columns { set; get; }

        public List<List<object>> Data { set; get; }

        public string ConstructorParameterList()
        {
            string[] array = new string[Columns.Count];

            for (int idx = 0; idx != Columns.Count; idx++)
            {
                array[idx] = $"{Columns[idx].PropertyType} {Columns[idx].PropertyName}";
            }
            return string.Join(", ", array);
        }

        public List<string> Usings
        {
            get
            {
                List<string> result = new List<string>();
                result.TryAddItem("System.Collections.Generic");
                foreach(Column col in Columns)
                {
                    result.TryAddItem(col.Using);
                }
                return result;
            }
        }

        public string NewObjectDefinition(int idx)
        {
            StringBuilder bldr = new StringBuilder();
            //new EmployeeRanges((byte?)1,"SelfEmployed","Self Employed")
            bldr.Append($"new {Name}(");

            List<object> values = Data[idx];
            for (int i = 0; i != values.Count; i++)
            {
                if (i != 0)
                {
                    bldr.Append(", ");
                }
                Column item = Columns[i];
                object val = values[i];

                if (val == DBNull.Value)
                {
                    if (item.PropertyType.ToLower().Replace("?", "") == "sqlhierarchyid")
                    {
                        bldr.Append("SqlHierarchyId.Null");
                    }
                    else
                    {
                        bldr.Append("null");
                    }
                }
                else
                {
                    switch (item.PropertyType)
                    {
                        case "DateTime":
                            bldr.Append($"DateTime.Parse(\"{val}\")");
                            break;
                        case "TimeSpan":
                            bldr.Append($"TimeSpan.Parse(\"{val}\")");
                            break;
                        case "DateTimeOffset":
                            bldr.Append($"DateTimeOffset.Parse(\"{val}\")");
                            break;
                        case "bool":
                            bool temp = (bool)val;
                            bldr.Append(temp.ToString().ToLower());
                            break;
                        case "Guid":
                            bldr.Append($"new System.Guid(\"{val}\")");
                            break;
                        case "byte[]":
                            byte[] array = val as byte[];
                            bldr.Append($"new byte[]{{{ByteArrayString(array)}}}");
                            break;
                        case "string":
                        case "object":
                            bldr.Append($"@\"{val.ToString().Replace(@"""", @"""""")}\"");
                            break;
                        case "SqlHierarchyId":
                            bldr.Append($"SqlHierarchyId.Parse(\"{val}\")");
                            break;
                        case "SqlGeometry":
                            bldr.Append($"SqlGeometry.Parse(\"{val}\")");
                            break;
                        case "SqlGeography":
                            bldr.Append($"SqlGeography.Parse(\"{val}\")");
                            break;
                        default:
                            bldr.Append($"({item.PropertyType}){val}");
                            break;
                    }
                }
            }
            bldr.Append(")");
            return bldr.ToString();
        }
        private string ByteArrayString(byte[] array)
        {
            StringBuilder bldr = new StringBuilder();

            for (int idx = 0; idx != array.Length; idx++)
            {
                if (idx != 0)
                {
                    bldr.Append(",");
                }
                bldr.Append($" {array[idx]}");
            }
            bldr.Append(" ");
            return bldr.ToString();
        }
    }
}