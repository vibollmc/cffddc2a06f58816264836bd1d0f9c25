var dltd = {
    elementContainerButton: "nav.box-toolbar",
    elementDocument: "#listvanbanden div#grid div.k-grid-content table.k-selectable tr.k-state-selected",
    token: "",
    urlApi: "{{URI}}/Api/VanBan",
    urlLogin: "{{URI}}/Account/Login",
    urlChiTietVanBanDi: "{{QLVBURI}}/Vanbandi/_XemChitietVanban?id=",
    urlChiTietVanBanDen: "{{QLVBURI}}/Vanbanden/_XemChitietVanban?id="
};

var idButton = "";
var isProcessing = false;
var indexAttachment = 0;
var dataDonVi = [
    {{DATADONVI}}
];
var dataDonViFiltered = [];

function makeDataDonViFiltered(filterText) {
    if (filterText || $.trim(filterText) === "" || $.trim(filterText) === ",") Object.assign(dataDonVi, dataDonViFiltered);

    var arr = filterText.split(", ");

    dataDonViFiltered = [];

    arr.forEach(function(item) {

        dataDonVi.forEach(function(data) {

            if (data.text === item) {
                dataDonViFiltered.push(data);
            }
        });
    });
}

function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 25; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}

function buildButton() {
    idButton = makeid();
    var buttonHtml = "<button class='btn btn-sm btn-warning btn-flat button-box' type='button' style='display:normal' id='" + idButton + "'>";
    buttonHtml += "<i class='glyphicon glyphicon-send'></i>";
    buttonHtml += "&nbsp;CSDL Theo dõi";
    buttonHtml += "</button>";

    var modalHtml = "<div id='" + idButton + "Modal'>";
    modalHtml += "<div class='box' style='position: fixed; width: 580px; z-index: 1000; left: 0px; top: 26px;'>";
    modalHtml += "<header>";
    modalHtml += "<nav style='padding: 3px;'>";
    modalHtml += "<button type='button' style='width:100px;' class='btn btn-sm btn-primary' id='" + idButton + "btnSend'><i class='glyphicon glyphicon-send'></i> Lưu</button>";
    modalHtml += "<button type='button' style='width:100px;margin-left:4px' class='btn btn-sm btn-primary' id='" + idButton + "btnContinue'><i class='glyphicon glyphicon-send'></i> Thêm tiếp</button>";
    //modalHtml += "<span> <input id='" + idButton + "AnotherOne' type='checkbox'> Phân công cho đơn vị khác</span>";
    modalHtml += "</nav>";
    modalHtml += "</header>";
    modalHtml += "</div>";

    modalHtml += "<div class='modal-body' style='margin-top: 25px;'>";

    modalHtml += "<div id='" + idButton + "Notification' class='alert alert-success'>";
    modalHtml += "<strong>Success!</strong> thông báo ở đây";
    modalHtml += "</div>";

    modalHtml += "<form id='" + idButton + "Form' method='post' enctype='multipart/form-data'>";
    modalHtml += "<table class='responsive-table'>";
    modalHtml += "<tbody>";

    modalHtml += "<tr>";
    modalHtml += "<td colspan='2'>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Văn bản:</label>";
    modalHtml += "<p class='control-label' id='" + idButton + "VanBan'></p>";
    modalHtml += "</div>";
    modalHtml += "</td>";
    modalHtml += "</tr>";

    modalHtml += "<tr>";
    modalHtml += "<td colspan='2'>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Đơn vị xử lý chính:</label><span>";//<input id='" + idButton + "AllDonVi' type='checkbox'> Hiện tất cả</span>";
    modalHtml += "<select name='IdDonVi' id='" + idButton + "DonVi' style='width: 100%' placeholder='Chọn đơn vị xử lý chính...'>";
    //modalHtml += "{{DONVI}}";
    modalHtml += "</select>";
    modalHtml += "</div>";
    modalHtml += "</td>";
    modalHtml += "</tr>";

    modalHtml += "<tr>";
    modalHtml += "<td colspan='2'>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Nội dung cần thực hiện:</label>";
    modalHtml += "<textarea type='text' name='YKienChiDao' rows='2' class='form-control' id='" + idButton + "YKCD' style='padding:2px 1px;'></textarea>";
    modalHtml += "</div>";
    modalHtml += "</td>";
    modalHtml += "</tr>";

    modalHtml += "<tr>";
    modalHtml += "<td>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Thời hạn xử lý:</label><br/>";
    modalHtml += "<input name='ThoiHanXuLy' class='form-control' id='" + idButton + "HanXL'>";
    modalHtml += "</div>";
    modalHtml += "</td>";

    modalHtml += "<td>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Độ khẩn:</label><br/>";
    modalHtml += "<input name='DoKhan' class='form-control' id='" + idButton + "DoKhan'>";
    modalHtml += "</div>";
    modalHtml += "</td>";
    modalHtml += "</tr>";

    modalHtml += "<tr>";
    modalHtml += "<td colspan='2'>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Nguồn chỉ đạo:</label><br>";
    modalHtml += "<select name='idKhoi' id='" + idButton + "NguonChiDao' style='width: 200px' placeholder='Chọn nguồn chỉ đạo...'>";
    modalHtml += "{{KHOI}}";
    modalHtml += "</select>";
    modalHtml += "</div>";
    modalHtml += "</td>";
    modalHtml += "</tr>";

    modalHtml += "<tr>";
    modalHtml += "<td colspan='2'>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Đơn vị Phối hợp:</label>";
    modalHtml += "<select name='IdDonViPhoiHop' id='" + idButton + "DonViPhoiHop' multiple='multiple'  data-placeholder='Chọn đơn vị phối hợp xử lý...'>";
    modalHtml += "{{DONVI}}";
    modalHtml += "</select>";
    modalHtml += "</div>";
    modalHtml += "</td>";
    modalHtml += "</tr>";

    modalHtml += "<tr>";
    modalHtml += "<td>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Người chỉ đạo:</label><br/>";
    modalHtml += "<select name='NguoiChiDao' class='form-control' id='" + idButton + "NguoiChiDao'>";
    modalHtml += "{{NGUOICHIDAO}}";
    modalHtml += "</select>";
    modalHtml += "</div>";
    modalHtml += "</td>";

    modalHtml += "<td>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Người theo dõi:</label><br/>";
    modalHtml += "<select name='NguoiTheoDoi' class='form-control' id='" + idButton + "NguoiTheoDoi'>";
    modalHtml += "{{NGUOITHEODOI}}";
    modalHtml += "</select>";
    modalHtml += "</div>";
    modalHtml += "</td>";
    modalHtml += "</tr>";

    modalHtml += "<tr>";
    modalHtml += "<td colspan='2'>";
    modalHtml += "<div class='rơw form-group' id='" + idButton + "groupAttachment'>";
    modalHtml += "<button type='button' class='btn btn-sm btn-primary' id='" + idButton + "AddAttachment'><i class='glyphicon glyphicon-paperclip'></i> Thêm file đính kèm</button><br><br>";
    //modalHtml += "<label for='" + idButton + "File' class='btn btn-default'>";
    //modalHtml += "<i class='glyphicon glyphicon-paperclip'></i> Đính kèm file";
    //modalHtml += "</label>";
    //modalHtml += " <span title='Xóa file đã chọn' style='display:none; cursor:pointer' class='glyphicon glyphicon-remove' id='" + idButton + "RemoveFile'></span>";
    //modalHtml += "<input name='FileDinhKem' id='" + idButton + "File' type='file' style='display:none'/><div style='height:10px;width:100%'></div>";
    modalHtml += "</div>";
    modalHtml += "</td>";
    modalHtml += "</tr>";

    modalHtml += "</tbody>";
    modalHtml += "</table>";

    modalHtml += "<input type='hidden' name='IdVanBan' id='" + idButton + "IdVanBan'>";
    modalHtml += "<input type='hidden' name='Ngayky' id='" + idButton + "Ngayky'>";    
    modalHtml += "<input type='hidden' name='SoKH' id='" + idButton + "SoKH'>";
    modalHtml += "<input type='hidden' name='UserId' id='" + idButton + "UserId'>";
    modalHtml += "<input type='hidden' name='Trichyeu' id='" + idButton + "Trichyeu'>";
    modalHtml += "<input type='hidden' name='FileVBDinhKem' id='" + idButton + "FileVBDinhKem'>";

    //end input
    modalHtml += "</form>";
    modalHtml += "</div>";//end modal-body

    //modalHtml += "<div class='modal-footer'>";
    //modalHtml += "<button type='button' class='btn btn-default' data-dismiss='modal'>Đóng</button>";
    //modalHtml += "</div>";
    modalHtml += "</div>";
    $(dltd.elementContainerButton).append(buttonHtml);
    $("body").append(modalHtml);
}

