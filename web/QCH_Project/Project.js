var gridManager = null;
function InitGrid(arrButtons) {
    if (arrButtons.length > 0) {
        for (var i = 0; i < arrButtons.length; i++) {
            if (arrButtons[i].BtnNo == 'add') {
                $("#btnAdd").css('display', 'block');
                continue;
            }
            if (arrButtons[i].BtnNo == 'delete') {
                $("#btnDel").css('display', 'block');
                continue;
            }
            if (arrButtons[i].BtnNo == 'up') {
                $("#btnUp").css('display', 'block');
                continue;
            }
            if (arrButtons[i].BtnNo == 'audit') {
                $("#btnAudit").css('display', 'block');
                continue;
            }
        }
    }
    var names = $("#txtName").val();
    $("#maingrid").ligerGrid({
        checkbox: true,
        columns: [
                { display: '项目名称', isSort: true, name: 't_Project_Name', align: "center" },
                { display: '城市', isSort: true, name: 't_Project_CityName', align: "center" },
                { display: '项目领域', isSort: true, name: 'FiledName', align: "center", width: '13%' },
                { display: '项目阶段', isSort: true, name: 'PhaseName', align: "center", width: '13%' },
                { display: '融资阶段', isSort: true, name: 'FinancePhaseName', align: "center" },
                { display: '发起人', isSort: true, name: 't_User_LoginId', align: "center" },
                {
                    display: '是否审核', isAllowHide: false, isSort: true, width: "6%",
                    render: function (_data) {
                        var html = "";
                        if (_data.t_Project_Audit == 0) {
                            html = "<span style='color:Red'>否</span>";
                        }
                        else if (_data.t_Project_Audit == 1) {
                            html = "<span style='color:Green'>是</span>";
                        }
                        return html;
                    }
                },
                {
                    display: '是否推荐', isAllowHide: false, isSort: true, width: "6%",
                    render: function (_data) {
                        var html = "";
                        if (_data.t_Project_Recommend == 0) {
                            html = "<span style='color:Red'>否</span>";
                        }
                        else if (_data.t_Project_Recommend == 1) {
                            html = "<span style='color:Green'>是</span>";
                        }
                        return html;
                    }
                },
                {
                    display: '操作', isAllowHide: false, width: '15%',
                    render: function (_data) {
                        var html = "";
                        if (arrButtons.length > 0) {
                            for (var i = 0; i < arrButtons.length; i++) {
                                if (arrButtons[i].BtnNo == 'add') {
                                    continue;
                                }
                                if (arrButtons[i].BtnNo == 'delete') {
                                    continue;
                                }
                                if (arrButtons[i].BtnNo == 'up') {
                                    continue;
                                }
                                if (arrButtons[i].BtnNo == 'audit') {
                                    continue;
                                }
                                var img = "<img  border='0' title='" + arrButtons[i].BtnName + "' src='../lib/ligerUI/skins/icons/" + arrButtons[i].BtnIcon + ".gif' width='18' height='18' alt='' />";
                                html = html + "<a onclick=onedit('" + _data.Guid + "','" + arrButtons[i].BtnNo + "') style='margin:4px;'>" + img + "</a>";

                            }
                        }
                        return html;
                    }
                }
        ],
        dataAction: 'local', pageSize: 15, rowHeight: 23, headerRowHeight: 30,
        rownumbers: true,
        url: 'Ajax_Project.ashx',
        parms: [
                  { name: "Action", value: 'List' },
                    { name: "name", value: names }
        ],
        pageSizeOptions: [5, 10, 15, 20]
    });
    gridManager = $("#maingrid").ligerGetGridManager();
}
//操作按钮
function onedit(guid, btnid) {
    switch (btnid) {
        case "add":
            f_add();
            break;
        case "modify":
            f_modify(guid);
            break;
        case "delete":
            f_delete();
            break;
        case "view":
            f_view(guid);
            break;
        case "audit"://审核
            f_audit();
            break;
        case "up"://置顶
            f_up();
            break;
    }
}
///审核
function f_audit() {
    var rowsdata = gridManager.getCheckedRows();
    if (!rowsdata.length) {
        $.ligerDialog.alert('请先选择行!');
        return;
    }
    $.ligerDialog.waitting("正在推荐中...");
    var idStr = "";
    $(rowsdata).each(function (i, item) {
        idStr += this.Guid;
        if (i < rowsdata.length - 1) idStr += ",";
    });
    $.ligerDialog.confirm("确认审核此信息吗?", function (result) {
        if (result) {
            var JsonData = { Action: "Audit", GUID: idStr };
            $.post("Ajax_Project.ashx", JsonData, function (result) {
                if (result == "ok") {
                    alert("审核成功!");
                    Refresh();
                }
                else if (result == "fail") {
                    alert("审核失败!");
                }
                else {
                    alert(result);
                }
            });
        }
    });
    $.ligerDialog.closeWaitting();
}
///推荐
function f_up() {
    var rowsdata = gridManager.getCheckedRows();
    if (!rowsdata.length) {
        $.ligerDialog.alert('请先选择行!');
        return;
    }
    $.ligerDialog.waitting("正在推荐中...");
    var idStr = "";
    $(rowsdata).each(function (i, item) {
        idStr += this.Guid;
        if (i < rowsdata.length - 1) idStr += ",";
    });
    $.ligerDialog.confirm("确认推荐此项目吗?", function (result) {
        if (result) {
            var JsonData = { Action: "Recommend", GUID: idStr };
            $.post("Ajax_Project.ashx", JsonData, function (result) {
                if (result == "ok") {
                    alert("推荐成功!");
                    Refresh();
                }
                else if (result == "fail") {
                    alert("推荐失败!");
                }
                else {
                    alert(result);
                }
            });
        }
    });
    $.ligerDialog.closeWaitting();
}
///打开弹出框
function f_openWindow(_url, _title, _width, _height) {
    $.ligerDialog.open({
        width: _width, height: _height, url: _url, isResize: true, name: "iframeImplementor", title: _title, btnClose: function () { Refresh(); }
    });
}
//明细
function f_view(guid) {
    f_openWindow('ProjectView.aspx?Guid=' + guid, '项目明细', 850, 650);
}
//添加
function f_add() {
    f_openWindow('ProjectAddOrModify.aspx?action=add', '添加项目信息', 950, 550);
}
///修改
function f_modify(guid) {
    f_openWindow("ProjectAddOrModify.aspx?action=modify&Guid=" + guid, '修改项目信息', 950, 550);
}
///删除
function f_delete() {
    var rowsdata = gridManager.getCheckedRows();
    if (!rowsdata.length) {
        $.ligerDialog.alert('请先选择行!');
        return;
    }
    $.ligerDialog.waitting("正在删除中...");
    var idStr = "";
    $(rowsdata).each(function (i, item) {
        idStr += this.Guid;
        if (i < rowsdata.length - 1) idStr += ",";
    });
    $.ligerDialog.confirm("确认删除此信息吗?", function (result) {
        if (result) {
            var JsonData = { Action: "Delete", GUID: idStr };
            $.post("Ajax_Project.ashx", JsonData, function (result) {
                if (result == "ok") {
                    alert("删除成功!");
                    Refresh();
                }
                else if (result == "fail") {
                    alert("删除失败!");
                }
                else {
                    alert(result);
                }
            });
        }
    });
    $.ligerDialog.closeWaitting();
}
//刷新父页面
function RefreshParent() {
    parent.Refresh();
}

