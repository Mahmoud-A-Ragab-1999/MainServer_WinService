using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class ServerUsers
    {
        public int Id { get; set; }
        public string MachineId { get; set; }
        public int UserId { get; set; }

        public virtual MontrMachinesList Machine { get; set; }
        public virtual Users User { get; set; }
    }
}
