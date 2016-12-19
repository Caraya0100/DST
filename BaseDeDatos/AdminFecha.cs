using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class AdminFecha
    {
        public AdminFecha()
        {

        }

        public string FechaConFormato(string fecha)
        {
            string[] f = fecha.Split('/');
            string anio = f[2].Split(' ')[0];
            string conFormato = anio + "-" + f[1] + "-01";

            return conFormato;
        }
    }
}
