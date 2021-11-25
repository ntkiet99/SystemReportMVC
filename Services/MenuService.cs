using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SystemReportMVC.Data;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;

namespace SystemReportMVC.Services
{
    public class MenuService : IMenuService
    {
        private DataContext _context;

        public MenuService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Menu> GetList()
        {
            var data = _context.Menus.Include(x => x.MenuCons)
                                        .Where(x => x.MenuChaId == null && x.IsDeleted != true)
                                        .ToList();
            return data;
        }

        public IEnumerable<Menu> GetParents()
        {
            var parents = _context.Menus.Where(x => x.MenuChaId == null && x.IsDeleted != true).ToList();
            return parents;
        }

        public void Create(Menu model)
        {
            if (string.IsNullOrEmpty(model.Ten))
                throw new Exception("Tên không được để trống.");
            var entity = _context.Menus.Where(x => x.Ten == model.Ten).FirstOrDefault();
            if (entity != default(Menu))
                throw new Exception("Tên danh mục đã tồn tại.");

            entity = new Menu();
            entity.Ten = model.Ten;
            entity.Path = model.Path;
            entity.Icon = model.Icon;
            entity.MenuChaId = model.MenuChaId;
            entity.Level = model.Level;
            entity.ThuTuHienThi = model.ThuTuHienThi;
            entity.IsDeleted = false;
            entity.AuditTs = DateTime.Now;
            entity.CreateAt = DateTime.Now;
            _context.Menus.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Menus.Where(x => x.Id == id).FirstOrDefault();
            if (entity == default(Menu))
                throw new Exception("Không tìm thấy.");
            entity.IsDeleted = true;
            _context.SaveChanges();
        }

        public Menu GetById(int id)
        {
            var entity = _context.Menus.Where(x => x.Id == id).FirstOrDefault();
            return entity;
        }

        public void Update(Menu model)
        {
            if (string.IsNullOrWhiteSpace(model.Ten))
                throw new Exception("Tên không được để trống.");

            var entity = _context.Menus.AsQueryable()
                                                .Where(x => x.Id == model.Id)
                                                .FirstOrDefault();
            if (entity == null)
                throw new Exception($"Không tìm thấy thực thể.");

            var entityNameExisted = _context.Menus.AsQueryable()
                                                           .Where(x => x.Id != model.Id && x.Ten.ToLower() == model.Ten.ToLower() && x.IsDeleted != true)
                                                           .FirstOrDefault();
            if (entityNameExisted != null)
                throw new Exception($"Tên danh mục đã tồn tại");

            entity.Ten = model.Ten;
            entity.Path = model.Path;
            entity.Icon = model.Icon;
            entity.MenuChaId = model.MenuChaId;
            entity.Level = model.Level;
            entity.ThuTuHienThi = model.ThuTuHienThi;
            entity.IsDeleted = false;
            entity.AuditTs = DateTime.Now;

            _context.SaveChanges();
        }
    }
}