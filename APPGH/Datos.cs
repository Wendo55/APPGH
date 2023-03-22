using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPGH
{
    internal class Datos
    {
        public string CodigoPostal { get; set; }
        public string Asentamiento { get; set; }

        public string Colonia { get; set; }

        public string Ciudad { get; set; }

        public Datos(string codigoPostal, string asentamiento, string colonia, string ciudad)
        {
            CodigoPostal = codigoPostal;
            Asentamiento = asentamiento;
            Colonia = colonia;
            Ciudad = ciudad;
        }
    }
}
