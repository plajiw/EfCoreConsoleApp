using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreConsoleApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderPlaced { get; set; } = DateTime.UtcNow;

        // Pode ser null até que o pedido seja cumprido
        public DateTime? OrderFulfilled { get; set; }

        // Chave estrangeira seguindo convenção
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [Required] // relacionamento obrigatório
        public Customer Customer { get; set; } = null!;

        // Navegação de coleção sempre iniciada
        public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}
