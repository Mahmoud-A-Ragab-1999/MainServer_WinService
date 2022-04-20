using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class MontrMonitorGroups
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string MachineId { get; set; }
        public string GroupImageId { get; set; }
        public int? DisplayOrder { get; set; }
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
