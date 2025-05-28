using System.ComponentModel.DataAnnotations;

namespace EfCoreConsoleApp.Models
{
    public class Customer
    {
        // Chave primária
        public int Id { get; set; }

        // Nome próprio obrigatório, não nulo
        [Required]
        public string FirstName { get; set; } = null!;

        // Sobrenome obrigatório, não nulo
        [Required]
        public string LastName { get; set; } = null!;

        // Endereço opcional
        public string? Address { get; set; }

        // Telefone opcional
        public string? Phone { get; set; }

        public string? Email { get; set; }

        // Coleção de pedidos sempre inicializada. Navegação 1:N para Order
        // Coleção inicializada para evitar NullReferenceException
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

/*
É uma interface genérica que herda de IEnumerable<T> e adiciona operações de modificação (adicionar, remover, verificar existência) e propriedades de contagem. 
Define o contrato mínimo para coleções que podem ser alteradas.
*/