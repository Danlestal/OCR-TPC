using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.DTOs
{
    public class LaboralLifeDTO
    {
        public LaboralLifeDTO()
        {
            Rows = new List<LaboralLifeRowDTO>();
        }

        public PersonalDataDTO PersonalData { get; set; }
        public List<LaboralLifeRowDTO> Rows { get; set; }



       
    }

    public class PersonalDataDTO
    {
        public string Name { get; set; }
        public string BornDate { get; set; }
        public string DNI { get; set; }
        public string HealthCareId { get; set; }
        public string FileName { get; set; }
    }

    public class LaboralLifeRowDTO
    {
        public string Regimen { get; set; }
        public string Code { get; set; }
        public string Company { get; set; }
        public string StartDate { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EndDate { get; set; }
        public string CT { get; set; }
        public string CTP { get; set; }
        public string GC { get; set; }
        public string Days { get; set; }
    }
}