namespace SQLPLUS.Builder.DataCollectors
{
    using SQLPLUS.Builder.ConfigurationModels;
    using SQLPLUS.Builder.DataServices.MSSQL;
    using SQLPLUS.Builder.DataServices.MSSQL.Models;
    using SQLPLUS.Builder.Mappings;
    using SQLPLUS.Builder.Tags;
    using SQLPLUS.Builder.TemplateModels;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class MSSQLDataCollector : DataCollectorBase
    {
        private const string ROUTINE_TYPE_PROCEDURE = "PROCEDURE";
        private const string ROUTINE_TYPE_FUNCTION = "FUNCTION";
        private const string ROUTINE_TYPE_QUERY = "QUERY";
        private const string DATA_TYPE_TABLE = "TABLE";

        private readonly Service dataService;

        public MSSQLDataCollector(BuildDefinition buildDefintion, DatabaseConnection databaseConnection, ProjectInformation projectInformation)
            : base(buildDefintion, databaseConnection, projectInformation)
        {
            dataService = new Service(databaseConnection.ConnectionString);
        }

        public override bool TestConnection()
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(databaseConnection.ConnectionString))
                {
                    cnn.Open();
                    cnn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        
        public override List<Routine> CollectRoutines()
        {

            List<Routine> routines = CollectDBRoutines();

            //if (routines != null)
            //{
            //    foreach (Routine routine in routines)
            //    {
            //        //HydrateDBRoutine(routine);

            //        //try
            //        //{
            //        //    HydrateDBRoutine(routine);
            //        //}
            //        //catch(Exception ex)
            //        //{
            //        //    throw new Exception(ex.Message) { Source = $"Routine: [{routine.Schema}].[{routine.Name}]" };
            //        //}
            //    }
            //}

            List<Routine> queries = CollectQueries();

#if DEBUG
            //File.WriteAllText(@"C:\Users\Alan\source\repos\sql-plus-builder-04\Routines.json", JsonConvert.SerializeObject(routines));
            //File.WriteAllText(@"C:\Users\Alan\source\repos\sql-plus-builder-04\Queries.json", JsonConvert.SerializeObject(queries));
#endif
            routines.AddRange(queries);
            return routines;

        }

        #region DBRoutines

        /// <summary>
        /// Routine collects all the routines form the DB that have a --+SQLPlusRoutine tag.
        /// Subsequently filters the list based on the build definition schemas and routines.
        /// </summary>
        /// <param name="sqlRoutines"></param>
        /// <returns>Full set of database routines for the build or null.</returns>
        private List<Routine> CollectDBRoutines()
        {
            List<Routine> result = new List<Routine>();

            // Query the DB for routines.
            var output = dataService.SQLPlusRoutines();

            // Checking to see if any results were found. Safe to exit if nothing was found.
            if (output.ReturnValue == SQLPlusRoutinesOutput.Returns.NotFound)
            {
                return result;
            }

            BuildSchema includedBuildSchema;
            BuildRoutine includedBuildRoutine;

            foreach (SQLPlusRoutinesResult sqlRoutine in output.ResultData)
            {
                includedBuildSchema = null;
                includedBuildRoutine = null;
                string @namespace = null;

                includedBuildSchema = build.BuildSchemas?.FirstOrDefault(s => s.Schema.Equals(sqlRoutine.Schema, StringComparison.OrdinalIgnoreCase));
                @namespace = includedBuildSchema?.Namespace;
                if (@namespace == null)
                {
                    includedBuildRoutine = build.BuildRoutines?.FirstOrDefault(r => r.Schema.Equals(sqlRoutine.Schema, StringComparison.OrdinalIgnoreCase) && r.Name.Equals(sqlRoutine.Name, StringComparison.OrdinalIgnoreCase));
                    @namespace = includedBuildRoutine?.Namespace;
                }
                if (@namespace != null)
                {
                    string[] routineLines = GetDBRoutineLines(sqlRoutine.Schema, sqlRoutine.Name);
                    SQLPlusRoutine routineTag = ExtractSQLPlusRoutineTag(routineLines);
                    List<QueryStart> queryTags = ExtractQueryTags(routineLines);
                    List<Ignore> ignores = ExtractIgnoreTags(routineLines);
                    List<EnumDefinition> returnValueEnums = EnumDefinitionsDB(ExtractReturnTags(routineLines));
                    List<Parameter> parameters = GetDBRoutineParameters(sqlRoutine.Schema, sqlRoutine.Name, sqlRoutine.RoutineType, routineLines, returnValueEnums.Count == 0);
                    Routine routine = new Routine(
                        sqlRoutine.Name,
                        @namespace,
                        sqlRoutine.Schema,
                        sqlRoutine.RoutineType,
                        sqlRoutine.DataType,
                        AdoCommandType(sqlRoutine.RoutineType, sqlRoutine.DataType),
                        AdoCommandText(sqlRoutine.Schema, sqlRoutine.Name, sqlRoutine.RoutineType, sqlRoutine.DataType, parameters, routineLines),
                        GetDBRoutineLines(sqlRoutine.Schema, sqlRoutine.Name),
                        project,
                        build,
                        routineTag,
                        queryTags,
                        parameters,
                        returnValueEnums,
                        sqlRoutine.LastModified
                    );
                    SetDBResultColumns(routine);
                    result.Add(routine);
                }
            }
            // Validating that the database returned at least one routine for each schema in the build definition.
            if (build.BuildSchemas != null)
            {
                List<string> invalidSchemas = new List<string>();

                foreach (var buildSchema in build.BuildSchemas)
                {
                    if (!result.Exists(s => s.Schema.Equals(buildSchema.Schema, StringComparison.OrdinalIgnoreCase)))
                    {
                        invalidSchemas.Add(buildSchema.Schema);
                    }
                }
                if (invalidSchemas.Count != 0)
                {
                    throw new Exception($"The schema(s): [{string.Join(", ", invalidSchemas)}] were included in the BuildDefintion.json file, but cannot be built. This indicates that the schema is not present in the database, or the schema does not contain any semantically tagged procedures. Correct the BuildDefinition.json file.");
                }
            }

            // Validating that the database returned at a routine for each routine in the build definition
            if (build.BuildRoutines != null)
            {
                List<string> invalidRoutines = new List<string>();
                foreach (var buildRoutine in build.BuildRoutines)
                {
                    if (!result.Exists(r => r.Schema.Equals(buildRoutine.Schema, StringComparison.OrdinalIgnoreCase) && r.Name.Equals(buildRoutine.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        invalidRoutines.Add($"{buildRoutine.Schema}.{buildRoutine.Name}");
                    }
                }
                if (invalidRoutines.Count != 0)
                {
                    throw new Exception($"The routine(s): [{string.Join(", ", invalidRoutines)}] were included in the BuildDefintion.json file, but cannot be built. This indicates that the routine is not present in the database, or the routine is not semantically tagged. Correct the BuildDefinition.json file.");
                }
            }

            return result;
        }

        private string[] GetDBRoutineLines(string schema, string name)
        {
            StringBuilder bldr = new StringBuilder();

            using (SqlConnection cnn = new SqlConnection(databaseConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $"exec sp_helptext @SP";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@SP",
                        SqlDbType = System.Data.SqlDbType.VarChar,
                        Size = 256,
                        Value = $"[{schema}].[{name}]"
                    });

                    cnn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string temp = rdr.GetString(0);
                            if (string.IsNullOrEmpty(temp))
                            {
                                continue;
                            }
                            bldr.Append(temp);
                        }
                        rdr.Close();
                        cnn.Close();
                    }
                }
            }

