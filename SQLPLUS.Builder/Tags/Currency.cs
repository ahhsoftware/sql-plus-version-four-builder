namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The currency tag applies to parameters.
    /// When present on a parameter, it will provide the Currency annnotation.
    /// Supports error message and error resource tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class Currency : BaseTag
    {
        public Currency(string primaryTagPrefix, string supplementalTagPrefix)
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
            this.TagType = TagTypes.Currency;
        }

        /// <summary>
        /// Builds Currency annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[DataType(DataType.Currency, ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[DataType(DataType.Currency, ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[DataType(DataType.Currency)]";
        }
    }
}
