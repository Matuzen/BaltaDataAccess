using System;
using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BaltaDataAccess
{
    class Program
    {
        private static void Main(string[] args)
        {
            const string connectionString = "Server=MAT-NOTE\\SQLEXPRESS;Database=balta;Integrated Security=SSPI";

            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Conectado");
                connection.Open();

                var category = new Category();
                // Gerar uma nova tabela
                category.Id = Guid.NewGuid();
                category.Title = "Amazon AWS";
                category.Url = "amazon";
                category.Description = "Categoria destinada a serviços do AWS";
                category.Order = 8;
                category.Summary = "AWS Cloud";
                category.Featured = false;
                
                // SQL INJECTION (é um ataque conhecido e muito executado)

                var insertSql = @"INSERT INTO [Category] 
                                VALUES(
                                @Id, 
                                @Title, 
                                @Url, 
                                @Summary, 
                                @Order, 
                                @Description, 
                                @Featured)";

                                // DAPPER
                using (var command = new SqlCommand())
                {
                    // Retorna int com a quantidade de linhas afetadas
                    var rows = connection.Execute(insertSql, new {
                        category.Id,
                        category.Title,
                        category.Url,
                        category.Summary,
                        category.Order,
                        category.Description,
                        category.Featured
                    });
                    Console.WriteLine($"Linhas inseridas { rows}");

                    // Retorna uma lista 
                    var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
                    foreach (var item in categories)
                    {
                        Console.WriteLine($"{item.Id} - {item.Title}");
                    }
                }

            }

            Console.WriteLine("Hello, World!");
        }
    }
}

