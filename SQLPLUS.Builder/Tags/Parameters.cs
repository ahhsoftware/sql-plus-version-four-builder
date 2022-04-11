namespace SQLPLUS.Builder.Tags
{
    /// <summary>
    /// The parameters tag is used in concrete queries to indicate the beggining or end of the parameters section
    /// Does not support supplemental tags.
    /// No values are assigned.
    /// Overrides TagContext.
    /// </summary>
    public class Parameters : BaseTag
    {
        public Parameters(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new System.ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            TagContext = TagContexts.ParameterSection;
            this.TagType = TagTypes.Parameters;
        }

        /// <summary>
        /// Since this tag does not support supplemental tags this throws an exception.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetSupplemental(string tagLine)
        {
            ThrowExceptionSupplementTagInvalidForTag(tagLine);
        }
    }
}