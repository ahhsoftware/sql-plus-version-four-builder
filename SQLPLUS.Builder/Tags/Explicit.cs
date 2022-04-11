namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The explicit tag applies to parameters and recieves a value for a custom annotation.
    /// Does not support any supplemental tags.
    /// Overrides GetAnnotation.
    /// *Note that this is available as # tag or Explicit tag.
    /// </summary>
    public class Explicit : BaseTag
    {
        private readonly string explicitTagPrefix;

        public Explicit(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            explicitTagPrefix = $"{primaryTagPrefix}#";
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.Explicit;
        }

        /// <summary>
        /// This will be the actual value of the annotation text provided by the tagline.
        /// </summary>
        public string Value { set; get; }

        public override void SetPrimary(string tagLine)
        {
            string tempLine = tagLine;
            if (tempLine.StartsWith(explicitTagPrefix))
            {
                tempLine = tempLine.Replace(explicitTagPrefix, $"{primaryTagPrefix}{nameof(Explicit)}");
            }
            if (tagLine.StartsWith(Tag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    PrimaryTagValueException(1, tagLine);
                }
                Value = result.Item2;
            }
        }

        /// <summary>
        /// Supplies the annotation text that was supplied.
        /// </summary>
        /// <returns>Annotation string.</returns>
        public override string GetAnnotation()
        {
            return Value;
        }

        /// <summary>
        /// Since this tag does not support supplemental tags this throws and exception.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetSupplemental(string tagLine)
        {
            ThrowExceptionSupplementTagInvalidForTag(tagLine);
        }
    }
}
