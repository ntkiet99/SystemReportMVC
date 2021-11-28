using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class KyBaoCaoController : BaseController
    {
        private readonly IKyBaoCaoService _kyBaoCaoService;
        string ControllerName = "kỳ báo cáo";
        public KyBaoCaoController(IKyBaoCaoService kyBaoCaoService)
        {
            _kyBaoCaoService = kyBaoCaoService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetListKyBaoCaoByNam(int id = 0)
        {
            var data = _kyBaoCaoService.GetList(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var data = _kyBaoCaoService.GetById(id);
            var result = new KyBaoCaoVM();
            if (data != null)
            {
                result.Id = data.Id;
                result.Ten = data.Ten;
                result.NgayBanHanh = data.NgayBanHanh.HasValue? data.NgayBanHanh.Value.ToString("dd/MM/yyyy"): "";
                result.NgayGui = data.NgayGui.HasValue ? data.NgayGui.Value.ToString("dd/MM/yyyy") : "";
                result.DenNgay = data.DenNgay.HasValue ? data.DenNgay.Value.ToString("dd/MM/yyyy") : "";
                result.TuNgay = data.TuNgay.HasValue ? data.TuNgay.Value.ToString("dd/MM/yyyy") : "";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdate(int id = 0)
        {
            var entity = new KyBaoCao();
            if (id == default(int))
            {
                ViewBag.Title = "Thêm kỳ báo cáo";
            }
            else
            {
                ViewBag.Title = "Cập nhật đơn vị";
                entity = _kyBaoCaoService.GetById(id);
            }
            return PartialView("Add", entity);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(KyBaoCao model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Ten))
                    throw new Exception("Tên kỳ báo cáo không được trống.");

                // Add
                if (model.Id == default(int))
                {
                    _kyBaoCaoService.Create(model);
                    return Json(new GenericMessageVM()
                    {
                        Status = true,
                        Message = $"Thêm {ControllerName} thành công!",
                        MessageType = GenericMessage.success
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _kyBaoCaoService.Update(model);
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
        public ActionResult Delete(int id = 0)
        {
            try
            {
                _kyBaoCaoService.Delete(id);
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