            return GetRoutineLinesFromText(bldr.ToString());
        }

        public string AdoCommandText(string schema, string name, string routineType, string dataType, List<Parameter> parameters, string[] routineText)
        {
            if (routineType == ROUTINE_TYPE_FUNCTION || routineType == ROUTINE_TYPE_PROCEDURE)
            {
                if (dataType == DATA_TYPE_TABLE)
                {
                    List<string> inputParameters = new List<string>();
                    foreach (Parameter parameter in parameters)
                    {
                        if (parameter.IsInput)
                        {
                            inputParameters.Add(parameter.Name);
                        }
                    }
                    if (inputParameters.Count != 0)
                    {
                        string arguments = string.Join(", ", inputParameters);
                        return $"\"SELECT * FROM [{schema}].[{name}]({arguments})\"";
                    }
                    else
                    {
                        return $"\"SELECT * FROM [{schema}].[{name}]()\"";
                    }
                }
                return $"\"[{schema}].[{name}]\"";
            }

            return "@\"" + Environment.NewLine + string.Join(Environment.NewLine, CleansedRoutineLines(routineText)) + "\"";
        }

        private string AdoCommandType(string routineType, string dataType)
        {
            if(routineType == ROUTINE_TYPE_PROCEDURE || (routineType == ROUTINE_TYPE_FUNCTION && dataType != DATA_TYPE_TABLE))
            {
                return "CommandType.StoredProcedure";
            }
            return "CommandType.Text";
        }

        private void ValidateRoutineAndQueryStarts(SQLPlusRoutine routineTag, List<QueryStart> queryStarts)
        {
            if (routineTag.SelectType == SelectTypes.MultiSet)
            {
                if (queryStarts.Count == 0)
                {
                    throw new Exception("MultiSet query expecting QueryStart/QueryEnd tags.");
                }
            }
            if (queryStarts.Count != 0)
            {
                if (routineTag.SelectType != SelectTypes.MultiSet)
                {
                    throw new Exception("QueryStart/QueryEnd tags found but select type is not MutliSet.");
                }
            }
        }
        private List<Parameter> GetDBRoutineParameters(string routineSchema, string routineName, string routineType, string[] routineLines, bool isReturnValueEnumerated)
        {
            List<Parameter> result = new List<Parameter>();
            RoutineParametersOutput routineParametersOutput = dataService.RoutineParameters(new RoutineParametersInput { Schema = routineSchema, Name = routineName });
            if (routineParametersOutput.ReturnValue == RoutineParametersOutput.Returns.Ok)
            {
                foreach (RoutineParametersResult prm in routineParametersOutput.ResultData)
                {
                    prm.Name = string.IsNullOrEmpty(prm.Name) ? "@ReturnValue" : prm.Name;
                    List<BaseTag> parameterTags = prm.Name == "@ReturnValue" ? new List<BaseTag>() : GetMSSQLParameterTags(routineLines, prm.Name);
                    DataTypeMapping dataTypeMapping = DataTypeMappings.Mappings.First(m => m.MSSQLDataType == prm.SQLType);
                    ParameterModeMapping parameterModeMapping = GetActualParameterModeMappingDB(prm, GetDirectionalParameterTagFromList(parameterTags));
                    Parameter parameter = CreateParameter(prm, parameterModeMapping, dataTypeMapping);
                    ApplyBasicParameterTags(parameter, parameterTags);
                    result.Add(parameter);
                }
                foreach (Parameter prm in result.Where(p => p.IsTableValueParameter))
                {
                    prm.TVColumns = GetTVColumns(prm.Name, prm.UserDefinedTypeSchema, prm.UserDefinedTypeName);
                }
            }
            if (routineType.Equals(ROUTINE_TYPE_PROCEDURE, StringComparison.OrdinalIgnoreCase))
            {
                result.Insert(0, CreateReturnValueParameter());
            }
            return result;
        }
        private List<BaseTag> GetMSSQLParameterTags(string[] routineLines, string parameterName)
        {
            List<BaseTag> result = new List<BaseTag>();

            int parameterLineIndex = IndexOf(routineLines, parameterName + " ", IndexOfTypes.Contains, 0);
            int firstTagLineIndex = parameterLineIndex;
            string paramterDefinitionLine = routineLines[parameterLineIndex];
            do
            {
                firstTagLineIndex--;
                string testLine = routineLines[firstTagLineIndex];
                LineTypes lineType = LineType(testLine);
                if (lineType != LineTypes.PrimaryTag && lineType != LineTypes.SupplementalTag)
                {
                    firstTagLineIndex++;
                    break;
                }
            } while (firstTagLineIndex != 0);

            if (firstTagLineIndex == parameterLineIndex)
            {
                return result;
            }

            BaseTag tag = null;

            for (int idx = firstTagLineIndex; idx != parameterLineIndex; idx++)
            {
                string line = routineLines[idx];
                LineTypes lineType = LineType(line);
                if (lineType == LineTypes.PrimaryTag)
                {
                    tag = tagFactory.TagFromLine(line);
                    result.Add(tag);
                    continue;
                }
                if (lineType == LineTypes.SupplementalTag)
                {
                    if (tag == null)
                    {
                        throw new Exception($"Unexpected supplemental tag found at line {line}");
                    }
                    tag.SetSupplemental(line);
                }
            }

            ValidateParameterTags(result, parameterName);

            return result;
        }
        private ParameterModeMapping GetActualParameterModeMappingDB(RoutineParametersResult parameter, BaseTag tag)
        {
            string returnMode = parameter.Mode;
            
            if (tag == null)
            {
                if (parameter.Mode == "INOUT")
                {
                    returnMode = "OUTPUT";
                }
            }
            else
            {
                if (tag.TagType == TagTypes.Output)
                {
                    throw new Exception($"{parameter.Name} has an invalid {tag.Tag} tag. This tag only applies when using concrete queries. Remove the {tag.Tag}.");
                }
                if (parameter.Mode == "IN")
                {
                    // @ParameterName int, Cannot change the mode of an input parameter since (missing output clause).

                    if (tag.TagType == TagTypes.Input)
                    {
                        throw new Exception($"{parameter.Name} has an invalid {tag.Tag} tag. This is the default direction for parameters in MSSQL routines and is therefore redundant. Remove the {tag.Tag}.");
                    }
                    if (tag.TagType == TagTypes.InOut || tag.TagType == TagTypes.Output)
                    {
                        throw new Exception($"{parameter.Name} has an invalid {tag.Tag} tag. This tag can only be applied to parameters marked as output parameters in the database. Add the output clause to the parameter or remove the {tag.Tag}.");
                    }
                }
            }

            return ParameterModeMappings.Mappings.Find(m => m.MSSQLParameterMode == returnMode);
        }
        protected BaseTag GetDirectionalParameterTagFromList(List<BaseTag> tags)
        {
            BaseTag result = null;
            if (tags != null)
            {
                result = tags.Find(t => t.TagType == TagTypes.InOut || t.TagType == TagTypes.Input || t.TagType == TagTypes.Output);
            }
            return result;
        }
        private Parameter CreateParameter(
            RoutineParametersResult parameter,
            ParameterModeMapping parameterModeMapping,
            DataTypeMapping dataTypeMapping)
        {
            return new Parameter(
                parameter.OrdinalPosition,
                parameter.Name,
                KeywordMappings.SafeName(parameter.Name.Substring(1)),
                parameter.Name,
                GetADOPrecision(parameter),
                parameter.NumericScale,
                parameter.CharacterMaxLength,
                parameter.UserDefinedTypeSchema,
                parameter.UserDefinedTypeName,
                parameterModeMapping,
                dataTypeMapping,
                build,
                project
                );
        }
        private Parameter CreateReturnValueParameter()
        {
            DataTypeMapping dataTypeMapping = DataTypeMappings.Mappings.FirstOrDefault(m => m.MSSQLDataType == "int");
            ParameterModeMapping parameterModeMapping = ParameterModeMappings.Mappings.FirstOrDefault(m => m.MSSQLParameterMode == "RETURN");
            RoutineParametersResult parameter = new RoutineParametersResult
            {
                OrdinalPosition = 0,
                Name = "@ReturnValue",
                NumericPrecision = 10,
                NumericScale = 0
            };
            return CreateParameter(parameter, parameterModeMapping, dataTypeMapping);
        }
        private List<Column> GetTVColumns(string parameterName, string userDefinedTypeSchema, string userDefinedTypeName)
        {
            List<Column> result = new List<Column>();

            ResultColumnsForTextOutput resultColumnsForTypeOutput = dataService.ResultColumnsForText(new ResultColumnsForTextInput { SQLText = SQLTextForType(userDefinedTypeSchema, userDefinedTypeName) });
            if (resultColumnsForTypeOutput.ReturnValue == ResultColumnsForTextOutput.Returns.NotFound)
            {
                throw new Exception($"Parameter: {parameterName} is defined as a table type parameter {userDefinedTypeSchema}.{userDefinedTypeName} but the table type was not found.");
            }
            foreach (ResultColumnsForTextResult col in resultColumnsForTypeOutput.ResultData)
            {
                DataTypeMapping dataTypeMapping = DataTypeMappings.Mappings.FirstOrDefault(m => m.MSSQLDataType == col.SQLType);
                List<string> annotations = new List<string>();
                if(!col.IsNullable.Value)
                {
                    annotations.Add(new Tags.Required(build.PrimaryTagPrefix,build.SupplementalTagPrefix).GetAnnotation());
                }
                if(col.CharacterMaxLength.HasValue && col.CharacterMaxLength != -1)
                {
                    Tags.MaxLength maxLengthTag = new MaxLength(build.PrimaryTagPrefix, build.SupplementalTagPrefix);
                    maxLengthTag.Value = col.CharacterMaxLength.Value;
                    annotations.Add(maxLengthTag.GetAnnotation());
                }
                result.Add(new Column(col.Index.Value, col.Name, col.Name, col.IsNullable.Value, col.CharacterMaxLength, annotations, project, build, dataTypeMapping));
            }
            return result;
        }
        private void SetDBResultColumns(Routine routine)
        {
            // If it is a scalar function the result is a return parameter.
            if (routine.RoutineType.Equals(ROUTINE_TYPE_FUNCTION, StringComparison.OrdinalIgnoreCase))
            {
                SetResultColumnsForFunction(routine);
            }
            else if (routine.SelectType == SelectTypes.MultiSet)
            {
                SetResultColumnsForMultiSet(routine);
            }
            else
            {
                SetResultColumnsForProcedure(routine);
            }
        }
        private void SetResultColumnsForFunction(Routine routine)
        {
            if (routine.DataType.Equals(DATA_TYPE_TABLE, StringComparison.OrdinalIgnoreCase))
            {
                var output = dataService.ResultColumnsForTable(new ResultColumnsForTableInput { Schema = routine.Schema, Function = routine.Name });
                if (output.ReturnValue == ResultColumnsForTableOutput.Returns.NotFound)
                {
                    throw new Exception("Unable to resolve result columns in table function.");
                }
                List<Column> resultColumns = new List<Column>();
                foreach (var col in output.ResultData)
                {
                    DataTypeMapping dataTypeMapping = DataTypeMappings.Mappings.Find(m => m.MSSQLDataType == col.SQLType);
                    resultColumns.Add(new Column(col.Index.Value, col.Name, col.Name, col.IsNullable.Value, col.CharacterMaxLength, null, project, build, dataTypeMapping));
                }
                routine.ResultSets.Add(new ResultSet("+", routine.SelectType, resultColumns));
            }
        }
        private void SetResultColumnsForMultiSet(Routine routine)
        {
            string tempProcedureName = $"[{routine.Schema}].[{routine.Name}_SQLPX]";
            string dropTempProcedureText = $"DROP PROCEDURE {tempProcedureName}";
            string[] copy = GetRoutineLinesSafeCopy(routine.RoutineLines);
            CommentOutIgnores(copy, routine.Ignores);
            int declarationLine = IndexOf(copy, "CREATE PROCEDURE", IndexOfTypes.StartsWith, 0);
            copy[declarationLine] = ReplaceFirst(copy[declarationLine], routine.Name, $"{routine.Name}_SQLPX");
            for (int idx = 0; idx != routine.Queries.Count; idx++)
            {
                QueryStart currentQuery = routine.Queries[idx];
                ExecuteRawText(dropTempProcedureText, true);
                string createTempProcureText = StringArrayToString(copy);
                ExecuteRawText(createTempProcureText);
                List<Column> columns = GetColumnsForText($"[{routine.Schema}].[{routine.Name}_SQLPX]");
                if (columns == null)
                {
                    throw new Exception($"MultiSet query {currentQuery.Name} failed to produce a result set.");
                }
                routine.ResultSets.Add(new ResultSet(currentQuery.Name, currentQuery.SelectType, columns));
                CommentOutQueryLines(copy, currentQuery);
            }

            ExecuteRawText(dropTempProcedureText, true);

            List<string> concatColumns = new List<string>();
            foreach(ResultSet rs in routine.ResultSets)
            {
                if(concatColumns.Contains(rs.ConcatColumns))
                {
                    throw new Exception("Each result set in a multiset query must be unique. Update the routine so that each result has at least one different column.");
                }
                concatColumns.Add(rs.ConcatColumns);
            }
        }
        private void SetResultColumnsForProcedure(Routine routine)
        {
            List<Column> resultColumns = GetColumnsForText($"[{routine.Schema}].[{routine.Name}]");
            if (resultColumns.Count == 0)
            {
                if(routine.SelectType != SelectTypes.NonQuery)
                {
                    throw new Exception("No result columns");
                }
            }
            routine.ResultSets.Add(new ResultSet("+", routine.SelectType, resultColumns));
        }

        #endregion

        /// <summary>
        /// Collects the Queries for the build.
        /// </summary>
        /// <returns>List of routines.</returns>
        private List<Routine> CollectQueries()
        {
            List<Routine> result = new List<Routine>();
            if (!Directory.Exists(project.SQLPLUSQueryFolder))
            {
                return result;
            }
            
            string[] directories = Directory.GetDirectories(project.SQLPLUSQueryFolder);
            foreach (string directory in directories)
            {
                string[] files = Directory.GetFiles(directory, "*.sql");
                foreach (string file in files)
                {
                    string nameSpace = new DirectoryInfo(directory).Name;
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string path = Path.Combine(directory, file);
                    string text = File.ReadAllText(path);
                    DateTime lastModified = File.GetLastWriteTime(path);

                    string[] routineLines = GetRoutineLinesFromText(text);
                    SQLPlusRoutine routineTag = ExtractSQLPlusRoutineTag(routineLines);
                    List<QueryStart> queryTags = ExtractQueryTags(routineLines);
                    List<Ignore> ignores = ExtractIgnoreTags(routineLines);
                    List<EnumDefinition> returnValueEnums = EnumDefinitionsDB(ExtractReturnTags(routineLines));
                    List<Parameter> parameters = GetQueryParameters(routineLines);

                    if(returnValueEnums.Count != 0)
                    {
                        Parameter parameter = parameters.Find(p => p.Name == "@ReturnValue");
                        if (parameter != null)
                        {
                            parameter.EnumerationName = "Returns";
                        }
                    }
                    Routine routine = new Routine(
                        fileName,
                        nameSpace,
                        "dbo",
                        "QUERY",
                        null,
                        AdoCommandType(ROUTINE_TYPE_QUERY, null),
                        AdoCommandText("dbo", fileName, ROUTINE_TYPE_QUERY, null, parameters, routineLines),
                        routineLines,
                        project,
                        build,
                        routineTag,
                        queryTags,
                        parameters,
                        returnValueEnums,
                        lastModified);
                    result.Add(routine);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the list of parameters for the given routine text.
        /// </summary>
        /// <param name="lines">Routine text.</param>
        /// <returns>List of parameters.</returns>
        private List<Parameter> GetQueryParameters(string[] lines)
        {
            List<Parameter> result = new List<Parameter>();

            int parametersStart = IndexOf(lines, tagFactory.ParametersTag, IndexOfTypes.Exact, 0);
            if (parametersStart == -1) return result;

            int parametersEnd = IndexOf(lines, tagFactory.ParametersTag, IndexOfTypes.Exact, parametersStart + 1);
            if (parametersEnd == -1)
            {
                throw new Exception("The --+Parameter tag requires an open and close, only a single --+Parameter tag was found");
            }

            List<BaseTag> tags = new List<BaseTag>();

            parametersStart++;
            string line = lines[parametersStart];

            if (!line.Equals("DECLARE", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"The first line following the {tagFactory.ParametersTag} must be a DECLARE statement.");
            }
            parametersStart++;

            int index = parametersStart;

            BaseTag currentTag = null;
            do
            {
                if (index == parametersEnd) break;

                line = lines[index];
                LineTypes lineType = LineType(line);
                if (lineType == LineTypes.PrimaryTag)
                {
                    currentTag = tagFactory.TagFromLine(line);
                    if (currentTag.TagContext != TagContexts.ParameterAnnotation)
                    {
                        throw new Exception($"The {currentTag.Tag} does not apply to parameters.");
                    }
                    tags.Add(currentTag);
                }
                else if (lineType == LineTypes.SupplementalTag)
                {
                    if (currentTag == null)
                    {
                        throw new Exception($"Unexpected supplemental tag near {line}");
                    }
                    currentTag.SetSupplemental(line);
                }
                else if (lineType == LineTypes.BodyText)
                {
                    RoutineParametersResult prm = RoutineParametersResultFromLine(line, result.Count);
                    DataTypeMapping dataTypeMapping = DataTypeMappings.Mappings.First(m => m.MSSQLDataType == prm.SQLType);
                    ParameterModeMapping parameterModeMapping = GetActualParameterModeMappingQuery(prm, GetDirectionalParameterTagFromList(tags));
                    Parameter parameter = CreateParameter(prm, parameterModeMapping, dataTypeMapping);
                    ApplyBasicParameterTags(parameter, tags);
                    result.Add(parameter);
                    tags.Clear();
                }
                index++;

            } while (index != parametersEnd);
            return result;
        }

        /// <summary>
        /// Uses the parmater mode combined with a more tag to determine the tagged intention for the parmater.
        /// </summary>
        /// <param name="parameter">RoutineParametersResult from a query</param>
        /// <param name="tag">Parameter mode tag. Input, Output, InOut</param>
        /// <returns>The desired parameter mode mapping.</returns>
        private ParameterModeMapping GetActualParameterModeMappingQuery(RoutineParametersResult parameter, BaseTag tag)
        {
            //By default all parameters are added as inputs when dealing with queries.

            // OUTPUT tag make an output parameter
            // InOut tag makes as input parameter.

            string returnMode = parameter.Mode;

            if (tag != null)
            {
                if (tag.TagType == TagTypes.Input)
                {
                    throw new Exception($"{parameter.Name} has an invalid {tag.Tag} tag. This is the default direction for parameters in MSSQL queries and is therefore redundant. Remove the {tag.Tag}.");
                }
                if (tag.TagType == TagTypes.InOut)
                {
                    returnMode = "INOUT";
                }
                else if (tag.TagType == TagTypes.Output)
                {
                    if(parameter.Name == "@ReturnValue")
                    {
                        returnMode = "OUTPUTASRETURN";
                    }
                    else
                    {
                        returnMode = "OUTPUT";
                    }
                }
            }

            return ParameterModeMappings.Mappings.Find(m => m.MSSQLParameterMode == returnMode);
        }

        /// <summary>
        /// Gets a routine parameter from SQL Text in a way that simulates what the DB returns.
        /// This makes subsequent processing of these values similar to those returned by the DB.
        /// </summary>
        /// <param name="parameterLine">Line that contains the parameter.</param>
        /// <param name="idx">Index to assign to the parameter</param>
        /// <returns>RoutineParametersResult instance</returns>
        private RoutineParametersResult RoutineParametersResultFromLine(string parameterLine, int idx)
        {
            string[] lineParts = parameterLine.Split(new char[] { ' ', '(', ')', '=', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            string name = lineParts[0].Trim();
            string dataType = lineParts[1].ToLower();

            string userDefineTypeSchema = null;
            string userDefinedTypeName = null;

            List<Column> tvColumns = new List<Column>();

            ParameterModeMapping parameterModeMapping = ParameterModeMappings.Mappings.FirstOrDefault(m => m.MSSQLParameterMode == "IN");
            DataTypeMapping dataTypeMapping = DataTypeMappings.Mappings.FirstOrDefault(m => m.MSSQLDataType == dataType);

            if (dataTypeMapping == null)
            {
                if (dataType.Contains("."))
                {
                    string[] nameParts = dataType.Split('.');
                    userDefinedTypeName = nameParts[1];
                    userDefineTypeSchema = nameParts[0];
                    dataType = "table";
                }
            }

            RoutineParametersResult result = new RoutineParametersResult
            {
                OrdinalPosition = idx,
                Mode = "IN",
                SQLType = dataType,
                UserDefinedTypeSchema = userDefinedTypeName,
                UserDefinedTypeName = userDefinedTypeName,
                Name = name,
            };

            switch (dataType)
            {
                case "bigint":
                case "bit":
                case "int":
                case "smallmoney":
                case "tinyint":
                case "uniqueidentifier":
                case "smallint":
                case "money":
                case "float":
                case "hierarchyid":
                case "real":
                case "date":
                case "datetime":
                case "datetimeoffset":
                case "smalldatetime":
                case "time":
                case "text":
                case "ntext":
                case "geography":
                case "geometry":
                case "image":
                case "xml":
                case "sql_variant":
                case "table":
                    break;
                case "decimal":
                case "numeric":
                    if (lineParts.Length > 2)
                    {
                        result.NumericPrecision = byte.Parse(lineParts[2]);
                    }
                    if (lineParts.Length > 3)
                    {
                        result.NumericScale = short.Parse(lineParts[3]);
                    }
                    break;
                case "datetime2":
                    if (lineParts.Length > 2)
                    {
                        result.DateTimePrecision = short.Parse(lineParts[2]);
                    }
                    break;
                case "binary":
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "varbinary":
                    {
                        if (lineParts.Length > 2)
                        {
                            if (lineParts[2].ToLower() == "max")
                            {
                                result.CharacterMaxLength = -1;
                            }
                            else
                            {
                                result.CharacterMaxLength = int.Parse(lineParts[2]);
                            }
                        }
                        break;
                    }
                default:
                    throw new Exception($"Could not read parameter definition line {parameterLine}");
            }

            return result;
        }

        /// <summary>
        /// This is used for creating and dropping temp procedures for multiset queries.
        /// </summary>
        /// <param name="text">Text to execute.</param>
        /// <param name="ignoreErrors">Indicates weather to swallow the error. Used for the drop procedure calls.</param>
        private void ExecuteRawText(string text, bool ignoreErrors = false)
        {
            using (SqlConnection cnn = new SqlConnection(databaseConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = text;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cnn.Open();
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        if (!ignoreErrors)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        cnn.Close();
                    }
                }
            }
        }


        /// <summary>
        /// Gets result columns for SQL Statement passed.
        /// </summary>
        /// <param name="text">SQL Statement.</param>
        /// <returns>List of columns.</returns>
        private List<Column> GetColumnsForText(string text)
        {
            List<Column> result = new List<Column>();
            var output = dataService.ResultColumnsForText(new ResultColumnsForTextInput { SQLText = text });
            foreach (var row in output.ResultData)
            {
                DataTypeMapping dataTypeMapping = DataTypeMappings.Mappings.Find(m => m.MSSQLDataType == row.SQLType);
                result.Add(new Column(row.Index.Value, row.Name, row.Name, row.IsNullable.Value, row.CharacterMaxLength, null, project, build, dataTypeMapping));
            }
            return result;
        }

        /// <summary>
        /// Gets the enum defintions for the given return tags.
        /// </summary>
        /// <param name="returns">List of return tags.</param>
        /// <returns>List of enum definitions.</returns>
        private List<EnumDefinition> EnumDefinitionsDB(List<Return> returns)
        {
            List<EnumDefinition> result = new List<EnumDefinition>();
            foreach (var item in returns)
            {
                string stringValue = Regex.Match(item.Value, @"\d+").Value;
                bool parsed = int.TryParse(stringValue, out int value);

                if (string.IsNullOrEmpty(stringValue) || !parsed)
                {
                    throw new Exception($"Could not extract integer value for return tag at {item.Value}");
                }

                result.Add(new EnumDefinition()
                {
                    Comment = item.Description,
                    Name = item.Name,
                    Value = value.ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// Ado precision can be applied to numeric of date precision from the database.
        /// </summary>
        /// <param name="row">RoutineParametersResult instance.</param>
        /// <returns>AdoPrecision value.</returns>
        private int? GetADOPrecision(RoutineParametersResult row)
        {
            if (row.NumericPrecision.HasValue)
            {
                return row.NumericPrecision.Value;
            }
            else if (row.DateTimePrecision.HasValue)
            {
                return row.DateTimePrecision;
            }
            return null;
        }

        /// <summary>
        /// When a parameter is a table type parameter, it is necessary to collect the meta data about that type.
        /// This generates the SQL statement to pass to columns for text routine based on the parameters UTD schema and name.
        /// </summary>
        /// <param name="schema">user defined type schema</param>
        /// <param name="typeName">user defined type name</param>
        /// <returns>SQL statement.</returns>
        private string SQLTextForType(string schema, string typeName)
        {
            return $"DECLARE @Table [{schema}].[{typeName}]; SELECT * FROM @Table;";
        }

        public override List<StaticCollection> CollectStaticCollections()
        {
            List<StaticCollection> result = new List<StaticCollection>();
            foreach(BuildQuery query in build.StaticQueries)
            {
                result.Add(new StaticCollection
                {
                    Data = DataForText(query.Query),
                    Name = query.Name,
                    Columns = GetColumnsForText(query.Query)
                });
            }
            return result;
        }

        public List<List<object>> DataForText(string query)
        {
            List<List<object>> result = new List<List<object>>();
            using (SqlConnection cnn = new SqlConnection(databaseConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = cnn;
                    cnn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        int count = rdr.FieldCount;
                        while (rdr.Read())
                        {
                            List<object> row = new List<object>();
                            for (int idx = 0; idx != count; idx++)
                            {
                                row.Add(rdr.GetValue(idx));
                            }
                            result.Add(row);
                        }
                    }
                    cnn.Close();
                }
            }
            return result;
        }
        public override List<EnumCollection> CollectEnumCollections()
        {
            List<EnumCollection> result = new List<EnumCollection>();

            foreach(var query in build.EnumQueries)
            {
                EnumCollection enumCollection = new EnumCollection { Name = query.Name, Enums = new List<EnumDefinition>() };
                result.Add(enumCollection);
                using(SqlConnection cnn = new SqlConnection(databaseConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = query.Query;
                        cnn.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        bool hasName = false;
                        bool hasValue = false;
                        bool hasComment = false;
                        bool first = true;

                        while(rdr.Read())
                        {
                            if(first)
                            {
                                for(int idx = 0; idx != rdr.FieldCount; idx++)
                                {
                                    switch(rdr.GetName(idx))
                                    {
                                        case "Name":
                                            hasName = true;
                                            break;
                                        case "Value":
                                            hasValue = true;
                                            enumCollection.DataType = rdr[idx].GetType().ToString();
                                            break;
                                        case "Comment":
                                            hasComment = true;
                                            break;
                                    }
                                }
                                if(!hasName)
                                {
                                    throw new ArgumentException($"Name is missing form Enum Query {query.Name}");
                                }
                                if (!hasValue)
                                {
                                    throw new ArgumentException($"Value is missing form Enum Query {query.Name}");
                                }
                            }

                            EnumDefinition enumDefinition = new EnumDefinition();
                            enumDefinition.Name = rdr["Name"].ToString();
                            enumDefinition.Value = rdr["Value"].ToString();
                            
                            if(hasComment)
                            {
                                enumDefinition.Comment = rdr["Comment"].ToString();
                            }   
                            enumCollection.Enums.Add(enumDefinition);
                        }
                        cnn.Close();
                    }
                }
            }

            return result;
        }
    }
}
