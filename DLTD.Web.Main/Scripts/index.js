var idVanbanSelected = "";
var isProcessing = false;

var urlTraVanBan = "";
var urlGetVanBanDetail = "";
var urlDeleteVanBan = "";
var urlUpdateCompleteVanBan = "";
var urlPdfViewer = "";
var urlWordViewer = "";
var urlDeleteTinhHinhThucHien = "";
var urlGetDonViPhoiHop = "";
var urlGetTinhHinhThucHien = "";
var urlSaveTinhHinhThucHien = "";
var urlDanhSachNhiemVu = "";
var trangThai = "";

$("#ngaykytu").kendoDatePicker();
$("#ngaykyden").kendoDatePicker();
$("#thoihanxltu").kendoDatePicker();
$("#thoihanxlden").kendoDatePicker();

var cboDouuTien = $("#douutien").kendoDropDownList({
    dataTextField: "text",
    dataValueField: "value",
    dataSource: [
        { text: "", value: "" },
        { text: "Thường", value: "0" },
        { text: "Hỏa tốc", value: "1" }
    ],
    index: 0
}).data("kendoDropDownList");

var comboDonvi = $("#donvixuly").kendoComboBox({
    filter: "contains",
    suggest: true
}).data("kendoComboBox");

var comboNguonChiDao = $("#nguonchidao").kendoComboBox({
    filter: "contains",
    suggest: true
}).data("kendoComboBox");

var comboNguoiChiDao = $("#nguoichidao").kendoComboBox({
    filter: "contains",
    suggest: true
}).data("kendoComboBox");

var comboNguoiTheoDoi = $("#nguoitheodoi").kendoComboBox({
    filter: "contains",
    suggest: true
}).data("kendoComboBox");

var combodonViPhoiHop = $("#donviphoihop").kendoComboBox({
    filter: "contains",
    suggest: true
}).data("kendoComboBox");

var comboTrangThai = $("#trangthai").kendoDropDownList().data("kendoDropDownList");

$("#btnSearchPlusReset").click(function () {
    $("#ngaykytu").val("");
    $("#ngaykyden").val("");
    $("#thoihanxltu").val("");
    $("#thoihanxlden").val("");
    $("#kyhieu").val("");
    $("#noidung").val("");
    $("#ykienchidao").val("");
    cboDouuTien.value("");
    comboDonvi.value("");
    combodonViPhoiHop.value("");
    comboNguoiTheoDoi.value("");
    comboNguoiChiDao.value("");
    comboNguonChiDao.value("");
});

$("#btnSearchPlusFind").click(function () {
    var objSearch = {
        NgayKyTu: $("#ngaykytu").val(),
        NgayKyDen: $("#ngaykyden").val(),
        KyHieu: $("#kyhieu").val(),
        DonViXuLy: comboDonvi.value(),
        NguoiChiDao: comboNguoiChiDao.value(),
        NguoiTheoDoi: comboNguoiTheoDoi.value(),
        ThoiHanXuLyTu: $("#thoihanxltu").val(),
        ThoiHanXuLyDen: $("#thoihanxlden").val(),
        DoUuTien: cboDouuTien.value(),
        DonViPhoiHop: combodonViPhoiHop.value(),
        NguonChiDao: comboNguonChiDao.value(),
        NoiDung: $("#noidung").val(),
        YKienChiDao: $("#ykienchidao").val()
    };

    $("#txtSearch").val("");

    return searchVanBan(objSearch);
});

$("#btnTraLaiSubmit").click(function () {
    if (isProcessing) return false;
    isProcessing = true;

    $.ajax({
        url: urlTraVanBan,
        type: "POST",
        data: { id: idVanbanSelected, lydo: $("#lydotralai").val() },
        success: function (response) {
            $('#modalTraLai').modal('hide');
        },
        complete: function () {
            isProcessing = false;
            searchVanBan();
        }
    });

    return false;
});

