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
    public partial class FrmFornecedor : Form
    {
        Fornecedor model = new Fornecedor();
        public FrmFornecedor()
        {
            InitializeComponent();
        }

        //procurar o index da linha selecionada e retornar o Id do fornecedor ou null 
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

        //preencher a data grid view com dados de fornecedores
        void PopularDataView()
        {
            using(CRUDProdEntities db = new CRUDProdEntities())
            {
                dataGridView1.DataSource = db.Fornecedor.ToList<Fornecedor>();
            }
        }

        //limpar campos e zerar objeto
        void Clear()
        {
            
            textBoxNome.Text = textBoxCNPJ.Text = textBoxEndereco.Text = "";
            checkBoxAtivo.Checked = false;
            model.IdFornecedor = 0;
        }
        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void FrmFornecedor_Load(object sender, EventArgs e)
        {
            Clear();
            PopularDataView();
            dataGridView1.Columns[0].Visible = false;
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            model.Nome_Fornecedor = textBoxNome.Text.Trim();
            model.CNPJ = textBoxCNPJ.Text.Trim();
            model.Endereco = textBoxEndereco.Text.Trim();
            model.Ativo = checkBoxAtivo.Checked;
            using (CRUDProdEntities db = new CRUDProdEntities())
            {
                if (model.IdFornecedor == 0)
                {
                    db.Fornecedor.Add(model);
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.CurrentRow.Index != -1)
            {
                model.IdFornecedor = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdFornecedor"].Value);
                using (CRUDProdEntities db = new CRUDProdEntities())
                {
                    model = db.Fornecedor.Where(x => x.IdFornecedor == model.IdFornecedor).FirstOrDefault();
                    textBoxNome.Text = model.Nome_Fornecedor;
                    textBoxCNPJ.Text = model.CNPJ;
                    textBoxEndereco.Text = model.Endereco;
                    checkBoxAtivo.Checked = model.Ativo;
                    tabControl1.SelectedIndex = 1;
                }
            }
        }

        private void buttonNovo_Click(object sender, EventArgs e)
        {
            Clear();
            tabControl1.SelectedIndex = 1;
        }

        private void buttonExcluir_Click(object sender, EventArgs e)
        {
            int? id = GetId();
            if (id != null)
            {
                using (CRUDProdEntities db = new CRUDProdEntities())
                {
                    Fornecedor model = db.Fornecedor.Find(id);
                    db.Fornecedor.Remove(model);
                    db.SaveChanges();
                }
                Clear();
                PopularDataView();
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                using (CRUDProdEntities db = new CRUDProdEntities())
                {
                    if (textBoxPesquisa.Text != string.Empty) {
                        var items = db.Fornecedor.Where(s => s.Nome_Fornecedor.Contains(textBoxPesquisa.Text));
                        dataGridView1.DataSource = items.ToList();
                    }
                    else
                    {
                        dataGridView1.DataSource = db.Fornecedor.ToList();
                    }
                }
            }
        }
    }
}
