using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class operContratos : Form
    {
        private decimal totalGeral = 0;
        private DataTable Dados = null;
        private ContratosDAO contratosDAO;

        public operContratos()
        {
            InitializeComponent();

        }

        private void operContratos_Load(object sender, EventArgs e)
        {
            CarregarContratos();
        }

        private void CarregarContratos(string filtroDescricao = "", string filtroStatus = "Todos", DateTime? inicio = null, DateTime? fim = null)
        {
            contratosDAO = new ContratosDAO();
            DataTable contratos = contratosDAO.GetAllContratos();

            // Filtro por descri��o
            if (!string.IsNullOrWhiteSpace(filtroDescricao))
            {
                contratos = contratos.Select($"Descricao LIKE '%{filtroDescricao}%'").CopyToDataTable();
            }

            // Filtro por status
            if (filtroStatus != "Todos")
            {
                contratos = contratos.Select($"Status = '{filtroStatus}'").CopyToDataTable();
            }

            // Filtro por per�odo
            if (inicio.HasValue && fim.HasValue)
            {
                contratos = contratos.Select($"DataInicio >= #{inicio.Value:yyyy-MM-dd}# AND DataTermino <= #{fim.Value:yyyy-MM-dd}#").CopyToDataTable();
            }

            dgvContratos.DataSource = contratos;
            dgvContratos.Columns[0].Visible = false;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            operEdContratos foperSQL = new operEdContratos(0);
            foperSQL.Show();
        }

        //private void btnEditar_Click(object sender, EventArgs e)
        //{
        //    int idContratoSelecionado = Convert.ToInt32(dgvContratos.SelectedRows[0].Cells["ID"].Value);
        //    operEdContratos foperSQL = new operEdContratos(idContratoSelecionado);            
        //    foperSQL.Show();
        //}

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Obt�m o ID do contrato selecionado
            int idContratoSelecionado = Convert.ToInt32(dgvContratos.SelectedRows[0].Cells["ID"].Value);

            // Abre a tela de edi��o
            using (var foperSQL = new operEdContratos(idContratoSelecionado))
            {
                // Exibe a tela de edi��o como modal
                var result = foperSQL.ShowDialog();

                // Ap�s fechar a tela, verifica se houve uma altera��o
                if (result == DialogResult.OK)
                {
                    // Recarrega os dados da DataGridView
                    AtualizarListaContratos();

                    // Restaura a sele��o para o contrato editado
                    foreach (DataGridViewRow row in dgvContratos.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["ID"].Value) == idContratoSelecionado)
                        {
                            row.Selected = true;
                            dgvContratos.FirstDisplayedScrollingRowIndex = row.Index; // Garante que a linha fique vis�vel
                            break;
                        }
                    }
                }
            }
        }

        private void AtualizarListaContratos()
        {
            // Obt�m todos os contratos do banco de dados
            DataTable contratos = contratosDAO.GetAllContratos();

            // Atualiza a DataGridView com os dados
            dgvContratos.DataSource = contratos;

            // Configura��es adicionais, se necess�rio
            dgvContratos.Refresh();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            int idContratoSelecionado = Convert.ToInt32(dgvContratos.SelectedRows[0].Cells["ID"].Value);
            tb.Contrato contrato = contratosDAO.GetContratoCompleto(idContratoSelecionado);

            if (contrato != null)
            {
                rel.ContratoPrinter printer = new rel.ContratoPrinter(
                    contrato.Contratante,
                    contrato.ContratanteCNPJ,
                    contrato.ContratanteEndereco,
                    contrato.Contratada,
                    contrato.ContratadaCNPJ,
                    contrato.ContratadaEndereco,
                    contrato.Clausulas.ToArray() // Converte List<string> para string[]
                );

                printer.Imprimir();
            }
            else
            {
                MessageBox.Show("Contrato n�o encontrado para impress�o.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }

}

