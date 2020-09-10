using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Sql
{
    public class OrderPasskey
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ClientPasskey { get; set; }


        public virtual Order Order { get; set; }
    }
}
