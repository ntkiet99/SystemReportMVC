using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using SystemReportMVC.Helpers;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class NguoiDungController : BaseController
    {
        // GET: NguoiDung
        private readonly IUserService _userService;
        string ControllerName = "người dùng";
        public NguoiDungController(IUserService userService)
        {
            _userService = userService;
        }
        public ActionResult Index()
        {
            var list = _userService.GetAll();
            return View(list);
        }
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var data = _userService.GetById(id);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(data, Formatting.Indented, jss);
            return Json(JsonConvert.DeserializeObject<NguoiDung>(result), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdate(int id = 0)
        {
            var entity = new NguoiDung();
            if (id == default(int))
            {
                ViewBag.Title = $"Thêm {ControllerName}";
            }
            else
            {
                ViewBag.Title = $"Cập nhật {ControllerName}";
                entity = _userService.GetById(id);
            }
            return PartialView("Add", entity);
        }

        public ActionResult Delete(int id = 0)
        {
            ViewBag.Title = $"Xóa {ControllerName}";
            var entity = _userService.GetById(id);
            return PartialView("Delete", entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(NguoiDung entity)
        {
            try
            {
                // Add
                if (entity.Id == default(int))
                {
                    _userService.Create(entity);
                    return Json(new GenericMessageVM()
                    {
                        Status = true,
                        Message = $"Thêm {ControllerName} thành công!",
                        MessageType = GenericMessage.success
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _userService.Update(entity);
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
        public ActionResult Delete(NguoiDung entity)
        {
            try
            {
                _userService.Delete(entity.Id);

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
        public ActionResult PhanDonVi()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PhanQuyen()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetNguoiDungChuaCoDonVi()
        {
            var nguoiDungChuaDonVi = _userService.GetNguoiDungChuaCoDonVi();
            return Json(new
            {
                data = nguoiDungChuaDonVi,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetNguoiDungTheoDonVi(NguoiDungParam nguoiDungParam)
        {
            int totalRecords = 0;
            var list = _userService.GetNguoiDungTheoDonVi(out totalRecords, nguoiDungParam);
            int recordsTotal = totalRecords;
            return Json(new
            {
                recordsTotal = recordsTotal,
                recordsFiltered = recordsTotal,
                data = list,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddNguoiDungVaoDonVi(NguoiDung model)
        {

            try
            {
                _userService.AddNguoiDungVaoDonVi(model);

                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Thêm người dùng vào đơn vị thành công!",
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

        [HttpPost]
        public ActionResult DeleteNguoiDungRaKhoiDonVi(NguoiDung model)
        {
            try
            {
                _userService.DeleteNguoiDungRaKhoiDonVi(model);

                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Xóa người dùng ra khỏi đơn vị thành công!",
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
        public JsonResult GetNguoiDungTree()
        {
            var data = _userService.GetNguoiDungTree();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}