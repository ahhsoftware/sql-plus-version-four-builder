// --------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the SQL PLUS Code Generation Utility.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
//     Underlying Routine: MergeESCWorkOrders
//     Last Modified On: 6/28/2023 8:50:22 AM
//     Written By: Kirk Barrett
//     Visit https://www.SQLPLUS.net for more information about the SQL PLUS build time ORM.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------
namespace SQLPLUS.Build.Test.Basic.Sync.Models
{
    #region usings

    using SQLPLUS.Build.Test.Basic.UserDefinedTypes;
    using SqlPlusBase;
    using System.Collections.Generic;

    #endregion usings

    /// <summary>
    /// Input object for the MergeESCWorkOrders service.
    /// </summary>
    public partial class MergeESCWorkOrdersInput : ValidInput
    {
        #region Constructors

        /// <summary>
        /// Empty constructor for MergeESCWorkOrdersInput.
        /// </summary>
        public MergeESCWorkOrdersInput() { }

        /// <summary>
        /// Parameterized constructor for MergeESCWorkOrdersInput.
        /// </summary>
        /// <param name="Data">Maps to parameter @Data.</param>
        public MergeESCWorkOrdersInput(List<EDispatch> Data)
        {
            this.Data = Data;
        }

        #endregion Constructors

        #region Fields

        private List<EDispatch> _Data;
        /// <summary>
        /// Maps to parameter @Data.
        /// </summary>
        public List<EDispatch> Data
        {
            get => _Data;
            set => _Data = value;
        }

        #endregion

    }
}