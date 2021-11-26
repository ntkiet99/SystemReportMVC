using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SystemReportMVC.Data;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Services
{
    public class RoleService : IRoleService
    {
        private DataContext _context;
        private string EntityName = "quyền";
        public RoleService(DataContext context)
        {
            _context = context;
        }

        public Quyen GetById(int id)
        {
            return _context.Quyens.Include(x => x.QuyenMenu.Select(b => b.Menu))
                        .Where(x => x.IsDeleted != true && x.Id == id)
                        .FirstOrDefault();
        }

        public IEnumerable<Quyen> GetList()
        {
            var data = _context.Quyens.Include(x => x.QuyenMenu.Select(y => y.Menu))
                                    .Where(x => x.IsDeleted != true)
                                    .ToList();

            foreach (var item in data)
            {
                var menus = item.QuyenMenu?.Select(x => x.Menu).ToList();
                item.Menus.AddRange(menus);
            }

            return data;
        }

        public void Create(Quyen model)
        {
            if (string.IsNullOrWhiteSpace(model.Ten))
                throw new Exception($"Tên {EntityName} không được để trống.");

            var entity = _context.Quyens.AsQueryable()
                                            .Where(x => x.Ten.ToLower() == model.Ten.ToLower() && x.IsDeleted != true)
                                            .FirstOrDefault();

            if (entity != null)
                throw new Exception($"Tên {EntityName} đã tồn tại.");

            entity = new Quyen();
            entity.Code = model.Code;
            entity.Ten = model.Ten;
            entity.CreateAt = DateTime.Now;
            entity.IsDeleted = false;
            entity.AuditTs = DateTime.Now;

            _context.Quyens.Add(entity);
            _context.SaveChanges();

            /// Remove menu
            var menus = _context.QuyenMenus.Where(x => x.QuyenId == entity.Id).ToList();
            _context.QuyenMenus.RemoveRange(menus);
            _context.SaveChanges();

            var dsmenus = _context.Menus.Where(x => x.IsDeleted != true).ToList();

            foreach (var item in model.MenuIds)
            {
                var menu = dsmenus.Where(x => x.Id == item).FirstOrDefault();
                if (menu != default(Menu))
                    _context.QuyenMenus.Add(new QuyenMenu() { MenuId = menu.Id, QuyenId = entity.Id });
            }
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Quyens.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default(Quyen))
                throw new Exception("Không tìm thấy dữ liệu.");
            entity.IsDeleted = true;
            _context.SaveChanges();
        }

        public IEnumerable<Quyen> GetRoleByUserId(int userId)
        {
            var roles = _context.NguoiDungQuyens.Include(x => x.Quyen).Where(x => x.NguoiDungId == userId).Select(x => x.Quyen).ToList();
            return roles;
        }

        public void AddQuyenVaoNguoiDung(QuyenNguoiDungVM model)
        {
            var nguoiDung = _context.NguoiDungs.Where(x => x.IsDeleted != true && x.Id == model.Id).FirstOrDefault();
            if (nguoiDung == default(NguoiDung))
                throw new Exception("Không tìm thấy người dùng.");

            var quyenNguoiDungModels = new List<NguoiDungQuyen>();
            var quyen = _context.Quyens.Where(x => x.IsDeleted != true && model.QuyenIds.Contains(x.Id)).ToList() ?? new List<Quyen>();
            if (quyen.Count() <= 0)
                throw new Exception("Danh sách quyền rỗng.");

            var quyenNguoiDungs = _context.NguoiDungQuyens.Where(x => x.NguoiDungId == model.Id).ToList();
            if (quyenNguoiDungs.Count() > 0)
            {
                _context.NguoiDungQuyens.RemoveRange(quyenNguoiDungs);
                _context.SaveChanges();
            }

            foreach (var item in quyen)
            {
                quyenNguoiDungModels.Add(new NguoiDungQuyen() { NguoiDungId = nguoiDung.Id, QuyenId = item.Id });
            }
            _context.NguoiDungQuyens.AddRange(quyenNguoiDungModels);

            _context.SaveChanges();
        }
    }
}