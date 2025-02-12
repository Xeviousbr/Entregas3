﻿
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeleBonifacio
{
    partial class OperFalta
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>-
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperFalta));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btGarantia = new System.Windows.Forms.Button();
            this.btEncomenda = new System.Windows.Forms.Button();
            this.ckEmFalta = new System.Windows.Forms.CheckBox();
            this.btComprei = new System.Windows.Forms.Button();
            this.cmbForn = new System.Windows.Forms.ComboBox();
            this.btAdicTpo = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btLmpFiltro = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txNvForn = new System.Windows.Forms.TextBox();
            this.cmbTipos = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txNvTipo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txValor = new System.Windows.Forms.TextBox();
            this.lbVlor = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txObs = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txDescr = new System.Windows.Forms.TextBox();
            this.cmbVendedor = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txMarca = new System.Windows.Forms.TextBox();
            this.txQuantidade = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.tbFaltas = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGrid2 = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGrid3 = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGrid4 = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.rtfTexto = new AtcCtrl.ATCRTF();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.griTaxas = new System.Windows.Forms.DataGridView();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.rtfWord = new AtcCtrl.ATCRTF();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Aumentar = new System.Windows.Forms.ToolStripMenuItem();
            this.DiminirPrio = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tbFaltas.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid3)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid4)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.griTaxas)).BeginInit();
            this.tabPage7.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1345, 108);
            this.panel1.TabIndex = 13;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btGarantia);
            this.groupBox3.Controls.Add(this.btEncomenda);
            this.groupBox3.Controls.Add(this.ckEmFalta);
            this.groupBox3.Controls.Add(this.btComprei);
            this.groupBox3.Controls.Add(this.cmbForn);
            this.groupBox3.Controls.Add(this.btAdicTpo);
            this.groupBox3.Controls.Add(this.btnExcluir);
            this.groupBox3.Controls.Add(this.btLmpFiltro);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txNvForn);
            this.groupBox3.Controls.Add(this.cmbTipos);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txNvTipo);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 50);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1345, 50);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filtro";
            // 
            // btGarantia
            // 
            this.btGarantia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btGarantia.Location = new System.Drawing.Point(885, 24);
            this.btGarantia.Name = "btGarantia";
            this.btGarantia.Size = new System.Drawing.Size(75, 23);
            this.btGarantia.TabIndex = 55;
            this.btGarantia.Text = "Garantia";
            this.btGarantia.Click += new System.EventHandler(this.btGarantia_Click);
            // 
            // btEncomenda
            // 
            this.btEncomenda.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btEncomenda.Location = new System.Drawing.Point(783, 24);
            this.btEncomenda.Name = "btEncomenda";
            this.btEncomenda.Size = new System.Drawing.Size(82, 23);
            this.btEncomenda.TabIndex = 54;
            this.btEncomenda.Text = "Encomenda";
            this.btEncomenda.Click += new System.EventHandler(this.btEncomenda_Click);
            // 
            // ckEmFalta
            // 
            this.ckEmFalta.AutoSize = true;
            this.ckEmFalta.Location = new System.Drawing.Point(1180, 25);
            this.ckEmFalta.Name = "ckEmFalta";
            this.ckEmFalta.Size = new System.Drawing.Size(67, 17);
            this.ckEmFalta.TabIndex = 49;
            this.ckEmFalta.Text = "Em Falta";
            this.ckEmFalta.UseVisualStyleBackColor = true;
            this.ckEmFalta.Click += new System.EventHandler(this.ckEmFalta_Click);
            // 
            // btComprei
            // 
            this.btComprei.Enabled = false;
            this.btComprei.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btComprei.Location = new System.Drawing.Point(1075, 24);
            this.btComprei.Name = "btComprei";
            this.btComprei.Size = new System.Drawing.Size(75, 23);
            this.btComprei.TabIndex = 48;
            this.btComprei.Text = "Comprei";
            this.btComprei.Click += new System.EventHandler(this.btComprei_Click);
            // 
            // cmbForn
            // 
            this.cmbForn.DisplayMember = "Nome";
            this.cmbForn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbForn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbForn.Location = new System.Drawing.Point(288, 24);
            this.cmbForn.Name = "cmbForn";
            this.cmbForn.Size = new System.Drawing.Size(204, 23);
            this.cmbForn.TabIndex = 49;
            this.cmbForn.ValueMember = "Id";
            this.cmbForn.SelectedIndexChanged += new System.EventHandler(this.cmbForn_SelectedIndexChanged);
            // 
            // btAdicTpo
            // 
            this.btAdicTpo.Enabled = false;
            this.btAdicTpo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAdicTpo.Location = new System.Drawing.Point(688, 24);
            this.btAdicTpo.Name = "btAdicTpo";
            this.btAdicTpo.Size = new System.Drawing.Size(75, 23);
            this.btAdicTpo.TabIndex = 42;
            this.btAdicTpo.Text = "Atualizar";
            this.btAdicTpo.Click += new System.EventHandler(this.btAdicTpo_Click);
            // 
            // btnExcluir
            // 
            this.btnExcluir.Enabled = false;
            this.btnExcluir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcluir.Location = new System.Drawing.Point(980, 24);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(75, 23);
            this.btnExcluir.TabIndex = 39;
            this.btnExcluir.Text = "Excluir";
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btLmpFiltro
            // 
            this.btLmpFiltro.Enabled = false;
            this.btLmpFiltro.Location = new System.Drawing.Point(593, 24);
            this.btLmpFiltro.Name = "btLmpFiltro";
            this.btLmpFiltro.Size = new System.Drawing.Size(75, 23);
            this.btLmpFiltro.TabIndex = 53;
            this.btLmpFiltro.Text = "Limpar";
            this.btLmpFiltro.Click += new System.EventHandler(this.btLmpFiltro_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(247, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 16);
            this.label5.TabIndex = 47;
            this.label5.Text = "Forn";
            // 
            // txNvForn
            // 
            this.txNvForn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txNvForn.Location = new System.Drawing.Point(288, 24);
            this.txNvForn.Name = "txNvForn";
            this.txNvForn.Size = new System.Drawing.Size(204, 21);
            this.txNvForn.TabIndex = 50;
            this.txNvForn.Visible = false;
            // 
            // cmbTipos
            // 
            this.cmbTipos.DisplayMember = "Nome";
            this.cmbTipos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipos.Location = new System.Drawing.Point(41, 24);
            this.cmbTipos.Name = "cmbTipos";
            this.cmbTipos.Size = new System.Drawing.Size(200, 23);
            this.cmbTipos.TabIndex = 44;
            this.cmbTipos.ValueMember = "Id";
            this.cmbTipos.SelectedIndexChanged += new System.EventHandler(this.cmbTipos_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(498, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 44;
            this.button2.Text = "Filtrar";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 45;
            this.label3.Text = "Tipo";
            // 
            // txNvTipo
            // 
            this.txNvTipo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txNvTipo.Location = new System.Drawing.Point(41, 24);
            this.txNvTipo.Name = "txNvTipo";
            this.txNvTipo.Size = new System.Drawing.Size(200, 21);
            this.txNvTipo.TabIndex = 45;
            this.txNvTipo.Visible = false;
            this.txNvTipo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txNvTipo_KeyUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txValor);
            this.groupBox1.Controls.Add(this.lbVlor);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txObs);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txDescr);
            this.groupBox1.Controls.Add(this.cmbVendedor);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txMarca);
            this.groupBox1.Controls.Add(this.txQuantidade);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtCodigo);
            this.groupBox1.Controls.Add(this.btnAdicionar);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1345, 50);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Adição";
            // 
            // txValor
            // 
            this.txValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txValor.Location = new System.Drawing.Point(905, 20);
            this.txValor.Name = "txValor";
            this.txValor.Size = new System.Drawing.Size(62, 21);
            this.txValor.TabIndex = 5;
            this.txValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txValor.Visible = false;
            this.txValor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txValor_KeyUp);
            // 
            // lbVlor
            // 
            this.lbVlor.AutoSize = true;
            this.lbVlor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVlor.Location = new System.Drawing.Point(877, 23);
            this.lbVlor.Name = "lbVlor";
            this.lbVlor.Size = new System.Drawing.Size(27, 16);
            this.lbVlor.TabIndex = 49;
            this.lbVlor.Text = "Vlr.";
            this.lbVlor.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(716, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 16);
            this.label10.TabIndex = 48;
            this.label10.Text = "Obs.";
            // 
            // txObs
            // 
            this.txObs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txObs.Location = new System.Drawing.Point(758, 21);
            this.txObs.Name = "txObs";
            this.txObs.Size = new System.Drawing.Size(113, 21);
            this.txObs.TabIndex = 4;
            this.txObs.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txObs_KeyUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(409, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 16);
            this.label8.TabIndex = 46;
            this.label8.Text = "Descr,";
            // 
            // txDescr
            // 
            this.txDescr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txDescr.Location = new System.Drawing.Point(461, 20);
            this.txDescr.Name = "txDescr";
            this.txDescr.Size = new System.Drawing.Size(250, 21);
            this.txDescr.TabIndex = 3;
            this.txDescr.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txDescr_KeyUp);
            // 
            // cmbVendedor
            // 
            this.cmbVendedor.DisplayMember = "Nome";
            this.cmbVendedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVendedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbVendedor.Location = new System.Drawing.Point(1053, 18);
            this.cmbVendedor.Name = "cmbVendedor";
            this.cmbVendedor.Size = new System.Drawing.Size(101, 23);
            this.cmbVendedor.TabIndex = 6;
            this.cmbVendedor.ValueMember = "Id";
            this.cmbVendedor.SelectedIndexChanged += new System.EventHandler(this.cmbVendedor_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(978, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 16);
            this.label7.TabIndex = 44;
            this.label7.Text = "Vendedor";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(249, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 16);
            this.label6.TabIndex = 43;
            this.label6.Text = "Marca";
            // 
            // txMarca
            // 
            this.txMarca.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txMarca.Location = new System.Drawing.Point(301, 19);
            this.txMarca.Name = "txMarca";
            this.txMarca.Size = new System.Drawing.Size(101, 21);
            this.txMarca.TabIndex = 2;
            this.txMarca.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txMarca_KeyUp);
            // 
            // txQuantidade
            // 
            this.txQuantidade.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txQuantidade.Location = new System.Drawing.Point(203, 19);
            this.txQuantidade.Name = "txQuantidade";
            this.txQuantidade.Size = new System.Drawing.Size(40, 21);
            this.txQuantidade.TabIndex = 1;
            this.txQuantidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txQuantidade.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txQuantidade_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(162, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 16);
            this.label4.TabIndex = 42;
            this.label4.Text = "Quant.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 16);
            this.label2.TabIndex = 41;
            this.label2.Text = "Código";
            this.label2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label2_MouseUp);
            // 
            // txtCodigo
            // 
            this.txtCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigo.Location = new System.Drawing.Point(69, 19);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(87, 21);
            this.txtCodigo.TabIndex = 0;
            this.txtCodigo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCodigo_KeyUp);
            this.txtCodigo.Leave += new System.EventHandler(this.txtCodigo_Leave);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Enabled = false;
            this.btnAdicionar.Location = new System.Drawing.Point(1160, 19);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(75, 23);
            this.btnAdicionar.TabIndex = 7;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // tbFaltas
            // 
            this.tbFaltas.Controls.Add(this.tabPage1);
            this.tbFaltas.Controls.Add(this.tabPage2);
            this.tbFaltas.Controls.Add(this.tabPage3);
            this.tbFaltas.Controls.Add(this.tabPage4);
            this.tbFaltas.Controls.Add(this.tabPage5);
            this.tbFaltas.Controls.Add(this.tabPage6);
            this.tbFaltas.Controls.Add(this.tabPage7);
            this.tbFaltas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFaltas.Location = new System.Drawing.Point(0, 108);
            this.tbFaltas.Name = "tbFaltas";
            this.tbFaltas.SelectedIndex = 0;
            this.tbFaltas.Size = new System.Drawing.Size(1345, 324);
            this.tbFaltas.TabIndex = 14;
            this.tbFaltas.SelectedIndexChanged += new System.EventHandler(this.tbFaltas_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1337, 298);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Faltas";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowUserToAddRows = false;
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.Location = new System.Drawing.Point(3, 3);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid1.Size = new System.Drawing.Size(1331, 292);
            this.dataGrid1.TabIndex = 1;
            this.dataGrid1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid1_CellClick_1);
            this.dataGrid1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGrid2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1337, 298);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Produtos";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGrid2
            // 
            this.dataGrid2.AllowUserToAddRows = false;
            this.dataGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid2.Location = new System.Drawing.Point(3, 3);
            this.dataGrid2.Name = "dataGrid2";
            this.dataGrid2.ReadOnly = true;
            this.dataGrid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid2.Size = new System.Drawing.Size(1331, 292);
            this.dataGrid2.TabIndex = 16;
            this.dataGrid2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid2_CellClick);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGrid3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1337, 298);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Encomenda";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGrid3
            // 
            this.dataGrid3.AllowUserToAddRows = false;
            this.dataGrid3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid3.Location = new System.Drawing.Point(0, 0);
            this.dataGrid3.Name = "dataGrid3";
            this.dataGrid3.ReadOnly = true;
            this.dataGrid3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid3.Size = new System.Drawing.Size(1337, 298);
            this.dataGrid3.TabIndex = 17;
            this.dataGrid3.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid3_CellClick);
            this.dataGrid3.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid3_CellDoubleClick);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGrid4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1337, 298);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Garantia";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGrid4
            // 
            this.dataGrid4.AllowUserToAddRows = false;
            this.dataGrid4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid4.Location = new System.Drawing.Point(0, 0);
            this.dataGrid4.Name = "dataGrid4";
            this.dataGrid4.ReadOnly = true;
            this.dataGrid4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid4.Size = new System.Drawing.Size(1337, 298);
            this.dataGrid4.TabIndex = 18;
            this.dataGrid4.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid4_CellClick);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.rtfTexto);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1337, 298);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Anotações";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // rtfTexto
            // 
            this.rtfTexto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfTexto.Location = new System.Drawing.Point(0, 0);
            this.rtfTexto.Name = "rtfTexto";
            this.rtfTexto.Size = new System.Drawing.Size(1337, 298);
            this.rtfTexto.TabIndex = 0;
            this.rtfTexto.VlrPerImr = 1F;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.griTaxas);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(1337, 298);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Taxas";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // griTaxas
            // 
            this.griTaxas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.griTaxas.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.griTaxas.DefaultCellStyle = dataGridViewCellStyle1;
            this.griTaxas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.griTaxas.Location = new System.Drawing.Point(0, 0);
            this.griTaxas.Name = "griTaxas";
            this.griTaxas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.griTaxas.Size = new System.Drawing.Size(1337, 298);
            this.griTaxas.TabIndex = 0;
            this.griTaxas.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.griTaxas_CellEndEdit);
            this.griTaxas.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.griTaxas_ColumnWidthChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.rtfWord);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(1337, 298);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Word";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // rtfWord
            // 
            this.rtfWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfWord.Location = new System.Drawing.Point(3, 3);
            this.rtfWord.Name = "rtfWord";
            this.rtfWord.Size = new System.Drawing.Size(1331, 292);
            this.rtfWord.TabIndex = 0;
            this.rtfWord.VlrPerImr = 1F;
            this.rtfWord.VlrPerImrChanged += new System.EventHandler<bool>(this.rtfWord_VlrPerImrChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Aumentar,
            this.DiminirPrio});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(235, 56);
            // 
            // Aumentar
            // 
            this.Aumentar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Aumentar.Name = "Aumentar";
            this.Aumentar.Size = new System.Drawing.Size(234, 26);
            this.Aumentar.Text = "Aumentar a prioridade";
            this.Aumentar.Click += new System.EventHandler(this.Aumentar_Click);
            // 
            // DiminirPrio
            // 
            this.DiminirPrio.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiminirPrio.Name = "DiminirPrio";
            this.DiminirPrio.Size = new System.Drawing.Size(234, 26);
            this.DiminirPrio.Text = "Diminuir a prioridade";
            this.DiminirPrio.Click += new System.EventHandler(this.DiminirPrio_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // OperFalta
            // 
            this.ClientSize = new System.Drawing.Size(1345, 432);
            this.Controls.Add(this.tbFaltas);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OperFalta";
            this.Text = "Produtos em Falta";
            //this.Activated += new System.EventHandler(this.OperFalta_Activated);
            this.Load += new System.EventHandler(this.OperFalta_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tbFaltas.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid3)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid4)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.griTaxas)).EndInit();
            this.tabPage7.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cmbTipos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btAdicTpo;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbVendedor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txMarca;
        private System.Windows.Forms.TextBox txQuantidade;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.TextBox txNvTipo;
        private System.Windows.Forms.Button btComprei;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txDescr;
        private System.Windows.Forms.ComboBox cmbForn;
        private System.Windows.Forms.TextBox txNvForn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txObs;
        private System.Windows.Forms.Button btLmpFiltro;
        private System.Windows.Forms.TabControl tbFaltas;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGrid1;
        private System.Windows.Forms.DataGridView dataGrid2;
        private System.Windows.Forms.CheckBox ckEmFalta;
        private System.Windows.Forms.Button btEncomenda;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGrid3;
        private System.Windows.Forms.Button btGarantia;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dataGrid4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Timer timer1;
        private TextBox txValor;
        private Label lbVlor;
        private AtcCtrl.ATCRTF rtfTexto;
        private AtcCtrl.ATCRTF rtfWord;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem Aumentar;
        private ToolStripMenuItem DiminirPrio;
        private TabPage tabPage6;
        private DataGridView griTaxas;
        private OpenFileDialog openFileDialog1;
        private TabPage tabPage7;
        //private AtcCtrl.ATCRTF rtfTexto;
    }
}

