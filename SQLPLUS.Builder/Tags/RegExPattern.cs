using System;
using System.Collections.Generic;

namespace SQLPLUS.Builder.Tags
{
    public class RegExPattern : BaseTag
    {
        public RegExPattern(string primaryTagPrefix, string supplementalTagPrefix = null)
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
            this.TagType = TagTypes.RegExPattern;
        }

        /// <summary>
        /// This will be the regular expression.
        /// </summary>
        public string Value { set; get; }

        /// <summary>
        /// RegExPattern=RegularExpression
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetPrimary(string tagLine)
        {
            Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
            if (result.Item1 == false)
            {
                PrimaryTagValueException(1, tagLine);
            }
            Value = result.Item2;
        }

        /// <summary>
        /// Builds RegularExpression annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[RegularExpression(@\"{Value}\", ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[RegularExpression(@\"{Value}\", ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[RegularExpression(@\"{Value}\")]";
        }
    }
}
