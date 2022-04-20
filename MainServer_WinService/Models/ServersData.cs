using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class ServersData
    {
        public string ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerDescription { get; set; }
        public string HostName { get; set; }
        public string IpAddress { get; set; }
        public string OsName { get; set; }
        public bool? IsCurrentMachine { get; set; }
        public string ConnDomainName { get; set; }
        public string ConnUserName { get; set; }
        public string ConnUserPassword { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDt { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDt { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteDt { get; set; }
    }
}
