﻿using System;
using System.IO;
using Zuby.ADGV;
using System.Data;
using System.Linq;
using System.Drawing;
using TeleBonifacio.dao;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;

namespace TeleBonifacio
{
    public partial class OperFalta : Form
    {
        private INI cINI;
        private FaltasDAO faltasDAO;
        private TpoFaltaDAO TpoFalta;
        private FornecedorDao Forn;
        private EncomendasDao EncoDao;
        private ProdutosDao cDaoP;      
        private GarantiasDao cDaoG;
        private pesCliente FpesCliente;
        private DataTable dadosCli;
        private Dictionary<int, Color> tipoFaltaCores = new Dictionary<int, Color>();        
        private Color originalBackgroundColor;
        private bool carregando = true;
        private bool Restrito = false;        
        private int BakidTipo = 0;
        private int BakidForn = 0;
        private int bakComprado = 0;
        private string Bakcodigo = "";
        private string Bakquantidade = "";
        private string Bakmarca = "";
        private string BakObs = "";
        private string BakDescr = "";
        private string iUser = "";
        private string BakCodigoLost = "";
        private int BakidVendedor = 0;
        private int bakEmFalta = 0;
        private int idCliente = 0;
        private bool AtualizarGridP = true;
        private bool AtualizarGridE = true;
        private bool AtualizarGridG = true;
        private int iID = 0;
        private string caminhoDoArquivo = "";
        private bool Instanciar = true;

        #region Inicializacao

        public OperFalta()
        {
            InitializeComponent();
            SetStartPosition();
        }

        // Refatorado em 09/09/24 Original 44 linhas, resultado 10 linhas
        private void OperFalta_Load(object sender, EventArgs e)
        {
            InicializarObjetos();
            ConfigurarUI();
            VerificarNivel();
            carregando = false;
            CarregaGrid();
            ConfigureDataGridView(this.dataGrid1);
            griTaxas.SortStringChanged += GriTaxas_SortStringChanged;
            rt.AdjustFormComponents(this);
        }

        private void InicializarObjetos()
        {
            VendedoresDAO Vendedor = new VendedoresDAO();
            iUser = glo.iUsuario.ToString();
            CarregarComboBoxV<tb.Vendedor>(cmbVendedor, Vendedor, iUser);
            faltasDAO = new FaltasDAO();
            TpoFalta = new TpoFaltaDAO();
            Forn = new FornecedorDao();
            MostraTipos();
        }
        private void ConfigurarUI()
        {
            if (iUser.Length == 0)
            {
                btAdicTpo.Visible = false;
                btComprei.Visible = false;
                btnExcluir.Visible = false;
                Restrito = true;
            }
            originalBackgroundColor = txtCodigo.BackColor;
            if (glo.ODBC)
            {
                btEncomenda.Enabled = false;
            }
        }

        private void VerificarNivel()
        {
            glo.Loga("glo.Nivel = " + glo.Nivel.ToString());
            if (glo.Nivel == 2)
            {
                lbVlor.Visible = true;
                txValor.Visible = true;
                ConfigureDataGridView(this.dataGrid2);
                ConfigureDataGridView(this.dataGrid3);
                ConfigureDataGridView(this.dataGrid4);
                glo.Loga("Vai entrar em PreparaAbasUsers");
                PreparaAbasUsers(new VendedoresDAO());
            }
            else
            {
                tbFaltas.TabPages.Remove(tabPage2);
                tbFaltas.TabPages.Remove(tabPage3);
                tbFaltas.TabPages.Remove(tabPage4);
                tbFaltas.TabPages.Remove(tabPage5);
            }
        }

        private void PreparaAbasUsers(VendedoresDAO vendedor)
        {
            string pastaDoPrograma = AppDomain.CurrentDomain.BaseDirectory;
            glo.Loga("pastaDoPrograma = "+ pastaDoPrograma);
            string padraoDeBusca = "Anotacoes*.rtf";
            string[] arquivosEncontrados = Directory.GetFiles(pastaDoPrograma, padraoDeBusca)
                                                        .Select(Path.GetFileName)
                                                        .Where(nome => !string.Equals(nome, "anotacoes.rtf", StringComparison.OrdinalIgnoreCase))
                                                        .ToArray();
            if (arquivosEncontrados.Length > 0)
            {
                foreach (string arquivo in arquivosEncontrados)
                {
                    string numeroString = new string(arquivo.SkipWhile(c => !char.IsDigit(c))
                                                .TakeWhile(char.IsDigit)
                                                .ToArray());
                    tb.Vendedor reg = (tb.Vendedor)vendedor.GetPeloNr(numeroString);
                    if (reg != null)
                    {
                        TabPage novaAba = new TabPage(reg.Usuario);
                        AtcCtrl.ATCRTF atcRtf = new AtcCtrl.ATCRTF();
                        atcRtf.Dock = DockStyle.Fill;
                        atcRtf.caminhoDoArquivo = Path.Combine(pastaDoPrograma, arquivo);
                        novaAba.Controls.Add(atcRtf);
                        tbFaltas.TabPages.Add(novaAba);
                    }
                    else
                    {
                        glo.Loga("reg = null");
                    }
                } 
            }
        }

        private void ConfigureDataGridView(DataGridView grid)
        {
            grid.Font = new System.Drawing.Font("Segoe UI", 12);
            grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grid.ColumnHeadersHeight = 30; 
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void CarregarComboBoxV<T>(ComboBox comboBox, VendedoresDAO vendedor, string iUser)
        {
            DataTable dados = vendedor.getBalconistas();
            List<tb.ComboBoxItem> lista = new List<tb.ComboBoxItem>();
            int NrLista = 0;
            int NrItem = 0;
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string Nro = row["Nro"].ToString();
                string nome = Nro + " - " + row["Nome"].ToString();
                tb.ComboBoxItem item = new tb.ComboBoxItem(id, nome);
                lista.Add(item);
                NrItem++;
                if (iUser == Nro)
                {
                    NrLista = NrItem;
                }
            }
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";
            if (NrLista > 0)
            {
                comboBox.SelectedValue = NrLista;
            }
            else
            {
                comboBox.SelectedIndex = -1;
            }
        }

        //private void OperFalta_Activated(object sender, EventArgs e)
        //{
        //    if (carregando)
        //    {
        //        carregando = false;
        //    }
        //}

        #endregion

        #region Adição
        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (btnAdicionar.Text != "Limpar")
            {
                string codigo = txtCodigo.Text;
                string ret = "";
                if (codigo.Length > 0)
                {
                    ret = faltasDAO.VeSeJaTem(codigo);
                }
                if (ret.Length > 0)
                {
                    MessageBox.Show(ret, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    int idBalconista = Convert.ToInt32(cmbVendedor.SelectedValue);
                    string quantidade = txQuantidade.Text;
                    string Marca = txMarca.Text;
                    string Descr = txDescr.Text;
                    string Obs = txObs.Text;
                    string UID = glo.GenerateUID();
                    int idForn = 0;
                    if (!Restrito)
                    {
                        if (cmbForn.FlatStyle == FlatStyle.Flat)
                        {
                            int iForn = cmbForn.SelectedIndex;
                            if (iForn > -1)
                            {
                                idForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
                            }
                        }
                    }
                    int idTipo = 0;
                    if (!Restrito)
                    {
                        if (cmbTipos.FlatStyle == FlatStyle.Flat)
                        {
                            int iTpo = cmbTipos.SelectedIndex;
                            if (iTpo > -1)
                            {
                                idTipo = ((tb.ComboBoxItem)cmbTipos.Items[iTpo]).Id;
                            }
                        }
                    }
                    glo.Loga($@"FA,{idBalconista}, {quantidade}, {codigo}, {Marca}, {Descr}, {Obs} , {idForn}, {idTipo}, {UID}");
                    faltasDAO.Adiciona(idBalconista, quantidade, codigo, Marca, Descr, Obs, idForn, idTipo, UID);
                    cmbVendedor.FlatStyle = FlatStyle.System;
                }
            }
            CarregaGrid();
            Limpar();
            cmbVendedor.Enabled = true;
        }

        private void Limpar()
        {            
            cmbTipos.FlatStyle = FlatStyle.System;
            cmbForn.FlatStyle = FlatStyle.System;
            cmbTipos.Tag = "";
            cmbForn.Tag = "";
            cmbTipos.SelectedIndex = 0;
            cmbForn.SelectedIndex = 0;            
            btnAdicionar.Text = "Adicionar";
            btnExcluir.Enabled = false;
            btAdicTpo.Enabled = false;
            BakCodigoLost = "";
            BakidTipo = 0;
            BakidForn = 0;
            bakComprado = 0;
            Bakcodigo = "";
            Bakquantidade = "";
            Bakmarca = "";
            BakObs = "";
            Normaliza(txtCodigo, 1);
            Normaliza(txMarca, 1);
            Normaliza(txDescr, 1);
            Normaliza(txObs, 1);
            Normaliza(txQuantidade, 1);
            Normaliza(txValor, 1);
            txtCodigo.Focus();
        }

        private void NormalizaCampos()
        {
            Normaliza(txtCodigo, 0);
            Normaliza(txMarca, 0);
            Normaliza(txDescr, 0);
            Normaliza(txObs, 0);
            Normaliza(txQuantidade, 0);
            Normaliza(txValor, 0);
            cmbTipos.FlatStyle = FlatStyle.System;
            cmbForn.FlatStyle = FlatStyle.System;
            cmbTipos.Tag = "";
            cmbForn.Tag = "";

        }

