using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SystemReportMVC.Data;
using SystemReportMVC.Helpers;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;
using SystemReportMVC.ViewModels;

namespace SystemReportMVC.Services
{
    public class MauBieuService : IMauBieuService
    {
        public readonly DataContext _context;
        private readonly IThuocTinhService _thuocTinhService;
        public MauBieuService(DataContext context, IThuocTinhService thuocTinhService)
        {
            _context = context;
            _thuocTinhService = thuocTinhService;
        }
        public void Create(MauBieu model)
        {
            var checkId = _context.MauBieus.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();
            if (checkId != default(MauBieu))
                throw new Exception("Mã đơn vị đã tồn tại.");

            var entity = new MauBieu();
            entity.Id = model.Id;
            entity.Ten = model.Ten;
            entity.KyHieu = model.KyHieu;
            entity.NhomMauBieu = model.NhomMauBieu;
            entity.GhiChu = model.GhiChu;
            entity.CreateAt = DateTime.Now;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;
            var mauBieuCha = GetById(model.MauBieuChaId);
            if (mauBieuCha == default(MauBieu))
            {
                entity.MauBieuChaId = null;
                entity.Level = 0;
            }
            else
            {
                entity.Level = mauBieuCha.Level + 1;
                entity.MauBieuChaId = mauBieuCha.Id;
            }
            _context.MauBieus.Add(entity);
            _context.WithTitle("Thêm mẫu biểu").SaveChangesWithLogs();
        }

        public void Delete(string id)
        {
            var entity = _context.MauBieus.Include(x => x.MauBieuCons).Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity.MauBieuCons.Count > 0)
                throw new Exception("Tồn tại mẫu biểu con.");
            if (entity == default(MauBieu))
                throw new Exception("Không tìm thấy.");
            _context.Remove(entity);
            _context.WithTitle("Xóa mẫu biểu").SaveChangesWithLogs();
        }

        public IEnumerable<MauBieu> Get()
        {
            var data = _context.MauBieus.Include(x => x.MauBieuCons).Where(x => x.IsDeleted != true).ToList();
            return data;
        }

        public MauBieu GetById(string id)
        {
            var entity = _context.MauBieus.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            return entity;
        }

        public List<DonVi> Pagination(out int totalRecords, DataTableAjaxPostModel datatableParams)
        {
            totalRecords = 0;
            int rowNum = datatableParams.start < 10 ? 1 : datatableParams.start + 1;

            var data = new List<DonVi>();
            if (datatableParams.search.value != null)
            {
                data = _context.DonVis.AsQueryable().Where(x => x.Ten.ToLower().Contains(datatableParams.search.value.ToLower()) && x.IsDeleted != true).ToList();
            }
            else
            {
                data = _context.DonVis.AsQueryable().Where(x => x.IsDeleted != true).ToList();
            }

            data = data.OrderByDescending(x => x.AuditTs).Skip(datatableParams.start).Take(datatableParams.length).ToList();

            data = data
                .Select((currRow, index) => new DonVi()
                {
                    RowNum = rowNum + index,
                    Id = currRow.Id,
                    Ten = currRow.Ten
                }).ToList();

            totalRecords = _context.DonVis.AsQueryable().Where(x => x.IsDeleted != true).Count();
            return data;
        }

        public void Update(MauBieu model)
        {
            var entity = _context.MauBieus.Where(x => x.Id == model.Id && x.IsDeleted != true).FirstOrDefault();

            //var checkTen = _context.DonVis.Where(x => x.Ten == model.Ten && x.Id != model.Id && x.IsDeleted != true).FirstOrDefault();
            //if (checkTen != default(DonVi))
            //    throw new Exception("Tên đơn vị đã tồn tại.");

            entity.Ten = model.Ten;
            entity.KyHieu = model.KyHieu;
            entity.NhomMauBieu = model.NhomMauBieu;
            entity.GhiChu = model.GhiChu;
            entity.CreateAt = DateTime.Now;
            entity.AuditTs = DateTime.Now;
            entity.IsDeleted = false;
            var mauBieuCha = GetById(model.MauBieuChaId);
            if (mauBieuCha == default(MauBieu))
            {
                entity.MauBieuChaId = null;
                entity.Level = 0;
            }
            else
            {
                entity.Level = mauBieuCha.Level + 1;
                entity.MauBieuChaId = mauBieuCha.Id;
            }

            _context.Update(entity);
            _context.WithTitle("Cập nhật mẫu biểu").SaveChangesWithLogs();
        }

