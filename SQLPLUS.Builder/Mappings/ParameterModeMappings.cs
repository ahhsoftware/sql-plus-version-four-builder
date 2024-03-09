using System.Collections.Generic;

namespace SQLPLUS.Builder.Mappings
{
    public class ParameterModeMappings
    {
        public static readonly List<ParameterModeMapping> Mappings = new List<ParameterModeMapping>()
        {
            // This is the default declaration
            new ParameterModeMapping()
            {
                MSSQLParameterMode = "IN",
                AdoDirection = "ParameterDirection.Input",
                IsInput = true,
                IsOutput = false,
                IsReturnValue = false
            },
            new ParameterModeMapping()
            {
                MSSQLParameterMode = "INOUT",
                AdoDirection = "ParameterDirection.InputOutput",
                IsInput = true,
                IsOutput = true,
                IsReturnValue = false
            },
            new ParameterModeMapping()
            {
                // MSSQL Special case. This has to be the return value of a scalar function.
                MSSQLParameterMode = "OUT",
                AdoDirection = "ParameterDirection.ReturnValue",
                IsInput = false,
                IsOutput = false,
                IsReturnValue = true,
                IsScalarReturnValue = true
            },
            new ParameterModeMapping()
            {
                // MSSQL Special case. This is Synthetic and is swapped out when the native parameter is INOUT but the desired mode is exclusively output.
                MSSQLParameterMode = "OUTPUT",
                AdoDirection = "ParameterDirection.Output",
                IsInput = false,
                IsOutput = true,
                IsReturnValue = false
            },
            new ParameterModeMapping()
            {
                // MSSQL Special case. All procedures have a return value that is not part of the parameters collection and is not returned in the meta data queries.
                // The value RETURN is Synthetic in this case
                MSSQLParameterMode = "RETURN",
                AdoDirection = "ParameterDirection.ReturnValue",
                IsInput = false,
                IsOutput = false,
                IsReturnValue = true,
                IsProcedureReturnValue = true
            },
            new ParameterModeMapping()
            {
                // MSSQL Special case. When using Queries this allows treating a variable marked output as the return value.
                // The value RETURN is Synthetic in this case
                MSSQLParameterMode = "OUTPUTASRETURN",
                AdoDirection = "ParameterDirection.Output",
                IsInput = false,
                IsOutput = true,
                IsReturnValue = false
            }
        };
    }

    public class ParameterModeMapping
    {
        /// <summary>
        /// WHEN MSSQL
        /// IN - the parameter default.
        /// INOUT - a parameter is marked with the out attribute. When this is the case
        /// the argument passed can be either inout or out depending on the presence of the --+Input tag
        /// OUT - this is the return value of a scalar function
        /// </summary>
        public string MSSQLParameterMode { set; get; }

        /// <summary>
        /// This is the direction for the command
        /// </summary>
        public string AdoDirection { set; get; }

        public bool IsInput { set; get; }

        public bool IsOutput { set; get; }

        public bool IsReturnValue { set; get; }

        public bool IsProcedureReturnValue { set; get; }

        public bool IsScalarReturnValue { set; get; }


        /*

        //public string PostgressParameterMode { set; get; }

        //public string MySQLParameterMode { set; get; }
        
        */
    }
}