function openAttachments(id) {
    if (isProcessing) return false;
    isProcessing = true;

    $.ajax({
        url: urlGetVanBanDetail,
        type: "POST",
        data: { id: id },
        success: function (response) {
            $('#modalAttachments .modal-body').html(response);
            $('#modalAttachments').modal('show');
        },
        complete: function () {
            isProcessing = false;
        }
    });

    return true;
}

$("#btnSearchPlus").click(function () {
    $("#modalSearch").modal("show");
});

$("#btnDelete").click(function () {
    if (isProcessing) return false;

    isProcessing = true;


    bootbox.confirm({
        title: "Xác nhận",
        message: "Bạn có muốn xóa văn bản đang chọn?",
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Hủy',
                className: 'btn-default'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Xóa',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: urlDeleteVanBan,
                    type: "POST",
                    data: { id: idVanbanSelected },
                    success: function (response) {
                        if (response.Result) {
                            isProcessing = false;

                            toastr.success("Xóa văn bản thành công.", "Thành công");

                            searchVanBan();
                        } else {
                            toastr.error("Xóa văn bản không thành công.", "Có lỗi xảy ra");
                        }


                    },
                    complete: function () {
                        isProcessing = false;
                    }
                });
            } else {
                isProcessing = false;
            }
        }
    });

    return false;
});

$("#btnHoanThanh").click(function () {
    if (isProcessing) return false;

    isProcessing = true;

    if (idVanbanSelected === '') {
        alert('Chưa chọn văn bản.');
        return false;
    }

    bootbox.confirm({
        title: "Xác nhận",
        message: "Bạn muốn chuyển trạng thái văn bản?",
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Hủy',
                className: 'btn-default'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Hoàn thành',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    url: urlUpdateCompleteVanBan,
                    type: "POST",
                    data: { id: idVanbanSelected },
                    success: function (response) {
                        if (response.Code === 1) {
                            isProcessing = false;
                            $("#btnSearch").click();
                        }

                    },
                    complete: function () {
                        isProcessing = false;
                    }
                });
            } else {
                isProcessing = false;
            }
        }
    });

    return false;
});

function buildHtmlTHTH(data, idContainer) {

    if (idContainer == undefined || idContainer === "") idContainer = "#danhsachbaocao";

    if (data.Code < 1) {
        $(idContainer).html(data.Message);
        return;
    }
    if (data.Code > 1) {
        $(idContainer).html(data.Results);
        return;
    }

    var html = '';
    for (var i = 0; i < data.Results.length; i++) {
        if (i > 0) html += '<hr/>';

        //var reportDate = moment(data.Results[i].NgayBaoCao).fromNow();
        var reportDate = moment(data.Results[i].NgayBaoCao).format('dddd, DD/MM/YYYY HH:mm');

        html += '<label class="control-label">' + data.Results[i].DonVi + '</label> <i class="date-right">Ngày báo cáo: ' + reportDate + '</i>';
        html += '<div class="clear"></div>';
        html += '<p class="control-label">' + data.Results[i].NoiDungThucHien + '</p>';
        html += '<div class="row">';
        html += '<div class="col-lg-11">';
        if (data.Results[i].FileDinhKem != null && data.Results[i].FileDinhKem !== '' &&
            data.Results[i].FileUrl != null && data.Results[i].FileUrl !== '') {
            html += '<a href="' + data.Results[i].FileUrl + '"><i class="glyphicon glyphicon-paperclip"></i> ' + data.Results[i].FileDinhKem + '</a>';

            if (data.Results[i].FileUrl.toLowerCase().endsWith('.pdf')) {
                html += '<a href="' + urlPdfViewer + data.Results[i].FileUrl + '" target="_blank">(Xem trước)</a>';
            }
            else if (data.Results[i].FileUrl.toLowerCase().endsWith('.doc') || data.Results[i].FileUrl.toLowerCase().endsWith('.docx')) {
                html += '<a href="' + urlWordViewer + encodeURIComponent('~/' + data.Results[i].FileUrl) + '" target="_blank">(Xem trước)</a>';
            }

        }
        html += '</div>';
        html += '<div class="col-lg-1" style="text-align: right">';
        html += '<span onclick="deleteBaoCaoTHTH(' + data.Results[i].Id + ',' + data.Results[i].LoaiBaoCao + ');" style="cursor:pointer" data-toggle="tooltip" title="Xóa" class="glyphicon glyphicon-trash"> </span>';
        html += '</div>';
        html += '</div>';
    }

    $(idContainer).html(html);

    $('[data-toggle="tooltip"]').tooltip();
}

