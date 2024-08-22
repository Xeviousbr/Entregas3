﻿using System.Data;

namespace TeleBonifacio.dao
{
    public abstract class BaseDAO
    {
        protected string connectionString;

        public bool Adicao { get; set; }

        protected BaseDAO()
        {
            this.connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + glo.CaminhoBase + ";";
        }

        #region Metodos Abstratos

        public abstract DataTable GetDadosOrdenados(string filtro = "", string ordem = "");

        public abstract void Grava(object obj);
        public abstract tb.IDataEntity Apagar(int direcao, tb.IDataEntity entidade);

        public abstract object GetUltimo();

        public abstract tb.IDataEntity ParaTraz();

        public abstract tb.IDataEntity ParaFrente();

        public abstract tb.IDataEntity GetEsse();

        public abstract DataTable getDados();

        public abstract tb.IDataEntity GetPeloID(string id);

        public abstract string VeSeJaTem(object obj);

        #endregion

        public virtual System.Data.DataTable CarregarDados()
        {
            return null;
        }        

        public virtual void SetarLinhas(int v)
        {

        }        

        public virtual DataTable Fitrar(string pesquisar)
        {
            return null;
        }

        public virtual int getIdAtual()
        {
            return 0;
        }

        public virtual DataTable getDadosC()
        {
            return null;
        }

        public virtual void SetId(int iD)
        {
            // 
        }

        public virtual void SetNome(string nome)
        {
            // 
        }

        //public virtual void SetSenha(string nome)
        //{
        //    // 
        //}
    }
}
