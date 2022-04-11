namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The credit card tag applies to parameters.
    /// When present on a parameter, it will provide the CreditCard annnotation.
    /// Supports error message and error resource supplemental tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class CreditCard : BaseTag
    {
        public CreditCard(string primaryTagPrefix, string supplementalTagPrefix)
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
            this.TagType = TagTypes.CreditCard;
        }

        /// <summary>
        /// Builds CreditCard annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[CreditCard( ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[CreditCard( ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[CreditCard]";
        }
    }
}