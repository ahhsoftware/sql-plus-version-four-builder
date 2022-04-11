namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The ForceEncryption tag applies to parameters.
    /// Does not support any supplemental tags.
    /// Value is assigned to parameter vs a rendered annotation.
    /// </summary>
    public class ForceColumnEncryption : BaseTag
    {
        public ForceColumnEncryption(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.ForceColumnEncryption;
        }

        /// <summary>
        /// Since this tag does not support supplemental tags this throws and exception.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetSupplemental(string tagLine)
        {
            ThrowExceptionSupplementTagInvalidForTag(tagLine);
        }
    }
}
