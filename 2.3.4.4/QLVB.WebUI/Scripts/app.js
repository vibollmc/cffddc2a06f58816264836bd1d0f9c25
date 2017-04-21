$(function () {
    $('[data-toggle="popover"]').popover({
        trigger: 'click' //click | hover | focus | manual

    });
    $('body').on('click', function (e) {
        // auto hide popover
        $('[data-toggle="popover"]').each(function () {
            //the 'is' for buttons that trigger popups
            //the 'has' for icons within a button that triggers a popup
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
        });
    });
});

//==============================================

function ShowResult(item, strnoidung) {
    var id = "#" + item;
    $(id).text(strnoidung);
    $(id).show();
}
function HideResult(item) {
    var id = "#" + item;
    $(id).hide();
}
function SetGridHeight() {

    //SetContentHeight();

    var gridElement = $("#grid"),
        newHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 0;
        
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();        
    });
    var contentHeight = $('#content').innerHeight(),
        headerHeight = $('#head').innerHeight(),
        contentWidth = $('#content').innerWidth(),
        $windowHeight = $(window).height();        

    if (contentWidth > 700) {       
        contentHeight = $windowHeight - 70;
        gridElement.children(".k-grid-content").height(contentHeight - otherElementsHeight - headerHeight - 65);
    } else {
        gridElement.children(".k-grid-content").height(200);
    }    
}
function SetContentHeight() {
    var contentHeight = $('#content').innerHeight(),
        headerHeight = $('#head').innerHeight(),
        contentWidth = $('#content').innerWidth(),
        $windowHeight = $(window).height(),
        $windowWidth = $(window).width(),
        $isFullScreen = $('body').hasClass('fullScreen');

    //console.log("height" + $windowHeight);
    //console.log("width" +$windowWidth);
    if ($isFullScreen) {        
        if ($windowWidth > 992) {
            $('#content').height($windowHeight - 70);
        }
    } else {
        if ($windowHeight > 450) {
            $('#content').height($windowHeight - 70);
        } else {
            $('#content').height($windowHeight - 100);
        }
    }
}

function RefreshGrid() {
    $("#grid").data("kendoGrid").dataSource.read();
    $("#grid").data("kendoGrid").refresh();
}
function SetTreeViewHeight() {
    var treeview = $("#treeview");

    var contentHeight = $('#content').innerHeight(),
        headerHeight = $('#head').innerHeight(),
        contentWidth = $('#content').innerWidth(),
        $windowHeight = $(window).height(),
        $windowWidth = $(window).width();

    if (contentWidth > 700) {
        contentHeight = $windowHeight - 100;        
        treeview.height(contentHeight - headerHeight - 65);
    } else {
        treeview.height(200);
    }
}
function ButtonEnable(item, bool) {
    var id = '#' + item;
    if (bool) {
        $(id).removeAttr("disabled");
    } else {
        $(id).attr("disabled", "disabled");
    }
}

$.fn.outerHTML = function () {
    //https://gist.github.com/jboesch/889005
    // IE, Chrome & Safari will comply with the non-standard outerHTML, all others (FF) will have a fall-back for cloning
    return (!this.length) ? this : (this[0].outerHTML || (
      function (el) {
          var div = document.createElement('div');
          div.appendChild(el.cloneNode(true));
          var contents = div.innerHTML;
          div = null;
          return contents;
      })(this[0]));

}
//===============================================
function editdate(str) {
    var value;
    if (str.substr(str.length - 1, 1) == "/" && str.substr(str.length - 2, 1) == "/") {
        return str.substr(0, str.length - 1)
    }
    switch (str.length) {
        case 1:
            if (str.substr(0, 1) >= 4) {
                value = "0" + str.substr(0, 1) + "/";
            }
            else {
                value = str;
            }
            break;
        case 2:
            if (str.substr(1, 1) == "/" && str.substr(0, 1) >= 1 && str.substr(0, 1) <= 9) {
                value = "0" + str.substr(0, str.length);
            }
            else {
                if (str >= 1 && str <= 31) {
                    value = str + "/";
                }
                else {
                    value = str.substr(0, 1);
                }
            }
            break;
        case 3:
            if (str.substr(1, 1) == "/" && str.substr(0, 1) >= 1 && str.substr(0, 1) <= 9) {
                value = "0" + str.substr(0, str.length);
            }
            else {
                value = str;
            }
            break;
        case 4:
            if (str.substr(3, 1) >= 2 && str.substr(2, 1) == "/") {
                value = str.substr(0, 3) + "0" + str.substr(3, 1) + "/";
            }
            else {
                value = str.substr(0, 4);
            }
            break;
        case 5:
            if (str.substr(str.length - 1, 1) == "/") {
                value = str.substr(0, str.length - 2) + "0" + str.substr(str.length - 2, str.length);
            }
            else {
                if (str.substr(3, 2) >= 1 && str.substr(3, 2) <= 12) {
                    value = str + "/";
                }
                else {
                    value = str.substr(0, str.length - 1);
                }
            }
            break;
        case 8:
            if (str.substr(str.length - 2, 2) >= 45) {
                value = str.substr(0, str.length - 2) + "19" + str.substr(str.length - 2, 2);
            }
            else {
                if (str.substr(str.length - 2, 2) <= 18) {
                    value = str.substr(0, str.length - 2) + "20" + str.substr(str.length - 2, 2);
                }
                else {
                    value = str;
                }
            }
            break;
        default:
            value = str;
    }
    return value;
}