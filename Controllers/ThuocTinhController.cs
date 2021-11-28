using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class ThuocTinhController : BaseController
    {
        private readonly IThuocTinhService _thuocTinhService;
        private readonly IMauBieuService _mauBieuService;
        private readonly IKyBaoCaoService _kyBaoCaoService;
        string ControllerName = "thuộc tính";
        public ThuocTinhController(
            IThuocTinhService thuocTinhService,
            IMauBieuService mauBieuService,
            IKyBaoCaoService kyBaoCaoService)
        {
            _mauBieuService = mauBieuService;
            _kyBaoCaoService = kyBaoCaoService;
            _thuocTinhService = thuocTinhService;
        }

        public ActionResult Index(string id)
        {
            try
            {
                ViewBag.MauBieuId = id;
                ViewBag.TenMauBieu = _mauBieuService.GetById(id)?.Ten;
            }
            catch (Exception)
            {
            }

            return View();
        }
        [HttpGet]
        public ActionResult GetById(string id)
        {
            var data = _thuocTinhService.GetById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadTreeDonVi(string id)
        {
            var data = _thuocTinhService.GetTreeJS(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult RenderTableThuocTinh(string id)
        {
            var data = _thuocTinhService.GetListOrderByLevel(id);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(data, Formatting.Indented, jss);
            return Json(JsonConvert.DeserializeObject<List<RenderThuocTinhVM>>(result), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdate(string id)
        {
            var entity = new ThuocTinh();
            if (id == default(string))
            {
                ViewBag.Title = $"Thêm {ControllerName}";
            }
            else
            {
                ViewBag.Title = $"Cập nhật {ControllerName}";
                entity = _thuocTinhService.GetById(id);
            }
            return PartialView("Add", entity);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(ThuocTinh model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                    throw new Exception($"Mã {ControllerName} không được trống.");

                var entity = _thuocTinhService.GetById(model.Id);
                // Add
                if (entity == default(ThuocTinh))
                {
                    _thuocTinhService.Create(model);
                    _mauBieuService.GenerateChiTieuThuocTinh(model.MauBieuId);
                    return Json(new GenericMessageVM()
                    {
                        Status = true,
                        Message = $"Thêm {ControllerName} thành công!",
                        MessageType = GenericMessage.success
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _thuocTinhService.Update(model);
                    _mauBieuService.GenerateChiTieuThuocTinh(model.MauBieuId);
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
                    throw new Exception($"Mã {ControllerName} không được trống.");
                _thuocTinhService.Delete(id);
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