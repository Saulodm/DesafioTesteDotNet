using Core.Enums;

namespace Core.Models
{
    public class Product
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public int StockQuantity { get; set; }

        public Product()
        {

        }

        public Product(string name, decimal price, Category category, int stockQauntity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Category = category;
            StockQuantity = stockQauntity;
        }
    }
}
