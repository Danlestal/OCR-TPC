using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR
{
    public class Constants
    {
        public static string PeriodRegExp = @"(.*) (\d\d\d\d) a (.*) (\d\d\d\d) (.*)";
        public static string IDRegExp = @"([\d?\]][\d?\]] [\d?\]][\d?\]][\d?\]][\d?\]][\d?\]][\d?\]][\d?\]] [\d?\]][\d?\]])";
        public static string SocialReasonRegExp = "Razón Social: (.*)";
        public static string CNAERegExp = "CNAE .*: (.*)";
        public static string NIFRegExp = "Fiscal: (.*)";
        public static string NumSeguridadSocialRegExp = @"([\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d])";
        public static string DNIRegExp = @"(\d\d\d\d\d\d\d\d\d[A-Z])";





        public static string[] SpanishMonths = { "Enero","Febrero","Marzo","Abril","Mayo","Junio","Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre" };

       
    }
}
