using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models.Enumerations
{
    public static class OrderStatus
    {
        public const string Unanswered = "unanswered";
        public const string Completed = "completed";
        public const string Refunded = "refunded";
        public const string Canceled = "canceled";
        public const string CancelationPending = "cancelation pending";
    }
}
