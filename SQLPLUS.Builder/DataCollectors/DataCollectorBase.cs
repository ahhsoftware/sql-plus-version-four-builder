namespace SQLPLUS.Builder.DataCollectors
{
    using SQLPLUS.Builder.ConfigurationModels;
    using SQLPLUS.Builder.Mappings;
    using SQLPLUS.Builder.Tags;
    using SQLPLUS.Builder.TemplateModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public abstract class DataCollectorBase : IDataCollector
    {
        protected readonly BuildDefinition build;
        protected readonly DatabaseConnection databaseConnection;
        protected readonly ProjectInformation project;
        protected readonly TagFactory tagFactory;

        /// <summary>
        /// Creates an instance of DataCollectorBase populated with required objects. Objects are available in derived classes by the names of the parameters.
        /// </summary>
        /// <param name="build">BuildDefinition instance.</param>
        /// <param name="connection">DatabaseConnection instance.</param>
        /// <param name="project">ProjectInformation instance.</param>
        public DataCollectorBase(BuildDefinition build, DatabaseConnection connection, ProjectInformation project)
        {
            this.build = build;
            this.databaseConnection = connection;
            this.project = project;
            this.tagFactory = new TagFactory(build);
        }

        #region Helper Functions

        /// <summary>
        /// Specifies how to collect data, for configuration vs for build;
        /// </summary>
        public DataCollectorModes DataCollectorMode { set; get; } = DataCollectorModes.Build;

        /// <summary>
        /// Determines the type of line (One of the LineTypes enum).
        /// </summary>
        /// <param name="line">Line of text to evaluate.</param>
        /// <returns>LineTypes enumeration.</returns>
        protected LineTypes LineType(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return LineTypes.EmptyString;
            }
            if(line.StartsWith(tagFactory.ParametersTag, StringComparison.OrdinalIgnoreCase))
            {
                return LineTypes.ParameterTag;
            }
            if (line.StartsWith(build.PrimaryTagPrefix))
            {
                return LineTypes.PrimaryTag;
            }
            if (line.StartsWith(build.SupplementalTagPrefix))
            {
                return LineTypes.SupplementalTag;
            }
            if (line.StartsWith(build.ExplicitTagPrefix))
            {
                return LineTypes.ExplicitTag;
            }
            if (line.StartsWith(build.SingleLineComment))
            {
                return LineTypes.CommentLine;
            }
            if (line.StartsWith(build.CommentBlockOpen))
            {
                if (line.EndsWith(build.CommentBlockClose))
                {
                    return LineTypes.CommentLine;
                }
                return LineTypes.CommentBlockOpen;
            }
            if (line.EndsWith(build.CommentBlockClose))
            {
                return LineTypes.CommentBlockClose;
            }
            return LineTypes.BodyText;
        }

        /// <summary>
        /// Finds the index of the first occurence of a match.
        /// </summary>
        /// <param name="lines">Line text that has been cleansed.</param>
        /// <param name="value">Value being searched.</param>
        /// <param name="indexOfType">One of the index of types enum. Determines the type of match.</param>
        /// <param name="startIndex">What line index to use as a starting point.</param>
        /// <returns>Returns actual index or -1 if not found.</returns>
        protected int IndexOf(string[] lines, string value, IndexOfTypes indexOfType, int startIndex)
        {
            int idx = startIndex;
            int count = lines.Length;
            while (idx != count)
            {
                string lineLowered = lines[idx].ToLower();
                string valueLowered = value.ToLower();

                switch (indexOfType)
                {
                    case IndexOfTypes.Contains:
                        if (lineLowered.Contains(valueLowered))
                        {
                            return idx;
                        }
                        break;
                    case IndexOfTypes.Exact:
                        if (lineLowered == valueLowered)
                        {
                            return idx;
                        }
                        break;
                    case IndexOfTypes.StartsWith:
                        if (lineLowered.StartsWith(valueLowered))
                        {
                            return idx;
                        }
                        break;
                    case IndexOfTypes.EndsWith:
                        if(lineLowered.EndsWith(valueLowered))
                        {
                            return idx;
                        }
                        break;
                }
                idx++;
            }

            return -1;
        }

        /// <summary>
        /// Replaces the first occurence of search within text.
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <param name="search">Value to search for.</param>
        /// <param name="replace">Value to replace search text with.</param>
        /// <returns>String with replace value.</returns>
        protected string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search, 0, StringComparison.OrdinalIgnoreCase);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        /// Takes the raw text and breaks into lines removing non essential comments.
        /// This is the first part of parsing for tags.
        /// </summary>
        /// <param name="rawText">Body of procedure or concrete query</param>
        /// <returns>Raw text lines.</returns>
        protected string[] GetRoutineLinesFromText(string rawText)
        {
            string[] rawLines = rawText.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            bool commentBlock = false;

            for (int idx = 0; idx != rawLines.Length; idx++)
            {
                string line = rawLines[idx].Trim();
                LineTypes lineType = LineType(line);
                if (lineType == LineTypes.EmptyString || lineType == LineTypes.CommentLine)
                {
                    continue;
                }
                if (commentBlock)
                {
                    if (lineType == LineTypes.CommentBlockClose)
                    {
                        commentBlock = false;
                    }
                    continue;
                }
                if (lineType == LineTypes.CommentBlockOpen)
                {
                    commentBlock = true;
                    continue;
                }

                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Takes the routine lines and creates a copy, commenting out the routine tag to prevent any temp routines from being picked up by the builder.
        /// This is utilized when generating multi set services, where temporary routines are created to get results sets.
        /// </summary>
        /// <param name="routine">Instance of Routine with populated routine lines.</param>
        /// <returns>Copy of the routine.RoutineLines.</returns>
        protected string[] GetRoutineLinesSafeCopy(string[] lines)
        {
            string[] temp = new string[lines.Length];
            lines.CopyTo(temp, 0);
            string replacedLine = $"{build.SingleLineComment}ReplacedRoutineTag";
            int firstIdx = IndexOf(temp, tagFactory.RoutineTag, IndexOfTypes.Exact, 0);
            int lastIdx = IndexOf(temp, tagFactory.RoutineTag, IndexOfTypes.Exact, firstIdx + 1);
            temp[firstIdx] = replacedLine;
            temp[lastIdx] = replacedLine;
            return temp;
        }

        /// <summary>
        /// Converst the string array into a single block of text.
        /// </summary>
        protected string StringArrayToString(string[] array)
        {
            StringBuilder bldr = new StringBuilder();
            for (int idx = 0; idx != array.Length; idx++)
            {
                bldr.AppendLine(array[idx]);
            }
            return bldr.ToString();
        }

        /// <summary>
        /// Comments out all lines that have the trailing ignore tag.
        /// </summary>
        /// /// <param name="lines">Cleansed SQL text.</param>
        /// <param name="ignores">List of Ignore tags.</param>
        protected void CommentOutIgnores(string[] lines, List<Ignore> ignores)
        {
            if (ignores != null)
            {
                foreach (Ignore ignore in ignores)
                {
                    lines[ignore.Index] = build.SingleLineComment + lines[ignore.Index];
                }
            }
        }

        /// <summary>
        /// Comments out the query text between the start and end index identified by the query.
        /// Changes the query start and query end lines to print statements to avoid and errors.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="query"></param>
        protected void CommentOutQueryLines(string[] lines, QueryStart query)
        {
            lines[query.StartIndex] = query.QueryStartAsPrintLine();

            for (int idx = query.StartIndex + 1; idx != query.EndIndex; idx++)
            {
                lines[idx] = $"--{lines[idx]}";
            }
            lines[query.EndIndex] = query.QueryEndAsPrintLine();
        }

        protected void ReverseCommentOutQueryLines(string[] lines, QueryStart query)
        {
            lines[query.StartIndex] = query.QueryStartTag();
            for (int idx = query.StartIndex + 1; idx != query.EndIndex; idx++)
            {
                lines[idx] = lines[idx].Substring(2);
            }
            lines[query.EndIndex] = query.QueryEndTag();
        }

        #endregion

        protected string[] CleansedRoutineLines(string[] lines)
        {
            List<string> temp = new List<string>();
            bool commentBlock = false;
            bool parameterBlock = false;

            for( int idx = 0; idx!= lines.Length; idx++)
            {
                string line = lines[idx];
                LineTypes lineType = LineType(line);
                if(parameterBlock)
                {
                    if(lineType == LineTypes.ParameterTag)
                    {
                        parameterBlock = false;
                    }
                    continue;
                }
                if(commentBlock)
                {
                    if(lineType == LineTypes.CommentBlockClose)
                    {
                        commentBlock = false;
                    }
                    continue;
                }
                else
                {
                    if (lineType == LineTypes.CommentBlockOpen)
                    {
                        commentBlock = true;
                        continue;
                    }
                    if( lineType == LineTypes.ParameterTag)
                    {
                        parameterBlock = true;
                        continue;
                    }
                    if (lineType == LineTypes.PrimaryTag || lineType == LineTypes.SupplementalTag || lineType == LineTypes.CommentLine)
                    {
                        continue;
                    }
                }
                temp.Add(line);
            }
            return temp.ToArray();
        }

        protected string[] CleansedRoutineLinesForResultSetQuery(string[] lines)
        {
            List<string> temp = new List<string>();
            bool commentBlock = false;
           
            for (int idx = 0; idx != lines.Length; idx++)
            {
                string line = lines[idx];
                LineTypes lineType = LineType(line);
                if (commentBlock)
                {
                    if (lineType == LineTypes.CommentBlockClose)
                    {
                        commentBlock = false;
                    }
                    continue;
                }
                else
                {
                    if (lineType == LineTypes.CommentBlockOpen)
                    {
                        commentBlock = true;
                        continue;
                    }
                    if (lineType == LineTypes.PrimaryTag || lineType == LineTypes.SupplementalTag || lineType == LineTypes.CommentLine || lineType == LineTypes.ParameterTag)
                    {
                        continue;
                    }
                }
                temp.Add(line);
            }
            return temp.ToArray();
        }


        #region Extract Methods

        /// <summary>
        /// Extracts the SQLPlusRoutine tag.
        /// </summary>
        /// <param name="lines">The text of a routine or concrete query - post empty lines removed and lines trimmed.</param>
        /// <returns>SQLPlusRoutine.</returns>
        protected SQLPlusRoutine ExtractSQLPlusRoutineTag(string[] lines)
        {
            int idx = 0;
            string line = lines[idx];
            if (!line.Equals(tagFactory.RoutineTag, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Expecting Tag {tagFactory.RoutineTag}. The SQL+ routine tag must be the first line in a SQL+ routine.");
            }

            SQLPlusRoutine routineTag = (SQLPlusRoutine)tagFactory.TagFromLine(line);

            idx++;
            while (idx != lines.Length)
            {
                line = lines[idx];
                LineTypes lineType = LineType(line);
                if (lineType == LineTypes.SupplementalTag)
                {
                    routineTag.SetSupplemental(line);
                    idx++;
                    continue;
                }
                if (line.Equals(tagFactory.RoutineTag, StringComparison.OrdinalIgnoreCase))
                {
                    routineTag.Validate();
                }
                break;
            }
            return routineTag;
        }

        /// <summary>
        /// Extracts the QueryStart tags.
        /// </summary>
        /// <param name="lines">The text of a routine or concrete query - post empty lines removed and lines trimmed.</param>
        /// <returns>List of QueryStart with potential of a zero count.</returns>
        protected List<QueryStart> ExtractQueryTags(string[] lines)
        {
            List<QueryStart> result = new List<QueryStart>();
            int startIndex = 0, endIndex = 0;
            do
            {
                startIndex = IndexOf(lines, tagFactory.QueryStartTag, IndexOfTypes.StartsWith, startIndex);
                if (startIndex == -1)
                {
                    break;
                }

                endIndex = IndexOf(lines, tagFactory.QueryEndTag, IndexOfTypes.Exact, startIndex + 1);
                if (endIndex == -1)
                {
                    throw new Exception("Missing Query End tag.");
                }

                QueryStart queryStart = (QueryStart)tagFactory.TagFromLine(lines[startIndex]);
                queryStart.StartIndex = startIndex;
                queryStart.EndIndex = endIndex;
                result.Add(queryStart);
                startIndex++;

            } while (true);

            return result;
        }

        /// <summary>
        /// Extracts the Ignore tags.
        /// </summary>
        /// <param name="lines">The text of a routine or concrete query - post empty lines removed and lines trimmed.</param>
        /// <returns>List of Ignore with potential of a zero count.</returns>
        protected List<Ignore> ExtractIgnoreTags(string[] lines)
        {
            List<Ignore> ignores = new List<Ignore>();
            int idx = 0;
            do
            {
                idx = IndexOf(lines, tagFactory.IgnoreTag, IndexOfTypes.EndsWith, idx);
                if (idx == -1)
                {
                    break;
                }
                ignores.Add(new Ignore(build.PrimaryTagIndicator) { Index = idx });
                idx++;

            } while (true);

            return ignores;
        }

        /// <summary>
        /// Parses return tags from the routine lines.
        /// </summary>
        /// <param name="lines">The text of a routine or concrete query - post removing unwanted comments and trimming.</param>
        /// <returns>List of return tags or null.</returns>
        protected List<Return> ExtractReturnTags(string[] lines)
        {
            int idx = 5;
            List<Return> tags = new List<Return>();
            do
            {
                idx = IndexOf(lines, tagFactory.ReturnTag, IndexOfTypes.StartsWith, idx);
                if (idx == -1)
                {
                    break;
                }
                string line = lines[idx];
                Return returnTag = (Return)tagFactory.TagFromLine(line);
                if (idx == lines.Length)
                {
                    throw new Exception($"{tagFactory.ReturnTag} tag found without following value line.");
                }
                idx++;
                line = lines[idx];
                returnTag.SetSupplemental(lines[idx]);
                returnTag.Validate();
                tags.Add(returnTag);
            } while (true);

            return tags;
        }
        #endregion

        #region Apply Methods

        /// <summary>
        /// Applies all tags to the parameter with the exeption of the parameter direction specific tags.
        /// Exclusions include: Input, Output, InOut
        /// </summary>
        /// <param name="routine">Routine instance.</param>
        /// <param name="parameter">Parameter instance.</param>
        /// <param name="tags">List of BaseTag intances.</param>
        protected void ApplyBasicParameterTags(Parameter parameter, List<BaseTag> tags)
        {
            foreach (BaseTag tag in tags)
            {
                // The range type needs the data type to pass validation.
                if (tag.TagType == TagTypes.Range)
                {
                    ((Tags.Range)tag).DataType = parameter.CSharpType;
                }

                tag.Validate();

                switch (tag.TagType)
                {
                    // These  are defered to database specific implementations.
                    case TagTypes.Input:
                    case TagTypes.InOut:
                    case TagTypes.Output:
                        break;
                    case TagTypes.CacheKey:
                        parameter.CacheOrder = ((CacheKey)tag).Order;
                        break;
                    case TagTypes.Comment:
                        parameter.Comment = ((Comment)tag).Value;
                        break;

                    case TagTypes.Default:
                        parameter.DefaultValue = ((Default)tag).Value;
                        break;

                    case TagTypes.Enum:
                        parameter.EnumerationName = ((Tags.Enum)tag).Value;
                        break;

                    case TagTypes.ForceColumnEncryption:
                        parameter.ForceColumnEncryption = true;
                        break;

                    case TagTypes.Required:
                        parameter.Annotations.Add(tag.GetAnnotation());
                        parameter.IsRequired = true;
                        break;

                    default:
                        string annotation = tag.GetAnnotation();
                        if(annotation != null)
                        {
                            parameter.Annotations.Add(tag.GetAnnotation());
                        }
                        else
                        {
                            throw new Exception("Null annotation");
                        }
                        break;
                }
            }
        }

        #endregion

        
        #region Validation

        /// <summary>
        /// Check for duplicate tags and duplicate modes on the list of parameters.
        /// </summary>
        /// <param name="tags">List of BaseTag.</param>
        /// <param name="parameterName">Name of parameter (included in error message).</param>
        protected void ValidateParameterTags(List<BaseTag> tags, string parameterName)
        {
            if (tags.Count != tags.GroupBy(t => t.Tag).Count())
            {
                throw new Exception($"{parameterName} has duplicate tags. Remove the invalid duplicate tag.");
            }
            if (tags.Count(t => t.TagType == TagTypes.InOut || t.TagType == TagTypes.Input || t.TagType == TagTypes.Output) > 1)
            {
                throw new Exception($"{parameterName} has multiple direction tags. A parameter may only have one of the following: Input, Output, or InOut.");
            }
        }
        #endregion

        public abstract List<Routine> CollectDBRoutinesAndQueryRoutines();

        public abstract bool TestConnection();

        public abstract List<EnumCollection> CollectEnumCollections();

        public abstract List<StaticCollection> CollectStaticCollections();

        public abstract List<Routine> CollectDBRoutines();

        public abstract List<Routine> CollectQueryRoutines();

    }
}