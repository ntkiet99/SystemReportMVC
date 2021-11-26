using System;
using System.Linq;
using System.Web.Mvc;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;
        string ControllerName = "danh mục";
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public ActionResult Index()
        {
            var list = _menuService.GetList();
            return View(list);
        }

        public ActionResult AddOrUpdate(int id = 0)
        {
            var parents = _menuService.GetParents();

            var entity = new Menu();
            if (id == default(int))
            {
                ViewBag.Title = $"Thêm {ControllerName}";
                ViewBag.ListOfParent = new SelectList(parents, "Id", "Ten"); ;
            }
            else
            {
                ViewBag.Title = $"Cập nhật {ControllerName}";
                entity = _menuService.GetById(id);
                parents = parents.Where(x => x.Id != id).ToList();
                ViewBag.ListOfParent = new SelectList(parents, "Id", "Ten", entity.Id); ;
            }
            return PartialView("Add", entity);
        }

        public ActionResult Delete(int id = 0)
        {
            ViewBag.Title = $"Xóa {ControllerName}";
            var entity = _menuService.GetById(id);
            return PartialView("Delete", entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Menu entity)
        {
            try
            {
                // Add
                if (entity.Id == default(int))
                {
                    _menuService.Create(entity);
                    return Json(new GenericMessageVM()
                    {
                        Status = true,
                        Message = $"Thêm {ControllerName} thành công!",
                        MessageType = GenericMessage.success
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _menuService.Update(entity);
                    return Json(new GenericMessageVM()
                    {
                        Status = true,
                        Message = $"Cập nhật {ControllerName} thành công!",
                        MessageType = GenericMessage.success
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new GenericMessageVM()
                {
                    Status = false,
                    Message = $"{ex.Message}",
                    MessageType = GenericMessage.error
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(Menu entity)
        {
            try
            {
                _menuService.Delete(entity.Id);

                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Xóa {ControllerName} thành công!",
                    MessageType = GenericMessage.success
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new GenericMessageVM()
                {
                    Status = false,
                    Message = $"{ex.Message}",
                    MessageType = GenericMessage.error
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}