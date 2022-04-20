using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class SetCountersCategories
    {
        public string CategoryId { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryName { get; set; }
        public byte? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}
