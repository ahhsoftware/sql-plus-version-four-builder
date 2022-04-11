namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The InOut tag applies to parameters.
    /// When applied to a parameter in SQL the parameter will be both an input and output.
    /// * Note that in the above case the parameter direction in the database must be in out - marked out *
    /// Does not support any supplemental tags.
    /// Value is assigned to parameter vs a rendered annotation.
    /// </summary>
    public class InOut : BaseTag
    {
        public InOut(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.InOut;
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