function addAttachment(number) {
    var attachmentHtml = "<label for='" + idButton + "File" + number + "' class='btn btn-default'>";
    attachmentHtml += "<i class='glyphicon glyphicon-paperclip'></i> Đính kèm file";
    attachmentHtml += "</label>";
    attachmentHtml += " <span title='Xóa file đã chọn' style='display:none; cursor:pointer' class='glyphicon glyphicon-remove' id='" + idButton + "RemoveFile" + number + "'></span>";
    attachmentHtml += "<input name='FileDinhKem' id='" + idButton + "File" + number + "' type='file' style='display:none'/>";

    attachmentHtml += "  <button type='button' class='btn btn-xs btn-danger' id='" + idButton + "RemoveAttachment" + number + "' ><span class='glyphicons glyphicons-remove-circle'></span> Xóa</button><div style='height:10px;width:100%'></div>";

    $("#" + idButton + "groupAttachment").append(attachmentHtml);


    $("#" + idButton + "File" + number).change(function () {
        $("label[for=" + idButton + "File" + number +"]").html("<i class='glyphicon glyphicon-paperclip'></i> " + $(this).val());
        $("#" + idButton + "RemoveFile" + number).show();
    });

    $("#" + idButton + "RemoveFile" + number).click(function () {
        var file = document.getElementById(idButton + "File" + number);
        file.value = file.defaultValue;

        $("label[for=" + idButton + "File" + number + "]").html("<i class='glyphicon glyphicon-paperclip'></i> Đính kèm file");

        $(this).hide();
    });

    $("#" + idButton + "RemoveAttachment" + number).click(function() {
        $(this).prev().remove();
        $(this).prev().remove();
        $(this).prev().remove();
        $(this).next().remove();
        $(this).remove();
    });

    $("#" + idButton + "File" + number).click();
}

