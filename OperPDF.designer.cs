﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeleBonifacio
{
    partial class OperPDF
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperPDF));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btObter = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btEmail = new System.Windows.Forms.Button();
            this.btMudar = new System.Windows.Forms.Button();
            this.btPDF = new System.Windows.Forms.Button();
            this.ckPago = new System.Windows.Forms.CheckBox();
            this.btFiltrar = new System.Windows.Forms.Button();
            this.dtpDataPagamento = new System.Windows.Forms.DateTimePicker();
            this.dtpDataVencimento = new System.Windows.Forms.DateTimePicker();
            this.dtpDataEmissao = new System.Windows.Forms.DateTimePicker();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.btExcluir = new System.Windows.Forms.Button();
            this.btLimparFiltro = new System.Windows.Forms.Button();
            this.labelFornecedor = new System.Windows.Forms.Label();
            this.cmbForn = new System.Windows.Forms.ComboBox();
            this.labelDataEmissao = new System.Windows.Forms.Label();
            this.labelDataVencimento = new System.Windows.Forms.Label();
            this.labelValorTotal = new System.Windows.Forms.Label();
            this.txValorTotal = new System.Windows.Forms.TextBox();
            this.labelChaveNotaFiscal = new System.Windows.Forms.Label();
            this.txChaveNotaFiscal = new System.Windows.Forms.TextBox();
            this.labelDescricao = new System.Windows.Forms.Label();
            this.txDescricao = new System.Windows.Forms.TextBox();
            this.labelDataPagamento = new System.Windows.Forms.Label();
            this.labelObservacoes = new System.Windows.Forms.Label();
            this.txObservacoes = new System.Windows.Forms.TextBox();
            this.txNvForn = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btStripObter = new System.Windows.Forms.ToolStripMenuItem();
            this.renomearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apagarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btObter);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.btEmail);
            this.groupBox3.Controls.Add(this.btMudar);
            this.groupBox3.Controls.Add(this.btPDF);
            this.groupBox3.Controls.Add(this.ckPago);
            this.groupBox3.Controls.Add(this.btFiltrar);
            this.groupBox3.Controls.Add(this.dtpDataPagamento);
            this.groupBox3.Controls.Add(this.dtpDataVencimento);
            this.groupBox3.Controls.Add(this.dtpDataEmissao);
            this.groupBox3.Controls.Add(this.btnAdicionar);
            this.groupBox3.Controls.Add(this.btExcluir);
            this.groupBox3.Controls.Add(this.btLimparFiltro);
            this.groupBox3.Controls.Add(this.labelFornecedor);
            this.groupBox3.Controls.Add(this.cmbForn);
            this.groupBox3.Controls.Add(this.labelDataEmissao);
            this.groupBox3.Controls.Add(this.labelDataVencimento);
            this.groupBox3.Controls.Add(this.labelValorTotal);
            this.groupBox3.Controls.Add(this.txValorTotal);
            this.groupBox3.Controls.Add(this.labelChaveNotaFiscal);
            this.groupBox3.Controls.Add(this.txChaveNotaFiscal);
            this.groupBox3.Controls.Add(this.labelDescricao);
            this.groupBox3.Controls.Add(this.txDescricao);
            this.groupBox3.Controls.Add(this.labelDataPagamento);
            this.groupBox3.Controls.Add(this.labelObservacoes);
            this.groupBox3.Controls.Add(this.txObservacoes);
            this.groupBox3.Controls.Add(this.txNvForn);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(782, 166);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            // 
            // btObter
            // 
            this.btObter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btObter.Location = new System.Drawing.Point(61, 142);
            this.btObter.Name = "btObter";
            this.btObter.Size = new System.Drawing.Size(75, 23);
            this.btObter.TabIndex = 56;
            this.btObter.Text = "Obter";
            this.btObter.UseVisualStyleBackColor = true;
            this.btObter.Click += new System.EventHandler(this.btObter_Click_1);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.button1.Location = new System.Drawing.Point(7, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 55;
            this.button1.Text = "Versão Anterior";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btEmail
            // 
            this.btEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btEmail.Enabled = false;
            this.btEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btEmail.Location = new System.Drawing.Point(550, 143);
            this.btEmail.Name = "btEmail";
            this.btEmail.Size = new System.Drawing.Size(75, 23);
            this.btEmail.TabIndex = 54;
            this.btEmail.Text = "Email";
            this.btEmail.UseVisualStyleBackColor = true;
            this.btEmail.Click += new System.EventHandler(this.btEmail_Click);
            // 
            // btMudar
            // 
            this.btMudar.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btMudar.Enabled = false;
            this.btMudar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btMudar.Location = new System.Drawing.Point(223, 143);
            this.btMudar.Name = "btMudar";
            this.btMudar.Size = new System.Drawing.Size(75, 23);
            this.btMudar.TabIndex = 53;
            this.btMudar.Text = "Mudar";
            this.btMudar.UseVisualStyleBackColor = true;
            this.btMudar.Click += new System.EventHandler(this.btMudar_Click);
            // 
            // btPDF
            // 
            this.btPDF.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btPDF.Enabled = false;
            this.btPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btPDF.Location = new System.Drawing.Point(469, 143);
            this.btPDF.Name = "btPDF";
            this.btPDF.Size = new System.Drawing.Size(75, 23);
            this.btPDF.TabIndex = 49;
            this.btPDF.Text = "PDF";
            this.btPDF.UseVisualStyleBackColor = true;
            this.btPDF.Click += new System.EventHandler(this.btPDF_Click);
            // 
            // ckPago
            // 
            this.ckPago.AutoSize = true;
            this.ckPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ckPago.Location = new System.Drawing.Point(642, 86);
            this.ckPago.Name = "ckPago";
            this.ckPago.Size = new System.Drawing.Size(55, 19);
            this.ckPago.TabIndex = 48;
            this.ckPago.Text = "Pago";
            this.ckPago.UseVisualStyleBackColor = true;
            this.ckPago.CheckedChanged += new System.EventHandler(this.ckPago_CheckedChanged);
            // 
            // btFiltrar
            // 
            this.btFiltrar.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btFiltrar.Enabled = false;
            this.btFiltrar.Location = new System.Drawing.Point(307, 143);
            this.btFiltrar.Name = "btFiltrar";
            this.btFiltrar.Size = new System.Drawing.Size(75, 23);
            this.btFiltrar.TabIndex = 10;
            this.btFiltrar.Text = "Filtrar";
            this.btFiltrar.UseVisualStyleBackColor = true;
            this.btFiltrar.Click += new System.EventHandler(this.btFiltrar_Click);
            // 
            // dtpDataPagamento
            // 
            this.dtpDataPagamento.Enabled = false;
            this.dtpDataPagamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataPagamento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataPagamento.Location = new System.Drawing.Point(546, 80);
            this.dtpDataPagamento.Name = "dtpDataPagamento";
            this.dtpDataPagamento.Size = new System.Drawing.Size(90, 23);
            this.dtpDataPagamento.TabIndex = 6;
            this.dtpDataPagamento.Tag = "H";
            this.dtpDataPagamento.ValueChanged += new System.EventHandler(this.dtpDataPagamento_ValueChanged);
            // 
            // dtpDataVencimento
            // 
            this.dtpDataVencimento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataVencimento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataVencimento.Location = new System.Drawing.Point(546, 51);
            this.dtpDataVencimento.Name = "dtpDataVencimento";
            this.dtpDataVencimento.Size = new System.Drawing.Size(90, 23);
            this.dtpDataVencimento.TabIndex = 5;
            this.dtpDataVencimento.Tag = "H";
            this.dtpDataVencimento.ValueChanged += new System.EventHandler(this.dtpDataVencimento_ValueChanged);
            // 
            // dtpDataEmissao
            // 
            this.dtpDataEmissao.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataEmissao.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataEmissao.Location = new System.Drawing.Point(546, 19);
            this.dtpDataEmissao.Name = "dtpDataEmissao";
            this.dtpDataEmissao.Size = new System.Drawing.Size(90, 23);
            this.dtpDataEmissao.TabIndex = 4;
            this.dtpDataEmissao.Tag = "H";
            this.dtpDataEmissao.ValueChanged += new System.EventHandler(this.dtpDataEmissao_ValueChanged);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdicionar.Enabled = false;
            this.btnAdicionar.Location = new System.Drawing.Point(142, 142);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(75, 23);
            this.btnAdicionar.TabIndex = 9;
            this.btnAdicionar.Text = "Alterar";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click_1);
            // 
            // btExcluir
            // 
            this.btExcluir.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btExcluir.Enabled = false;
            this.btExcluir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btExcluir.Location = new System.Drawing.Point(631, 143);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(75, 23);
            this.btExcluir.TabIndex = 12;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.UseVisualStyleBackColor = true;
            this.btExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btLimparFiltro
            // 
            this.btLimparFiltro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btLimparFiltro.Enabled = false;
            this.btLimparFiltro.Location = new System.Drawing.Point(388, 143);
            this.btLimparFiltro.Name = "btLimparFiltro";
            this.btLimparFiltro.Size = new System.Drawing.Size(75, 23);
            this.btLimparFiltro.TabIndex = 11;
            this.btLimparFiltro.Text = "Limpar";
            this.btLimparFiltro.UseVisualStyleBackColor = true;
            this.btLimparFiltro.Click += new System.EventHandler(this.btLimparFiltro_Click);
            // 
            // labelFornecedor
            // 
            this.labelFornecedor.AutoSize = true;
            this.labelFornecedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelFornecedor.Location = new System.Drawing.Point(133, 23);
            this.labelFornecedor.Name = "labelFornecedor";
            this.labelFornecedor.Size = new System.Drawing.Size(78, 16);
            this.labelFornecedor.TabIndex = 47;
            this.labelFornecedor.Text = "Fornecedor";
            // 
            // cmbForn
            // 
            this.cmbForn.DisplayMember = "Nome";
            this.cmbForn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbForn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.cmbForn.Location = new System.Drawing.Point(220, 23);
            this.cmbForn.Name = "cmbForn";
            this.cmbForn.Size = new System.Drawing.Size(204, 23);
            this.cmbForn.TabIndex = 0;
            this.cmbForn.ValueMember = "Id";
            this.cmbForn.SelectedIndexChanged += new System.EventHandler(this.cmbForn_SelectedIndexChanged);
            // 
            // labelDataEmissao
            // 
            this.labelDataEmissao.AutoSize = true;
            this.labelDataEmissao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelDataEmissao.Location = new System.Drawing.Point(447, 26);
            this.labelDataEmissao.Name = "labelDataEmissao";
            this.labelDataEmissao.Size = new System.Drawing.Size(93, 16);
            this.labelDataEmissao.TabIndex = 47;
            this.labelDataEmissao.Text = "Data Emissão";
            // 
            // labelDataVencimento
            // 
            this.labelDataVencimento.AutoSize = true;
            this.labelDataVencimento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelDataVencimento.Location = new System.Drawing.Point(430, 55);
            this.labelDataVencimento.Name = "labelDataVencimento";
            this.labelDataVencimento.Size = new System.Drawing.Size(111, 16);
            this.labelDataVencimento.TabIndex = 47;
            this.labelDataVencimento.Text = "Data Vencimento";
            // 
            // labelValorTotal
            // 
            this.labelValorTotal.AutoSize = true;
            this.labelValorTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelValorTotal.Location = new System.Drawing.Point(466, 110);
            this.labelValorTotal.Name = "labelValorTotal";
            this.labelValorTotal.Size = new System.Drawing.Size(74, 16);
            this.labelValorTotal.TabIndex = 47;
            this.labelValorTotal.Text = "Valor Total";
            // 
            // txValorTotal
            // 
            this.txValorTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txValorTotal.Location = new System.Drawing.Point(546, 110);
            this.txValorTotal.Name = "txValorTotal";
            this.txValorTotal.Size = new System.Drawing.Size(90, 21);
            this.txValorTotal.TabIndex = 8;
            this.txValorTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txValorTotal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txValorTotal_KeyUp);
            // 
            // labelChaveNotaFiscal
            // 
            this.labelChaveNotaFiscal.AutoSize = true;
            this.labelChaveNotaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelChaveNotaFiscal.Location = new System.Drawing.Point(100, 55);
            this.labelChaveNotaFiscal.Name = "labelChaveNotaFiscal";
            this.labelChaveNotaFiscal.Size = new System.Drawing.Size(118, 16);
            this.labelChaveNotaFiscal.TabIndex = 47;
            this.labelChaveNotaFiscal.Text = "Chave Nota Fiscal";
            // 
            // txChaveNotaFiscal
            // 
            this.txChaveNotaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txChaveNotaFiscal.Location = new System.Drawing.Point(220, 53);
            this.txChaveNotaFiscal.Name = "txChaveNotaFiscal";
            this.txChaveNotaFiscal.Size = new System.Drawing.Size(204, 21);
            this.txChaveNotaFiscal.TabIndex = 1;
            this.txChaveNotaFiscal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txChaveNotaFiscal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txChaveNotaFiscal_KeyUp);
            // 
            // labelDescricao
            // 
            this.labelDescricao.AutoSize = true;
            this.labelDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelDescricao.Location = new System.Drawing.Point(150, 85);
            this.labelDescricao.Name = "labelDescricao";
            this.labelDescricao.Size = new System.Drawing.Size(70, 16);
            this.labelDescricao.TabIndex = 47;
            this.labelDescricao.Text = "Descrição";
            // 
            // txDescricao
            // 
            this.txDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txDescricao.Location = new System.Drawing.Point(220, 83);
            this.txDescricao.Name = "txDescricao";
            this.txDescricao.Size = new System.Drawing.Size(204, 21);
            this.txDescricao.TabIndex = 2;
            this.txDescricao.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txDescricao.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txDescricao_KeyUp);
            // 
            // labelDataPagamento
            // 
            this.labelDataPagamento.AutoSize = true;
            this.labelDataPagamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelDataPagamento.Location = new System.Drawing.Point(430, 85);
            this.labelDataPagamento.Name = "labelDataPagamento";
            this.labelDataPagamento.Size = new System.Drawing.Size(110, 16);
            this.labelDataPagamento.TabIndex = 47;
            this.labelDataPagamento.Text = "Data Pagamento";
            // 
            // labelObservacoes
            // 
            this.labelObservacoes.AutoSize = true;
            this.labelObservacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelObservacoes.Location = new System.Drawing.Point(130, 112);
            this.labelObservacoes.Name = "labelObservacoes";
            this.labelObservacoes.Size = new System.Drawing.Size(90, 16);
            this.labelObservacoes.TabIndex = 47;
            this.labelObservacoes.Text = "Observações";
            // 
            // txObservacoes
            // 
            this.txObservacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txObservacoes.Location = new System.Drawing.Point(220, 110);
            this.txObservacoes.Name = "txObservacoes";
            this.txObservacoes.Size = new System.Drawing.Size(204, 21);
            this.txObservacoes.TabIndex = 3;
            this.txObservacoes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txObservacoes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txObservacoes_KeyUp);
            // 
            // txNvForn
            // 
            this.txNvForn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txNvForn.Location = new System.Drawing.Point(220, 23);
            this.txNvForn.Name = "txNvForn";
            this.txNvForn.Size = new System.Drawing.Size(204, 21);
            this.txNvForn.TabIndex = 51;
            this.txNvForn.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 181);
            this.panel1.TabIndex = 13;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(0, 181);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(782, 251);
            this.treeView1.TabIndex = 14;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btStripObter,
            this.renomearToolStripMenuItem,
            this.apagarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(129, 70);
            // 
            // btStripObter
            // 
            this.btStripObter.Name = "btStripObter";
            this.btStripObter.Size = new System.Drawing.Size(128, 22);
            this.btStripObter.Text = "Nova";
            this.btStripObter.Click += new System.EventHandler(this.btStripObter_Click);
            // 
            // renomearToolStripMenuItem
            // 
            this.renomearToolStripMenuItem.Name = "renomearToolStripMenuItem";
            this.renomearToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.renomearToolStripMenuItem.Text = "Renomear";
            this.renomearToolStripMenuItem.Click += new System.EventHandler(this.renomearToolStripMenuItem_Click);
            // 
            // apagarToolStripMenuItem
            // 
            this.apagarToolStripMenuItem.Name = "apagarToolStripMenuItem";
            this.apagarToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.apagarToolStripMenuItem.Text = "Apagar";
            this.apagarToolStripMenuItem.Click += new System.EventHandler(this.apagarToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.treeView2);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Location = new System.Drawing.Point(245, 206);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(271, 214);
            this.panel2.TabIndex = 15;
            this.panel2.Visible = false;
            // 
            // treeView2
            // 
            this.treeView2.Location = new System.Drawing.Point(14, 14);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(237, 156);
            this.treeView2.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(176, 176);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Fechar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 176);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OperPDF
            // 
            this.ClientSize = new System.Drawing.Size(782, 432);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OperPDF";
            this.Text = "PDFs";
            this.Load += new System.EventHandler(this.OperPagar_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox3;
        private Button btFiltrar;
        private DateTimePicker dtpDataPagamento;
        private DateTimePicker dtpDataVencimento;
        private DateTimePicker dtpDataEmissao;
        private Button btnAdicionar;
        private Button btExcluir;
        private Button btLimparFiltro;
        private Label labelFornecedor;
        private ComboBox cmbForn;
        private Label labelDataEmissao;
        private Label labelDataVencimento;
        private Label labelValorTotal;
        private TextBox txValorTotal;
        private Label labelChaveNotaFiscal;
        private TextBox txChaveNotaFiscal;
        private Label labelDescricao;
        private TextBox txDescricao;
        private Label labelDataPagamento;
        private Label labelObservacoes;
        private TextBox txObservacoes;
        private Panel panel1;
        private CheckBox ckPago;
        private OpenFileDialog openFileDialog;
        private Button btPDF;
        private TextBox txNvForn;
        private Button btMudar;
        private Button btEmail;
        private Button button1;
        private TreeView treeView1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem btStripObter;
        private Button btObter;
        private Panel panel2;
        private TreeView treeView2;
        private Button button3;
        private Button button2;
        private ToolStripMenuItem renomearToolStripMenuItem;
        private ToolStripMenuItem apagarToolStripMenuItem;
    }
}