function deleteBaoCaoTHTH(id, loaiBaoCao) {
    if (isProcessing) return false;

    isProcessing = true;

    $.ajax({
        url: urlDeleteTinhHinhThucHien,
        type: "POST",
        data: { id: id, loaiBaoCao: loaiBaoCao, idVanBan: $("#IdVanBan").val() },
        success: function (response) {
            buildHtmlTHTH(response);
        },
        complete: function () {
            isProcessing = false;
        }
    });

    return false;
}

$("#FileDinhKem").change(function () {
    $("label[for=FileDinhKem]").html("<i class=\"glyphicon glyphicon-paperclip\"></i> " + $(this).val());
    $("#RemoveFile").show();
});

$("#RemoveFile").click(function () {
    var file = document.getElementById("FileDinhKem");
    file.value = file.defaultValue;

    $("label[for=FileDinhKem]").html("<i class=\"glyphicon glyphicon-paperclip\"></i> Đính kèm file");

    $(this).hide();
});

$("#btnTinhHinhXuLy").click(function () {
    var comboDonvi = $("#selectDonVi").kendoDropDownList({
        placeholder: "Chọn đơn vị",
        dataTextField: "Name",
        dataValueField: "Id",
        filter: "contains",
        autoBind: true,
        minLength: 3,
        dataSource: {
            type: "json",
            serverFiltering: false,
            transport: {
                read: {
                    url: urlGetDonViPhoiHop + "?idVanBan=" + $("#IdVanBan").val()
                }
            }
        }
    }).data("kendoDropDownList");

    comboDonvi.value("");
    $('#TinhHinhXuLy').val("");
    var file = document.getElementById("FileDinhKem");
    file.value = file.defaultValue;
    $('#danhsachbaocao').empty();

    $.ajax({
        url: urlGetTinhHinhThucHien + "?idVanBanChiDao=" + $("#IdVanBan").val(),
        type: "GET",
        success: function (response) {
            buildHtmlTHTH(response);
        }
    });
});

$("#btnSave").click(function () {
    if (isProcessing) return false;

    isProcessing = true;

    $(this).attr("disabled", "disabled");
    $(this).html("<i class=\"glyphicon glyphicon-floppy-disk\"></i> Đang thực hiện");

    var formData = new FormData($("form#FormInput")[0]);


    $.ajax({
        url: urlSaveTinhHinhThucHien,
        type: "POST",
        data: formData,
        async: false,
        success: function (response) {
            if (response.Code === 1) {
                toastr.success("Gửi báo cáo thành công.", "Thành công");
            } else {
                toastr.error(response.Message, "Có lỗi xảy ra");
            }

            buildHtmlTHTH(response);
        },
        complete: function () {
            isProcessing = false;
            $("#btnSave").removeAttr("disabled");
            $("#btnSave").html("<i class=\"glyphicon glyphicon-floppy-disk\"></i> Gửi phản hồi");
            searchVanBan();
        },
        cache: false,
        contentType: false,
        processData: false
    });

    return false;
});

