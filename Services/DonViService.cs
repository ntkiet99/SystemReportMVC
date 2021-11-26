using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SystemReportMVC.Data;
using SystemReportMVC.Helpers;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Services
{
    public class DonViService : IDonViService
    {
        public readonly DataContext _context;
        public DonViService(DataContext context)
        {
            _context = context;
        }
        public void Create(DonVi model)
        {
            var checkId = _context.DonVis.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();
            if (checkId != default(DonVi))
                throw new Exception("Mã đơn vị đã tồn tại.");

            var entity = new DonVi();
            entity.Id = model.Id;
            entity.Ten = model.Ten;
            entity.TenVietTat = model.TenVietTat;
            entity.CreateAt = DateTime.Now;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;
            var donViCha = GetById(model.DonViChaId);
            if (donViCha == default(DonVi))
            {
                entity.DonViChaId = null;
                entity.Level = 0;
            }
            else
            {
                entity.Level = donViCha.Level + 1;
                entity.DonViChaId = donViCha.Id;
            }
            _context.DonVis.Add(entity);
            _context.WithTitle("Thêm đơn vị").SaveChangesWithLogs();
        }

        public void Delete(string id)
        {
            var entity = _context.DonVis.Include(x => x.DonViCons).Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity.DonViCons.Count > 0)
                throw new Exception("Tồn tại đơn vị con.");
            if (entity == default(DonVi))
                throw new Exception("Không tìm thấy.");
            _context.Remove(entity);
            _context.WithTitle("Xóa đơn vị").SaveChangesWithLogs();
        }

        public IEnumerable<DonVi> Get()
        {
            var data = _context.DonVis.Include(x => x.DonViCons).Where(x => x.IsDeleted != true).ToList();
            return data;
        }

        public DonVi GetById(string id)
        {
            var entity = _context.DonVis.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            return entity;
        }

        public List<DonVi> Pagination(out int totalRecords, DataTableAjaxPostModel datatableParams)
        {
            totalRecords = 0;
            int rowNum = datatableParams.start < 10 ? 1 : datatableParams.start + 1;

            var data = new List<DonVi>();
            if (datatableParams.search.value != null)
            {
                data = _context.DonVis.AsQueryable().Where(x => x.Ten.ToLower().Contains(datatableParams.search.value.ToLower()) && x.IsDeleted != true).ToList();
            }
            else
            {
                data = _context.DonVis.AsQueryable().Where(x => x.IsDeleted != true).ToList();
            }

            data = data.OrderByDescending(x => x.AuditTs).Skip(datatableParams.start).Take(datatableParams.length).ToList();

            data = data
                .Select((currRow, index) => new DonVi()
                {
                    RowNum = rowNum + index,
                    Id = currRow.Id,
                    Ten = currRow.Ten
                }).ToList();

            totalRecords = _context.DonVis.AsQueryable().Where(x => x.IsDeleted != true).Count();
            return data;
        }

        public void Update(DonVi model)
        {
            var entity = _context.DonVis.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();

            //var checkTen = _context.DonVis.Where(x => x.Ten == model.Ten && x.Id != model.Id && x.IsDeleted != true).FirstOrDefault();
            //if (checkTen != default(DonVi))
            //    throw new Exception("Tên đơn vị đã tồn tại.");

            entity.Ten = model.Ten;
            entity.TenVietTat = model.TenVietTat;
            entity.DonViChaId = model.DonViChaId;
            entity.AuditTs = DateTime.Now;
            var donViCha = GetById(model.DonViChaId);
            if (donViCha == default(DonVi))
            {
                entity.DonViChaId = null;
                entity.Level = 0;
            }
            else
            {
                entity.Level = donViCha.Level + 1;
                entity.DonViChaId = donViCha.Id;
            }

            _context.Update(entity);
            _context.WithTitle("Cập nhật đơn vị").SaveChangesWithLogs();
        }

        public List<DonViVM> GetTreeJS()
        {
            var datatest = _context.DonVis.Include(x => x.DonViCons).ToList();
            var donViChas = _context.DonVis
                                    .Include(x => x.DonViCons).AsEnumerable()
                                    .Where(x => x.DonViChaId == null && x.IsDeleted != true)
                                    .OrderBy(x => x.AuditTs)
                                    .Select(x => new DonViVM()
                                    {
                                        id = x.Id.ToString(),
                                        text = x.Ten,
                                        type = "root",
                                        state = new StateVM() { opened = true },
                                        children = x.GetAllChildrens(x.DonViCons)
                                    }).ToList();
            return donViChas;
        }
    }
}