namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The string length tag applies to parameters.
    /// Supports error message and error resource supplemental tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class StringLength : BaseTag
    {
        public StringLength(string primaryTagPrefix, string supplementalTagPrefix = null)
            : base(primaryTagPrefix, supplementalTagPrefix)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            if (string.IsNullOrEmpty(supplementalTagPrefix))
            {
                throw new ArgumentException($"'{nameof(supplementalTagPrefix)}' cannot be null or empty.", nameof(supplementalTagPrefix));
            }
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.StringLength;
        }

        /// <summary>
        /// Minimum Length of the string.
        /// </summary>
        public int? MinLength { set; get; }

        /// <summary>
        /// Maximum Length of the string.
        /// </summary>
        public int? MaxLength { set; get; }

        /// <summary>
        /// StringLength=MinimumLength,MaximumLength
        /// </summary>
        /// <param name="tagLine"></param>
        public override void SetPrimary(string tagLine)
        {
            Tuple<bool, int, int> result = Helpers.Int2ValueAfterEqualsSign(tagLine);
            if (result.Item1 == false)
            {
                PrimaryTagValueException(2, tagLine);
            }
            MinLength = result.Item2;
            MaxLength = result.Item3;
        }

        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[StringLength({MaxLength}, MinimumLength = {MinLength}, ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[StringLength({MaxLength}, MinimumLength = {MinLength}, ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[StringLength({MaxLength}, MinimumLength = {MinLength})]";
        }
    }
}
