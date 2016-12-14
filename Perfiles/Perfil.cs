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
        // Diccionario con las habilidades blandas, duras, y caracteristicas fisicas.
        private Dictionary<string, Componente> blandas;
        private Dictionary<string, Componente> duras;
        private Dictionary<string, Componente> fisicas;
        // Componentes generales (HBs, HDs, CFs).
        private Componente hb;
        private Componente hd;
        private Componente cf;

        /// <summary>
        /// Constructor, inicializa un perfil vacio.
        /// </summary>
        public Perfil()
        {
            blandas = new Dictionary<string, Componente>();
            duras = new Dictionary<string, Componente>();
            fisicas = new Dictionary<string, Componente>();
            hb = new Componente("HB", "Habilidades blandas del perfil", "general");
            hd = new Componente("HD", "Habilidades duras del perfil", "general");
            cf = new Componente("CF", "Caracteristicas fisicas del perfil", "general");
        }

        /// <summary>
        /// Constructor, recibe las habilidades blandas, duras y las caracteristicas fisica 
        /// del perfil.
        /// </summary>
        /// <param name="hb"></param>
        /// <param name="hd"></param>
        /// <param name="cf"></param>
        public Perfil(Dictionary<string, Componente> blandas, Dictionary<string, Componente> duras, Dictionary<string, Componente> fisicas)
        {
            this.blandas = new Dictionary<string, Componente>();
            this.duras = new Dictionary<string, Componente>();
            this.fisicas = new Dictionary<string, Componente>();

            IniciarComponentes(this.blandas, blandas);
            IniciarComponentes(this.duras, duras);
            IniciarComponentes(this.fisicas, fisicas);

            hb = new Componente("HB", "Habilidades blandas del perfil", "general");
            hd = new Componente("HD", "Habilidades duras del perfil", "general");
            cf = new Componente("CF", "Caracteristicas fisicas del perfil", "general");
        }

        /// <summary>
        /// Constructor, inicializa un perfil a partir de otro (copia). 
        /// </summary>
        /// <param name="hb"></param>
        /// <param name="hd"></param>
        /// <param name="cf"></param>
        public Perfil(Perfil perfil)
        {
            this.blandas = new Dictionary<string, Componente>();
            this.duras = new Dictionary<string, Componente>();
            this.fisicas = new Dictionary<string, Componente>();

            IniciarComponentes(this.blandas, perfil.Blandas);
            IniciarComponentes(this.duras, perfil.Duras);
            IniciarComponentes(this.fisicas, perfil.Fisicas);

            hb = new Componente("HB", "Habilidades blandas del perfil", "general");
            hd = new Componente("HD", "Habilidades duras del perfil", "general");
            cf = new Componente("CF", "Caracteristicas fisicas del perfil", "general");
        }

        /// <summary>
        /// Inicializa una lista de componentes a partir de otra.
        /// </summary>
        /// <param name="inicializar">Componentes a inicializar</param>
        /// <param name="inicializados">Componentes inicializados</param>
        private void IniciarComponentes(Dictionary<string, Componente> inicializar, Dictionary<string, Componente> inicializados)
        {
            foreach (KeyValuePair<string, Componente> inicilizada in inicializados)
            {
                inicializar.Add(inicilizada.Key, new Componente(
                    inicilizada.Value.Nombre,
                    inicilizada.Value.Descripcion,
                    inicilizada.Value.Tipo,
                    inicilizada.Value.Puntaje,
                    inicilizada.Value.Importancia
                ));
            }
        }


        /// <summary>
        /// Agrega un componente al perfil.
        /// </summary>
        /// <param name="componente"></param>
        public void AgregarComponente(Componente componente)
        {
            if (componente.Tipo == "hb")
            {
                Blandas.Add(componente.ID, new Componente(
                    componente.ID,
                    componente.Nombre,
                    componente.Descripcion,
                    componente.Tipo,
                    componente.Puntaje,
                    componente.Importancia
                ));
            } else if (componente.Tipo == "hd")
            {
                Duras.Add(componente.ID, new Componente(
                    componente.ID,
                    componente.Nombre,
                    componente.Descripcion,
                    componente.Tipo,
                    componente.Puntaje,
                    componente.Importancia
                ));
            } else if (componente.Tipo == "cf")
            {
                Fisicas.Add(componente.ID, new Componente(
                    componente.ID,
                    componente.Nombre,
                    componente.Descripcion,
                    componente.Tipo,
                    componente.Puntaje,
                    componente.Importancia
                ));
            }
        }

        /// <summary>
        /// Elimina un componente del perfil.
        /// </summary>
        /// <param name="componente">Nombre del componente</param>
        /// <returns></returns>
        public bool EliminarComponente(string componente)
        {
            if (Blandas.ContainsKey(componente))
            {
                Blandas.Remove(componente);
                return true;
            } else if (Duras.ContainsKey(componente))
            {
                Duras.Remove(componente);
                return true;
            } else if (Fisicas.ContainsKey(componente))
            {
                Fisicas.Remove(componente);
                return true;
            }

            return false;
        }

        public Dictionary<string, Componente> Blandas
        {
            get { return blandas; }
            set { blandas = value; }
        }

        public Dictionary<string, Componente> Duras
        {
            get { return duras; }
            set { duras = value; }
        }

        public Dictionary<string, Componente> Fisicas
        {
            get { return fisicas; }
            set { fisicas = value; }
        }

        public Componente HB
        {
            get { return hb; }
        }

        public Componente HD
        {
            get { return hd; }
        }

        public Componente CF
        {
            get { return cf; }
        }
    }
}
