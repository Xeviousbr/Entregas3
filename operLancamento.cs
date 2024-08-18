﻿using TeleBonifacio.dao;
using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.tb;
using System.Globalization;

namespace TeleBonifacio
{
    public partial class operLancamento : Form
    {
        private EntregasDAO entregasDAO;
        private int iID = 0;
        private string UID = "";
        private bool carregou = false;

        public operLancamento()
        {
            InitializeComponent();
        }

        private void operLancamento_Load(object sender, EventArgs e)
        {
            EntregadorDAO Entregador = new EntregadorDAO();
            ClienteDAO Cliente = new ClienteDAO();
            VendedoresDAO Vendedor = new VendedoresDAO();
            glo.CarregarComboBox<Entregador>(cmbMotoBoy, Entregador);
            glo.CarregarComboBox<Cliente>(cmbCliente, Cliente);
            glo.CarregarComboBox<Vendedor>(cmbVendedor, Vendedor,"", " Where Vendedores.Atende = 1 ", " desc ");
            cmbMotoBoy.SelectedIndex = 0;
            cmbCliente.SelectedIndex = -1;
            rt.AdjustFormComponents(this);
        }

        #region MetodosPrincipais

        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Width = 0;
            dataGrid1.Columns[1].Width = 75;
            dataGrid1.Columns[2].Width = 110;
            dataGrid1.Columns[3].Width = 70;    // Valor
            dataGrid1.Columns[4].Width = 70;    // Desconto
            dataGrid1.Columns[5].Width = 80; // 70;    // Compra
            dataGrid1.Columns[6].Width = 60;    
            dataGrid1.Columns[7].Width = 170;   // Cliente
            dataGrid1.Columns[8].Width = 170;   // Vendedor
            dataGrid1.Columns[9].Width = 90;    // Obs
            dataGrid1.Columns[10].Width = 0;
            dataGrid1.Columns[11].Width = 0;
            dataGrid1.Columns[12].Width = 0;
            dataGrid1.Columns[13].Width = 0;
            dataGrid1.Columns[14].Width = 0;
            if (rt.IsLargeScreen())
            {
                for (int i = 0; i < 9; i++)
                {
                    dataGrid1.Columns[i].Width = (int)(dataGrid1.Columns[i].Width * rt.scaleFactor);
                }
            }
            dataGrid1.Invalidate();
        }

        private void Limpar()
        {
            cmbMotoBoy.SelectedIndex = 0;
            cmbCliente.SelectedIndex = 0;
            cmbFormaPagamento.SelectedIndex = -1;
            cmbVendedor.SelectedIndex = 0;
            txtValor.Text = "";
            txCompra.Text = "";
            txDesc.Text = "";
            lbTotal.Text = "";
        }

