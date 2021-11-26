using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemReportMVC.Data;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class QuyenController : Controller
    {
        public DataContext _context;
        private string ControllerName = "quyền";
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;

        public QuyenController(IRoleService roleService, IMenuService menuService)
        {
            _roleService = roleService;
            _menuService = menuService;
        }

        public ActionResult Index()
        {
            var list = _roleService.GetList();
            return View(list);
        }
        public ActionResult AddOrUpdate(int id = 0)
        {
            var entity = new Quyen();
            if (id == default(int))
            {
                ViewBag.Title = $"Thêm {ControllerName}";
                ViewBag.ListOfMenu = new SelectList(_menuService.GetList(), "Id", "Ten");
            }
            else
            {
                ViewBag.Title = $"Cập nhật {ControllerName}";
                entity = _roleService.GetById(id);
                var menuIds = entity.QuyenMenu.Select(x => x.Menu.Id).ToList();
                entity.MenuIds = menuIds;
                ViewBag.ListOfMenu = new MultiSelectList(_menuService.GetList(), "Id", "Ten", menuIds);
            }
            return PartialView("Add", entity);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(Quyen entity)
        {
            try
            {
                // Add
                if (entity.Id == default(int))
                {
                    _roleService.Create(entity);
                    return Json(new GenericMessageVM()
                    {
                        Status = true,
                        Message = $"Thêm {ControllerName} thành công!",
                        MessageType = GenericMessage.success
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //await _roleService.Update(AppUser, entity);
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
        public ActionResult Delete(int id)
        {
            ViewBag.Title = $"Xóa {ControllerName}";
            var entity = _roleService.GetById(id);
            return PartialView("Delete", entity);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Quyen entity)
        {
            try
            {
                _roleService.Delete(entity.Id);

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

        [HttpGet]
        public ActionResult GetQuyenByUserId(int id)
        {
            var data = _roleService.GetRoleByUserId(id);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(data.Select(x => x.Id).ToList(), Formatting.Indented, jss);
            return Json(JsonConvert.DeserializeObject<List<string>>(result), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetList()
        {
            var data = _roleService.GetList();
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(data, Formatting.Indented, jss);
            return Json(JsonConvert.DeserializeObject<List<Quyen>>(result), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddQuyenVaoNguoiDung(QuyenNguoiDungVM model)
        {
            try
            {
                _roleService.AddQuyenVaoNguoiDung(model);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Cập nhật {ControllerName} thành công!",
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