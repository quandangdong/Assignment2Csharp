using BusinessObject;
using DataAccess.Repository;
using System;
using System.Windows.Forms;

namespace MyStoreWinApp
{
    public partial class FrmMemberDetail : Form
    {
        public FrmMemberDetail()
        {
            InitializeComponent();
        }

        //----------------------------------------------------
        public IMemberRepository MemberRepository { get; set; }
        public bool InsertOrUpdate { get; set; }
        public MemberObject MemberInfo { get; set; }

        //----------------------------------------------------

        private void FrmMemberDetail_Load(object sender, EventArgs e)
        {
            txtMemberID.Enabled = !InsertOrUpdate;
            if(InsertOrUpdate == true)
            {
                txtMemberID.Text = MemberInfo.MemberId.ToString();
                txtCompanyName.Text = MemberInfo.CompanyName;
                txtEmail.Text = MemberInfo.Email;
                txtMemberPassword.Text = MemberInfo.Password;
                txtCity.Text = MemberInfo.City;
                txtCountry.Text = MemberInfo.Country;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var Member = new MemberObject
                {
                    MemberId = int.Parse(txtMemberID.Text),
                    CompanyName = txtCompanyName.Text,
                    Password = txtMemberPassword.Text,
                    Email = txtEmail.Text,
                    City = txtCity.Text,
                    Country = txtCountry.Text
                };
                Console.WriteLine("Form member detail: " + Member);
                if(InsertOrUpdate == false)
                {
                    MemberRepository.InsertMember(Member);
                }
                else
                {
                    MemberRepository.Update(Member);
                }
                Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add Member" : "Update a Member");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}
