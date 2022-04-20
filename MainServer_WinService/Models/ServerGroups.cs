using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class ServerGroups
    {
        public ServerGroups()
        {
            MontrMachinesList = new HashSet<MontrMachinesList>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MontrMachinesList> MontrMachinesList { get; set; }
    }
}
