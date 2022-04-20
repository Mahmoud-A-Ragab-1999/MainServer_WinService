using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class ServersUpdates
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDt { get; set; }
        public string ServerId { get; set; }
        public string SysSystemName { get; set; }
        public string SysIpAddress { get; set; }
        public string CpuProcessorId { get; set; }
        public string CpuProcessorName { get; set; }
        public short? CpuCoresCount { get; set; }
        public short? CpuCoresEnabledCount { get; set; }
        public short? CpuLogicalProcessorsCount { get; set; }
        public short? CpuMaxClockSpeed { get; set; }
        public decimal? MemoryTotal { get; set; }
        public decimal? StorageTotal { get; set; }
    }
}
