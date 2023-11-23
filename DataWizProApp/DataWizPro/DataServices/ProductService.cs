using System.Collections.Generic;
using System.Data;
using DataAccessNamespace;
using DataWizPro.Models;

namespace DataWizPro.ProductServices
{
    public static class StoredProcedures
    {
        public static readonly string GetSpecificProductsWithSp = "[net_app].[GetProductsOfSpecificType]";
        public static readonly string GetAllProductsWithSp = "[net_app].[GetAllProductsWithoutParams]";
        public static readonly string GetMaxPriceWithSp = "[net_app].[GetMaxPrice]";
        public static readonly string InsertNewProductsWithSp = "[net_app].[InsertProducts]";
        public static readonly string InsertNewUserWithSp = "[net_app].[InsertUsers]";
        
        // ... Add more stored procedure names as needed
    }

    public static class QueryDefinitions
    {
        public static readonly string GetSpecificProductsWithQuery = "SELECT * FROM [net_app].[Products] WHERE [name] = @name AND [description] = @description";
        public static readonly string GetAllProductsWithQuery = "SELECT * FROM [net_app].[Products]";
        public static readonly string GetProductsAmountWithQuery = "SELECT COUNT(*) as LiczbaProduktow FROM [net_app].[Products] WHERE available = @available";
        public static readonly string UpdateNameOfRecord = "UPDATE [net_app].[Products] SET name = @name WHERE id = @id";
        // ... Add more query definitions as needed

        public static readonly string GetUsersWithQuery = "SELECT [Name], [Role], [Age], [Status] FROM [net_app].[UsersExtended] WHERE Name = @name";
    }


    public class ProductService
    {
        private readonly DataAccess _dataAccess;
        private readonly SqlParameterManager _parameterManager;

        public ProductService()
        {
            _parameterManager = new SqlParameterManager();
            AddProductServiceParameters();


            _dataAccess = new DataAccess(_parameterManager);
        }

        private void AddProductServiceParameters()
        {
            // Example of adding parameters for a specific query
            var specificProductsWithSp = new List<ParameterDefinition>
            {
                new ParameterDefinition("name", SqlDbType.VarChar),
                new ParameterDefinition("price", SqlDbType.Decimal),
                new ParameterDefinition("available", SqlDbType.Bit)
                // ... other parameters
            };

            _parameterManager.AddParameters(StoredProcedures.GetSpecificProductsWithSp, specificProductsWithSp);


            var specificProductsWithQuery = new List<ParameterDefinition>
            {
                new ParameterDefinition("@name", SqlDbType.VarChar),
                new ParameterDefinition("@description", SqlDbType.VarChar),
            };

            _parameterManager.AddParameters(QueryDefinitions.GetSpecificProductsWithQuery, specificProductsWithQuery);

            var maxPriceWithSp = new List<ParameterDefinition>
            {
                new ParameterDefinition("@available", SqlDbType.Bit),
            };

            _parameterManager.AddParameters(StoredProcedures.GetMaxPriceWithSp, maxPriceWithSp);

            var productsAmountWithQuery = new List<ParameterDefinition>
            {
                new ParameterDefinition("@available", SqlDbType.Bit),
            };

            _parameterManager.AddParameters(QueryDefinitions.GetProductsAmountWithQuery, productsAmountWithQuery);

            var insertNewProducts = new List<ParameterDefinition>
            {
                new ParameterDefinition("@ProductList", SqlDbType.Structured, "[net_app].[ProductType]"),
            };

            _parameterManager.AddParameters(StoredProcedures.InsertNewProductsWithSp, insertNewProducts);

            var updateNameOfRecordWithQuery = new List<ParameterDefinition>
            {
                new ParameterDefinition("@name", SqlDbType.VarChar),
                new ParameterDefinition("@id", SqlDbType.Int),
            };

            _parameterManager.AddParameters(QueryDefinitions.UpdateNameOfRecord, updateNameOfRecordWithQuery);

            var getUsersWithQueryParams = new List<ParameterDefinition>
            {
                new ParameterDefinition("@name", SqlDbType.VarChar),
            };

            _parameterManager.AddParameters(QueryDefinitions.GetUsersWithQuery, getUsersWithQueryParams);

        }

