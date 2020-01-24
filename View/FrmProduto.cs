using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProdutoFornecedor.Models;

namespace ProdutoFornecedor.View
{
    public partial class FrmProduto : Form
    {
        Produto model = new Produto();
        public FrmProduto()
        {
            InitializeComponent();
        }

        //procurar o index da linha selecionada e retornar o Id do produto ou null 
        private int? GetId()
        {
            try
            {
                return int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            }
            catch
            {
                return null;
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            Clear();
        }

        void Clear() {
            textBoxNome.Text = numericUpDown1.Text = "";
            comboBoxFornecedor.SelectedItem = 0;
            buttonCancelar.Enabled = true;
            model.IdProduto = 0;
        }

        private void FrmProduto_Load(object sender, EventArgs e)
        {
            Clear();
            PopularDataView();
            dataGridView1.Columns[0].Visible = false;
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            model.NomeProduto = textBoxNome.Text.Trim();
            model.Nome_Fornecedor = comboBoxFornecedor.GetItemText(this.comboBoxFornecedor.SelectedItem);
            model.Quantidade = int.Parse(numericUpDown1.Text);
            using (CRUDProdEntities db = new CRUDProdEntities())
            {
                if (model.IdProduto == 0) { 
                    db.Produto.Add(model);
                }
                else
                {
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            Clear();
            tabControl1.SelectedIndex = 0;
            PopularDataView();

        }

        //preencher a data grid view com dados de produtos
        void PopularDataView()
        {
            using(CRUDProdEntities db = new CRUDProdEntities())
            {
                dataGridView1.DataSource = db.Produto.ToList<Produto>();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.CurrentRow.Index != -1)
            {
                model.IdProduto = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdProduto"].Value);
                using (CRUDProdEntities db = new CRUDProdEntities())
                {
                    model = db.Produto.Where(x => x.IdProduto == model.IdProduto).FirstOrDefault();
                    textBoxNome.Text = model.NomeProduto;
                    numericUpDown1.Text = Convert.ToString(model.Quantidade);
                    comboBoxFornecedor.SelectedItem = model.Nome_Fornecedor;
                    tabControl1.SelectedIndex = 1;
                }
            }
        }

        private void buttonNovo_Click(object sender, EventArgs e)
        {
            Clear();
            tabControl1.SelectedIndex = 1;
        }

        //preencher o combo box com os fornecedores da tabela Fornecedor
        private void comboBoxFornecedor_MouseClick(object sender, MouseEventArgs e)
        {
            using (CRUDProdEntities f = new CRUDProdEntities())
            {
                comboBoxFornecedor.DataSource = f.Fornecedor.Where(c => c.Ativo == true).ToList();
                comboBoxFornecedor.ValueMember = "IdFornecedor";
                comboBoxFornecedor.DisplayMember = "Nome_Fornecedor";
            }
        }

        private void textBoxPesquisar_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)13)
            {
                using (CRUDProdEntities db = new CRUDProdEntities())
                {
                    if (textBoxPesquisa.Text != string.Empty)
                    {
                        var items = db.Produto.Where(s => s.NomeProduto.Contains(textBoxPesquisa.Text));
                        dataGridView1.DataSource = items.ToList();
                    }
                    else
                    {
                        dataGridView1.DataSource = db.Produto.ToList();
                    }
                }
            }
        }

        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (CRUDProdEntities db = new CRUDProdEntities())
                {
                    Produto model = db.Produto.Find(id);
                    db.Produto.Remove(model);
                    db.SaveChanges();
                }
                Clear();
                PopularDataView();
            }
        }
    }
}
