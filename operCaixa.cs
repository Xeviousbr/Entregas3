﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.gen;

/*
Logs
    CA  Adição
    CE  Edição
    CD  Deleção
    CT  Alteração de data
*/

namespace TeleBonifacio
{
    public partial class operCaixa : Form
    {

        private CaixaDao Caixa;
        private FormasDAO cFormas;
        private int iID = 0;
        private string UID = "";
        private int DefCred=0;
        private int DefDeb = 0;
        private bool Especial=false;
        private bool mudando = true;
        private tb.ComboBoxItem IdDoVendNoCombo;
        private DateTime DT1;

        private void operCaixa_Load(object sender, EventArgs e)
        {
            if (glo.Nivel == 1)
            {
                btnFiltrar.Visible = false;
                button5.Visible = false;
            }
            else
            {
                if (glo.Nivel == 2)
                {
                    INI2 cINI2 = new INI2();
                    string suser = cINI2.ReadString("Usuario", "User", "0");
                    if (suser == "1")
                    {
                        this.Especial = true;
                        label1.Visible = true;
                        label2.Visible = true;
                        textBox1.Visible = true;
                    }
                }
            }
            Caixa = new CaixaDao();
            ClienteDAO Cliente = new ClienteDAO();
            VendedoresDAO Vendedor = new VendedoresDAO();
            glo.CarregarComboBox<tb.Cliente>(cmbCliente, Cliente, "NÃO IDENTIFICADO");
            glo.CarregarComboBox<tb.Vendedor>(cmbVendedor, Vendedor, "", " Where Vendedores.Atende = -1 or Vendedores.Atende = 1 ", " desc ");
            int idUsuarioLogado = glo.iUsuario;
            if (cmbVendedor.Items.Count > 0)
            {
                foreach (tb.ComboBoxItem item in cmbVendedor.Items)
                {
                    if (item.Id == idUsuarioLogado)
                    {
                        this.IdDoVendNoCombo = item;
                        cmbVendedor.SelectedItem = this.IdDoVendNoCombo;
                        break;
                    }
                }
            }
            cmbCliente.SelectedIndex = 0;
            DateTime ontem = DateTime.Today.AddDays(-1);
            dtpDataIN.Value = ontem;
            CarregaGrid();
            ConfigurarGrid();
            CarregaFormas();
            mudando = false;
        }        

        public operCaixa()
        {
            InitializeComponent();
            rt.AdjustFormComponents(this);
        }

        private void VeSeHab()
        {
            bool OK = !string.IsNullOrEmpty(txCompra.Text) || !string.IsNullOrEmpty(textBox1.Text);
            btnLimpar.Enabled = OK;

            if (this.Especial)
            {
                OK = !string.IsNullOrEmpty(txCompra.Text);
                foreach (Control control in grpCredito.Controls)
                {
                    if (control is Button button)
                    {
                        button.Enabled = OK;
                    }
                }
                foreach (Control control in grpDebito.Controls)
                {
                    if (control is Button button)
                    {
                        button.Enabled = !OK;
                    }
                }
            }
            else
            {
                foreach (Control control in grpCredito.Controls)
                {
                    if (control is Button button)
                    {
                        button.Enabled = OK;
                    }
                }
                foreach (Control control in grpDebito.Controls)
                {
                    if (control is Button button)
                    {
                        button.Enabled = OK;
                    }
                }
            }
        }

        private void MostraTotal()
        {
            float compra = glo.LeValor(txCompra.Text);
            float desc = glo.LeValor(txDesc.Text);
            float total = compra - desc;
            if (total > 0)
            {
                lbTotal.Text = total.ToString("C");
            }
            else
            {
                lbTotal.Text = "";
            }
        }

        private void CarregaFormas()
        {
            cFormas = new FormasDAO();
            CarregaForma(ref cFormas, 0, grpCredito);

            CarregaFormaDuasLinhas(ref cFormas, 1, grpDebito);

            glo.CarregarComboBox<tb.Forma>(cbFormas, cFormas," ");

            string query = "Select * From Config";            
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            DataRow row = dt.Rows[0];
            this.DefCred = Convert.ToInt32(row["DefCred"]);
            this.DefDeb = this.DefCred+1;
        }

