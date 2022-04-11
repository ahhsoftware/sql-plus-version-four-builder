namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The maxlength tag applies to parameters.
    /// Supports error message and error resource supplemental tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class MaxLength : BaseTag
    {
        public MaxLength(string primaryTagPrefix, string supplementalTagPrefix)
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
            this.TagType = TagTypes.MaxLength;
        }

        public int? Value { set; get; }

        /// <summary>
        /// maxlength=value
        /// </summary>
        /// <param name="tagLine"></param>
        public override void SetPrimary(string tagLine)
        {
            Tuple<bool, int> result = Helpers.Int1ValueAfterEqualsSign(tagLine);
            if (result.Item1 == false)
            {
                PrimaryTagValueException(1, tagLine);
            }
            Value = result.Item2;
        }

        /// <summary>
        /// Builds MaxLength annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[MaxLength({Value}, ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[MaxLength({Value}, ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[MaxLength({Value})]";
        }
    }
}
