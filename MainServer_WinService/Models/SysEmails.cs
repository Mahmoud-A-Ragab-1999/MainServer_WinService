using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class SysEmails
    {
        public int EmailId { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string EmailTo { get; set; }
        public string EmailSenderSmtp { get; set; }
    }
}