function checkValidData() {
    var message = "<ul>";
    var valid = true;
    var comboDonvi = $("#" + idButton + "DonVi").data("kendoComboBox");
    if (comboDonvi.value() == "") {
        valid = false;
        message += "<li>Yêu cầu chọn đơn vị xử lý chính.</li>";
    }

    var ykcd = $("#" + idButton + "YKCD");
    if ($.trim(ykcd.val()) == "") {
        valid = false;
        message += "<li>Yêu cầu nhập ý kiến chỉ đạo.</li>";
    }

    var comboNguoiTheoDoi = $("#" + idButton + "NguoiTheoDoi").data("kendoComboBox");
    if ($.trim(comboNguoiTheoDoi.value()) == "") {
        valid = false;
        message += "<li>Yêu cầu chọn người theo dõi.</li>";
    }

    if (!valid) {
        message += "</ul>";
        $("#" + idButton + "Notification")
            .attr("class", "alert alert-danger")
            .html("<strong></strong> " + message)
            .show();
    }
    return valid;
}

function getUserId() {
    if (isProcessing) return false;
    isProcessing = true;

    $.ajax({
        url: dltd.urlApi,
        type: "GET",
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function(data) {
            if (data.length === 0) {
                $("#" + idButton + "UserId").val("");
                //ToDo: need implement in case didn't login   

                $("#" + idButton + "Notification")
                    .attr("class", "alert alert-warning")
                    .html("<strong>Yêu cầu!</strong> Bạn chưa đăng nhập vào hệ thống dữ liệu theo dõi. <a href='" + dltd.urlLogin + "' target='_blank' onclick='$('#' + idButton + 'Modal').modal('hide');'>Đăng nhập ngay</a>")
                    .show();

                $("#" + idButton + "Modal .responsive-table").hide();
                $("#" + idButton + "btnSend").parent().parent().parent().hide();
                //$("#" + idButton + "btnSend").hide();
                //$("#" + idButton + "AnotherOne").parent().hide();
            }
            else {
                $("#" + idButton + "UserId").val(data);
                $("#" + idButton + "Modal .responsive-table").show();
                $("#" + idButton + "btnSend").parent().parent().parent().show();
                if ($("#" + idButton + "NguoiTheoDoi").find("option[value='" + data + "']").length > 0) {
                    var comboNguoiTheoDoi = $("#" + idButton + "NguoiTheoDoi").data("kendoComboBox");
                    comboNguoiTheoDoi.value(data);
                }
            }
        },
        error: function() {
            
        },
        complete: function () {
            isProcessing = false;
        }
    });

    return true;
}

