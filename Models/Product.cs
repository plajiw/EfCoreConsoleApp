using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreConsoleApp.Models
{
    public class Product
    {
        // Chave primária
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
    }
}
