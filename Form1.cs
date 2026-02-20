using System.ComponentModel;
using System.Data;
using System.Text;

namespace PC_Parts_Inventory_Management_System
{


    public partial class Form1 : Form
    {

        BindingList<PcPart> inventoryList = new BindingList<PcPart>(); //binding list so it immediately update the ui


        public Form1()
        {
            InitializeComponent();
            dgvInventory.DataSource = inventoryList;
            PopulateCategoryDropdown();

        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv";
            sfd.FileName = "InventoryExport.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();


                sb.AppendLine("Category,Manufacturer,Model,Specs,Price,Stock");//header

                //traverse binding list 
                foreach (PcPart part in inventoryList)
                {

                    sb.AppendLine($"{part.Category},{part.Manufacturer},{part.Model},{part.Specs},{part.Price},{part.Stock}"); //separate by comma
                }

                File.WriteAllText(sfd.FileName, sb.ToString());
                MessageBox.Show("Export Successful.");
            }
        }


        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (dgvInventory.CurrentRow != null && !dgvInventory.CurrentRow.IsNewRow)
            {
                PcPart? selectedPart = (PcPart?)dgvInventory.CurrentRow.DataBoundItem;
                inventoryList.Remove(selectedPart);
            }
        }


        private void PopulateCategoryDropdown()
        {

            comboBox1.Items.Add("CPU");
            comboBox1.Items.Add("GPU");
            comboBox1.Items.Add("Motherboard");
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {

            PcPart newPart = new PcPart
            {
                Category = comboBox1.Text,
                Manufacturer = comboBox2.Text,
                Model = textBox2.Text,
                Specs = textBox3.Text,
                Price = Convert.ToDecimal(textBox4.Text),
                Stock = Convert.ToInt32(textBox5.Text)
            };


            inventoryList.Add(newPart);

            ClearInputFields();
        }
        private void ClearInputFields()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV files (*.csv)|*.csv";
            ofd.Title = "Select CSV File";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(ofd.FileName);

                if (lines.Length > 0)
                {
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] cellData = lines[i].Split(',');

                        if (cellData.Length == 6)
                        {

                            PcPart importedPart = new PcPart
                            {
                                Category = cellData[0],
                                Manufacturer = cellData[1],
                                Model = cellData[2],
                                Specs = cellData[3],
                                Price = Convert.ToDecimal(cellData[4]),
                                Stock = Convert.ToInt32(cellData[5])
                            };

                            inventoryList.Add(importedPart);
                        }
                        else
                        {
                            MessageBox.Show(
                                $"Error importing row {i + 1}. Expected 6 elements, found {cellData.Length}. Row skipped.",
                                "Import Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    MessageBox.Show("Import Complete.");
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            if (comboBox1.SelectedItem == null) return;

            string? selectedCategory = comboBox1.SelectedItem.ToString();

            if (selectedCategory == "CPU")
            {
                comboBox2.Items.Add("Intel");
                comboBox2.Items.Add("AMD");
            }
            else if (selectedCategory == "GPU")
            {
                comboBox2.Items.Add("Nvidia");
                comboBox2.Items.Add("AMD");
                comboBox2.Items.Add("Intel");
            }
            else if (selectedCategory == "Motherboard")
            {
                comboBox2.Items.Add("ASUS");
                comboBox2.Items.Add("MSI");
            }

            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
    public class PcPart
    {
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? Specs { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
    }
}
