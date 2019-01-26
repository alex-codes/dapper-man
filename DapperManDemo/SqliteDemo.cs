using DapperMan.SQLite;
using DapperManDemo.Models.Chinook;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace DapperManDemo
{
    // project requires the chinook sqlite database (included in /data folder)
    // https://github.com/lerocha/chinook-database/blob/master/ChinookDatabase/DataSources/Chinook_Sqlite.sqlite
    public class SqliteDemo
    {
        private SQLiteConnection connection;

        public SqliteDemo()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connStr = configuration.GetConnectionString("SQLite");
            connection = new SQLiteConnection(connStr);
        }

        public void RunAll()
        {
            try
            {
                ReadAllGenres();
                AddGenre();
                GenreExists();
                FindGenre();
                UpdateGenre();
                ReadAllGenres();
                DeleteGenre();
                ReadAllGenres();
                ReadGenresByPage();
            }
            finally
            {
                if (connection != null)
                {
                    if (connection.State != System.Data.ConnectionState.Closed)
                    {
                        try
                        {
                            connection.Clone();
                        }
                        catch(Exception)
                        {

                        }
                    }

                    connection.Dispose();
                    connection = null;
                }
            }
        }

        private void LogTest(string test)
        {
            Console.WriteLine($"/// {test} ///");
            Console.WriteLine();
        }

        private void AddGenre()
        {
            LogTest("AddGenre");

            int id = DapperQuery.Count("Genre", connection)
                .Execute();

            var genre = new Genre
            {
                GenreId = ++id,
                Name = "testing"
            };

            DapperQuery.Insert("Genre", connection)
                .Execute<Genre>(genre);

            Console.WriteLine();
            Console.WriteLine($"Added genre with id {genre.GenreId}");
            Console.WriteLine();
        }

        private void DeleteGenre()
        {
            LogTest("DeleteGenre");

            Genre genre = FindGenreInternal("testing123");

            Console.WriteLine();

            if (genre != null)
            {
                int deleted = DapperQuery.Delete("Genre", connection)
                    .Where("GenreId = @id")
                    .Execute(new { id = genre.GenreId });

                Console.WriteLine($"{deleted} row(s) deleted");
            }
            else
            {
                Console.WriteLine("Genre not found");
            }

            Console.WriteLine();
        }

        private void FindGenre()
        {
            LogTest("FindGenre");

            Genre genre = FindGenreInternal("testing");

            Console.WriteLine();

            if (genre != null)
            {
                Console.WriteLine("Found genre");
                Console.WriteLine(genre.ToString());
            }
            else
            {
                Console.WriteLine("Genre not found");
            }

            Console.WriteLine();
        }

        private Genre FindGenreInternal(string name)
        {
            return DapperQuery.Find("Genre", connection)
                .Where("Name = @name")
                .Execute<Genre>(new { name = name });
        }

        private void GenreExists()
        {
            LogTest("GenreExists");

            string name = "testing";

            bool exists = DapperQuery.Exists("Genre", connection)
                .Where("Name = @name")
                .Execute(new { name });

            Console.WriteLine();
            Console.WriteLine($"Genre with name {name} {(exists ? "does" : "does not")} exist");
            Console.WriteLine();
        }

        private void ReadAllGenres()
        {
            LogTest("ReadAllGenres");

            (var genres, int count) = DapperQuery.Select("Genre", connection)
                .Execute<Genre>();

            foreach (var genre in genres)
            {
                Console.WriteLine(genre.ToString());
            }

            Console.WriteLine();
            Console.WriteLine($"{count} genre records found.");
            Console.WriteLine();
        }

        private void ReadGenresByPage()
        {
            LogTest("ReadGenresByPage");

            int currentPage = 0;
            int pageSize = 3;

            int totalRows = ReadGenresByPageInternal(currentPage, pageSize);

            while (++currentPage * pageSize < totalRows)
            {
                ReadGenresByPageInternal(currentPage, pageSize);
            }

            Console.WriteLine();
        }

        private int ReadGenresByPageInternal(int currentPage, int pageSize)
        {
            (var page, int totalRows) = DapperQuery.Select("Genre", connection)
                .Where("GenreId % 2 == 1")
                .SkipTake(currentPage * pageSize, pageSize)
                .OrderBy("GenreId")
                .Execute<Genre>();

            Console.WriteLine();
            Console.WriteLine($"Reading page {currentPage} of {pageSize} items. {totalRows} total rows.");

            foreach (var genre in page)
            {
                Console.WriteLine(genre.ToString());
            }

            return totalRows;
        }

        private void UpdateGenre()
        {
            LogTest("UpdateGenre");

            Genre genre = FindGenreInternal("testing");

            Console.WriteLine();

            if (genre != null)
            {
                genre.Name = "testing123";

                int updated = DapperQuery.Update("Genre", connection)
                    .Where("GenreId = @GenreId")
                    .Execute<Genre>(genre);

                Console.WriteLine($"{updated} row(s) updated");
            }
            else
            {
                Console.WriteLine("Genre not found");
            }

            Console.WriteLine();
        }
    }
}
