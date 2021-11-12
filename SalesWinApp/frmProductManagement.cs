using BusinessObject;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmProductManagement : Form
    {
        private MemberObject loggedInUser { get; set; }


        public frmProductManagement(MemberObject user)
        {
            InitializeComponent();
            loggedInUser = user;
        }

        //---------------------------------------------------------
        IProductRepository ProductRepository = new ProductRepository();

        //Create data source
        BindingSource source;
        //---------------------------------------------------------

        private ProductObject GetProductObject()
        {
            ProductObject product = null;
            try
            {
                if (loggedInUser.Email == "admin@fstore.com")
                {
                     product = new ProductObject
                        {
                            ProductId = int.Parse(txtProductID.Text),
                            CategoryId = int.Parse(txtCategoryId.Text),
                            ProductName = txtProductName.Text,
                            Weight = txtWeight.Text,
                            UnitPrice = decimal.Parse(txtUnitPrice.Text),
                            UnitInStock = int.Parse(txtUnitInStock.Text),
                        };
                }
                else
                {
                    MessageBox.Show("You must be an admin to update product !");
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Product");

            }
            return product;
        }
        //---------------------------------------------------------

        public void ClearText()
        {
            txtProductID.Text = string.Empty;
            txtCategoryId.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtUnitInStock.Text = string.Empty;
        }

        public void LoadProductList(string action)
        {
            IEnumerable<ProductObject> products = null;

            if (action == "search")
            {
                if (txtSearchProductID.Text == "" || txtSearchProductName.Text == "" || txtSearchPrice.Text == "" || txtSearchUnitInStock.Text == "")
                {

                    MessageBox.Show("Please input all field for search product");
                    return;

                }
                else
                {
                    int productId = int.Parse(txtSearchProductID.Text);
                    string productName = txtSearchProductName.Text;
                    decimal unitPrice = decimal.Parse(txtSearchPrice.Text);
                    int unitsInStock = int.Parse(txtSearchUnitInStock.Text);

                    products = ProductRepository.SearchProduct(productId, productName, unitPrice, unitsInStock);
                }
            }
            else if (action == "all")
            {
                products = ProductRepository.GetProductList();
            }

            try
            {
                source = new BindingSource();
                source.DataSource = products;

                txtProductID.DataBindings.Clear();
                txtCategoryId.DataBindings.Clear();
                txtProductName.DataBindings.Clear();
                txtWeight.DataBindings.Clear();
                txtUnitPrice.DataBindings.Clear();
                txtUnitInStock.DataBindings.Clear();

                txtProductID.DataBindings.Add("Text", source, "ProductId");
                txtCategoryId.DataBindings.Add("Text", source, "CategoryId");
                txtProductName.DataBindings.Add("Text", source, "ProductName");
                txtWeight.DataBindings.Add("Text", source, "Weight");
                txtUnitPrice.DataBindings.Add("Text", source, "UnitPrice");
                txtUnitInStock.DataBindings.Add("Text", source, "UnitInStock");

                dgvProductTable.DataSource = null;
                dgvProductTable.DataSource = source;

                if (products.Count() == 0)
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
                MessageBox.Show(ex.Message, "Load product list");
            }
        }

        //---------------------------------------------------------

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadProductList("all");
        }

        //---------------------------------------------------------

        private void frmProductManagement_Load(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;
            dgvProductTable.CellDoubleClick += dgvProductTable_CellContentClick;
        }

        //---------------------------------------------------------

        private void dgvProductTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GetProductObject() == null)
            {
                return;
            }
            frmProductsDetail frmProductsDetail = new frmProductsDetail
            {
                Text = "Update Product",
                InsertOrUpdate = true,
                ProductRepository = ProductRepository,
                ProductInfo = GetProductObject(),
            };
            if (frmProductsDetail.ShowDialog() == DialogResult.OK)
            {
                LoadProductList("all");
                source.Position = source.Count - 1;
                dgvProductTable.AllowUserToAddRows = false;
            }
        }

        //---------------------------------------------------------
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (loggedInUser.Email == "admin@fstore.com")
            {
                frmProductsDetail frmProductsDetail = new frmProductsDetail
                {
                    Text = "Add a product",
                    ProductRepository = ProductRepository,
                    InsertOrUpdate = false,
                };
                if (frmProductsDetail.ShowDialog() == DialogResult.OK)
                {
                    LoadProductList("all");
                    source.Position = source.Count - 1;
                    dgvProductTable.AllowUserToAddRows = false;
                }
            }
            else
            {
                MessageBox.Show("You must be admin to create new product");
            }
        }

        //---------------------------------------------------------

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var product = GetProductObject();
                ProductRepository.DeleteProductById(product.ProductId);
                LoadProductList("search");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete a product");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProductList("search");
        }

        private void back_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain(loggedInUser);
            this.Close();
            frmMain.Show();
        }

        //---------------------------------------------------------

    }

}
