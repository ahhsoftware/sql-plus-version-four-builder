namespace SQLPLUS.Builder.Tags
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for tags.
    /// 
    /// </summary>
    public abstract class BaseTag
    {
        protected string primaryTagPrefix;
        protected string supplementalTagPrefix;
        
        public BaseTag(string primaryTagPrefix, string supplementalTagPrefix)
        {
            this.primaryTagPrefix = primaryTagPrefix;
            this.supplementalTagPrefix = supplementalTagPrefix;
            this.Tag = $"{primaryTagPrefix}{this.GetType().Name}";
            this.ErrorMessageTag = $"{supplementalTagPrefix}ErrorMessage";
            this.ErrorResourceTag = $"{supplementalTagPrefix}ErrorResource";
        }

        /// <summary>
        /// Convenient way to safe identify tags.
        /// </summary>
        public TagTypes TagType { protected set; get; }


        /// <summary>
        /// This is the combination of the primary tag prefix and the class name
        /// Made this protected to allow overriding the contructor set value.
        /// </summary>
        public string Tag { protected set; get; }

        /// <summary>
        /// Identifies the context or type of tag.
        /// </summary>
        public TagContexts TagContext { protected set; get; }
        
        #region Applies to parameter tags

        /// <summary>
        /// Supplemental tag ErrorMessage = value.
        /// </summary>
        protected string ErrorMessageTag { private set; get; }

        /// <summary>
        /// Supplemental tag ErrorResource = Resource, Key.
        /// </summary>
        protected string ErrorResourceTag { private set; get; }

        /// <summary>
        /// Name of ErrorResource supplied by ErrorResourceTag.
        /// </summary>
        public string ResourceName { set; get; }

        /// <summary>
        /// Name of Resource Name supplied by ErrorResourceTag.
        /// </summary>
        public string ResourceKey { set; get; }

        /// <summary>
        /// Message supplied by ErrorMessageTag.
        /// </summary>
        public string ErrorMessage { set; get; }

        #endregion

        /// <summary>
        /// The tag factory is responsible for instantiating all tags and should only call this method in tags that support parameters.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public virtual void SetPrimary(string tagLine)
        {
            throw new Exception($"Tag factory called {nameof(SetPrimary)} in error.");
        }

        /// <summary>
        /// Since most tags support the error resource and error message tags, this is the default implementation.
        /// In classes that support those tags check for native values first then call base.
        /// For classes that do not support those do not call base.
        /// </summary>
        /// <param name="tagLine">Tagline provided.</param>
        public virtual void SetSupplemental(string tagLine)
        {
            if (tagLine.StartsWith(ErrorMessageTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(1, tagLine);
                }
                ErrorMessage = result.Item2;
            }
            else if (tagLine.StartsWith(ErrorResourceTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string, string> result = Helpers.Text2AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(2, tagLine);
                }
                ResourceName = result.Item2;
                ResourceKey = result.Item3;
            }
            else
            {
                ThrowExceptionSupplementTagInvalidForTag(tagLine);
            }
        }

        /// <summary>
        /// Override this in any class where multiple supplemental tags are requuired.
        /// </summary>
        public virtual void Validate() { }


        /// <summary>
        /// For any tag that renders an annotation override this.
        /// </summary>
        public virtual string GetAnnotation() { return null; }


        #region Exceptions

        //keep
        protected void PrimaryTagValueException(int numberOfValues, string tagLine, string validValues = null)
        {
            if (numberOfValues == 1)
            {
                throw new Exception($"Error near {tagLine}{Environment.NewLine}  The {Tag} tag requires a value that was not supplied or is not valid. Edit the {Tag} tag in the SQL. {validValues}");
            }
            throw new Exception($"Error near {tagLine}{Environment.NewLine}  The {Tag} requires {numberOfValues} values that were not supplied or are not valid. Edit the {Tag} tag in the SQL. {validValues}");

        }

        //keep
        protected void ThrowExceptionSupplementalTagValueError(int numberOfValues, string tagLine)
        {
            if(numberOfValues == 1)
            {
                throw new Exception($"Error near {tagLine}{Environment.NewLine}  The {Tag} has a supplemental tag with a missing or invalid value. Supply the appropriate value or remove the tag.");
            }
            throw new Exception($"Error near {tagLine}{Environment.NewLine}  The {Tag} has a supplemental tag with a missing or invalid values. Supply the appropriate values or remove the tag.");
        }

        //keep
        protected void ThrowExceptionSupplementTagInvalidForTag(string tagLine)
        {
            throw new Exception($"Error near {tagLine}{Environment.NewLine}  The {Tag} does not support the supplemental tag supplied. Remove or change the supplemental to one that is supported.");
        }

        #endregion
    }
}
