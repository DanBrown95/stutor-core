using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.ViewModels
{
    public class UpdatePhoneVM
    {
        public string UserId { get; set; }
        public string OldPhone { get; set; }
        public string NewPhone { get; set; }
    }
}
