using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tablas {
    internal class Marca {
        public int ID { get; set; } = 0;
        public string name { get; set; }
        public bool state { get; set; }

        public Marca(int id, string name,  bool state) {
            this.ID = id;
            this.name = name;
            this.state = state;
        }


        public string[] Print() {
            string[] result = new string[3];
            result[0] = Convert.ToString(this.ID);
            result[1] = this.name;
            result[2] = Convert.ToString(this.state);
            return result;
        }


    }
}
