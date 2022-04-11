namespace SQLPLUS.Builder
{
    using System;

    public static class Enumerations
    {
        public static SelectTypes GetSelectType(string selectType)
        {
            if (!string.IsNullOrEmpty(selectType))
            {
                switch (selectType.ToLower())
                {
                    case "multiset":
                        return SelectTypes.MultiSet;
                    case "nonquery":
                        return SelectTypes.NonQuery;
                    case "singlerow":
                        return SelectTypes.SingleRow;
                    case "multirow":
                        return SelectTypes.MultiRow;
                    case "json":
                        return SelectTypes.Json;
                    case "xml":
                        return SelectTypes.Xml;
                }
            }
            return SelectTypes.EnumParseError;
        }
    }
    public enum ObsoleteTypes
    {
        None = 1,
        Error = 2,
        Warning = 3
    }

    /// <summary>
    /// These are the SQL Plus Select types.
    /// Found in the routine supplemental (SelectType).
    /// Found in the QueryStart tag.
    /// </summary>
    public enum SelectTypes
    {
        /// <summary>
        /// Unable to parse the enum.
        /// </summary>
        EnumParseError = 0,

        /// <summary>
        /// Identifies the routine as a non querying type.
        /// </summary>
        NonQuery = 1,

        /// <summary>
        /// Single result set from single query.
        /// </summary>
        SingleRow = 2,

        /// <summary>
        /// Multiple result sets from a single query.
        /// </summary>
        MultiRow = 3,

        /// <summary>
        /// Single or multiple rows in json format.
        /// </summary>
        Json = 4,

        /// <summary>
        /// Single or multiple rows in xml format. 
        /// </summary>
        Xml = 5,

        /// <summary>
        /// Has multiple results sets.
        /// When this is the case, query start and end tags must be present.
        /// </summary>
        MultiSet = 6
    }

    /// <summary>
    /// Enums for parsing.
    /// </summary>
    public enum LineTypes
    {
        /// <summary>
        /// The line is an empty string.
        /// </summary>
        EmptyString,
        
        /// <summary>
        /// The line starts with the primary tag prefix.
        /// </summary>
        PrimaryTag,

        /// <summary>
        /// The line starts with the supplemental tag prefix.
        /// </summary>
        SupplementalTag,

        /// <summary>
        /// The line starts with the exlicit tag prefix.
        /// </summary>
        ExplicitTag,

        /// <summary>
        /// The line is a comment line
        /// </summary>
        CommentLine,

        /// <summary>
        /// Indicates the start of a comment block.
        /// </summary>
        CommentBlockOpen,

        /// <summary>
        /// Indicates the end of a comment block.
        /// </summary>
        CommentBlockClose,

        /// <summary>
        /// The line is part of the SQL text of procedure text.
        /// </summary>
        BodyText,

        /// <summary>
        /// The line is a parameter start or end tag
        /// </summary>
        ParameterTag
    }

    /// <summary>
    /// Identifies the context or application of the tag.
    /// </summary>
    public enum TagContexts
    {
        /// <summary>
        /// SQL plus routine tag.
        /// </summary>
        Routine,

        /// <summary>
        /// One of the parameter annotation tags.
        /// </summary>
        ParameterAnnotation,

        /// <summary>
        /// A return value tag
        /// </summary>
        ReturnValue,

        /// <summary>
        /// Query start or query end.
        /// </summary>
        Query,

        /// <summary>
        /// Start or end of parameters section.
        /// </summary>
        ParameterSection,

        /// <summary>
        /// This specialty tag allows selective multiset results.
        /// </summary>
        Ignore
    }

    /// <summary>
    /// Identifies the exact tag type. Each tag is aware of it's type and has a tag type property.
    /// </summary>
    public enum TagTypes
    {
        CacheKey,
        Comment,
        CreditCard,
        Currency,
        Default,
        Display,
        Email,
        Enum,
        Explicit,
        ForceColumnEncryption,
        Html,
        Ignore,
        InOut,
        Input,
        MaxLength,
        MinLength,
        Output,
        Parameters,
        Password,
        Phone,
        PostalCode,
        QueryEnd,
        QueryStart,
        Range,
        RegExPattern,
        Required,
        Return,
        SQLPlusRoutine,
        StringLength,
        Url
    }

    /// <summary>
    /// When searching through lines of text this indicates the search type.
    /// The text casing management is the responsibility of the 
    /// </summary>
    public enum IndexOfTypes
    {
        /// <summary>
        /// The line is an exact match, 
        /// </summary>
        Exact,

        /// <summary>
        /// The line starts with the search text.
        /// </summary>
        StartsWith,

        /// <summary>
        /// The line contains the text.
        /// </summary>
        Contains,

        /// <summary>
        /// The line ends with the text.
        /// </summary>
        EndsWith
    }
    public enum EqualityTestTypes
    {
        Equals = 0,
        ValuesAreEqual = 1
    }
}
