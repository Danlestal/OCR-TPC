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

        public bool Valido { get; internal set; }

        public override bool Equals(object obj)
        {
            PeriodoContribucion target = obj as PeriodoContribucion;
            return ((target.ComienzoPeriodo == this.ComienzoPeriodo) && (target.FinPeriodo == this.FinPeriodo) && (this.Dinero == target.Dinero));
        }
    }
}