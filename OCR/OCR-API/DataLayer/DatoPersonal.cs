using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.DataLayer
{
    public class DatoPersonal
    {

        public DatoPersonal()
        {
            PeriodosVidaLaboral = new List<PeriodoVidaLaboral>();
        }

        public int DatoPersonalId { get; set; }
        
        public string Nombre { get; set; }

        public string FechaNacimiento { get; set; }

        public string DNI { get; set; }
        
        public string NumeroSeguridadSocial { get; set; }

        public string FileName { get; set; }

        public virtual ICollection<PeriodoVidaLaboral> PeriodosVidaLaboral { get; set; }

        public void AddContributionPeriod(PeriodoVidaLaboral newPeriod)
        {
            if (!PeriodosVidaLaboral.Contains(newPeriod))
            {
                PeriodosVidaLaboral.Add(newPeriod);
            }

        }
    }
}