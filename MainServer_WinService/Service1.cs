using MainServer_WinService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Utilities;

namespace MainServer_WinService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        SmartMonitoringContext context = new SmartMonitoringContext();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log("| ---Service Started At: " + DateTime.Now.ToString());
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 30 * 1000; //number in milisecinds  
            timer.Enabled = true;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Log("Service is recall at:   " + DateTime.Now);
            timer.Enabled = false;
            DoWork();
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            Log("Service Ended at:   " + DateTime.Now.ToString());
        }

        public void Log(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        public void DoWork()
        {           
            var rule = context.MontrMonitorRules.Where(r => r.IsActive == true)./*Where(x => x.IsActionRaised == false && x.IsAlarmRaised == false)*/ToList();

            var cont = context.MontrMonitorTransactions./*Where(x=>rule.Select(s=>s.MachineId).Contains(x.MachineId)&& rule.Select(s => s.CounterId).Contains(x.CounterId) && rule.Select(s => s.InstanceId).Contains(x.InstanceId)).*/ToList();
            var agg = context.TransactionCalculated.ToList();
            foreach (var tr in rule)
            {
                var transactions = cont.Where(x => x.MachineId == tr.MachineId && x.CounterId == tr.CounterId && x.InstanceId == tr.InstanceId).ToList();
                var history = agg.Where(x => x.MachineId == tr.MachineId && x.CounterId == tr.CounterId && x.InstanceId == tr.InstanceId).ToList();

                if (transactions != null && transactions.Any())
                {
                    if (tr.RuleField == "current")
                    {
                        var value = Math.Round(transactions.LastOrDefault().CounterValue, 2);
                        if (tr.RuleMathSymbol == "equals")
                        {
                            if (value == Math.Round(tr.RuleValue, 2))
                            { 
                                //var q = cont.OrderByDescending(t => t.CounterDatetime).Take(tr.OcuuranceInterval);
                                if (tr.RuleOcuuranceType == 1)
                                {                                    
                                    if (transactions.OrderByDescending(t => t.CounterDatetime).Take(tr.OcuuranceInterval).All(x => Math.Round(x.CounterValue, 2) == Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "not_equals")
                        {
                            if (value != Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (transactions.OrderByDescending(t => t.CounterDatetime).Take(tr.OcuuranceInterval).All(x => Math.Round(x.CounterValue, 2) != Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "more_than")
                        {
                            if (value >= Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (transactions.OrderByDescending(t => t.CounterDatetime).Take(tr.OcuuranceInterval).All(x => Math.Round(x.CounterValue, 2) > Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "less_than")
                        {
                            if (value <= Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (transactions.OrderByDescending(t => t.CounterDatetime).Take(tr.OcuuranceInterval).All(x => Math.Round(x.CounterValue, 2) < Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    //if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OcuuranceInterval))
                                    if (transactions.Where(x=> x.CounterDatetime >= transactions.LastOrDefault().CounterDatetime.AddSeconds(-tr.OcuuranceInterval)).All(s=> Math.Round(s.CounterValue, 2) < Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                    }
                    else if (tr.RuleField == "average")
                    {
                        var value = Math.Round((double)history.LastOrDefault().Average, 2);
                        if (tr.RuleMathSymbol == "equals")
                        {
                            if (value == Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Average, 2) == Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "not_equals")
                        {
                            if (value != Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Average, 2) != Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "more_than")
                        {
                            if (value >= Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Average, 2) > Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "less_than")
                        {
                            if (value <= Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Average, 2) < Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                    }
                    else if (tr.RuleField == "minimum")
                    {
                        var value = Math.Round((double)history.LastOrDefault().Minimum, 2);
                        if (tr.RuleMathSymbol == "equals")
                        {
                            if (value == Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Minimum, 2) == Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "not_equals")
                        {
                            if (value != Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Minimum, 2) != Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "more_than")
                        {
                            if (value >= Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Minimum, 2) > Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "less_than")
                        {
                            if (value <= Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Minimum, 2) < Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                    }
                    else if (tr.RuleField == "maximum")
                    {
                        var value = Math.Round((double)history.LastOrDefault().Maximum, 2);
                        if (tr.RuleMathSymbol == "equals")
                        {
                            if (value == Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Maximum, 2) == Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "not_equals")
                        {
                            if (value != Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Maximum, 2) != Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "more_than")
                        {
                            if (value >= Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Maximum, 2) > Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                        else if (tr.RuleMathSymbol == "less_than")
                        {
                            if (value <= Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (history.OrderByDescending(t => t.CreationDate).Take(tr.OcuuranceInterval).All(x => Math.Round((double)x.Maximum, 2) < Math.Round(tr.RuleValue, 2)))
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr);
                                        SeedRuleEvent(tr);
                                        ResetRule(tr);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                    }

                }
            }
        }

        private void ResetRule(MontrMonitorRules monitorRule)
        {
            monitorRule.FirstOccuranceDatetime = null;
            monitorRule.LastOccuranceDatetime = null;
            monitorRule.OccuranceCount = null;
            monitorRule.OccuranceInterval = null;

            context.Entry(monitorRule).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        private void SeedRuleEvent(MontrMonitorRules monitorRule)
        {
            RuleEvents ruleEvent = new RuleEvents();
            ruleEvent.MachineId = monitorRule.MachineId;
            ruleEvent.CounterId = monitorRule.CounterId;
            ruleEvent.InstanceId = monitorRule.InstanceId;
            ruleEvent.RuleField = monitorRule.RuleField;
            ruleEvent.RuleMathSymbol = monitorRule.RuleMathSymbol;
            ruleEvent.RuleValue = monitorRule.RuleValue;
            ruleEvent.RuleOcuuranceType = monitorRule.RuleOcuuranceType;
            ruleEvent.OcuuranceInterval = monitorRule.OcuuranceInterval;
            ruleEvent.RaisedActionId = monitorRule.ActionId;
            ruleEvent.RaisedActionFireDate = DateTime.Now;
            ruleEvent.RuleId = monitorRule.RuleId;

            context.RuleEvents.Add(ruleEvent);
            context.SaveChanges();
        }

        private void SeedSuccessRule(MontrMonitorRules monitorRule)
        {
            SmartMonitoringContext context = new SmartMonitoringContext();
            monitorRule.FirstOccuranceDatetime = monitorRule.FirstOccuranceDatetime == null ? DateTime.Now : monitorRule.FirstOccuranceDatetime;
            monitorRule.LastOccuranceDatetime = DateTime.Now;

            //if (monitorRule.RuleOcuuranceType == 1)
            //    monitorRule.OccuranceCount = monitorRule.OccuranceCount == null ? 1 : monitorRule.OccuranceCount + 1;

            //if (monitorRule.RuleOcuuranceType == 2)
            //    monitorRule.OccuranceInterval = monitorRule.OccuranceInterval == null ? 1 : monitorRule.OccuranceInterval + 1;

            context.Entry(monitorRule).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        private void SeedRuleAction(MontrMonitorRules monitorRule)
        {
            if (monitorRule.ActionId == 0)
            {

            }
            else if (monitorRule.ActionId == 1)
            {
                string error;
                var userIds = context.ServerUsers.Where(s => s.MachineId == monitorRule.MachineId).Select(s => s.UserId).ToList();
                foreach (var id in userIds)
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == id);
                    string body = $"The {monitorRule.RuleField} Value of the {monitorRule.CounterId} counter was {monitorRule.RuleMathSymbol} {monitorRule.RuleValue} at {monitorRule.LastOccuranceDatetime}";
                    EmailUtilities.SendEmail(user.Email, user.Name, $"{monitorRule.CounterId} Alert.", body, out error);
                }                                               
            }
            else if (monitorRule.ActionId == 2)
            {
            }
        }
    }
}
