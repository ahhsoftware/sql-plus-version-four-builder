namespace SQLPLUS.Builder.Tags
{
    using System;
    using System.Text;

    /// <summary>
    /// The display tag applies to parameters.
    /// Supports shortname, prompt, resource, group, and order tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class Display : BaseTag
    {
        private readonly string shortNameTag;
        private readonly string promptTag;
        private readonly string resourceTag;
        private readonly string groupTag;
        private readonly string orderTag;

        public Display(string primaryTagPrefix, string supplementalTagPrefix = null)
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
            shortNameTag = $"{supplementalTagPrefix}ShortName";
            promptTag = $"{supplementalTagPrefix}Prompt";
            resourceTag = $"{supplementalTagPrefix}Resource";
            groupTag = $"{supplementalTagPrefix}Group";
            orderTag = $"{supplementalTagPrefix}Order";
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.Display;
        }

        /// <summary>
        /// This will be the actual value of the Name provided by the primary tagline.
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// This will be the actual value of the Description provided by the primary tagline.
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// This will be the actual value of the ShortName provided by the supplemental tagline.
        /// </summary>
        public string ShortName { set; get; }

        /// <summary>
        /// This will be the actual value of the Prompt provided by the supplemental tagline.
        /// </summary>
        public string Prompt { set; get; }

        /// <summary>
        /// This will be the actual value of the ResourceType provided by the supplemental tagline.
        /// </summary>
        public string ResourceType { set; get; }

        /// <summary>
        /// This will be the actual value of the GroupName provided by the supplemental tagline.
        /// </summary>
        public string GroupName { set; get; }

        /// <summary>
        /// This will be the actual value of the Order provided by the supplemental tagline.
        /// </summary>
        public int? Order { set; get; }

        /// <summary>
        /// Display=Name,Description* where description is optional.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetPrimary(string tagLine)
        {
            Tuple<bool, int, string, string> result = Helpers.Text1Or2AfterEqualsSign(tagLine);
            if (result.Item1 == false)
            {
                PrimaryTagValueException(1, tagLine);
            }
            Name = result.Item3;

            if (result.Item2 == 2)
            {
                Description = result.Item4;
            }
        }

        /// <summary>
        /// ShortName=Value
        /// Prompt=value
        /// Resource=value
        /// GroupName=value
        /// Order=value
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetSupplemental(string tagLine)
        {
            if (tagLine.StartsWith(shortNameTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    PrimaryTagValueException(1, tagLine);
                }
                ShortName = result.Item2;
            }
            else if (tagLine.StartsWith(promptTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    PrimaryTagValueException(1, tagLine);
                }
                Prompt = result.Item2;
            }
            else if (tagLine.StartsWith(resourceTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    PrimaryTagValueException(1, tagLine);
                }
                ResourceType = result.Item2;
            }
            else if (tagLine.StartsWith(groupTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    PrimaryTagValueException(1, tagLine);
                }
                GroupName = result.Item2;
            }
            else if (tagLine.StartsWith(orderTag))
            {
                Tuple<bool, int> result = Helpers.Int1ValueAfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    PrimaryTagValueException(1, tagLine);
                }
                Order = result.Item2;
            }
            else
            {
                ThrowExceptionSupplementTagInvalidForTag(tagLine);
            }
        }

        /// <summary>
        /// Builds Display annotation for property.
        /// </summary>
        /// <returns>Formatted annotation string.</returns>
        public override string GetAnnotation()
        {
            StringBuilder bldr = new StringBuilder();
            bldr.Append($"[Display(Name = \"{Name}\"");

            if (!string.IsNullOrEmpty(Description))
            {
                bldr.Append($", Description = \"{Description}\"");
            }
            if (!string.IsNullOrEmpty(ShortName))
            {
                bldr.Append($", ShortName = \"{ShortName}\"");
            }
            if (!string.IsNullOrEmpty(Prompt))
            {
                bldr.Append($", Prompt = \"{Prompt}\"");
            }
            if (!string.IsNullOrEmpty(GroupName))
            {
                bldr.Append($", GroupName = \"{GroupName}\"");
            }
            if (Order.HasValue)
            {
                bldr.Append($", Order = {Order.Value}");
            }
            if (!string.IsNullOrEmpty(ResourceType))
            {
                bldr.Append($", ResourceType = typeof({ResourceType})");
            }
            bldr.Append(")]");
            return bldr.ToString();
        }
    }
}
