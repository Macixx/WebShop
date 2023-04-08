using System.Reflection.Metadata;

namespace WebApplication4.Areas.Identity.Data
{
    public class CartItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get;set; }
        public decimal Total
        {
            get { return Quantity * Price; }
        }
        public string Category { get; set; }
        public ShopItem ShopItem{ get; }
        public CartItems()
        {


        }

        public CartItems(ShopItem product)
        {
            decimal n1;
            Id = product.ID;
            Name = product.ItemName;
            Decimal.TryParse(product.ItemPrice, out n1);
            Price = n1;
            Quantity = 1;
            ImageUrl = product.ItemImage;
            Category = product.Category;





        }

    }
}
