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
    
    #line 1 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class UserDefinedType : UserDefinedTypeBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"// --------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the SQL+ .NET Code Generation Utility.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
//     Underlying Type: [");
            
            #line 16 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.UserDefinedTypeSchema));
            
            #line default
            #line hidden
            this.Write("].[");
            
            #line 16 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.UserDefinedTypeName));
            
            #line default
            #line hidden
            this.Write("]\r\n//     For more information on SQL+ .NET visit https://www.SQLPlus.net\r\n// </a" +
                    "uto-generated>\r\n// -------------------------------------------------------------" +
                    "-------------------------------------------\r\nnamespace ");
            
            #line 20 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(project.UserDefinedTypeNamepace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n");
            
            #line 22 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
if (parameter.TVColumnsUsings.Count != 0){
            
            #line default
            #line hidden
            this.Write("    #region usings\r\n\r\n");
            
            #line 25 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
foreach(string @using in parameter.TVColumnsUsings){
            
            #line default
            #line hidden
            this.Write("    using ");
            
            #line 26 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(@using));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 27 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
}
            
            #line default
            #line hidden
            this.Write("\r\n    #endregion usings\r\n\r\n");
            
            #line 31 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
}
            
            #line default
            #line hidden
            this.Write("\t/// <summary>\r\n    /// Input object table value parameter ");
            
            #line 33 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.UserDefinedTypeName));
            
            #line default
            #line hidden
            this.Write(".\r\n    /// </summary>\r\n    public partial class ");
            
            #line 35 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.UserDefinedTypeName));
            
            #line default
            #line hidden
            this.Write("\r\n    {\r\n        #region Constructors\r\n\r\n        /// <summary>\r\n        /// Empty" +
                    " constructor for ");
            
            #line 40 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.UserDefinedTypeName));
            
            #line default
            #line hidden
            this.Write(".\r\n        /// </summary>\r\n        public ");
            
            #line 42 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.UserDefinedTypeName));
            
            #line default
            #line hidden
            this.Write("() { }\r\n\r\n        /// <summary>\r\n        /// Parameterized constructor for ");
            
            #line 45 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.UserDefinedTypeName));
            
            #line default
            #line hidden
            this.Write(".\r\n        /// </summary>\r\n");
            
            #line 47 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
foreach(Column c in parameter.TVColumns){
            
            #line default
            #line hidden
            this.Write("        /// <param name=\"");
            
            #line 48 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.PropertyName));
            
            #line default
            #line hidden
            this.Write("\">Maps to column ");
            
            #line 48 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.Name));
            
            #line default
            #line hidden
            this.Write(".</param>\r\n");
            
            #line 49 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
}
            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 50 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.UserDefinedTypeName));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 50 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(parameter.TVColumnsConcat));
            
            #line default
            #line hidden
            this.Write(")\r\n        {\r\n");
            
            #line 52 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
foreach(Column c in parameter.TVColumns){
            
            #line default
            #line hidden
            this.Write("            this.");
            
            #line 53 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.PropertyName));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 53 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.PropertyName));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 54 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
}
            
            #line default
            #line hidden
            this.Write("        }\r\n\r\n        #endregion Constructors\r\n\r\n        #region Fields\r\n\r\n");
            
            #line 61 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
foreach(Column c in parameter.TVColumns){
            
            #line default
            #line hidden
            this.Write("        /// <summary>\r\n        /// ");
            
            #line 63 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.Comment));
            
            #line default
            #line hidden
            this.Write("\r\n        /// </summary>\r\n");
            
            #line 65 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
  foreach(string annotation in c.Annotations){
            
            #line default
            #line hidden
            this.Write("        ");
            
            #line 66 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(annotation));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 67 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
  }
            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 68 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.PropertyType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 68 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.PropertyName));
            
            #line default
            #line hidden
            this.Write(" { set; get; }\r\n\r\n");
            
            #line 70 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"
}
            
            #line default
            #line hidden
            this.Write("        #endregion Fields\r\n    }\r\n}");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Users\Alan\source\repos\sql-plus-version-four-builder\SQLPLUS.Builder\Render\T4Net\UserDefinedType.tt"

private global::SQLPLUS.Builder.TemplateModels.Parameter _parameterField;

/// <summary>
/// Access the parameter parameter of the template.
/// </summary>
private global::SQLPLUS.Builder.TemplateModels.Parameter parameter
{
    get
    {
        return this._parameterField;
    }
}

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


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool parameterValueAcquired = false;
if (this.Session.ContainsKey("parameter"))
{
    this._parameterField = ((global::SQLPLUS.Builder.TemplateModels.Parameter)(this.Session["parameter"]));
    parameterValueAcquired = true;
}
if ((parameterValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("parameter");
    if ((data != null))
    {
        this._parameterField = ((global::SQLPLUS.Builder.TemplateModels.Parameter)(data));
    }
}
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
    public class UserDefinedTypeBase
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
