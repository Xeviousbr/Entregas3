﻿using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace TeleBonifacio.dao
{
    public class CaixaDao
    {
        public void Adiciona(int idForma, float compra, int idCliente, string obs, float desc, int idVend, string UID)
        {
            String sql = @"INSERT INTO Caixa (idCliente, idForma, Valor, VlNota, Obs, Desconto, idVend, UID, Data) VALUES ("
                + idCliente.ToString() + ", "
                + idForma.ToString() + ", "
                + glo.sv(compra) + ", "
                + glo.sv(compra - desc) + ", "
                + glo.fa(obs) + ", "
                + glo.sv(desc) + ", "
                + idVend.ToString() + ", "
                + glo.fa(UID)
                + ",Now)";
            DB.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(DateTime DT1, DateTime DT2, int idForma, string sObs)
        {
            bool Sair = false;
            DataTable dt = null;
            int qtD = 0;
            while (Sair==false)
            {
                DateTime dataInicio = DT1.Date;
                DateTime dataFim = DT2.Date;
                string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
                string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");

                StringBuilder query = new StringBuilder();
                query.Append(@"SELECT ca.ID, c.Nome AS Cliente, ca.Valor, ca.Desconto, ca.VlNota, 
                    v.Nome AS Vendedor, ca.Data, f.Nome AS Pagamento, ca.Obs,
                    c.NrCli, ca.idVend, ca.idForma, ca.UID 
                    FROM ((Caixa ca
                    LEFT JOIN Clientes c ON c.NrCli = ca.idCliente)
                    LEFT JOIN Vendedores v ON v.ID = ca.idVend)
                    LEFT JOIN Formas f ON f.ID = (ca.idForma + 1)");
                query.AppendFormat(" WHERE ca.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
                if (idForma>0)
                {
                    query.AppendFormat(" AND ca.idForma = {0}", idForma-1);
                }
                if (sObs.Length>0)
                {
                    query.AppendFormat(" AND ca.Obs Like '%{0}%' ", sObs);
                }
                query.Append(" ORDER BY ca.ID DESC");
                dt = DB.ExecutarConsulta(query.ToString());
                if (dt.Rows.Count == 0)
                {
                    string q2 = "Select Data From Caixa Order by Data desc";
                    DataTable dt2 = DB.ExecutarConsulta(q2);
                    if (dt2.Rows.Count == 0)
                    {
                        Sair = true;
                    } else
                    {
                        DT1 = (DateTime)dt2.Rows[0]["Data"];
                        if (qtD<10)
                        {
                            qtD++;
                            DT2 = DT2.AddDays(1);
                        } else
                        {
                            Sair = true;
                        }                        
                    }                        
                } else
                {
                    Sair = true;
                }

            }
            return dt;
        }

        public void Edita(int iID, int idForma, float compra, int idCliente, string obs, float desc, int idVend)
        {
            String sql = @"UPDATE Caixa SET 
                              idCliente = " + idCliente.ToString() +
                            ",idForma = " + idForma.ToString() +
                            ",Valor = " + glo.sv(compra) +
                            ",VlNota = " + glo.sv(compra) +
                            ",Obs = " + glo.fa(obs) +
                            ",Desconto = " + glo.sv(desc) +
                            ",idVend = " + idVend.ToString() +
                            " WHERE ID = " + iID.ToString();
            DB.ExecutarComandoSQL(sql);
        }

        public void Exclui(int iID)
        {
            String sql = @"Delete From Caixa WHERE ID = " + iID.ToString();
            DB.ExecutarComandoSQL(sql);
        }

        public void MudaData(DateTime data, string lista)
        {           
            string sData = data.ToString("MM/dd/yyyy");
            String sql = $@"Update Caixa Set Data = #{sData}# WHERE ID in ({lista}) ";
            DB.ExecutarComandoSQL(sql);
        }

        public void EditaFormaPagamento(int registroId, int novaFormaId)
        {
            string query = $@"UPDATE Caixa SET idForma = {novaFormaId} WHERE ID = {registroId} ";
            DB.ExecutarComandoSQL(query);
        }

        public void AtualizaForma(int iID, int idForma)
        {
            String sql = @"UPDATE Caixa SET 
                      idForma = " + idForma.ToString() +
                           " WHERE ID = " + iID.ToString();
            DB.ExecutarComandoSQL(sql);
        }
    }
}
