using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCR_API.DataLayer
{
    public class PeriodoVidaLaboral
    {

        public int PeriodoVidaLaboralId { get; set; }
        
        public string Regimen { get; set; }

        public string Codigo { get; set; }

        public string Compania { get; set; }

        public string FechaDeInicio { get; set; }

        public string FechaDeInicioEfectiva { get; set; }

        public string FechaDeFin { get; set; }

        public string CT { get; set; }

        public string CTP { get; set; }

        public string GC { get; set; }

        public string Dias { get; set; }
        
        public int DatoPersonalRefId { get; set; }

        [ForeignKey("DatoPersonalRefId")]
        public virtual DatoPersonal Persona { get; set; }


    }
}