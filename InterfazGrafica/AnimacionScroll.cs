using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InterfazGrafica
{
    class AnimacionScroll
    {       
        private DispatcherTimer dispatcher;
        private int contador;
        private ScrollViewer visualizador;

        public AnimacionScroll()
        {            
            this.dispatcher = new DispatcherTimer();
        }

        /// <summary>
        /// Metodo que activa el movimiento vertical de los objetos contenidos en el scroll.
        /// </summary>
        public void comenzarAnimacionVertical()
        {
            this.dispatcher = new DispatcherTimer();
            dispatcher.Tick += delegate
            {
                contador++;
                visualizador.ScrollToVerticalOffset((double)contador);
                if (contador >= visualizador.ScrollableHeight)
                {
                    dispatcher.Stop();
                    bucleAnimacionVertical();
                }
            };
            dispatcher.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcher.Start();
        }

        /// <summary>
        /// Metodo que activa el movimiento Horizontal de los objetos contenidos en el scroll.
        /// </summary>
        public void comenzarAnimacionHorizontal()
        {
            this.dispatcher = new DispatcherTimer();
            dispatcher.Tick += delegate
            {
                contador++;
                visualizador.ScrollToHorizontalOffset((double)contador);
                if (contador >= visualizador.ScrollableWidth)
                {
                    dispatcher.Stop();
                    bucleAnimacionHorizontal();
                }
            };
            dispatcher.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcher.Start();
        }

        /// <summary>
        /// Metodo que reestable el movimiento vertical de los objetos en el scroll, desde el inicio.
        /// </summary>
        private void bucleAnimacionVertical()
        {
            visualizador.ScrollToVerticalOffset(0);
            comenzarAnimacionVertical();
        }

        /// <summary>
        ///  Metodo que reestable el movimiento horizontal de los objetos en el scroll, desde el inicio.
        /// </summary>
        private void bucleAnimacionHorizontal()
        {
            visualizador.ScrollToHorizontalOffset(0);
            comenzarAnimacionHorizontal();
        }

        /// <summary>
        /// Metodo que detiene la animacion vertical.
        /// </summary>
        public void detenerAnimacionVertical()
        {
            dispatcher.Stop();
        }

        /// <summary>
        /// metodo que detiene la animacion horizontal.
        /// </summary>
        public void detenerAnimacionHorizontal()
        {
            dispatcher.Stop();
        }

        public ScrollViewer Visualizador
        {
            get { return visualizador; }
            set { visualizador = value; }
        }

        public int Contador
        {
            get { return contador; }
            set { contador = value; }
        }
    }
}
