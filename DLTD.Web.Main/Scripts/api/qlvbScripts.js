var dltd = {
    elementContainerButton: "nav.box-toolbar",
    elementDocument: "#listvanbandi div#grid div.k-grid-content table.k-selectable tr.k-state-selected",
    token: "",
    urlApi: "{{URI}}/Api/VanBan",
    urlLogin: "{{URI}}/Account/Login",
    urlChiTietVanBanDi: "{{QLVBURI}}/Vanbandi/_XemChitietVanban?id=",
    urlChiTietVanBanDen: "{{QLVBURI}}/Vanbanden/_XemChitietVanban?id="
};

var idButton = "";
var isProcessing = false;
var indexAttachment = 0;

function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 25; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}

function buildButton() {
    idButton = makeid();
    var buttonHtml = "<button data-toggle='modal' data-target='#"+ idButton +"Modal' class='btn btn-sm btn-warning btn-flat button-box' type='button' style='display:normal' id='" + idButton + "'>";
    buttonHtml += "<i class='glyphicon glyphicon-send'></i>";
    buttonHtml += "&nbsp;CSDL Theo dõi";
    buttonHtml += "</button>";

    var modalHtml = "<div id='" + idButton + "Modal' class='modal fade' role='dialog'>";
    modalHtml += "<div class='modal-dialog'>";
    modalHtml += "<div class='modal-content'>";
    modalHtml += "<div class='modal-header'>";
    modalHtml += "<button type='button' class='close' data-dismiss='modal'>&times;</button>";
    modalHtml += "<h4 class='modal-title'>NHẬP CSDL THEO DÕI</h4>";
    modalHtml += "</div>";

    modalHtml += "<div class='modal-footer'>";
    modalHtml += "<span>Phân công cho nhiều đơn vị <input id='" + idButton + "AnotherOne' type='checkbox'>  </span>";
    modalHtml += "<button type='button' style='width:60px;' class='btn btn-primary' id='" + idButton + "btnSend'><i class='glyphicon glyphicon-send'></i> Lưu </button>";
    modalHtml += "<button type='button' class='btn btn-default' data-dismiss='modal'>Đóng</button>";
    modalHtml += "</div>";

    modalHtml += "<div class='modal-body'>";
    modalHtml += "<form id='"+idButton+"Form' method='post' enctype='multipart/form-data'>";
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Văn bản:</label>";
    modalHtml += "<p class='control-label' id='"+ idButton + "VanBan'></p>";
    modalHtml += "</div>";

    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Đơn vị xử lý chính:</label>";
    modalHtml += "<select name='IdDonVi' id='" + idButton + "DonVi' style='width: 100%' placeholder='Chọn đơn vị xử lý chính...'>";
    //modalHtml += "<option></option>";
    modalHtml += "{{DONVI}}";
    modalHtml += "</select>";
    modalHtml += "</div>";
    //design all input here
    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Nội dung cần thực hiện:</label>";
    modalHtml += "<textarea type='text' name='YKienChiDao' rows='2' class='form-control' id='" + idButton + "YKCD'></textarea>";
    modalHtml += "</div>";

    modalHtml += "<div class='form-group row'>";
    modalHtml += "<div class='col-md-6'>";
    modalHtml += "<label class='control-label'>Thời hạn xử lý:</label><br/>";
    modalHtml += "<input name='ThoiHanXuLy' class='form-control' id='" + idButton + "HanXL'>";
    modalHtml += "</div>";
    modalHtml += "<div class='col-md-6'>";
    modalHtml += "<label class='control-label'>Độ khẩn:</label><br/>";
    modalHtml += "<input name='DoKhan' class='form-control' id='" + idButton + "DoKhan'>";
    modalHtml += "</div>";
    modalHtml += "</div>";

    //modalHtml += "<div class='form-group'>";
    //modalHtml += "<label class='control-label'>Độ khẩn:</label><br/>";
    //modalHtml += "<input name='DoKhan' class='form-control' id='" + idButton + "DoKhan'>";
    //modalHtml += "</div>";  

    //modalHtml += "<div class='form-group'>";
    //modalHtml += "<label class='control-label'>Nguồn chỉ đạo:</label><br/>";
    //modalHtml += "<input name='NguonChiDao' class='form-control' id='" + idButton + "NguonChiDao'>";
    //modalHtml += "</div>";

    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Nguồn chỉ đạo:</label>";
    modalHtml += "<select name='idKhoi' id='" + idButton + "NguonChiDao' style='width: 100%' placeholder='Chọn nguồn chỉ đạo...'>";
    //modalHtml += "<option></option>";
    modalHtml += "{{KHOI}}";
    modalHtml += "</select>";
    modalHtml += "</div>";

    modalHtml += "<div class='form-group'>";
    modalHtml += "<label class='control-label'>Đơn vị Phối hợp:</label>";
    modalHtml += "<select name='IdDonViPhoiHop' id='" + idButton + "DonViPhoiHop' multiple='multiple'  data-placeholder='Chọn đơn vị phối hợp xử lý...'>";
    modalHtml += "{{DONVI}}";
    modalHtml += "</select>";
    modalHtml += "</div>";

    modalHtml += "<div class='form-group row'>";
    modalHtml += "<div class='col-md-6'>";
    modalHtml += "<label class='control-label'>Người chỉ đạo:</label><br/>";
    modalHtml += "<select name='NguoiChiDao' class='form-control' id='" + idButton + "NguoiChiDao'>";
    modalHtml += "{{NGUOICHIDAO}}";
    modalHtml += "</select>";
    modalHtml += "</div>";

    modalHtml += "<div class='col-md-6'>";
    modalHtml += "<label class='control-label'>Người theo dõi:</label><br/>";
    modalHtml += "<select name='NguoiTheoDoi' class='form-control' id='" + idButton + "NguoiTheoDoi'>";
    modalHtml += "{{NGUOITHEODOI}}";
    modalHtml += "</select>";
    modalHtml += "</div>";
 

    modalHtml += "<div class='form-group' id='" + idButton + "groupAttachment'>";
    modalHtml += "<button type='button' class='btn btn-sm btn-primary' id='" + idButton + "AddAttachment'><span class='glyphicons glyphicons-plus'></span> Thêm file đính kèm</button><br><br>";
    modalHtml += "<label for='" + idButton + "File' class='btn btn-default'>";
    modalHtml += "<i class='glyphicon glyphicon-paperclip'></i> Đính kèm file";
    modalHtml += "</label>";
    modalHtml += " <span title='Xóa file đã chọn' style='display:none; cursor:pointer' class='glyphicon glyphicon-remove' id='" + idButton + "RemoveFile'></span>";
    modalHtml += "<input name='FileDinhKem' id='" + idButton + "File' type='file' style='display:none'/><div style='height:10px;width:100%'></div>";
    modalHtml += "</div>";

    modalHtml += "<input type='hidden' name='IdVanBan' id='" + idButton + "IdVanBan'>";
    modalHtml += "<input type='hidden' name='Ngayky' id='" + idButton + "Ngayky'>";    
    modalHtml += "<input type='hidden' name='SoKH' id='" + idButton + "SoKH'>";
    modalHtml += "<input type='hidden' name='UserId' id='" + idButton + "UserId'>";
    modalHtml += "<input type='hidden' name='Trichyeu' id='" + idButton + "Trichyeu'>";
    modalHtml += "<input type='hidden' name='FileVBDinhKem' id='" + idButton + "FileVBDinhKem'>";

    //end input
    modalHtml += "</form>";

    modalHtml += "<div id='" + idButton + "Notification' class='alert alert-success'>";
    modalHtml += "<strong>Success!</strong> thông báo ở đây";
    modalHtml += "</div>";

    modalHtml += "</div>";//end modal-body

    modalHtml += "</div>";
    modalHtml += "</div>";
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

}

