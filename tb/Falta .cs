﻿using System;

namespace TeleBonifacio.tb
{
    public class Falta : IDataEntity
    {
        public int Id { get; set; } 

        public int IDBalconista { get; set; } 

        public DateTime Data { get; set; } 

        public float Quant { get; set; } 

        public string Codigo { get; set; } 
        public bool Adicao { get; set; }
    }
}