        public UserExtended GetUsersWithQuery(string name)
        {
            string query = QueryDefinitions.GetUsersWithQuery;
            var parameters = new Dictionary<string, object>
            {
                { "@name", name }
            };
            DataTable dataTable = _dataAccess.CallQueryForDt(query, parameters);
            DataRow row = dataTable.Rows[0];
            return DataTransformer.ConvertToClass<UserExtended>(row);
        }

        public void UpdateNameOfRecordWithQuery(string name, int id)
        {
            string query = QueryDefinitions.UpdateNameOfRecord;
            var parameters = new Dictionary<string, object>
            {
                { "@name", name },
                { "@id", id }
            };
            _dataAccess.ExecQuery(query, parameters);
        }

        public int GetProductsAmountWithQuery(bool available)
        {
            string query = QueryDefinitions.GetProductsAmountWithQuery;
            var parameters = new Dictionary<string, object>
            {
                { "@available", available }
            };
            object productsAmount = _dataAccess.CallQueryForScalar(query, parameters);
            return DataTransformer.ToInt(productsAmount);
        }

        public double GetMaxPriceWithSp(bool available)
        {
            string sp = StoredProcedures.GetMaxPriceWithSp;
            var parameters = new Dictionary<string, object>
            {
                { "@available", available }
            };
            object maxPrice = _dataAccess.CallSpForScalar(sp, parameters);
            return DataTransformer.ToDouble(maxPrice);
        }

        public List<Product> GetAllProducts()
        {
            string query = QueryDefinitions.GetAllProductsWithQuery;
            DataTable dataTable = _dataAccess.CallQueryForDt(query, null);
            return DataTransformer.ConvertToList<Product>(dataTable);
        }

        public List<Product> GetAllProductsWithSp()
        {
            string sp = StoredProcedures.GetAllProductsWithSp;
            return ErrorHandler.ExecuteWithHandling(() =>
            {
                DataTable dataTable = _dataAccess.CallSpForDt(sp, null);
                return DataTransformer.ConvertToList<Product>(dataTable);
            }, new List<Product>());
        }

        //public Dictionary<int, Dictionary<string, object>> GetAllProductsWithSpDict()
        //{
        //    string sp = StoredProcedures.GetAllProductsWithSp;

        //    DataTable dataTable = _dataAccess.CallStoredProcedureForDataTable(sp, null);
        //    return DataTransformer.ConvertToDictionary<int>(dataTable, "Id");
        //}

        public List<Product> GetSpecificProductsWithSp(string name, decimal price, bool available)
        {
            string sp = StoredProcedures.GetSpecificProductsWithSp;
            var parameters = new Dictionary<string, object>
            {
                { "name", name },
                { "price", price },
                { "available", available }
            };
            DataTable dataTable = _dataAccess.CallSpForDt(sp, parameters);
            return DataTransformer.ConvertToList<Product>(dataTable);
        }

        public List<Product> GetSpecificProductsWithQuery(string name, string description)
        {
            string query = QueryDefinitions.GetSpecificProductsWithQuery;
            var parameters = new Dictionary<string, object>
                {
                    { "@name", name },
                    { "@description", description },
                };
            DataTable dataTable = _dataAccess.CallQueryForDt(query, parameters);
            return DataTransformer.ConvertToList<Product>(dataTable);
        }

        public void InsertNewProducts(DataTable dataTable)
        {
            string sp = StoredProcedures.InsertNewProductsWithSp;
            var parameters = new Dictionary<string, object>
                {
                    { "@ProductList", dataTable }
                };
            _dataAccess.ExecSp(sp, parameters);
        }

        //public void InsertNewProductAndUser(DataTable products, DataTable users)
        //{
        //    string spProducts = StoredProcedures.InsertNewProductsWithSp;
        //    var parametersProducts = new Dictionary<string, object>
        //        {
        //            { "@ProductList", products }
        //        };

        //    string spUsers = StoredProcedures.InsertNewUserWithSp;
        //    var parametersUsers = new Dictionary<string, object>
        //        {
        //            { "userTable", users }
        //        };

        //    _dataAccess.ExecuteInTransaction((conn, transaction) =>
        //    {
        //        _dataAccess.ExecuteStoredProcedure(spProducts, parametersProducts, conn, transaction);
        //        _dataAccess.ExecuteStoredProcedure(spUsers, parametersUsers, conn, transaction);
        //        // More calls as needed
        //    });



        //_dataAccess.ExecuteStoredProcedure(sp, parameters);
    }
}





