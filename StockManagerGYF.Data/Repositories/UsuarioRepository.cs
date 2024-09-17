using Dapper;
using MySql.Data.MySqlClient;
using StockManagerGYF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StockManagerGYF.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public UsuarioRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<Usuario> autenticarUsuario(string nombre, string contrasenia)
        {
            var db = dbConnection();

            var sql = @"SELECT nombre,
                        contrasenia FROM Usuarios 
                        WHERE nombre = @Nombre AND contrasenia = @Contrasenia";

            return await db.QueryFirstOrDefaultAsync<Usuario>(sql, new { nombre, contrasenia });

        }

        public async Task<bool> registrarUsuario(string nombre, string contrasenia)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO Usuarios (nombre,contrasenia )
                       VALUES(@Nombre,@Contrasenia)"
            ;

            var result = await db.ExecuteAsync(sql, new { nombre, contrasenia });

            return result > 0;
        }
    }
}