        public List<MauBieuVM> GetTreeJS()
        {
            var donViChas = _context.MauBieus
                                    .Include(x => x.MauBieuCons).AsEnumerable()
                                    .Where(x => x.MauBieuChaId == null && x.IsDeleted != true)
                                    .OrderBy(x => x.AuditTs)
                                    .Select(x => new MauBieuVM()
                                    {
                                        id = x.Id.ToString(),
                                        text = x.Ten,
                                        type = x.NhomMauBieu == 1 ? "root" : "text",
                                        state = new StateVM() { opened = true },
                                        children = x.GetAllChildrens(x.MauBieuCons)
                                    }).ToList();
            return donViChas;
        }

        public void GenerateChiTieuThuocTinh(string mauBieuId)
        {
            var ChiTieuThuocTinhs = _context.ChiTieuThuocTinhs.Where(x => x.IsDeleted != true && x.MauBieuId == mauBieuId).ToList();
            foreach (var item in ChiTieuThuocTinhs)
            {
                item.IsDeleted = true;
                item.AuditTs = DateTime.Now;
            }
            _context.SaveChanges();
            var thuocTinhs = _context.ThuocTinhs.Include(x => x.ThuocTinhCons).Where(x => x.IsDeleted != true && x.MauBieuId == mauBieuId).ToList();
            var chiTieus = _context.ChiTieus.Where(x => x.IsDeleted != true && x.MauBieuId == mauBieuId).ToList();
            if (thuocTinhs.Count() > chiTieus.Count())
            {
                foreach (var thuocTinh in thuocTinhs)
                {
                    foreach (var chiTieu in chiTieus)
                    {
                        if (thuocTinh.ThuocTinhCons.Count() <= 0)
                        {
                            var chitieuthuoctinh = new ChiTieuThuocTinh();
                            chitieuthuoctinh.ChiTieuId = chiTieu.Id;
                            chitieuthuoctinh.ThuocTinhId = thuocTinh.Id;
                            chitieuthuoctinh.MauBieuId = mauBieuId;
                            chitieuthuoctinh.IsDeleted = false;
                            chitieuthuoctinh.CreateAt = DateTime.Now;
                            chitieuthuoctinh.AuditTs = DateTime.Now;
                            _context.ChiTieuThuocTinhs.Add(chitieuthuoctinh);
                        }
                    }
                }
                _context.SaveChanges();
            }
            else
            {

                foreach (var chiTieu in chiTieus)
                {
                    foreach (var thuocTinh in thuocTinhs)
                    {
                        if (thuocTinh.ThuocTinhCons.Count() <= 0)
                        {
                            var chitieuthuoctinh = new ChiTieuThuocTinh();
                            chitieuthuoctinh.ChiTieuId = chiTieu.Id;
                            chitieuthuoctinh.ThuocTinhId = thuocTinh.Id;
                            chitieuthuoctinh.MauBieuId = mauBieuId;
                            chitieuthuoctinh.IsDeleted = false;
                            chitieuthuoctinh.CreateAt = DateTime.Now;
                            chitieuthuoctinh.AuditTs = DateTime.Now;
                            _context.ChiTieuThuocTinhs.Add(chitieuthuoctinh);
                        }
                    }
                }
                _context.SaveChanges();
            }
        }
        public RenderMauBieuVM RenderMauBieu(string mauBieuId)
        {
            var data = new RenderMauBieuVM();
            var maubieu = _context.MauBieus.Where(x => x.Id == mauBieuId && x.IsDeleted != true).FirstOrDefault();
            data.MauBieuId = mauBieuId;
            data.TenMauBieu = maubieu.Ten;
            data.GhiChu = maubieu.GhiChu;
            data.KyHieu = maubieu.KyHieu;
            data.RenderThuocTinh = _thuocTinhService.GetListOrderByLevel(mauBieuId);
            data.RenderChiTieu = RenderChiTieu(mauBieuId);
            return data;
        }

