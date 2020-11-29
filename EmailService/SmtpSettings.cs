using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService
{
    public class SmtpSettings
    {
        public string EmailFrom { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

    }
}
