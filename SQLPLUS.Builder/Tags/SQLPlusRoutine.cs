namespace SQLPLUS.Builder.Tags
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Routine provides additional information about a routine to the builder.
    /// </summary>
    public class SQLPlusRoutine : BaseTag
    {
        private readonly string authorTag;
        private readonly string cacheRegionTag;
        private readonly string commandTimeoutTag;
        private readonly string commentTag;
        private readonly string selectTypeTag;
        private readonly string obsoleteTag;

        public SQLPlusRoutine(string primaryTagPrefix, string supplementalTagPrefix) :
            base(primaryTagPrefix, supplementalTagPrefix)
        {
            authorTag = $"{supplementalTagPrefix}Author";
            cacheRegionTag = $"{supplementalTagPrefix}CacheRegion";
            commandTimeoutTag = $"{supplementalTagPrefix}CommandTimeout";
            commentTag = $"{supplementalTagPrefix}Comment";
            selectTypeTag = $"{supplementalTagPrefix}SelectType";
            obsoleteTag = $"{supplementalTagPrefix}ObsoleteTag";
            TagContext = TagContexts.Routine;
            TagType = TagTypes.SQLPlusRoutine;
        }

        #region Custom Supplemental Tags for the Routine Tag.

        /// <summary>
        /// The author of the routine
        /// </summary>
        public string Author { private set; get; }

        /// <summary>
        /// Allows setting a cache region for selectable queries.
        /// </summary>
        public string CacheRegion { set; get; }

        /// <summary>
        /// The command timeout maximum.
        /// </summary>
        public int? CommandTimeout { private set; get; }

        /// <summary>
        /// The comment for the routine.
        /// </summary>
        public string Comment { private set; get; }

        /// <summary>
        /// Utilized in the obsolete annotation.
        /// </summary>
        public string ObsoletMessage { private set; get; }

        /// <summary>
        /// Utilized in the obsolete annotation.
        /// </summary>
        public ObsoleteTypes ObsoleteType { private set; get; } = ObsoleteTypes.None;

        /// <summary>
        /// The select type for the routine.
        /// </summary>
        public SelectTypes SelectType { set; get; } = SelectTypes.EnumParseError;

        #endregion

        /// <summary>
        /// author=value*
        /// comment=value*
        /// obsolete=error|warning,message
        /// selecttype=value*
        /// commandtimeout=value
        /// </summary>
        /// <param name="tagLine"></param>
        public override void SetSupplemental(string tagLine)
        {
            if (tagLine.StartsWith(authorTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(1,tagLine);
                }
                Author = result.Item2;
                return;
            }
            else if (tagLine.StartsWith(commentTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(1, tagLine);
                }
                Comment = result.Item2;
                return;
            }
            else if (tagLine.StartsWith(obsoleteTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string, string> tuple = Helpers.Text2AfterEqualsSign(tagLine);
                if (tuple.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(1, tagLine);
                }
                else
                {
                    if (tuple.Item2.Equals("warning", StringComparison.OrdinalIgnoreCase))
                    {
                        ObsoleteType = ObsoleteTypes.Warning;
                    }
                    else if (tuple.Item2.Equals("error", StringComparison.OrdinalIgnoreCase))
                    {
                        ObsoleteType = ObsoleteTypes.Error;
                    }
                    else
                    {
                        ThrowExceptionSupplementalTagValueError(1, tagLine);
                    }
                    ObsoletMessage = tuple.Item3;
                }
                return;
            }
            else if (tagLine.StartsWith(selectTypeTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(1, tagLine);
                }
                else
                {
                    SelectType = Enumerations.GetSelectType(result.Item2.ToLower());
                    if (SelectType == SelectTypes.EnumParseError)
                    {
                        ThrowExceptionSupplementalTagValueError(1, tagLine);
                    }
                }
                return;
            }
            else if (tagLine.StartsWith(commandTimeoutTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, int> result = Helpers.Int1ValueAfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(1, tagLine);
                }
                CommandTimeout = result.Item2;
                return;
            }
            else if (tagLine.StartsWith(cacheRegionTag, StringComparison.OrdinalIgnoreCase))
            {
                Tuple<bool, string> result = Helpers.Text1AfterEqualsSign(tagLine);
                if (result.Item1 == false)
                {
                    ThrowExceptionSupplementalTagValueError(1, tagLine);
                }
                CacheRegion = result.Item2;
                return;
            }

            ThrowExceptionSupplementTagInvalidForTag(tagLine);
        }

        public List<string> Annotations
        {
            get
            {
                List<string> result = new List<string>();
                if (ObsoleteType == ObsoleteTypes.Error)
                {
                    result.Add($"[Obsolete({ObsoletMessage}, true)]");
                }

                if (ObsoleteType == ObsoleteTypes.Warning)
                {
                    result.Add($"[Obsolete({ObsoletMessage}, false)]");
                }
                return result;
            }
        }

        public override void Validate()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(Author))
            {
                errors.Add(nameof(Author));
            }
            if (string.IsNullOrEmpty(Comment))
            {
                errors.Add(nameof(Comment));
            }
            if(SelectType == SelectTypes.EnumParseError)
            {
                errors.Add("SelectType");
            }
            if (errors.Count != 0)
            {
                throw new Exception($"{Tag} is missing required supplemental tags: {string.Join(", ", errors)}");
            }
        }
    }
}
