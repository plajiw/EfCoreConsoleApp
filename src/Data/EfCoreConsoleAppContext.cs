using EfCoreConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace EfCoreConsoleApp.Data
{
    internal class EfCoreConsoleAppContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada nas variáveis de ambiente.");
            }

            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}

// Configuração da string de conexão via variável de ambiente "DefaultConnection"
//
// Para configurar a variável de ambiente no PowerShell:
//
// 1. Na sessão atual, execute:
//    $env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=EfCoreConsoleApp;Username=seu_usuario;Password=sua_senha"
//
// Para verificar a variável no PowerShell:
//    $env:ConnectionStrings__DefaultConnection
//
// Substitua 'seu_usuario' e 'sua_senha' pelas credenciais do seu banco PostgreSQL.