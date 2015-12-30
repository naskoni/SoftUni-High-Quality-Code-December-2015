using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using _2.Orders.Models;

namespace _2.Orders
{
    public class OrdersApplication
    {
        public static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var dataMapper = new DataMapper();
            var allCategories = dataMapper.GetAllCategories();
            var allProducts = dataMapper.GetAllProducts();
            var allOrders = dataMapper.GetAllOrders();
            
            var fiveMostExpensiveProducts = GetFiveMostExpensiveProducts(allProducts);
            Console.WriteLine(string.Join(Environment.NewLine, fiveMostExpensiveProducts));

            PrintSeparatorLine();
            
            PrintTheNumberOfProductsInEachCategory(allProducts, allCategories);

            PrintSeparatorLine();

            PrintTopFiveProductsByOrderQuantity(allOrders, allProducts);

            PrintSeparatorLine();

            PrintMostProfitableCategory(allOrders, allProducts, allCategories);
        }

        private static void PrintMostProfitableCategory(IEnumerable<Order> allOrders, IEnumerable<Product> allProducts, IEnumerable<Category> allCategories)
        {
            var mostProfitableCategory = allOrders
                .GroupBy(o => o.ProductId)
                .Select(g => new
                {
                    catId = allProducts.First(
                        p => p.Id == g.Key).CategoryId,
                    price = allProducts.First(p => p.Id == g.Key).UnitPrice,
                    quantity = g.Sum(p => p.Quantity)
                })
                .GroupBy(group => group.catId)
                .Select(group => new
                {
                    CategoryName = allCategories.First(
                        c => c.Id == group.Key).Name,
                    TotalQuantity = group.Sum(g => g.quantity * g.price)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .First();
            Console.WriteLine("{0}: {1}", mostProfitableCategory.CategoryName, mostProfitableCategory.TotalQuantity);
        }

        private static void PrintTopFiveProductsByOrderQuantity(IEnumerable<Order> allOrders, IEnumerable<Product> allProducts)
        {
            var topFiveProductsByOrderQuantity = allOrders
                .GroupBy(o => o.ProductId)
                .Select(group => new
                {
                    Product = allProducts.First(
                        p => p.Id == group.Key).Name,
                    Quantities = group.Sum(g => g.Quantity)
                })
                .OrderByDescending(q => q.Quantities)
                .Take(5);
            foreach (var item in topFiveProductsByOrderQuantity)
            {
                Console.WriteLine("{0}: {1}", item.Product, item.Quantities);
            }
        }

        private static void PrintTheNumberOfProductsInEachCategory(IEnumerable<Product> allProducts, IEnumerable<Category> allCategories)
        {
            var numberOfProductsInEachCategory = allProducts
                .GroupBy(p => p.CategoryId)
                .Select(group => new
                {
                    Category = allCategories.First(
                        c => c.Id == group.Key).Name,
                    Count = group.Count()
                })
                .ToList();
            foreach (var item in numberOfProductsInEachCategory)
            {
                Console.WriteLine("{0}: {1}", item.Category, item.Count);
            }
        }

        private static void PrintSeparatorLine()
        {
            Console.WriteLine(new string('-', 10));
        }

        private static IEnumerable<string> GetFiveMostExpensiveProducts(IEnumerable<Product> allProducts)
        {
            var fiveMostExpensiveProducts = allProducts
                .OrderByDescending(p => p.UnitPrice)
                .Take(5)
                .Select(p => p.Name);
            
            return fiveMostExpensiveProducts;
        }
    }
}
