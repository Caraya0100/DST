using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase para el perfil. Contiene las habilidades blandas, duras, y 
    /// caracteristicas fisicas.
    /// </summary>
    public class Perfil
    {
        private List<Componente> hb;
        private List<Componente> hd;
        private List<Componente> cf;

        /// <summary>
        /// Constructor, recibe las habilidades blandas, duras y las caracteristicas fisica 
        /// del perfil.
        /// </summary>
        /// <param name="hb"></param>
        /// <param name="hd"></param>
        /// <param name="cf"></param>
        public Perfil(List<Componente> blandas, List<Componente> duras, List<Componente> fisicas)
        {
            hb = new List<Componente>();
            hd = new List<Componente>();
            cf = new List<Componente>();

            IniciarComponentes(hb, blandas);
            IniciarComponentes(hd, duras);
            IniciarComponentes(cf, fisicas);
        }

        /// <summary>
        /// Inicializa una lista de componentes a partir de otra.
        /// </summary>
        /// <param name="inicializar">Componentes a inicializar</param>
        /// <param name="inicializados">Componentes inicializados</param>
        private void IniciarComponentes(List<Componente> inicializar, List<Componente> inicializados)
        {
            foreach (Componente inicilizada in inicializados)
            {
                inicializar.Add(new Componente(
                    inicilizada.Nombre, 
                    inicilizada.Descripcion, 
                    inicilizada.Tipo, 
                    inicilizada.Puntaje, 
                    inicilizada.Importancia
                ));
            }
        }

        public List<Componente> HB
        {
            get { return hb; }
            set { hb = value; }
        }

        public List<Componente> HD
        {
            get { return hd; }
            set { hd = value; }
        }

        public List<Componente> CF
        {
            get { return cf; }
            set { cf = value; }
        }
    }
}