function gridSelected(arg) {
    var selected = $.map(this.select(), function (item) {
        var grid = $("#grid").data("kendoGrid");
        var row = grid.select();
        var id = grid.dataItem(row).Id;
        var trangThai = grid.dataItem(row).TrangThaiVB;
        var trichYeu = grid.dataItem(row).Trichyeu;


        $('#TrichYeuVanBan').html(trichYeu);
        $('#pvanban').html(trichYeu);
        $('#IdVanBan').val(id);

        if (trangThai.toLowerCase() === "hoàn thành") {
            $('#btnHoanThanh').attr('disabled', 'disabled');
            $('#btnDelete').attr('disabled', 'disabled');
            $('#btnTraLai').attr('disabled', 'disabled');
            $('#btnTinhHinhXuLy').removeAttr('disabled');

            comboTrangThai.enable(false);
            comboTrangThai.value("4");
        } else if (trangThai.toLowerCase() === "trả lại") {
            $('#btnTinhHinhXuLy').attr('disabled', 'disabled');
            $('#btnHoanThanh').attr('disabled', 'disabled');
            $('#btnTraLai').attr('disabled', 'disabled');
            $("#btnDelete").removeAttr('disabled');
        } else {
            $("#btnHoanThanh").removeAttr('disabled');
            $("#btnDelete").removeAttr('disabled');
            $("#btnTraLai").removeAttr('disabled');
            $('#btnTinhHinhXuLy').removeAttr('disabled');

            comboTrangThai.enable();
            comboTrangThai.value("2");
        }

        idVanbanSelected = id;
        return id;
    });

    if (selected.length === 0) {
        $('#btnTinhHinhXuLy').attr('disabled', 'disabled');
        $('#btnHoanThanh').attr('disabled', 'disabled');
        $('#btnDelete').attr('disabled', 'disabled');
        $('#btnTraLai').attr('disabled', 'disabled');
    }
}


$("#btnSearch").click(function () {

    $("#btnSearchPlusReset").click();

    return searchVanBan();
});

function searchVanBan(objSearch) {
    if (isProcessing) return false;

    isProcessing = true;

    if (objSearch) objSearch.TrangThai = trangThai;
    else objSearch = { SearchText: $("#txtSearch").val(), TrangThai: trangThai }

    $.ajax({
        url: urlDanhSachNhiemVu,
        type: 'POST',
        data: objSearch,
        success: function (data) {
            $('#DanhSachNhiemVu').html(data);
        },
        complete: function () {
            isProcessing = false;
            $("#modalSearch").modal("hide");
        }
    });

    return false;
}

function setGridHeight() {
    var height = $(window).innerHeight();
    var used = 70;
    $('div[fixedsize=true]').each(function () {
        used += $(this).outerHeight();
    });
    var girdHeaderAndFooter = $('#DanhSachNhiemVu .k-grid-pager').outerHeight() + $('#DanhSachNhiemVu .k-grid-header').outerHeight();
    $('#DanhSachNhiemVu').css("height", height - used);
    $('#DanhSachNhiemVu .k-grid-content').css("height", height - used - girdHeaderAndFooter);
}

$("#txtSearch").keypress(function (e) {
    if (e.keyCode === 13) {
        $("#btnSearch").click();
    }
});

$(document).ready(function () {
    setGridHeight();
});
$(window).resize(function () {
    setGridHeight();
});

function gridDataBound(arg) {
    var dataView = this.dataSource.view();
    for (var i = 0; i < dataView.length; i++) {
        var uid = dataView[i].uid;
        if (dataView[i].TrangThaiVB.toLocaleLowerCase().indexOf("hoàn thành") >= 0) {
            $("#grid tbody").find("tr[data-uid=" + uid + "]").addClass('hoanthanh');
        } else if (dataView[i].TrangThaiVB.toLocaleLowerCase().indexOf("đang thực hiện") >= 0) {
            $("#grid tbody").find("tr[data-uid=" + uid + "]").addClass('dangthuchien');
        } else if (dataView[i].TrangThaiVB.toLocaleLowerCase().indexOf("trễ hạn") >= 0) {
            $("#grid tbody").find("tr[data-uid=" + uid + "]").addClass('trehan');
        } else if (dataView[i].TrangThaiVB.toLocaleLowerCase().indexOf("mới") >= 0) {
            $("#grid tbody").find("tr[data-uid=" + uid + "]").addClass('moi');
        } else if (dataView[i].TrangThaiVB.toLocaleLowerCase().indexOf("trả lại") >= 0) {
            $("#grid tbody").find("tr[data-uid=" + uid + "]").addClass('tralai');
        } else {
            $("#grid tbody").find("tr[data-uid=" + uid + "]").addClass('khac');
        }
    }
}