namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The enum tag applies to parameters.
    /// Does not support any supplemental tags.
    /// Value is assigned to parameter vs a rendered annotation.
    /// </summary>
    public class Enum : BaseTag
    {
        public Enum(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.Enum;
        }

        /// <summary>
        /// This will be the name for the enum.
        /// </summary>
        public string Value { set; get; }

        /// <summary>
        /// enum=EnumerationName
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetPrimary(string tagLine)
        {
            Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
            if (result.Item1 == false)
            {
                PrimaryTagValueException(1, tagLine);
            }
            Value = result.Item2;
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
