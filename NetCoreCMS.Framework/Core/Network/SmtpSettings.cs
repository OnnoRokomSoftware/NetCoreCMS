using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreCMS.Framework.Core.Network
{
    public class SmtpSettings
    {
        [Required]
        public string Host { get; set; }
        [Required]
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FromEmail { get; set; }
        [Required]
        public string FromName { get; set; }
    }
}
