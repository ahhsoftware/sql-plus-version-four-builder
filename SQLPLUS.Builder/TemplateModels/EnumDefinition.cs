using System.Collections.Generic;

namespace SQLPLUS.Builder.TemplateModels
{
    public class EnumDefinition
    {
        private string description;
        public string Name { set; get; }

        public string Value { set; get; }

        public string Comment
        {
            set
            {
                description = value;
            }
            get
            {
                return description ?? $"Enumerated value for {Name}";
            }
        }
    }

    public class EnumCollection
    {
        public string Name { set; get; }

        public string DataType { set; get; } = "int";

        public List<EnumDefinition> Enums { set; get; }
    }
}
