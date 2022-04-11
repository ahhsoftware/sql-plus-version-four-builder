namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The InOut tag applies to parameters.
    /// When applied to a parameter in SQL the parameter is exclusive output.
    /// When applied to a parameter in a concrete query the parameter is exclusive output.
    /// Does not support any supplemental tags.
    /// Value is assigned to parameter vs a rendered annotation.
    /// </summary>
    public class Output : BaseTag
    {
        public Output(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.Output;
        }

        public override void SetSupplemental(string tagLine)
        {
            ThrowExceptionSupplementTagInvalidForTag(tagLine);
        }
    }
}
