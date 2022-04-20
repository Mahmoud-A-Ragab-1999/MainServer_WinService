using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class SetCounters
    {
        public string CounterId { get; set; }
        public string CounterDescription { get; set; }
        public string CounterName { get; set; }
        public string CategoryName { get; set; }
        public string CounterUnit { get; set; }
        public string CategoryId { get; set; }
        public string InstanceTotalName { get; set; }
        public string InstanceAllPrefix { get; set; }
        public string InstanceBlankName { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}
