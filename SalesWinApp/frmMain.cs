using BusinessObject;
using MyStoreWinApp;
using System;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmMain : Form
    {
        private MemberObject loggedInUser { get; set; }
        public frmMain(MemberObject user)
        {
            InitializeComponent();
            loggedInUser = user;
        }

        private void btnMember_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMemberManagement frmMemberManagement = new frmMemberManagement(loggedInUser);
            //frmMemberManagement.FormClosed += (sender, e)=> this.Close();
            frmMemberManagement.Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmProductManagement frmProductManagement = new frmProductManagement(loggedInUser);
            //frmProductManagement.FormClosed += (sender, e) => this.Close();
            frmProductManagement.Show();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmOrderManagement frmOrderManagement = new frmOrderManagement(loggedInUser);
            //frmOrderManagement.FormClosed += (sender, e) => this.Close();
            frmOrderManagement.Show();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
