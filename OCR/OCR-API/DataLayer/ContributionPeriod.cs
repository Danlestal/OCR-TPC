using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCR_API.DataLayer
{
    public class PeriodoContribucion
    {

        public int PeriodoContribucionId { get; set; }

        public DateTime ComienzoPeriodo { get; set; }

        public DateTime FinPeriodo { get; set; }

        public double Dinero { get; set; }

        public int ContribuidorRefId { get; set; }

        [MaxLength(250)]
        public string HighResImagenId { get; set; }

        [ForeignKey("ContributorRefId")]
        public virtual Contribuidor Contributor { get; set; }

       
    }
}