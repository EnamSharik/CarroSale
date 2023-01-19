using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tablas {
    internal class Vehiculo {
        public string placa { get; set; } = "";
        public string model { get; set; } = "";
        public string tradeMark { get;  set; } = "";
        public string owner { get;  set; }
        public string year { get;  set; } = "";
        public string stateVehicle { get; set; } = "";
        public bool state { get; set; }

        public Vehiculo(string placa, string model, string tradeMark, string owner, string year, string stateVehicle,  bool state) {
            this.placa = placa;
            this.model = model;
            this.tradeMark = tradeMark; 
            this.owner = owner;
            this.year = year;
            this.stateVehicle = stateVehicle;
            this.state = state;
        }

        public string[] Print() {
            string[] result = new string[7];
            result[0] = this.placa;
            result[1] = this.model;
            result[2] = this.tradeMark;
            result[3] = this.owner;
            result[4] = this.year;
            result[5] = this.stateVehicle;
            result[6] = Convert.ToString(this.state);
            return result;
        }
    }
}