function getAttachments(id) {

    var linksFile = "";
    $.ajax({
        url: dltd.urlChiTietVanBanDen + id,
        type: "GET",
        success: function (response) {
            if (response.length > 0 && response.indexOf("THÔNG TIN VĂN BẢN") >= 0) {
                //get nguoi soan

                var tdNguoiSoan = $(response).find('tr:nth-child(9) td:nth-child(4)');
                if (tdNguoiSoan) {
                    $("#" + idButton + "NguoiTheoDoi").find("option").each(function() {
                        if ($(this).html() == tdNguoiSoan.html()) {
                            var comboNguoiTheoDoi = $("#" + idButton + "NguoiTheoDoi").data("kendoComboBox");
                            comboNguoiTheoDoi.value($(this).attr("value"));
                            $("#" + idButton + "NguoiTheoDoi").val($(this).attr("value"));
                        }
                    });
                }

                //get all attachments
                var tdFile = $(response).find("#strfileattach");
                tdFile.find("a").each(function () {
                    if ($(this).attr("href").indexOf("javascript") === -1) {
                        linksFile += "$" + location.href.replace(location.pathname, "") + $(this).attr("href");
                        linksFile += "*" + $.trim($(this).text());
                    }
                });

                $("#" + idButton + "FileVBDinhKem").val(linksFile);
            }
        },
        complete: function () {

        }
    });
}

