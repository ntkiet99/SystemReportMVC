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
    public class UserService : IUserService
    {
        private DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }


        public void Create(NguoiDung model)
        {
            if (string.IsNullOrEmpty(model.TaiKhoan))
                throw new Exception("Tên tài khoản không được để trống.");
            if (string.IsNullOrEmpty(model.HoTen))
                throw new Exception("Họ tên không được để trống.");

            var entity = new NguoiDung();
            entity.HoTen = model.HoTen;
            entity.Email = model.Email;
            entity.TaiKhoan = model.TaiKhoan;
            entity.MatKhau = model.MatKhau;
            entity.SoDienThoai = model.SoDienThoai;
            entity.HinhAnh = model.HinhAnh;
            entity.AuditTs = DateTime.Now;
            _context.NguoiDungs.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.NguoiDungs.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default(NguoiDung))
                throw new Exception("Không tìm thấy dữ liệu.");
            entity.IsDeleted = true;
            entity.AuditTs = DateTime.Now;
            _context.SaveChanges();
        }

        public void DeleteNguoiDungRaKhoiDonVi(NguoiDung model)
        {
            var nguoiDung = _context.NguoiDungs.Where(x => x.Id == model.Id).FirstOrDefault();
            if (nguoiDung == default(NguoiDung))
                throw new Exception("Không tìm thấy người dùng.");

            nguoiDung.DonViId = null;
            _context.SaveChanges();
        }
        public void AddNguoiDungVaoDonVi(NguoiDung model)
        {
            var donVi = _context.DonVis.Where(x => x.Id == model.DonViId).FirstOrDefault();
            if (donVi == default(DonVi))
                throw new Exception("Không tìm thấy đơn vị.");
            var nguoiDung = _context.NguoiDungs.Where(x => x.Id == model.Id).FirstOrDefault();
            if (donVi == default(DonVi))
                throw new Exception("Không tìm thấy người dùng.");

            nguoiDung.DonViId = donVi.Id;
            _context.SaveChanges();
        }

        public IEnumerable<NguoiDung> GetAll()
        {
            return _context.NguoiDungs.Include(x => x.DonVi).Include(x => x.NguoiDungQuyen.Select(b => b.Quyen)).Where(x => x.IsDeleted != true).ToList();
        }

        public NguoiDung GetById(int id)
        {
            return _context.NguoiDungs.Include(x => x.DonVi).Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
        }

        public IEnumerable<NguoiDung> GetNguoiDungChuaCoDonVi()
        {
            var data = _context.NguoiDungs.Include(x => x.DonVi).Where(x => x.IsDeleted != true && x.DonViId == null).ToList();
            return data;
        }

        public IEnumerable<NguoiDung> GetNguoiDungTheoDonVi(out int totalRecords, NguoiDungParam nguoiDungParam)
        {
            var query = _context.NguoiDungs.AsQueryable().Where(x => x.DonViId == nguoiDungParam.DonViId && x.IsDeleted != true);

            totalRecords = 0;
            int rowNum = nguoiDungParam.start < 10 ? 1 : nguoiDungParam.start + 1;

            var data = new List<NguoiDung>();


            data = query.OrderByDescending(x => x.AuditTs).Skip(nguoiDungParam.start).Take(nguoiDungParam.length).ToList();

            data = data
                .Select((currRow, index) => new NguoiDung()
                {
                    RowNum = rowNum + index,
                    Id = currRow.Id,
                    HoTen = currRow.HoTen,
                    TaiKhoan = currRow.TaiKhoan,
                }).ToList();

            totalRecords = _context.NguoiDungs.AsQueryable().Where(x => x.IsDeleted != true).Count();
            return data;
        }
        public void Update(NguoiDung model)
        {
            if (string.IsNullOrEmpty(model.TaiKhoan))
                throw new Exception("Tên tài khoản không được để trống.");
            if (string.IsNullOrEmpty(model.HoTen))
                throw new Exception("Họ tên không được để trống.");

            var entity = _context.NguoiDungs.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default(NguoiDung)) throw new Exception("Không tìm thấy người dùng.");

            var checkTen = _context.NguoiDungs.Where(x => x.Id != model.Id && x.IsDeleted != true && x.TaiKhoan == model.TaiKhoan).FirstOrDefault();
            if (checkTen != default(NguoiDung)) throw new Exception("Tên tài khoản đã tồn tại.");

            entity.HoTen = model.HoTen;
            entity.Email = model.Email;
            entity.TaiKhoan = model.TaiKhoan;
            entity.MatKhau = model.MatKhau;
            entity.SoDienThoai = model.SoDienThoai;
            entity.HinhAnh = model.HinhAnh;
            entity.AuditTs = DateTime.Now;
            _context.SaveChanges();
        }

        public IEnumerable<NguoiDungVM> GetNguoiDungTree()
        {
            var nguoiDungs = _context.NguoiDungs
                                    .Include(x => x.DonVi).AsEnumerable()
                                    .Where(x => x.IsDeleted != true)
                                    .OrderBy(x => x.AuditTs)
                                    .ToList()
                                    .Select(x => new NguoiDungVM()
                                    {
                                        id = x.Id.ToString(),
                                        text = x.TaiKhoan + "-" + x.HoTen,
                                        type = "text",
                                        state = new StateVM() { opened = true },
                                    }).ToList();

            return nguoiDungs;
        }
    }
}