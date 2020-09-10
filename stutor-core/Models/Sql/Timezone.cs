using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class Timezone
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string TZName { get; set; }


        public virtual Expert Expert { get; set; }
    }
}
