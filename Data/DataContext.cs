using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using SystemReportMVC.Helpers;
using SystemReportMVC.Models;

namespace SystemReportMVC.Data
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<DonVi> DonVis { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<NguoiDungQuyen> NguoiDungQuyens { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<QuyenMenu> QuyenMenus { get; set; }
        public virtual DbSet<TrangThai> TrangThais { get; set; }
        public virtual DbSet<HistoryData> HistoryDatas { get; set; }
        public static string GetCFConnection()
        {
            string Connection = "name=";

            string Machine = System.Environment.MachineName.ToLower();

            switch (Machine)
            {
                case "anhdev99":
                    Connection += @"DevConnectionString";
                    break;
                case "khanhvd":
                    Connection += @"ProductionConnectionString";
                    break;
            }

            return Connection;
        }
        public DataContext() : base(GetCFConnection())
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DonVi>().Ignore(e => e.RowNum);
            modelBuilder.Entity<DonVi>().HasOptional(d => d.DonViCha)
                    .WithMany(p => p.DonViCons)
                    .HasForeignKey(d => d.DonViChaId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Menu>().HasOptional(d => d.MenuCha)
                    .WithMany(p => p.MenuCons)
                    .HasForeignKey(d => d.MenuChaId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quyen>().Ignore(e => e.Menus);
            modelBuilder.Entity<Menu>().Ignore(e => e.Active);
            modelBuilder.Entity<Quyen>().Ignore(e => e.MenuIds);
            modelBuilder.Entity<NguoiDung>().HasOptional(d => d.DonVi)
                   .WithMany(p => p.NguoiDung)
                   .HasForeignKey(d => d.DonViId)
                   .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDungQuyen>().HasOptional(d => d.NguoiDung)
                    .WithMany(p => p.NguoiDungQuyen)
                    .HasForeignKey(d => d.NguoiDungId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDungQuyen>().HasOptional(d => d.Quyen)
                .WithMany(p => p.NguoiDungQuyen)
                .HasForeignKey(d => d.QuyenId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuyenMenu>().HasOptional(d => d.Menu)
                .WithMany(p => p.QuyenMenu)
                .HasForeignKey(d => d.MenuId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuyenMenu>().HasOptional(d => d.Quyen)
                .WithMany(p => p.QuyenMenu)
                .HasForeignKey(d => d.QuyenId)
                .WillCascadeOnDelete(false);
        }
        private bool IsEfRecordAudit { get; set; }
        private List<string> IgnoreTables = new List<string>();
        private List<string> IgnoreColumnsTables = new List<string>();
        private List<string> AuditTable = new List<string> { "HistoryData", "AuditAction", "Logs" };
        private string KeyId = "";
        private string ActionTitle { get; set; }
        private string RefColumnKey { get; set; }
        private string Reason { get; set; }
        private Guid ActionId { get; set; }
        private string TypeCode { get; set; }
        private string AuditCode { get; set; }
        private int? UserId { get; set; }
        private Guid? TraceId { get; set; }
        public DataContext WithTitle(string title = "")
        {
            if (!string.IsNullOrEmpty(title))
            {
                ActionTitle = title;
            }
            return this;
        }
        public int SaveChangesWithLogs(string auditCode = "", string key = "")
        {
            ConfigAuditData(auditCode, key);
            return SaveChanges();
        }
        public override int SaveChanges()
        {
            try
            {
                if (!IsEfRecordAudit)
                {
                    return base.SaveChanges();
                }
                ObjectStateManager objectStateManager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;
                GetContextInfo(this);
                IgnoreTables = GetIgnoreTables();
                IgnoreColumnsTables = GetIgnoreColumnsTables();
                List<AuditEntry> list = BeforeContextSaveChange(objectStateManager, this);
                IEnumerable<ObjectStateEntry> objectStateEntries = objectStateManager.GetObjectStateEntries(EntityState.Added);
                int result = base.SaveChanges();
                List<AuditEntry> list2 = AfterContextSaveChange(objectStateManager, objectStateEntries, this);
                if (list2.Count() > 0 || list.Count > 0)
                {
                    list.ForEach(delegate (AuditEntry audit)
                    {
                        AddToHistoryTable(audit);
                    });
                    list2.ForEach(delegate (AuditEntry audit)
                    {
                        AddToHistoryTable(audit);
                    });
                    return base.SaveChanges();
                }
                return result;
            }
            catch (DbEntityValidationException e)
            {
                //Create empty list to capture Validation error(s)
                var outputLines = new List<string>();

                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(
                        $"{DateTime.Now}: Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    outputLines.AddRange(eve.ValidationErrors.Select(ve =>
                        $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\""));
                }
                //Write to external file
                throw;
            }
        }
        private List<string> GetIgnoreTables()
        {
            return IgnoreTables;
        }
        private List<string> GetIgnoreColumnsTables()
        {
            return IgnoreColumnsTables;
        }
        private List<AuditEntry> BeforeContextSaveChange(ObjectStateManager entities, DataContext context)
        {
            List<AuditEntry> list = new List<AuditEntry>();
            foreach (ObjectStateEntry objectStateEntry2 in entities.GetObjectStateEntries(EntityState.Deleted | EntityState.Modified))
            {
                string name = objectStateEntry2.Entity.GetType().Name;
                if (context.IgnoreTables.Contains(name) || context.AuditTable.Contains(name) || context.IgnoreColumnsTables.Contains(name))
                {
                    continue;
                }
                string primaryKeys = GetPrimaryKeys(objectStateEntry2);
                AuditEntry auditEntry = new AuditEntry
                {
                    TableName = name,
                    KeyValues = primaryKeys
                };
                if (objectStateEntry2.State == EntityState.Modified)
                {
                    ObjectStateEntry objectStateEntry = entities.GetObjectStateEntry(objectStateEntry2.EntityKey);
                    CurrentValueRecord currentValues = objectStateEntry.CurrentValues;
                    DbDataRecord originalValues = objectStateEntry.OriginalValues;
                    foreach (string modifiedProperty in objectStateEntry.GetModifiedProperties())
                    {
                        object obj = originalValues[modifiedProperty];
                        object obj2 = currentValues[modifiedProperty];
                        auditEntry.OldValues[modifiedProperty] = obj;
                        if (!(obj.ToString() == obj2.ToString()))
                        {
                            auditEntry.NewValues[modifiedProperty] = obj2;
                        }
                    }
                    auditEntry.EventTable = "M";
                }
                else if (objectStateEntry2.State == EntityState.Deleted)
                {
                    DbDataRecord originalValues2 = entities.GetObjectStateEntry(objectStateEntry2.EntityKey).OriginalValues;
                    for (int i = 0; i < originalValues2.FieldCount; i++)
                    {
                        object value = originalValues2[i];
                        string name2 = originalValues2.GetName(i);
                        auditEntry.OldValues[name2] = value;
                        auditEntry.EventTable = "D";
                    }
                }
                list.Add(auditEntry);
            }
            return list;
        }
        private string GetPrimaryKeys(ObjectStateEntry entry)
        {
            string text = string.Empty;
            if (entry.EntityKey == null || entry.EntityKey.EntityKeyValues == null || entry.EntityKey.EntityKeyValues.Length == 0)
            {
                return "N/A";
            }
            EntityKeyMember[] entityKeyValues = entry.EntityKey.EntityKeyValues;
            foreach (EntityKeyMember entityKeyMember in entityKeyValues)
            {
                text += $"{entityKeyMember.Key}={entityKeyMember.Value};";
                KeyId = entityKeyMember.Value.ToString();
            }
            return text;
        }
        private List<AuditEntry> AfterContextSaveChange(ObjectStateManager entities, IEnumerable<ObjectStateEntry> addEntities, DataContext context)
        {
            List<AuditEntry> list = new List<AuditEntry>();
            foreach (ObjectStateEntry addEntity in addEntities)
            {
                string name = addEntity.Entity.GetType().Name;
                if (!context.IgnoreTables.Contains(name) && !context.AuditTable.Contains(name) && !context.IgnoreColumnsTables.Contains(name))
                {
                    string primaryKeys = GetPrimaryKeys(addEntity);
                    CurrentValueRecord currentValues = addEntity.CurrentValues;
                    AuditEntry auditEntry = new AuditEntry
                    {
                        TableName = name,
                        KeyValues = primaryKeys
                    };
                    for (int i = 0; i < currentValues.FieldCount; i++)
                    {
                        string name2 = currentValues.DataRecordInfo.FieldMetadata[i].FieldType.Name;
                        object value = currentValues[name2];
                        auditEntry.NewValues[name2] = value;
                        auditEntry.EventTable = "A";
                    }
                    list.Add(auditEntry);
                }
            }
            return list;
        }
        private void AddToHistoryTable(AuditEntry audit)
        {
            HistoryData entity = new HistoryData
            {
                HistoryDataId = Guid.NewGuid(),
                Action = ActionTitle,
                ColumnName = (string.IsNullOrEmpty(RefColumnKey) ? KeyId : RefColumnKey),
                RefValue = "",
                ActionId = ActionId,
                HistoryDataCode = TypeCode,
                HistoryDataName = AuditCode,
                EventTable = audit.EventTable,
                TableName = audit.TableName,
                PrimaryKey = audit.KeyValues,
                OldValue = ((audit.OldValues.Count == 0) ? null : JsonConvert.SerializeObject(audit.OldValues)),
                NewValue = ((audit.NewValues.Count == 0) ? null : JsonConvert.SerializeObject(audit.NewValues)),
                Deleted = false,
                TraceId = TraceId,
                UserId = UserId,
                CreatedAt = DateTime.Now
            };
            HistoryDatas.Add(entity);
        }
        private void ConfigAuditData(string auditCode, string key)
        {
            IsEfRecordAudit = true;
            AuditCode = auditCode;
            RefColumnKey = key;
        }
        public void Update<T>(T obj) where T : class
        {
            Entry(obj).State = EntityState.Modified;
        }

        public void Update<T>(DbSet<T> entity, T obj) where T : class
        {
            Entry(obj).State = EntityState.Modified;
        }
        public void Remove<T>(DbSet<T> entity, T obj) where T : class
        {
            Entry(obj).State = EntityState.Deleted;
        }
        public void Remove<T>(T obj) where T : class
        {
            Entry(obj).State = EntityState.Deleted;
        }
        private void GetContextInfo(DataContext context)
        {
            try
            {
                HttpContext current = HttpContext.Current;
                var session = (AppUser)current.Session[Constants.USER_SESSION];
                Guid actionId = Guid.NewGuid();
                if (session != null)
                {
                    context.ActionId = actionId;
                    context.TypeCode = "MVC";
                    context.UserId = session.Id;
                    context.ActionTitle = (string.IsNullOrEmpty(context.ActionTitle) ? "" : context.ActionTitle);
                    context.TraceId = Guid.NewGuid(); ;
                    return;
                }            
                else
                {
                    context.ActionId = actionId;
                    context.UserId = -1;
                }
            }
            catch
            {
            }
        }

    }
}
