using System.Collections.Generic;

namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The password tag applies to parameters.
    /// Supports error message and error resource supplemental tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class Password : BaseTag
    {
        public Password(string primaryTagPrefix, string supplementalTagPrefix = null)
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
            this.TagType = TagTypes.Password;
        }

        /// <summary>
        /// Builds Password annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[DataType(DataType.Password, ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[DataType(DataType.Password, ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[DataType(DataType.Password)]";
        }
    }
}
