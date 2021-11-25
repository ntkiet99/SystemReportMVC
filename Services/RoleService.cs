using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SystemReportMVC.Data;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;

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
    }
}