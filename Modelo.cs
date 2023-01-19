using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tablas {
    internal class Modelo {
        public int ID { get; set; }
        public string name { get; set; }

        public int IDTradeMark { get; set; }

        public bool state { get; set; }

        public Modelo(int id , string name, int iDTradeMark, bool state) {
            this.ID = id;
            this.name = name;
            this.IDTradeMark = iDTradeMark;  
            this.state = state;
        }

        public string[] Print() {
            string[] result = new string[4];
            result[0] = Convert.ToString(this.ID);
            result[1] = this.name;
            result[2] = Convert.ToString(this.IDTradeMark);
            result[3] = Convert.ToString(this.state);
            return result;
        }
    }

}
