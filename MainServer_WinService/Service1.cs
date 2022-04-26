using MainServer_WinService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Utilities;
using System.Configuration;

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
            var counter = context.MontrMonitorCounters;

            foreach (var tr in rule)
            {
                double? value = counter.Where(c => c.MachineId == tr.MachineId && c.CounterId == tr.CounterId && c.InstanceId == tr.InstanceId).Select(tr.RuleField.ToUpper() + "Value").AsEnumerable().FirstOrDefault();
                if (value != null)
                {
                    if (ValidationI(value, tr.RuleMathSymbol, tr.RuleValue))
                    {
                        SeedSuccessRule(tr);
                        if (ValidationII(tr.RuleOcuuranceType, tr.OcuuranceInterval, tr.OccuranceCount.Value, tr.OccuranceInterval.Value))
                        {
                            SeedRuleAction(tr);
                            SeedRuleEvent(tr);
                            ResetRule(tr);
                        }
                    }
                    else
                    {
                        ResetRule(tr);
                    }
                }
            }
        }

        private bool ValidationI(double? value, string mathSymbol, double ruleValue)
        {
            if (mathSymbol == "equals")
                return value == ruleValue;
            else if (mathSymbol == "more_than")
                return value > ruleValue;
            else if (mathSymbol == "less_than")
                return value < ruleValue;
            else
                return value != ruleValue;
        }

        private bool ValidationII(int ocuuranceType, int ocuuranceInterval, int occuranceCount, int occuranceInterval)
        {
            if (ocuuranceType == 1)
                return occuranceCount == ocuuranceInterval;
            else if (ocuuranceType == 2)
                return occuranceInterval >= ocuuranceInterval;
            return false;
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
            monitorRule.FirstOccuranceDatetime = monitorRule.FirstOccuranceDatetime == null ? DateTime.Now : monitorRule.FirstOccuranceDatetime;
            monitorRule.LastOccuranceDatetime = DateTime.Now;
            monitorRule.OccuranceCount = monitorRule.OccuranceCount == null ? 1 : monitorRule.OccuranceCount + 1;
            monitorRule.OccuranceInterval = (int)(monitorRule.LastOccuranceDatetime - monitorRule.FirstOccuranceDatetime).Value.TotalSeconds;

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
                string body = $"The {monitorRule.RuleField} Value of the {monitorRule.CounterId} counter was {monitorRule.RuleMathSymbol} {monitorRule.RuleValue} at {monitorRule.LastOccuranceDatetime}";
                string subject = $"{monitorRule.CounterId} Alert.";
                string emailTo = "";

                foreach (var id in userIds)
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == id);
                    EmailUtilities.SendEmail(user.Email, user.Name, subject, body, out error);
                    if (emailTo == "")
                        emailTo += user.Email;
                    else
                        emailTo += "," + user.Email;
                }
                SysEmails email = new SysEmails();
                email.EmailSubject = subject;
                email.EmailBody = body;
                email.EmailTo = emailTo;
                email.EmailSenderSmtp = ConfigurationManager.AppSettings["SenderEmail"];

                context.SysEmails.Add(email);
                context.SaveChanges();
            }
            else if (monitorRule.ActionId == 2)
            {
            }
        }
    }
}
