using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class MontrMonitorTransactions
    {
        public int TransactionId { get; set; }
        public string MachineId { get; set; }
        public string CounterId { get; set; }
        public string InstanceId { get; set; }
        public DateTime CounterDatetime { get; set; }
        public double CounterValue { get; set; }
    }
}
