using System;
using System.Collections.Generic;
using System.Linq;
using SystemReportMVC.Data;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;

namespace SystemReportMVC.Services
{
    public class KyBaoCaoService : IKyBaoCaoService
    {
        private DataContext _context;
        public KyBaoCaoService(DataContext context)
        {
            _context = context;
        }

        public void Create(KyBaoCao model)
        {
            if (string.IsNullOrEmpty(model.Ten))
                throw new Exception("Tên không được để trống.");
            var entity = _context.KyBaoCaos.Where(x => x.Ten == model.Ten && x.Nam == model.Nam && x.IsDeleted != true).FirstOrDefault();
            if (entity != default(KyBaoCao))
                throw new Exception("Tên kỳ báo cáo không được trùng");

            entity = new KyBaoCao();
            entity.Nam = model.Nam;
            entity.Ten = model.Ten;
            entity.NgayBanHanh = model.NgayBanHanh;
            entity.NgayGui = model.NgayGui;
            entity.TuNgay = model.TuNgay;
            entity.DenNgay = model.DenNgay;
            entity.CreateAt = DateTime.Now;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;

            _context.KyBaoCaos.Add(entity);
            _context.WithTitle("Thêm kỳ báo cáo").SaveChangesWithLogs();
        }

        public void Delete(int id)
        {
            var entity = _context.KyBaoCaos.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default(KyBaoCao))
                throw new Exception("Không tìm thấy dữ liệu");
            entity.IsDeleted = true;
            _context.Update(entity);
            _context.WithTitle("Xóa kỳ báo cáo").SaveChangesWithLogs();
        }

        public KyBaoCao GetById(int id)
        {
            return _context.KyBaoCaos.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefault();
        }

        public IEnumerable<KyBaoCao> GetList(int nam)
        {
            return _context.KyBaoCaos.Where(x => x.Nam == nam && x.IsDeleted != true).ToList();
        }

        public void Update(KyBaoCao model)
        {
            var entity = _context.KyBaoCaos.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default(KyBaoCao))
                throw new Exception("Kỳ báo cáo không được trống");
            var checkTen = _context.KyBaoCaos.Where(x => x.Ten == model.Ten && x.Nam == model.Nam && x.Id != model.Id && x.IsDeleted != true).FirstOrDefault();
            if (checkTen != default(KyBaoCao))
                throw new Exception("Tên kỳ báo cáo không được trùng");

            entity.Nam = model.Nam;
            entity.Ten = model.Ten;
            entity.NgayBanHanh = model.NgayBanHanh;
            entity.NgayGui = model.NgayGui;
            entity.TuNgay = model.TuNgay;
            entity.DenNgay = model.DenNgay;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;

            _context.Update(entity);
            _context.WithTitle("Cập nhật kỳ báo cáo").SaveChangesWithLogs();
        }
    }
}