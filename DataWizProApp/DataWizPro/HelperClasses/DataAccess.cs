using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessNamespace
{
    public class DataAccess
    {
        private readonly string _connectionString;
        private readonly SqlParameterManager _parameterManager;

        public DataAccess(SqlParameterManager parameterManager)
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _parameterManager = parameterManager;
        }

        // Bez transakcji
        public DataTable CallSpForDt(string storedProcedureName, Dictionary<string, object> sqlParameters)
        {
            var parameters = _parameterManager.GetParameters(storedProcedureName, sqlParameters);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // Bez transakcji
        public DataTable CallQueryForDt(string query, Dictionary<string, object> queryParameters)
        {
            var parameters = _parameterManager.GetParameters(query, queryParameters);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // Bez transakcji
        public object CallSpForScalar(string storedProcedureName, Dictionary<string, object> queryParameters)
        {
            var parameters = _parameterManager.GetParameters(storedProcedureName, queryParameters);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        // Bez transakcji
        public object CallQueryForScalar(string query, Dictionary<string, object> queryParameters)
        {
            var parameters = _parameterManager.GetParameters(query, queryParameters);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    return cmd.ExecuteScalar();
                }
            }
        }

        // Bez transakcji
        public void ExecSp(string storedProcedureName, Dictionary<string, object> sqlParameters)
        {
            var parameters = _parameterManager.GetParameters(storedProcedureName, sqlParameters);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Z transakcją
        public DataTable CallSpForDt(string storedProcedureName, Dictionary<string, object> sqlParameters, SqlConnection conn, SqlTransaction transaction)
        {
            List<SqlParameter> parameters = _parameterManager.GetParameters(storedProcedureName, sqlParameters);

            using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
            {
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        // Z transakcją
        public void ExecSp(string storedProcedureName, Dictionary<string, object> sqlParameters, SqlConnection conn, SqlTransaction transaction)
        {
            List<SqlParameter> parameters = _parameterManager.GetParameters(storedProcedureName, sqlParameters);

            using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                cmd.ExecuteNonQuery();
            }
        }

        public void ExecuteInTransaction(Action<SqlConnection, SqlTransaction> action)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        action(conn, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        try
                        {
                            if (transaction.Connection != null) // Check if transaction is still active
                            {
                                transaction.Rollback();
                            }
                        }
                        catch (Exception rollbackEx)
                        {
                            // Log the rollback exception
                        }
                        throw;
                    }
                }
            }
        }

    }


    public class ParameterDefinition
    {
        public string Name { get; set; }
        public SqlDbType Type { get; set; }
        public string TypeName { get; set; } // Only used for table-valued parameters

        public ParameterDefinition(string name, SqlDbType type, string typeName = null)
        {
            Name = name;
            Type = type;
            TypeName = typeName;
        }
    }

    public class SqlParameterManager
    {
        private Dictionary<string, List<ParameterDefinition>> parameters;

        public SqlParameterManager()
        {
            parameters = new Dictionary<string, List<ParameterDefinition>>();
        }

        public List<SqlParameter> GetParameters(string procedureName, Dictionary<string, object> providedParameters)
        {
            if (parameters.ContainsKey(procedureName))
            {
                var requiredParameters = parameters[procedureName];
                var sqlParameters = new List<SqlParameter>();

                foreach (var paramDef in requiredParameters)
                {
                    if (providedParameters != null && providedParameters.ContainsKey(paramDef.Name))
                    {
                        var param = new SqlParameter(paramDef.Name, paramDef.Type)
                        {
                            Value = providedParameters[paramDef.Name] ?? DBNull.Value
                        };

                        // Set TypeName for table-valued parameters
                        if (paramDef.Type == SqlDbType.Structured)
                        {
                            param.TypeName = paramDef.TypeName;
                        }

                        sqlParameters.Add(param);
                    }
                    else
                    {
                        throw new ArgumentException($"Parameter '{paramDef.Name}' is required for '{procedureName}'.");
                    }
                }

                return sqlParameters;
            }

            return new List<SqlParameter>();
        }

        // Method to add parameters for a specific procedure or query
        public void AddParameters(string procedureName, List<ParameterDefinition> parameterDefinitions)
        {
            parameters[procedureName] = parameterDefinitions;
        }

        // Method to remove parameters for a specific procedure or query
        public void RemoveParameters(string procedureName)
        {
            parameters.Remove(procedureName);
        }

    }
}


