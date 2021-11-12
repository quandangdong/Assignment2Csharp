using BusinessObject;
using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using DataAccess.Repository;


namespace SalesWinApp
{
    public partial class frmLogin : Form
    {
        MemberObject adminMember = Program.Configuration.GetSection("AdminAccount").Get<MemberObject>();
        IMemberRepository repo = new MemberRepository();
        
        public frmLogin()
        {
            InitializeComponent();
        }

        public MemberObject checkUser()
        {
            string userEmail = txtEmail.Text;
            string password = txtPassword.Text;

            if (userEmail == adminMember.Email && password == adminMember.Password)
            {
                return adminMember;
            }
            else
            {
                return repo.GetMemberByEmailAndPassword(userEmail, password);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MemberObject user = checkUser();
            string userEmail = txtEmail.Text;
            string password = txtPassword.Text;
            if (userEmail == "" || password == "")
            {
                DialogResult dialogResult = MessageBox.Show("Please enter all email and password",
                    "Fstore error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if(user == null)
            {
                DialogResult dialogResult = MessageBox.Show("User not exist!", "Log In error" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if (user != null)
            {
                frmMain frmMain = new frmMain (user)
                {
                    Text = "Main Management",
                };
                this.Hide();
                frmMain.ShowDialog();
            } else
            {
                MessageBox.Show("Fail to login");
            }


        }
    }
}
