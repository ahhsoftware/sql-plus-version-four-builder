namespace SQLPLUS.Builder.Mappings
{
    using System;
    using System.Collections.Generic;
    using SQLPLUS.Builder.TemplateModels;

    public class DataTypeMappings
    {
        public static readonly List<DataTypeMapping> Mappings = new List<DataTypeMapping>()
        {
            new DataTypeMapping()
            {
                MSSQLDataType = "bigint",
                AdoSqlDbType = "SqlDbType.BigInt",
                AdoGetterFormatter = "rdr.GetInt64({0})",
                CSharpType = "long",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "binary",
                AdoSqlDbType = "SqlDbType.Binary",
                AdoGetterFormatter = "(byte[])rdr[{0}]",
                CSharpType = "byte[]",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.ValuesAreEqual
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "bit",
                AdoSqlDbType = "SqlDbType.Bit",
                AdoGetterFormatter = "rdr.GetBoolean({0})",
                CSharpType = "bool",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "char",
                AdoSqlDbType = "SqlDbType.Char",
                AdoGetterFormatter = "rdr.GetString({0})",
                CSharpType = "string",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "date",
                AdoSqlDbType = "SqlDbType.Date",
                AdoGetterFormatter = "rdr.GetDateTime({0})",
                CSharpType = "DateTime",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals,
                Using = "System"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "datetime",
                AdoSqlDbType = "SqlDbType.DateTime",
                AdoGetterFormatter = "rdr.GetDateTime({0})",
                CSharpType = "DateTime",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals,
                Using = "System"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "datetime2",
                AdoSqlDbType = "SqlDbType.DateTime2",
                AdoGetterFormatter = "rdr.GetDateTime({0})",
                CSharpType = "DateTime",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals,
                Using = "System"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "datetimeoffset",
                AdoSqlDbType = "SqlDbType.DateTimeOffset",
                AdoGetterFormatter = "rdr.GetDateTimeOffset({0})",
                CSharpType = "DateTimeOffset",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals,
                Using = "System"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "decimal",
                AdoSqlDbType = "SqlDbType.Decimal",
                AdoGetterFormatter = "rdr.GetDecimal({0})",
                CSharpType = "decimal",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "float",
                AdoSqlDbType = "SqlDbType.Float",
                AdoGetterFormatter = "rdr.GetDouble({0})",
                CSharpType = "double",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "geometry",
                AdoSqlDbType = "SqlDbType.Udt",
                AdoGetterFormatter = "(SqlGeometry)rdr[{0}]",
                CSharpType = "SqlGeometry",
                IsCSharpValueType = false,
                AdoUdtTypeName = "geometry",
                EqualityTestType = EqualityTestTypes.ValuesAreEqual,
                Using = "Microsoft.SqlServer.Types"

            },
            new DataTypeMapping()
            {
                MSSQLDataType = "geography",
                AdoSqlDbType = "SqlDbType.Udt",
                AdoGetterFormatter = "(SqlGeography)rdr[{0}]",
                CSharpType = "SqlGeography",
                IsCSharpValueType = false,
                AdoUdtTypeName = "geography",
                EqualityTestType = EqualityTestTypes.ValuesAreEqual,
                Using = "Microsoft.SqlServer.Types"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "hierarchyid",
                AdoSqlDbType = "SqlDbType.Udt",
                AdoGetterFormatter = "(SqlHierarchyId)rdr[{0}]",
                CSharpType = "SqlHierarchyId",
                IsCSharpValueType = true,
                AdoUdtTypeName = "hierarchyid",
                DefaultSize = 892,
                EqualityTestType = EqualityTestTypes.ValuesAreEqual,
                Using = "Microsoft.SqlServer.Types"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "int",
                AdoSqlDbType = "SqlDbType.Int",
                AdoGetterFormatter = "rdr.GetInt32({0})",
                CSharpType = "int",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "image",
                AdoSqlDbType = "SqlDbType.Binary",
                AdoGetterFormatter = "(byte[])rdr[{0}]",
                CSharpType = "byte[]",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.ValuesAreEqual
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "money",
                AdoSqlDbType = "SqlDbType.Money",
                AdoGetterFormatter = "rdr.GetDecimal({0})",
                CSharpType = "decimal",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "numeric",
                AdoSqlDbType = "SqlDbType.Decimal",
                AdoGetterFormatter = "rdr.GetDecimal({0})",
                CSharpType = "decimal",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "nchar",
                AdoSqlDbType = "SqlDbType.NChar",
                AdoGetterFormatter = "rdr.GetString({0})",
                CSharpType = "string",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "ntext",
                AdoSqlDbType = "SqlDbType.NText",
                AdoGetterFormatter = "rdr.GetString({0})",
                CSharpType = "string",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "nvarchar",
                AdoSqlDbType = "SqlDbType.NVarChar",
                AdoGetterFormatter = "rdr.GetString({0})",
                CSharpType = "string",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "real",
                AdoSqlDbType = "SqlDbType.Real",
                AdoGetterFormatter = "rdr.GetFloat({0})",
                CSharpType = "float",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "smalldatetime",
                AdoSqlDbType = "SqlDbType.DateTime",
                AdoGetterFormatter = "rdr.GetDateTime({0})",
                CSharpType = "DateTime",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals,
                Using = "System"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "smallint",
                AdoSqlDbType = "SqlDbType.SmallInt",
                AdoGetterFormatter = "rdr.GetInt16({0})",
                CSharpType = "short",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "smallmoney",
                AdoSqlDbType = "SqlDbType.Decimal",
                AdoGetterFormatter = "rdr.GetDecimal({0})",
                CSharpType = "decimal",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "structured",
                AdoSqlDbType = "SqlDbType.Structured",
                AdoGetterFormatter = "null",
                CSharpType = "object",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "sql_variant",
                AdoSqlDbType = "SqlDbType.Variant",
                AdoGetterFormatter = "rdr.GetValue({0})",
                CSharpType = "object",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.VariantsAreEqual
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "table type",
                AdoSqlDbType = "SqlDbType.Structured",
                CSharpType = "object",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.ValuesAreEqual,
                IsTableValueParameter = true,
                Using = "System.Collections.Generic"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "text",
                AdoSqlDbType = "SqlDbType.Text",
                AdoGetterFormatter = "rdr.GetString({0})",
                CSharpType = "string",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "tinyint",
                AdoSqlDbType = "SqlDbType.TinyInt",
                AdoGetterFormatter = "rdr.GetByte({0})",
                CSharpType = "byte",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "time",
                AdoSqlDbType = "SqlDbType.Time",
                AdoGetterFormatter = "rdr.GetTimeSpan({0})",
                CSharpType = "TimeSpan",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals,
                Using = "System"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "timestamp",
                AdoSqlDbType = "SqlDbType.Timestamp",
                AdoGetterFormatter = "(byte[])rdr[{0}]",
                CSharpType = "byte[]",
                IsCSharpValueType = false
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "uniqueidentifier",
                AdoSqlDbType = "SqlDbType.UniqueIdentifier",
                AdoGetterFormatter = "rdr.GetGuid({0})",
                CSharpType = "Guid",
                IsCSharpValueType = true,
                EqualityTestType = EqualityTestTypes.Equals,
                Using = "System"
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "varbinary",
                AdoSqlDbType = "SqlDbType.VarBinary",
                AdoGetterFormatter = "(byte[])rdr[{0}]",
                CSharpType = "byte[]",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.ValuesAreEqual
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "varchar",
                AdoSqlDbType = "SqlDbType.VarChar",
                AdoGetterFormatter = "rdr.GetString({0})",
                CSharpType = "string",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.Equals
            },
            new DataTypeMapping()
            {
                MSSQLDataType = "xml",
                AdoSqlDbType = "SqlDbType.Xml",
                AdoGetterFormatter = "rdr.GetString({0})",
                CSharpType = "string",
                IsCSharpValueType = false,
                EqualityTestType = EqualityTestTypes.Equals
            }
        };
    }
    public class DataTypeMapping
    {
        public string AdoGetterFormatter { get; set; }
        public string AdoSqlDbType { get; set; }
        public string AdoUdtTypeName { set; get; }
        public string CSharpType { get; set; }
        public bool IsCSharpValueType { get; set; }
        public EqualityTestTypes EqualityTestType { set; get; }
        public string MSSQLDataType { get; set; }
        public int? DefaultSize { set; get; }
        public bool IsTableValueParameter { set; get; }
        
        public string Using { set; get; }

        public string EqualityTest
        {
            get
            {
                if (EqualityTestType == EqualityTestTypes.Equals)
                {
                    return "if ({0} != {1})";
                }
                if(EqualityTestType == EqualityTestTypes.ValuesAreEqual)
                {
                    return "if (Helpers.ValueIsChanged({0}, {1}))";
                }
                return "if (Helpers.VariantIsChanged({0}, {1}))";
            }
        }
    }
}
