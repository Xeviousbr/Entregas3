﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio.rel
{
    public partial class RH : Form
    {

        private RHDAO cRHDAO;
        private bool ativou = false;
        internal DateTime setID1;
        private List<Lanctos> relcaixa { get; set; }
        public DateTime DT1 { get; set; }       
        public DateTime DT2 { get; set; }
        private List<string[]> gridDataParaImpressao;

        public RH()
        {
            InitializeComponent();
            SetStartPosition();
            rt.AdjustFormComponents(this);
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void Mostra()
        {
            DateTime DT1 = dtpDataIN.Value.Date;
            DateTime DT2 = dtnDtFim.Value.Date;
            int idFunc = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
            DataTable dados = cRHDAO.getDados(DT1, DT2, idFunc);

            relcaixa = new List<Lanctos>();
            TimeSpan totalzao = TimeSpan.Zero;

            foreach (DataRow row in dados.Rows)
            {
                Lanctos lancto = CriarLancto(row);
                TimeSpan totalDia = CalcularTotalDia(lancto);

                totalzao += totalDia;
                lancto.Total = FormatTotal(totalDia);
                relcaixa.Add(lancto);
            }

            GerarRelCaixa();
        }

        private Lanctos CriarLancto(DataRow row)
        {
            return new Lanctos
            {
                ID = Convert.ToInt32(row["ID"]),
                UID = row["uid"].ToString(),
                Data = Convert.ToDateTime(row["Data"]),
                Nome = row["Nome"].ToString(),
                InMan = row["InMan"].ToString(),
                FmMan = row["FmMan"].ToString(),
                InTrd = row["InTrd"].ToString(),
                FnTrd = row["FnTrd"].ToString(),
                InCafeMan = row["InCafeMan"].ToString(),
                FmCafeMan = row["FmCafeMan"].ToString(),
                InCafeTrd = row["InCafeTrd"].ToString(),
                FmCafeTrd = row["FmCafeTrd"].ToString(),
                FuncID = Convert.ToInt32(row["FuncID"])
            };
        }

        private TimeSpan CalcularTotalDia(Lanctos lancto)
        {
            TimeSpan inMan = ProcHora(lancto.InMan);
            TimeSpan fmMan = ProcHora(lancto.FmMan);
            TimeSpan inCafeMan = ProcHora(lancto.InCafeMan);
            TimeSpan fmCafeMan = ProcHora(lancto.FmCafeMan);
            TimeSpan inTrd = ProcHora(lancto.InTrd);
            TimeSpan fnTrd = ProcHora(lancto.FnTrd);
            TimeSpan inCafeTrd = ProcHora(lancto.InCafeTrd);
            TimeSpan fmCafeTrd = ProcHora(lancto.FmCafeTrd);

            TimeSpan totalDia = TimeSpan.Zero;

            totalDia += CalcularPeriodo(inMan, inCafeMan, fmMan);
            totalDia += CalcularPeriodo(fmCafeMan, fmMan, inCafeMan);
            totalDia += CalcularPeriodo(inTrd, inCafeTrd, fnTrd);
            totalDia += CalcularPeriodo(fmCafeTrd, fnTrd, inCafeTrd);

            return totalDia;
        }

        private TimeSpan CalcularPeriodo(TimeSpan inicio, TimeSpan meio, TimeSpan fim)
        {
            TimeSpan periodo = TimeSpan.Zero;

            if (inicio != TimeSpan.Zero && (meio != TimeSpan.Zero || fim != TimeSpan.Zero))
            {
                if (meio != TimeSpan.Zero)
                    periodo += meio - inicio;
                else
                    periodo += fim - inicio;
            }

            return periodo;
        }

        private string FormatTotal(TimeSpan totalDia)
        {
            int totalHoras = (int)totalDia.TotalHours;
            int totalMinutos = totalDia.Minutes;
            return $"{totalHoras:D2}:{totalMinutos:D2}";
        }
        
        private void GerarTextoParaTextBox(List<string[]> gridData)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var row in gridData)
            {
                sb.AppendLine(string.Join("\t", row));
            }
            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private TimeSpan ProcHora(string horaStr)
        {
            if (string.IsNullOrEmpty(horaStr))
                return TimeSpan.Zero;
            return TimeSpan.Parse(horaStr);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            Mostra();
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            ImprimirRelatorioComGrade(gridDataParaImpressao);
        }

        public void SetDados(int selectedIndex, DateTime value1, DateTime value2)
        {
            dtpDataIN.Value = value1;
            dtnDtFim.Value = value2;            
            VendedoresDAO Vendedor = new VendedoresDAO();
            glo.CarregarComboBox<tb.Vendedor>(cmbVendedor, Vendedor, "Selecione");
            cmbVendedor.SelectedIndex = selectedIndex;
            cRHDAO = new RHDAO();
        }

        private void RH_Activated_1(object sender, EventArgs e)
        {
            if (!ativou)
            {
                ativou = true;
                Mostra();                
            }
        }

        #region Desenho

        private void DesenharLinha(Graphics g, Font font, List<string[]> gridData, int i, float[] columnWidths, ref float currentX, float startY, float lineHeight, bool isDomingo, bool isFalta, int lastColumnIndex)
        {
            if (isDomingo)
            {
                DesenharDomingo(g, font, gridData, i, currentX, startY, lineHeight, columnWidths, lastColumnIndex);
                return;
            }
            for (int j = 0; j <= lastColumnIndex; j++)
            {
                RectangleF cellRect = new RectangleF(currentX, startY, columnWidths[j], lineHeight);
                g.DrawRectangle(Pens.Black, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                if (isFalta && j == 1)
                {
                    currentX = DesenharFalta(g, font, gridData, i, currentX, startY, lineHeight, columnWidths);
                }
                else if (gridData[i][j] == "FALTA" && j > 4)
                {
                    currentX = DesenharFaltaPeriodo(g, font, currentX, startY, lineHeight, columnWidths, "FALTA TARDE", j, 8);
                    RectangleF cellRectT = new RectangleF(currentX + columnWidths[4] + columnWidths[5] + columnWidths[6] + columnWidths[7], startY, columnWidths[9], lineHeight);
                    g.DrawRectangle(Pens.Black, cellRectT.X, cellRectT.Y, cellRectT.Width, cellRectT.Height);
                    DesenharTexto(g, font, gridData[i][9], cellRectT);
                    break;
                }
                else
                {
                    DesenharTexto(g, font, gridData[i][j], cellRect);
                    currentX += columnWidths[j];
                }
            }
        }

        private float DesenharFalta(Graphics g, Font font, List<string[]> gridData, int i, float currentX, float startY, float lineHeight, float[] columnWidths)
        {
            bool faltaManha = string.IsNullOrEmpty(gridData[i][2]);
            bool faltaTarde = string.IsNullOrEmpty(gridData[i][5]);

            if (faltaManha && faltaTarde)
            {
                return DesenharFaltaDiaInteiro(g, font, currentX, startY, lineHeight, columnWidths);
            }
            else if (faltaManha)
            {
                currentX = DesenharFaltaPeriodo(g, font, currentX, startY, lineHeight, columnWidths, "FALTA MANHÃ", 1, 4);
                // Draw afternoon hours
                for (int j = 5; j <= 8; j++)
                {
                    RectangleF cellRect = new RectangleF(currentX, startY, columnWidths[j], lineHeight);
                    g.DrawRectangle(Pens.Black, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    DesenharTexto(g, font, gridData[i][j], cellRect);
                    currentX += columnWidths[j];
                }
                RectangleF cellRectT = new RectangleF(currentX, startY, columnWidths[9], lineHeight);
                g.DrawRectangle(Pens.Black, cellRectT.X, cellRectT.Y, cellRectT.Width, cellRectT.Height);
                DesenharTexto(g, font, gridData[i][9], cellRectT);
                currentX += columnWidths[9];
                return currentX;
            }
            else if (faltaTarde)
            {
                // Draw morning hours
                for (int j = 1; j <= 4; j++)
                {
                    RectangleF cellRect = new RectangleF(currentX, startY, columnWidths[j], lineHeight);
                    g.DrawRectangle(Pens.Black, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    DesenharTexto(g, font, gridData[i][j], cellRect);
                    currentX += columnWidths[j];
                }
                return DesenharFaltaPeriodo(g, font, currentX, startY, lineHeight, columnWidths, "FALTA TARDE", 5, 8);
            }

            return currentX;
        }

        private float DesenharFaltaPeriodo(Graphics g, Font font, float currentX, float startY, float lineHeight, float[] columnWidths, string textoFalta, int startColIndex, int endColIndex)
        {
            float periodWidth = columnWidths.Skip(startColIndex).Take(endColIndex - startColIndex + 1).Sum();
            float MX = 0;
            float MY = 0;
            if (textoFalta == "FALTA TARDE")
            {
                MX = 2;
                MY = 2;
            }
            RectangleF faltaRect = new RectangleF(currentX+MX, startY+MY, periodWidth, lineHeight);
            g.FillRectangle(Brushes.White, faltaRect); 
            RectangleF faltaRectT = new RectangleF(currentX, startY, periodWidth, lineHeight);
            g.DrawString(textoFalta, font, Brushes.Black, faltaRectT, new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
            if (textoFalta == "FALTA TARDE")
            {
                return currentX;
            }
            else
            {
                return currentX + periodWidth;
            }
        }

        private void DesenharDomingo(Graphics g, Font font, List<string[]> gridData, int i, float currentX, float startY, float lineHeight, float[] columnWidths, int lastColumnIndex)
        {
            // Desenha a célula da data
            RectangleF dateRect = new RectangleF(currentX, startY, columnWidths[0], lineHeight);
            g.DrawRectangle(Pens.Black, dateRect.X, dateRect.Y, dateRect.Width, dateRect.Height);
            g.DrawString(gridData[i][0], font, Brushes.Black, dateRect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            // Desenha "DOMINGO" no restante da linha
            float domingoWidth = columnWidths.Skip(1).Take(lastColumnIndex).Sum();
            RectangleF domingoRect = new RectangleF(currentX + columnWidths[0], startY, domingoWidth, lineHeight);
            g.DrawRectangle(Pens.Black, domingoRect.X, domingoRect.Y, domingoRect.Width, domingoRect.Height);
            g.DrawString("DOMINGO", font, Brushes.Black, domingoRect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        private float DesenharFaltaDiaInteiro(Graphics g, Font font, float currentX, float startY, float lineHeight, float[] columnWidths)
        {
            float totalWidth = columnWidths.Sum() - columnWidths[0];
            RectangleF faltaRect = new RectangleF(currentX, startY, totalWidth, lineHeight);
            g.DrawString("FALTA DIA INTEIRO", font, Brushes.Black, faltaRect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            return currentX + totalWidth;
        }        

        private void DesenharTexto(Graphics g, Font font, string texto, RectangleF cellRect)
        {
            g.DrawString(texto, font, Brushes.Black, cellRect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        private void DesenharDados(Graphics g, Font font, List<string[]> gridData, int cabecalhoLinhas, float[] columnWidths, float startX, ref float startY, float lineHeight, PrintPageEventArgs e)
        {
            for (int i = (cabecalhoLinhas-1); i < gridData.Count; i++)
            {
                float currentX = startX;
                bool isDomingo = gridData[i].Length > 1 && gridData[i][1] == "DOMINGO";
                bool isFalta = gridData[i].Length > 1 && gridData[i][1] == "FALTA";

                int lastColumnIndex = isDomingo || isFalta ? 1 : gridData[i].Length - 1;

                DesenharLinha(g, font, gridData, i, columnWidths, ref currentX, startY, lineHeight, isDomingo, isFalta, lastColumnIndex);

                // Desenha o indicador de final de linha na última coluna válida
                DesenharFinalLinha(g, columnWidths, startX, currentX, startY, lineHeight, isDomingo, isFalta, lastColumnIndex);

                startY += lineHeight;

                if (startY > e.MarginBounds.Bottom - lineHeight)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
        }

        private void DesenharGrade(object sender, PrintPageEventArgs e, List<string[]> gridData, int cabecalhoLinhas)
        {
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 8);
            float lineHeight = font.GetHeight() + 2;  // Altura de cada linha de texto
            float startY = e.MarginBounds.Top;        // Ponto de início vertical
            float startX = e.MarginBounds.Left;       // Ponto de início horizontal
            float[] columnWidths = { 80, 100, 50, 50, 50, 50, 50, 50, 50, 60 }; // Larguras das colunas

            // Desenha o cabeçalho que não faz parte da grid de dados
            DesenharCabecalho(g, font, gridData, ref startY, startX, lineHeight, cabecalhoLinhas);

            // Adiciona um espaço extra entre o cabeçalho e a grade de dados
            startY += lineHeight * 2;  // Dobrar a linha de espaço para garantir separação

            // Desenha o cabeçalho dos campos da grid
            DesenharCabecalho2(g, font, columnWidths, startX, startY, lineHeight);

            // Aumentar startY após o cabeçalho da grid para iniciar os dados abaixo deste
            startY += lineHeight;

            // Desenha a grade de dados começando após o cabeçalho
            DesenharDados(g, font, gridData, cabecalhoLinhas, columnWidths, startX, ref startY, lineHeight, e);
        }

        private void DesenharCabecalho(Graphics g, Font font, List<string[]> gridData, ref float startY, float startX, float lineHeight, int cabecalhoLinhas)
        {
            for (int i = 0; i < (cabecalhoLinhas - 1); i++)
            {
                g.DrawString(gridData[i][0], font, Brushes.Black, startX, startY);
                startY += lineHeight;
            }
        }

        private void DesenharCabecalho2(Graphics g, Font font, float[] columnWidths, float startX, float startY, float lineHeight)
        {
            float currentX = startX;
            string[] cabecalhos = new string[] { "Data", "InMan", "FmMan", "InTrd", "FnTrd", "InCafeMan", "FmCafeMan", "InCafeTrd", "FmCafeTrd", "Total" };

            for (int i = 0; i < cabecalhos.Length; i++)
            {
                RectangleF cellRect = new RectangleF(currentX, startY, columnWidths[i], lineHeight);
                g.DrawRectangle(Pens.Black, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                DesenharTexto(g, font, cabecalhos[i], cellRect);
                currentX += columnWidths[i];
            }
        }


        private void DesenharFinalLinha(Graphics g, float[] columnWidths, float startX, float currentX, float startY, float lineHeight, bool isDomingo, bool isFalta, int lastColumnIndex)
        {
            if (!isDomingo && !isFalta)
            {
                RectangleF finalCellRect = new RectangleF(startX + columnWidths.Take(lastColumnIndex).Sum(), startY, columnWidths[lastColumnIndex], lineHeight);
                g.DrawRectangle(Pens.Black, finalCellRect.X, finalCellRect.Y, finalCellRect.Width, finalCellRect.Height);
            }
            else
            {
                RectangleF finalCellRect = new RectangleF(startX, startY, columnWidths.Sum(), lineHeight);
                g.DrawRectangle(Pens.Black, finalCellRect.X, finalCellRect.Y, finalCellRect.Width, finalCellRect.Height);
            }
        }

        #endregion

        #region Impressão 

        private void AdicionarLancamento(List<string[]> gridData, Lanctos lancto, ref TimeSpan totalMensal, ref TimeSpan totalzao)
        {
            string[] linha = new string[10];
            linha[0] = lancto.Data.ToString("dd/MM/yyyy");

            if (lancto.Nome == "DOMINGO")
            {
                linha[1] = "DOMINGO";
                for (int i = 2; i < 10; i++) linha[i] = "";
            }
            else
            {
                TimeSpan totalDoDia = CalcularTotalDoDia(lancto);

                bool faltaManha = string.IsNullOrEmpty(lancto.InMan) && string.IsNullOrEmpty(lancto.FmMan);
                bool faltaTarde = string.IsNullOrEmpty(lancto.InTrd) && string.IsNullOrEmpty(lancto.FnTrd);

                if (faltaManha && faltaTarde)
                {
                    linha[1] = "FALTA";
                    for (int i = 2; i < 9; i++) linha[i] = "";
                }
                else
                {
                    if (faltaManha)
                    {
                        linha[1] = "FALTA";
                        for (int i = 2; i <= 4; i++) linha[i] = "";
                    }
                    else
                    {
                        linha[1] = FormatarHora(lancto.InMan);
                        linha[2] = FormatarHora(lancto.InCafeMan);
                        linha[3] = FormatarHora(lancto.FmCafeMan);
                        linha[4] = FormatarHora(lancto.FmMan);
                    }

                    if (faltaTarde)
                    {
                        linha[5] = "FALTA";
                        for (int i = 6; i <= 8; i++) linha[i] = "";
                    }
                    else
                    {
                        linha[5] = FormatarHora(lancto.InTrd);
                        linha[6] = FormatarHora(lancto.InCafeTrd);
                        linha[7] = FormatarHora(lancto.FmCafeTrd);
                        linha[8] = FormatarHora(lancto.FnTrd);
                    }
                }
                linha[9] = FormatarTotalDoDia(totalDoDia);
                totalMensal += totalDoDia;
                totalzao += totalDoDia;
            }
            gridData.Add(linha);
        }

        public void GerarRelCaixa()
        {
            DateTime dataInicial = ObterDataInicial();
            List<string[]> gridData = new List<string[]>();
            List<string> cabecalho = GerarCabecalhoRelatorio(dataInicial);
            gridData.AddRange(cabecalho.Select(linha => new string[] { linha }));
            List<string[]> corpoGridData = new List<string[]>();
            TimeSpan totalzao = TimeSpan.Zero;
            TimeSpan totalMensal = TimeSpan.Zero;
            TimeSpan totalSemanal = TimeSpan.Zero;
            int currentMonth = -1;
            int currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dataInicial, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            bool isFirstMonth = true;
            List<Lanctos> relcaixaCompleto = CompletarRelCaixa(dataInicial);
            foreach (Lanctos lancto in relcaixaCompleto)
            {
                int weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(lancto.Data, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                if (weekOfYear != currentWeek)
                {
                    if (totalSemanal != TimeSpan.Zero)
                    {
                        corpoGridData.Add(new string[] { "", "", "", "", "", "", "", "Total da semana:", $"{totalSemanal.TotalHours:n0}:{totalSemanal.Minutes:00}" });
                        totalSemanal = TimeSpan.Zero;
                    }
                    currentWeek = weekOfYear;
                }
                if (currentMonth != lancto.Data.Month)
                {
                    if (currentMonth != -1)
                    {
                        AdicionarTotalMensal(corpoGridData, totalMensal);
                        totalMensal = TimeSpan.Zero;
                    }
                    currentMonth = lancto.Data.Month;
                    if (!isFirstMonth)
                    {
                        corpoGridData.Add(new string[] { "" });
                        corpoGridData.Add(new string[] { $"-- {lancto.Data:MMMM yyyy} --" });
                    }
                    isFirstMonth = false;
                }
                AdicionarLancamento(corpoGridData, lancto, ref totalMensal, ref totalzao);
                totalSemanal += CalcularTotalDoDia(lancto);
            }
            if (totalSemanal != TimeSpan.Zero)
            {
                corpoGridData.Add(new string[] { "", "", "", "", "", "", "", "", "Total da semana:", $"{totalSemanal.TotalHours:n0}:{totalSemanal.Minutes:00}" });
            }
            if (currentMonth != -1)
            {
                AdicionarTotalMensal(corpoGridData, totalMensal);
            }
            gridData.AddRange(corpoGridData);
            gridData.Add(new string[] { "Total de horas:", "", "", "", "", "", "", "", "", $"{totalzao.TotalHours:n0}:{totalzao.Minutes:00}" });
            GerarTextoParaTextBox(gridData);
            this.gridDataParaImpressao = gridData;
        }

        private List<string> GerarCabecalhoRelatorio(DateTime dataInicial)
        {
            INI cINI = new INI();
            List<string> cabecalho = new List<string>();
            cabecalho.Add(cINI.ReadString("Identidade", "Nome", "") + " - " + cINI.ReadString("Identidade", "CNPj", ""));
            cabecalho.Add(cINI.ReadString("Identidade", "Endereco", ""));
            cabecalho.Add(cINI.ReadString("Identidade", "Fone", ""));
            cabecalho.Add("Relatório de Horas Trabalhadas: " + cmbVendedor.Text);
            cabecalho.Add($"Período: {dataInicial:dd/MM/yyyy} a {dtnDtFim.Value:dd/MM/yyyy}");
            cabecalho.Add("");
            // cabecalho.Add("Data                          InMan            InCafe       FmCafe       FmMan        InTrd        InCafe       FmCafe       FnTrd       Total");
            return cabecalho;
        }
        private void ImprimirRelatorioComGrade(List<string[]> gridData)
        {
            int cabecalhoLinhas = 7; // Ajuste conforme necessário para o número de linhas do cabeçalho
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += (sender, e) => DesenharGrade(sender, e, gridData, cabecalhoLinhas);

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private List<Lanctos> CompletarRelCaixa(DateTime dataInicial)
        {
            List<Lanctos> relcaixaCompleto = new List<Lanctos>();
            DateTime dataAtual = dataInicial;
            while (dataAtual <= dtnDtFim.Value.Date)
            {
                Lanctos lanctoExistente = relcaixa.FirstOrDefault(l => l.Data.Date == dataAtual);
                if (lanctoExistente != null)
                {
                    relcaixaCompleto.Add(lanctoExistente);
                }
                else if (dataAtual.DayOfWeek == DayOfWeek.Sunday)
                {
                    relcaixaCompleto.Add(new Lanctos { Data = dataAtual, Nome = "DOMINGO" });
                }
                else
                {
                    relcaixaCompleto.Add(new Lanctos { Data = dataAtual, Nome = "FALTA" });
                }
                dataAtual = dataAtual.AddDays(1);
            }
            return relcaixaCompleto;
        }

        private void AdicionarTotalMensal(List<string[]> gridData, TimeSpan totalMensal)
        {
            gridData.Add(new string[] { "Total do mês:", "", "", "", "", "", "", "", "", $"{totalMensal.TotalHours:n0}:{totalMensal.Minutes:00}" });
        }

        private TimeSpan CalcularTotalDoDia(Lanctos lancto)
        {
            TimeSpan totalDoDia = TimeSpan.Zero;

            if (!string.IsNullOrEmpty(lancto.InMan) && !string.IsNullOrEmpty(lancto.FmMan))
            {
                totalDoDia += DateTime.Parse(lancto.FmMan) - DateTime.Parse(lancto.InMan);
            }

            if (!string.IsNullOrEmpty(lancto.InTrd) && !string.IsNullOrEmpty(lancto.FnTrd))
            {
                totalDoDia += DateTime.Parse(lancto.FnTrd) - DateTime.Parse(lancto.InTrd);
            }

            if (!string.IsNullOrEmpty(lancto.InCafeMan) && !string.IsNullOrEmpty(lancto.FmCafeMan))
            {
                totalDoDia -= DateTime.Parse(lancto.FmCafeMan) - DateTime.Parse(lancto.InCafeMan);
            }

            if (!string.IsNullOrEmpty(lancto.InCafeTrd) && !string.IsNullOrEmpty(lancto.FmCafeTrd))
            {
                totalDoDia -= DateTime.Parse(lancto.FmCafeTrd) - DateTime.Parse(lancto.InCafeTrd);
            }

            return totalDoDia;
        }

        private string FormatarHora(string hora)
        {
            return hora ?? "";
        }

        private string FormatarTotalDoDia(TimeSpan totalDoDia)
        {
            return $"{totalDoDia.TotalHours:n0}:{totalDoDia.Minutes:00}";
        }

        private DateTime ObterDataInicial()
        {
            DateTime primeiroLancamento = relcaixa.Min(l => l.Data);
            return new DateTime(Math.Max(dtpDataIN.Value.Ticks, primeiroLancamento.Ticks));
        }

        #endregion

        public class Lanctos
        {
            public int ID { get; set; }
            public string UID { get; set; }
            public DateTime Data { get; set; }
            public string Nome { get; set; }
            public string InMan { get; set; }
            public string FmMan { get; set; }
            public string InTrd { get; set; }
            public string FnTrd { get; set; }
            public string InCafeMan { get; set; }
            public string FmCafeMan { get; set; }
            public string InCafeTrd { get; set; }
            public string FmCafeTrd { get; set; }
            public int FuncID { get; set; }
            public string Total { get; set; }
        }

    }
}