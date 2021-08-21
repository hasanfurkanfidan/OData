using Default;
using System;

namespace ODataConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceRoot = "https://localhost:44315/odata";
            var context = new Container(new Uri(serviceRoot));
            var products = context.Products.Execute();
            var filteredProducts = context.Products.AddQueryOption("$filter", "Id eq 1");
            foreach (var item in filteredProducts)
            {
                Console.WriteLine(item.Name);
            }
            
        }
    }
}
