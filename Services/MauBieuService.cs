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
            entity.ChoPhepNhap = model.ChoPhepNhap;
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
            entity.IsDeleted = true;
            _context.Update(entity);
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
            entity.ChoPhepNhap = model.ChoPhepNhap;
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
            data.RenderChiTieu = RenderChiTieuNhapLieu(mauBieuId);
            return data;
        }
        public RenderMauBieuVM RenderMauBieuCPN(string mauBieuId)
        {
            var data = new RenderMauBieuVM();
            var maubieu = _context.MauBieus.Where(x => x.Id == mauBieuId && x.IsDeleted != true).FirstOrDefault();
            data.MauBieuId = mauBieuId;
            data.TenMauBieu = maubieu.Ten;
            data.GhiChu = maubieu.GhiChu;
            data.KyHieu = maubieu.KyHieu;
            data.RenderThuocTinh = _thuocTinhService.GetListOrderByLevel(mauBieuId);
            data.RenderChiTieu = RenderChiTieuChoNhap(mauBieuId);
            return data;
        }
        private List<RenderChiTieuVM> RenderChiTieuChoNhap(string mauBieuId)
        {
            var result = new List<RenderChiTieuVM>();
            var chitieu = _context.ThuocTinhNhapLieus.Where(x => x.MauBieuId == mauBieuId).OrderBy(x => x.OrderBy).ToList();
            var listCTTT = _context.ChiTieuThuocTinhs.Where(x => x.IsDeleted != true && x.MauBieuId == mauBieuId).ToList();

            var soThuocTinh = _context.ThuocTinhs.Include(x => x.ThuocTinhCons).Where(x => x.MauBieuId == mauBieuId && x.IsDeleted != true && x.ThuocTinhCons.Count <= 0).ToList().Count;

            //var row = chitieu.Count / soThuocTinh;
            RenderChiTieuVM chiTieuVM = new RenderChiTieuVM();
            var index = 0;
            //for (int i = 0; i < row; i++)
            //{

            //    //if (index == 0)
            //    //{
            //    //    chiTieuVM = new RenderChiTieuVM();
            //    //    chiTieuVM.TenChiTieu = index.ToString();
            //    //    chiTieuVM.ThuTu = index;
            //    //}
            //    //var itemInput = new RenderInputVM();
            //    //itemInput.Id = ct.Id;
            //    //itemInput.TypeInput = 1;
            //    //itemInput.TypeValue = 1;
            //    //itemInput.Value = ct.Value;
            //    //chiTieuVM.Inputs.Add(itemInput);
            //    //if (index == soThuocTinh - 1)
            //    //{
            //    //    result.Add(chiTieuVM);
            //    //    index = 0;
            //    //}

            //    //index++;
            //}

            foreach (var ct in chitieu)
            {


                if(index == 0)
                {
                    chiTieuVM = new RenderChiTieuVM();
                    chiTieuVM.TenChiTieu = index.ToString();
                    chiTieuVM.ThuTu = index;
                }
                var itemInput = new RenderInputVM();
                itemInput.Id = ct.Id;
                itemInput.TypeInput = 1;
                itemInput.TypeValue = 1;
                itemInput.Value = ct.Value;
                chiTieuVM.Inputs.Add(itemInput);
                if(index == soThuocTinh - 1)
                {
                    result.Add(chiTieuVM);
                    index = 0;
                }
                else
                {

                    index++;
                }

            }
            return result;
        }

        public RenderMauBieuVM RenderMauBieuNhapLieu(AppUser user, string mauBieuId)
        {
            var data = new RenderMauBieuVM();
            var maubieu = _context.MauBieus.Where(x => x.Id == mauBieuId && x.IsDeleted != true).FirstOrDefault();
            data.MauBieuId = mauBieuId;
            data.TenMauBieu = maubieu.Ten;
            data.GhiChu = maubieu.GhiChu;
            data.KyHieu = maubieu.KyHieu;
            data.DonViNhan = _context.DonVis.Where(x => x.Id == user.DonViId).FirstOrDefault().Ten;
            data.RenderThuocTinh = _thuocTinhService.GetListOrderByLevel(mauBieuId);
            data.RenderChiTieu = RenderChiTieuNhapLieu(mauBieuId);
            return data;
        }
        private List<RenderChiTieuVM> RenderChiTieuNhapLieu(string mauBieuId)
        {
            var result = new List<RenderChiTieuVM>();
            var chitieu = _context.ChiTieus.Where(x => x.MauBieuId == mauBieuId && x.IsDeleted != true).OrderBy(x => x.ThuTu).ToList();
            var listCTTT = _context.ChiTieuThuocTinhs.Where(x => x.IsDeleted != true && x.MauBieuId == mauBieuId).ToList();
            var duLieus = _context.DuLieuMauBieus.Where(x => x.MauBieuId == mauBieuId).ToList();
            foreach (var ct in chitieu)
            {
                var chiTieuVM = new RenderChiTieuVM();
                chiTieuVM.TenChiTieu = ct.TenChiTieu;
                chiTieuVM.ThuTu = ct.ThuTu ?? 0;
                foreach (var item in listCTTT.Where(x => x.ChiTieuId == ct.Id).ToList())
                {
                    var dataTemp = duLieus.Where(x => x.ChiTieuThuocTinhId == item.Id).FirstOrDefault();
                    if(dataTemp != null)
                    {
                        var itemInput = new RenderInputVM();
                        itemInput.Id = dataTemp.Id;
                        itemInput.TypeInput = 1;
                        itemInput.TypeValue = 1;
                        itemInput.Value = dataTemp.Value;
                        chiTieuVM.Inputs.Add(itemInput);
                    }
                }
                result.Add(chiTieuVM);
            }
            return result;
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
                chiTieuVM.ThuTu = ct.ThuTu ?? 0;
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
            if (mauBieu == default(MauBieu))
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
            //_context.DuLieuMauBieus.RemoveRange(dulieuTonTai);
            //_context.SaveChanges();
            foreach (var item in listCTTT)
            {
                var checkTonTai = dulieuTonTai.Where(x => x.ChiTieuThuocTinhId == item.Id).FirstOrDefault();
                if(CheckTonTai == default(DuLieuMauBieu))
                {
                    var dlmb = new DuLieuMauBieu();
                    dlmb.MauBieuId = mauBieuId;
                    dlmb.DonViId = donViId;
                    dlmb.ChiTieuThuocTinhId = item.Id;
                    dlmb.TrangThai = (int)TrangThaiBaoCao.ChoNhapLieu;
                    _context.DuLieuMauBieus.Add(dlmb);
                }
                else
                {
                    checkTonTai.TrangThai = (int)TrangThaiBaoCao.ChoNhapLieu;
                }
                _context.SaveChanges();
            }

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

        public List<MauBieu> ListMauBaoCaoByDonViId(string donViId)
        {
            var dulieusCTTT = _context.DuLieuMauBieus.Where(x => x.DonViId == donViId).Select(x => x.MauBieuId).ToList();
            var mauBieumau = _context.MauBieus.Where(x => dulieusCTTT.Contains(x.Id) && x.IsDeleted != true).ToList();
            return mauBieumau;
        }

        public void SaveData(List<InputFormVM> inputs)
        {
            var data = _context.DuLieuMauBieus.ToList();
            foreach (var item in inputs)
            {
                var id = Convert.ToInt32(item.Id);
                var entity = data.Where(x => x.Id == id).FirstOrDefault();
                entity.Value = item.Value;

            }
            _context.SaveChanges();
        }

        public void TrangThaiNhapLieu(string mauBieuId, string donViId)
        {
            var data = _context.DuLieuMauBieus.Where(x => x.MauBieuId == mauBieuId && x.DonViId == donViId).ToList();
            foreach (var item in data)
            {
                item.TrangThai = (int)TrangThaiBaoCao.ChoDuyet;
            }
            _context.SaveChanges();
        }
        public void TrangThaiDuyet(string mauBieuId, string donViId)
        {
            var data = _context.DuLieuMauBieus.Where(x => x.MauBieuId == mauBieuId && x.DonViId == donViId).ToList();
            foreach (var item in data)
            {
                item.TrangThai = (int)TrangThaiBaoCao.ChoXuatBan;
            }
            _context.SaveChanges();
        }
        public void TrangThaiXuatBan(string mauBieuId, string donViId)
        {
            var data = _context.DuLieuMauBieus.Where(x => x.MauBieuId == mauBieuId && x.DonViId == donViId).ToList();
            foreach (var item in data)
            {
                item.TrangThai = (int)TrangThaiBaoCao.XuatBan;
            }
            _context.SaveChanges();
        }
        public List<DuLieuMauBieu> GetDuLieuMau(string donViId)
        {
            return _context.DuLieuMauBieus.Where(x => x.DonViId == donViId).ToList();
        }
    }
}