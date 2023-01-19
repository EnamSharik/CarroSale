using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Mask.Internal;
using DevExpress.XtraBars.Alerter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Tablas.BasesDeDatos;
using static DevExpress.XtraEditors.RoundedSkinPanel;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;


namespace Tablas {
    internal class DB {
        /*
         * CREAR BD CLASE
         *      DEBE ESCRIBIR Y LEER ARCHIVOS Y ESTAR DISPONIBLE PARA ENVIARLOS A CUALQUIER LUGAR 
         *      SERÁN ALMACENADOS EN DATATABLES PUBLICAS PARA COPIARLAS HACIA LAS TABLAS 
         *      1. VEHICULOS
         */

        //Coneccion a  base de datos
        BasesDeDatos.SQLContextDataContext Conection = new BasesDeDatos.SQLContextDataContext(BasesDeDatos.Globales.CHAIN);

        //Tablas
        public IQueryable vehicleTable;
        public IQueryable tradeMarkTable; 
        public IQueryable ownerTable;
        public IQueryable modelTable;
        public IQueryable searchTable;

        //Constructor
        public DB() {
            updateTables();//Siempre se debe leer primero
        }


        public void updateTables() {

            //Marca
            
            var tradeMark = from tradeMarkSQL in Conection.TRADEMARK
                            select new {
                                ID = tradeMarkSQL.ID_TRADEMARK,
                                MARK = tradeMarkSQL.NAME,
                                STATE = tradeMarkSQL.STATE,
                            };
            tradeMarkTable = tradeMark;

            //Modelo
            var Model = from ModelSQL in Conection.MODEL
                        select new {
                            ID = ModelSQL.ID_MODEL,
                            MARK = ModelSQL.TRADEMARK.NAME,
                            MODEL = ModelSQL.NAME,
                            STATE = ModelSQL.STATE
                        };
            modelTable= Model;

            //PROPIETARIO
            var Owner = from OwnerSQL in Conection.OWNER
                        select new {
                            ID = OwnerSQL.ID_OWNER,
                            NOMBRE1 = OwnerSQL.NAME1,
                            NOMBRE2 = OwnerSQL.NAME2,
                            APELLIDO1 = OwnerSQL.LASTN1,
                            APELLIDO2= OwnerSQL.LASTN2,
                            TELEFONO = OwnerSQL.NUMBERPHONE,
                            DPI = OwnerSQL.DPI,
                            GENERO = OwnerSQL.GENRE,
                            DIRECCION = OwnerSQL.DIRECTION,
                            EMAIL = OwnerSQL.EMAIL,
                            NIT = OwnerSQL.NIT,
                            STATE = OwnerSQL.STATE,
                        };
            ownerTable = Owner;

            //Vehiculos
            var Vehicles = from vehiclesSQL in Conection.VEHICLE
                           where vehiclesSQL.STATE == true
                           select new {
                               ID = vehiclesSQL.ID_VEHICLE,
                               PLACA = vehiclesSQL.PLACA,
                               PROPIETARIO = vehiclesSQL.OWNER.NAME1,
                               MARCA = vehiclesSQL.MODEL.TRADEMARK.NAME,
                               MODELO = vehiclesSQL.MODEL.NAME,
                               YEAR = vehiclesSQL.YEAR
                           };
            vehicleTable= Vehicles;


        }

        // Validations about existence
        public bool existLicensePlate(string placa) {
            var vehicleComp = (from comp in Conection.VEHICLE
                               select comp.PLACA);

            foreach (string c in vehicleComp) {
                if (c.Equals(placa)) {
                    
                    return true;

                }
            }
            return false;

        }

        public bool existTradeMark(string tradeMarkName) {
            var tradeMarkComp = (from comp in Conection.TRADEMARK
                               select comp.NAME);

            foreach (string c in tradeMarkComp) {
                if (c.Equals(tradeMarkName)) {

                    return true;

                }
            }
            return false;

        }

        public bool existOwner(string dpi) {
            var ownerComp = (from comp in Conection.OWNER
                               select comp.DPI);

            foreach (string c in ownerComp) {
                if (c.Equals(dpi)) {

                    return true;

                }
            }
            return false;

        }

        public bool existModel(string Name) {
            var ModelComp = (from comp in Conection.MODEL
                             select comp.NAME);

            foreach (string c in ModelComp) {
                if (c.Equals(Name)) {

                    return true;

                }
            }
            return false;

        }

