using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreConsoleApp.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        // FK para Product
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        // FK para Order
        [Required]
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
    }
}

/* O atribuido `ProductId` é usado como chave-estrangeira para apontar ao Product de Id referenciado
 * chave-estrangeira: é um campo numa tabela (ou classe) que armazena o identificador de um registro em outra tabela. Serve para “ligar” dois objetos diferentes.
 * 
*/