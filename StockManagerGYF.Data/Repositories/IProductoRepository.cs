using StockManagerGYF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagerGYF.Data.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> obtenerProductos();
        Task <Producto> obtenerDetalleProducto(int id);
        Task<bool> insertarProducto(Producto producto);
        Task<bool> actualizarProducto(Producto producto);
        Task<bool> eliminarProducto(Producto producto);
    }
}
