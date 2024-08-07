﻿using System;
using System.Data;

namespace TeleBonifacio.dao
{
    public class ContasAPagarDao
    {        

        public int AdicObter(bool perm, DateTime dataEmissao, string NmArq, string UID)
        {
            string sql = $@"INSERT INTO ContasAPagar (Perm, DataEmissao, CaminhoPDF, UID) VALUES (
                {(perm ? 1 : 0)},
                '{dataEmissao.ToString("yyyy-MM-dd HH:mm:ss")}', 
                '{NmArq}',
                '{UID}' )";
            DB.ExecutarComandoSQL(sql);
            string queryNome = $"SELECT Max(ID) FROM ContasAPagar ";
            return DB.ExecutarConsultaCount(queryNome);
        }

        public void Exclui(string id, string CaminhoPDF)
        {
            string sql = $@"DELETE FROM ContasAPagar WHERE ID = {id} ";
            DB.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int idFornecedor, DateTime dataEmissao, DateTime dataVencimento, float valorTotal, string chaveNotaFiscal, string Descricao, bool pago, DateTime? dataPagamento, string observacoes)
        {
            string sVenc = "";
            if (dataVencimento!=DateTime.MinValue)
            {
                sVenc = $@"DataVencimento = '{dataVencimento.ToString("yyyy -MM-dd HH:mm:ss")}', ";
            }
            string sql = $@"UPDATE ContasAPagar SET 
                idFornecedor = {idFornecedor}, 
                DataEmissao = '{dataEmissao.ToString("yyyy-MM-dd HH:mm:ss")}', 
                {sVenc}
                ValorTotal = {glo.sv(valorTotal)}, 
                ChaveNotaFiscal = '{chaveNotaFiscal}', 
                Pago = {(pago ? 1 : 0)}, 
                DataPagamento = {(dataPagamento.HasValue ? $"'{dataPagamento.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL")}, 
                Observacoes = '{observacoes}',
                Descricao = '{Descricao}' 
                WHERE ID = {id}";
            Console.WriteLine(sql);
            DB.ExecutarComandoSQL(sql);
        }

        public void MudaFornecedor(int id, int idFornecedor)
        {
            string sql = $@"UPDATE ContasAPagar SET 
                idFornecedor = {idFornecedor} 
                WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public DataTable GetDados(bool Perm, int idFornecedor, DateTime? dataPagamento, DateTime? dataVencimento, DateTime? dataEmissao, string valorTotal, string descricao, string observacoes, bool? pago)
        {
            string sWhe = " WHERE ContasAPagar.Perm = " + (Perm ? "True" : "False");
            if (idFornecedor>0)
            {
                sWhe += " And ContasAPagar.idFornecedor = " + idFornecedor;
            }
            if (dataPagamento.HasValue)
            {
                sWhe += " And ContasAPagar.DataPagamento = #" + dataPagamento.Value.ToString("yyyy-MM-dd") + "#";
            }
            if (dataVencimento.HasValue)
            {
                sWhe += " And ContasAPagar.DataVencimento = #" + dataVencimento.Value.ToString("yyyy-MM-dd") + "#";
            }
            if (dataEmissao.HasValue)
            {
                sWhe += " And ContasAPagar.DataEmissao = #" + dataEmissao.Value.ToString("yyyy-MM-dd") + "#";
            }
            if (!string.IsNullOrEmpty(valorTotal))
            {
                sWhe += " And ContasAPagar.ValorTotal LIKE '%" + valorTotal + "%'";
            }
            if (!string.IsNullOrEmpty(descricao))
            {
                sWhe += " And ContasAPagar.Descricao LIKE '%" + descricao + "%'";
            }
            if (!string.IsNullOrEmpty(observacoes))
            {
                sWhe += " And ContasAPagar.Observacoes LIKE '%" + observacoes + "%'";
            }
            if (pago.HasValue)
            {
                sWhe += " And ContasAPagar.Pago = " + (pago.Value ? "True" : "False");
            }
            string sql = $@"SELECT ContasAPagar.CaminhoPDF as Arquivo, ContasAPagar.ID, ContasAPagar.idFornecedor, Fornecedores.Nome as Fornecedor, ContasAPagar.DataEmissao,
                            ContasAPagar.DataVencimento, ContasAPagar.ValorTotal, ContasAPagar.ChaveNotaFiscal, ContasAPagar.Descricao, 
                            ContasAPagar.CaminhoPDF, ContasAPagar.Pago, ContasAPagar.DataPagamento, ContasAPagar.Observacoes, 
                            ContasAPagar.Perm, ContasAPagar.UID, ContasAPagar.idArquivo 
                    FROM ContasAPagar                            
                    LEFT JOIN Fornecedores ON Fornecedores.IdForn = ContasAPagar.idFornecedor
                    {sWhe} 
                    ORDER BY ContasAPagar.ID DESC";
            DataTable dt = DB.ExecutarConsulta(sql);
            Console.WriteLine("Trouxe dados da tabela de PDFs");
            return dt;
        }

        public void Muda(int iID, bool v)
        {
            string sql = $@"UPDATE ContasAPagar SET Perm = {(v ? 1 : 0)} WHERE ID = {iID}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Imprimiu(int iID)
        {
            string sql = $@"UPDATE ContasAPagar SET idArquivo = 1 WHERE ID = {iID} ";
            DB.ExecutarComandoSQL(sql);
        }
    }
}
