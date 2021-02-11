using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.ViewModels
{
    public class ResendConfirmation
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
