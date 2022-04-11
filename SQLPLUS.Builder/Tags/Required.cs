namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The required tag applies to parameters.
    /// When present on a parameter, it will overide the default value of the Comment.
    /// Does not support supplemental tags.
    /// </summary>
    public class Required : BaseTag
    {
        private readonly string allowEmptyStringsTag;
        public Required(string primaryTagPrefix, string supplementalTagPrefix = null)
            : base(primaryTagPrefix, supplementalTagPrefix)
        {
            allowEmptyStringsTag = $"{supplementalTagPrefix}allowEmptyStrings";
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.Required;
        }

        /// <summary>
        /// AllowEmptyStrings=true|false.
        /// </summary>
        /// <param name="tagLine"></param>
        public override void SetSupplemental(string tagLine)
        {
            if (tagLine.StartsWith(allowEmptyStringsTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, bool> result = Helpers.BoolValueAfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(1, tagLine);
                }
                AllowEmptyStrings = result.Item2;
            }
            else
            {
                base.SetSupplemental(tagLine);
            }
        }

        public bool AllowEmptyStrings { set; get; }

        /// <summary>
        /// Builds Currency annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {

            string allowEmptyStrings = $"AllowEmptyStrings = {AllowEmptyStrings.ToString().ToLower()}";

            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[Required({allowEmptyStrings}, ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[Required({allowEmptyStrings}, ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[Required({allowEmptyStrings})]";
        }
    }
}
