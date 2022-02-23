using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.ViewModels
{
    public class UpdateLocation
    {
        public string UserId { get; set; }
        public LocationData location { get; set; }
    }
}
