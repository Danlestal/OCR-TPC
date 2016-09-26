using System.ComponentModel.DataAnnotations;

namespace OCR_API.DataLayer
{
    public class PdfToDelete
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string PathAbsolutoArchivo { get; set; }
    }
}