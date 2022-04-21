using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MainServer_WinService.Models
{
    public partial class SmartMonitoringContext : DbContext
    {
        public SmartMonitoringContext()
        {
        }

        public SmartMonitoringContext(DbContextOptions<SmartMonitoringContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MontrMachinesList> MontrMachinesList { get; set; }
        public virtual DbSet<MontrMonitorCounters> MontrMonitorCounters { get; set; }
        public virtual DbSet<MontrMonitorGroups> MontrMonitorGroups { get; set; }
        public virtual DbSet<MontrMonitorGroupsCounters> MontrMonitorGroupsCounters { get; set; }
        public virtual DbSet<MontrMonitorRules> MontrMonitorRules { get; set; }
        public virtual DbSet<MontrMonitorTransactions> MontrMonitorTransactions { get; set; }
        public virtual DbSet<RuleEvents> RuleEvents { get; set; }
        public virtual DbSet<SchdSchedulersData> SchdSchedulersData { get; set; }
        public virtual DbSet<ServerGroups> ServerGroups { get; set; }
        public virtual DbSet<ServerUsers> ServerUsers { get; set; }
        public virtual DbSet<ServersData> ServersData { get; set; }
        public virtual DbSet<ServersUpdates> ServersUpdates { get; set; }
        public virtual DbSet<SetCounters> SetCounters { get; set; }
        public virtual DbSet<SetCountersCategories> SetCountersCategories { get; set; }
        public virtual DbSet<SetSerialno> SetSerialno { get; set; }
        public virtual DbSet<SysSettings> SysSettings { get; set; }
        public virtual DbSet<TransactionCalculated> TransactionCalculated { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<XxCountersTransactions> XxCountersTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-QQD7L2O;Database=SmartMonitoring;Persist Security Info=True;User ID =sa; Password=123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MontrMachinesList>(entity =>
            {
                entity.HasKey(e => e.MachineId);

                entity.ToTable("montr_machines_list");

                entity.Property(e => e.MachineId)
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConnDomainName)
                    .HasColumnName("conn_domain_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConnUserName)
                    .HasColumnName("conn_user_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConnUserPassword)
                    .HasColumnName("conn_user_password")
                    .HasMaxLength(150);

                entity.Property(e => e.DeleteDt)
                    .HasColumnName("delete_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteUserId).HasColumnName("delete_user_id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.HostName)
                    .IsRequired()
                    .HasColumnName("host_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDt)
                    .HasColumnName("insert_dt")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId).HasColumnName("insert_user_id");

                entity.Property(e => e.IntervalTimer).HasColumnName("interval_timer");

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ip_address")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsCurrentMachine).HasColumnName("is_current_machine");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.MachineDescription)
                    .HasColumnName("machine_description")
                    .HasMaxLength(250);

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasColumnName("machine_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OsName)
                    .HasColumnName("os_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDt)
                    .HasColumnName("update_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.MontrMachinesList)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_montr_machines_list_server_groups");
            });

            modelBuilder.Entity<MontrMonitorCounters>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("montr_monitor_counters");

                entity.Property(e => e.AverageDatetime)
                    .HasColumnName("average_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.AverageValue).HasColumnName("average_value");

                entity.Property(e => e.CounterId)
                    .IsRequired()
                    .HasColumnName("counter_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentDatetime)
                    .HasColumnName("current_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.CurrentValue).HasColumnName("current_value");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InsertDt)
                    .HasColumnName("insert_dt")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId).HasColumnName("insert_user_id");

                entity.Property(e => e.InstanceId)
                    .HasColumnName("instance_id")
                    .HasMaxLength(150);

                entity.Property(e => e.InstanceName)
                    .HasColumnName("instance_name")
                    .HasMaxLength(150);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaximumDatetime)
                    .HasColumnName("maximum_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.MaximumValue).HasColumnName("maximum_value");

                entity.Property(e => e.MinimumDatetime)
                    .HasColumnName("minimum_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.MinimumValue).HasColumnName("minimum_value");
            });

            modelBuilder.Entity<MontrMonitorGroups>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("montr_monitor_groups");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.DeleteDt)
                    .HasColumnName("delete_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteUserId).HasColumnName("delete_user_id");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GroupImageId)
                    .HasColumnName("group_image_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasColumnName("group_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDt)
                    .HasColumnName("insert_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.InsertUserId).HasColumnName("insert_user_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDt)
                    .HasColumnName("update_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            });

            modelBuilder.Entity<MontrMonitorGroupsCounters>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("montr_monitor_groups_counters");

                entity.Property(e => e.CounterId)
                    .IsRequired()
                    .HasColumnName("counter_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.InstanceId)
                    .IsRequired()
                    .HasColumnName("instance_id")
                    .HasMaxLength(150);

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MontrMonitorRules>(entity =>
            {
                entity.HasKey(e => e.RuleId)
                    .HasName("PK__montr_mo__E92A9296C12CB03B");

                entity.ToTable("montr_monitor_rules");

                entity.Property(e => e.RuleId)
                    .HasColumnName("rule_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActionId)
                    .HasColumnName("action_id")
                    .HasComment("0 => Notification, 1 => ");

                entity.Property(e => e.CounterId)
                    .IsRequired()
                    .HasColumnName("counter_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayLevel)
                    .IsRequired()
                    .HasColumnName("display_level")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstOccuranceDatetime)
                    .HasColumnName("first_occurance_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.InstanceId)
                    .HasColumnName("instance_id")
                    .HasMaxLength(150);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.LastOccuranceDatetime)
                    .HasColumnName("last_occurance_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OccuranceCount).HasColumnName("occurance_count");

                entity.Property(e => e.OccuranceInterval).HasColumnName("occurance_interval");

                entity.Property(e => e.OcuuranceInterval).HasColumnName("ocuurance_interval");

                entity.Property(e => e.RuleField)
                    .IsRequired()
                    .HasColumnName("rule_field")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RuleMathSymbol)
                    .IsRequired()
                    .HasColumnName("rule_math_symbol")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RuleOcuuranceType).HasColumnName("rule_ocuurance_type");

                entity.Property(e => e.RuleValue).HasColumnName("rule_value");
            });

            modelBuilder.Entity<MontrMonitorTransactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("montr_monitor_transactions");

                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

                entity.Property(e => e.CounterDatetime)
                    .HasColumnName("counter_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.CounterId)
                    .IsRequired()
                    .HasColumnName("counter_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CounterValue).HasColumnName("counter_value");

                entity.Property(e => e.InstanceId)
                    .HasColumnName("instance_id")
                    .HasMaxLength(150);

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RuleEvents>(entity =>
            {
                entity.ToTable("rule_events");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AckDateTime)
                    .HasColumnName("ack_date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.AckUser).HasColumnName("ack_user");

                entity.Property(e => e.CounterId)
                    .IsRequired()
                    .HasColumnName("counter_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.InstanceId)
                    .HasColumnName("instance_id")
                    .HasMaxLength(150);

                entity.Property(e => e.IsAck).HasColumnName("is_ack");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OcuuranceInterval).HasColumnName("ocuurance_interval");

                entity.Property(e => e.RaisedActionFireDate)
                    .HasColumnName("raised_action_fire_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RaisedActionId).HasColumnName("raised_action_id");

                entity.Property(e => e.RuleField)
                    .IsRequired()
                    .HasColumnName("rule_field")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RuleId).HasColumnName("rule_id");

                entity.Property(e => e.RuleMathSymbol)
                    .IsRequired()
                    .HasColumnName("rule_math_symbol")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RuleOcuuranceType).HasColumnName("rule_ocuurance_type");

                entity.Property(e => e.RuleValue).HasColumnName("rule_value");
            });

            modelBuilder.Entity<SchdSchedulersData>(entity =>
            {
                entity.HasKey(e => e.SchedulerId);

                entity.ToTable("schd_schedulers_data");

                entity.Property(e => e.SchedulerId).HasColumnName("scheduler_id");

                entity.Property(e => e.EntityId)
                    .IsRequired()
                    .HasColumnName("entity_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EntityType)
                    .IsRequired()
                    .HasColumnName("entity_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchdDailyFrequency)
                    .IsRequired()
                    .HasColumnName("schd_daily_frequency")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SchdDailyOnceTime)
                    .HasColumnName("schd_daily_once_time")
                    .HasColumnType("time(0)");

                entity.Property(e => e.SchdDailyRepeatEndTime).HasColumnName("schd_daily_repeat_end_time");

                entity.Property(e => e.SchdDailyRepeatInterval).HasColumnName("schd_daily_repeat_interval");

                entity.Property(e => e.SchdDailyRepeatStartTime)
                    .HasColumnName("schd_daily_repeat_start_time")
                    .HasColumnType("time(0)");

                entity.Property(e => e.SchdDailyRepeatUnit)
                    .HasColumnName("schd_daily_repeat_unit")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SchdDurationEndDate)
                    .HasColumnName("schd_duration_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.SchdDurationStartDate)
                    .HasColumnName("schd_duration_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.SchdFrequencey)
                    .IsRequired()
                    .HasColumnName("schd_frequencey")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SchdIsEnabled)
                    .IsRequired()
                    .HasColumnName("schd_is_enabled")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SchdMonthlyDay).HasColumnName("schd_monthly_day");

                entity.Property(e => e.SchdWeeklyFriday).HasColumnName("schd_weekly_friday");

                entity.Property(e => e.SchdWeeklyMonday).HasColumnName("schd_weekly_monday");

                entity.Property(e => e.SchdWeeklySaturday).HasColumnName("schd_weekly_saturday");

                entity.Property(e => e.SchdWeeklySunday).HasColumnName("schd_weekly_sunday");

                entity.Property(e => e.SchdWeeklyThursday).HasColumnName("schd_weekly_thursday");

                entity.Property(e => e.SchdWeeklyTuesday).HasColumnName("schd_weekly_tuesday");

                entity.Property(e => e.SchdWeeklyWednesday).HasColumnName("schd_weekly_wednesday");
            });

            modelBuilder.Entity<ServerGroups>(entity =>
            {
                entity.ToTable("server_groups");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ServerUsers>(entity =>
            {
                entity.ToTable("server_users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.ServerUsers)
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_server_users_montr_machines_list");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServerUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_server_users_Users");
            });

            modelBuilder.Entity<ServersData>(entity =>
            {
                entity.HasKey(e => e.ServerId);

                entity.ToTable("servers_data");

                entity.Property(e => e.ServerId)
                    .HasColumnName("server_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConnDomainName)
                    .HasColumnName("conn_domain_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConnUserName)
                    .HasColumnName("conn_user_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConnUserPassword)
                    .HasColumnName("conn_user_password")
                    .HasMaxLength(150);

                entity.Property(e => e.DeleteDt)
                    .HasColumnName("delete_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteUserId).HasColumnName("delete_user_id");

                entity.Property(e => e.HostName)
                    .IsRequired()
                    .HasColumnName("host_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDt)
                    .HasColumnName("insert_dt")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertUserId).HasColumnName("insert_user_id");

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ip_address")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsCurrentMachine).HasColumnName("is_current_machine");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.OsName)
                    .HasColumnName("os_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServerDescription)
                    .HasColumnName("server_description")
                    .HasMaxLength(250);

                entity.Property(e => e.ServerName)
                    .IsRequired()
                    .HasColumnName("server_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDt)
                    .HasColumnName("update_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");
            });

            modelBuilder.Entity<ServersUpdates>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("servers_updates");

                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

                entity.Property(e => e.CpuCoresCount).HasColumnName("cpu_cores_count");

                entity.Property(e => e.CpuCoresEnabledCount).HasColumnName("cpu_cores_enabled_count");

                entity.Property(e => e.CpuLogicalProcessorsCount).HasColumnName("cpu_logical_processors_count");

                entity.Property(e => e.CpuMaxClockSpeed).HasColumnName("cpu_max_clock_speed");

                entity.Property(e => e.CpuProcessorId)
                    .HasColumnName("cpu_processor_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CpuProcessorName)
                    .HasColumnName("cpu_processor_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemoryTotal)
                    .HasColumnName("memory_total")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ServerId)
                    .IsRequired()
                    .HasColumnName("server_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StorageTotal)
                    .HasColumnName("storage_total")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.SysIpAddress)
                    .HasColumnName("sys_ip_address")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SysSystemName)
                    .HasColumnName("sys_system_name")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDt)
                    .HasColumnName("transaction_dt")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<SetCounters>(entity =>
            {
                entity.HasKey(e => e.CounterId);

                entity.ToTable("set_counters");

                entity.Property(e => e.CounterId)
                    .HasColumnName("counter_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasColumnName("category_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryName)
                    .HasColumnName("category_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CounterDescription)
                    .HasColumnName("counter_description")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CounterName)
                    .IsRequired()
                    .HasColumnName("counter_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CounterUnit)
                    .HasColumnName("counter_unit")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasColumnType("decimal(8, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InstanceAllPrefix)
                    .HasColumnName("instance_all_prefix")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InstanceBlankName)
                    .HasColumnName("instance_blank_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InstanceTotalName)
                    .HasColumnName("instance_total_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<SetCountersCategories>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("set_counters_categories");

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasColumnName("category_description")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasColumnName("category_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<SetSerialno>(entity =>
            {
                entity.HasKey(e => e.IdentifierId);

                entity.ToTable("set_serialno");

                entity.Property(e => e.IdentifierId)
                    .HasColumnName("identifier_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SerialnoValue).HasColumnName("serialno_value");
            });

            modelBuilder.Entity<SysSettings>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sys_settings");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionCalculated>(entity =>
            {
                entity.ToTable("transaction_calculated");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Average)
                    .HasColumnName("average")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CounterId)
                    .IsRequired()
                    .HasColumnName("counter_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creation_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.InstanceId)
                    .HasColumnName("instance_id")
                    .HasMaxLength(150);

                entity.Property(e => e.MachineId)
                    .IsRequired()
                    .HasColumnName("machine_id")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Maximum)
                    .HasColumnName("maximum")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Minimum)
                    .HasColumnName("minimum")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<XxCountersTransactions>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("xx_counters_transactions");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CounterDatetime)
                    .HasColumnName("counter_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.CounterName)
                    .IsRequired()
                    .HasColumnName("counter_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CounterValue).HasColumnName("counter_value");

                entity.Property(e => e.InstanceName)
                    .HasColumnName("instance_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineName)
                    .HasColumnName("machine_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
