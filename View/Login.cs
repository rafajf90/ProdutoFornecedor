using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProdutoFornecedor.Models;

namespace ProdutoFornecedor.View
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBoxUsuario.Text))
            {
                MessageBox.Show("Insira o nome de Usuário");
                textBoxUsuario.Focus();
                return;
            }
            try
            {
                CRUDProdEntities context = new CRUDProdEntities();
                {
                    var query = from o in context.Usuario
                                where o.NomeUsuario == textBoxUsuario.Text && o.SenhaUsuario == textBoxSenha.Text
                                select o;
                    if (query.SingleOrDefault() != null)
                    {
                        this.Hide();
                        Index i = new Index();
                        i.ShowDialog(); 
                        this.Close();


                    }
                    else
                    {
                        MessageBox.Show("Usuário e/ou Senha Inválidos");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message");
            }
        }

        private void buttonSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
