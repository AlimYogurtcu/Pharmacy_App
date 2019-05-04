﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SQLite;
using System.Xml.Linq;

namespace Pharmacy_App
{
    public partial class AdminPanelAdd : Form
    {
        SQLiteConnection conn = new SQLiteConnection(@"Data Source= medicines.db"); //sql

        List<medicineRecords> temporaryMedicineRecordList = new List<medicineRecords>();

        string xmlFileLocation = @"C:/Users/Public/PharmacyAppData/medicineInfo.xml"; // xml file location
        public AdminPanelAdd()
        {
            InitializeComponent();
        }

        public void Form_Reload(object sender, EventArgs e)
        {
            listViewMedicines.Items.Clear();
            listViewMedicines.Columns.Clear();
            
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //decleration of values for get informations from textboxes

            string name = "", experationDate, status = "", category,errorMessage = "";
            double cost = 0,
                   price = 0,
                   mg = 0;
            int amount = 0;
                

            //*sql add
            conn.Open();
            string sql = "insert into Medicines(Name, Category, Milligram, Amount, Cost, Price, ExperationDate, UpdatedDate) Values('" + textBoxName.Text + "', '" + comboBoxCategory.Text + "', '" + textBoxMg.Text + "', '" + textBoxAmount.Text + "', '" + textBoxCost.Text + "', '" + textBoxPrice.Text + "', '" + dateTimePickerExpirationDate.Text + "', '" + System.DateTime.Now + "')";
            SQLiteCommand uploadDB = new SQLiteCommand(sql, conn);
            uploadDB.ExecuteNonQuery();
            conn.Close();
            //*sql add

            bool validation = true;// value for check if medicine is valid or not


            //---------------------------------------------------------


            // getting values from textboxes to values with *validations !!!

            // for validation, validation bool value used. If
            // valudation is true program continues to check 
            // values. If all values correctly writted adding
            // values to xml document proccess is starts. Else
            // gives errors for each value.

            name = textBoxName.Text.ToString();

            if (name == "")
            {
                errorMessage += "\nName";
                textBoxName.Text = "";
                textBoxName.Focus();
            }
            else { /*doNothing*/}

            try
            {
                amount = int.Parse(textBoxAmount.Text.ToString());
            }
            catch (Exception Ex)
            {
                textBoxAmount.Text = "";
                errorMessage += "\nAmount";
                textBoxAmount.Focus();
            }

            try
            {
                mg = double.Parse(textBoxMg.Text.ToString());

            }
            catch (Exception Ex)
            {
                textBoxMg.Text = "";
                errorMessage += "\nMg";
                textBoxMg.Focus();
            }

            try
            {
                cost = double.Parse(textBoxCost.Text.ToString());
            }
            catch (Exception Ex)
            {
                textBoxCost.Text = "";
                errorMessage += "\nCost";
                textBoxCost.Text = "";
                textBoxCost.Focus();
            }

            try
            {
                
                price = double.Parse(textBoxPrice.Text.ToString());
            }
            catch
            {
                textBoxPrice.Text = "";
                errorMessage = errorMessage += "\nCost";
                textBoxPrice.Text = "";
                textBoxPrice.Focus();
            }


            experationDate = dateTimePickerExpirationDate.Text.ToString();
            category = comboBoxCategory.Text.ToString();


            if (category == "")
            {
                errorMessage += "\nCategory";
                comboBoxCategory.Focus();
            }
            else {/*doNothing*/}


            if (radioButtonSaleable.Checked) //here we are get status from salable radioButton
            {
                status = radioButtonSaleable.Text.ToString();
            }

            else if (radioButtonUnsaleable.Checked) //here we are get status from unSalable radioButton
            {
                status = radioButtonUnsaleable.Text.ToString();
            }


            if(errorMessage == "")
            {
                validation = true;
            }
            else
            {
                validation = false;
                MessageBox.Show("Invalid inputs found. Please check your ınputs and try again! \n" + errorMessage, "value input eror message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //-------------------------------------------

            if (validation)
            {
                // Asks if user wants to continue to add medicine.
                // If dialog result is Yes. Program adds the medicine.
                // If dialog result is no Program dont do anything.

                if(MessageBox.Show("Do you want continue to add this medicine ?\n\n" + "name : " + name + "\n" + "category : " +category + "\n" + "amount :" + amount + "\n" + "mg : " + mg + "\n" + "experationDate : " + experationDate + "\n" + "cost : " + cost + "\n" + "price : " + price + "\n" + "status : " + status, "validation of adding process", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    /*--------------------sql code here--------------------*/

                    Form_Reload(sender, e);

                    // Adding colums to the list view

                    listViewMedicines.Columns.Add(" ", 25, HorizontalAlignment.Center);
                    listViewMedicines.Columns.Add("Name", 170, HorizontalAlignment.Left);
                    listViewMedicines.Columns.Add("Category", 150, HorizontalAlignment.Center);
                    listViewMedicines.Columns.Add("Mg", 70, HorizontalAlignment.Center);
                    listViewMedicines.Columns.Add("Expiration Date", 150, HorizontalAlignment.Center);
                    listViewMedicines.Columns.Add("Amount", 70, HorizontalAlignment.Center);
                    listViewMedicines.Columns.Add("Cost", 70, HorizontalAlignment.Center);
                    listViewMedicines.Columns.Add("Price", 70, HorizontalAlignment.Center);
                    listViewMedicines.Columns.Add("Status", 100, HorizontalAlignment.Center);
                    listViewMedicines.Columns.Add("Upload Date", 150, HorizontalAlignment.Center);

                    //------------------------------

                    temporaryMedicineRecordList.Add(new medicineRecords // adding new item to temporary list.
                    {
                        name = name,
                        category = category,
                        mg = mg,
                        amount = amount,
                        cost = cost,
                        price = price,
                        experationDate = experationDate,
                        status = status,
                        updatedDate = System.DateTime.Now.ToString(),

                });

                    for (var i = 0; i < temporaryMedicineRecordList.Count; i++)// Adding temporaryMedicineRecors list's elements to the list view 
                    {
                        ListViewItem row = new ListViewItem((i + 1).ToString());

                        ListViewItem.ListViewSubItem itms1 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].name.ToString());
                        ListViewItem.ListViewSubItem itms8 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].category.ToString());
                        ListViewItem.ListViewSubItem itms2 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].mg.ToString());
                        ListViewItem.ListViewSubItem itms3 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].experationDate.ToString());
                        ListViewItem.ListViewSubItem itms4 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].amount.ToString());
                        ListViewItem.ListViewSubItem itms5 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].cost.ToString());
                        ListViewItem.ListViewSubItem itms6 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].price.ToString());
                        ListViewItem.ListViewSubItem itms7 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].status.ToString());
                        ListViewItem.ListViewSubItem itms9 = new ListViewItem.ListViewSubItem(row, temporaryMedicineRecordList[i].updatedDate.ToString());


                        row.SubItems.Add(itms1);
                        row.SubItems.Add(itms8);
                        row.SubItems.Add(itms2);
                        row.SubItems.Add(itms3);
                        row.SubItems.Add(itms4);
                        row.SubItems.Add(itms5);
                        row.SubItems.Add(itms6);
                        row.SubItems.Add(itms7);
                        row.SubItems.Add(itms9);

                        listViewMedicines.Items.Add(row);
                    }

                    var doc = XDocument.Load(@xmlFileLocation); //opening xml file

                    //adding new element and saving it 
                    var newElement = new XElement("medicine",
                    new XElement("name", name),
                    new XElement("mg", mg),
                    new XElement("amount", amount),
                    new XElement("cost", cost),
                    new XElement("price", price),
                    new XElement("experationDate", experationDate),
                    new XElement("status", status),
                    new XElement("category", category),
                    new XElement("updatedDate", System.DateTime.Now.ToString()));

                    doc.Element("medicines").Add(newElement);

                    doc.Save(@xmlFileLocation);

                    MessageBox.Show("Medicine is added.", "add proccess complation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBoxName.Text = "";
                    textBoxMg.Text = "";
                    textBoxAmount.Text = "";
                    textBoxMg.Text = "";
                    textBoxCost.Text = "";
                    textBoxPrice.Text = "";
                    comboBoxCategory.Text = "";
                    radioButtonSaleable.Checked = true;

                    

                }
                else
                {
                    /*doNothing*/
                }
                
            }
            else
            {
                //do nothing
            }

        }

        private void AdminPanelAdd_Load(object sender, EventArgs e)
        {
            radioButtonSaleable.Checked = true;// Radio button salable is checked

            this.comboBoxCategory.DropDownStyle = ComboBoxStyle.DropDownList; // Makes combo box unvriteable
        }

        private void ButtonReturn_Click_1(object sender, EventArgs e)
        {
            // For return to admin main panel

            AdminPanel AP = new AdminPanel();
            AP.Show();
            this.Close();

        }



    }
}
