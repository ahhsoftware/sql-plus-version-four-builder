// --------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the SQL  Code Generation Utility.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
//     Underlying Routine: CustomerPasswordById
//     Last Modified On: 2/22/2023 9:45:17 AM
//     Written By: Alan
//     Select Type: SingleRow
//     Visit https://www.SQLPLUS.net for more information about the SQL Plus build time ORM.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------
namespace SQLPlus.AzureFunctions.Default.Models
{
    /// <summary>
    /// Output object for CustomerPasswordById service.
    /// </summary>
    public partial class CustomerPasswordByIdOutput
    {

        #region Return Value Enumerations

        /// <summary>
        /// Enumerated return values based on the ReturnTags in the procedure.
        /// </summary>
        public enum Returns
        {
            /// <summary>
            /// Enumerated value for NotFound
            /// </summary>
             NotFound = 0,
            /// <summary>
            /// Enumerated value for Ok
            /// </summary>
             Ok = 1
        }

        #endregion Return Value Enumerations

        #region Return Value

        /// <summary>
        /// Enumerated return value.
        /// </summary>
        public Returns ReturnValue { set; get; }

        #endregion Return Value

        #region Result Data

        /// <summary>
        /// Single instance of CustomerPasswordByIdResult.
        /// </summary>
        public CustomerPasswordByIdResult ResultData { set; get; }

        #endregion Result Data
    }



    #region Result Set Objects

    /// <summary>
    /// Result object for CustomerPasswordById service.
    /// </summary>




    public partial class CustomerPasswordByIdResult
	{
        /// <summary>
        /// Result set object for CustomerPasswordById.
        /// </summary>
        /// <param name="PasswordIterations">Maps to table value column PasswordIterations.</param>
        /// <param name="PasswordSalt">Maps to table value column PasswordSalt.</param>
        /// <param name="PasswordHash">Maps to table value column PasswordHash.</param>
        public CustomerPasswordByIdResult(int PasswordIterations, byte[] PasswordSalt, byte[] PasswordHash)
        {
             this.PasswordIterations = PasswordIterations;
             this.PasswordSalt = PasswordSalt;
             this.PasswordHash = PasswordHash;
        }


        /// <summary>
        /// Maps to table value column PasswordIterations.
        /// </summary>
        public int PasswordIterations { set; get; }

        /// <summary>
        /// Maps to table value column PasswordSalt.
        /// </summary>
        public byte[] PasswordSalt { set; get; }

        /// <summary>
        /// Maps to table value column PasswordHash.
        /// </summary>
        public byte[] PasswordHash { set; get; }
    }


    #endregion Result Set Objects

}
