using Microsoft.AspNetCore.Http;

namespace stutor_core.Models.ViewModels
{
    public class DocumentUploadVm
    {
        public IFormCollection Documents { get; set; }

        public int ApplicationId { get; set; }
    }
}
