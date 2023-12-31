// --------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the SQL  Code Generation Utility.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
//     Underlying Routine: MergeESCWorkOrderDispatches
//     Last Modified On: 6/28/2023 8:50:22 AM
//     Written By: Kirk Barrett
//     Select Type: NonQuery
//     Visit https://www.SQLPLUS.net for more information about the SQL Plus build time ORM.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------
namespace SQLPLUS.Build.Test.Basic.Sync.Models
{
    /// <summary>
    /// Output object for MergeESCWorkOrderDispatches service.
    /// </summary>
    public partial class MergeESCWorkOrderDispatchesOutput
    {

        #region Return Value Enumerations

        /// <summary>
        /// Enumerated return values based on the ReturnTags in the procedure.
        /// </summary>
        public enum Returns
        {
            /// <summary>
            /// Enumerated value for MergeDone
            /// </summary>
             MergeDone = 0,
            /// <summary>
            /// Enumerated value for MergeFail
            /// </summary>
             MergeFail = 9
        }

        #endregion Return Value Enumerations

        #region Output Parameters

        /// <summary>
        /// Maps to parameter @OutMsg.
        /// </summary>
        public string OutMsg { set; get; }

	    #endregion Output Parameters

        #region Return Value

        /// <summary>
        /// Enumerated return value.
        /// </summary>
        public Returns ReturnValue { set; get; }

        #endregion Return Value
    }









}
