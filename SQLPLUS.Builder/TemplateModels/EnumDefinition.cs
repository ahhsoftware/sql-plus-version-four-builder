using System.Collections.Generic;

namespace SQLPLUS.Builder.TemplateModels
{
    public class EnumDefinition
    {
        public string Name { set; get; }

        public string Value { set; get; }

        public string Description { set; get; }

        private string comment;
        public string Comment
        {
            set
            {
                comment = value;
            }
            get
            {
                if(string.IsNullOrEmpty(comment))
                {
                    return $"Enumerated value for {Name}";
                }
                return comment;
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
