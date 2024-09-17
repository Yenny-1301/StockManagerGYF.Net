using Dapper;
using MySql.Data.MySqlClient;
using StockManagerGYF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagerGYF.Data.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public ProductoRepository (MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;   
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> actualizarProducto(Producto producto)
        {
            var db = dbConnection();

            var sql = @"UPDATE productos
                        SET precio = @Precio,
                            fecha_de_carga = @Fecha_de_carga,
                            categoria = @Categoria
                        WHERE id = @Id
                      ";

            var result = await db.ExecuteAsync(sql, new { producto.precio, producto.fecha_de_carga, producto.categoria, producto.id});

            return result > 0;
        }

        public async Task<bool> eliminarProducto(Producto producto)
        {
            var db = dbConnection();

            var sql = @"DELETE FROM productos WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new { producto.id });

            return result > 0;
        }

        public async Task<bool> insertarProducto(Producto producto)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO productos (precio, fecha_de_carga, categoria )
                       VALUES(@Precio, @Fecha_de_carga, @Categoria)";

            var result = await db.ExecuteAsync(sql, new { producto.precio, producto.fecha_de_carga, producto.categoria});

            return result > 0;
        }

        public async Task<Producto> obtenerDetalleProducto(int id)
        {
            var db = dbConnection();

            var sql = @"SELECT id, precio, fecha_de_carga, categoria 
                       FROM productos
                       WHERE id = @Id";

            return await db.QueryFirstOrDefaultAsync<Producto>(sql, new { id = id});
        }

        public async Task<IEnumerable<Producto>> obtenerProductos()
        {
            var db = dbConnection();

            var sql = @"SELECT id, precio, fecha_de_carga, categoria 
                       FROM productos";

            return await db.QueryAsync<Producto>(sql, new {});
        }
    }
}