        private void CarregaGrid()
        {
            entregasDAO = new EntregasDAO();
            DateTime DT2 = dtpData.Value.AddDays(-1);
            DataTable dados = entregasDAO.getDados(DT2, dtpData.Value);
            if (dados.Rows.Count > 0)
            {
                decimal totalValor = 0;
                decimal totalCompra = 0;

                foreach (DataRow row in dados.Rows)
                {
                    if (decimal.TryParse(row["Valor"].ToString(), out decimal valor))
                    {
                        totalValor += valor;
                    }

                    if (decimal.TryParse(row["Compra"].ToString(), out decimal compra))
                    {
                        totalCompra += compra;
                    }
                }

                // Adiciona a linha de total ao DataTable
                DataRow totalRow = dados.NewRow();
                totalRow["MotoBoy"] = "Total"; // Você pode personalizar essa célula para indicar que é uma linha de total
                totalRow["Valor"] = totalValor.ToString("C"); // Formata como moeda
                totalRow["Compra"] = totalCompra.ToString("C"); // Formata como moeda
                dados.Rows.Add(totalRow);


            }

            DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(dados.DefaultView);
            dataGrid1.DataSource = boundDataView;

        }


        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            int idBoy = Convert.ToInt32(cmbMotoBoy.SelectedValue);
            int idForma = Convert.ToInt32(cmbFormaPagamento.SelectedIndex);
            int idCliente = Convert.ToInt32(cmbCliente.SelectedValue);
            int idVend = Convert.ToInt32(cmbVendedor.SelectedValue);
            float valor;
            if (!float.TryParse(txtValor.Text, out valor))
            {
                valor = 0;
            }
            float compra;
            if (!float.TryParse(txCompra.Text, out compra))
            {
                compra = 0;
            }
            string obs = txObs.Text;
            float desc;
            if (!float.TryParse(txDesc.Text, out desc))
            {
                desc = 0;
            }
            if (btnAdicionar.Text == "Salvar")
            {
                glo.Loga($@"EE,{this.iID}, {idBoy}, {idForma}, {valor}, {idCliente}, {compra}, {obs}, {desc}, {idVend}, {this.UID}");
                entregasDAO.Edita(this.iID, idBoy, idForma, valor, idCliente, compra, obs, desc, idVend);
                btnAdicionar.Text = "Adicionar";
            }
            else
            {
                string UID = glo.GenerateUID();
                glo.Loga($@"EA,{idForma},{compra},{idCliente}, {obs}, {desc}, {idVend}, {UID}");
                entregasDAO.Adiciona(idBoy, idForma, valor, idCliente, compra, obs, desc, idVend, UID);
            }
            CarregaGrid();
            Limpar();
        }

        private void MostraTotal()
        {
            float valor = glo.LeValor(txtValor.Text);
            float compra = glo.LeValor(txCompra.Text);
            float desc = glo.LeValor(txDesc.Text);
            float total = valor + compra - desc;
            if (total > 0)
            {
                lbTotal.Text = total.ToString("C");
            }
            else
            {
                lbTotal.Text = "";
            }
        }

        private void dataGrid1_Click(object sender, EventArgs e)
        {
            SourceGrid.DataGrid grid = (SourceGrid.DataGrid)sender;
            if (grid != null && grid.Rows.Count > 0)
            {
                SourceGrid.Position position = grid.Selection.ActivePosition;
                if (position != SourceGrid.Position.Empty)
                {
                    this.iID = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["Id"]);
                    txtValor.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Valor"]);
                    txDesc.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Desconto"]);
                    txCompra.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Compra"]);
                    txObs.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Obs"]);
                    cmbMotoBoy.SelectedValue = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["idBoy"]);
                    cmbCliente.SelectedValue = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["NrCli"]);
                    cmbVendedor.SelectedValue = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["idVend"]);
                    cmbFormaPagamento.SelectedIndex = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["idForma"]);
                    this.UID = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["UID"]);
                    btnAdicionar.Text = "Salvar";
                    MostraTotal();
                }
            }
        }

        #endregion

        #region Criticas
        private void VeSeHab()
        {
            bool OK = true;
            if (cmbFormaPagamento.SelectedIndex == -1)
            {
                OK = false;
            } else
            {
                if (cmbMotoBoy.SelectedIndex == -1)
                {
                    if (txCompra.Text == "")
                    {
                        OK = false;
                    }
                }
                else
                {
                    if (txtValor.Text == "")
                    {
                        OK = false;
                    }
                }
            }
            btnAdicionar.Enabled = OK;
        }

        #endregion

        #region Eventos

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                TextBox textBox = sender as TextBox;
                string S = textBox.Text;
                if ((e.KeyChar == ',' || e.KeyChar == '.') && !S.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                {
                    if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "," && e.KeyChar == '.')
                    {
                        e.KeyChar = ','; 
                    }
                    else if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "." && e.KeyChar == ',')
                    {
                        e.KeyChar = '.'; 
                    }
                }
                else
                {
                    e.Handled = true; 
                }
            }
        }

        private void cmbMotoBoy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string searchText = cmbMotoBoy.Text.Trim();
                cmbMotoBoy.SelectedValue = int.Parse(searchText);
            }
        }

        private void cmbCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                dao.ClienteDAO cCli = new dao.ClienteDAO();
                string searchText = cmbCliente.Text.Trim();
                int idCli = cCli.RetIdNrAlter(searchText);
                if (idCli>0)
                {
                    cmbCliente.SelectedValue = idCli;
                }                
            }
        }

        private void txtValor_KeyUp(object sender, KeyEventArgs e)
        {
            MostraTotal();
            VeSeHab();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void cmbFormaPagamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            VeSeHab();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            //DateTime DT = dtpData.Value;
            CarregaGrid();
        }

        private void operLancamento_Resize(object sender, EventArgs e)
        {
            dataGrid1.Width = this.Width;
        }

        private void btnNovoCliente_Click(object sender, EventArgs e)
        {
            glo.IdAdicionado = -1;
            fCadClientes Cad = new fCadClientes();            
            Cad.ShowDialog();
            if (glo.IdAdicionado > 0)
            {
                ClienteDAO Cliente = new ClienteDAO();
                glo.CarregarComboBox<tb.Cliente>(cmbCliente, Cliente);
                cmbCliente.SelectedValue = glo.IdAdicionado;
            }
        }

        #endregion

        private void txtValor_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox?.SelectAll();
        }

        private void btRelat_Click(object sender, EventArgs e)
        {
            rel.RelEntegas fRel = new rel.RelEntegas();
            fRel.Data = dtpData.Value;
            fRel.Show();
        }

        private void operLancamento_Activated(object sender, EventArgs e)
        {
            if (!carregou)
            {
                carregou = true;
                dtpData.Value = DateTime.Now;
                CarregaGrid();
                ConfigurarGrid();
            }            
        }
    }
}

