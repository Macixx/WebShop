using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Imaging;

namespace WebApplication4.Areas.Identity.Data
{
    public class ShopItem
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        public string? ItemName { get; set; }
        [DataType(DataType.Currency)]
        //[Range(0, 100000)]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        //[Column(TypeName = "decimal(2, 2)")]
        public string ?ItemPrice { get; set; }
        [Required]
        [Range(0,100000)]
        public int Count { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        public string ?ItemImage { get; set; }
        [Required]
        public string ?Category { get; set; }
        [Required]
        public string  ?Producer { get; set; }
        [Required]
        public int Sales { get; set; } = 0;
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime whenAdded { get; set; }
        

        
        


    }
}
