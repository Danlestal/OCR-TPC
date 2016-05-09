using System.Collections.Generic;

namespace OCR_API.DataLayer
{
    public class Contribuidor
    {

        public Contribuidor()
        {
            PeriodosContribucion = new List<PeriodoContribucion>();
        }

        public int ContribuidorId { get; set; }

        public double CuentaCotizacion { get; set; }

        public string RazonSocial { get; set; }

        public string CNAE { get; set; }

        public string NIF { get; set; }

        public virtual ICollection<PeriodoContribucion> PeriodosContribucion { get; set; }

        public void AddContributionPeriod(PeriodoContribucion newPeriod)
        {
            if (!PeriodosContribucion.Contains(newPeriod))
            {
                PeriodosContribucion.Add(newPeriod);
            }

        }
    }
}