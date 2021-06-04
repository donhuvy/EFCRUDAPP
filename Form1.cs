using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace EFCRUDAPP
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();


        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            using (EFDBEntities db = new EFDBEntities())
            {
                if (model.CustomerID == 0) // Insert
                {
                    db.Customers.Add(model);
                }
                else // Update
                {
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            Clear();
            MessageBox.Show("Submitted successfully.");
            PopulateDataGridView();
        }

        void Clear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;

            model.CustomerID = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        void PopulateDataGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;

            using (EFDBEntities db = new EFDBEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            {
                if (dgvCustomer.CurrentRow.Index != -1)
                {
                    model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                    using (EFDBEntities db = new EFDBEntities())
                    {
                        model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                        txtFirstName.Text = model.FirstName;
                        txtLastName.Text = model.LastName;
                        txtCity.Text = model.City;
                        txtAddress.Text = model.Address;
                    }
                    btnSave.Text = "Update";
                    btnDelete.Enabled = true;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this record?", "EF CRUD Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (EFDBEntities db = new EFDBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                    {
                        db.Customers.Attach(model);
                    }
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Delete successfully.");
                }
            }
        }
    }
}
