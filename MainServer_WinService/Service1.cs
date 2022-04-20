﻿using MainServer_WinService.Models;
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
            var rule = context.MontrMonitorRules.Where(x => x.IsActionRaised == false && x.IsAlarmRaised == false).ToList();

            var cont = context.MontrMonitorTransactions./*Where(x=>rule.Select(s=>s.MachineId).Contains(x.MachineId)&& rule.Select(s => s.CounterId).Contains(x.CounterId) && rule.Select(s => s.InstanceId).Contains(x.InstanceId)).*/ToList();
            foreach (var tr in rule)
            {
                var transactions = cont.Where(x => x.MachineId == tr.MachineId && x.CounterId == tr.CounterId && x.InstanceId == tr.InstanceId).ToList();
                if (transactions != null && transactions.Any())
                {
                    if (tr.RuleField == "current")
                    {
                        var value = Math.Round(transactions.LastOrDefault().CounterValue, 2);
                        if (tr.RuleMathSymbol == "equals")
                        {
                            if (value == Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    var q = context.MontrMonitorTransactions.OrderByDescending(t => t.CounterDatetime).Take(tr.OcuuranceInterval);
                                    if (q.Where(x => Math.Round(x.CounterValue, 2) == Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    var q = context.MontrMonitorTransactions.OrderByDescending(t => t.CounterDatetime).Take(tr.OcuuranceInterval);
                                    if (q.Where(x => Math.Round(x.CounterValue, 2) != Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (transactions.Where(x => Math.Round(x.CounterValue, 2) > Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (transactions.Where(x => Math.Round(x.CounterValue, 2) < Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                    }
                    else if (tr.RuleField == "average")
                    {
                        var value = Math.Round((double)context.TransactionCalculated.LastOrDefault().Average, 2);
                        if (tr.RuleMathSymbol == "equals")
                        {
                            if (value == Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Average, 2) == Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Average, 2) != Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Average, 2) > Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Average, 2) < Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                    }
                    else if (tr.RuleField == "minimum")
                    {
                        var value = Math.Round((double)context.TransactionCalculated.LastOrDefault().Minimum, 2);
                        if (tr.RuleMathSymbol == "equals")
                        {
                            if (value == Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Minimum, 2) == Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Minimum, 2) != Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Minimum, 2) > Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Minimum, 2) < Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                    }
                    else if (tr.RuleField == "maximum")
                    {
                        var value = Math.Round((double)context.TransactionCalculated.LastOrDefault().Maximum, 2);
                        if (tr.RuleMathSymbol == "equals")
                        {
                            if (value == Math.Round(tr.RuleValue, 2))
                            {
                                if (tr.RuleOcuuranceType == 1)
                                {
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Maximum, 2) == Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Maximum, 2) != Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Maximum, 2) > Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
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
                                    if (context.TransactionCalculated.Where(x => Math.Round((double)x.Maximum, 2) < Math.Round(tr.RuleValue, 2)).Count() == tr.OcuuranceInterval)
                                    {
                                        tr.OccuranceCount = tr.OccuranceCount == null ? 1 : tr.OccuranceCount + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                else if (tr.RuleOcuuranceType == 2)
                                {
                                    if (tr.LastOccuranceDatetime.HasValue && tr.OccuranceInterval.HasValue && DateTime.Now == tr.LastOccuranceDatetime.Value.AddSeconds(tr.OccuranceInterval.Value))
                                    {
                                        tr.OccuranceInterval = tr.OccuranceInterval == null ? 1 : tr.OccuranceInterval.Value + 1;
                                        SeedRuleAction(tr.ActionId);
                                    }
                                }
                                SeedSuccessRule(tr);
                            }
                        }
                    }

                }
            }
        }

        private void SeedSuccessRule(MontrMonitorRules monitorRule)
        {
            SmartMonitoringContext context = new SmartMonitoringContext();
            // monitorRule.IsAlarmRaised = true;
            monitorRule.FirstOccuranceDatetime = monitorRule.FirstOccuranceDatetime == null ? DateTime.Now : monitorRule.FirstOccuranceDatetime;
            monitorRule.LastOccuranceDatetime = DateTime.Now;
            context.Entry(monitorRule).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        private void SeedRuleAction(int actionId)
        {
            if (actionId == 0)
            {

            }
            else if (actionId == 1)
            {

            }
            else if (actionId == 2)
            {
            }
        }
    }
}