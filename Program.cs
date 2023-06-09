﻿using System;
using System.Data;
using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace BaltaDataAccess
{
    class Program
    {
        private static void Main(string[] args)
        {
            const string connectionString = "Server=MAT-NOTE\\SQLEXPRESS;Database=balta;Integrated Security=SSPI";

            using (var connection = new SqlConnection(connectionString))
            {
                //UpdateCategory(connection);
                //ListCategories(connection);
                //CreateCategory(connection);
                //CreateManyCategory(connection);
                //GetCategory(connection);
                //DeleteCategory(connection);
                //ExecuteProcedure(connection);
                //ExecuteReadProcedure(connection);
                //ExecuteScalar(connection);
                //ReadView(connection);
                //OneToOne(connection);
                OneToMany(connection);
            }
        }

        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void GetCategory(SqlConnection connection)
        {
            var category = connection
                .QueryFirstOrDefault<Category>(
                    "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id",
                    new
                    {
                        id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
                    });
            Console.WriteLine($"{category.Id} - {category.Title}");

        }

        static void CreateCategory(SqlConnection connection)
        {
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

            var rows = connection.Execute(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"Linhas inseridas {rows}");
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title] = @title WHERE [Id] = @id";
            // Executar a Query passando os parametros
            var rows = connection.Execute(updateQuery, new
            {
                id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
                title = "Frontend 2021"
            });

            Console.WriteLine($"Registros atualizados {rows}");
        }

        static void DeleteCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE [Category] WHERE [Id]=@id";
            var rows = connection.Execute(deleteQuery, new
            {
                id = new Guid("ea8059a2-e679-4e74-99b5-e4f0b310fe6f"),
            });

            Console.WriteLine($"{rows} registros excluídos");
        }

        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();
            // Gerar uma nova tabela
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria Nova";
            category2.Url = "categoria-nova";
            category2.Description = "Categoria Nova";
            category2.Order = 9;
            category2.Summary = "Categoria";
            category2.Featured = true;
            // SQL INJECTION (é um ataque conhecido e muito executado)
            var insertSql = @"INSERT INTO [Category] 
                                VALUES(
                                @Id, 
                                @Title, 
                                @Url, 
                                @Description,
                                @Order, 
                                @Summary, 
                                @Featured)";

            var rows = connection.Execute(insertSql, new[]

            {
                new
                {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
                },
                new
                {
                category2.Id,
                category2.Title,
                category2.Url,
                category2.Summary,
                category2.Order,
                category2.Description,
                category2.Featured
                }
            });
            Console.WriteLine($"Linhas inseridas {rows}");
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var procedure = "[dbo].[spDeleteStudent]";
            var pars = new { StudentId = "6bd552ea-7187-4bae-abb6-54e8f8b9f530" };
            var affectedRows = connection.Execute(
                procedure,
                pars,
                commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{affectedRows} linhas afetadas");
        }

        static void ExecuteReadProcedure(SqlConnection connection)

        {
            // Essa procedure não retorna informações, faz uma leitura de dados
            // Chamar a procedure tem que usar o EXEC no DAPPER

            var procedure = "[dbo].[spGetCoursesByCategory]";
            var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
            var courses = connection.Query(
            procedure,
            pars,
            commandType: CommandType.StoredProcedure);
            foreach (var item in courses)
            {
                Console.WriteLine(item.Title);
                // Aqui ele lista pelo item que está no banco de dados, nesse exemplo é pelo Title
            }
        }

        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO [Category] 
                              OUTPUT inserted.[Id] 
                            VALUES(
                                NEWID(), 
                                @Title, 
                                @Url, 
                                @Summary, 
                                @Order, 
                                @Description, 
                                @Featured)";
            // Esse OUTPUT serve para ver o último id criado 
            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"A categoria inserida foi: {id}");
        }

        static void ReadView(SqlConnection connection)
        {
            var sql = "SELECT * FROM [vwCourses]";
            var courses = connection.Query(sql);
            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }

        }

        // Fazer o Inner Join no Dapper
        static void OneToOne(SqlConnection connection)
        {
            var sql = @"
            SELECT 
            * 
            FROM 
                [CareerItem] 
            INNER JOIN 
                [Course] ON [CareerItem].[CourseId] = [Course].[Id]";

            // Tabelas do banco de dados
            var items = connection.Query<CareerItem, Course, CareerItem>(
                sql,
                (careerItem, course) =>
                {
                    careerItem.Course = course;
                    return careerItem;
                }, splitOn: "Id");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
            }
        }

        static void OneToMany(SqlConnection connection)
        {
            var sql = @"
                SELECT 
                    [Career].[Id],
                    [Career].[Title],
                    [CareerItem].[CareerId],
                    [CareerItem].[Title]
                FROM 
                    [Career] 
                INNER JOIN 
                    [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                ORDER BY
                    [Career].[Title]";

            var careers = new List<Career>();
            var items = connection.Query<Career, CareerItem, Career>(
                sql,
                (career, item) =>
                {
                    var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
                    if (car == null)
                    {
                        car = career;
                        car.Items.Add(item);
                        careers.Add(car);
                    }
                    else
                    {
                        car.Items.Add(item);
                    }
                    return career;
                }, splitOn: "CareerId");

            foreach (var career in careers)
            {
                Console.WriteLine($"{career.Title}");
                foreach (var item in career.Items)
                {
                    Console.WriteLine($" - {item.Title}");
                }
            }
        }
    }
}