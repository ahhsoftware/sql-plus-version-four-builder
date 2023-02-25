// --------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the SQL PLUS Code Generation Utility.
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
//     Underlying Routine: CustomerPasswordById
//     Last Modified On: 2/22/2023 9:45:17 AM
//     Written By: Alan
//     Visit https://www.SQLPLUS.net for more information about the SQL PLUS build time ORM.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------
namespace SQLPlus.AzureFunctions.Default
{
    #region Using Statments

    using SQLPlus.AzureFunctions.Default.Models;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;

    #endregion Using Statements

    /// <summary>
    /// This file contains the source code for the CustomerPasswordById routine.
    /// </summary>
    public partial class Service
    {
        #region Build SqlCommand

        private SqlCommand CustomerPasswordById_BuildCommand(SqlConnection cnn, CustomerPasswordByIdInput input)
        {
            SqlCommand result = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[CustomerPasswordById]",
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
                ParameterName = "@CustomerId",
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.Int,
                Scale = 0,
                Precision = 10,
                Value = input.CustomerId
            });

            return result;
        }

        #endregion Build SqlCommand

        #region Read Output Parameters And Return Value

        private void CustomerPasswordById_SetParameters(SqlCommand cmd, CustomerPasswordByIdOutput output)
        {
            if(cmd.Parameters[0].Value != DBNull.Value)
            {
                output.ReturnValue = (CustomerPasswordByIdOutput.Returns)cmd.Parameters[0].Value;
            }
        }

        #endregion Read Output Parameters And Return Value

        #region Reader To Result Objects
        
        private CustomerPasswordByIdResult CustomerPasswordById_ResultData(SqlDataReader rdr)
        {
            return new CustomerPasswordByIdResult(
            rdr.GetInt32(0),
            (byte[])rdr[1],
            (byte[])rdr[2]
            );
        }
    
        #endregion Reader To Result Objects

        #region Execute Command

        private void CustomerPasswordById_Execute(SqlCommand cmd, CustomerPasswordByIdOutput output)
        {
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if(rdr.Read())
                {
                    output.ResultData = CustomerPasswordById_ResultData(rdr);
                }
                rdr.Close();
            }

            CustomerPasswordById_SetParameters(cmd, output);
        }

        #endregion Execute Command

        #region Public Service

        /// <summary>
        /// Password information for the given customer Email.<br/>
        /// DB Routine: dbo.CustomerPasswordById<br/>
        /// Author: Alan<br/>
        /// </summary>
        /// <param name="input">CustomerPasswordByIdInput instance.</param>
        /// <returns>Instance of CustomerPasswordByIdOutput</returns>
        public CustomerPasswordByIdOutput CustomerPasswordById(CustomerPasswordByIdInput input)
        {
            ValidateInput(input, nameof(CustomerPasswordById));
            CustomerPasswordByIdOutput output = new CustomerPasswordByIdOutput();
			if(sqlConnection != null)
            {
                using (SqlCommand cmd = CustomerPasswordById_BuildCommand(sqlConnection, input))
                {
                    cmd.Transaction = sqlTransaction;
                    CustomerPasswordById_Execute(cmd, output);
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
                    using (SqlCommand cmd = CustomerPasswordById_BuildCommand(cnn, input))
                    {
                        cnn.Open();
						CustomerPasswordById_Execute(cmd, output);
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

