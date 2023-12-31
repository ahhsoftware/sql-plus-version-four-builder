// --------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the SQL PLUS Code Generation Utility.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
//     Underlying Routine: MergeESCWorkOrderDispatches
//     Last Modified On: 6/28/2023 8:50:22 AM
//     Written By: Kirk Barrett
//     Visit https://www.SQLPLUS.net for more information about the SQL PLUS build time ORM.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------
namespace SQLPLUS.Build.Test.Basic.Sync
{
    #region Using Statments

    using SQLPLUS.Build.Test.Basic.Sync.Models;
    using SqlPlusBase;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;

    #endregion Using Statements

    /// <summary>
    /// This file contains the source code for the MergeESCWorkOrderDispatches routine.
    /// </summary>
    public partial class Service
    {
        #region Build SqlCommand

        private SqlCommand MergeESCWorkOrderDispatches_BuildCommand(SqlConnection cnn, MergeESCWorkOrderDispatchesInput input)
        {
            SqlCommand result = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[Sync].[MergeESCWorkOrderDispatches]",
                Connection = cnn
            };

            result.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@ReturnValue",
                Direction = ParameterDirection.ReturnValue,
                SqlDbType = SqlDbType.Int,
                Scale = 0,
                Precision = 10,
                Value = DBNull.Value
            });

            result.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@Data",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Structured,
                TypeName = "Sync.EDispatchTech",
                Value = DBNull.Value
            });

            result.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@OutMsg",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.NVarChar,
                Size = 2048,
                Value = DBNull.Value
            });

            if (input.Data != null)
            {
			    result.Parameters["@Data"].Value = Helpers.BuildDataTable(input.Data);
            }
            return result;
        }

        #endregion Build SqlCommand

        #region Read Output Parameters And Return Value

        private void MergeESCWorkOrderDispatches_SetParameters(SqlCommand cmd, MergeESCWorkOrderDispatchesOutput output)
        {
            if(cmd.Parameters[0].Value != DBNull.Value)
            {
                output.ReturnValue = (MergeESCWorkOrderDispatchesOutput.Returns)cmd.Parameters[0].Value;
            }
            if(cmd.Parameters[2].Value != DBNull.Value)
            {
                output.OutMsg = (string)cmd.Parameters[2].Value;
            }
        }

        #endregion Read Output Parameters And Return Value

        #region Execute Command

        private void MergeESCWorkOrderDispatches_Execute(SqlCommand cmd, MergeESCWorkOrderDispatchesOutput output)
        {
            cmd.ExecuteNonQuery();

            MergeESCWorkOrderDispatches_SetParameters(cmd, output);
        }

        #endregion Execute Command

        #region Public Service

        /// <summary>
        /// Merge ESC Dispatch Table to ESCWorkOrderDispatch<br/>
        /// DB Routine: Sync.MergeESCWorkOrderDispatches<br/>
        /// Author: Kirk Barrett<br/>
        /// </summary>
        /// <param name="input">MergeESCWorkOrderDispatchesInput instance.</param>
        /// <returns>Instance of MergeESCWorkOrderDispatchesOutput</returns>
        public MergeESCWorkOrderDispatchesOutput MergeESCWorkOrderDispatches(MergeESCWorkOrderDispatchesInput input)
        {
            ValidateInput(input, nameof(MergeESCWorkOrderDispatches));
            MergeESCWorkOrderDispatchesOutput output = new MergeESCWorkOrderDispatchesOutput();
			if(sqlConnection != null)
            {
                using (SqlCommand cmd = MergeESCWorkOrderDispatches_BuildCommand(sqlConnection, input))
                {
                    cmd.Transaction = sqlTransaction;
                    MergeESCWorkOrderDispatches_Execute(cmd, output);
                }
                return output;
            }
            for(int idx=0; idx <= retryOptions.RetryIntervals.Count; idx++)
            {
                if (idx > 0)
                {
                    Thread.Sleep(retryOptions.RetryIntervals[idx - 1]);
                }
                try
                {
                    using (SqlConnection cnn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = MergeESCWorkOrderDispatches_BuildCommand(cnn, input))
                    {
                        cnn.Open();
						MergeESCWorkOrderDispatches_Execute(cmd, output);
                        cnn.Close();
                    }
					break;
                }
                catch(SqlException sqlException)
                {
                    AllowRetryOrThrowError(idx, sqlException);
                }
            }
            return output;
        }

        #endregion

    }
}

