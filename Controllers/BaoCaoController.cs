using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class BaoCaoController : BaseController
    {
        private readonly IMauBieuService _mauBieuService;
        private readonly IKyBaoCaoService _kyBaoCaoService;
        string ControllerName = "mẫu biểu";
        public BaoCaoController(IMauBieuService mauBieuService, IKyBaoCaoService kyBaoCaoService)
        {
            _mauBieuService = mauBieuService;
            _kyBaoCaoService = kyBaoCaoService;
        }

        public ActionResult Index()
        {
            ViewBag.KyBaoCaos = _kyBaoCaoService.GetList(DateTime.Now.Year);
            return View();
        }
        public ActionResult RenderMauBieu(string id)
        {
            var data = _mauBieuService.RenderMauBieu(id);
            return View(data);
        }
        public ActionResult PhanQuyenBaoCao(string id)
        {
            ViewBag.TenMauBieu = _mauBieuService.GetById(id).Ten;
            ViewBag.MauBieuId = _mauBieuService.GetById(id).Id;
            return View();
        }
        public ActionResult LoadDanhSachDonViTheoMauBieuId(string id)
        {
            var data = _mauBieuService.GetDonViByMauBieuId(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PhanQuyenBaoCao(string mauBieuId, string donViId)
        {
            try
            {
                if (string.IsNullOrEmpty(mauBieuId))
                    throw new Exception("Mã mẫu biểu không được trống.");
                if (string.IsNullOrEmpty(donViId))
                    throw new Exception("Mã đơn vị không được trống.");
                _mauBieuService.PhanQuyenBaoCao(mauBieuId, donViId);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Phân quyền {ControllerName} thành công!",
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
        public ActionResult XoaDonViTrongBaoCao(string mauBieuId, string donViId)
        {
            try
            {
                if (string.IsNullOrEmpty(mauBieuId))
                    throw new Exception("Mã mẫu biểu không được trống.");
                if (string.IsNullOrEmpty(donViId))
                    throw new Exception("Mã đơn vị không được trống.");
                _mauBieuService.DeleteDonViInMauBieu(mauBieuId, donViId);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Xóa phân quyền {ControllerName} thành công!",
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