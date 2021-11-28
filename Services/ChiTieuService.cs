using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemReportMVC.Data;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;
using System.Data.Entity;
using SystemReportMVC.Helpers;

namespace SystemReportMVC.Services
{
    public class ChiTieuService : IChiTieuService
    {
        public readonly DataContext _context;
        string ServiceName = "chỉ tiêu";
        public ChiTieuService(DataContext context)
        {
            _context = context;
        }
        public void Create(ChiTieu model)
        {
            var checkId = _context.ChiTieus.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();
            if (checkId != default(ChiTieu))
                throw new Exception($"Mã {ServiceName} đã tồn tại.");
            var intId = _context.ChiTieus.OrderByDescending(x =>new { x.IdInt, x.AuditTs}).FirstOrDefault()?.IdInt??0;
            var entity = new ChiTieu();
            entity.Id = model.Id;
            entity.IdInt = intId;
            entity.TenChiTieu = model.TenChiTieu;
            entity.NhanChiTieu = model.NhanChiTieu;
            entity.ThuTu = model.ThuTu;
            entity.KieuDuLieu = model.KieuDuLieu;
            entity.MauBieuId = model.MauBieuId;
            entity.CreateAt = DateTime.Now;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;
            var chiTieuCha = GetById(model.ChiTieuChaId);
            if (chiTieuCha == default(ChiTieu))
            {
                entity.ChiTieuChaId = null;
                entity.Level = 0;
            }
            else
            {
                entity.Level = chiTieuCha.Level + 1;
                entity.ChiTieuChaId = chiTieuCha.Id;
            }
            _context.ChiTieus.Add(entity);
            _context.WithTitle($"Thêm {ServiceName}").SaveChangesWithLogs();
        }

        public void Delete(string id)
        {
            var entity = _context.ChiTieus.Include(x => x.ChiTieuCons).Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity.ChiTieuCons.Count > 0)
                throw new Exception($"Tồn tại {ServiceName} con.");
            if (entity == default(ChiTieu))
                throw new Exception("Không tìm thấy.");
            _context.Remove(entity);
            _context.WithTitle($"Xóa {ServiceName}").SaveChangesWithLogs();
        }

        public IEnumerable<ChiTieu> Get()
        {
            var data = _context.ChiTieus.Include(x => x.ChiTieuCons).Where(x => x.IsDeleted != true).ToList();
            return data;
        }

        public ChiTieu GetById(string id)
        {
            var entity = _context.ChiTieus.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public void Update(ChiTieu model)
        {
            var entity = _context.ChiTieus.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();

            //var checkTen = _context.DonVis.Where(x => x.Ten == model.Ten && x.Id != model.Id && x.IsDeleted != true).FirstOrDefault();
            //if (checkTen != default(DonVi))
            //    throw new Exception("Tên đơn vị đã tồn tại.");

            entity.TenChiTieu = model.TenChiTieu;
            entity.NhanChiTieu = model.NhanChiTieu;
            entity.ThuTu = model.ThuTu;
            entity.KieuDuLieu = model.KieuDuLieu;
            entity.MauBieuId = model.MauBieuId;
            entity.CreateAt = DateTime.Now;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;
            var chiTieuCha = GetById(model.ChiTieuChaId);
            if (chiTieuCha == default(ChiTieu))
            {
                entity.ChiTieuChaId = null;
                entity.Level = 0;
            }
            else
            {
                entity.Level = chiTieuCha.Level + 1;
                entity.ChiTieuChaId = chiTieuCha.Id;
            }
            _context.Update(entity);
            _context.WithTitle($"Cập nhật {ServiceName}").SaveChangesWithLogs();
        }

        public List<ChiTieuVM> GetTreeJS(string mauBieuId)
        {
            var donViChas = _context.ChiTieus
                                    .Include(x => x.ChiTieuCons).AsEnumerable()
                                    .Where(x => x.ChiTieuChaId == null && x.IsDeleted != true && x.MauBieuId == mauBieuId)
                                    .OrderBy(x => x.ThuTu)
                                    .Select(x => new ChiTieuVM()
                                    {
                                        id = x.Id.ToString(),
                                        text = x.TenChiTieu,
                                        type = "text",
                                        state = new StateVM() { opened = true },
                                        children = x.GetAllChildrens(x.ChiTieuCons)
                                    }).ToList();
            return donViChas;
        }
    }
}