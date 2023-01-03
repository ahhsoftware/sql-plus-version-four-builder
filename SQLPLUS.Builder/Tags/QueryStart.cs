namespace SQLPLUS.Builder.Tags
{
    using SQLPLUS.Builder.TemplateModels;
    using System;

    /// <summary>
    /// The QueryStart tag is embedded in the SQL and marks the beginning of a query in a multiset query.
    /// </summary>
    public class QueryStart : BaseTag
    {
        public QueryStart(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            this.TagContext = TagContexts.Query;
            this.TagType = TagTypes.QueryStart;
        }

        public int StartIndex { set; get; }
        public int EndIndex { set; get; }
        public string Name { private set; get; }
        public SelectTypes SelectType { private set; get; }

        /// <summary>
        /// QueryStart=Name,SelectType
        /// </summary>
        /// <param name="tagLine">Tagline supplied</param>
        public override void SetPrimary(string tagLine)
        {
            Tuple<bool, string, string> result = Helpers.Text2AfterEqualsSign(tagLine);
            if (result.Item1 == false)
            {
                PrimaryTagValueException(2, tagLine);
            }

            Name = result.Item2;
            SelectType = Enumerations.GetSelectType(result.Item3.ToLower());
            if (SelectType == SelectTypes.EnumParseError || SelectType == SelectTypes.MultiSet || SelectType == SelectTypes.NonQuery)
            {
                PrimaryTagValueException(2, tagLine, $"Valid values include: {SelectTypes.Json}, {SelectTypes.MultiRow}, {SelectTypes.SingleRow}, {SelectTypes.Xml}");
            }
            if(SelectType == SelectTypes.MultiSet || SelectType == SelectTypes.NonQuery)
            {
                PrimaryTagValueException(2, tagLine);
            }
        }

        /// <summary>
        /// Since this tag does not support supplemental tags this throws and exception.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetSupplemental(string tagLine)
        {
            ThrowExceptionSupplementTagInvalidForTag(tagLine);
        }

        //TODO Use prefix from config
        public String QueryStartAsPrintLine()
        {
            return $"PRINT 'QueryStart={Name},{SelectType.ToString()}'";
        }

        public String QueryEndAsPrintLine()
        {
            return $"PRINT 'QueryEnd'";
        }


        public string QueryEndTag()
        {
            return $"--+QueryEnd";
        }

        public string QueryStartTag()
        {
            return $"--+QueryStart={Name},{SelectType.ToString()}'";
        }
    }
}