$(document).ready(function() {
    buildButton();

    $("#" + idButton + "Modal").kendoWindow({
        width: "600px",
        height: "500px",
        title: "NHẬP CƠ SỠ DỮ LIỆU THEO DÕI",
        visible: false,
        actions: [
            "Close"
        ]
    }).data("kendoWindow").center();

    $("#" + idButton + "HanXL").kendoDatePicker();

    var comboDoKhan = $("#"+idButton+"DoKhan").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: [
                        { text: "Thường", value: "0" },
                        { text: "Hỏa tốc", value: "1" }
        ],
        index: 0
    }).data("kendoDropDownList");

    var comboNguonChiDao = $("#" + idButton + "NguonChiDao").kendoComboBox({
            filter: "contains",
            suggest: true
        }).data("kendoComboBox");

    var comboNguoiChiDao = $("#" + idButton + "NguoiChiDao").kendoComboBox({
        filter: "contains",
        suggest: true
    }).data("kendoComboBox");

    var comboNguoiTheoDoi = $("#" + idButton + "NguoiTheoDoi").kendoComboBox({
        filter: "contains",
        suggest: true
    }).data("kendoComboBox");

    var combodonViPhoiHop = $("#" + idButton + "DonViPhoiHop").kendoMultiSelect({
        autoClose: false,
        filter: "contains",
        suggest: true
    }).data("kendoMultiSelect");

    var comboDonvi = $("#" + idButton + "DonVi").kendoComboBox({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: dataDonVi,
        filter: "contains",
        suggest: true
    }).data("kendoComboBox");

    comboDonvi.value("");

    $("#" + idButton + "File").change(function() {
        $("label[for=" + idButton + "File]").html("<i class='glyphicon glyphicon-paperclip'></i> " + $(this).val());
        $("#" + idButton + "RemoveFile").show();
    });

    $("#" + idButton + "RemoveFile").click(function() {
        var file = document.getElementById(idButton + "File");
        file.value = file.defaultValue;

        $("label[for=" + idButton + "File]").html("<i class='glyphicon glyphicon-paperclip'></i> Đính kèm file");

        $(this).hide();
    });

    $("#" + idButton + "AddAttachment").click(function () {
        indexAttachment = indexAttachment + 1;
        addAttachment(indexAttachment);
    });

    $("#" + idButton).click(function () {

        $("#" + idButton + "Notification").hide();

        $('#' + idButton + "Modal").data("kendoWindow").open();

        if ($(dltd.elementDocument).length === 0) {

            $("#" + idButton + "Modal .responsive-table").hide();
            $("#" + idButton + "btnSend").parent().parent().parent().hide();
            //$("#" + idButton + "btnSend").hide();
            //$("#" + idButton + "AnotherOne").parent().hide();

            $("#" + idButton + "Notification")
                    .attr("class", "alert alert-warning")
                    .html("<strong>Yêu cầu!</strong> Bạn chưa chọn văn bản để gửi.")
                    .show();

            return;
        }
       
        $("#" + idButton + "YKCD").val("");
        $("#" + idButton + "HanXL").val("");

        comboDoKhan.value("0");
        $("#" + idButton + "DoKhan").val("0");
        //$("#" + idButton + "AllDonVi").attr('checked', false);
        comboNguonChiDao.value("");
        comboNguoiChiDao.value("");
        comboNguoiTheoDoi.value("");
        combodonViPhoiHop.dataSource.filter({});
        combodonViPhoiHop.value("".split(","));

        getUserId();

        $("#" + idButton + "RemoveFile").trigger("click");
        
        $("#" + idButton + "Modal .responsive-table").show();
        $("#" + idButton + "btnSend").parent().parent().parent().show();
        //$("#" + idButton + "Modal .form-group").show();
        //$("#" + idButton + "AnotherOne").parent().show();
        //$("#" + idButton + "btnSend").show();
        //$("#" + idButton + "Modal .form-group:first label").show();

        var idVanBan = $.trim($(dltd.elementDocument).find("td").eq(0).html());
        getAttachments(idVanBan);

        var Ngayky = $.trim($(dltd.elementDocument).find("td").eq(2).html());
        var SoKH = $.trim($(dltd.elementDocument).find("td").eq(5).html()); // +"/"+ $.trim($(dltd.elementDocument).find("td").eq(4).html());;
        var Trichyeu = $.trim($(dltd.elementDocument).find("td").eq(6).html());

        $("#" + idButton + "VanBan").html(SoKH + " - " + Trichyeu);
        $("#" + idButton + "IdVanBan").val(idVanBan);
        $("#" + idButton + "Ngayky").val(Ngayky);
        $("#" + idButton + "SoKH").val(SoKH);
        $("#" + idButton + "Trichyeu").val(Trichyeu);

        $("button[id^=" + idButton + "RemoveAttachment]").each(function() {
            $(this).click();
        });

    });

    $("#" + idButton + "DonVi").change(function() {
        $("#" + idButton + "Notification").hide();
    });
    $("#" + idButton + "NguonChiDao").change(function () {
        $("#" + idButton + "Notification").hide();
    });

    //$("#" + idButton + "AllDonVi").click(function() {
    //    var data = this.checked ? dataDonVi : dataDonViFiltered;

    //    var comboDonvi = $("#" + idButton + "DonVi").kendoComboBox({
    //        dataTextField: "text",
    //        dataValueField: "value",
    //        dataSource: data,
    //        filter: "contains",
    //        suggest: true
    //    }).data("kendoComboBox");

    //    comboDonvi.value("");
    //});

    $("#" + idButton + "btnSend").click(function () {

        if (isProcessing) return false;

        if (!checkValidData()) {
            $("#" + idButton + "Modal").animate({ scrollTop: "0px" });
            return false;
        }

        isProcessing = true;
        $("#" + idButton + "btnSend").attr("disabled", "disabled");
        $("#" + idButton + "btnSend").html("<i class='glyphicon glyphicon-send'></i> Đang thực hiện");

        var formData = new FormData($("form#" + idButton + "Form")[0]);
        $("#" + idButton + "Notification").hide();

        $.ajax({
            url: dltd.urlApi,
            type: "POST",
            data: formData,
            async: false,
            crossDomain: true,
            success: function (response) {
                if (response === "OK") {
                    $("#" + idButton + "Notification")
                        .attr("class", "alert alert-success")
                        .html("<strong>Thành công!</strong> Đã gửi văn bản qua hệ thống theo dõi chỉ đạo thành công.")
                        .show();

                    if ($("#" + idButton + "AnotherOne").is(":checked")) {
                        //TODO: May be need implement some business here
                    } else {
                        //Close modal now
                        setTimeout("$('#' + idButton + 'Modal').data('kendoWindow').close();", 3000);

                    }
                } else {
                    $("#" + idButton + "Notification")
                        .attr("class", "alert alert-danger")
                        .html("<strong>Lỗi!</strong> " + response)
                        .show();

                }

            },
            complete: function() {
                isProcessing = false;
                $("#" + idButton + "btnSend").removeAttr("disabled");
                $("#" + idButton + "btnSend").html("<i class='glyphicon glyphicon-send'></i> Lưu");

                $("#" + idButton + "Modal").animate({ scrollTop: "0px" });
            },
            cache: false,
            contentType: false,
            processData: false
        });

        return false;
    });
    $("#" + idButton + "btnContinue").click(function () {

        if (isProcessing) return false;

        if (!checkValidData()) {
            $("#" + idButton + "Modal").animate({ scrollTop: "0px" });
            return false;
        }

        isProcessing = true;
        $("#" + idButton + "btnSend").attr("disabled", "disabled");
        $("#" + idButton + "btnSend").html("<i class='glyphicon glyphicon-send'></i> Đang thực hiện");

        var formData = new FormData($("form#" + idButton + "Form")[0]);
        $("#" + idButton + "Notification").hide();

        $.ajax({
            url: dltd.urlApi,
            type: "POST",
            data: formData,
            async: false,
            crossDomain: true,
            success: function (response) {
                if (response === "OK") {
                    $("#" + idButton + "Notification")
                        .attr("class", "alert alert-success")
                        .html("<strong>Thành công!</strong> Đã gửi văn bản qua hệ thống theo dõi chỉ đạo thành công.")
                        .show();
                   
                } else {
                    $("#" + idButton + "Notification")
                        .attr("class", "alert alert-danger")
                        .html("<strong>Lỗi!</strong> " + response)
                        .show();

                }

            },
            complete: function() {
                isProcessing = false;
                $("#" + idButton + "btnSend").removeAttr("disabled");
                $("#" + idButton + "btnSend").html("<i class='glyphicon glyphicon-send'></i> Lưu");

                $("#" + idButton + "Modal").animate({ scrollTop: "0px" });
            },
            cache: false,
            contentType: false,
            processData: false
        });

        return false;
    });
});