namespace SQLPLUS.Builder.Tags
{
    using System;

    /// <summary>
    /// The range tag applies to parameters.
    /// Supports error message and error resource tags.
    /// Overrides GetAnnotation.
    /// </summary>
    public class Range : BaseTag
    {
        public Range(string primaryTagPrefix, string supplementalTagPrefix = null) 
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
            this.TagType = TagTypes.Range;
        }

        /// <summary>
        /// Minimun value of range
        /// </summary>
        public string MinValue { private set; get; }

        /// <summary>
        /// Maximum Value of range
        /// </summary>
        public string MaxValue { private set; get; }

        /// <summary>
        /// This value needs to be set form the parameter definition.
        /// </summary>
        public string DataType { set; get; }

        public override void SetPrimary(string tagLine)
        {
            if (tagLine.StartsWith(Tag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string, string> result = Helpers.Text2AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    PrimaryTagValueException(2, tagLine);
                }
                MinValue = result.Item2;
                MaxValue = result.Item3;
            }
        }
        public override string GetAnnotation()
        {
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return $"[Range(typeof({DataType}), \"{MinValue}\", \"{MaxValue}\", ErrorMessageResourceName = \"{ResourceKey}\", ErrorMessageResourceType = typeof({ResourceName}))]";
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"[Range(typeof({DataType}), \"{MinValue}\", \"{MaxValue}\", ErrorMessage = \"{ErrorMessage}\")]";
            }
            return $"[Range(typeof({DataType}), \"{MinValue}\", \"{MaxValue}\")]";
        }

        public override void Validate()
        {
            if(string.IsNullOrEmpty(DataType))
            {
                throw new Exception("Data type not set");
            }
        }
    }
}
