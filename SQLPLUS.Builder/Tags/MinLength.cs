namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The minlength tag applies to parameters.
    /// Supports error message and error resource supplemental tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class MinLength : BaseTag
    {
        public MinLength(string primaryTagPrefix, string supplementalTagPrefix = null)
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
            this.TagType = TagTypes.MinLength;
        }

        public int? Value { set; get; }

        /// <summary>
        /// minlength=value
        /// </summary>
        /// <param name="tagLine">Tagline supplied</param>
        public override void SetPrimary(string tagLine)
        {
            if (tagLine.StartsWith(Tag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, int> result = Helpers.Int1ValueAfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    PrimaryTagValueException(1, tagLine);
                }
                Value = result.Item2;
            }
        }

        /// <summary>
        /// Builds MinLength annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[MinLength({Value}, ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[MinLength({Value}, ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[MinLength({Value})]";
        }
    }
}