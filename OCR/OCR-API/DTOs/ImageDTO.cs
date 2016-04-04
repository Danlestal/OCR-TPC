using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.DTOs
{
    public class ImageDTO
    {
        public int PhotoID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime UploadedOn { get; set; }
    }
}

