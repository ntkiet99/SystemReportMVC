using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemReportMVC.Data;
using SystemReportMVC.Interfaces;
using System.Data.Entity;
using SystemReportMVC.Models;
using SystemReportMVC.Helpers;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Services
{
    public class ThuocTinhService : IThuocTinhService
    {
        public readonly DataContext _context;
        string ServiceName = "thuộc tính";
        public ThuocTinhService(DataContext context)
        {
            _context = context;
        }
        public void Create(ThuocTinh model)
        {
            var checkId = _context.ThuocTinhs.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();
            if (checkId != default(ThuocTinh))
                throw new Exception($"Mã {ServiceName} đã tồn tại.");
            var intId = _context.ThuocTinhs.OrderByDescending(x => x.IdInt).FirstOrDefault()?.IdInt ?? 0;
            var entity = new ThuocTinh();
            entity.Id = model.Id;
            entity.IdInt = intId;
            entity.TenThuocTinh = model.TenThuocTinh;
            entity.NhanThuocTinh = model.NhanThuocTinh;
            entity.ThuTu = model.ThuTu;
            entity.KieuDuLieu = model.KieuDuLieu;
            entity.Row = model.Row;
            entity.Col = model.Col;
            entity.Width = model.Width;
            entity.MauBieuId = model.MauBieuId;
            entity.CreateAt = DateTime.Now;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;
            var thuocTinhCha = GetById(model.ThuocTinhChaId);
            if (thuocTinhCha == default(ThuocTinh))
            {
                entity.ThuocTinhChaId = null;
                entity.Level = 0;
            }
            else
            {
                entity.Level = thuocTinhCha.Level + 1;
                entity.ThuocTinhChaId = thuocTinhCha.Id;
            }
            _context.ThuocTinhs.Add(entity);
            _context.WithTitle($"Thêm {ServiceName}").SaveChangesWithLogs();
        }

        public void Delete(string id)
        {
            var entity = _context.ThuocTinhs.Include(x => x.ThuocTinhCons).Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity.ThuocTinhCons.Count > 0)
                throw new Exception($"Tồn tại {ServiceName} con.");
            if (entity == default(ThuocTinh))
                throw new Exception("Không tìm thấy.");
            _context.Remove(entity);
            _context.WithTitle($"Xóa {ServiceName}").SaveChangesWithLogs();
        }

        public IEnumerable<ThuocTinh> Get()
        {
            var data = _context.ThuocTinhs.Include(x => x.ThuocTinhCons).Where(x => x.IsDeleted != true).ToList();
            return data;
        }

        public ThuocTinh GetById(string id)
        {
            var entity = _context.ThuocTinhs.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public void Update(ThuocTinh model)
        {
            var entity = _context.ThuocTinhs.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();

            //var checkTen = _context.DonVis.Where(x => x.Ten == model.Ten && x.Id != model.Id && x.IsDeleted != true).FirstOrDefault();
            //if (checkTen != default(DonVi))
            //    throw new Exception("Tên đơn vị đã tồn tại.");

            entity.TenThuocTinh = model.TenThuocTinh;
            entity.NhanThuocTinh = model.NhanThuocTinh;
            entity.ThuTu = model.ThuTu;
            entity.KieuDuLieu = model.KieuDuLieu;
            entity.MauBieuId = model.MauBieuId;
            entity.Row = model.Row;
            entity.Col = model.Col;
            entity.Width = model.Width;
            entity.CreateAt = DateTime.Now;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;
            var thuocTinhCha = GetById(model.ThuocTinhChaId);
            if (thuocTinhCha == default(ThuocTinh))
            {
                entity.ThuocTinhChaId = null;
                entity.Level = 0;
            }
            else
            {
                entity.Level = thuocTinhCha.Level + 1;
                entity.ThuocTinhChaId = thuocTinhCha.Id;
            }
            _context.Update(entity);
            _context.WithTitle($"Cập nhật {ServiceName}").SaveChangesWithLogs();
        }

        public List<ThuocTinhVM> GetTreeJS(string bieuMauId)
        {
            var donViChas = _context.ThuocTinhs
                                    .Include(x => x.ThuocTinhCons).AsEnumerable()
                                    .Where(x => x.ThuocTinhChaId == null && x.IsDeleted != true && x.MauBieuId == bieuMauId)
                                    .OrderBy(x => x.ThuTu)
                                    .Select(x => new ThuocTinhVM()
                                    {
                                        id = x.Id.ToString(),
                                        text = x.TenThuocTinh,
                                        type = "text",
                                        state = new StateVM() { opened = true },
                                        children = x.GetAllChildrens(x.ThuocTinhCons)
                                    }).ToList();
            return donViChas;
        }

        public List<RenderThuocTinhVM> GetListOrderByLevel(string bieuMauId)
        {
            var data = _context.ThuocTinhs.Where(x => x.IsDeleted != true && x.MauBieuId == bieuMauId).OrderBy(x => x.Level).ToList();
            var result = new List<RenderThuocTinhVM>();
            if (data.Count() <= 0)
                return result;
            var maxLevel = data.Max(x => x.Level).Value;
            for (int i = 0; i <= maxLevel; i++)
            {
                var tempThuocTinh = data.Where(x => x.Level == i).OrderBy(x => x.ThuTu).ToList();
                var tempRender = new RenderThuocTinhVM();
                tempRender.Level = i;
                tempRender.ThuocTinhs.AddRange(tempThuocTinh);
                result.Add(tempRender);
            }
            return result;

        }

        public List<ThuocTinh> GetListMaxLevel(string bieuMauId)
        {
            var data = _context.ThuocTinhs.Include(x => x.ThuocTinhCons).Where(x => x.IsDeleted != true && x.MauBieuId == bieuMauId).OrderBy(x => x.Level).ToList();
            //var result = new List<RenderThuocTinhVM>();
            if (data.Count() <= 0)
                return new List<ThuocTinh>();
            var maxLevel = data.Max(x => x.Level).Value;
            var tempThuocTinh = data.Where(x => x.ThuocTinhCons.Count == 0).OrderBy(x => x.ThuTu).ToList();
            return tempThuocTinh;
        }

        public void NhapLieuThuocTinh(List<ThuocTinhNhapLieu> list)
        {
            var first = list.FirstOrDefault();
            var data = _context.ThuocTinhNhapLieus.Where(x => x.ThuocTinhId == first.ThuocTinhId && x.MauBieuId == first.MauBieuId).ToList();
            _context.ThuocTinhNhapLieus.RemoveRange(data);
            _context.SaveChanges();
            foreach (var item in list)
            {
                _context.ThuocTinhNhapLieus.Add(item);
            }
            _context.SaveChanges();
        }
    }
}