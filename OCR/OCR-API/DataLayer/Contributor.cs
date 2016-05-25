using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OCR_API.DataLayer
{
    public class Contribuidor
    {

        public Contribuidor()
        {
            PeriodosContribucion = new List<PeriodoContribucion>();
        }

        public int ContribuidorId { get; set; }

        [MaxLength(11)]
        public string CuentaCotizacion { get; set; }

        [MaxLength(250)]
        public string RazonSocial { get; set; }

        [MaxLength(50)]
        public string CNAE { get; set; }

        [MaxLength(20)]
        public string NIF { get; set; }

        public bool Traspasado { get; set; }

        public DateTime? FechaTraspaso { get; set; }

        [MaxLength(250)]
        public string PathAbsolutoArchivo { get; set; }

        public virtual ICollection<PeriodoContribucion> PeriodosContribucion { get; set; }

        public bool AddContributionPeriod(PeriodoContribucion newPeriod)
        {
            if (!PeriodosContribucion.Contains(newPeriod))
            {
                PeriodosContribucion.Add(newPeriod);
                return true;
            }

            return false;
        }
    }
}