function checkValidData() {
    var message = "<ul>";
    var valid = true;
    var comboDonvi = $("#" + idButton + "DonVi").data("kendoComboBox"); // Đối với mấy thằng xài kendo thì vầy
    if (comboDonvi.value() == "") {
        valid = false;
        message += "<li>Yêu cầu chọn đơn vị xử lý chính.</li>";
    }

    var ykcd = $("#" + idButton + "YKCD"); // Mấy thằng bình thường thì xài kiểu này. 
    if ($.trim(ykcd.val()) == "") {
        valid = false;
        message += "<li>Yêu cầu nhập ý kiến chỉ đạo.</li>";
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

                $("#" + idButton + "Modal .form-group").hide();
                $("#" + idButton + "btnSend").hide();
                $("#" + idButton + "AnotherOne").parent().hide();
            }
            else {
                $("#" + idButton + "UserId").val(data);
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
        url: dltd.urlChiTietVanBanDi + id,
        type: "GET",
        success: function (response) {
            if (response.length > 0 && response.indexOf("THÔNG TIN VĂN BẢN") >= 0) {
                var tdFile = $(response).find("#strfileattach");
                tdFile.find("a").each(function () {
                    if ($(this).attr("href").indexOf("javascript") === -1) {
                        linksFile += "$" + location.href.replace(location.pathname, "") + $(this).attr("href");
                        linksFile += "*" + $.trim($(this).text());
                    }
                });

                $("#" + idButton + "FileVBDinhKem").val(linksFile);

                var linkVBDen = $(response).find("a[href^='javascript:OpenTraloiVBDen']");
                if (linkVBDen.length > 0) {
                    var idVBDen = linkVBDen.attr("href").replace("javascript:OpenTraloiVBDen", "");
                    idVBDen = idVBDen.substr(1, idVBDen.length - 2);
                    getVanBanDen(idVBDen);
                }
            }
        },
        complete: function () {

        }
    });
}

