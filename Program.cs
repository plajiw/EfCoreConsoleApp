using EfCoreConsoleApp.Data;
using EfCoreConsoleApp.Models;
using System;

namespace EfCoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Cria uma instância do contexto do banco de dados
                using (var context = new EfCoreConsoleAppContext())
                {
                    // Cria um cliente
                    var customer = new Customer
                    {
                        FirstName = "João",
                        LastName = "Silva",
                        Address = "Rua das Pizzas, 123",
                        Phone = "987654321"
                    };
                    context.Customers.Add(customer);
                    context.SaveChanges();
                    Console.WriteLine($"Cliente {customer.FirstName} {customer.LastName} adicionado com ID: {customer.Id}");

                    // Cria o produto "Veggie Special Pizza"
                    var veggieSpecial = new Product
                    {
                        Name = "Veggie Special Pizza",
                        Price = 9.99m
                    };
                    context.Products.Add(veggieSpecial);
                    context.SaveChanges();
                    Console.WriteLine($"Produto {veggieSpecial.Name} adicionado com ID: {veggieSpecial.Id}");

                    // Cria um pedido associado ao cliente
                    var order = new Order
                    {
                        OrderPlaced = DateTime.UtcNow,
                        CustomerId = customer.Id
                    };
                    context.Orders.Add(order);
                    context.SaveChanges();
                    Console.WriteLine($"Pedido criado com ID: {order.Id} em {order.OrderPlaced}");

                    // Adiciona um detalhe do pedido com a pizza
                    var orderDetail = new OrderDetail
                    {
                        Quantity = 2, // Exemplo: 2 pizzas
                        ProductId = veggieSpecial.Id,
                        OrderId = order.Id
                    };
                    context.OrderDetails.Add(orderDetail);
                    context.SaveChanges();
                    Console.WriteLine($"Detalhe do pedido adicionado: {orderDetail.Quantity} x {veggieSpecial.Name}");

                    // Lista todos os pedidos do cliente para verificação
                    Console.WriteLine("\nResumo dos pedidos do cliente:");
                    var orders = context.Orders
                        .Where(o => o.CustomerId == customer.Id)
                        .Select(o => new
                        {
                            o.Id,
                            o.OrderPlaced,
                            Details = o.OrderDetails.Select(od => new
                            {
                                od.Quantity,
                                ProductName = od.Product.Name,
                                ProductPrice = od.Product.Price
                            })
                        })
                        .ToList();

                    foreach (var o in orders)
                    {
                        Console.WriteLine($"Pedido ID: {o.Id}, Data: {o.OrderPlaced}");
                        foreach (var detail in o.Details)
                        {
                            Console.WriteLine($"  - {detail.Quantity} x {detail.ProductName} (R${detail.ProductPrice} cada)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o pedido: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detalhes: {ex.InnerException.Message}");
                }
            }
        }
    }
}