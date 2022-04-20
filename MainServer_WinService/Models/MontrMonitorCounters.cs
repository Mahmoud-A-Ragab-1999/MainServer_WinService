using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class MontrMonitorCounters
    {
        public string MachineId { get; set; }
        public string CounterId { get; set; }
        public string InstanceId { get; set; }
        public string InstanceName { get; set; }
        public int? DisplayOrder { get; set; }
        public double? CurrentValue { get; set; }
        public DateTime? CurrentDatetime { get; set; }
        public double? MinimumValue { get; set; }
        public DateTime? MinimumDatetime { get; set; }
        public double? MaximumValue { get; set; }
        public DateTime? MaximumDatetime { get; set; }
        public double? AverageValue { get; set; }
        public DateTime? AverageDatetime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDt { get; set; }
    }
}
