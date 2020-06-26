using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.Models
{
    public class ContactForm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }

        public string FullName {
            get {
                return FirstName + " " + LastName;
            }
        }

        public string FormattedMessage
        {
            get
            {
                return "From: " + FullName + ": \n\n Email: " + Email + "\n\n" + Message;
            }
        }

    }
}