        private void Normaliza(TextBox obj, int tipo)
        {
            if (tipo == 1)
            {
                obj.Text = "";
            }
            obj.BackColor = originalBackgroundColor;
            obj.ReadOnly = false;
            obj.Tag = "";
        }

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txtCodigo);
            if (tbFaltas.SelectedIndex == 0)
            {
                btnAdicionar.Enabled = true;
            }
        }

        private void VeSeHab(TextBox obj = null)
        {
            if (obj != null)
            {
                if (!Restrito)
                {
                    if (!obj.ReadOnly)
                    {
                        if (obj.Text.Length > 0)
                        {
                            if (tbFaltas.SelectedIndex == 0)
                            {
                                obj.BackColor = Color.Yellow;

                            }
                            button2.Enabled = btLmpFiltro.Enabled = true;
                            btAdicTpo.Enabled = true;
                            obj.Tag = "M";
                        } 
                    }
                }
            }
            bool ok = true;
            if (cmbVendedor.SelectedIndex == -1)
            {
                ok = false;
            }
            if (txMarca.Text.Length == 0)
            {
                ok = false;
            }
            if (txDescr.Text.Length == 0)
            {
                ok = false;
            }
            btnAdicionar.Enabled = ok;
        }

        private void txQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txQuantidade);
        }

        private void txMarca_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txMarca);
        }

        private void txObs_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txObs);
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                VeSeHab();
                cmbVendedor.FlatStyle = FlatStyle.Flat;
                button2.Enabled = true;
            }
        }

        private void ReadlyOnly(bool v)
        {
            cmbVendedor.Enabled = !v;
            txQuantidade.ReadOnly = v;
            txMarca.ReadOnly = v;
            txtCodigo.ReadOnly = v;
        }

        private void ExcluirRegistrosDaGrid(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                int gID = Convert.ToInt32(row.Cells["ID"].Value);
                string UID = Convert.ToString(row.Cells["UID"].Value);
                glo.Loga($@"FD,{gID}, {UID}");
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        faltasDAO.Exclui(gID);
                        break;
                    case 1:
                        cDaoP.Exclui(gID);
                        break;
                    case 2:
                        EncoDao.Exclui(gID);
                        break;
                    case 3:
                        cDaoG.Exclui(gID);
                        break;
                    default:
                        return;
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        ExcluirRegistrosDaGrid(dataGrid1);
                        CarregaGrid();
                        break;
                    case 1:
                        ExcluirRegistrosDaGrid(dataGrid2);
                        CarregaGridP();
                        break;
                    case 2:
                        ExcluirRegistrosDaGrid(dataGrid3);
                        CarregaGridE();
                        break;
                    case 3:
                        ExcluirRegistrosDaGrid(dataGrid4);
                        CarregaGridG();
                        break;
                    default:
                        return;
                }
                Limpar();
            }
        }

        private void txDescr_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txDescr);
        }

        #endregion

        #region Faltas
        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Width = 50;       // Contador

            dataGrid1.Columns[1].Width = 100;       // Compra
            dataGrid1.Columns[2].Width = 130;       // Forn
            dataGrid1.Columns[3].Visible = false;    // ID false;
            dataGrid1.Columns[4].Visible = false;
            dataGrid1.Columns[5].Width = 80;        // Data
            dataGrid1.Columns[6].Width = 80;        // Código

            dataGrid1.Columns[7].Width = 50;        // Quantidade
            dataGrid1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGrid1.Columns[8].Width = 80;        // Marca
            dataGrid1.Columns[9].Width = 240; //  160;       // Descrição
            dataGrid1.Columns[10].Width = 130;       // Balconista
            dataGrid1.Columns[11].Visible = false;  // UID
            dataGrid1.Columns[12].Width = 110;  //  130;      // Tipo - colocado o texto
            dataGrid1.Columns[13].Visible = false;  // Tipo valor original
            dataGrid1.Columns[14].Visible = false;  // idForn

            if (glo.Nivel == 2)
            {
                dataGrid1.Columns[15].Visible = true;   // Valor
                dataGrid1.Columns[15].Width = 50; 
                dataGrid1.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else
            {
                dataGrid1.Columns[15].Visible = false;  // Valor
            }            
            dataGrid1.Columns[16].Width = 170;      // Obs
            dataGrid1.Columns[17].Visible = false;  // Prioridade
            if (rt.IsLargeScreen())
            {
                for (int i = 1; i < 16; i++)
                {
                    dataGrid1.Columns[i].Width = (int)(dataGrid1.Columns[i].Width * rt.scaleFactor);
                }
            }
            dataGrid1.Invalidate();
        }

        // Refatorado em 26/08/24 Original 53 linhas, resultado 21 linhas
        private void CarregaGrid()
        {
            if (!carregando)
            {
                int scrollPosition = dataGrid1.FirstDisplayedScrollingRowIndex;
                FaltasDAO faltasDAO = new FaltasDAO();
                DataTable dados = faltasDAO.getDados(BakidTipo, BakidForn, bakComprado, Bakcodigo, Bakquantidade, Bakmarca, BakObs, BakidVendedor, bakEmFalta, BakDescr);
                List<tb.TpoFalta> tipos = TpoFalta.getTipos();
                List<tb.Fornecedor> fornecs = Forn.getForns();
                dataGrid1.DataSource = dados;
                ProcessarLinhas(dataGrid1.Rows, tipos, fornecs);
                if (dados != null)
                {
                    ConfigurarGrid();
                    if (scrollPosition > 0)
                    {
                        if (dataGrid1.Rows.Count> scrollPosition)
                        {
                            dataGrid1.FirstDisplayedScrollingRowIndex = scrollPosition;
                        }                        
                    }
                }
            }
        }

        private void ProcessarLinhas(DataGridViewRowCollection linhas, List<tb.TpoFalta> tipos, List<tb.Fornecedor> fornecs)
        {
            int contador = 0;
            foreach (DataGridViewRow row in linhas)
            {
                AplicarCorPorTipo(row);
                AplicarFontePorPrioridade(row);
                AtualizarLinha(row, tipos, "Tipo", "Tipo");
                AtualizarLinha(row, fornecs, "idForn", "Forn");
                contador++;
                row.Cells["Cont"].Value = contador.ToString();
            }
        }

        private void AplicarCorPorTipo(DataGridViewRow row)
        {
            if (!row.Cells["Tipo"].Value.Equals(DBNull.Value))
            {
                int tipoId = Convert.ToInt32(row.Cells["Tipo"].Value);

                // Busca a cor no array global
                if (tipoFaltaCores.TryGetValue(tipoId, out Color cor))
                {
                    row.DefaultCellStyle.BackColor = cor;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = SystemColors.Window; // Cor padrão se não tiver cor definida
                }
            }
        }

        private void AplicarFontePorPrioridade(DataGridViewRow row)
        {
            int prioridade = Convert.ToInt32(row.Cells["Prioridade"].Value);
            if (prioridade > 0)
            {
                row.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
            }
            else
            {
                row.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);
            }
        }        

        private void dataGrid1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                txQuantidade.Text = FiltraOZero(selectedRow.Cells["Quant"].Value);
                txtCodigo.Text = FiltraOZero(selectedRow.Cells["Codigo"].Value);
                txMarca.Text = FiltraOZero(selectedRow.Cells["Marca"].Value);
                txObs.Text = FiltraOZero(selectedRow.Cells["Obs"].Value);
                txDescr.Text = Convert.ToString(selectedRow.Cells["Descricao"].Value);
                txValor.Text = glo.fmtVlr(Convert.ToString(selectedRow.Cells["Valor"].Value));
                cmbVendedor.SelectedValue = Convert.ToInt32(selectedRow.Cells["IDBalconista"].Value);
                try
                {
                    cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idForn"].Value);
                }
                catch (Exception)
                {
                    cmbForn.SelectedValue = -1;
                }
                try
                {
                    cmbTipos.SelectedValue = Convert.ToInt32(selectedRow.Cells["TipoOrig"].Value);
                }
                catch (Exception)
                {
                    cmbTipos.SelectedValue = -1;
                }
                ReadlyOnly(true);
                btnAdicionar.Text = "Limpar";
                btnAdicionar.Enabled = true;
                btnExcluir.Enabled = true;
                btComprei.Enabled = true;
                txQuantidade.ReadOnly = false;
                txMarca.ReadOnly = false;
                if (dataGrid1.SelectedRows.Count == 1)
                {
                    txtCodigo.ReadOnly = false;
                }
                else
                {
                    txtCodigo.ReadOnly = true;
                }
                txQuantidade.BackColor = originalBackgroundColor;
                txMarca.BackColor = originalBackgroundColor;
                txtCodigo.BackColor = originalBackgroundColor;
                txDescr.BackColor = originalBackgroundColor;
                txObs.BackColor = originalBackgroundColor;
                carregando = false;
            }
        }

        private void AtualizarLinha<T>(DataGridViewRow row, List<T> items, string idColumnName, string displayColumnName)
            where T : tb.IDataEntity
        {
            if (!row.Cells[idColumnName].Value.Equals(DBNull.Value))
            {
                if (row.Cells[idColumnName].Value.ToString().Equals("0"))
                {
                    row.Cells[displayColumnName].Value = "";
                }
                else
                {
                    object oItem = row.Cells[idColumnName].Value;
                    int itemId = 0;
                    if (oItem != null)
                    {
                        if (oItem is int directInt)
                        {
                            itemId = Convert.ToInt32(oItem.ToString());
                        }
                        else
                        {
                            if (oItem is string itemAsString && int.TryParse(itemAsString, out int convertedInt))
                                itemId = convertedInt;
                        }
                    }
                    var itemEncontrado = items.Find(i => i.Id == itemId);
                    if (itemEncontrado != null)
                    {
                        row.Cells[displayColumnName].Value = itemEncontrado.Nome;
                    }
                    else
                    {
                        row.Cells[displayColumnName].Value = "";
                    }
                }
            }
        }
        
        private void ckEmFalta_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            // Verifica se o botão direito do mouse foi clicado
            if (e.Button == MouseButtons.Right)
            {
                // Obtém a posição da célula no ponto clicado
                DataGridView.HitTestInfo hit = this.dataGrid1.HitTest(e.X, e.Y);

                // Verifica se o clique foi em uma célula válida (não cabeçalho ou fora da grade)
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    // Seleciona a linha onde o botão direito foi clicado
                    this.dataGrid1.ClearSelection();
                    this.dataGrid1.Rows[hit.RowIndex].Selected = true;
                    this.dataGrid1.CurrentCell = this.dataGrid1.Rows[hit.RowIndex].Cells[hit.ColumnIndex];

                    this.iID = (int)this.dataGrid1.Rows[hit.RowIndex].Cells["ID"].Value;
                    int Prio = (int)this.dataGrid1.Rows[hit.RowIndex].Cells["Prioridade"].Value;
                    DiminirPrio.Enabled = (Prio > 0);

                    // Exibe o menu de contexto na posição do mouse
                    contextMenuStrip1.Show(this.dataGrid1, new Point(e.X, e.Y));
                }
            }
        }

        private void Aumentar_Click(object sender, EventArgs e)
        {
            faltasDAO.Prio(this.iID, "+");
            CarregaGrid();
        }

        private void DiminirPrio_Click(object sender, EventArgs e)
        {
            faltasDAO.Prio(this.iID, "-");
            CarregaGrid();
        }

        #endregion

        #region Tipos

        private void MostraTipos()
        {
            if (glo.ODBC)
            {
                glo.CarregarComboBox<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA");
                glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", filtro:"EhForn = 1 ");
            }
            else
            {
                glo.CarregarComboBoxComCores<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA", "", "", "ADICIONE", "EDIÇÃO", tipoFaltaCores: tipoFaltaCores);
                glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO", filtro: "EhForn = 1 ");
            }
        }

        private void btAdicTpo_Click(object sender, EventArgs e)
        {
            if (btAdicTpo.Text == "Adicionar")
            {
                if ((txNvTipo.Text.Length == 0) && (txNvForn.Text.Length == 0))
                {
                    MessageBox.Show("Só é possível adicionar se dizer qual ele é",
                                                  "Faltou dizer",
                                                  MessageBoxButtons.OK);
                    txNvTipo.Focus();
                }
                else
                {
                    if (txNvTipo.Visible)
                    {
                        TpoFalta.Adiciona(txNvTipo.Text, null);
                    }
                    else
                    {
                        Forn.Adiciona(txNvForn.Text);
                    }
                    RetCmboTpo();
                }
            }
            else
            {
                // ATUALIZAÇÃO
                int idForn = 0;
                if (cmbForn.Tag == "M")
                {
                    int iForn = cmbForn.SelectedIndex;
                    if (iForn > -1)
                    {
                        idForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
                    }
                }
                if (tbFaltas.SelectedIndex == 3)
                {
                    string sID = dataGrid4.SelectedRows[0].Cells[0].Value.ToString();
                    string UID = dataGrid4.SelectedRows[0].Cells[5].Value.ToString();
                    int gID = Convert.ToInt32(sID);
                    glo.Loga($@"FG,{gID}, {idForn}, {UID}");
                    cDaoG.MudaForn(gID, idForn);
                    CarregaGridG();
                    Limpar();
                }
                else
                {
                    string codigo = "";
                    if (txtCodigo.Tag == "M")
                    {
                        codigo = txtCodigo.Text;
                    }
                    int idTipo = 0;
                    if (cmbTipos.Tag == "M")
                    {
                        int iTpo = cmbTipos.SelectedIndex;
                        if (iTpo > -1)
                        {                            
                            string sTipo = cmbTipos.Text;
                            DataTable dados = TpoFalta.GetDadosOrdenados($" and Nome = '{sTipo}'");
                            idTipo = Convert.ToInt32(dados.Rows[0]["id"]);
                            // idTipo = 0; // Preciso colocar o conteúdo do campo id nesta variável
                            // Antes estava assim
                            // idTipo = ((tb.ComboBoxItem)cmbTipos.Items[iTpo]).Id;
                        }
                    }
                    string quantidade = "";
                    if (txQuantidade.Tag == "M")
                    {
                        quantidade = txQuantidade.Text;
                    }
                    string marca = "";
                    if (txMarca.Tag == "M")
                    {
                        marca = txMarca.Text;
                    }
                    string Obs = "";
                    if (txObs.Tag == "M")
                    {
                        Obs = txObs.Text;
                    }
                    string Descr = "";
                    if (txDescr.Tag == "M")
                    {
                        Descr = txDescr.Text;
                    }
                    float Vlr = 0;
                    if (txValor.Tag == "M")
                    {
                        Vlr = glo.LeValor(txValor.Text);
                    }
                    switch (tbFaltas.SelectedIndex)
                    {
                        case 0:
                            AtualizaItensSelecionados(0, dataGrid1, idTipo, idForn, codigo, quantidade, marca, Obs, Descr, Vlr);
                            break;
                        case 1:
                            AtualizaItensSelecionados(1, dataGrid2, idTipo, idForn, codigo, quantidade, marca, Obs, Descr, Vlr);
                            break;
                        case 2:
                            AtualizaItensSelecionados(2, dataGrid3, idTipo, idForn, codigo, quantidade, marca, Obs, Descr, Vlr);
                            break;
                    }
                }

            }
        }

        private void AtualizaItensSelecionados(int nrGrid, DataGridView grid, int idTipo, int idForn, string codigo, string quantidade, string marca, string obs, string descr, float Vlr)
        {
            HashSet<string> selectedCodes = new HashSet<string>();
            int scrollPosition = grid.FirstDisplayedScrollingRowIndex;
            if (grid.SelectedRows.Count > 1)
            {
                codigo = "";
            }
            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                string sID = Convert.ToString(row.Cells["ID"].Value);
                selectedCodes.Add(sID);
                int gID = Convert.ToInt32(row.Cells["ID"].Value);
                string UID = Convert.ToString(row.Cells["UID"].Value);
                glo.Loga($@"FA,{gID}, {idTipo}, {idForn}, {codigo}, {quantidade}, {marca}, {obs}, {descr}, {Vlr}, {UID}");
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        faltasDAO.Atualiza(gID, idTipo, idForn, codigo, quantidade, marca, obs, descr, Vlr);
                        break;
                    case 1:
                        cDaoP.Atualiza(gID, idTipo, idForn, codigo, quantidade, marca, obs, descr, Vlr);
                        break;
                    case 2:
                        EncoDao.Atualiza(gID, this.idCliente, idForn, codigo, quantidade, marca, obs, descr);
                        break;
                }
            }
            switch (tbFaltas.SelectedIndex)
            {
                case 0:
                    CarregaGrid();
                    break;
                case 1:
                    CarregaGridP();
                    break;
                case 2:
                    CarregaGridE();
                    break;
            }
            if (grid.Rows.Count > 0)
            {
                grid.FirstDisplayedScrollingRowIndex = scrollPosition;
                foreach (string code in selectedCodes)
                {
                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        string cod = row.Cells["ID"].Value.ToString();
                        if (cod == code)
                        {
                            row.Selected = true;
                            break;
                        }
                    }
                }
            }
            if (grid.SelectedRows.Count > 1)
            {                
                NormalizaCampos();
            }
        }


        private void AtualizouEmBaixo()
        {
            cmbTipos.SelectedIndex = 0;
            cmbTipos.FlatStyle = FlatStyle.System;
            cmbForn.SelectedIndex = 0;
            cmbForn.FlatStyle = FlatStyle.System;
            btAdicTpo.Enabled = false;
            btComprei.Enabled = false;
            Normaliza(txtCodigo, 0);
            Normaliza(txMarca, 0);
            Normaliza(txDescr, 0);
            Normaliza(txObs, 0);
            Normaliza(txQuantidade, 0);
        }

        private void cmbTipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                if (cmbTipos.SelectedItem != null)
                {
                    string ItemCombo = cmbTipos.SelectedItem.ToString();
                    if (ItemCombo != "ESCOLHA")
                    {
                        if (ItemCombo == "ADICIONE")
                        {
                            btAdicTpo.Text = "Adicionar";
                            btAdicTpo.Enabled = true;
                            txNvTipo.Visible = true;
                            cmbTipos.Visible = false;
                            btComprei.Text = "Cancelar";
                            btComprei.Enabled = true;
                            txNvTipo.Focus();
                        }
                        else
                        {
                            if (ItemCombo == "EDIÇÃO")
                            {
                                fCadTiposFaltas novoForm = new fCadTiposFaltas();
                                novoForm.ShowDialog();
                                MostraTipos();
                                CarregaGrid();
                            }
                            else
                            {
                                btAdicTpo.Enabled = true;
                                cmbTipos.FlatStyle = FlatStyle.Flat;
                                cmbTipos.Tag = "M";
                                button2.Enabled = true;
                                btLmpFiltro.Enabled = true;
                                btAdicTpo.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void cmbForn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                if (cmbForn.SelectedItem != null)
                {
                    string ItemCombo = cmbForn.SelectedItem.ToString();
                    if (ItemCombo != "ESCOLHA")
                    {
                        if (ItemCombo == "ADICIONE")
                        {
                            btAdicTpo.Text = "Adicionar";
                            btAdicTpo.Enabled = true;
                            txNvForn.Visible = true;
                            cmbForn.Visible = false;
                            btComprei.Text = "Cancelar";
                            btComprei.Enabled = true;
                            txNvForn.Focus();
                        }
                        else
                        {
                            if (ItemCombo == "EDIÇÃO")
                            {
                                CadFornec novoForm = new CadFornec();
                                novoForm.ShowDialog();
                                MostraTipos();
                            }
                            else
                            {
                                btAdicTpo.Enabled = true;
                                cmbForn.FlatStyle = FlatStyle.Flat;
                                cmbForn.Tag = "M";
                                button2.Enabled = true;
                                btLmpFiltro.Enabled = true;
                                btAdicTpo.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void txForn_KeyUp(object sender, KeyEventArgs e)
        {
            btAdicTpo.Enabled = true;
        }

        private void btComprei_Click(object sender, EventArgs e)
        {
            if (btComprei.Text == "Comprei")
            {
                //  COMPROU A MERCADORIA QUE ESTAVA EM FALTA
                int scrollPosition = dataGrid1.FirstDisplayedScrollingRowIndex;
                if (dataGrid1.SelectedRows.Count==1)
                {
                    string sID = dataGrid1.SelectedRows[0].Cells["ID"].Value.ToString();
                    int gID = Convert.ToInt32(sID);
                    string sVlr = dataGrid1.SelectedRows[0].Cells["Valor"].Value.ToString();
                    if (sVlr.Length==0)
                    {
                        sVlr = "0";
                    }
                    string inputValue = Microsoft.VisualBasic.Interaction.InputBox("Valor de compra", "Input Value", sVlr, -1, -1);
                    if (inputValue.Length>0)
                    {
                        float valor = glo.LeValor(inputValue);
                        faltasDAO.Comprou(gID, valor);
                    }
                } else
                {
                    foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                    {
                        int gID = Convert.ToInt32(row.Cells["ID"].Value);
                        faltasDAO.Comprou(gID, 0);
                    }
                }
                AtualizarGridP = true;
                CarregaGrid();
                if (scrollPosition>0)  
                    dataGrid1.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
            else
            {
                if (btComprei.Text == "Em Falta")
                {
                    // RETORNOU PARA EM FALTA A MERCADORIA QUE ESTAVA COMO COMPRADA
                    DialogResult result = MessageBox.Show("Tem certeza que deseja retornar para faltas?",
                                                          "Esta em falta",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dataGrid2.SelectedRows)
                        {
                            int gID = Convert.ToInt32(row.Cells["ID"].Value);
                            cDaoP.EmFalta(gID);
                        }
                        CarregaGridP();
                        CarregaGrid();
                        AtualizouEmBaixo();
                    }
                }
                else
                {
                    RetCmboTpo();
                    Limpar();
                }
            }
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (tbFaltas.SelectedIndex == 0)
            {
                string codigo = txtCodigo.Text;
                if (BakCodigoLost != codigo)
                {
                    BakCodigoLost = codigo;
                    string ret = "";
                    if (codigo.Length > 0)
                    {
                        ret = faltasDAO.VeSeJaTem(codigo);
                        if (ret.Length > 0)
                        {
                            if (ret.Contains("comprado"))
                            {
                                txtCodigo.BackColor = Color.Red;
                            }
                            else
                            {
                                txtCodigo.BackColor = Color.Orange;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Filtro

        private string FiltraOZero(object Value)
        {
            string texto = Convert.ToString(Value);
            if (texto == "0")
            {
                return "";
            }
            else
            {
                return texto;
            }
        }

        private void cmbTiposFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                button2.Enabled = btLmpFiltro.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Tag = "F";
            glo.Loga("Filtro");
            int iForn = cmbForn.SelectedIndex;
            BakidForn = 0;
            if (iForn > 0)
            {
                BakidForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;

            }
            if (tbFaltas.SelectedIndex == 3)
            {
                CarregaGridG();
            }
            else
            {
                int idTipo = cmbTipos.SelectedIndex;
                BakidTipo = 0;
                if (idTipo > 0)
                {
                    BakidTipo = ((tb.ComboBoxItem)cmbTipos.Items[idTipo]).Id;
                }
                if (cmbVendedor.FlatStyle == FlatStyle.Flat)
                {
                    int idVendedor = cmbVendedor.SelectedIndex;
                    BakidVendedor = ((tb.ComboBoxItem)cmbVendedor.Items[idVendedor]).Id;
                }
                Bakcodigo = "";
                if (txtCodigo.Tag == "M")
                {
                    Bakcodigo = txtCodigo.Text;
                }
                Bakquantidade = "";
                if (txQuantidade.Tag == "M")
                {
                    Bakquantidade = txQuantidade.Text;
                }
                Bakmarca = "";
                if (txMarca.Tag == "M")
                {
                    Bakmarca = txMarca.Text;
                }
                BakObs = "";
                if (txObs.Tag == "M")
                {
                    BakObs = txObs.Text;
                }
                BakDescr = "";
                if (txDescr.Tag == "M")
                {
                    BakDescr = txDescr.Text;
                }
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        bakEmFalta = (ckEmFalta.Checked == true) ? 1 : 0;
                        CarregaGrid();
                        break;
                    case 1:
                        CarregaGridP();
                        break;
                    case 2:
                        CarregaGridE();
                        break;
                }
                btnAdicionar.Text = "Limpar";
                btAdicTpo.Enabled = false;
            }
        }

        private void ckComprado_Click(object sender, EventArgs e)
        {
            button2.Enabled = btLmpFiltro.Enabled = true;
        }

        private void btLmpFiltro_Click(object sender, EventArgs e)
        {
            button2.Tag = "";
            btLmpFiltro.Enabled = false; 
            Limpar();
            BakidTipo = 0;
            BakidForn = 0;
            bakComprado = 0;
            Bakcodigo = "";
            Bakquantidade = "";
            Bakmarca = "";
            BakObs = "";
            switch (tbFaltas.SelectedIndex)
            {
                case 0:
                    CarregaGrid();
                    break;
                case 1:
                    CarregaGridP();
                    break;
                case 2:
                    CarregaGridE();
                    break;
                case 3:
                    CarregaGridG();
                    break;
            }
            btLmpFiltro.Enabled = true;
        }

        #endregion

        #region Produtos

        private void ConfigurarGridP()
        {
            dataGrid2.Columns[0].Width = 100;       // Compra
            dataGrid2.Columns[1].Width = 130;       // Forn
            dataGrid2.Columns[2].Visible = false;   // ID
            dataGrid2.Columns[3].Width = 80; // 100;       // Data
            dataGrid2.Columns[4].Width = 80;        // Código
            if (glo.Nivel==2)
            {
                dataGrid2.Columns[5].Visible = true;
                dataGrid2.Columns[5].Width = 80;
                dataGrid2.Columns[5].DefaultCellStyle.Format = "F2";
                dataGrid2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            } else
            {
                dataGrid2.Columns[5].Visible = false;
            }
            dataGrid2.Columns[6].Width = 50;        // Quantidade
            dataGrid2.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid2.Columns[7].Width = 80;        // Marca
            dataGrid2.Columns[8].Width = 230; // 240; //  150;       // Descrição
            dataGrid2.Columns[9].Visible = false;  // UID
            dataGrid2.Columns[10].Width = 130;      // Tipo - colocado o texto
            dataGrid2.Columns[11].Visible = false;  // Tipo valor original
            dataGrid2.Columns[12].Visible = false;  // idForn
            dataGrid2.Columns[13].Width = 290;      // Obs
            if (rt.IsLargeScreen())
            {
                for (int i = 0; i < 13; i++)
                {
                    dataGrid2.Columns[i].Width = (int)(dataGrid2.Columns[i].Width * rt.scaleFactor);
                }
            }
            dataGrid2.Invalidate();
        }

        private void CarregaGridP()
        {
            int scrollPosition = dataGrid2.FirstDisplayedScrollingRowIndex;
            cDaoP = new ProdutosDao();
            DataTable dados = cDaoP.getDados(BakidTipo, BakidForn, Bakcodigo, Bakquantidade, Bakmarca, BakObs, BakDescr);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid2.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid2.Rows)
            {
                AtualizarLinha(row, tipos, "Tipo", "Tipo");
                AtualizarLinha(row, Fornecs, "idForn", "Forn");
            }
            if (dados != null)
            {
                ConfigurarGridP();
                if (scrollPosition > 0)
                    dataGrid2.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
        }

        private void tbFaltas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                switch (tbFaltas.SelectedIndex)
                {
                    case 0: // Faltas
                        groupBox1.Enabled = true;
                        cmbTipos.Enabled = true;
                        btComprei.Visible = true;
                        btComprei.Text = "Comprei";
                        ckEmFalta.Visible = true;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        timer1.Enabled = false;
                        if (iUser.Length > 0)
                        {
                            cmbVendedor.SelectedItem = Convert.ToInt16(iUser);
                        }
                        else
                        {
                            cmbVendedor.Enabled = true;
                        }
                        break;
                    case 1: // Produtos
                        groupBox1.Enabled = true;
                        cmbTipos.Enabled = true;
                        btComprei.Visible = true;
                        btComprei.Text = "Em Falta";
                        ckEmFalta.Visible = false;
                        timer1.Enabled = false;
                        if (glo.Nivel==2)
                        {
                            lbVlor.Visible = true;
                            txValor.Visible = true;
                        }
                        if (AtualizarGridP)
                        {
                            carregando = true;
                            cmbVendedor.SelectedIndex = -1;
                            cmbVendedor.Enabled = false;
                            CarregaGridP();
                            AtualizarGridP = false;
                            carregando = false;
                        }
                        break;
                    case 2: //Encomendas
                        groupBox1.Enabled = true;
                        cmbTipos.Enabled = true;
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        timer1.Enabled = false;
                        if (AtualizarGridE)
                        {
                            carregando = true;
                            cmbVendedor.SelectedIndex = -1;
                            cmbVendedor.Enabled = false;
                            CarregaGridE();
                            AtualizarGridE = false;
                            carregando = false;
                        }
                        break;
                    case 3: // Garantias
                        groupBox1.Enabled = false;
                        cmbTipos.Enabled = false;
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        timer1.Enabled = false;
                        if (AtualizarGridG)
                        {
                            carregando = true;
                            cmbVendedor.SelectedIndex = -1;
                            cmbVendedor.Enabled = false;
                            CarregaGridG();
                            carregando = false;
                        }
                        break;
                    case 4: // Anotações
                        groupBox1.Enabled = false;
                        cmbTipos.Enabled = false;
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        if (rtfTexto.Text.Length == 0)
                        {
                            caminhoDoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "anotacoes.rtf");
                            rtfTexto.caminhoDoArquivo = caminhoDoArquivo;
                            rtfTexto.Criptografia = true;
                            rtfTexto.Carrega();
                        }
                        break;
                    case 5:
                        if (this.iniGrid.Length==0)
                        {
                            SetupDataGridView();
                        } 
                        break;

                    case 6:
                        groupBox1.Enabled = false;
                        cmbTipos.Enabled = false;
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        if (rtfWord.Text.Length == 0)
                        {
                            cINI = new INI();
                            bool AdaptAtivo = cINI.ReadBool("Opcoes", "AdaptAtivo", true);
                            string caminhoWord = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Word.rtf");
                            rtfWord.caminhoDoArquivo = caminhoWord;
                            rtfWord.Criptografia = false;
                            rtfWord.Carrega();                            
                            rtfWord.txPercVisivel(AdaptAtivo);
                        }
                        break;
                    case 7:
                        if (!excelAppVisible)
                        {
                            AbrirExcel();
                            excelAppVisible = true;
                        }                            
                        //excelApp = new Excel.Application();
                        //excelApp.Visible = true; // Exibe o Excel

                        //excelWorkbook = excelApp.Workbooks.Open(caminhoArquivo);
                        //this.Text = "Planilha Aberta: " + Path.GetFileName(caminhoArquivo);
                        break;

                    default: // Novas abas criadas dinamicamente
                        groupBox1.Enabled = false;
                        cmbTipos.Enabled = false;
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        timer1.Enabled = false;
                        TabPage abaAtual = tbFaltas.SelectedTab;
                        if (abaAtual.Controls.Count > 0 && abaAtual.Controls[0] is AtcCtrl.ATCRTF)
                        {
                            AtcCtrl.ATCRTF atcRtf = (AtcCtrl.ATCRTF)abaAtual.Controls[0];
                            if (string.IsNullOrEmpty(atcRtf.Text))
                            {
                                atcRtf.Carrega();
                            }
                        }
                        break;
                }
                Limpar();
            }
        }

        private void dataGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                txQuantidade.Text = FiltraOZero(selectedRow.Cells["Quant"].Value);
                txtCodigo.Text = FiltraOZero(selectedRow.Cells["Codigo"].Value);
                txMarca.Text = FiltraOZero(selectedRow.Cells["Marca"].Value);
                txObs.Text = FiltraOZero(selectedRow.Cells["Obs"].Value);
                txDescr.Text = Convert.ToString(selectedRow.Cells["Descricao"].Value);
                txValor.Text = glo.fmtVlr(Convert.ToString(selectedRow.Cells["Valor"].Value));
                try
                {
                    cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idForn"].Value);
                }
                catch (Exception)
                {
                    cmbForn.SelectedValue = -1;
                }
                try
                {
                    cmbTipos.SelectedValue = Convert.ToInt32(selectedRow.Cells["TipoOrig"].Value);
                }
                catch (Exception)
                {
                    cmbTipos.SelectedValue = -1;
                }
                ReadlyOnly(true);
                btnExcluir.Enabled = true;
                btComprei.Enabled = true;
                txQuantidade.ReadOnly = false;
                txMarca.ReadOnly = false;
                if (dataGrid2.SelectedRows.Count == 1)
                {
                    txtCodigo.ReadOnly = false;
                }
                else
                {
                    txtCodigo.ReadOnly = true;
                }
                txQuantidade.BackColor = originalBackgroundColor;
                txMarca.BackColor = originalBackgroundColor;
                txtCodigo.BackColor = originalBackgroundColor;
                txDescr.BackColor = originalBackgroundColor;
                txObs.BackColor = originalBackgroundColor;
                carregando = false;
            }
        }

        private void txValor_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txValor);
        }

        #endregion

        #region Garantias

        private void btGarantia_Click(object sender, EventArgs e)
        {
            operGarantia FoperGarantia = new operGarantia();
            FoperGarantia.ShowDialog();
            if (tbFaltas.SelectedIndex == 3)
            {
                CarregaGridG();
            }
        }

        private void CarregaGridG()
        {
            int scrollPosition = dataGrid4.FirstDisplayedScrollingRowIndex;
            AtualizarGridG = false;
            cDaoG = new GarantiasDao();
            DataTable dados = cDaoG.getDados(BakidForn);
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid4.DataSource = dados;
            if (dados != null)
            {
                ConfigurarGridG();
                if (scrollPosition > 0)
                    dataGrid4.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
        }

        private void ConfigurarGridG()
        {
            dataGrid4.Columns[0].Visible = false;   // Id
            dataGrid4.Columns[1].Width = 110;        // Data
            dataGrid4.Columns[2].Visible = false;   // idForn
            dataGrid4.Columns[3].Width = 70;        // Nota
            dataGrid4.Columns[4].Width = 110;        // Prometida
            dataGrid4.Columns[5].Width = 110;        // DataDoForn
            dataGrid4.Columns[6].Visible = false;   // UID
            try
            {
                dataGrid4.Columns[7].Width = 200;       // Fornecedor
            }
            catch (Exception)
            {
                // 
            }
            if (rt.IsLargeScreen())
            {
                for (int i = 0; i < 5; i++)
                {
                    dataGrid4.Columns[i].Width = (int)(dataGrid4.Columns[i].Width * rt.scaleFactor);
                }
            }
            dataGrid4.Invalidate();
        }

        private void dataGrid4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                //this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
                try
                {
                    cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idForn"].Value);
                }
                catch (Exception)
                {
                    cmbForn.SelectedValue = -1;
                }
                btnExcluir.Enabled = true;
                carregando = false;
            }
        }

        #endregion

        #region Encomendas

        private void btEncomenda_Click(object sender, EventArgs e)
        {
            glo.IdAdicionado = 0;
            string Descricao = "";
            string codigo = "";
            bool ProdNovo = false;
            switch (tbFaltas.SelectedIndex)
            {
                case 0: // Faltas                    
                    DefineCampos(dataGrid1, ref Descricao, ref ProdNovo, ref codigo);
                    break;
                case 1: // Produtos
                    DefineCampos(dataGrid2, ref Descricao, ref ProdNovo, ref codigo);
                    break;
                default:
                    ProdNovo = true;
                    break;
            }
            btEncomenda.Enabled = false;
            if (FpesCliente == null)
            {
                AcionaPesq(true, Descricao, ProdNovo, codigo);
            }
            else
            {
                try
                {
                    AcionaPesq(false, Descricao, ProdNovo, codigo);
                }
                catch (Exception)
                {
                    AcionaPesq(true, Descricao, ProdNovo, codigo);
                }
            }
            if (!glo.ODBC)
            {
                btEncomenda.Enabled = true;
            }
            if (FpesCliente.OK)
            {
                if (tbFaltas.SelectedIndex==2)
                {
                    CarregaGridE();
                } else
                {
                    AtualizarGridE = true;
                    if (ProdNovo == false)
                    {
                        if (tbFaltas.SelectedIndex == 0)
                        {
                            CarregaGrid();
                            AtualizouEmBaixo();
                        }
                    }
                }
            }
        }

        private void dataGrid3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView grid = sender as DataGridView;
                DataGridViewRow row = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(row.Cells["ID"].Value);
                string nome = row.Cells["Nome"].Value?.ToString() ?? "";
                string telefone = row.Cells["Telefone"].Value?.ToString() ?? "";
                DateTime data = Convert.ToDateTime(row.Cells["Data"].Value);
                DateTime dataPrometida = Convert.ToDateTime(row.Cells["DtPrometida"].Value);
                string codigo = row.Cells["Codigo"].Value?.ToString() ?? "";
                decimal valor = Convert.ToDecimal(row.Cells["Valor"].Value);
                string descricao = row.Cells["Descricao"].Value?.ToString() ?? "";
                PrepareAndShowFpesCliente(Instanciar, nome, telefone, data, dataPrometida, codigo, valor, descricao);
                if (FpesCliente.getAlterado())
                {
                    CarregaGridE();
                    Limpar();                                       
                }
            }
        }

        private void PrepareAndShowFpesCliente(bool instanciar, string nome, string telefone, DateTime data, DateTime dataPrometida, string codigo, decimal valor, string descricao)
        {
            bool sair = false;
            while (sair==false)
            {
                glo.Loga("Preparando FpesCliente");
                if (FpesCliente == null)
                {
                    glo.Loga("FpesCliente = null");
                    FpesCliente = new pesCliente();
                    if (dadosCli == null)
                    {
                        glo.Loga("dadosCli = null");
                        ClienteDAO Cliente = new ClienteDAO();
                        dadosCli = Cliente.GetDadosOrdenados();
                        FpesCliente.RecebeDadosCli(ref dadosCli, ref Forn, ref EncoDao, tbFaltas.SelectedIndex);
                    }
                }
                FpesCliente.setOperacao(2);
                FpesCliente.setId(this.iID);
                FpesCliente.CarregarDados(nome, telefone, data, dataPrometida, codigo, valor, descricao);
                if (instanciar)
                {
                    FpesCliente.Ativar();
                    FpesCliente.ShowDialog();
                    sair = true;
                }
                else
                {
                    try
                    {
                        FpesCliente.Visible = true;                        
                    }
                    catch (Exception)
                    {
                        FpesCliente = null;
                        instanciar = true;
                    }                    
                }
            }
        }

        private void DefineCampos(DataGridView Grid, ref string descricao, ref bool ProdNovo, ref string codigo)
        {
            if (Grid.SelectedRows.Count == 1)
            {
                if (Grid.SelectedRows.Count == 1)
                {
                    if (Grid.Rows.Count == 1 || Grid.SelectedRows[0].Index != 0)
                    {
                        descricao = Grid.SelectedRows[0].Cells["Descricao"].Value.ToString();
                        codigo = FiltraOZero(Grid.SelectedRows[0].Cells["Codigo"].Value.ToString());
                    }
                    else
                    {
                        ProdNovo = true;
                    }
                }
            }

        }

        private void AcionaPesq(bool Instanciar, string Descricao, bool ProdNovo, string codigo)
        {
            if (Instanciar)
            {
                FpesCliente = new pesCliente();
            }
            FpesCliente.SetDescricao(Descricao, ProdNovo, codigo);
            if (dadosCli == null)
            {
                ClienteDAO Cliente = new ClienteDAO();
                dadosCli = Cliente.GetDadosOrdenados();                
            }
            FpesCliente.setOperacao(1);
            if (Instanciar)
            {
                if (EncoDao == null)
                    EncoDao = new EncomendasDao();
                FpesCliente.RecebeDadosCli(ref dadosCli, ref Forn, ref EncoDao, tbFaltas.SelectedIndex);
                FpesCliente.Ativar();                
                FpesCliente.ShowDialog();
            }
            else
            {
                FpesCliente.ShowDialog();
            }
        }

        private void ConfigurarGridE()
        {
            dataGrid3.Columns[0].Visible = false;   // ID
            dataGrid3.Columns[1].Width = 130;       // Cliente
            dataGrid3.Columns[2].Width = 80; // 100;       // Data
            dataGrid3.Columns[3].Width = 100;       // Código
            dataGrid3.Columns[4].Width = 50;        // Valor
            dataGrid3.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid3.Columns[5].Width = 40;        // Quant
            dataGrid3.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid3.Columns[6].Width = 60;        // Marca
            dataGrid3.Columns[7].Width = 280; // 270; // 250; // 190;       // Descrição
            dataGrid3.Columns[8].Visible = false;   // UID
            dataGrid3.Columns[9].Width = 50;       // Tipo
            dataGrid3.Columns[10].Width = 80; // 130;      // Compra
            dataGrid3.Columns[11].Width = 100;      // Forn
            dataGrid3.Columns[12].Visible = false;  // IdForn
            dataGrid3.Columns[13].Width = 220;      // Obs
            dataGrid3.Columns[14].Visible = false;  // idCliente 
            dataGrid3.Columns[15].Visible = true;   //  false;  // Telefone
            dataGrid3.Columns[16].Visible = false;  // DtPrometida
            dataGrid3.Columns[17].Visible = false;  // idCliente
            if (rt.IsLargeScreen())
            {
                for (int i = 1; i < 13; i++)
                {
                    dataGrid3.Columns[i].Width = (int)(dataGrid3.Columns[i].Width * rt.scaleFactor);
                }
            }
            dataGrid3.Invalidate();
        }

        private void CarregaGridE()
        {
            int scrollPosition = dataGrid3.FirstDisplayedScrollingRowIndex;            
            if (EncoDao==null)
                EncoDao = new EncomendasDao();
            DataTable dados = EncoDao.getDados(BakidTipo, BakidForn, Bakcodigo, Bakquantidade, Bakmarca, BakObs, BakDescr);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid3.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid3.Rows)
            {
                AtualizarLinha(row, tipos, "Tipo", "Tipo");
                AtualizarLinha(row, Fornecs, "idForn", "Forn");
            }
            if (dados != null)
            {
                ConfigurarGridE();
            if (scrollPosition > 0)
                dataGrid3.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
        }

        private void dataGrid3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                txQuantidade.Text = FiltraOZero(selectedRow.Cells["Quant"].Value);
                txtCodigo.Text = FiltraOZero(selectedRow.Cells["Codigo"].Value);
                txMarca.Text = FiltraOZero(selectedRow.Cells["Marca"].Value);
                txObs.Text = FiltraOZero(selectedRow.Cells["Obs"].Value);
                txDescr.Text = Convert.ToString(selectedRow.Cells["Descricao"].Value);
                this.idCliente = Convert.ToInt16(selectedRow.Cells["idCliente"].Value);
                try
                {
                    cmbForn.SelectedValue = Convert.ToInt16(selectedRow.Cells["idForn"].Value);
                }
                catch (Exception)
                {
                    cmbForn.SelectedValue = -1;
                }
                ReadlyOnly(true);
                btnAdicionar.Text = "Limpar";
                btnAdicionar.Enabled = true;
                btnExcluir.Enabled = true;
                btComprei.Enabled = true;
                txQuantidade.ReadOnly = false;
                txMarca.ReadOnly = false;
                if (dataGrid3.SelectedRows.Count == 1)
                {
                    txtCodigo.ReadOnly = false;
                }
                else
                {
                    txtCodigo.ReadOnly = true;
                }
                txQuantidade.BackColor = originalBackgroundColor;
                txMarca.BackColor = originalBackgroundColor;
                txtCodigo.BackColor = originalBackgroundColor;
                txDescr.BackColor = originalBackgroundColor;
                txObs.BackColor = originalBackgroundColor;
                carregando = false;
            }
        }

        #endregion

        #region Taxas

        private string iniGrid = "";
        private int colunaClicada;
        private bool acionadoFiltroSort = false;
        private const int INITIAL_COLUMN_COUNT = 13;
        private const int INITIAL_ROW_COUNT = 28;
        private int ModoGrid =0;
        private List<CellInfo> gridCellsInfo = new List<CellInfo>();        

        private void SetupDataGridView()
        {
            this.iniGrid = @"C:\Entregas\Grid.ini";
            InitializeGrid();
            LoadColumnWidths();
            griTaxas.CellEndEdit -= griTaxas_CellEndEdit;
            griTaxas.ColumnWidthChanged -= griTaxas_ColumnWidthChanged;
            LoadDataFromDatabase();
            griTaxas.CellEndEdit += griTaxas_CellEndEdit;
            griTaxas.ColumnWidthChanged += griTaxas_ColumnWidthChanged;
            griTaxas.CellEndEdit += griTaxas_CellEndEdit;
            griTaxas.ColumnWidthChanged += griTaxas_ColumnWidthChanged;
            griTaxas.Refresh();
        }

        public void LoadDataFromDatabase()
        {
            string query = "SELECT RowIndex, ColumnIndex, CellValue FROM DynamicGrid ORDER BY RowIndex, ColumnIndex";
            DataTable dataTable = DB.ExecutarConsulta(query);

            foreach (DataRow row in dataTable.Rows)
            {
                int rowIndex = Convert.ToInt32(row["RowIndex"]);
                int columnIndex = Convert.ToInt32(row["ColumnIndex"]);
                string cellValue = row["CellValue"].ToString();
                EnsureGridSize(rowIndex, columnIndex);
                Console.WriteLine($"rowIndex: {rowIndex}, {cellValue}");
                griTaxas.Rows[rowIndex].Cells[columnIndex].Value = cellValue;

                gridCellsInfo.Add(new CellInfo
                {
                    RowIndexOriginal = rowIndex,
                    GridRowIndex = rowIndex,
                    ColumnIndex = columnIndex,
                    CellValue = cellValue
                });

            }
        }

        private void EnsureGridSize(int requiredRowIndex, int requiredColumnIndex)
        {
            // Adicionar colunas, se necessário
            while (griTaxas.ColumnCount <= requiredColumnIndex)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"Column {griTaxas.ColumnCount}";
                column.Name = $"Column{griTaxas.ColumnCount}";
                column.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);
                griTaxas.Columns.Add(column);
            }
            // Console.WriteLine($"Total de Linhas: {griTaxas.RowCount}, Total de Colunas: {griTaxas.ColumnCount}");
            while (griTaxas.RowCount <= requiredRowIndex)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);
                griTaxas.Rows.Add(row);
            }
        }

        private void InitializeGrid()
        {
            griTaxas.Rows.Clear();
            griTaxas.Columns.Clear();

            Font font = new Font("Arial", 12, FontStyle.Regular);

            // Adicionar colunas
            for (int i = 0; i < INITIAL_COLUMN_COUNT; i++)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"Column {i}";
                column.Name = $"Column{i}";
                column.DefaultCellStyle.Font = font;
                griTaxas.Columns.Add(column);
            }
            for (int i = 0; i < INITIAL_ROW_COUNT; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.DefaultCellStyle.Font = font;
                griTaxas.Rows.Add(row);
            }
            for (int i = 0; i < INITIAL_ROW_COUNT; i++)
            {
                for (int j = 0; j < INITIAL_COLUMN_COUNT; j++)
                {
                    griTaxas[j, i].Value = "";
                }
            }
        }

        private void griTaxas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string cellValue = griTaxas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
            int rowIndexParaSalvar=0;
            switch (ModoGrid)
            {
                case 0: // 🔹 Modo padrão (sem ordenação/filtragem)
                    rowIndexParaSalvar = e.RowIndex;
                    break;

                case 1: // 🔹 Modo ordenado (usa GridRowIndex)
                    rowIndexParaSalvar = gridCellsInfo[e.RowIndex].GridRowIndex;
                    break;

                case 2: // 🔹 Modo filtrado (precisa localizar RowIndexOriginal através do GridRowIndex)

                    var itemEncontrado = gridCellsInfo
                                    .FirstOrDefault(x => x.GridRowIndex == e.RowIndex && x.ColumnIndex == e.ColumnIndex);

                    if (itemEncontrado != null)
                    {
                        rowIndexParaSalvar = itemEncontrado.RowIndexOriginal;
                        Console.WriteLine($"DEBUG: Encontrado - RowIndexOriginal: {itemEncontrado.RowIndexOriginal}, GridRowIndex: {itemEncontrado.GridRowIndex}, ColumnIndex: {itemEncontrado.ColumnIndex}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠ AVISO: Nenhum item encontrado para GridRowIndex={e.RowIndex} e ColumnIndex={e.ColumnIndex}. Usando índice padrão.");
                    }
                    break;

                default:
                    Console.WriteLine($"⚠ ModoGrid inválido: {ModoGrid}, usando índice padrão.");
                    rowIndexParaSalvar = e.RowIndex;
                    break;
            }

            SaveCellToDatabase(rowIndexParaSalvar, e.ColumnIndex, cellValue);
            EnsureGridSize(e.RowIndex + 1, e.ColumnIndex + 1);
        }

        private void SaveCellToDatabase(int rowIndex, int columnIndex, string cellValue)
        {
            // 🔹 Evita erro de SQL Injection ao tratar aspas no valor da célula
            string safeCellValue = cellValue.Replace("'", "''");

            // 🔹 Verifica se o usuário deseja apagar o conteúdo (inserindo um ".")
            if (safeCellValue == ".")
            {
                string deleteQuery = $@"DELETE FROM DynamicGrid 
                                WHERE RowIndex = {rowIndex} AND ColumnIndex = {columnIndex}";
                glo.Loga($@"DL,{rowIndex}, {columnIndex} - Registro removido");
                DB.ExecutarComandoSQL(deleteQuery);
                Console.WriteLine($"🗑 Registro removido para RowIndex {rowIndex}, ColumnIndex {columnIndex}");
                return; // Sai antes de continuar o fluxo normal
            }

            // 🔹 Verifica se o registro já existe
            string checkQuery = $"SELECT COUNT(*) FROM DynamicGrid WHERE RowIndex = {rowIndex} AND ColumnIndex = {columnIndex}";
            int count = DB.ExecutarConsultaCount(checkQuery);

            string query;
            if (count > 0)
            {
                // 🔹 Caso o registro já exista, faz um UPDATE
                query = $@"UPDATE DynamicGrid 
                   SET CellValue = '{safeCellValue}' 
                   WHERE RowIndex = {rowIndex} AND ColumnIndex = {columnIndex}";
                glo.Loga($@"UD,{rowIndex}, {columnIndex}, {safeCellValue} - Registro atualizado");
            }
            else
            {
                // 🔹 Caso não exista, faz um INSERT
                query = $@"INSERT INTO DynamicGrid (RowIndex, ColumnIndex, CellValue) 
                   VALUES ({rowIndex}, {columnIndex}, '{safeCellValue}')";
                glo.Loga($@"ID,{rowIndex}, {columnIndex}, {safeCellValue} - Novo registro inserido");
            }

            // 🔹 Executa o comando SQL final (INSERT ou UPDATE)
            DB.ExecutarComandoSQL(query);
        }


        //private void SaveCellToDatabase(int rowIndex, int columnIndex, string cellValue)
        //{
        //    // 🔹 Evita erro de SQL Injection ao tratar aspas no valor da célula
        //    string safeCellValue = cellValue.Replace("'", "''");

        //    // 🔹 Verifica se o registro já existe
        //    string checkQuery = $"SELECT COUNT(*) FROM DynamicGrid WHERE RowIndex = {rowIndex} AND ColumnIndex = {columnIndex}";
        //    int count = DB.ExecutarConsultaCount(checkQuery);

        //    string query;
        //    if (count > 0)
        //    {
        //        // 🔹 Caso o registro já exista, faz um UPDATE
        //        query = $@"UPDATE DynamicGrid 
        //           SET CellValue = '{safeCellValue}' 
        //           WHERE RowIndex = {rowIndex} AND ColumnIndex = {columnIndex}";
        //        glo.Loga($@"UD,{rowIndex}, {columnIndex}, {safeCellValue} - Registro atualizado");
        //    }
        //    else
        //    {
        //        // 🔹 Caso não exista, faz um INSERT
        //        query = $@"INSERT INTO DynamicGrid (RowIndex, ColumnIndex, CellValue) 
        //           VALUES ({rowIndex}, {columnIndex}, '{safeCellValue}')";
        //        glo.Loga($@"ID,{rowIndex}, {columnIndex}, {safeCellValue} - Novo registro inserido");
        //    }

        //    // 🔹 Executa o comando SQL final (INSERT ou UPDATE)
        //    DB.ExecutarComandoSQL(query);
        //}        

        private void griTaxas_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            SaveColumnWidths();
        }

        private void SaveColumnWidths()
        {
            using (StreamWriter writer = new StreamWriter(this.iniGrid))
            {
                for (int i = 0; i < griTaxas.ColumnCount; i++)
                {
                    writer.WriteLine($"Column{i}Width={griTaxas.Columns[i].Width}");
                }
            }
        }

        private void LoadColumnWidths()
        {
            if (File.Exists(this.iniGrid))
            {
                string[] lines = File.ReadAllLines(this.iniGrid);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        string columnName = parts[0].Replace("Width", "");
                        int width;
                        if (int.TryParse(parts[1], out width))
                        {
                            DataGridViewColumn column = griTaxas.Columns[columnName];
                            if (column != null)
                            {
                                column.Width = width;
                            }
                        }
                    }
                }
            }
        }

        private void griTaxas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) // Verifica se a tecla pressionada é a seta para baixo
            {
                // Obtém a linha e a coluna atuais do cursor
                int rowIndex = griTaxas.CurrentCell.RowIndex;
                int colIndex = griTaxas.CurrentCell.ColumnIndex;
                if (rowIndex == griTaxas.RowCount - 1)
                {
                    AdicionarNovaLinha();
                }
                griTaxas.CurrentCell = griTaxas.Rows[rowIndex + 1].Cells[colIndex];
            }
        }
        private void AdicionarNovaLinha()
        {
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);
            griTaxas.Rows.Add(newRow);
        }
        private void griTaxas_SortStringChanged(object sender, Zuby.ADGV.AdvancedDataGridView.SortEventArgs e)
        {
            if (!this.acionadoFiltroSort)
            {
                LoadDataOrdenado();
                //griTaxas.ReadOnly = true;
                this.acionadoFiltroSort = true;
            }
            else
            {
                this.acionadoFiltroSort = false;
            }
        }

        public void LoadDataOrdenado()
        {
            ModoGrid = 1;
            Console.WriteLine("LoadDataOrdenado Versao 4");

            griTaxas.SuspendLayout(); // 🔹 Impede atualizações visuais temporariamente
            griTaxas.Rows.Clear();
            griTaxas.Refresh();
            Application.DoEvents();

            string ordenacao = griTaxas.SortString?.Trim(); // Obtém a string de ordenação e remove espaços extras
            string direcaoOrdenacao = ordenacao.Contains("DESC") ? "DESC" : "ASC"; // 🔹 Define a direção da ordenação

            // 🔹 Etapa 1: Obter os RowIndex ordenados com base na CellValue da coluna clicada
            string subQuery = $@"
        SELECT RowIndex 
        FROM DynamicGrid 
        WHERE ColumnIndex = {this.colunaClicada} 
        GROUP BY RowIndex, CellValue 
        ORDER BY CellValue {direcaoOrdenacao}";

            Console.WriteLine("DEBUG: Subconsulta de Ordenação -> " + subQuery);

            try
            {
                DataTable dtRowIndexes = DB.ExecutarConsulta(subQuery);

                // 🔹 Obtém os RowIndex ordenados corretamente
                List<int> rowIndexes = dtRowIndexes.AsEnumerable()
                                                   .Select(r => r.Field<int>("RowIndex"))
                                                   .ToList();

                if (!rowIndexes.Contains(0)) rowIndexes.Insert(0, 0); // 🔹 Garante que a linha de cabeçalho sempre apareça

                // 🔹 Etapa 2: Consulta principal para buscar os dados dos RowIndex ordenados
                string mainQuery = $@"
            SELECT RowIndex, ColumnIndex, CellValue 
            FROM DynamicGrid 
            WHERE RowIndex IN ({string.Join(",", rowIndexes)}) 
            ORDER BY RowIndex, ColumnIndex";

                Console.WriteLine("DEBUG: Consulta Principal -> " + mainQuery);

                DataTable dataTable = DB.ExecutarConsulta(mainQuery);

                // 🔹 Criamos as linhas primeiro
                foreach (int rowIndex in rowIndexes)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);
                    griTaxas.Rows.Add(newRow);
                }
                gridCellsInfo.Clear();
                // 🔹 Preenche os dados na DataGridView
                foreach (DataRow row in dataTable.Rows)
                {
                    int rowIndex = rowIndexes.IndexOf(Convert.ToInt32(row["RowIndex"])); // Obtém a posição correta
                    int columnIndex = Convert.ToInt32(row["ColumnIndex"]);
                    string cellValue = row["CellValue"].ToString();

                    int rowIndexOriginal = Convert.ToInt32(row["RowIndex"]);
                    int gridRowIndex = rowIndexes.IndexOf(rowIndexOriginal);

                    griTaxas.Rows[rowIndex].Cells[columnIndex].Value = cellValue;

                    gridCellsInfo.Add(new CellInfo
                    {
                        RowIndexOriginal = rowIndexOriginal,
                        GridRowIndex = gridRowIndex,
                        ColumnIndex = columnIndex,
                        CellValue = cellValue
                    });

                    Console.WriteLine($"DEBUG: Linha Atualizada -> RowIndex: {rowIndex}, ColumnIndex: {columnIndex}, Valor: {cellValue}");
                }

                Console.WriteLine($"DEBUG: Total de linhas carregadas após ordenação: {griTaxas.Rows.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERRO ao executar a ordenação: " + ex.Message);
            }
            griTaxas.ResumeLayout(); // 🔹 Retoma as atualizações visuais
        }

        private void griTaxas_FilterStringChanged_1(object sender, AdvancedDataGridView.FilterEventArgs e)
        {
            LoadDataFiltrado();
        }

        public void LoadDataFiltrado()
        {
            ModoGrid = 2;
            griTaxas.SuspendLayout(); // 🔹 Impede atualizações visuais
            griTaxas.Rows.Clear();
            griTaxas.Refresh();
            Application.DoEvents();

            string filtro = griTaxas.FilterString;
            string ordenacao = griTaxas.SortString;

            // 🔹 Etapa 1: Obter a lista de RowIndex filtrados
            string subQuery = "SELECT DISTINCT RowIndex FROM DynamicGrid";
            List<string> subCondicoes = new List<string>();

            Match match = Regex.Match(filtro, @"'([^']*)'"); // Captura o valor do filtro
            string filtroFormatado = match.Success ? match.Groups[1].Value : "";

            subCondicoes.Add($"CellValue LIKE '%{filtroFormatado}%'");
            subCondicoes.Add($"ColumnIndex = {this.colunaClicada}");
            subQuery += " WHERE " + string.Join(" AND ", subCondicoes);

            Console.WriteLine("DEBUG: Subconsulta -> " + subQuery);

            DataTable rowIndexesTable = DB.ExecutarConsulta(subQuery);
            List<int> rowIndexes = rowIndexesTable.AsEnumerable().Select(r => r.Field<int>("RowIndex")).ToList();

            if (rowIndexes.Count == 0)
            {
                Console.WriteLine("DEBUG: Nenhum resultado encontrado.");
                griTaxas.ResumeLayout();
                return;
            }

            // 🔹 Criamos as linhas primeiro com base na quantidade de resultados da subconsulta
            for (int i = 0; i < rowIndexes.Count; i++)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);
                griTaxas.Rows.Add(newRow);
            }

            // 🔹 Etapa 2: Buscar os dados para os RowIndex obtidos
            string query = $@"
        SELECT RowIndex, ColumnIndex, CellValue 
        FROM DynamicGrid 
        WHERE RowIndex IN ({string.Join(",", rowIndexes)}) 
        ORDER BY RowIndex, ColumnIndex";

            Console.WriteLine("DEBUG: Consulta Final -> " + query);

            DataTable dataTable = DB.ExecutarConsulta(query);
            gridCellsInfo.Clear();
            // 🔹 Preenche as células da grid com os dados recuperados
            foreach (DataRow row in dataTable.Rows)
            {
                int rowIndex = rowIndexes.IndexOf(Convert.ToInt32(row["RowIndex"])); // Pega a posição correta na grid
                int columnIndex = Convert.ToInt32(row["ColumnIndex"]);
                string cellValue = row["CellValue"].ToString();

                int rowIndexOriginal = Convert.ToInt32(row["RowIndex"]);
                int gridRowIndex = rowIndexes.IndexOf(rowIndexOriginal);

                griTaxas.Rows[rowIndex].Cells[columnIndex].Value = cellValue;

                if (cellValue == "ArrudaDeve2.6")
                {
                    int x = 0;
                }

                gridCellsInfo.Add(new CellInfo
                {
                    RowIndexOriginal = rowIndexOriginal,
                    GridRowIndex = gridRowIndex,
                    ColumnIndex = columnIndex,
                    CellValue = cellValue
                });

                Console.WriteLine($"DEBUG: Linha Atualizada -> RowIndex: {rowIndex}, ColumnIndex: {columnIndex}, Valor: {cellValue}");
            }

            Console.WriteLine($"DEBUG: Total de linhas carregadas: {griTaxas.Rows.Count}");
            griTaxas.ResumeLayout(); // 🔹 Retoma a atualização visual
        }

        private void griTaxas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.colunaClicada = e.ColumnIndex;
        }

        private void GriTaxas_SortStringChanged(object sender, AdvancedDataGridView.SortEventArgs e)
        {
            if (!this.acionadoFiltroSort)
            {
                LoadDataFiltrado();
                //griTaxas.ReadOnly = true;
                this.acionadoFiltroSort = true;
            }
            else
            {
                this.acionadoFiltroSort = false;
            }
        }

        #endregion

        #region Excel

        private bool excelAppVisible=false;
        private Excel.Application excelApp;
        private Excel.Workbook excelWorkbook;
        private Excel.Worksheet excelSheet;
        private string caminhoArquivoExcel = @"C:\Entregas\Arquivo.xlsx";

        private Excel.Workbook workbook;

        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const uint SWP_SHOWWINDOW = 0x0040;

        private const int SW_MAXIMIZE = 3;

        private void AbrirExcel()
        {
            foreach (var process in Process.GetProcessesByName("EXCEL"))
            {
                process.Kill();
            }
            if (!File.Exists(caminhoArquivoExcel))
            {
                CriarNovoArquivoExcel();
            }

            excelApp = new Excel.Application
            {
                Visible = true,
                DisplayFullScreen = false,
            };
            File.SetAttributes(caminhoArquivoExcel, FileAttributes.Normal);

            workbook = excelApp.Workbooks.Open(caminhoArquivoExcel, ReadOnly: false);

            // Força exibição da Ribbon corretamente:
            excelApp.ExecuteExcel4Macro("SHOW.TOOLBAR(\"Ribbon\",TRUE)");
            excelApp.SendKeys("^{F1}", true); // Ctrl + F1 para mostrar Ribbon (garantia adicional)

            // Embute o Excel no painel
            IntPtr excelHandle = new IntPtr(excelApp.Hwnd);
            SetParent(excelHandle, panelExcel.Handle);
            ShowWindow(excelHandle, SW_MAXIMIZE);
        }

        private void CriarNovoArquivoExcel()
        {
            try
            {
                excelApp = new Excel.Application();
                excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelWorkbook.Sheets[1];

                // 🔹 Criando um cabeçalho na planilha
                excelSheet.Cells[1, 1] = "ID";
                excelSheet.Cells[1, 2] = "Nome";
                excelSheet.Cells[1, 3] = "Data";
                excelSheet.Cells[1, 4] = "Valor";

                // 🔹 Salvando o arquivo
                excelWorkbook.SaveAs(caminhoArquivoExcel);
                excelWorkbook.Close();
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar novo arquivo Excel: " + ex.Message);
            }
        }

        #endregion

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string arquivo = openFileDialog1.FileName;
                int total = ProcessarLog(arquivo);
                MessageBox.Show(total.ToString() + " Registros importados", "Processamento do log", MessageBoxButtons.OK);
            }
        }

        private int ProcessarLog(string caminhoArquivoLog)
        {
            string[] linhas = File.ReadAllLines(caminhoArquivoLog);
            int c = 0;
            foreach (string linha in linhas)
            {
                if (linha.Contains("FA,"))
                {
                    ProcessarAdicao(linha);
                }
                else if (linha.Contains("FD,"))
                {
                    ProcessarExclusao(linha);
                }
                c++;
            }
            return c;
        }

        private void ProcessarAdicao(string linha)
        {
            string[] partes = linha.Split(',');
            int idBalconista = Convert.ToInt32(partes[1].Trim());
            string quantidade = string.IsNullOrWhiteSpace(partes[2]) ? "0" : partes[2].Trim();
            string codigo = string.IsNullOrWhiteSpace(partes[3]) ? "N/A" : partes[3].Trim();
            string marca = string.IsNullOrWhiteSpace(partes[4]) ? "N/A" : partes[4].Trim().Replace("'", "''"); // Escapa aspas simples
            string descricao = string.IsNullOrWhiteSpace(partes[5]) ? "N/A" : partes[5].Trim().Replace("'", "''"); // Escapa aspas simples
            string obs = string.IsNullOrWhiteSpace(partes[6]) ? "" : partes[6].Trim().Replace("'", "''"); // Escapa aspas simples
            int idForn = string.IsNullOrWhiteSpace(partes[7]) ? 0 : Convert.ToInt32(partes[7].Trim());
            int idTipo = string.IsNullOrWhiteSpace(partes[8]) ? 0 : Convert.ToInt32(partes[8].Trim());
            string UID = string.IsNullOrWhiteSpace(partes[9]) ? "N/A" : partes[9].Trim().Replace("'", "''"); //
            glo.Loga($@"FA,{idBalconista}, {quantidade}, {codigo}, {marca}, {descricao}, {obs} , {idForn}, {idTipo}, {UID}");
            faltasDAO.Adiciona(idBalconista, quantidade, codigo, marca, descricao, obs, idForn, idTipo, UID);
        }

        private void ProcessarExclusao(string linha)
        {
            string[] partes = linha.Split(',');
            int gID = Convert.ToInt32(partes[1].Trim());
            string UID = partes[2].Trim(); 
            glo.Loga($@"FD,{gID}, {UID}");
            faltasDAO.Exclui(gID);
        }

        private void rtfWord_VlrPerImrChanged(object sender, bool e)
        {
            cINI.WriteBool("Opcoes", "AdaptAtivo", rtfWord.AltImprHab);
        }

        private void txNvTipo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txNvTipo.Text.Length>0)
                {
                    TpoFalta.Adiciona(txNvTipo.Text, null);
                    RetCmboTpo();
                }                
            }
        }

        private void RetCmboTpo()
        {
            txNvTipo.Text = "";
            txNvForn.Text = "";
            txNvTipo.Visible = false;
            txNvForn.Visible = false;
            cmbTipos.Visible = true;
            cmbForn.Visible = true;
            cmbTipos.DataSource = null;
            cmbForn.DataSource = null;
            MostraTipos(); 
            btAdicTpo.Text = "Atualizar";
            btAdicTpo.Enabled = false;
            btComprei.Text = "Comprei";
            btComprei.Enabled = false;
        }

    }

    public class CellInfo
    {
        public int RowIndexOriginal { get; set; } // Índice real do banco
        public int GridRowIndex { get; set; } // Índice da grid
        public int ColumnIndex { get; set; }
        public string CellValue { get; set; }
    }

}