        private List<RenderChiTieuVM> RenderChiTieu(string mauBieuId)
        {
            var result = new List<RenderChiTieuVM>();
            var chitieu = _context.ChiTieus.Where(x => x.MauBieuId == mauBieuId && x.IsDeleted != true).OrderBy(x => x.ThuTu).ToList();
            var listCTTT = _context.ChiTieuThuocTinhs.Where(x => x.IsDeleted != true && x.MauBieuId == mauBieuId).ToList();
            foreach (var ct in chitieu)
            {
                var chiTieuVM = new RenderChiTieuVM();
                chiTieuVM.TenChiTieu = ct.TenChiTieu;
                chiTieuVM.ThuTu = ct.ThuTu??0;
                foreach (var item in listCTTT.Where(x => x.ChiTieuId == ct.Id).ToList())
                {
                    var itemInput = new RenderInputVM();
                    itemInput.Id = item.Id;
                    itemInput.TypeInput = 1;
                    itemInput.TypeValue = 1;
                    itemInput.Value = "";
                    chiTieuVM.Inputs.Add(itemInput);
                }
                result.Add(chiTieuVM);
            }
            return result;
        }

        public void PhanQuyenBaoCao(string mauBieuId, string donViId)
        {
            var mauBieu = _context.MauBieus.Where(x => x.Id == mauBieuId && x.IsDeleted != true).FirstOrDefault();
            if(mauBieu == default(MauBieu))
                throw new Exception("Không tìm thấy mẫu biểu");
            var donVi = _context.DonVis.Where(x => x.Id == donViId && x.IsDeleted != true).FirstOrDefault();
            if (donVi == default(DonVi))
                throw new Exception("Không tìm thấy đơn vị");
            var CheckTonTai = _context.DuLieuMauBieus.Where(x => x.MauBieuId == mauBieuId && x.DonViId == donViId).FirstOrDefault();
            if (CheckTonTai != default(DuLieuMauBieu))
                throw new Exception("Đơn vị đã tồn tại");
            var listCTTT = _context.ChiTieuThuocTinhs.Where(x => x.MauBieuId == mauBieuId && x.IsDeleted != true).ToList();
            var data = new List<DuLieuMauBieu>();
            /// remove data
            var dulieuTonTai = _context.DuLieuMauBieus.Where(x => x.MauBieuId == mauBieuId && x.DonViId == donViId).ToList();
            _context.DuLieuMauBieus.RemoveRange(dulieuTonTai);
            _context.SaveChanges();
            foreach (var item in listCTTT)
            {
                var dlmb = new DuLieuMauBieu();
                dlmb.MauBieuId = mauBieuId;
                dlmb.DonViId = donViId;
                dlmb.ChiTieuThuocTinhId = item.Id;
                data.Add(dlmb);
            }
            _context.DuLieuMauBieus.AddRange(data);
            _context.SaveChanges();
        }
        public void DeleteDonViInMauBieu(string mauBieuId, string donViId)
        {
            var dulieuTonTai = _context.DuLieuMauBieus.Where(x => x.MauBieuId == mauBieuId && x.DonViId == donViId).ToList();
            _context.DuLieuMauBieus.RemoveRange(dulieuTonTai);
            _context.SaveChanges();
        }
        public List<DonVi> GetDonViByMauBieuId(string mauBieuId)
        {
            var donvis = _context.DuLieuMauBieus.Where(x => x.MauBieuId == mauBieuId).Select(x => x.DonViId).ToList();
            return _context.DonVis.Where(x => donvis.Contains(x.Id)).ToList();
        }
    }
}