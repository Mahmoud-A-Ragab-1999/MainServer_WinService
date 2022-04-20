using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class MontrMonitorGroupsCounters
    {
        public string MachineId { get; set; }
        public int GroupId { get; set; }
        public string CounterId { get; set; }
        public string InstanceId { get; set; }
    }
}
