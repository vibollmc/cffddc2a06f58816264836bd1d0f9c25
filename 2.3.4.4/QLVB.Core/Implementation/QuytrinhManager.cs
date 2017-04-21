using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Quytrinh;
using QLVB.DTO;
using QLVB.Common.Logging;
using Newtonsoft.Json;
using QLVB.Common.Sessions;
using QLVB.Common.Date;

namespace QLVB.Core.Implementation
{
    public class QuytrinhManager : IQuytrinhManager
    {
        #region Constructor
        private ILogger _logger;
        private ISessionServices _session;

        private IPhanloaiQuytrinhRepository _loaiqtRepo;
        private IQuytrinhRepository _quytrinhRepo;
        private IQuytrinhNodeRepository _qtNodeRepo;
        private IQuytrinhConnectionRepository _qtConnectionRepo;
        private IQuytrinhXulyRepository _xulyRepo;

        private IDonvitructhuocRepository _donviRepo;
        private ICanboRepository _canboRepo;
        private IQuytrinhXulyRepository _qtXulyRepo;
        private IQuytrinhVersionRepository _qtVersionRepo;

        public QuytrinhManager(IPhanloaiQuytrinhRepository loaiqtRepo, ILogger logger,
            IQuytrinhRepository quytrinhRepo, IQuytrinhNodeRepository qtNodeRepo,
            IQuytrinhConnectionRepository qtConnectionRepo, IQuytrinhXulyRepository xulyRepo,
            IDonvitructhuocRepository donviRepo, ICanboRepository canboRepo,
            IQuytrinhXulyRepository qtXulyRepo, IQuytrinhVersionRepository qtVersionRepo,
            ISessionServices session)
        {
            _loaiqtRepo = loaiqtRepo;
            _logger = logger;
            _quytrinhRepo = quytrinhRepo;
            _qtNodeRepo = qtNodeRepo;
            _qtConnectionRepo = qtConnectionRepo;
            _xulyRepo = xulyRepo;
            _donviRepo = donviRepo;
            _canboRepo = canboRepo;
            _qtXulyRepo = qtXulyRepo;
            _qtVersionRepo = qtVersionRepo;
            _session = session;
        }
        #endregion Constructor

        #region Loaiquytrinh

        public ListLoaiQuytrinhViewModel GetListLoaiQuytrinh()
        {
            ListLoaiQuytrinhViewModel quytrinh = new ListLoaiQuytrinhViewModel
            {
                LoaiQuytrinh = _loaiqtRepo.PhanloaiQuytrinhs
                    .Select(p => new EditLoaiQuytrinhViewModel
                    {
                        intid = p.intid,
                        strtenloaiquytrinh = p.strtenloai
                    }),
                Quytrinh = _quytrinhRepo.AllQuytrinhs
                    .Select(p => new EditQuytrinhViewModel
                    {
                        intid = p.intid,
                        idloai = p.intidloai,
                        strtenquytrinh = p.strten
                    })
            };

            return quytrinh;
        }

        public EditLoaiQuytrinhViewModel GetEditLoaiQuytrinh(int id)
        {
            EditLoaiQuytrinhViewModel loaiqt = new EditLoaiQuytrinhViewModel();
            if (id > 0)
            {   // cap nhat
                loaiqt = _loaiqtRepo.PhanloaiQuytrinhs
                    .Where(p => p.intid == id)
                    .Select(p => new EditLoaiQuytrinhViewModel
                    {
                        intid = p.intid,
                        strtenloaiquytrinh = p.strtenloai
                    })
                    .FirstOrDefault();
            }

            return loaiqt;
        }

        public ResultFunction SaveLoaiQuytrinh(int id, string strten)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                if (!string.IsNullOrEmpty(strten))
                {
                    if (id == 0)
                    {
                        // them moi
                        _loaiqtRepo.Them(strten);
                        _logger.Info("Thêm mới loại quy trình " + strten);
                    }
                    else
                    {
                        // cap nhat
                        _loaiqtRepo.Sua(id, strten);
                        _logger.Info("Cập nhật loại quy trình " + strten);
                    }

                    kq.id = (int)ResultViewModels.Success;
                }
                else
                {
                    kq.id = (int)ResultViewModels.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }
        #endregion Loaiquytrinh

        #region Quytrinh

        #region Update
        /// <summary>
        /// form them moi/cap nhat ten quy trinh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditQuytrinhViewModel GetEditQuytrinh(int id)
        {
            EditQuytrinhViewModel model = new EditQuytrinhViewModel();

            if (id > 0)
            {
                // cap nhat
                model = _quytrinhRepo.AllQuytrinhs
                    .Where(p => p.intid == id)
                    .Select(p => new EditQuytrinhViewModel
                    {
                        intid = p.intid,
                        idloai = p.intidloai,
                        strtenquytrinh = p.strten,
                        intSongay = p.intSoNgay,
                        dteThoigianApdung = p.strNgayApdung,
                        IsActive = (p.inttrangthai == (int)enumQuytrinh.inttrangthai.IsActive) ? true : false
                    }).FirstOrDefault();
            }
            else
            {
                // them moi
                model.dteThoigianApdung = DateTime.Now;
            }

            var LoaiQuytrinh = _loaiqtRepo.PhanloaiQuytrinhs
                .Select(p => new EditLoaiQuytrinhViewModel
                {
                    intid = p.intid,
                    strtenloaiquytrinh = p.strtenloai
                });
            model.LoaiQuytrinh = LoaiQuytrinh;



            return model;
        }

