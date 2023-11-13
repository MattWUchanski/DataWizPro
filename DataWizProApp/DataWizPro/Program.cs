using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DataWizPro.Models;
using DataWizPro.ProductServices;

namespace DataWizPro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string productName = "MacStudioM3Ultra";
            string description = "Description of Product1";
            decimal price = 10.99m;
            bool isAvailable = true;
            string version = "6";
            int id = 48;

            ProductService productService = new ProductService();

            // Create a new DataTable instance
            DataTable productsTable = new DataTable();

            // Define columns in the DataTable to match the SQL table type structure
            productsTable.Columns.Add("name", typeof(string));
            productsTable.Columns.Add("description", typeof(string));
            productsTable.Columns.Add("price", typeof(decimal));
            productsTable.Columns.Add("available", typeof(bool));

            // Add rows to the DataTable
            productsTable.Rows.Add("Product 345v" + version, "Description 2137v" + version, 21.37m, true);
            productsTable.Rows.Add("Product 567v" + version, "Description 2137v" + version, 37.21m, false);

            TestUpdateNameOfRecordWithQuery(productService, productName, id);

            //TestInsertNewProducts(productService, productsTable);

            //TestGetSpecificProductsWithSp(productService, productName, price, isAvailable);
            //TestGetSpecificProductsWithQuery(productService, productName, description);

            //TestGetMaxPriceWithSp(productService, isAvailable);

            //TestGetProductsAmountWithQuery(productService, isAvailable);



            Console.ReadLine();
        }


        private static void TestGetSpecificProductsWithSp(ProductService productService, string productName, decimal price, bool isAvailable)
        {
            List<Product> products = productService.GetSpecificProductsWithSp(productName, price, isAvailable);

            foreach (Product product in products)
            {
                ObjectPropertyPrinter.PrintProperties(product);
            }
        }

        private static void TestGetSpecificProductsWithQuery(ProductService productService, string productName, string description)
        {
            List<Product> products = productService.GetSpecificProductsWithQuery(productName, description);

            foreach (Product product in products)
            {
                ObjectPropertyPrinter.PrintProperties(product);
            }
        }

        private static void TestGetMaxPriceWithSp(ProductService productService, bool available)
        {
            double maxPrice = productService.GetMaxPriceWithSp(available);
            Console.WriteLine($"Max price: {maxPrice}");

        }

        private static void TestGetProductsAmountWithQuery(ProductService productService, bool available)
        {
            int productsAmount = productService.GetProductsAmountWithQuery(available);
            Console.WriteLine($"Products amount: {productsAmount}");
        }

        private static void TestInsertNewProducts(ProductService productService, DataTable productsTable)
        {
            productService.InsertNewProducts(productsTable);
            Console.WriteLine("Products inserted successfully.");
        }

        private static void TestUpdateNameOfRecordWithQuery(ProductService productService, string name, int id)
        {
            productService.UpdateNameOfRecordWithQuery(name, id);
            Console.WriteLine("Product name updated successfully.");
        }


    }
}
