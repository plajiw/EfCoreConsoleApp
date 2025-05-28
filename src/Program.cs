using EfCoreConsoleApp.Data;
using EfCoreConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace EfCoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            using (var context = new EfCoreConsoleAppContext())
            {
                bool sair = false;
                while (!sair)
                {
                    Console.WriteLine("\n--- Menu Principal ---");
                    Console.WriteLine("1. Adicionar Novo Cliente");
                    Console.WriteLine("2. Adicionar Novo Produto");
                    Console.WriteLine("3. Criar Novo Pedido");
                    Console.WriteLine("4. Listar Pedidos de um Cliente");
                    Console.WriteLine("5. Listar Todos os Clientes");
                    Console.WriteLine("6. Listar Todos os Produtos");
                    Console.WriteLine("0. Sair");
                    Console.Write("Escolha uma opção: ");

                    string? escolha = Console.ReadLine();
                    Console.WriteLine("---");

                    try
                    {
                        switch (escolha)
                        {
                            case "1":
                                AdicionarNovoCliente(context);
                                break;
                            case "2":
                                AdicionarNovoProduto(context);
                                break;
                            case "3":
                                CriarNovoPedido(context);
                                break;
                            case "4":
                                ListarPedidosDeUmCliente(context);
                                break;
                            case "5":
                                ListarTodosOsClientes(context);
                                break;
                            case "6":
                                ListarTodosOsProdutos(context);
                                break;
                            case "0":
                                sair = true;
                                Console.WriteLine("Saindo...");
                                break;
                            default:
                                Console.WriteLine("Opção inválida. Tente novamente.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Ocorreu um erro: {ex.Message}");

                        if (ex.InnerException != null)
                        {
                            Console.WriteLine($"Detalhe: {ex.InnerException!.Message}");
                        }
                        Console.ResetColor();
                    }

                    if (!sair)
                    {
                        Console.WriteLine("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }

        static void AdicionarNovoCliente(EfCoreConsoleAppContext context)
        {
            Console.WriteLine("--- Adicionar Novo Cliente ---");

            Console.Write("Primeiro Nome: ");
            string? firstNameInput = Console.ReadLine();

            Console.Write("Último Nome: ");
            string? lastNameInput = Console.ReadLine();

            Console.Write("Endereço: ");
            string? addressInput = Console.ReadLine();

            Console.Write("Telefone: ");
            string? phoneInput = Console.ReadLine();

            // Converter null para string vazia antes da validação
            string firstName = firstNameInput ?? string.Empty;
            string lastName = lastNameInput ?? string.Empty;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Nome e Sobrenome são obrigatórios.");
                return;
            }

            var cliente = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Address = addressInput ?? string.Empty,
                Phone = phoneInput ?? string.Empty
            };
            context.Customers.Add(cliente);
            context.SaveChanges();
            Console.WriteLine($"Cliente '{cliente.FirstName} {cliente.LastName}' adicionado com ID: {cliente.Id}.");
        }

        static void AdicionarNovoProduto(EfCoreConsoleAppContext context)
        {
            Console.WriteLine("--- Adicionar Novo Produto ---");
            Console.Write("Nome do Produto: ");
            string? nameInput = Console.ReadLine();

            Console.Write("Preço (ex: 9.99): ");
            string? precoStr = Console.ReadLine();

            string name = nameInput ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(precoStr) || !decimal.TryParse(precoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) || price <= 0)
            {
                Console.WriteLine("Nome e preço válido são obrigatórios.");
                return;
            }

            var produto = new Product { Name = name, Price = price };
            context.Products.Add(produto);
            context.SaveChanges();
            Console.WriteLine($"Produto '{name}' adicionado com ID: {produto.Id}.");
        }

        static void CriarNovoPedido(EfCoreConsoleAppContext context)
        {
            Console.WriteLine("--- Criar Novo Pedido ---");
            ListarTodosOsClientes(context, false);

            Console.Write("Digite o ID do Cliente para o pedido: ");
            string? customerIdInput = Console.ReadLine();

            if (!int.TryParse(customerIdInput, out int customerId) || context.Customers.Find(customerId) == null)
            {
                Console.WriteLine("ID do Cliente inválido ou não encontrado.");
                return;
            }

            var order = new Order { CustomerId = customerId, OrderPlaced = DateTime.UtcNow, OrderDetails = new List<OrderDetail>() };
            bool adicionarMaisItens = true;

            while (adicionarMaisItens)
            {
                Console.WriteLine("\n--- Adicionar Item ao Pedido ---");
                ListarTodosOsProdutos(context, false);

                Console.Write("Digite o ID do Produto para adicionar ao pedido (ou 'FIM' para finalizar): ");
                string? produtoInput = Console.ReadLine();

                if (produtoInput?.Equals("FIM", StringComparison.OrdinalIgnoreCase) == true)
                {
                    adicionarMaisItens = false;
                    continue;
                }

                if (!int.TryParse(produtoInput, out int productId) || context.Products.Find(productId) == null)
                {
                    Console.WriteLine("ID do Produto inválido ou não encontrado.");
                    continue;
                }

                Console.Write($"Quantidade do produto ID {productId}: ");
                string? quantityInput = Console.ReadLine();

                if (!int.TryParse(quantityInput, out int quantity) || quantity <= 0)
                {
                    Console.WriteLine("Quantidade inválida.");
                    continue;
                }

                order.OrderDetails.Add(new OrderDetail { ProductId = productId, Quantity = quantity });
                Console.WriteLine("Item adicionado.");
            }

            if (!order.OrderDetails.Any())
            {
                Console.WriteLine("Nenhum item adicionado. O pedido não será criado.");
                return;
            }

            context.Orders.Add(order);
            context.SaveChanges();
            Console.WriteLine($"Pedido ID {order.Id} criado com {order.OrderDetails.Count} tipo(s) de item(ns).");
        }

        static void ListarPedidosDeUmCliente(EfCoreConsoleAppContext context)
        {
            Console.WriteLine("--- Listar Pedidos de um Cliente ---");
            ListarTodosOsClientes(context, false);

            Console.Write("Digite o ID do Cliente para ver os pedidos: ");
            string? customerIdInput = Console.ReadLine();

            if (!int.TryParse(customerIdInput, out int customerId))
            {
                Console.WriteLine("ID do Cliente inválido.");
                return;
            }

            var cliente = context.Customers.FirstOrDefault(c => c.Id == customerId);

            if (cliente == null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }

            Console.WriteLine($"\n--- Pedidos de {cliente.FirstName} {cliente.LastName} ---");

            var pedidos = context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) // Product dentro de OrderDetail pode ser null se não for obrigatório
                .OrderByDescending(o => o.OrderPlaced)
                .ToList();

            if (!pedidos.Any())
            {
                Console.WriteLine("Nenhum pedido encontrado para este cliente.");
                return;
            }

            foreach (var pedido in pedidos)
            {
                Console.WriteLine($"  Pedido ID: {pedido.Id}, Data: {pedido.OrderPlaced.ToLocalTime()}");

                decimal totalPedido = 0;

                foreach (var detalhe in pedido.OrderDetails)
                {
                    Console.WriteLine($"    - {detalhe.Quantity} x {detalhe.Product?.Name ?? "Produto Desconhecido"} (R${detalhe.Product?.Price.ToString("F2", CultureInfo.InvariantCulture) ?? "N/A"})");
                    totalPedido += detalhe.Quantity * (detalhe.Product?.Price ?? 0);
                }

                Console.WriteLine($"    Total do Pedido: R${totalPedido.ToString("F2", CultureInfo.InvariantCulture)}");
            }
        }

        static void ListarTodosOsClientes(EfCoreConsoleAppContext context, bool comPausaNoFinal = true)
        {
            Console.WriteLine("--- Lista de Clientes ---");
            var clientes = context.Customers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName).ToList();

            if (!clientes.Any())
            {
                Console.WriteLine("Nenhum cliente cadastrado.");
                return;
            }

            foreach (var c in clientes)
            {
                Console.WriteLine($"ID: {c.Id} | Nome: {c.FirstName} {c.LastName} | Tel: {c.Phone}");
            }

            if (!comPausaNoFinal) Console.WriteLine("--- Fim da Lista de Clientes ---");
        }

        static void ListarTodosOsProdutos(EfCoreConsoleAppContext context, bool comPausaNoFinal = true)
        {
            Console.WriteLine("--- Lista de Produtos ---");
            var produtos = context.Products.OrderBy(p => p.Name).ToList();
            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }

            foreach (var p in produtos)
            {
                Console.WriteLine($"ID: {p.Id} | Nome: {p.Name} | Preço: R${p.Price.ToString("F2", CultureInfo.InvariantCulture)}");
            }

            if (!comPausaNoFinal) Console.WriteLine("--- Fim da Lista de Produtos ---");
        }
    }
}