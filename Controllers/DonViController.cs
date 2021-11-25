using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemReportMVC.Helpers;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class DonViController : Controller
    {
        private readonly IDonViService _donViService;
        string ControllerName = "đơn vị";
        public DonViController(IDonViService donViService)
        {
            _donViService = donViService;
        }

        public ActionResult Index()
        {
            var data = _donViService.GetTreeJS();
            return View();
        }
        [HttpGet]
        public ActionResult GetById(string id)
        {
            var data = _donViService.GetById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadTreeDonVi()
        {

            var data = _donViService.GetTreeJS();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadData(DataTableAjaxPostModel datatableParams)
        {
            int totalRecords = 0;
            var list = _donViService.Pagination(out totalRecords, datatableParams);
            int recordsTotal = totalRecords;
            int recordsFiltered = list.Count;
            return Json(new
            {
                recordsTotal = recordsTotal,
                recordsFiltered = recordsTotal,
                data = list,
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdate(string id)
        {
            var entity = new DonVi();
            if (id == default(string))
            {
                ViewBag.Title = "Thêm đơn vị";
            }
            else
            {
                ViewBag.Title = "Cập nhật đơn vị";
                entity = _donViService.GetById(id);
            }
            return PartialView("Add", entity);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(DonVi model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                    throw new Exception("Mã đơn vị không được trống.");
                if (string.IsNullOrEmpty(model.Ten))
                    throw new Exception("Tên đơn vị không được trống.");

                var entity = _donViService.GetById(model.Id);
                // Add
                if (entity == default(DonVi))
                {
                    _donViService.Create(model);
                    return Json(new GenericMessageVM()
                    {
                        Status = true,
                        Message = $"Thêm {ControllerName} thành công!",
                        MessageType = GenericMessage.success
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _donViService.Update(model);
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
                    throw new Exception("Mã đơn vị không được trống.");
               _donViService.Delete(id);
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