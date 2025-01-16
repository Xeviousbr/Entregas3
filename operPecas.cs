using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class operPecas : Form
    {

        private PecasDAO pecasDao;

        public operPecas()
        {
            InitializeComponent();
            pecasDao = new PecasDAO();
        }

        private void btnAddCarro_Click(object sender, EventArgs e)
        {
            string novoCarro = txtCarro.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novoCarro))
            {
                // Inserir o carro no banco de dados
                pecasDao.InsertCarro(novoCarro);

                // Atualizar a lista de carros
                AtualizarListaCarros();
                txtCarro.Clear();
            }
        }

        private void btnAddPeca_Click(object sender, EventArgs e)
        {
            string novaPeca = txtPeca.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novaPeca) && lstCarros.SelectedItem != null)
            {
                // Obter o ID do carro selecionado
                DataRowView carroSelecionado = lstCarros.SelectedItem as DataRowView;
                int idCarro = Convert.ToInt32(carroSelecionado["IdCarro"]);

                // Inserir a pe�a no banco de dados
                pecasDao.InsertPeca(novaPeca, idCarro);

                // Atualizar a lista de pe�as
                AtualizarListaPecas(idCarro);
                txtPeca.Clear();
            }
        }

        private void btnAddCaracteristica_Click(object sender, EventArgs e)
        {
            string novaCaracteristica = txtCaracteristica.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novaCaracteristica) && lstPecas.SelectedItem != null)
            {
                // Obter o ID da pe�a selecionada
                DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);

                // Inserir a caracter�stica no banco de dados
                pecasDao.InsertCaracteristica(novaCaracteristica, idPeca);

                // Atualizar a lista de caracter�sticas
                AtualizarListaCaracteristicas(idPeca);
                txtCaracteristica.Clear();
            }
        }

        private void AtualizarListaCarros()
        {
            DataTable carros = pecasDao.GetAllCarros();
            if (carros.Rows.Count > 0)
            {
                lstCarros.DataSource = carros;
                lstCarros.DisplayMember = "Nome";
                lstCarros.ValueMember = "IdCarro";
            }
            else
            {
                lstCarros.DataSource = null; // Caso n�o haja carros
            }
        }


        private void AtualizarListaPecas(int idCarro)
        {
            DataTable pecas = pecasDao.GetPecasByCarroId(idCarro);
            if (pecas.Rows.Count > 0)
            {
                lstPecas.DataSource = pecas;
                lstPecas.DisplayMember = "Nome";
                lstPecas.ValueMember = "IdPeca";
            }
            else
            {
                lstPecas.DataSource = null; // Caso n�o haja pe�as
            }
        }


        private void AtualizarListaCaracteristicas(int idPeca)
        {
            DataTable caracteristicas = pecasDao.GetCaracteristicasByPecaId(idPeca);
            if (caracteristicas.Rows.Count > 0)
            {
                lstCaracteristicas.DataSource = caracteristicas;
                lstCaracteristicas.DisplayMember = "Descricao";
                lstCaracteristicas.ValueMember = "IdCaracteristica";
            }
            else
            {
                lstCaracteristicas.DataSource = null; // Caso n�o haja caracter�sticas
            }
        }


        private void operPecas_Load(object sender, EventArgs e)
        {
            AtualizarListaCarros();

            // Caso haja carros, atualiza as pe�as do primeiro carro
            if (lstCarros.Items.Count > 0)
            {
                lstCarros.SelectedIndex = 0; // Seleciona o primeiro carro
                DataRowView carroSelecionado = lstCarros.SelectedItem as DataRowView;
                int idCarro = Convert.ToInt32(carroSelecionado["IdCarro"]);
                AtualizarListaPecas(idCarro);

                // Caso haja pe�as, atualiza as caracter�sticas da primeira pe�a
                if (lstPecas.Items.Count > 0)
                {
                    lstPecas.SelectedIndex = 0; // Seleciona a primeira pe�a
                    DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                    int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);
                    AtualizarListaCaracteristicas(idPeca);
                }
                else
                {
                    lstCaracteristicas.DataSource = null; // Limpa as caracter�sticas
                }
            }
            else
            {
                lstPecas.DataSource = null; // Limpa as pe�as
                lstCaracteristicas.DataSource = null; // Limpa as caracter�sticas
            }
        }

        private void lstCarros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCarros.SelectedItem != null)
            {
                DataRowView carroSelecionado = lstCarros.SelectedItem as DataRowView;
                int idCarro = Convert.ToInt32(carroSelecionado["IdCarro"]);
                AtualizarListaPecas(idCarro);

                // Atualizar caracter�sticas da primeira pe�a do carro selecionado
                if (lstPecas.Items.Count > 0)
                {
                    lstPecas.SelectedIndex = 0;
                    DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                    int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);
                    AtualizarListaCaracteristicas(idPeca);
                }
                else
                {
                    lstCaracteristicas.DataSource = null;
                }
            }
        }

        private void lstPecas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPecas.SelectedItem != null)
            {
                DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);
                AtualizarListaCaracteristicas(idPeca);
            }
        }


    }
}

