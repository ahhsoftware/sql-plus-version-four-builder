namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The phone tag applies to parameters.
    /// Supports error message and error resource tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class Phone : BaseTag
    {
        public Phone(string primaryTagPrefix, string supplementalTagPrefix)
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
            this.TagType = TagTypes.Phone;
        }

        /// <summary>
        /// Builds annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[Phone( ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[Phone( ErrorMessage = \"{ErrorMessage}\")]";

            }
            return $"[Phone]";
        }
    }
}