function getVanBanDen(id) {
    var linksFile = $("#" + idButton + "FileVBDinhKem").val();
    $.ajax({
        url: dltd.urlChiTietVanBanDen + id,
        type: "GET",
        success: function (response) {
            if (response.length > 0 && response.indexOf("THÔNG TIN VĂN BẢN") >= 0) {
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

    var comboDonvi = $("#" + idButton + "DonVi").kendoComboBox({
            filter: "contains",
            suggest: true
        }).data("kendoComboBox");

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

        if ($(dltd.elementDocument).length === 0) {

            $("#" + idButton + "Modal .form-group").hide();
            $("#" + idButton + "btnSend").hide();
            $("#" + idButton + "AnotherOne").parent().hide();

            $("#" + idButton + "VanBan").html("Yêu cầu chọn văn bản để gửi.");

            $("#" + idButton + "Modal .form-group:first").show();
            $("#" + idButton + "Modal .form-group:first label").hide();
            return;
        }
       
        $("#" + idButton + "YKCD").val("");
        $("#" + idButton + "HanXL").val("");
        comboDoKhan.value("0");
        comboNguonChiDao.value("");
        comboDonvi.value("");
        comboNguoiChiDao.value("");
        comboNguoiTheoDoi.value("");
        combodonViPhoiHop.dataSource.filter({});
        combodonViPhoiHop.value("".split(","));

        getUserId();

        $("#" + idButton + "RemoveFile").trigger("click");

        $("#" + idButton + "Modal .form-group").show();
        $("#" + idButton + "AnotherOne").parent().show();
        $("#" + idButton + "btnSend").show();
        $("#" + idButton + "Modal .form-group:first label").show();

        var idVanBan = $.trim($(dltd.elementDocument).find("td").eq(0).html());
        getAttachments(idVanBan);

        var Ngayky = $.trim($(dltd.elementDocument).find("td").eq(2).html());
        var SoKH = $.trim($(dltd.elementDocument).find("td").eq(3).html()) +"/"+ $.trim($(dltd.elementDocument).find("td").eq(4).html());;
        var Trichyeu = $.trim($(dltd.elementDocument).find("td").eq(5).html());

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

    $("#" + idButton + "btnSend").click(function () {

        if (isProcessing) return false;

        if (!checkValidData()) return false;

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
                        setTimeout("$('#' + idButton + 'Modal').modal('hide');", 3000);
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
            },
            cache: false,
            contentType: false,
            processData: false
        });

        return false;
    });
});