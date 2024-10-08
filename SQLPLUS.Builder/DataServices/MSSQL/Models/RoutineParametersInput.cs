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
    /// Interface for RoutineParametersInput object.
    /// </summary>
    public interface IRoutineParametersInput : IValidInput
    {
        string Schema { set; get; }
        string Name { set; get; }
    }

    /// <summary>
    /// Input object for RoutineParameters method.
    /// </summary>
    public class RoutineParametersInput : ValidInput, IRoutineParametersInput
    {
        /// <summary>
        /// The schema name (SPECIFIC_SCHEMA)
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Schema { set; get; }

        /// <summary>
        /// The routine name (SPECIFIC_NAME)
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { set; get; }

    }
} 