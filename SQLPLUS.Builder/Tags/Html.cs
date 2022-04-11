namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The Html tag applies to parameters.
    /// Supports error message and error resource supplemental tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class Html : BaseTag
    {
        public Html(string primaryTagPrefix, string supplementalTagPrefix)
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
            this.TagType = TagTypes.Html;
        }

        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[DataType(DataType.Html, ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[DataType(DataType.Html, ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[DataType(DataType.Html)]";
        }
    }
}