        public void CarregaForma(ref FormasDAO cForma, int tipoForma, GroupBox targetGroupBox)
        {
            List<tb.Forma> lstFormas = cForma.getFormas(tipoForma);
            int buttonWidth = 75;
            int buttonHeight = 23;
            int margin = 15;
            int availableWidth = targetGroupBox.Width - (2 * margin);
            int totalButtons = lstFormas.Count;
            if (totalButtons > 0)
            {
                int spacing = (availableWidth - (buttonWidth * totalButtons)) / (totalButtons - 1);
                for (int i = 0; i < totalButtons; i++)
                {
                    Button btn = new Button();
                    btn.Width = buttonWidth;
                    btn.Height = buttonHeight;
                    btn.Text = lstFormas[i].Nome;
                    btn.Tag = lstFormas[i].Id;
                    btn.Enabled = false;
                    int xPosition = margin + (i * (buttonWidth + spacing));
                    btn.Location = new Point(xPosition, (targetGroupBox.Height - buttonHeight) / 2);
                    targetGroupBox.Controls.Add(btn);
                    btn.Click += new EventHandler(Button_Click);
                }
            }
        }

        public void CarregaFormaDuasLinhas(ref FormasDAO cForma, int tipoForma, GroupBox targetGroupBox)
        {
            List<tb.Forma> lstFormas = cForma.getFormas(tipoForma);
            int buttonWidth = 75;
            int buttonHeight = 23;
            int margin = 15;
            int availableWidth = targetGroupBox.Width - (2 * margin);
            int totalButtons = lstFormas.Count;

            if (totalButtons > 0)
            {
                int buttonsPerRow = (int)Math.Ceiling(totalButtons / 2.0); // Divide em 2 linhas
                int spacing = (availableWidth - (buttonWidth * buttonsPerRow)) / (buttonsPerRow - 1);
                int rowHeight = targetGroupBox.Height / 3; // Define altura das linhas

                for (int i = 0; i < totalButtons; i++)
                {
                    Button btn = new Button();
                    btn.Width = buttonWidth;
                    btn.Height = buttonHeight;
                    btn.Text = lstFormas[i].Nome;
                    btn.Tag = lstFormas[i].Id;
                    btn.Enabled = false;

                    int row = i / buttonsPerRow; // Define a linha (0 ou 1)
                    int col = i % buttonsPerRow; // Define a coluna

                    int xPosition = margin + (col * (buttonWidth + spacing));
                    int yPosition = margin + (row * rowHeight); // Alterna entre primeira e segunda linha

                    btn.Location = new Point(xPosition, yPosition);
                    targetGroupBox.Controls.Add(btn);
                    btn.Click += new EventHandler(Button_Click);
                }
            }
        }


        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                if (clickedButton.Tag != null && int.TryParse(clickedButton.Tag.ToString(), out int IdTag))
                {
                    IdTag--; // Ajusta o idForma

                    if (dataGrid1.SelectedRows.Count > 1)
                    {
                        // Se mais de uma linha está selecionada, atualiza a forma de pagamento em todos os registros selecionados
                        AtualizarFormaEmRegistrosSelecionados(IdTag);
                    }
                    else
                    {
                        // Se apenas uma linha está selecionada, chama o método normal de registro
                        Registra(IdTag);
                    }
                }
                else
                {
                    MessageBox.Show("Erro: Tag do botão inválida ou não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AtualizarFormaEmRegistrosSelecionados(int idForma)
        {
            foreach (DataGridViewRow row in dataGrid1.SelectedRows)
            {
                if (row.Cells["ID"].Value != null)
                {
                    int idRegistro = Convert.ToInt32(row.Cells["ID"].Value);

                    // Atualiza apenas o campo de forma de pagamento (idForma) no banco de dados
                    Caixa.AtualizaForma(idRegistro, idForma);
                }
            }

            // Recarrega a grid após a alteração
            CarregaGrid();
            Limpar();
        }

        private void txDesc_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab();
            MostraTotal();
        }

        private void Registra(int idForma, bool VendIdenf=true)
        {
            int idCliente = Convert.ToInt32(cmbCliente.SelectedValue);
            int idVend = 0;
            if (VendIdenf)
            {
                idVend = Convert.ToInt32(cmbVendedor.SelectedValue);
            }
            float compra;
            if (!float.TryParse(txCompra.Text, out compra))
            {
                if (!float.TryParse(textBox1.Text, out compra))
                {
                    compra = 0;
                }                    
            }
            string obs = txObs.Text;
            float desc;
            if (!float.TryParse(txDesc.Text, out desc))
            {
                desc = 0;
            }
            if (this.iID == 0)
            {
                string UID = glo.GenerateUID();
                glo.Loga($@"CA,{idForma},{compra},{idCliente}, {obs}, {desc}, {idVend}, {UID}");


                int novoId = Caixa.Adiciona(idForma, compra, idCliente, obs, desc, idVend, UID);

                AdicionarNovaLinhaNaGrid(novoId, idForma, compra, idCliente, obs, desc, idVend);

            }
            else
            {
                glo.Loga($@"CE,{this.iID},{idForma},{compra},{idCliente}, {obs}, {desc}, {idVend}, {this.UID}");
                Caixa.Edita(this.iID, idForma, compra, idCliente, obs, desc, idVend);

                AtualizarLinhaNaGrid(this.iID, idForma, compra, idCliente, obs, desc, idVend);
            }
            // CarregaGrid();
            Limpar();
        }

        private void AdicionarNovaLinhaNaGrid(int id, int idForma, float valor, int idCliente, string obs, float desconto, int idVend)
        {
            if (dataGrid1.DataSource is DataTable dt)
            {
                tb.Forma regF = (tb.Forma)cFormas.GetPeloID((idForma + 1).ToString());
                string formaPagamento = regF != null ? regF.Nome : "Desconhecido";

                DataRow novaLinha = dt.NewRow();
                novaLinha["ID"] = id;
                novaLinha["Cliente"] = cmbCliente.Text;
                novaLinha["Valor"] = valor.ToString("F2");
                novaLinha["Desconto"] = desconto.ToString("F2");
                novaLinha["VlNota"] = (valor - desconto).ToString("F2");
                novaLinha["Vendedor"] = cmbVendedor.Text;
                novaLinha["idVend"] = idVend;  // 🔹 Armazena corretamente na grid
                novaLinha["Data"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                novaLinha["Pagamento"] = formaPagamento;
                novaLinha["Obs"] = obs;

                dt.Rows.InsertAt(novaLinha, 0);

                dataGrid1.DataSource = dt;

                // 🔹 Log para verificar se o ID foi salvo corretamente na grid
                Console.WriteLine($"DEBUG: Nova linha adicionada na grid - idVend = {novaLinha["idVend"]}");
            }
        }


        private void AtualizarLinhaNaGrid(int id, int idForma, float valor, int idCliente, string obs, float desconto, int idVend)
        {
            tb.Forma regF = (tb.Forma)cFormas.GetPeloID((idForma + 1).ToString());
            string formaPagamento = regF != null ? regF.Nome : "Desconhecido"; // Obtém a descrição correta

            foreach (DataGridViewRow row in dataGrid1.Rows)
            {
                if (Convert.ToInt32(row.Cells["ID"].Value) == id) // 🔹 Usa o nome da coluna ao invés de índice fixo
                {
                    row.Cells["Cliente"].Value = cmbCliente.Text; // Nome do cliente atualizado
                    row.Cells["Valor"].Value = valor.ToString("F2"); // Valor atualizado
                    row.Cells["Desconto"].Value = desconto.ToString("F2"); // Desconto atualizado
                    row.Cells["VlNota"].Value = (valor - desconto).ToString("F2"); // Valor Nota atualizado
                    row.Cells["Vendedor"].Value = cmbVendedor.Text; // Nome do vendedor atualizado
                    row.Cells["Data"].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm"); // Data/Hora atualizada
                    row.Cells["Pagamento"].Value = formaPagamento; // 🔹 Forma de pagamento corrigida
                    row.Cells["Obs"].Value = obs; // Observação atualizada
                    break; // 🔹 Sai do loop assim que encontrar o ID correspondente
                }
            }
        }

        private void Limpar()
        {
            txDesc.Text = "";
            txObs.Text = "";
            txCompra.Text = "";
            lbTotal.Text = "";
            textBox1.Text = "";
            cmbCliente.SelectedIndex = 0;
            cmbVendedor.SelectedItem = this.IdDoVendNoCombo;
            this.iID = 0;
            BotoesNormais();
        }

        #region Botões

        private void BotoesNormais()
        {
            foreach (Control control in grpCredito.Controls)
            {
                if (control is Button button)
                {
                    button.Enabled = true;
                    button.BackColor = SystemColors.Control;
                }
            }
            foreach (Control control in grpDebito.Controls)
            {
                if (control is Button button)
                {
                    button.Enabled = true;
                    button.BackColor = SystemColors.Control;
                }
            }
            btExcluir.Visible = false;
            btEditar.Visible = false;
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

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (cbFormas.SelectedItem is tb.ComboBoxItem selectedForma)
            {
                int idForma = selectedForma.Id;
                CarregaGrid(idForma);
            }
            else
            {
                CarregaGrid();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                glo.Loga($@"CD,{this.iID}, {this.UID}");
                Caixa.Exclui(this.iID);
                CarregaGrid();
                Limpar();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rel.Caixa fRel = new rel.Caixa();
            fRel.DT1 = dtpDataIN.Value;
            fRel.DT2 = dtnDtFim.Value;
            int idForma = -1;
            if (cbFormas.SelectedItem is tb.ComboBoxItem selectedForma)
            {
                idForma = selectedForma.Id;
            }
            fRel.Forma = idForma;
            fRel.txtForma = cbFormas.Text;
            fRel.Show();
        }

        private void btEditar_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Digite a data (DD/MM/AAAA):", "Digite a Data");
            if (input != "")
            {
                DateTime data;
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
                {
                    string Lista = "";
                    foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                    {
                        int id;
                        if (int.TryParse(row.Cells[0].Value.ToString(), out id))
                        {
                            Lista += id.ToString() + ",";
                        }
                    }
                    Lista = Lista.Remove(Lista.Length - 1);
                    glo.Loga($@"CT,{data},{Lista}");
                    Caixa.MudaData(data, Lista);
                    CarregaGrid();
                    Limpar();
                }
                else
                {
                    MessageBox.Show("Data inválida. Por favor, digite uma data no formato DD/MM/AAAA.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Grid

        private void dataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];

                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                int nrCli = selectedRow.Cells["NrCli"].Value != DBNull.Value ? Convert.ToInt32(selectedRow.Cells["NrCli"].Value) : 0;
                decimal valor = Convert.ToDecimal(selectedRow.Cells["Valor"].Value);
                decimal desconto = Convert.ToDecimal(selectedRow.Cells["Desconto"].Value);

                // 🔹 Obtém idVend e garante que não seja nulo
                object idVendObj = selectedRow.Cells["idVend"].Value;
                int idVend = (idVendObj != DBNull.Value && idVendObj != null) ? Convert.ToInt32(idVendObj) : -1;

                // 🔹 Obtém idForma corretamente antes de usá-lo
                int idForma = selectedRow.Cells["idForma"].Value != DBNull.Value ? Convert.ToInt32(selectedRow.Cells["idForma"].Value) : -1;

                Console.WriteLine($"DEBUG: idVend recebido ao clicar = {idVend}");
                Console.WriteLine($"DEBUG: idForma recebido ao clicar = {idForma}");

                this.iID = id;
                this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
                cmbCliente.SelectedValue = nrCli;

                // 🔹 Agora idForma sempre terá um valor válido antes de ser usado
                tb.Forma regF = idForma >= 0 ? (tb.Forma)cFormas.GetPeloID((idForma + 1).ToString()) : null;

                if (idForma == this.DefDeb || (regF != null && regF.Tipo == 1))
                {
                    textBox1.Text = valor.ToString();
                    txCompra.Text = "";
                }
                else
                {
                    txCompra.Text = valor.ToString();
                    textBox1.Text = "";
                }
                txDesc.Text = desconto.ToString();
                txObs.Text = selectedRow.Cells["Obs"].Value.ToString();

                // 🔹 Agora garantimos que idVend só será atualizado se for válido
                if (idVend != -1 && cmbVendedor.DataSource is List<tb.ComboBoxItem> vendedorList)
                {
                    Console.WriteLine($"DEBUG: Lista de vendedores contém {vendedorList.Count} registros.");

                    var vendedorEncontrado = vendedorList.FirstOrDefault(v => v.Id == idVend);

                    if (vendedorEncontrado != null)
                    {
                        Console.WriteLine($"DEBUG: Nome do vendedor encontrado = {vendedorEncontrado.Nome}");
                        cmbVendedor.Text = vendedorEncontrado.Nome;
                    }
                    else
                    {
                        Console.WriteLine("DEBUG: Nenhum vendedor encontrado! Definindo para 'Não Definido'.");
                        cmbVendedor.Text = "Não Definido";
                    }
                }
                else
                {
                    Console.WriteLine("DEBUG: Vendedor inválido! Definindo como 'Não Definido'.");
                    cmbVendedor.Text = "Não Definido";
                }

                MostraTotal();
                btnLimpar.Enabled = true;
                BotoesNormais();

                Color cor = Color.FromArgb(128, 255, 128);
                if (idForma != this.DefCred && idForma != this.DefDeb)
                {
                    SetBotaoColor(idForma + 1, cor);
                }

                btExcluir.Visible = true;
                btEditar.Visible = true;
            }
        }

        private void SetBotaoColor(int idForma, Color cor)
        {
            foreach (GroupBox grp in new[] { grpCredito, grpDebito })
            {
                foreach (Control control in grp.Controls)
                {
                    if (control is Button button && button.Tag != null)
                    {
                        if (int.TryParse(button.Tag.ToString(), out int buttonId) && buttonId == idForma)
                        {
                            button.BackColor = cor;
                            return; // Sai do método após encontrar e colorir o botão correto
                        }
                    }
                }
            }
        }

        private void CarregaGrid(int idForma = 0)
        {
            string sObs = txObs.Text;
            string sCliente = "";
            string sVendedor = "";
            if (cmbCliente.SelectedIndex>0)
            {
                sCliente = cmbCliente.Text;
            }
            if (cmbVendedor.SelectedIndex>0)
            {
                sVendedor = cmbVendedor.Text;
            }
            string sValor = txCompra.Text;
            string sValorDebito = textBox1.Text;
            string sDesconto = txDesc.Text;

            DateTime DT11 = DateTime.FromOADate(Convert.ToDouble(dtpDataIN.Tag));
            DataTable dados = Caixa.getDados(DT11, dtnDtFim.Value, idForma, sObs, sCliente, sVendedor, sValor, sValorDebito, sDesconto);
            dtpDataIN.Tag = Caixa.DT1.ToOADate().ToString();

            dataGrid1.DataSource = dados;
        }

        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Visible = false; // Tornar a primeira coluna invisível (largura = 0)
            dataGrid1.Columns[1].Width = 170;   // Cliente
            dataGrid1.Columns[2].Width = 70;    // Valor
            dataGrid1.Columns[3].Width = 70;    // Desconto
            dataGrid1.Columns[4].Width = 70;    // VlNota            
            dataGrid1.Columns[5].Width = 170;   // Vendedor
            dataGrid1.Columns[6].Width = 100;   // Data
            dataGrid1.Columns[7].Width = 70;    // Forma de pagamento
            dataGrid1.Columns[8].Width = 200;    // Obs
            dataGrid1.Columns[9].Visible = false;
            dataGrid1.Columns[10].Visible = false;
            dataGrid1.Columns[11].Visible = false;
            dataGrid1.Columns[12].Visible = false;
            if (rt.IsLargeScreen())
            {
                for (int i = 1; i < 12; i++)
                {
                    dataGrid1.Columns[i].Width = (int)(dataGrid1.Columns[i].Width * rt.scaleFactor);
                }
            }
        }

        #endregion

        private void txCompra_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (glo.Nivel == 2)
                {
                    Registra(this.DefCred);
                }                    
            }
            else
            {
                VeSeHab();
            }
            MostraTotal();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (glo.Nivel == 2)
                {
                    Registra(this.DefDeb, false);
                }                    
            }
            else
            {
                VeSeHab();
            }
            MostraTotal();
        }

        private void txObs_KeyUp(object sender, KeyEventArgs e)
        {
            if (glo.Nivel == 2)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (textBox1.Text.Length > 0)
                    {
                        Registra(this.DefDeb, false);
                    }
                    else
                    {
                        Registra(this.DefCred, false);
                    }
                }
            }
        }

        private void dtpDataIN_ValueChanged(object sender, EventArgs e)
        {
            dtpDataIN.Tag = dtpDataIN.Value.ToOADate().ToString();
        }
    }
}
