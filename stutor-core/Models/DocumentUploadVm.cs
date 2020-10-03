using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models
{
    public class DocumentUploadVm
    {
        public IFormCollection Documents { get; set; }

        public int ApplicationId { get; set; }
    }
}
