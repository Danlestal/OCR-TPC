using OCR_API.DataLayer;
using OCR_API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.InternalService
{
    public class LaboralLifeInsertService
    {
        private VidaLaboral_Context dbContext;

        public LaboralLifeInsertService(VidaLaboral_Context context)
        {
            dbContext = context;
        }

        public bool Insert(LaboralLifeDTO dataToInsert)
        {
            if (dbContext.DatoPersonal.FirstOrDefault( s => s.DNI == dataToInsert.PersonalData.DNI && s.CuentaCotizacion == dataToInsert.PersonalData.HealthCareId) == null)
            {
                DatoPersonal personal = new DatoPersonal();
                personal.CuentaCotizacion = dataToInsert.PersonalData.HealthCareId;
                personal.DNI = dataToInsert.PersonalData.DNI;
                personal.FechaNacimiento = dataToInsert.PersonalData.BornDate;
                personal.Nombre = dataToInsert.PersonalData.Name;

                foreach (var row in dataToInsert.Rows)
                {
                    personal.PeriodosVidaLaboral.Add(new PeriodoVidaLaboral()
                    {
                        Codigo = row.Code,
                        Compania = row.Company,
                        CT = row.CT,
                        CTP = row.CTP,
                        GC = row.GC,
                        Dias = row.Days,
                        FechaDeFin = row.EndDate,
                        FechaDeInicio = row.StartDate,
                        FechaDeInicioEfectiva = row.EffectiveStartDate,
                        Regimen = row.Regimen
                    });
                }

                dbContext.DatoPersonal.Add(personal);
                dbContext.SaveChanges();

                return true;
            }
            return false;
        }
    }
}