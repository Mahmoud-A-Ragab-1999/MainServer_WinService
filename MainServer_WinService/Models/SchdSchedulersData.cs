using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class SchdSchedulersData
    {
        public int SchedulerId { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public bool? SchdIsEnabled { get; set; }
        public DateTime SchdDurationStartDate { get; set; }
        public DateTime SchdDurationEndDate { get; set; }
        public string SchdFrequencey { get; set; }
        public string SchdDailyFrequency { get; set; }
        public TimeSpan? SchdDailyOnceTime { get; set; }
        public byte? SchdDailyRepeatInterval { get; set; }
        public string SchdDailyRepeatUnit { get; set; }
        public TimeSpan? SchdDailyRepeatStartTime { get; set; }
        public TimeSpan? SchdDailyRepeatEndTime { get; set; }
        public bool? SchdWeeklySaturday { get; set; }
        public bool? SchdWeeklySunday { get; set; }
        public bool? SchdWeeklyMonday { get; set; }
        public bool? SchdWeeklyTuesday { get; set; }
        public bool? SchdWeeklyWednesday { get; set; }
        public bool? SchdWeeklyThursday { get; set; }
        public bool? SchdWeeklyFriday { get; set; }
        public byte? SchdMonthlyDay { get; set; }
    }
}