//刷新
function Refresh() {
    var gm = $("#maingrid").ligerGetGridManager();
    gm.loadData();
}
//保存
function btnSave() {
    var title = $.trim($("#txtTitle").val());
    if (title.trim() == "") {
        alert("项目名称不能为空!");
        return false;
    }
    var instruction = $.trim(UE.getEditor('content1').getContent());
    if (instruction.trim() == "") {
        alert("项目详情不能为空!");
        return false;
    }
    var userguid = $.trim($("#selUsers").val());
    if (userguid == "") {
        alert("发起人不能为空!");
        return false;
    }
    var placeguid = $.trim($("#selPlace").val());
    var oneword = $.trim($("#txtOneWord").val());
    var field = $.trim($("#FieldValue").val());
    var phase = $.trim($("#PhaseValue").val());
    var finance = $.trim($("#txtFinance").val());
    var financeuse = $.trim($("#txtFinanceUse").val());
    var financephase = $.trim($("#FinancePhaseValue").val());
    var parterwant = $.trim($("#ParterWantValue").val());
    var province = $.trim($("#selProvince").val());
    //城市id
    var city = $.trim($("#selCity").val());
    //城市名称
    var cityname = $.trim($("#selCity option:selected").text());
    var roadshow = $.trim($("#selRoadShow").val());
    var isin = $.trim($("#selIsIn").val());
    var coverpic = $.trim($("#hidFileName").val());
    var methodType = $.trim($("#hdType").val());
    var Guid = $.trim($("#hdId").val());
    var JsonData = { Action: "AddOrModify", UserGuid: userguid, PlaceGuid: placeguid, Title: title, OneWord: oneword, Field: field, Phase: phase, Finance: finance, FinanceUse: financeuse, FinancePhase: financephase, ParterWant: parterwant, Province: province, City: city, CityName: cityname, Instruction: instruction, IsRoadShow: roadshow, IsIn: isin, CoverPic: coverpic, Guid: Guid, MethodType: methodType };
    $.post("Ajax_Project.ashx", JsonData, function (result) {
        if (result == "ok") {
            alert("保存成功!");
            location.reload();
        }
        else if (result == "fail") {
            alert("保存失败!");
        }
        else {
            alert(result);
        }
    });
}
///城市列表
function provinceList() {
    $.ajax({
        type: "POST",
        url: "../China.ashx",
        data: { Action: "getProvince" },
        dataType: "html",
        async: false,
        success: function (msg) {
            $("#selProvince").html(msg);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (e, x) {
        }
    });
}
///城市列表
function cityList(province) {
    $.ajax({
        type: "POST",
        url: "../China.ashx",
        data: { Action: "getCity", Province: province },
        dataType: "html",
        async: false,
        success: function (msg) {
            $("#selCity").html(msg);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (e, x) {
        }
    });
}
///区县列表
function districtList(city) {
    $.ajax({
        type: "POST",
        url: "../China.ashx",
        data: { Action: "getDistrict", City: city },
        dataType: "html",
        async: false,
        success: function (msg) {
            $("#selDistrict").html(msg);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (e, x) {
        }
    });
}
///根据条件获取控件
function placelist(placename) {
    $.ajax({
        type: "POST",
        url: "../QCH_Place/Ajax_Place.ashx",
        data: { Action: "GetPlace", PlaceName: placename },
        dataType: "html",
        async: false,
        success: function (msg) {
            $("#selPlace").html(msg);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (e, x) {
        }
    });
}
///根据条件获取注册用户
function userlist(loginid) {
    $.ajax({
        type: "POST",
        url: "../QCH_User/Ajax_User.ashx",
        data: { Action: "GetUser", LoginID: loginid },
        dataType: "html",
        async: false,
        success: function (msg) {
            $("#selUsers").html(msg);
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function (e, x) {
        }
    });
}
///绑定数据
function getModel() {
    var guid = GetUrlParam("Guid");
    if (guid != null) {
        $.ajax({
            type: "POST",
            url: "Ajax_Project.ashx",
            data: { Action: "getModel", Guid: guid },
            dataType: "json",
            async: false,
            beforeSend: function () {

            },
            success: function (data) {
                $("#selUsers").val(data.t_User_Guid);
                $("#selPlace").val(data.t_Place_Guid);
                $("#selProvince").val(data.t_Project_Province);
                $("#selCity").val(data.t_Project_City);
            },
            complete: function (XMLHttpRequest, textStatus) {

            }
        });
    }
}
//此处用于加载类别的下拉列表无复选框
function StyleList(obj,valuename,cb) {
    $("#" + obj).ligerComboBox({
        width: 150,
        selectBoxWidth: 150,
        selectBoxHeight: 200,
        valueField: 'id',
        slide: false,
        treeLeafOnly: false,
        tree: {
            url: "../QCH_Style/Ajax_Style.ashx?Action=StyleData",
            textFieldName: "text",
            idFieldName: "id",
            parentIDFieldName: "pid",
            checkbox: cb
        },
        isExtend: function (row) {
            return false;
        },
        initValue: $("#" + obj).val(),
        //获取id时用
        valueFieldID: valuename
    });
}