        //Metodos ADD Agregan elementos a sus respectivos listados y tablas
        //Esto recibira solo IDS
        public void AddVehicle(string placa, int model, int owner, string year, string stateVehicle) {           
 
            var modelo = (from k in Conection.MODEL
                         where k.ID_MODEL== model
                         select k).FirstOrDefault();

            var propi = (from l in Conection.OWNER
                          where l.ID_OWNER == owner
                          select l).FirstOrDefault();

            BasesDeDatos.VEHICLE v = new BasesDeDatos.VEHICLE {
                PLACA= placa,
                MODEL = modelo,
                OWNER= propi,
                YEAR= year,
                VEHICLE_STATE= stateVehicle,
                STATE = true
            };
            Conection.VEHICLE.InsertOnSubmit(v);
            Conection.SubmitChanges();
            updateTables();
        }

        public void AddTradeMark(string name) {
            BasesDeDatos.TRADEMARK v = new BasesDeDatos.TRADEMARK {
                NAME = name,
                STATE = true
            };
            Conection.TRADEMARK.InsertOnSubmit(v);
            Conection.SubmitChanges();
            updateTables();
        }

        public void AddOwner(string name1, string name2, string last1, string last2, string tel, string dpi, string genero, string direccion, string email, string nit) {
            BasesDeDatos.OWNER v = new BasesDeDatos.OWNER {
                NAME1 = name1,
                NAME2= name2,
                LASTN1 = last1,
                LASTN2 = last2,
                NUMBERPHONE = tel,
                DPI= dpi,
                GENRE = genero,
                DIRECTION = direccion,
                EMAIL = email,
                NIT = nit,
                STATE = true
            };
            Conection.OWNER.InsertOnSubmit(v);
            Conection.SubmitChanges();
            updateTables();
        }

        public void AddModel(int idTradeMark, string name) {
            var tt = (from i in Conection.TRADEMARK
                     where i.ID_TRADEMARK== idTradeMark
                     select i).FirstOrDefault();

            BasesDeDatos.MODEL m = new BasesDeDatos.MODEL {
                TRADEMARK = tt,
                NAME = name,
                STATE = true
            };
            Conection.MODEL.InsertOnSubmit(m);
            Conection.SubmitChanges();
            updateTables();
        }

        //Metodos MODIFICAR

        public void ModifyVehicle(int ID, string placa, int model, int owner, string year, string stateVehicle) {
            var modV = (from mod in Conection.VEHICLE
                        where mod.ID_VEHICLE == ID
                        select mod).FirstOrDefault();

            var modelo = (from mod in Conection.MODEL
                          where mod.ID_MODEL == model
                          select mod).FirstOrDefault();

            var propie = (from mod in Conection.OWNER
                          where mod.ID_OWNER == owner
                          select mod).FirstOrDefault();

            modV.PLACA = placa;
            modV.MODEL = modelo;
            modV.OWNER = propie;
            modV.YEAR = year;
            modV.VEHICLE_STATE = stateVehicle;

            Conection.SubmitChanges();
            updateTables();
        }

        public void ModifyTradeMark(int id, string name, bool state) {
            var modTradeMark = (from mod in Conection.TRADEMARK
                               where mod.ID_TRADEMARK== id
                               select mod).FirstOrDefault();

            modTradeMark.STATE = state;
            modTradeMark.NAME = name;

            Conection.SubmitChanges();
            updateTables();
        }

        public void ModifyOwner(int id, string name1, string name2, string last1, string last2, string tel, string dpi, string genero, string direccion, string email, string nit, bool estado) {
            var oo = (from mod in Conection.OWNER
                                where mod.ID_OWNER == id
                                select mod).FirstOrDefault();

            oo.NAME1= name1;
            oo.NAME2= name2;
            oo.LASTN1= last1;
            oo.LASTN2= last2;
            oo.NUMBERPHONE= tel;
            oo.DPI= dpi;
            oo.GENRE= genero;
            oo.DIRECTION= direccion;
            oo.EMAIL= email;
            oo.NIT= nit;
            oo.STATE= estado;

            Conection.SubmitChanges();
            updateTables();
        }

