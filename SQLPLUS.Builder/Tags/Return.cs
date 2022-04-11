namespace SQLPLUS.Builder.Tags
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The return tag can appear randomly in SQL.
    /// It captures enumerated values for returns.
    /// </summary>
    public class Return : BaseTag
    {
        public Return(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            TagContext = TagContexts.ReturnValue;
            this.TagType = TagTypes.Return;
        }

        /// <summary>
        /// Name for the enumerated value
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Value of enumerated value
        /// </summary>
        public string Description { private set; get; }

        public string Value{ private set; get; }

        /// <summary>
        /// --+Return=Name,Description*
        /// </summary>
        /// <param name="tagLine"></param>
        public override void SetPrimary(string tagLine)
        {
            Tuple<bool, int, string, string> result = Helpers.Text1Or2AfterEqualsSign(tagLine);
            if (result.Item1 == false)
            {
                PrimaryTagValueException(2, tagLine);
            }
            Name = result.Item3;
            if(result.Item2 == 2)
            {
                Description = result.Item4;
            }
        }

        /// <summary>
        /// Since this tag does not support supplemental tags this throws and exception.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetSupplemental(string tagLine)
        {
            Value = tagLine;
        }

        public override void Validate()
        {
            if(string.IsNullOrEmpty(Value))
            {
                throw new Exception($"Unable to locate the return value for a {this.Tag} tag.");
            }
        }
    }
}
