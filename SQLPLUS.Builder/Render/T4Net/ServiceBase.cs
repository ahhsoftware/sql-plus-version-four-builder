﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace SQLPLUS.Builder.Render.T4Net
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using SQLPLUS.Builder;
    using SQLPLUS.Builder.ConfigurationModels;
    using SQLPLUS.Builder.TemplateModels;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class ServiceBase : ServiceBaseBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"// --------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the SQL PLUS Code Generation Utility.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
//     Visit https://www.SQLPLUS.net for more information about the SQL PLUS build time ORM.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------
namespace ");
            
            #line 19 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(nameSpace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n    #region usings\r\n\r\n");
            
            #line 23 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
foreach(string s in Usings()){
            
            #line default
            #line hidden
            this.Write("    using ");
            
            #line 24 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(s));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 25 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
}
            
            #line default
            #line hidden
            this.Write(@"
    #endregion usings

    #region Service Partial

    /// <summary>
    /// Non-service partial class contains constructors and utility functions available in all service specific partials.
    /// </summary>
    public partial class Service
    {

        #region Private Readonly Variables

");
            
            #line 39 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
if (build.LicenseType != "Community"){
            
            #line default
            #line hidden
            this.Write("        private readonly SqlConnection sqlConnection;\r\n        private readonly S" +
                    "qlTransaction sqlTransaction;\r\n");
            
            #line 42 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
}
            
            #line default
            #line hidden
            this.Write(@"        private readonly string connectionString;
        private readonly IRetryOptions retryOptions;

        #endregion Private Readonly Variables

        #region Contructors

        /// <summary>
        /// Creates a new service object that will connect to the database using the connection string provided.
        /// Note that additional contructors with options for transient error management and transactions are available in pro and enterprise versions.
        /// Upgrade here: https://www.SQLPlus.net
        /// </summary>
        /// <param name=""connectionString"">Connection String to the relevant database with appropriate credentials and settings.</param>
        public Service(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            this.connectionString = connectionString;
            this.retryOptions = new DefaultRetryOptions();
        }
");
            
            #line 63 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
if(build.LicenseType != "Community"){
            
            #line default
            #line hidden
            this.Write("\r\n        /// <summary>\r\n        /// Creates a new service object that will conne" +
                    "ct to the database using the connection string provided.\r\n        /// All servic" +
                    "e calls will execute using the retry options provided.\r\n        /// Visit https:" +
                    "//www.SQLPlus.net/ for more information on transient error management.\r\n        " +
                    "/// </summary>\r\n        /// <param name=\"connectionString\">Connection String to " +
                    "the relevant database with appropriate credentials and settings.</param>\r\n      " +
                    "  /// <param name=\"retryOptions\">Object implementing the IRetryOptions interface" +
                    ".</param>\r\n        public Service(string connectionString, IRetryOptions retryOp" +
                    "tions)\r\n        {\r\n            if (string.IsNullOrEmpty(connectionString))\r\n    " +
                    "        {\r\n                throw new ArgumentException($\"\'{nameof(connectionStri" +
                    "ng)}\' cannot be null or empty.\", nameof(connectionString));\r\n            }\r\n\r\n  " +
                    "          this.connectionString = connectionString;\r\n            this.retryOptio" +
                    "ns = retryOptions ?? throw new ArgumentNullException(nameof(retryOptions));\r\n   " +
                    "     }\r\n\r\n        /// <summary>\r\n        /// Creates a new service object that a" +
                    "llows developer control of the connection and transactions.\r\n        /// Visit h" +
                    "ttps://www.SQLPlus.net/ for more information on transaction management.\r\n       " +
                    " /// User is responsible for connection and transaction management.\r\n        ///" +
                    " </summary>\r\n        /// <param name=\"sqlConnection\">Ready to execute SqlConnect" +
                    "ion.</param>\r\n        /// <param name=\"sqlTransaction\">Ready to execute SqlTrans" +
                    "action.</param>\r\n        public Service(SqlConnection sqlConnection, SqlTransact" +
                    "ion sqlTransaction)\r\n        {\r\n            if (sqlConnection == null)\r\n        " +
                    "    {\r\n                throw new ArgumentNullException(nameof(sqlConnection));\r\n" +
                    "            }\r\n            if (sqlTransaction == null)\r\n            {\r\n         " +
                    "       throw new ArgumentNullException(nameof(sqlTransaction));\r\n            }\r\n" +
                    "            this.sqlConnection = sqlConnection;\r\n            this.sqlTransaction" +
                    " = sqlTransaction;\r\n        }\r\n");
            
            #line 103 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"
}
            
            #line default
            #line hidden
            this.Write("\r\n        #endregion Contructors\r\n\r\n        #region Default Retry Options\r\n\r\n    " +
                    "    /// <summary>\r\n        /// Default implementation of Retry Options. Used whe" +
                    "n no retry options are passed.\r\n        /// </summary>\r\n        private class De" +
                    "faultRetryOptions : RetryOptions\r\n        {\r\n            public DefaultRetryOpti" +
                    "ons() :\r\n            base(\r\n                new System.Collections.Generic.List<" +
                    "int>(),\r\n                new System.Collections.Generic.List<int>(),\r\n          " +
                    "      null\r\n                )\r\n            { }\r\n        }\r\n\r\n        #endregion " +
                    "Default Retry Options\r\n\r\n        #region Private Methods\r\n\r\n        /// <summary" +
                    ">\r\n        /// This method is called on every service call to validate the input" +
                    " prior to sumbitting to the database.\r\n        /// </summary>\r\n        /// <para" +
                    "m name=\"input\">Input object derive from ValidInput.</param>\r\n        /// <param " +
                    "name=\"method\">The method name where the validation is taking place.</param>\r\n   " +
                    "     private void ValidateInput(ValidInput input, string method)\r\n        {\r\n   " +
                    "         if(input is null)\r\n            {\r\n                throw new ArgumentNul" +
                    "lException($\"The input object passed to service method {method} cannot be null.\"" +
                    ", \"input\");\r\n            }\r\n            if (!input.IsValid())\r\n            {\r\n  " +
                    "              throw new ArgumentException($\"The input object passed to service m" +
                    "ethod {method} fails validation. Use the {method}Input.IsValid() method prior to" +
                    " calling the service.\", \"input\");\r\n            }\r\n        }\r\n\r\n        /// <summ" +
                    "ary>\r\n        /// This method checks a SQL exception to see it is to be treated " +
                    "as a transient error.\r\n        /// If it is infact a transient error the retry o" +
                    "ptions will determine if the exception is thrown or handled.\r\n        /// </summ" +
                    "ary>\r\n        /// <param name=\"idx\">Current index in the currently executing ser" +
                    "vice.</param>\r\n        /// <param name=\"exception\">The SqlException that was the" +
                    " source of the call.</param>\r\n        private void AllowRetryOrThrowError(int id" +
                    "x, SqlException exception)\r\n        {\r\n            bool throwException = true;\r\n" +
                    "\r\n            if (retryOptions.TransientErrorNumbers.Contains(exception.Number))" +
                    "\r\n            {\r\n                throwException = (idx == retryOptions.RetryInte" +
                    "rvals.Count);\r\n\r\n                if (retryOptions.Logger != null)\r\n             " +
                    "   {\r\n                    retryOptions.Logger.Log(exception);\r\n                }" +
                    "\r\n            }\r\n            if (throwException)\r\n            {\r\n               " +
                    " throw exception;\r\n            }\r\n        }\r\n\r\n        #endregion Private Method" +
                    "s\r\n\r\n    }\r\n\r\n    #endregion Service Partial\r\n}\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 176 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"

    private List<string> Usings()
    {
        List<string> result = new List<string>();
        result.Add("System");
        result.Add("System.Collections.Generic");
        result.Add(project.SQLPLUSBaseNamespace);
        result.Add(build.SQLClientNamespace);
        result.Add(build.SQLExceptionNamespace);
        return result;
    }

        
        #line default
        #line hidden
        
        #line 1 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\ServiceBase.tt"

private global::SQLPLUS.Builder.ConfigurationModels.ProjectInformation _projectField;

/// <summary>
/// Access the project parameter of the template.
/// </summary>
private global::SQLPLUS.Builder.ConfigurationModels.ProjectInformation project
{
    get
    {
        return this._projectField;
    }
}

private global::SQLPLUS.Builder.ConfigurationModels.BuildDefinition _buildField;

/// <summary>
/// Access the build parameter of the template.
/// </summary>
private global::SQLPLUS.Builder.ConfigurationModels.BuildDefinition build
{
    get
    {
        return this._buildField;
    }
}

private string _nameSpaceField;

/// <summary>
/// Access the nameSpace parameter of the template.
/// </summary>
private string nameSpace
{
    get
    {
        return this._nameSpaceField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool projectValueAcquired = false;
if (this.Session.ContainsKey("project"))
{
    this._projectField = ((global::SQLPLUS.Builder.ConfigurationModels.ProjectInformation)(this.Session["project"]));
    projectValueAcquired = true;
}
if ((projectValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("project");
    if ((data != null))
    {
        this._projectField = ((global::SQLPLUS.Builder.ConfigurationModels.ProjectInformation)(data));
    }
}
bool buildValueAcquired = false;
if (this.Session.ContainsKey("build"))
{
    this._buildField = ((global::SQLPLUS.Builder.ConfigurationModels.BuildDefinition)(this.Session["build"]));
    buildValueAcquired = true;
}
if ((buildValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("build");
    if ((data != null))
    {
        this._buildField = ((global::SQLPLUS.Builder.ConfigurationModels.BuildDefinition)(data));
    }
}
bool nameSpaceValueAcquired = false;
if (this.Session.ContainsKey("nameSpace"))
{
    this._nameSpaceField = ((string)(this.Session["nameSpace"]));
    nameSpaceValueAcquired = true;
}
if ((nameSpaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("nameSpace");
    if ((data != null))
    {
        this._nameSpaceField = ((string)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class ServiceBaseBase
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
