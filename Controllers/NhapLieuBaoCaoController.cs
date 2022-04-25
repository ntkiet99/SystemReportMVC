using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Controllers
{
    public class NhapLieuBaoCaoController : BaseController
    {
        private readonly IDonViService _donViService;
        private readonly IMauBieuService _mauBieuService;
        private readonly IThuocTinhService _thuocTinhService;
        public NhapLieuBaoCaoController(IMauBieuService mauBieuService, IDonViService donViService, IThuocTinhService thuocTinhService)
        {
            _mauBieuService = mauBieuService;
            _donViService = donViService;
            _thuocTinhService = thuocTinhService;
        }

        public ActionResult Index()
        {
            var data = _mauBieuService.ListMauBaoCaoByDonViId(AppUser.DonViId);
            ViewBag.DonViId = AppUser.DonViId;
            ViewBag.DuLieu = _mauBieuService.GetDuLieuMau(AppUser.DonViId);
            return View(data);
        }
        public ActionResult KiemTra()
        {
            var data = _mauBieuService.ListMauBaoCaoByDonViId(AppUser.DonViId);
            ViewBag.DonViId = AppUser.DonViId;
            ViewBag.DuLieu = _mauBieuService.GetDuLieuMau(AppUser.DonViId);
            return View(data);
        }
        public ActionResult BanHanh()
        {
            var data = _mauBieuService.ListMauBaoCaoByDonViId(AppUser.DonViId);
            ViewBag.DonViId = AppUser.DonViId;
            ViewBag.DuLieu = _mauBieuService.GetDuLieuMau(AppUser.DonViId);
            return View(data);
        }
        public ActionResult TongHop()
        {
            var data = _mauBieuService.ListMauBaoCaoByDonViId(AppUser.DonViId);
            ViewBag.DonViId = AppUser.DonViId;
            ViewBag.DuLieu = _mauBieuService.GetDuLieuMau(AppUser.DonViId);
            return View(data);
        }
        public ActionResult Render(string id)
        {
            var data = _mauBieuService.RenderMauBieuNhapLieu(AppUser, id);
            return View(data);
        }

        [HttpPost]
        public ActionResult SaveData(List<InputFormVM> inputs)
        {
            try
            {
                _mauBieuService.SaveData(inputs);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Lưu dữ liệu thành công!",
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
        public ActionResult NhapLieuBC(List<ThuocTinhNhapLieu> inputs)
        {
            try
            {
                _thuocTinhService.NhapLieuThuocTinh(inputs);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Lưu dữ liệu thành công!",
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
        public ActionResult TrangThaiNhapLieu(string mauBieuId, string donViId)
        {
            try
            {
                _mauBieuService.TrangThaiNhapLieu(mauBieuId, donViId);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Nhập liệu thành công!",
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
        public ActionResult TrangThaiDuyet(string mauBieuId, string donViId)
        {
            try
            {
                _mauBieuService.TrangThaiDuyet(mauBieuId, donViId);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Duyệt thành công!",
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
        public ActionResult TrangThaiXuatBan(string mauBieuId, string donViId)
        {
            try
            {
                _mauBieuService.TrangThaiXuatBan(mauBieuId, donViId);
                return Json(new GenericMessageVM()
                {
                    Status = true,
                    Message = $"Xuất bản thành công!",
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