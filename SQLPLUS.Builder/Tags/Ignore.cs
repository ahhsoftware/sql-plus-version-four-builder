namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The Ignore tag applies to routine text and allows the generation of selective multiset queries.
    /// Lines identified with the ignore tag will be commented out during the builds.
    /// Does not support supplemental tags.
    /// </summary>
    public class Ignore : BaseTag
    {
        public Ignore(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            this.TagContext = TagContexts.Ignore;
            this.TagType = TagTypes.Ignore;
        }

        /// <summary>
        /// Index of the line marked with the ignore tag
        /// </summary>
        public int Index { set; get; }

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
