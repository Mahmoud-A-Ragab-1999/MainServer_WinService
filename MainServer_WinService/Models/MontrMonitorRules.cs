using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class MontrMonitorRules
    {
        public int RuleId { get; set; }
        public string MachineId { get; set; }
        public string CounterId { get; set; }
        public string InstanceId { get; set; }
        public string RuleField { get; set; }
        public string RuleMathSymbol { get; set; }
        public double RuleValue { get; set; }
        public byte RuleOcuuranceType { get; set; }
        public int OcuuranceInterval { get; set; }
        public string DisplayLevel { get; set; }
        public int ActionId { get; set; }
        public DateTime? FirstOccuranceDatetime { get; set; }
        public DateTime? LastOccuranceDatetime { get; set; }
        public int? OccuranceCount { get; set; }
        public int? OccuranceInterval { get; set; }
        public bool? IsAlarmRaised { get; set; }
        public bool? IsActionRaised { get; set; }
    }
}
