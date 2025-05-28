# EfCoreConsoleApp: Um Estudo de Entity Framework Core com PostgreSQL

Este projeto documenta meu estudo introdutório sobre o **Entity Framework Core (EF Core)**, com foco em sua utilização com **PostgreSQL**. Embora o aprendizado vise também contextos com **ASP.NET**, esta aplicação é um exemplo prático em console. A base para este estudo foi o vídeo ["Getting Started with Entity Framework Core [1 of 5]"](https://www.youtube.com/watch?v=SryQxUeChMc) da Microsoft. O projeto consiste em uma aplicação de console desenvolvida para gerenciar clientes, produtos, pedidos e detalhes de pedidos.

## Objetivos de Aprendizagem Alcançados

Ao longo do desenvolvimento deste projeto, alcancei os seguintes objetivos de aprendizagem:

* **Configuração do Ambiente EF Core**: Realizei a configuração do Entity Framework Core em um projeto .NET, integrando-o com o provedor Npgsql para PostgreSQL.
* **Definição de Modelos de Dados e Relacionamentos**: Defini os modelos de entidade (`Customer`, `Product`, `Order`, `OrderDetail`) e seus respectivos relacionamentos.
* **Implementação do Contexto de Banco de Dados**: Criei o contexto de banco de dados (`EfCoreConsoleAppContext`), que atua como a sessão com o banco de dados para operações de consulta e persistência de dados.
* **Utilização de Migrações (Migrations)**: Empreguei o recurso de migrações do EF Core para gerar e aplicar o esquema do banco de dados no PostgreSQL com base nos modelos definidos.
* **Configuração Segura da String de Conexão**: Implementei a configuração da string de conexão utilizando variáveis de ambiente para proteger informações sensíveis.
* **Demonstração Prática**: Desenvolvi um exemplo funcional, agora interativo via menu, para inserir e consultar dados, validando a configuração e as operações CRUD básicas.

## Pré-requisitos

Para a compilação e execução deste projeto, são necessários os seguintes componentes:

* .NET 8 SDK (ou superior)
* PostgreSQL (versão 13 ou superior recomendada)
* Ferramentas de linha de comando do EF Core (EF Core CLI):
    ```bash
    dotnet tool install --global dotnet-ef
    ```
* Pacotes NuGet (já referenciados no projeto):
    * `Microsoft.EntityFrameworkCore`
    * `Npgsql.EntityFrameworkCore.PostgreSQL`
    * `Microsoft.Extensions.Configuration.EnvironmentVariables`

## Configuração do Ambiente de Desenvolvimento

Executei os seguintes passos para configurar o ambiente antes de iniciar o projeto:

1.  **Criação do Banco de Dados**:
    Utilizei o seguinte comando SQL para criar o banco de dados no PostgreSQL:
    ```sql
    CREATE DATABASE EfCoreConsoleApp;
    ```

2.  **Configuração da Variável de Ambiente `ConnectionStrings__DefaultConnection`**:
    A string de conexão deve ser configurada como uma variável de ambiente. Substitua `seu_usuario` e `sua_senha` pelas suas credenciais do PostgreSQL.

    * **Windows (PowerShell):**
        ```powershell
        $env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=EfCoreConsoleApp;Username=seu_usuario;Password=sua_senha"
        ```
        *Nota: Para persistência entre sessões, utilize `[System.Environment]::SetEnvironmentVariable("...", "User")`.*

    * **Linux/macOS (bash):**
        ```bash
        export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=EfCoreConsoleApp;Username=seu_usuario;Password=sua_senha"
        ```
        *Nota: Para persistência, adicione o comando `export` ao seu arquivo de perfil do shell (ex: `~/.bashrc`, `~/.zshrc`) e recarregue-o (`source ~/.bashrc`).*

    Após configurar, verifique se a variável foi definida corretamente no seu terminal.

## Estrutura do Projeto

A organização dos arquivos e diretórios principais do projeto é a seguinte:

* `src/Data/EfCoreConsoleAppContext.cs`: Contém a definição do `DbContext`, responsável pela configuração da sessão com o banco de dados, mapeamento das entidades e configuração do provedor PostgreSQL. Comentários adicionais sobre a configuração podem ser encontrados neste arquivo.
* `src/Models/`: Diretório contendo as classes de entidade:
    * `Customer.cs`
    * `Product.cs`
    * `Order.cs`
    * `OrderDetail.cs`
* `src/Migrations/`: Contém os arquivos de migração gerados pelo EF Core, como a migração `InitialCreate`.
* `Program.cs`: Ponto de entrada da aplicação console. Ele inicia um **menu interativo** que permite ao usuário realizar operações como adicionar clientes e produtos, criar pedidos e listar dados. Toda a interação com o banco de dados PostgreSQL é gerenciada através do Entity Framework Core. O programa também lida com a entrada de dados do usuário e exibe feedback no console.

## Execução do Projeto

Para executar a aplicação, segui estes procedimentos:

1.  **Clonagem do Repositório** (se aplicável):
    ```bash
    git clone [https://github.com/seu_usuario/EfCoreConsoleApp.git](https://github.com/seu_usuario/EfCoreConsoleApp.git)
    cd EfCoreConsoleApp
    ```
    (Ajuste o URL do repositório conforme necessário.)

2.  **Restauração de Pacotes NuGet**:
    No diretório raiz do projeto:
    ```bash
    dotnet restore
    ```

3.  **Aplicação das Migrações**:
    Este comando executa as migrações pendentes para criar/atualizar o esquema do banco:
    ```bash
    dotnet ef database update
    ```

4.  **Execução da Aplicação**:
    ```bash
    dotnet run
    ```
    Ao executar, a aplicação apresentará um menu no console para interagir com o banco de dados.

## Notas Adicionais

* **Verificação do Serviço PostgreSQL**: Recomendo verificar se o serviço do PostgreSQL está em execução antes de rodar a aplicação:
    ```bash
    pg_isready -h localhost -p 5432
    ```
* **Gerenciamento de Credenciais**: É fundamental que credenciais sensíveis (usuário e senha do banco de dados) sejam gerenciadas exclusivamente através de variáveis de ambiente ou outros mecanismos seguros (como o Secret Manager em desenvolvimento) e nunca versionadas no código-fonte.
* **Detalhes de Configuração do DbContext**: Informações mais detalhadas sobre a configuração das entidades e seus relacionamentos foram documentadas como comentários dentro do arquivo `src/Data/EfCoreConsoleAppContext.cs`.

## Referência Principal

* **Vídeo**: "Getting Started with Entity Framework Core [1 of 5]" - Microsoft. (Link original no documento: `https://www.youtube.com/watch?v=SryQxUeChMc` - sugere-se verificar e atualizar para o link oficial do YouTube, se disponível).
