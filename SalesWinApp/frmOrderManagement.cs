using BusinessObject;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmOrderManagement : Form
    {
        public frmOrderManagement(MemberObject user)
        {
            InitializeComponent();
            loggedInUser = user;
        }

        IOrderRepository orderRepository = new OrderRepository();
        BindingSource source = null;
        private MemberObject loggedInUser { get; set; }



        private OrderObject GetOrderObject()
        {
            OrderObject order = new OrderObject
            {
                OrderId = int.Parse(txtOrderID.Text),
                MemberId = int.Parse(txtMemberId.Text),
                OrderDate = DateTime.Parse(txtOderDate.Text),
                ShippedDate = DateTime.Parse(txtShippedDate.Text),
                RequireDate = DateTime.Parse(txtRequireDate.Text),
                Freight = decimal.Parse(txtFreight.Text),
            };

            return order;
        }


            private void ClearText()
        {
            txtMemberId.Text = string.Empty;
            txtOrderID.Text = string.Empty;
            txtOderDate.Text = string.Empty;
            txtRequireDate.Text = string.Empty;
            txtShippedDate.Text = string.Empty;
            txtFreight.Text = string.Empty;
        }

        public void LoadOrderList()
        {
            IEnumerable<OrderObject> order = null;
            order = orderRepository.GetOrderList();

            try
            {
                source = new BindingSource();
                source.DataSource = order;

                txtMemberId.DataBindings.Clear();
                txtOrderID.DataBindings.Clear();
                txtOderDate.DataBindings.Clear();
                txtRequireDate.DataBindings.Clear();
                txtShippedDate.DataBindings.Clear();
                txtFreight.DataBindings.Clear();

                txtOrderID.DataBindings.Add("Text", source, "OrderId");
                txtMemberId.DataBindings.Add("Text", source, "MemberId");
                txtOderDate.DataBindings.Add("Text", source, "OrderDate");
                txtShippedDate.DataBindings.Add("Text", source, "ShippedDate");
                txtRequireDate.DataBindings.Add("Text", source, "RequireDate");
                txtFreight.DataBindings.Add("Text", source, "Freight");

                dgvOrder.DataSource = null;
                dgvOrder.DataSource = source;
                if (order.Count() == 0)
                {
                    ClearText();
                    btnDelete.Enabled = false;
                }
                if (loggedInUser.Email == "admin@fstore.com")
                {
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Member List");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadOrderList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var order = GetOrderObject();
                orderRepository.DeleteOrderById(order.MemberId);
                LoadOrderList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete a member");
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain(loggedInUser);
            this.Close();
            frmMain.Show();
        }
    }
}
