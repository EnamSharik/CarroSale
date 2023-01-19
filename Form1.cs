using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using DevExpress.XtraExport.Helpers;
using DevExpress.DocumentView.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Tablas {
    public partial class Form1 : Form {

        DB DB;

        List<string> tableList = new List<string>();
        int indice = 0;
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

            DB = new DB();
            sinc();


        }

        private void sinc() {
            //Este metodo será utilizado para actualizar todas las tablas

            gridControl_tradeMark.DataSource = DB.tradeMarkTable;
            gridView_tradeMark.Columns[0].Visible = false;

            gridControl1.DataSource = DB.EnableVehicle();
            gridView_vehicles.Columns[0].Visible = false;

            gridControl_owner.DataSource = DB.ownerTable;
            gridView_owner.Columns[0].Visible = false;

            gridControl_model.DataSource = DB.modelTable;
            gridView_model.Columns[0].Visible = false;

            //Aqui se actualizan los combobox

            //Vehiculos
            entry_vehicle_stateOfVehicle.Properties.DataSource = DB.VehicleStates();

            entry_vehicle_owner.Properties.DataSource = DB.EnableOwnerCombo();
            entry_vehicle_owner.Properties.DisplayMember = "NOMBRE";
            entry_vehicle_owner.Properties.ValueMember = "ID";


            entry_vehicle_tradeMark.Properties.DataSource = DB.EnableTradeMarksCombo();
            entry_vehicle_tradeMark.Properties.DisplayMember = "MARK";
            entry_vehicle_tradeMark.Properties.ValueMember = "ID";

            entry_vehicle_model.Properties.DataSource = DB.EnableModelCombo();
            entry_vehicle_model.Properties.DisplayMember = "MODEL";
            entry_vehicle_model.Properties.ValueMember = "ID";

            // Congelar botones
            button_tradeMark_delete.Enabled = false;
            button_tradeMark_modify.Enabled = false;

            button_vehicle_delete.Enabled = false;
            button_vehicle_modify.Enabled = false;

            button_model_modify.Enabled = false;
            button_model_delete.Enabled = false;

            button_owner_delete.Enabled = false;
            button_owner_modify.Enabled = false;

            //BooleanStates

                //tradeMark
            entry_tradeMark_state.Properties.DataSource = DB.BooleanStates();

            //Model
            entry_model_tradeMark.Properties.DataSource = DB.EnableTradeMarksCombo();
            entry_model_tradeMark.Properties.DisplayMember = "MARK";
            entry_model_tradeMark.Properties.ValueMember = "ID";
            entry_model_state.Properties.DataSource = DB.BooleanStates();


                //Owner
            entry_owner_genre.Properties.DataSource = DB.genreStates();
            entry_owner_state.Properties.DataSource = DB.BooleanStates();

        }


        //add_VEHICLE
        private void simpleButton1_Click(object sender, EventArgs e) {
            string placa = entry_vehicle_placa.Text;
            if (DB.existLicensePlate(placa)) {
                alertControl1.Show(this, "DUPLICATE LICENSE PLATE", "Please, Check your license plate, already exist");
            } else {
                DB.AddVehicle(placa, Convert.ToInt32(entry_vehicle_model.GetColumnValue("ID")), Convert.ToInt32(entry_vehicle_owner.GetColumnValue("ID")), entry_vehicle_year.Text, entry_vehicle_stateOfVehicle.Text);
            }

            //CleanTextBox();
            entry_vehicle_placa.Text = String.Empty;
            entry_vehicle_model.EditValue = 1;
            entry_vehicle_owner.EditValue = 1;
            entry_vehicle_tradeMark.EditValue = 1;
            entry_vehicle_year.Text= String.Empty;
            entry_vehicle_stateOfVehicle.Text = String.Empty;

            sinc();
        }

        private void button_vehicle_modify_Click(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_vehicles.GetFocusedRowCellValue("ID"));

            button_vehicle_add.Enabled = true;
            entry_vehicle_placa.Enabled = true;

            DB.ModifyVehicle(indice, entry_vehicle_placa.Text, Convert.ToInt32(entry_vehicle_model.GetColumnValue("ID")), Convert.ToInt32(entry_vehicle_owner.GetColumnValue("ID")), entry_vehicle_year.Text, entry_vehicle_stateOfVehicle.Text);

            //CleanTextBox();
            entry_vehicle_placa.Text = String.Empty;
            entry_vehicle_model.EditValue = 1;
            entry_vehicle_owner.EditValue = 1;
            entry_vehicle_tradeMark.EditValue = 1;
            entry_vehicle_year.Text = String.Empty;
            entry_vehicle_stateOfVehicle.Text = String.Empty;

            button_vehicle_add.Enabled = true;
            sinc();
        }

        private void button_vehicle_delete_Click(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_vehicles.GetFocusedRowCellValue("ID"));

            DB.DeleteVehicle(indice);

            button_vehicle_add.Enabled = true;
            button_vehicle_modify.Enabled = true;
            button_vehicle_delete.Enabled = true;

            //CleanTextBox();
            entry_vehicle_placa.Text = String.Empty;
            entry_vehicle_model.EditValue = 1;
            entry_vehicle_owner.EditValue = 1;
            entry_vehicle_tradeMark.EditValue = 1;
            entry_vehicle_year.Text = String.Empty;
            entry_vehicle_stateOfVehicle.Text = String.Empty;

            sinc();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_vehicles.GetFocusedRowCellValue("ID"));

            entry_vehicle_placa.EditValue = DB.getVehicleByID(indice).PLACA;
            entry_vehicle_tradeMark.EditValue = DB.getVehicleByID(indice).MODEL.TRADEMARK.ID_TRADEMARK;
            entry_vehicle_model.EditValue = DB.getVehicleByID(indice).MODEL.ID_MODEL;
            entry_vehicle_owner.EditValue = DB.getVehicleByID(indice).OWNER.ID_OWNER;
            entry_vehicle_year.EditValue = DB.getVehicleByID(indice).YEAR;
            entry_vehicle_stateOfVehicle.EditValue = DB.getVehicleByID(indice).VEHICLE_STATE;

            button_vehicle_add.Enabled= false;
            button_vehicle_delete.Enabled= true;
            button_vehicle_modify.Enabled= true;
        }

        private void gridControl_tradeMark_DoubleClick(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_tradeMark.GetFocusedRowCellValue("ID"));

            entry_tradeMark_name.Text = Convert.ToString(gridView_tradeMark.GetFocusedRowCellValue("MARK"));
            entry_tradeMark_state.Text = Convert.ToString(gridView_tradeMark.GetFocusedRowCellValue("STATE"));

            button_tradeMark_add.Enabled = false;
            button_tradeMark_delete.Enabled = true;
            button_tradeMark_modify.Enabled = true;
        }

        private void button_tradeMark_add_Click(object sender, EventArgs e) {
            string trade = entry_tradeMark_name.Text;
            if (DB.existTradeMark(trade)) {
                alertControl1.Show(this, "DUPLICATE TRADEMARK", "Please, Check your trade mark, already exist");
            } else {
                DB.AddTradeMark(trade);
            }

            //CleanTextBox();
            entry_tradeMark_state.EditValue = 1;
            entry_tradeMark_name.Text = String.Empty;
            sinc();
        }

        private void button_tradeMark_modify_Click(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_tradeMark.GetFocusedRowCellValue("ID"));

            button_tradeMark_add.Enabled = true;

            DB.ModifyTradeMark(indice, entry_tradeMark_name.Text, Convert.ToBoolean(entry_tradeMark_state.Text));

            //CleanTextBox();
            entry_tradeMark_state.EditValue = 1;
            entry_tradeMark_name.Text = String.Empty;

            button_tradeMark_add.Enabled = true;
            sinc();
        }

        private void button_tradeMark_delete_Click(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_tradeMark.GetFocusedRowCellValue("ID"));

            DB.DeleteTradeMark(indice);

            button_tradeMark_add.Enabled = true;
            button_tradeMark_modify.Enabled = true;
            button_tradeMark_delete.Enabled = true;

            //CleanTextBox();
            entry_tradeMark_state.EditValue = 1;
            entry_tradeMark_name.Text = String.Empty;

            sinc();
        }

        private void gridControl_owner_DoubleClick(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_owner.GetFocusedRowCellValue("ID"));

            entry_owner_name1.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("NOMBRE1"));
            entry_owner_name2.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("NOMBRE2"));
            entry_owner_l1.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("APELLIDO1"));
            entry_owner_l2.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("APELLIDO2"));
            entry_owner_phone.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("TELEFONO"));
            entry_owner_dpi.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("DPI"));
            entry_owner_genre.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("GENERO"));
            entry_owner_address.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("DIRECCION"));
            entry_owner_email.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("EMAIL"));
            entry_owner_nit.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("NIT"));
            entry_owner_state.Text = Convert.ToString(gridView_owner.GetFocusedRowCellValue("STATE"));


            button_owner_add.Enabled = false;
            button_owner_delete.Enabled = true;
            button_owner_modify.Enabled = true;
        }

        private void button_owner_add_Click(object sender, EventArgs e) {
            string n1 = entry_owner_name1.Text;
            string n2 = entry_owner_name2.Text;
            string l1 = entry_owner_l1.Text;
            string l2 = entry_owner_l2.Text;
            string NPhone = entry_owner_phone.Text;
            string dpi = entry_owner_dpi.Text;
            string gen = entry_owner_genre.Text;
            string addr = entry_owner_address.Text;
            string email = entry_owner_email.Text;
            string nit = entry_owner_nit.Text;
            
            if (DB.existOwner(dpi)) {
                alertControl1.Show(this, "DUPLICATE OWNER", "Please, Check your owner, already exist");
            } else {
                DB.AddOwner(n1, n2, l1, l2, NPhone, dpi, gen, addr, email, nit);
            }

            entry_owner_name1.Text = string.Empty;
            entry_owner_name2.Text = string.Empty;
            entry_owner_l1.Text = string.Empty;
            entry_owner_l2.Text = string.Empty;
            entry_owner_phone.Text = string.Empty;
            entry_owner_dpi.Text = string.Empty;
            entry_owner_genre.Text = string.Empty;
            entry_owner_address.Text = string.Empty;
            entry_owner_email.Text = string.Empty;
            entry_owner_nit.Text = string.Empty;
            entry_owner_state.EditValue = 1;

            sinc();
        }

        private void button_owner_modify_Click(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_owner.GetFocusedRowCellValue("ID"));

            string n1 = entry_owner_name1.Text;
            string n2 = entry_owner_name2.Text;
            string l1 = entry_owner_l1.Text;
            string l2 = entry_owner_l2.Text;
            string NPhone = entry_owner_phone.Text;
            string dpi = entry_owner_dpi.Text;
            string gen = entry_owner_genre.Text;
            string addr = entry_owner_address.Text;
            string email = entry_owner_email.Text;
            string nit = entry_owner_nit.Text;
            bool st = Convert.ToBoolean(entry_owner_state.Text);

            DB.ModifyOwner(indice, n1, n2, l1, l2, NPhone, dpi, gen, addr, email, nit, st);

            entry_owner_name1.Text = string.Empty;
            entry_owner_name2.Text = string.Empty;
            entry_owner_l1.Text = string.Empty;
            entry_owner_l2.Text = string.Empty;
            entry_owner_phone.Text = string.Empty;
            entry_owner_dpi.Text = string.Empty;
            entry_owner_genre.Text = string.Empty;
            entry_owner_address.Text = string.Empty;
            entry_owner_email.Text = string.Empty;
            entry_owner_nit.Text = string.Empty;
            entry_owner_state.EditValue = 1;
            
            button_owner_modify.Enabled = false;
            button_owner_delete.Enabled = false;
            button_owner_add.Enabled = true;

            sinc();
        }

        private void button_owner_delete_Click(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_owner.GetFocusedRowCellValue("ID"));

            DB.DeleteOwner(indice);

            entry_owner_name1.Text = string.Empty;
            entry_owner_name2.Text = string.Empty;
            entry_owner_l1.Text = string.Empty;
            entry_owner_l2.Text = string.Empty;
            entry_owner_phone.Text = string.Empty;
            entry_owner_dpi.Text = string.Empty;
            entry_owner_genre.Text = string.Empty;
            entry_owner_address.Text = string.Empty;
            entry_owner_email.Text = string.Empty;
            entry_owner_nit.Text = string.Empty;
            entry_owner_state.EditValue = 1;



            button_owner_modify.Enabled = false;
            button_owner_delete.Enabled = false;
            button_owner_add.Enabled = true;

            sinc();
        }

        private void button_model_add_Click(object sender, EventArgs e) {
            string name = entry_model_name.Text;

            if (DB.existModel(name)) {
                alertControl1.Show(this, "DUPLICATE MODEL", "Please, Check your model, already exist");
            } else {
                DB.AddModel(Convert.ToInt32(entry_model_tradeMark.GetColumnValue("ID")), entry_model_name.Text);
            }

            entry_model_name.Text = String.Empty;
            entry_model_tradeMark.EditValue = 1;
            
            sinc();
        }

        private void button_model_modify_Click(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_model.GetFocusedRowCellValue("ID"));

            DB.ModifyModel(Convert.ToInt32(entry_model_tradeMark.GetColumnValue("ID")), indice, entry_model_name.Text, Convert.ToBoolean(entry_model_state.Text));

            entry_model_name.Text = String.Empty;
            entry_model_tradeMark.Text = String.Empty;
            entry_model_state.EditValue = 1;


            button_model_add.Enabled = true;
            button_model_modify.Enabled = false;
            button_model_delete.Enabled = false;
            
            sinc();
        }

        private void button_model_delete_Click(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_model.GetFocusedRowCellValue("ID"));

            DB.DeleteModel(indice);

            
            button_model_add.Enabled = true;
            button_model_modify.Enabled = false;
            button_model_delete.Enabled = false;

            entry_model_name.Text = string.Empty;
            entry_model_tradeMark.Text = string.Empty;
            
            sinc();
        }

        private void gridControl_model_DoubleClick(object sender, EventArgs e) {
            indice = Convert.ToInt32(gridView_model.GetFocusedRowCellValue("ID"));
            entry_model_name.Text = Convert.ToString(gridView_model.GetFocusedRowCellValue("MODEL"));
            entry_model_tradeMark.Text = Convert.ToString(gridView_model.GetFocusedRowCellValue("MARK"));
            entry_model_state.Text = Convert.ToString(gridView_model.GetFocusedRowCellValue("STATE"));

            button_model_add.Enabled = false;
            button_model_delete.Enabled = true;
            button_model_modify.Enabled = true;
        }

        private void entry_vehicle_tradeMark_EditValueChanged(object sender, EventArgs e) {
            int k = Convert.ToInt32(entry_vehicle_tradeMark.GetColumnValue("ID"));
            entry_vehicle_model.Properties.DataSource = DB.EnableModelCombo(k);
        }
    }
}
