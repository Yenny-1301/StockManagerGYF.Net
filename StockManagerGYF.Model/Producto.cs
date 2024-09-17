using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagerGYF.Model
{
    public class Producto
    {
        public int id { get; set; }
        public int precio { get; set; }
        public DateTime fecha_de_carga { get; set; }
        public string categoria { get; set; }

    }
}
