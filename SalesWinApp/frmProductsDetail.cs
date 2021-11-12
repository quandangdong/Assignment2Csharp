using BusinessObject;
using DataAccess.Repository;
using System;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmProductsDetail : Form
    {
        public frmProductsDetail()
        {
            InitializeComponent();
        }
        public bool InsertOrUpdate { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ProductObject ProductInfo { get; set; }



        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ProductObject product = new ProductObject
                {
                    ProductId = int.Parse(txtInfoProductId.Text),
                    CategoryId = int.Parse(txtInfoCategoryId.Text),
                    ProductName = txtInfoProductName.Text,
                    Weight = txtInfoWeight.Text,
                    UnitPrice = decimal.Parse(txtInfoUnitPrice.Text),
                    UnitInStock = int.Parse(txtInfoUnitInStock.Text),
                };
                if(InsertOrUpdate == false)
                {
                    ProductRepository.InsertProduct(product);
                }
                else
                {
                    ProductRepository.UpdateProduct(product);
                }
                Close();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false? "Insert Product" : "Update Product");
            }

        }

        private void frmProductsDetail_Load(object sender, EventArgs e)
        {
            txtInfoProductId.Enabled = !InsertOrUpdate;
            if(ProductInfo == null)
            {
                return;
            }
            if (InsertOrUpdate == true)
            {
                txtInfoProductId.Text = ProductInfo.ProductId.ToString();
                txtInfoCategoryId.Text = ProductInfo.CategoryId.ToString();
                txtInfoProductName.Text = ProductInfo.ProductName;
                txtInfoWeight.Text = ProductInfo.Weight;
                txtInfoUnitPrice.Text = ProductInfo.UnitPrice.ToString();
                txtInfoUnitInStock.Text = ProductInfo.UnitInStock.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}
