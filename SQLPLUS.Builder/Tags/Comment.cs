namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The comment tag applies to parameters.
    /// When present on a parameter, it will overide the default value of the Comment.
    /// Does not support supplemental tags.
    /// </summary>
    public class Comment : BaseTag
    {
        public Comment(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.Comment;
        }

        /// <summary>
        /// This will be the actual value of the comment provided by the tagline.
        /// </summary>
        public string Value { private set; get; }

        /// <summary>
        /// Comment=value
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
        /// Since this tag does not support supplemental tags this throws an exception.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetSupplemental(string tagLine)
        {
            ThrowExceptionSupplementTagInvalidForTag(tagLine);
        }
    }
}
