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
                var insertSql = "INSERT INTO [Category] VALUES(id, title, url, summary, order, description, featured)";

                using (var command = new SqlCommand())
                {

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

// Para começar a escrever a Query primeiro vc devevcriar uma pasta de Models
// Para cada tabela no banco, vamos ter uma classe dela no C#
// O Dapper vai pegar essa informação no banco e vai transformar em um objeto

// Vai no banco e executa a Query e traz a categorias em forma de lista