        public void ModifyModel(int IDTradeMark, int id, string name, bool state) {
            var modModel = (from mod in Conection.MODEL
                                where mod.ID_MODEL == id
                                select mod).FirstOrDefault();


            var trade = (from k in Conection.TRADEMARK
                         where k.ID_TRADEMARK == IDTradeMark
                         select k).FirstOrDefault();

            modModel.NAME = name;
            modModel.TRADEMARK = trade;
            modModel.STATE = state;

            Conection.SubmitChanges();
            updateTables();
        }

        // Metodos DELETE
        public void DeleteVehicle(int ID) {

            var modV = (from mod in Conection.VEHICLE
                        where mod.ID_VEHICLE == ID
                        select mod).FirstOrDefault();

            modV.STATE = false;

            Conection.SubmitChanges();
            updateTables();
        }

        public void DeleteTradeMark(int id) {

            var modTradeMark = (from mod in Conection.TRADEMARK
                                where mod.ID_TRADEMARK == id
                                select mod).FirstOrDefault();

            modTradeMark.STATE = false;

            Conection.SubmitChanges();
            updateTables();
        }

        public void DeleteOwner(int id) {

            var oo = (from mod in Conection.OWNER
                      where mod.ID_OWNER == id
                      select mod).FirstOrDefault();

            oo.STATE = false;

            Conection.SubmitChanges();
            updateTables();
        }

        public void DeleteModel(int id) {

            var modModel = (from mod in Conection.MODEL
                            where mod.ID_MODEL == id
                            select mod).FirstOrDefault();


            modModel.STATE = false;

            Conection.SubmitChanges();
            updateTables();
        }

        public IQueryable EnableVehicle() {
            var Vehicles = from vehiclesSQL in Conection.VEHICLE
                           where vehiclesSQL.STATE == true
                           select new {
                               ID = vehiclesSQL.ID_VEHICLE,
                               PLACA = vehiclesSQL.PLACA,
                               PROPIETARIO = vehiclesSQL.OWNER.NAME1,
                               MARCA = vehiclesSQL.MODEL.TRADEMARK.NAME,
                               MODELO = vehiclesSQL.MODEL.NAME,
                               YEAR = vehiclesSQL.YEAR
                           };
            
            return Vehicles;
        }

        public List<string> VehicleStates() {
            List<string> model = new List<string>();

            model.Add("USADO");
            model.Add("NUEVO");
            model.Add("SEMI-NUEVO");

            return model;
        }
        public List<string> BooleanStates() {
            List<string> model = new List<string>();

            model.Add("True");
            model.Add("False");

            return model;
        }

        public List<string> genreStates() {
            List<string> model = new List<string>();

            model.Add("MASCULINO");
            model.Add("FEMENINO");

            return model;
        }

        public BasesDeDatos.VEHICLE getVehicleByID(int id) {

            var key = (from m in Conection.VEHICLE
                       where m.ID_VEHICLE == id
                       select m).FirstOrDefault();

            return key;
        }
        //Enable List Combobox
        public IQueryable EnableTradeMarksCombo() {
            var tradeMark = from tradeMarkSQL in Conection.TRADEMARK
                            where tradeMarkSQL.STATE == true
                            select new {
                                ID = tradeMarkSQL.ID_TRADEMARK,
                                MARK = tradeMarkSQL.NAME,
                            };
            return tradeMark;
        }
        public IQueryable EnableModelCombo() {
            var Model = from ModelSQL in Conection.MODEL
                        where ModelSQL.STATE == true
                        select new {
                            ID = ModelSQL.ID_MODEL,
                            MODEL = ModelSQL.NAME
                        };
            return Model;
        }
        public IQueryable EnableOwnerCombo() {
            var Owner = from OwnerSQL in Conection.OWNER
                        where OwnerSQL.STATE == true
                        select new {
                            ID = OwnerSQL.ID_OWNER,
                            NOMBRE = OwnerSQL.NAME1
                        };
            return Owner;
        }
        public IQueryable EnableModelCombo(int IDtradeMark) {
            var Model = from ModelSQL in Conection.MODEL
                        where ModelSQL.STATE == true && ModelSQL.ID_TRADEMARK == IDtradeMark
                        select new {
                            ID = ModelSQL.ID_MODEL,
                            MODEL = ModelSQL.NAME,
                        };
            return Model;
        }

         
    }
}

