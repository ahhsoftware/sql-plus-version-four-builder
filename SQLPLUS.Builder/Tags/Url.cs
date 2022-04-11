namespace SQLPLUS.Builder.Tags
{
    using System;

    public class Url : BaseTag
    {
        public Url(string primaryTagPrefix, string supplementalTagPrefix = null)
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
            this.TagType = TagTypes.Url;
        }

        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[Url(ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[Url(ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[Url]";
        }
        public override void Validate()
        {
            return;
        }
    }
}
