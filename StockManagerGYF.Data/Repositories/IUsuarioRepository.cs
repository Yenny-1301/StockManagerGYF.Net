using StockManagerGYF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagerGYF.Data.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> autenticarUsuario(string nombre, string contrasenia);
        Task<bool> registrarUsuario(string nombre, string contrasenia);
    }
}
