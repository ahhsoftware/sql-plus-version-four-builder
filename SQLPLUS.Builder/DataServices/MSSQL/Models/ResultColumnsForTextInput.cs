// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by SqlPlus.net
//     For more information on SqlPlus.net visit http://www.SqlPlus.net
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SQLPLUS.Builder.DataServices.MSSQL.Models
{
    /// <summary>
    /// Interface for ResultColumnsForTextInput object.
    /// </summary>
    public interface IResultColumnsForTextInput : IValidInput
    {
        string SQLText { set; get; }
    }

    /// <summary>
    /// Input object for ResultColumnsForText method.
    /// </summary>
    public class ResultColumnsForTextInput : ValidInput, IResultColumnsForTextInput
    {
        /// <summary>
        /// When using for Procedures
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string SQLText { set; get; }

    }
} 