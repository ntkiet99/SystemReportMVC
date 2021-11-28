using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class MauBieuController : Controller
    {
        private readonly IMauBieuService _mauBieuService;
        string ControllerName = "mẫu biểu";
        public MauBieuController(IMauBieuService mauBieuService)
        {
            _mauBieuService = mauBieuService;
        }

        public ActionResult Index()
        {
            var data = _mauBieuService.GetTreeJS();
            return View();
        }
        [HttpGet]
        public ActionResult GetById(string id)
        {
            var data = _mauBieuService.GetById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadTreeDonVi()
        {

            var data = _mauBieuService.GetTreeJS();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult LoadData(DataTableAjaxPostModel datatableParams)
        //{
        //    int totalRecords = 0;
        //    var list = _mauBieuService.Pagination(out totalRecords, datatableParams);
        //    int recordsTotal = totalRecords;
        //    int recordsFiltered = list.Count;
        //    return Json(new
        //    {
        //        recordsTotal = recordsTotal,
        //        recordsFiltered = recordsTotal,
        //        data = list,
        //    }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult AddOrUpdate(string id)
        {
            var entity = new MauBieu();
            if (id == default(string))
            {
                ViewBag.Title = "Thêm mẫu biểu";
            }
            else
            {
                ViewBag.Title = "Cập nhật mẫu biểu";
                entity = _mauBieuService.GetById(id);
            }
            return PartialView("Add", entity);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(MauBieu model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                    throw new Exception("Mã mẫu biểu không được trống.");
                if (string.IsNullOrEmpty(model.Ten))
                    throw new Exception("Tên mẫu biểu không được trống.");

                var entity = _mauBieuService.GetById(model.Id);
                // Add
                if (entity == default(MauBieu))
                {
                    _mauBieuService.Create(model);
                    return Json(new GenericMessageVM()
                    {
                        Status = true,
                        Message = $"Thêm {ControllerName} thành công!",
                        MessageType = GenericMessage.success
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _mauBieuService.Update(model);
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
        public ActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Mã mẫu biểu không được trống.");
                _mauBieuService.Delete(id);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Xóa {ControllerName} thành công!",
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