using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class TransactionCalculated
    {
        public int Id { get; set; }
        public decimal? Average { get; set; }
        public decimal? Minimum { get; set; }
        public decimal? Maximum { get; set; }
        public DateTime? CreationDate { get; set; }
        public string MachineId { get; set; }
        public string CounterId { get; set; }
        public string InstanceId { get; set; }
    }
}