        public ResultFunction SaveQuytrinh(EditQuytrinhViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                if (!string.IsNullOrEmpty(model.strtenquytrinh))
                {
                    int inttrangthai = (model.IsActive == true) ? (int)enumQuytrinh.inttrangthai.IsActive : (int)enumQuytrinh.inttrangthai.NotActive;
                    if (model.intid == 0)
                    {
                        // them moi
                        _quytrinhRepo.Them(model.strtenquytrinh, model.idloai, model.intSongay, model.dteThoigianApdung, inttrangthai);
                        _logger.Info("Thêm mới quy trình " + model.strtenquytrinh);
                    }
                    else
                    {
                        // cap nhat
                        _quytrinhRepo.Sua(model.intid, model.strtenquytrinh, model.intSongay, model.dteThoigianApdung, inttrangthai);
                        _logger.Info("Cập nhật quy trình " + model.strtenquytrinh);
                    }

                    kq.id = (int)ResultViewModels.Success;
                }
                else
                {
                    kq.id = (int)ResultViewModels.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        #endregion Update

        #region Flowchart

        /// <summary>
        /// luu js flowchart
        /// </summary>
        /// <param name="strjson"></param>
        /// <returns></returns>
        public ResultFunction SaveFlowChart(int idquytrinh, string strjson)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                if (!string.IsNullOrEmpty(strjson))
                {
                    bool isNew = _CheckNewFlowchart(idquytrinh);
                    if (isNew)
                    {
                        // them moi
                        kq = _SaveNewFlowChart(idquytrinh, strjson);
                    }
                    else
                    {
                        // cap nhat 
                        kq = _UpdateFlowChart(idquytrinh, strjson);
                    }
                }
                else
                {
                    kq.id = (int)ResultViewModels.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }
        /// <summary>
        /// kiem tra xem la quy trinh moi(chua co node) hay cap nhat (them/xoa node)
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns>true: quy trinh moi</returns>
        private bool _CheckNewFlowchart(int idquytrinh)
        {
            var nodes = _qtNodeRepo.QuytrinhNodes
                .Where(p => p.intidquytrinh == idquytrinh);
            if (nodes == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// them moi flowchart
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="strjson"></param>
        /// <returns></returns>
        private ResultFunction _SaveNewFlowChart(int idquytrinh, string strjson)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                if (!string.IsNullOrEmpty(strjson))
                {
                    FlowchartViewModel flowchart = ReadJson(strjson);

                    if (flowchart != null)
                    {
                        var strtenquytrinh = _quytrinhRepo.SaveNumberOfElements(idquytrinh, flowchart.numberOfElements);

                        Dictionary<string, int> nodeConnect = new Dictionary<string, int>();

                        foreach (var p in flowchart.nodes)
                        {
                            QuytrinhNode node = new QuytrinhNode
                            {
                                intidquytrinh = idquytrinh,
                                NodeId = p.Id,
                                intleft = p.left,
                                inttop = p.top,
                                strten = p.text
                            };
                            _qtNodeRepo.Them(node);
                            // luu intid node de them vao connection
                            nodeConnect.Add(node.NodeId, node.intid);
                        }
                        foreach (var c in flowchart.connections)
                        {
                            int intidNode;
                            QuytrinhConnection conn = new QuytrinhConnection();
                            conn.strlabel = c.label;

                            nodeConnect.TryGetValue(c.from, out intidNode);
                            conn.intidFrom = intidNode;

                            nodeConnect.TryGetValue(c.to, out intidNode);
                            conn.intidTo = intidNode;

                            _qtConnectionRepo.Them(conn);
                        }
                        _logger.Info("Cập nhật quy trình " + strtenquytrinh);
                        kq.id = (int)ResultViewModels.Success;
                    }
                    else
                    {
                        kq.id = (int)ResultViewModels.Error;
                    }
                }
                else
                {
                    kq.id = (int)ResultViewModels.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        /// <summary>
        /// cap nhat flowchart: cap nhat cac node da xoa/ them moi
        /// xoa toan bo connection, add lai connection
        /// 
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="strjson"></param>
        /// <returns></returns>
        private ResultFunction _UpdateFlowChart(int idquytrinh, string strjson)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                if (!string.IsNullOrEmpty(strjson))
                {
                    FlowchartViewModel flowchart = ReadJson(strjson);

                    if (flowchart != null)
                    {
                        var strtenquytrinh = _quytrinhRepo.SaveNumberOfElements(idquytrinh, flowchart.numberOfElements);

                        // cac node dang co trong flowchart
                        var currentNodes = _qtNodeRepo.QuytrinhNodes
                            .Where(p => p.intidquytrinh == idquytrinh).ToList();

                        // ds cac nodeid de xoa connection
                        List<int> deleteIdNodeConnection = new List<int>();

                        Dictionary<string, int> nodeConnect = new Dictionary<string, int>();

                        // cac node tu client submit
                        List<QuytrinhNode> submitNode = new List<QuytrinhNode>();

                        // cac node bi xoa( co trong currentNode nhung khong co trong submit node
                        List<int> DeleteNodes = new List<int>();

                        // cac node moi them vao
                        List<QuytrinhNode> NewNodes = new List<QuytrinhNode>();

                        // cac node tu client submit
                        foreach (var p in flowchart.nodes)
                        {
                            QuytrinhNode node = new QuytrinhNode
                            {
                                intidquytrinh = idquytrinh,
                                NodeId = p.Id,
                                intleft = p.left,
                                inttop = p.top,
                                strten = p.text
                            };
                            submitNode.Add(node);
                        }

                        // kiem tra cac node moi
                        foreach (var submit in submitNode)
                        {
                            try
                            {
                                var find = currentNodes.FirstOrDefault(p => p.NodeId == submit.NodeId);
                                if (find == null)
                                {
                                    //khong tim thay trong current, la node moi
                                    NewNodes.Add(submit);
                                    _qtNodeRepo.Them(submit);
                                }
                                else
                                {
                                    // tim thay node trong current, cap nhat vi tri left, top
                                    find.strten = submit.strten;
                                    find.intleft = submit.intleft;
                                    find.inttop = submit.inttop;
                                    _qtNodeRepo.Capnhat(find);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex.Message);
                            }
                        }

                        // kiem tra cac node bi xoa
                        foreach (var current in currentNodes)
                        {
                            try
                            {
                                deleteIdNodeConnection.Add(current.intid);

                                var find = submitNode.Find(p => p.NodeId == current.NodeId);
                                if (find == null)
                                {
                                    //khong tim thay trong submit, delete node
                                    DeleteNodes.Add(current.intid);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex.Message);
                            }
                        }
                        _qtNodeRepo.Xoa(DeleteNodes);




                        // xoa toan bo connection hien co
                        _qtConnectionRepo.Xoa(deleteIdNodeConnection);

                        // lay intid cua toan bo cac node de them vao connection
                        var newnodes = _qtNodeRepo.QuytrinhNodes.Where(p => p.intidquytrinh == idquytrinh);
                        foreach (var p in newnodes)
                        {
                            nodeConnect.Add(p.NodeId, p.intid);
                        }

                        // them moi lai cac connection
                        foreach (var c in flowchart.connections)
                        {
                            int intidNode;
                            QuytrinhConnection conn = new QuytrinhConnection();
                            conn.strlabel = c.label;

                            nodeConnect.TryGetValue(c.from, out intidNode);
                            conn.intidFrom = intidNode;

                            nodeConnect.TryGetValue(c.to, out intidNode);
                            conn.intidTo = intidNode;

                            _qtConnectionRepo.Them(conn);
                        }
                        _logger.Info("Cập nhật quy trình " + strtenquytrinh);
                        kq.id = (int)ResultViewModels.Success;
                    }
                    else
                    {
                        kq.id = (int)ResultViewModels.Error;
                    }
                }
                else
                {
                    kq.id = (int)ResultViewModels.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }


        #region Convert Json
        /// <summary>
        /// convert json to flowchart
        /// </summary>
        /// <param name="jsFlowchart"></param>
        /// <returns></returns>
        private FlowchartViewModel ReadJson(string jsFlowchart)
        {
            try
            {
                FlowchartViewModel flowchart = JsonConvert.DeserializeObject<FlowchartViewModel>(jsFlowchart);
                return flowchart;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// convert flowchart to json
        /// </summary>
        /// <param name="flowchart"></param>
        /// <returns></returns>
        private string WriteJson(FlowchartViewModel flowchart)
        {
            try
            {
                string output = JsonConvert.SerializeObject(flowchart);
                return output;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        #endregion Convert Json

        /// <summary>
        /// doc quy trinh
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns>json</returns>
        public string ReadFlowChart(int idquytrinh)
        {
            if (idquytrinh > 0)
            {
                try
                {
                    int? numberOfElements = _quytrinhRepo.GetNumberOfElements(idquytrinh);

                    Dictionary<int, string> nodeConnect = new Dictionary<int, string>();

                    List<int> listidnode = new List<int>();

                    var nodes = _qtNodeRepo.QuytrinhNodes
                            .Where(p => p.intidquytrinh == idquytrinh);

                    List<NodeViewModel> nodeView = new List<NodeViewModel>();

                    foreach (var p in nodes)
                    {
                        nodeConnect.Add(p.intid, p.NodeId);
                        listidnode.Add(p.intid);
                        var node = new NodeViewModel
                        {
                            Id = p.NodeId,
                            text = p.strten,
                            left = p.intleft,
                            top = p.inttop
                        };
                        nodeView.Add(node);
                    }

                    var connects = _qtConnectionRepo.QuytrinhConnections
                            .Where(p => listidnode.Contains(p.intidFrom));

                    List<ConnectionViewModel> connectionView = new List<ConnectionViewModel>();

                    foreach (var conn in connects)
                    {
                        ConnectionViewModel connect = new ConnectionViewModel();
                        connect.label = conn.strlabel;
                        connect.from = nodeConnect[conn.intidFrom];
                        connect.to = nodeConnect[conn.intidTo];

                        connectionView.Add(connect);
                    }

                    // them vao flowchart
                    FlowchartViewModel flowchart = new FlowchartViewModel();
                    flowchart.nodes = nodeView;
                    flowchart.connections = connectionView;
                    flowchart.numberOfElements = (int)numberOfElements;

                    string jsFlowchart = WriteJson(flowchart);
                    return jsFlowchart;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// lay ten quy trinh
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        public string GetFlowChartName(int idquytrinh)
        {
            try
            {
                return _quytrinhRepo.AllQuytrinhs.FirstOrDefault(p => p.intid == idquytrinh).strten;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// xoa toan bo quy trinh
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        public ResultFunction DeleteFlowChart(int idquytrinh)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                var nodes = _qtNodeRepo.QuytrinhNodes
                    .Where(p => p.intidquytrinh == idquytrinh);
                List<int> listidnode = new List<int>();
                foreach (var p in nodes)
                {
                    listidnode.Add(p.intid);
                }
                // xoa connection
                var deleteConnections = _qtConnectionRepo.Xoa(listidnode);
                // xoa node
                var deleteNodes = _qtNodeRepo.Xoa(listidnode);
                // xoa number of elements
                _quytrinhRepo.SaveNumberOfElements(idquytrinh, null);

                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }

            return kq;
        }

        #endregion Flowchart

        #region Thongtin xuly

        /// <summary>
        /// lay cac thong tin xu ly cua Node
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="NodeId"></param>
        /// <returns></returns>
        public EditThongtinXulyViewModel GetThongtinXuly(int idquytrinh, string NodeId)
        {
            EditThongtinXulyViewModel model = new EditThongtinXulyViewModel();

            model.strtenquytrinh = _quytrinhRepo.AllQuytrinhs.FirstOrDefault(p => p.intid == idquytrinh).strten;

            model.listDonvi = _donviRepo.Donvitructhuocs
                    .Select(p => new QLVB.DTO.Donvi.EditDonviViewModel
                    {
                        intid = p.Id,
                        strtendonvi = p.strtendonvi
                    })
                    .OrderBy(p => p.strtendonvi);

            var Node = _qtNodeRepo.QuytrinhNodes
                .Where(p => p.intidquytrinh == idquytrinh)
                .Where(p => p.NodeId == NodeId)
                .FirstOrDefault();

            if (Node != null)
            {
                // node da luu 
                model.intidNode = Node.intid;
                model.idquytrinh = Node.intidquytrinh;
                model.strNodeId = Node.NodeId;
                model.strTenNode = Node.strten;



                var xuly = _xulyRepo.QuytrinhXulys
                    .Where(p => p.intidNode == Node.intid)
                    .FirstOrDefault();
                if (xuly != null)
                {
                    // da co thong tin
                    model.intDonvi = xuly.intidDonvi;
                    model.intCanbo = xuly.intidCanbo;
                    model.intSoNgay = xuly.intSoNgay;
                    model.IsNext = (xuly.intNext == (int)enumQuytrinhXuly.intNext.Co) ? true : false;
                    model.IsHoanthanh = (xuly.intHoanthanh == (int)enumQuytrinhXuly.intHoanthanh.Co) ? true : false;

                    //model.intVaitro = xuly.intVaitro;
                    //model.LoaiVaitro = xuly.intVaitro;
                    switch (xuly.intVaitro)
                    {
                        case (int)enumQuytrinhXuly.intVaitro.Khongthamgia:
                            model.LoaiVaitro = enumEditThongtinXulyViewModel.Khongthamgia;
                            break;
                        case (int)enumQuytrinhXuly.intVaitro.Lanhdaogiaoviec:
                            model.LoaiVaitro = enumEditThongtinXulyViewModel.Lanhdaogiaoviec;
                            break;
                        case (int)enumQuytrinhXuly.intVaitro.Lanhdaophutrach:
                            model.LoaiVaitro = enumEditThongtinXulyViewModel.Lanhdaophutrach;
                            break;
                        case (int)enumQuytrinhXuly.intVaitro.Xulychinh:
                            model.LoaiVaitro = enumEditThongtinXulyViewModel.Xulychinh;
                            break;
                        case (int)enumQuytrinhXuly.intVaitro.Phoihopxuly:
                            model.LoaiVaitro = enumEditThongtinXulyViewModel.Phoihopxuly;
                            break;
                        default:
                            model.LoaiVaitro = enumEditThongtinXulyViewModel.Khongthamgia;
                            break;
                    }
                    model.IsXulyDongthoi = (xuly.intXulyDongthoi == (int)enumQuytrinhXuly.intXulyDongthoi.Co) ? true : false;

                }
            }
            else
            {
                // node moi them vao, chua save vao DB
                model.intidNode = 0;
                model.idquytrinh = 0;
                //model.intVaitro = (int)enumEditThongtinXulyViewModel.Xulychinh;
                model.LoaiVaitro = enumEditThongtinXulyViewModel.Xulychinh;

            }

            return model;
        }

        /// <summary>
        /// lay thong tin cac user thuoc don vi : iddonvi
        /// </summary>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        public IEnumerable<QLVB.DTO.Donvi.EditUserViewModel> GetListCanbo(int iddonvi)
        {
            try
            {
                var canbo = _canboRepo.GetActiveCanbo
                .Where(p => p.intdonvi == iddonvi)
                    //.OrderBy(p => p.strkyhieu)
                    //.ThenBy(p => p.strhoten)
                .Select(p => new QLVB.DTO.Donvi.EditUserViewModel
                {
                    intid = p.intid,
                    strhoten = p.strhoten
                })
                ;
                return canbo;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new List<QLVB.DTO.Donvi.EditUserViewModel>();
            }
        }

        /// <summary>
        /// luu cac thong tin xu ly tai node
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultFunction SaveThongtinXuly(EditThongtinXulyViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                if (model.intidNode == 0)
                {
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = "Lỗi! Bước xử lý phải được lưu trước khi thêm thông tin";
                }
                else
                {
                    // xoa cac xu ly truoc khi cap nhat moi
                    _xulyRepo.Xoa(model.intidNode);

                    QuytrinhXuly xuly = new QuytrinhXuly
                    {
                        intidNode = model.intidNode,
                        intidDonvi = model.intDonvi,
                        intidCanbo = model.intCanbo,
                        intSoNgay = model.intSoNgay,
                        intNext = (model.IsNext == true) ? (int)enumQuytrinhXuly.intNext.Co : (int)enumQuytrinhXuly.intNext.Khong,
                        intHoanthanh = (model.IsHoanthanh == true) ? (int)enumQuytrinhXuly.intHoanthanh.Co : (int)enumQuytrinhXuly.intHoanthanh.Khong,
                        intXulyDongthoi = (model.IsXulyDongthoi == true) ? (int)enumQuytrinhXuly.intXulyDongthoi.Co : (int)enumQuytrinhXuly.intXulyDongthoi.Khong,

                        intVaitro = (int)model.LoaiVaitro
                    };
                    // khi intvaitro = khongthamgia thi tu dong chon mac dinh hoan thanh
                    if (xuly.intVaitro == (int)enumQuytrinhXuly.intVaitro.Khongthamgia)
                    {
                        xuly.intHoanthanh = (int)enumQuytrinhXuly.intHoanthanh.Co;
                    }

                    _xulyRepo.Them(xuly);
                    _logger.Info("Cập nhật thông tin xử lý của quy trình " + model.strtenquytrinh + " . Công việc: " + model.strTenNode);
                    kq.id = (int)ResultViewModels.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = ex.Message;
            }

            return kq;
        }

        #endregion Thongtin xuly

        #endregion Quytrinh

        #region SaveVersion
        /// <summary>
        /// lay toan bo quy trinh xu ly
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        public QuytrinhXulyViewModels GetQuytrinhXuly(int idquytrinh)
        {
            QuytrinhXulyViewModels model = new QuytrinhXulyViewModels();
            var quytrinh = _quytrinhRepo.AllQuytrinhs.FirstOrDefault(p => p.intid == idquytrinh);
            if (quytrinh != null)
            {
                try
                {
                    model.idquytrinh = idquytrinh;
                    model.strtenquytrinh = quytrinh.strten;
                    model.intTongSongayxuly = quytrinh.intSoNgay;
                    model.dteNgayApdung = (DateTime)quytrinh.strNgayApdung;

                    var congviecs = _qtNodeRepo.QuytrinhNodes.Where(p => p.intidquytrinh == idquytrinh)
                    .GroupJoin(
                        _qtXulyRepo.QuytrinhXulys,
                        cv => cv.intid,
                        xl => xl.intidNode,
                        (cv, xl) => new { congviec = cv, xuly = xl.FirstOrDefault() }
                    )
                    .Select(p => new CongviecXulyViewModel
                    {
                        idcongviec = p.congviec.intid,
                        strtencongviec = p.congviec.strten,
                        nodeid = p.congviec.NodeId,
                        intLeft = p.congviec.intleft,
                        intTop = p.congviec.inttop,
                        intidCanbo = p.xuly.intidCanbo,
                        intVaitro = p.xuly.intVaitro,
                        intidDonvi = p.xuly.intidDonvi,
                        intNext = p.xuly.intNext,
                        intSoNgay = p.xuly.intSoNgay,
                        Hoanthanh = p.xuly.intHoanthanh,
                        intXulyDongthoi = p.xuly.intXulyDongthoi

                    }).ToList();

                    model.congviecs = congviecs;

                    var xulys = _qtConnectionRepo.QuytrinhConnections
                    .Join(
                        _qtNodeRepo.QuytrinhNodes.Where(p => p.intidquytrinh == idquytrinh),
                        con => con.intidFrom,
                        node => node.intid,
                        (con, node) => new { con, node }
                    )
                    .OrderBy(p => p.node.NodeId)
                    .Select(p => new ThutuXulyViewModel
                    {
                        idFrom = p.con.intidFrom,
                        idTo = p.con.intidTo,
                        label = p.con.strlabel
                    })
                    .ToList();

                    model.BuocXuly = xulys;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }

            }

            return model;

        }


        /// <summary>
        /// luu toan bo cac buoc xu ly cong viec theo quy trinh vao table QuytrinhVersion
        /// </summary>        
        /// <param name="model"></param>
        private ResultFunction _SaveQuytrinhXulyVerion(QuytrinhXulyViewModels quytrinh)
        {
            ResultFunction kq = new ResultFunction();
            // kiểm tra tổng thời gian xử lý của các bước
            var countThoigian = _CheckThoigianDongthoi(quytrinh); //_CheckThoigianXuly(quytrinh);
            if (countThoigian.id == (int)ResultViewModels.Error)
            {
                kq.id = (int)ResultViewModels.Error;
                kq.message = countThoigian.message;
                return kq;
            }
            //==============================================
            bool checkVersion = _CheckVersion(quytrinh.idquytrinh, quytrinh.dteNgayApdung);
            if (!checkVersion)
            {
                kq = _AddQuytrinhXulyVersion(quytrinh);
                kq.message = "Thêm mới phiên bản quy trình ngày " + DateServices.FormatDateVN(quytrinh.dteNgayApdung);
            }
            else
            {
                // da co ngay phien ban roi
                kq = _DeleteQuytrinhXulyVersion(quytrinh.idquytrinh, quytrinh.dteNgayApdung);
                if (kq.id == (int)ResultViewModels.Success)
                {
                    kq = _AddQuytrinhXulyVersion(quytrinh);
                    kq.message = "Cập nhật phiên bản quy trình ngày " + DateServices.FormatDateVN(quytrinh.dteNgayApdung);
                }
                else
                {
                    kq.message = "Lỗi cập nhật quy trình";
                }
            }

            return kq;
        }
        /// <summary>
        /// them moi version
        /// </summary>
        /// <param name="quytrinh"></param>
        /// <returns></returns>
        private ResultFunction _AddQuytrinhXulyVersion(QuytrinhXulyViewModels quytrinh)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                List<string> listNode = new List<string>();
                int idcanbo = _session.GetUserId();
                DateTime dteNgay = DateTime.Now;
                foreach (var p in quytrinh.BuocXuly)
                {
                    QuytrinhVersion _qtVersion = new QuytrinhVersion();
                    _qtVersion.intidquytrinh = quytrinh.idquytrinh;
                    _qtVersion.strNgayApdung = quytrinh.dteNgayApdung;

                    _qtVersion.intidFrom = p.idFrom;
                    _qtVersion.intidTo = p.idTo;
                    _qtVersion.strlabel = p.label;

                    var congviec = quytrinh.congviecs
                        .FirstOrDefault(x => x.idcongviec == p.idFrom);
                    if (congviec != null)
                    {
                        _qtVersion.intidCanbo = congviec.intidCanbo;
                        _qtVersion.intVaitro = congviec.intVaitro;
                        _qtVersion.intSongay = congviec.intSoNgay;
                        _qtVersion.intNext = congviec.intNext;
                        _qtVersion.intHoanthanh = congviec.Hoanthanh;
                        _qtVersion.intXulyDongthoi = congviec.intXulyDongthoi;

                        _qtVersion.nodeidFrom = congviec.nodeid;
                        _qtVersion.strTenNodeFrom = congviec.strtencongviec;
                        _qtVersion.intLeft = congviec.intLeft;
                        _qtVersion.intTop = congviec.intTop;

                    }
                    _qtVersion.intidCanboCapnhat = idcanbo;
                    _qtVersion.strNgayCapnhat = dteNgay;

                    // them cac node vao ds de kiem tra con thieu hay khong
                    listNode.Add(_qtVersion.nodeidFrom);

                    var id = _qtVersionRepo.Them(_qtVersion);
                }

                var nodeend = quytrinh.congviecs.Where(p => !listNode.Contains(p.nodeid));
                foreach (var n in nodeend)
                {
                    QuytrinhVersion _hoso = new QuytrinhVersion();
                    _hoso.intidquytrinh = quytrinh.idquytrinh;
                    _hoso.strNgayApdung = quytrinh.dteNgayApdung;

                    _hoso.intidFrom = n.idcongviec;
                    _hoso.intidTo = null;
                    _hoso.strlabel = null;


                    _hoso.intidCanbo = n.intidCanbo;
                    _hoso.intVaitro = n.intVaitro;
                    _hoso.intSongay = n.intSoNgay;
                    _hoso.intNext = n.intNext;
                    _hoso.intHoanthanh = n.Hoanthanh;

                    _hoso.nodeidFrom = n.nodeid;
                    _hoso.strTenNodeFrom = n.strtencongviec;
                    _hoso.intLeft = n.intLeft;
                    _hoso.intTop = n.intTop;

                    _hoso.intidCanboCapnhat = idcanbo;
                    _hoso.strNgayCapnhat = dteNgay;

                    _qtVersionRepo.Them(_hoso);
                }

                //var node_end = quytrinh.congviecs.FirstOrDefault(p => p.idcongviec == idnode_end);

                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                kq.id = (int)ResultViewModels.Error;
                _logger.Error(ex.Message);
            }
            return kq;
        }

        /// <summary>
        /// xoa version cu de them version moi
        /// </summary>
        /// <param name="quytrinh"></param>
        /// <returns></returns>
        private ResultFunction _DeleteQuytrinhXulyVersion(int idquytrinh, DateTime ngayapdung)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                bool result = _qtVersionRepo.Xoa(idquytrinh, ngayapdung);
                kq.id = (result) ? (int)ResultViewModels.Success : (int)ResultViewModels.Error;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        /// <summary>
        /// kiem tra xem ngay phien ban da co chua. return true:daco
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="ngay"></param>
        /// <returns>true: da co</returns>
        private bool _CheckVersion(int idquytrinh, DateTime ngay)
        {
            var qt = _qtVersionRepo.QuytrinhVersions
                .Where(p => p.intidquytrinh == idquytrinh)
                .Where(p => p.strNgayApdung == ngay)
                .Count();
            return (qt > 0) ? true : false;
        }

        /// <summary>
        /// kiem tra tong thoi gian xu ly cua cac buoc.
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        private ResultFunction _CheckThoigianXuly(QuytrinhXulyViewModels quytrinh)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                int TongSongayxuly = quytrinh.intTongSongayxuly;
                int countThoigian = 0;
                // ds cac node bat dau de tinh thoi gian
                List<int> listNodeIdFrom = new List<int>();
                List<int> listNodeIdTo = new List<int>();

                foreach (var p in quytrinh.BuocXuly)
                {
                    //_qtVersion.intidFrom = p.idFrom;
                    //_qtVersion.intidTo = p.idTo;

                    var congviec = quytrinh.congviecs
                        .FirstOrDefault(x => x.idcongviec == p.idFrom);
                    if (congviec != null)
                    {
                        //_qtVersion.intidCanbo = congviec.intidCanbo;
                        //_qtVersion.intVaitro = congviec.intVaitro;
                        //_qtVersion.intSongay = congviec.intSoNgay;
                        //_qtVersion.intNext = congviec.intNext;
                        //_qtVersion.intHoanthanh = congviec.Hoanthanh;

                        //_qtVersion.nodeidFrom = congviec.nodeid;
                        //_qtVersion.strTenNodeFrom = congviec.strtencongviec;
                        //_qtVersion.intLeft = congviec.intLeft;
                        //_qtVersion.intTop = congviec.intTop;


                        if (!listNodeIdFrom.Contains(p.idTo))
                        {   // khong chua cac node da qua
                            // de loai bo dieu kien re nhanh

                            if (!listNodeIdFrom.Contains(p.idFrom))
                            {   // chi tinh thoi gian tai 1 node duy nhat
                                listNodeIdFrom.Add(p.idFrom);

                                if (!listNodeIdTo.Contains(p.idFrom))
                                {
                                    listNodeIdTo.Add(p.idFrom);
                                }

                                if (congviec.intSoNgay > 0)
                                {
                                    countThoigian += (int)congviec.intSoNgay;
                                }
                            }
                        }

                    }
                }

                kq.id = (int)ResultViewModels.Success;
                //if (TongSongayxuly >= countThoigian)
                //{
                //    kq.id = (int)ResultViewModels.Success;
                //}
                //else
                //{
                //    kq.id = (int)ResultViewModels.Error;
                //    kq.message = "Lỗi! Tổng số ngày xử lý trong quy trình là "
                //                + countThoigian + " ngày lớn hơn số ngày khai báo";
                //}

                var listNodeSongsong = _CheckThoigianDongthoi(quytrinh);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
            }
            return kq;
        }

        /// <summary>
        /// dung thuat toan BFS va Dijkstra de tim duong di lon nhat tren do thi
        /// </summary>
        /// <param name="quytrinh"></param>
        /// <returns></returns>
        private ResultFunction _CheckThoigianDongthoi(QuytrinhXulyViewModels quytrinh)
        {
            ResultFunction result = new ResultFunction();

            // dung thuat toan BFS va Dijkstra de tim duong di lon nhat tren do thi
            List<int> listAllNode = new List<int>();

            List<int> listNodeDaduyet = new List<int>();

            Dictionary<int, int> dicAllNode = new Dictionary<int, int>();

            var nodes = quytrinh.BuocXuly
                .GroupBy(p => p.idFrom);

            foreach (var p in nodes)
            {
                listAllNode.Add(p.FirstOrDefault().idFrom);
                //AllNodeValue.Add(p.FirstOrDefault().idFrom, 0);
            }
            int countNode = listAllNode.Count();

            int TongTime = 0;
            int intidNodeStart = quytrinh.congviecs.Where(p => p.nodeid == "node_begin").FirstOrDefault().idcongviec; ;
            int intidNodeEnd = quytrinh.congviecs.Where(p => p.nodeid == "node_end").FirstOrDefault().idcongviec;

            List<int> listNodeOld = new List<int>();
            List<int> listNodeNew = new List<int>();
            Dictionary<int, int> dicNodeNew = new Dictionary<int, int>();
            listNodeOld.Add(intidNodeStart);
            if (!listAllNode.Contains(intidNodeEnd)) { listAllNode.Add(intidNodeEnd); }

            // danh dau nhung node yeu cau xu ly dong thoi
            // (nhung node ket thuc xu ly song song)
            List<int> listNodeDongthoi = new List<int>();
            var xulydongthoi = quytrinh.congviecs.Where(p => p.intXulyDongthoi == (int)enumQuytrinhXuly.intXulyDongthoi.Co);
            if (xulydongthoi != null)
            {
                foreach (var x in xulydongthoi)
                {
                    listNodeDongthoi.Add(x.idcongviec);
                }
            }


            while (listNodeDaduyet.Count < listAllNode.Count)
            {
                bool isStop = false;
                if (isStop) { break; }
                if (listNodeDaduyet.Count > listAllNode.Count) { break; }
                //==============================================================
                listNodeNew = new List<int>();
                dicNodeNew = new Dictionary<int, int>();

                foreach (var node in listNodeOld)
                {
                    listNodeDaduyet.Add(node);

                    var duongdi = quytrinh.BuocXuly.Where(d => d.idFrom == node);
                    int maxTime = 0;
                    foreach (var d in duongdi)
                    {
                        if (!listNodeDaduyet.Contains(d.idTo))
                        {   // tap moi gom nhung node chua duyet va gan ke 
                            if (!listNodeNew.Contains(d.idTo))
                            {
                                listNodeNew.Add(d.idTo);

                                var congviec = quytrinh.congviecs
                                    .FirstOrDefault(x => x.idcongviec == d.idTo);

                                int temptime = (congviec.intSoNgay > 0) ? (int)congviec.intSoNgay : 0;
                                dicNodeNew.Add(d.idTo, temptime);
                                maxTime = (maxTime < temptime) ? temptime : maxTime;
                            }
                        }
                    }
                    int countNew = listNodeNew.Count;
                    if (countNew >= 2)
                    {
                        // listnodenew >=2 la bat dau di song song
                        // goi ham duyet tu node bat dau song song den node ket thuc song song
                        int maxTimeNhanh = 0;
                        foreach (var m in dicNodeNew)
                        {
                            // cong thoi gian tai node bat dau song song
                            int countTimeNhanh = 0;
                            countTimeNhanh += m.Value;
                            if (listNodeDongthoi.Count >= 1)
                            {   // co node xu ly dong thoi thi tinh thoi gian tren nhanh song song
                                // va danh dau cac node da duyet
                                Dictionary<int, int> kq = _DuyetNhanhSongsong
                                                (quytrinh, listNodeDaduyet,
                                                listAllNode, m.Key, listNodeDongthoi);
                                foreach (var k in kq)
                                {
                                    listNodeDaduyet.Add(k.Key);
                                    dicAllNode.Add(k.Key, k.Value);
                                    countTimeNhanh += k.Value;
                                }
                                maxTimeNhanh = (maxTimeNhanh < countTimeNhanh) ? countTimeNhanh : maxTimeNhanh;
                            }
                            else
                            {   // khong co node xu ly dong thoi thi tinh thoi gian tung nhanh cho toi node_end
                                Dictionary<int, int> kq = _DuyetNhanhEnd
                                                (quytrinh, listNodeDaduyet,
                                                listAllNode, m.Key, intidNodeEnd);
                                foreach (var k in kq)
                                {
                                    //listNodeDaduyet.Add(k.Key);
                                    //dicAllNode.Add(k.Key, k.Value);
                                    countTimeNhanh += k.Value;
                                }
                                maxTimeNhanh = (maxTimeNhanh < countTimeNhanh) ? countTimeNhanh : maxTimeNhanh;
                                isStop = true;
                            }

                        }

                        // da duyet qua cac nhanh song song
                        // bat dau duyet tiep tu node ket thuc song song                        
                        listNodeNew = listNodeDongthoi;
                        TongTime += maxTimeNhanh;
                    }
                    else
                    {
                        dicAllNode.Add(node, maxTime);
                        TongTime += maxTime;
                    }
                }
                listNodeOld = listNodeNew;
            }

            int TongSongayxuly = quytrinh.intTongSongayxuly;
            if (TongSongayxuly >= TongTime)
            {
                result.id = (int)ResultViewModels.Success;
            }
            else
            {
                result.id = (int)ResultViewModels.Error;
                result.message = "Lỗi! Tổng số ngày xử lý trong quy trình là "
                            + TongTime + " ngày lớn hơn số ngày khai báo";
            }
            return result;
        }

        /// <summary>
        /// duyet tren 1 nhanh song song de tinh tong thoi gian xu ly tren nhanh nay.
        /// chu y: trong nhanh nay khong duoc co them nhanh song song khac.
        /// Tra ve: dictionary ds cac node da duyet va gia tri thoi gian cua node
        /// </summary>
        /// <param name="quytrinh"></param>
        /// <param name="nodebd"></param>
        /// <param name="nodekt"></param>
        /// <returns></returns>
        private Dictionary<int, int> _DuyetNhanhSongsong(
                QuytrinhXulyViewModels quytrinh,
                List<int> listNodeDaduyet, List<int> listAllNode,
                int nodebd,
                List<int> listNodeDongthoi)
        {
            Dictionary<int, int> kq = new Dictionary<int, int>();
            List<int> _listNodeDaDuyet = listNodeDaduyet;
            List<int> _listAllNode = listAllNode;
            int _nodebd = nodebd;
            while (_listNodeDaDuyet.Count < listAllNode.Count) //&& (_nodebd != nodekt) )
            {
                bool isfound = false;
                foreach (var kt in listNodeDongthoi)
                {   // neu gap node ket thuc xu ly dong thoi thi ngung lai
                    if (_nodebd == kt) { isfound = true; }
                }
                if (isfound) { break; }

                if (!_listNodeDaDuyet.Contains(_nodebd))
                {
                    _listNodeDaDuyet.Add(_nodebd);

                    var duongdi = quytrinh.BuocXuly.Where(d => d.idFrom == _nodebd);
                    int maxTime = 0;
                    int NodeNext = 0;
                    foreach (var d in duongdi)
                    {
                        if (!_listNodeDaDuyet.Contains(d.idTo))
                        {
                            var congviec = quytrinh.congviecs
                                .FirstOrDefault(x => x.idcongviec == d.idTo);

                            int temptime = (congviec.intSoNgay > 0) ? (int)congviec.intSoNgay : 0;
                            maxTime = (maxTime < temptime) ? temptime : maxTime;
                            NodeNext = d.idTo;
                        }
                    }
                    kq.Add(_nodebd, maxTime);
                    _nodebd = NodeNext;
                }
                else
                {
                    // de tranh lap vo han
                    break;
                }
            }

            return kq;
        }

        /// <summary>
        /// neu khong co node yeu cau xu ly dong thoi thi duyet den node_end 
        /// </summary>
        /// <param name="quytrinh"></param>
        /// <param name="listNodeDaduyet"></param>
        /// <param name="listAllNode"></param>
        /// <param name="nodebd"></param>
        /// <param name="listNodeDongthoi"></param>
        /// <returns></returns>
        private Dictionary<int, int> _DuyetNhanhEnd(
                QuytrinhXulyViewModels quytrinh,
                List<int> listNodeDaduyet, List<int> listAllNode,
                int nodebd,
                int nodekt)
        {
            Dictionary<int, int> kq = new Dictionary<int, int>();
            List<int> _listNodeDaDuyet = listNodeDaduyet;
            List<int> _listAllNode = listAllNode;
            int _nodebd = nodebd;
            while ((_listNodeDaDuyet.Count < listAllNode.Count) && (_nodebd != nodekt))
            {
                if (!_listNodeDaDuyet.Contains(_nodebd))
                {
                    _listNodeDaDuyet.Add(_nodebd);

                    var duongdi = quytrinh.BuocXuly.Where(d => d.idFrom == _nodebd);
                    int maxTime = 0;
                    int NodeNext = 0;
                    foreach (var d in duongdi)
                    {
                        if (!_listNodeDaDuyet.Contains(d.idTo))
                        {
                            var congviec = quytrinh.congviecs
                                .FirstOrDefault(x => x.idcongviec == d.idTo);

                            int temptime = (congviec.intSoNgay > 0) ? (int)congviec.intSoNgay : 0;
                            maxTime = (maxTime < temptime) ? temptime : maxTime;
                            NodeNext = d.idTo;
                        }
                    }
                    kq.Add(_nodebd, maxTime);
                    _nodebd = NodeNext;
                }
                else
                {
                    // de tranh lap vo han
                    break;
                }
            }
            return kq;
        }

        public ResultFunction SaveVersion(int idquytrinh)
        {
            ResultFunction kq = new ResultFunction();

            QuytrinhXulyViewModels quytrinh = GetQuytrinhXuly(idquytrinh);

            kq = _SaveQuytrinhXulyVerion(quytrinh);

            return kq;
        }
        #endregion SaveVersion

        #region ViewVersion

        public CategoryNgayViewModel GetCategoryNgay(int idquytrinh)
        {
            try
            {
                var ngay = _qtVersionRepo.QuytrinhVersions
                .Where(p => p.intidquytrinh == idquytrinh)
                .Select(p => p.strNgayApdung)
                .Distinct()
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ThenByDescending(p => p.Day)
                .ToList();

                CategoryNgayViewModel listngay = new CategoryNgayViewModel();
                List<string> alldate = new List<string>();
                foreach (var p in ngay)
                {
                    string date = DateServices.FormatDateVN(p);
                    alldate.Add(date);
                }
                listngay.ListNgay = alldate;
                return listngay;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        ///  doc quy trinh trong phien ban
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="strngay"></param>
        /// <returns>json</returns>
        public string ReadFlowChartVersion(int idquytrinh, string strngay)
        {
            if ((idquytrinh > 0) && (!string.IsNullOrEmpty(strngay)))
            {
                return _ReadQuytrinhVersion(idquytrinh, strngay);
            }
            else
            {
                return null;
            }
        }

        private string _ReadQuytrinhVersion(int idquytrinh, string strngay)
        {
            QuytrinhXulyViewModels model = new QuytrinhXulyViewModels();
            try
            {
                DateTime? date = DateServices.FormatDateEn(strngay);
                var quytrinh = _qtVersionRepo.QuytrinhVersions
                    .Where(p => p.intidquytrinh == idquytrinh)
                    .Where(p => p.strNgayApdung == date)
                    .ToList();

                List<NodeViewModel> nodeView = new List<NodeViewModel>();
                List<ConnectionViewModel> connectionView = new List<ConnectionViewModel>();
                List<NodeXulyViewModel> Xulys = new List<NodeXulyViewModel>();
                string strngaycapnhat = "";

                foreach (var q in quytrinh)
                {
                    var node = new NodeViewModel
                    {
                        Id = q.nodeidFrom,
                        text = q.strTenNodeFrom,
                        left = (int)q.intLeft,
                        top = (int)q.intTop
                    };
                    nodeView.Add(node);
                    //===============================================
                    ConnectionViewModel connect = new ConnectionViewModel();
                    connect.label = q.strlabel;
                    connect.from = q.nodeidFrom;

                    string nodeTo = null;
                    if (q.intidTo != null)
                    {   // bo node_end
                        try
                        {
                            nodeTo = quytrinh.FirstOrDefault(p => p.intidFrom == q.intidTo).nodeidFrom;
                        }
                        catch
                        {
                            _logger.Error("Lỗi view quytrinh. intidTo :" + q.intidTo);
                        }
                        connect.to = nodeTo;
                        connectionView.Add(connect);
                    }
                    //==============================================
                    if (q.intidCanbo > 0)
                    {   // khong add node_begin va node_end (khong co idcanbo)
                        NodeXulyViewModel xuly = new NodeXulyViewModel();
                        xuly.Id = q.nodeidFrom;
                        var canbo = _canboRepo.GetAllCanboByID((int)q.intidCanbo);
                        xuly.strhotencanbo = canbo.strhoten;
                        xuly.intSongay = q.intSongay;
                        string strVaitro = "";
                        switch ((int)q.intVaitro)
                        {
                            case (int)enumEditThongtinXulyViewModel.Khongthamgia:
                                strVaitro = "Không tham gia xử lý";
                                break;
                            case (int)enumEditThongtinXulyViewModel.Lanhdaogiaoviec:
                                strVaitro = "Lãnh đạo giao việc";
                                break;
                            case (int)enumEditThongtinXulyViewModel.Lanhdaophutrach:
                                strVaitro = "Lãnh đạo phụ trách";
                                break;
                            case (int)enumEditThongtinXulyViewModel.Phoihopxuly:
                                strVaitro = "Phối hợp xử lý";
                                break;
                            case (int)enumEditThongtinXulyViewModel.Xulychinh:
                                strVaitro = "Xử lý chính";
                                break;
                        }
                        xuly.strVaitro = strVaitro;
                        xuly.intXulyDongthoi = q.intXulyDongthoi;

                        Xulys.Add(xuly);
                    }

                    strngaycapnhat = DateServices.FormatDateVN(q.strNgayCapnhat);
                }

                // them vao flowchart
                FlowchartViewModel flowchart = new FlowchartViewModel();
                flowchart.nodes = nodeView;
                flowchart.connections = connectionView;
                flowchart.numberOfElements = 0;
                flowchart.xulys = Xulys;

                // đổi sang thông tin ngày cập nhật cua phiên bản, không phải ngày phiên bản
                flowchart.strNgayApdung = "Ngày cập nhật phiên bản: " + strngaycapnhat;

                string jsFlowchart = WriteJson(flowchart);
                return jsFlowchart;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// hien thi thong tin xu ly tai node 
        /// HIEN KHONG SU DUNG
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="NodeId"></param>
        /// <returns></returns>
        public ViewThongtinXulyViewModel LoadThongtinXuly(int idquytrinh, string strngay, string NodeId)
        {
            ViewThongtinXulyViewModel model = new ViewThongtinXulyViewModel();
            try
            {
                DateTime date = (DateTime)DateServices.FormatDateEn(strngay);
                var quytrinh = _qtVersionRepo.QuytrinhVersions
                    .Where(p => p.intidquytrinh == idquytrinh)
                    .Where(p => p.strNgayApdung == date)
                    .Where(p => p.nodeidFrom == NodeId)
                    .FirstOrDefault();

                var canbo = _canboRepo.GetAllCanboByID((int)quytrinh.intidCanbo);
                model.Tencanbo = canbo.strhoten;
                model.TenPhong = _donviRepo.Donvitructhuocs.FirstOrDefault(p => p.Id == canbo.intdonvi).strtendonvi;
                model.Songay = quytrinh.intSongay;
                string strVaitro = "";
                switch ((int)quytrinh.intVaitro)
                {
                    case (int)enumEditThongtinXulyViewModel.Khongthamgia:
                        strVaitro = "Không tham gia xử lý";
                        break;
                    case (int)enumEditThongtinXulyViewModel.Lanhdaogiaoviec:
                        strVaitro = "Lãnh đạo giao việc";
                        break;
                    case (int)enumEditThongtinXulyViewModel.Lanhdaophutrach:
                        strVaitro = "Lãnh đạo phụ trách";
                        break;
                    case (int)enumEditThongtinXulyViewModel.Phoihopxuly:
                        strVaitro = "Phối hợp xử lý";
                        break;
                    case (int)enumEditThongtinXulyViewModel.Xulychinh:
                        strVaitro = "Xử lý chính";
                        break;
                }
                model.Vaitro = strVaitro;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return model;
        }

        #endregion ViewVersion

    }
}
