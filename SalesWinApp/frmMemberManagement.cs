using BusinessObject;
using DataAccess.Repository;
using SalesWinApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyStoreWinApp
{
    public partial class frmMemberManagement : Form
    {

        private MemberObject loggedInUser { get; set; }
        public frmMemberManagement(MemberObject user)
        {
            InitializeComponent();
            loggedInUser = user;

        }

        //----------------------------------
        IMemberRepository MemberRepository = new MemberRepository();

        //create data source
        BindingSource source;

        //----------------------------------


        private MemberObject GetMemberObject()
        {
            MemberObject member = null;
            try
            {
                if (loggedInUser.Email != "admin@fstore.com")
                {
                    int txtMemberIDValue = int.Parse(txtMemberID.Text);
                    if (loggedInUser.MemberId == txtMemberIDValue)
                    {
                        member = new MemberObject
                        {
                            MemberId = int.Parse(txtMemberID.Text),
                            CompanyName = txtCompanyName.Text,
                            City = txtMemberCity.Text,
                            Country = txtMemberCountry.Text,
                            Email = txtMemberEmail.Text,
                            Password = txtPassword.Text,
                        };
                    }
                    else
                    {
                        MessageBox.Show("You can only update your own account!");
                    }

                }
                else
                {
                    member = new MemberObject
                    {
                        MemberId = int.Parse(txtMemberID.Text),
                        CompanyName = txtCompanyName.Text,
                        City = txtMemberCity.Text,
                        Country = txtMemberCountry.Text,
                        Email = txtMemberEmail.Text,
                        Password = txtPassword.Text,
                    };
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Member");
            }
            return member;
        }

        //---------------------------------------------------------

        private void ClearText()
        {
            txtMemberID.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtMemberEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtMemberCity.Text = string.Empty;
            txtMemberCountry.Text = string.Empty;
        }

        //---------------------------------------------------------

        public void LoadMemberList()
        {
            IEnumerable<MemberObject> mems = null;
            mems = MemberRepository.GetMemberList();

            try
            {
                source = new BindingSource();
                source.DataSource = mems;

                txtMemberID.DataBindings.Clear();
                txtCompanyName.DataBindings.Clear();
                txtMemberEmail.DataBindings.Clear();
                txtPassword.DataBindings.Clear();
                txtMemberCity.DataBindings.Clear();
                txtMemberCountry.DataBindings.Clear();

                txtMemberID.DataBindings.Add("Text", source, "MemberId");
                txtCompanyName.DataBindings.Add("Text", source, "CompanyName");
                txtMemberEmail.DataBindings.Add("Text", source, "Email");
                txtPassword.DataBindings.Add("Text", source, "Password");
                txtMemberCity.DataBindings.Add("Text", source, "City");
                txtMemberCountry.DataBindings.Add("Text", source, "Country");

                MemberDataTable.DataSource = null;
                MemberDataTable.DataSource = source;
                if (mems.Count() == 0)
                {
                    ClearText();
                    btnDelete.Enabled = false;
                }
                if (loggedInUser.Email == "admin@fstore.com")
                {
                    btnDelete.Enabled = true;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Member List");
            }
        }


        //-----------------------------------------------------
        private void frmMemberManagement_Load(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            MemberDataTable.CellDoubleClick += MemberDataTable_CellContentClick;
        }

        private void MemberDataTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FrmMemberDetail frmMemberDetail = new FrmMemberDetail
            {
                Text = "Update Member",
                InsertOrUpdate = true,
                MemberInfo = GetMemberObject(),
                MemberRepository = MemberRepository
            };
            if(frmMemberDetail.ShowDialog() == DialogResult.OK)
            {
                LoadMemberList();
                source.Position = source.Count - 1;
                MemberDataTable.AllowUserToAddRows = false;
            }
        }

        //---------------------------------------------------------

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadMemberList();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (loggedInUser.Email == "admin@fstore.com")
            {
                FrmMemberDetail frmMemberDetail = new FrmMemberDetail
                {
                    Text = "Add Member",
                    InsertOrUpdate = false,
                    MemberRepository = MemberRepository,
                };
                if (frmMemberDetail.ShowDialog() == DialogResult.OK)
                {
                    LoadMemberList();
                    source.Position = source.Count - 1;
                    MemberDataTable.AllowUserToAddRows = false;
                }
            }
            else
            {
                MessageBox.Show("You must be admin to create new Nember");
            }
        }

        //---------------------------------------------------------

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var member = GetMemberObject();
                MemberRepository.DeleteMemberById(member.MemberId);
                LoadMemberList();
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
