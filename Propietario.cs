using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tablas {
    internal class Propietario {

        public int ID { set; get; } = 0;
        public string nombrePrimero { set; get; } = "";
        public string nombreSegundo { set; get; } = "";
        public string apellidoPrimero { set; get; } = "";
        public string apellidoSegundo { set; get; } = "";
        public string telefono { set; get; } = "";
        public string dpi { set; get; } = "";
        public string genero { set; get; } = "";
        public string direccion { set; get; } = "";
        public string email { set; get; } = "";
        public string nit { set; get; } = "";
        public bool estado { set; get; }

        public Propietario(int id, string name1, string name2, string last1, string last2, string tel, string dpi, string genero, string direccion, string email, string nit,  bool state) {
            this.ID = id;
            this.nombrePrimero = name1;
            this.nombreSegundo = name2;
            this.apellidoPrimero = last1;
            this.apellidoSegundo = last2;
            this.email = email;
            this.genero = genero;
            this.telefono = tel;
            this.dpi = dpi;
            this.nit = nit;
            this.direccion = direccion;
            this.estado = state;
        }
        public string[] Print() {
            string[] result = new string[12];
            result[0] = Convert.ToString(this.ID);
            result[1] = this.nombrePrimero;
            result[2] = this.nombreSegundo;
            result[3] = this.apellidoPrimero;
            result[4] = this.apellidoSegundo;
            result[5] = this.telefono;
            result[6] = this.dpi;
            result[7] = this.genero;
            result[8] = this.direccion;
            result[9] = this.email;
            result[10] = this.nit;
            result[11] = Convert.ToString(this.estado);
            return result;
        }
    }
}
