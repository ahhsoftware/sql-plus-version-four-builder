﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace SQLPLUS.Builder.Render.Common
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\Common\BuildDefinitionTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class BuildDefinitionTemplate : BuildDefinitionTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\n/* This file provides information to the SQL+ Code Generation Utility and deter" +
                    "mines the build output */\r\n/* For more detailed information view the documentati" +
                    "on at www.SQPlus.net */\r\n\r\n// SQLClientNamespace - this defines the SQL Client u" +
                    "tilized at runtime.\r\n// SQLServer options:\r\n//    1) System.Data.SqlClient - ful" +
                    "l support (default)\r\n//    2) Microsoft.Data.SqlClient - full support\r\n// The se" +
                    "lection must match the nuget package utilized by your library project.\r\n\r\n\r\n// S" +
                    "QLExceptionNamespace - defines the exception type utilized in the services.\r\n// " +
                    "SQLServer options:\r\n//     1) System.Data\r\n\r\n\r\n// Template - defines the renderi" +
                    "ng template used for generation\r\n//     1) DotNet - generates a C# library\r\n\r\n\r\n" +
                    "// BuildRoutines and BuildSchemas determines which database objects to generate " +
                    "code for where:\r\n//     1) BuildSchemas will build all routines within the speci" +
                    "fied schema\r\n//     2) BuildRoutines will build only the routines identified\r\n//" +
                    "     3) The routines will be build withing the Namespace specified by a string i" +
                    "dentifier\r\n//     4) Use the special symbol + to build objects in the root of th" +
                    "e project.\r\n// Example:\r\n// \"BuildRoutines\": [\r\n//  {\r\n//     This will build on" +
                    "ly the DB object [dbo].[MyRoutine] into the MyNamespace.\r\n//     \"Schema\" : \"dbo" +
                    "\",\r\n//     \"RoutineName\" : \"MyRoutine\",\r\n//     \"Namespace\" : \"MyNamespace\"\r\n// " +
                    "  },\r\n//   {\r\n//      \"Schema\" : \"Database Schema\"\r\n//      \"RoutineName\" : \"Dat" +
                    "abase Routine Name\",\r\n//      \"Namespace\" : \"Namespace\" or \"+\"\r\n//  },\r\n//],\r\n\r\n" +
                    "\r\n// Enum Queries (optional) are utilized to generate enumerations based on a SQ" +
                    "L query where:\r\n//     1) Name - identifies the named of the enumeration.\r\n//   " +
                    "  2) Query - defines the query for the enumeration items where:\r\n//          A) " +
                    "[Name] column maps to the named constant string representation for the underlyin" +
                    "g value.\r\n//          B) [Value] column maps to the underlying numeric value.\r\n/" +
                    "/          C) [Comment] column maps to a comment for the enumerated value.\r\n// E" +
                    "xample:\r\n// \"EnumQueries\": [\r\n//    {\r\n//      \"Name\": \"MyEnum\",\r\n//      \"Query" +
                    "\": \"SELECT [Column1] AS [Name], [Column1] AS [Value], [Column3] AS [Comment] FRO" +
                    "M [TableName] WHERE...\"\r\n//    },...\r\n//  ]\r\n// Example Generated Code:\r\n// publ" +
                    "ic enum MyEnum\r\n// {\r\n//\r\n//     /// <summary>\r\n       /// row.[Comment]\r\n      " +
                    " /// </summary>\r\n//     row.[Name] = row.[Value],\r\n//     ...\r\n// }\r\n\r\n\r\n// Stat" +
                    "ic Queries (optional) are utilized to generate lists of data based on a SQL quer" +
                    "y where:\r\n//    1) Name - identifies the name for the static data where:\r\n//    " +
                    "    A) Name maps to the class name generated.\r\n//        B) Data from the query " +
                    "is represented as a list of class name.\r\n//    2) Query - defines the query for " +
                    "list of data generated where:\r\n//        A) Each Column maps to a property of th" +
                    "e class.\r\n//        B) Each row of data represents a class instance within the l" +
                    "ist.\r\n// Example:\r\n// \"StaticQueries\": [\r\n//  {\r\n//    \"Name\": \"MyData\",\r\n//    " +
                    "\"Query\": \"SELECT Field1, Field2 FROM [dbo].[MyTableName]\"\r\n//  },...\r\n// ]\r\n// G" +
                    "enerated code\r\n// public class MyData\r\n// {\r\n//     public MyData(int Field1, st" +
                    "ring Field2)\r\n//     {\r\n//         this.Field1 = Field1,\r\n//         this.Field2" +
                    " = Field2\r\n//     }\r\n//     public int Field1 { get; }\r\n//     public string Fie" +
                    "ld2 { get; }\r\n// }\r\n// public partial class StaticData\r\n// {\r\n//    public stati" +
                    "c List<MyData> MyDataList = new List<MyData>()\r\n//    {\r\n//      new MyData(row[" +
                    "0].Field1, row[0].Field2),\r\n//      new MyData(row[1].Field1, row[1].Field2),\r\n/" +
                    "/    }\r\n// }\r\n\r\n// Build Options allows turning on/off (true/flase) features whe" +
                    "re:\r\n//     1) ImplementIChangeTracking - generated code implements the IChangeT" +
                    "racking interface.\r\n//     2) ImplementIRevertibleChangeTracking - generated cod" +
                    "e implements the IRevertibleChangeTracking interface.\r\n//     3) ImplementINotif" +
                    "yPropertyChanged - generated code implements the INotifyPropertyChanged interfac" +
                    "e.\r\n//     4) IncludeAsyncServices - generated code will include asynchronus met" +
                    "hods for services.\r\n//     5) UseNullableReferenceTypes - generated code will ut" +
                    "ilize nullable references types.\r\n// Turn features on by setting any of the valu" +
                    "es to true.\r\n// Example:\r\n// \"BuildOptions\": {\r\n//  \"ImplementIChangeTracking\": " +
                    "false,\r\n//  \"ImplementIRevertibleChangeTracking\": false,\r\n//  \"ImplementINotifyP" +
                    "ropertyChanged\": false,\r\n//  \"IncludeAsyncServices\": false, (future release)\r\n//" +
                    "  \"UseNullableReferenceTypes\": false\r\n// },\r\n\r\n/* Instructions (End) */\r\n\r\n/* Co" +
                    "nfiguration (Start) */\r\n\r\n{\r\n  \"SQLClientNamespace\": choose \"System.Data.SqlClie" +
                    "nt\" or \"Microsoft.Data.SqlClient\",\r\n  \"SQLExceptionNamespace\": \"System.Data\",\r\n " +
                    " \"Template\": \"DotNet\",\r\n  \"BuildRoutines\": [\r\n    {\r\n        \"Schema\" : \"Databas" +
                    "e Schema\",\r\n        \"RoutineName\" : \"Database Routine Name\",\r\n        \"Namespace" +
                    "\" : enter \"Namespace\" or \"+\"\r\n    },\r\n    {\r\n        \"Schema\" : \"Database Schema" +
                    "\"\r\n        \"RoutineName\" : \"Database Routine Name\",\r\n        \"Namespace\" : enter" +
                    " \"Namespace\" or \"+\"\r\n    },\r\n  ],\r\n  \"BuildSchemas\": [\r\n    {\r\n      \"Schema\": \"" +
                    "funcs\",\r\n      \"Namespace\": \"Funcs\"\r\n    },\r\n    {\r\n      \"Schema\": \"procs\",\r\n  " +
                    "    \"Namespace\": \"Procs\"\r\n    }\r\n  ],\r\n  \"EnumQueries\": [\r\n    {\r\n      \"Name\": " +
                    "\"EnumerationName\",\r\n      \"Query\": \"SELECT [ColumnName] AS [Name], [ColumnName] " +
                    "AS [Value], [ColumnName] AS [Comment] FROM [TableName] WHERE...\"\r\n    },\r\n    {\r" +
                    "\n      \"Name\": \"EnumerationName\",\r\n      \"Query\": \"SELECT [ColumnName] AS [Name]" +
                    ", [ColumnName] AS [Value], [ColumnName] AS [Comment] FROM [TableName] WHERE...\"\r" +
                    "\n    }\r\n  ],\r\n  \"StaticQueries\": [\r\n    {\r\n      \"Name\": \"StaticDataName\",\r\n    " +
                    "  \"Query\": \"SELECT [Column], [Column], ... FROM [dbo].[Table] WHERE...\"\r\n    },\r" +
                    "\n    {\r\n      \"Name\": \"StaticDataName\",\r\n      \"Query\": \"SELECT [Column], [Colum" +
                    "n], ... FROM [dbo].[Table] WHERE...\"\r\n    }\r\n  ],\r\n  \"BuildOptions\": {\r\n    \"Imp" +
                    "lementIChangeTracking\": false,\r\n    \"ImplementIRevertibleChangeTracking\": false," +
                    "\r\n    \"ImplementINotifyPropertyChanged\": false,\r\n    \"IncludeAsyncServices\": fal" +
                    "se,\r\n    \"UseNullableReferenceTypes\": false\r\n  },\r\n  \"BuildRoutines\": [\r\n    {\r\n" +
                    "        \"Schema\": \"Database Schema\",\r\n        \"RoutineName\": \"Database Routine N" +
                    "ame\",\r\n        \"Namespace\": \"Namespace\" or \"+\"\r\n    },\r\n  ],\r\n  \"BuildSchemas\": " +
                    "[\r\n    {\r\n      \"Schema\": \"funcs\",\r\n      \"Namespace\": \"Funcs\"\r\n    },\r\n    {\r\n " +
                    "     \"Schema\": \"procs\",\r\n      \"Namespace\": \"Procs\"\r\n    }\r\n  ]\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class BuildDefinitionTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
