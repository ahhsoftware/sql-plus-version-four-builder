namespace SQLPLUS.Builder.Tags
{
    #region Usings

    using System;

    #endregion

    public class CacheKey : BaseTag
    {
        public CacheKey(string primaryTagPrefix)
            : base(primaryTagPrefix, null)
        {
            if (string.IsNullOrEmpty(primaryTagPrefix))
            {
                throw new ArgumentException($"'{nameof(primaryTagPrefix)}' cannot be null or empty.", nameof(primaryTagPrefix));
            }
            this.TagContext = TagContexts.ParameterAnnotation;
            this.TagType = TagTypes.CacheKey;
        }

        /// <summary>
        /// This will be the actual value of the comment provided by the tagline.
        /// </summary>
        public int Order { private set; get; }

        /// <summary>
        /// CacheKey=order
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public override void SetPrimary(string tagLine)
        {
            Tuple<bool, int> result = Helpers.Int1ValueAfterEqualsSign(tagLine);
            if (result.Item1 == false)
            {
                PrimaryTagValueException(1, tagLine);
            }
            Order = result.Item2;
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
