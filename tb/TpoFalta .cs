﻿namespace TeleBonifacio.tb
{
    public class TpoFalta : IDataEntity
    {
        public string Nome { get; set; }
        public int Id { get; set; }
        public bool Adicao { get; set; }
        public string Cor { get; set; } // 🔹 Novo campo adicionado
    }
}
