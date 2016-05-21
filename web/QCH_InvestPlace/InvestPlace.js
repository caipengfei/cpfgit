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
        }
    }
    var names = $("#txtName").val();
    $("#maingrid").ligerGrid({
        checkbox: true,
        columns: [
                { display: '投资空间名称', isSort: true, name: 't_InvestPlace_Title', align: "center" },
                { display: '投资金额', isSort: true, name: 't_InvestPlace_Money', align: "center" },
                {
                    display: '是否推荐', isAllowHide: false, isSort: true, width: "6%",
                    render: function (_data) {
                        var html = "";
                        if (_data.t_InvestPlace_Recommend == 0) {
                            html = "<span style='color:Red'>否</span>";
                        }
                        else if (_data.t_InvestPlace_Recommend == 1) {
                            html = "<span style='color:Green'>是</span>";
                        }
                        return html;
                    }
                },
                {
                    display: '操作', isAllowHide: false, width: '10%',
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
        url: 'Ajax_InvestPlace.ashx',
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
        case "delete"://删除
            f_delete();
            break;
        case "view"://明细
            f_view(guid);
            break;
        case "up"://推荐
            f_up();
            break;
        case "case"://孵化案例
            f_case(guid);
            break;
        case "memeber"://入驻成员
            f_memeber(guid);
            break;
    }
}
//孵化案例选择列表
function f_case(Guid) {
    f_openWindow('InvestPlaceCase.aspx?Guid=' + Guid, '选择项目列表', 850, 500);
}
//入驻成员选择列表
function f_memeber(Guid) {
    f_openWindow('InvestPlaceMember.aspx?Guid=' + Guid, '选择成员列表', 550, 500);
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
    $.ligerDialog.confirm("确认推荐此信息吗?", function (result) {
        if (result) {
            var JsonData = { Action: "Up", GUID: idStr };
            $.post("Ajax_InvestPlace.ashx", JsonData, function (result) {
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
function f_openWindow(_url, _title, _width, _height) {
    $.ligerDialog.open({
        width: _width, height: _height, url: _url, isResize: true, name: "iframeImplementor", title: _title, btnClose: function () { Refresh(); }
    });
}
//明细
function f_view(guid) {
    f_openWindow('InvestPlaceView.aspx?Guid=' + guid, '投资空间明细', 750, 500);
}
//添加
function f_add() {
    f_openWindow('InvestPlaceAddOrModify.aspx?action=add', '添加投资空间', 650, 500);
}
///修改
function f_modify(guid) {
    f_openWindow("InvestPlaceAddOrModify.aspx?action=modify&Guid=" + guid, '修改投资空间', 650, 500);
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
            $.post("Ajax_InvestPlace.ashx", JsonData, function (result) {
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
    if (title == "") {
        alert("标题不能为空!");
        return false;
    }
    var instruction = $.trim($("#txtInstruction").val());
    if (instruction == "") {
        alert("详细介绍内容不能为空!");
        return false;
    }
    var phase = $.trim($("#PhaseValue").val());
    var area = $.trim($("#AreaValue").val());
    var money = $.trim($("#txtMoney").val());
 
    var pic = $.trim($("#hidFileName").val());
    var methodType = $.trim($("#hdType").val());
    var Guid = $.trim($("#hdId").val());
    var JsonData = { Action: "AddOrModify", Title: title, Instruction: instruction, Phase: phase, Area: area, Money: money, Pic: pic, Guid: Guid, MethodType: methodType };
    $.post("Ajax_InvestPlace.ashx", JsonData, function (result) {
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
//此处用于加载类别的下拉列表无复选框
function StyleList(objContral, valueName, ifche) {
    $("#" + objContral).ligerComboBox({
        width: 200,
        selectBoxWidth: 200,
        selectBoxHeight: 200,
        valueField: 'id',
        slide: false,
        treeLeafOnly: false,
        tree: {
            url: "../QCH_Style/Ajax_Style.ashx?Action=StyleData",
            textFieldName: "text",
            idFieldName: "id",
            parentIDFieldName: "pid",
            checkbox: ifche
        },
        isExtend: function (row) {
            return false;
        },
        initValue: $("#" + objContral).val(),//objContral
        //获取id时用
        valueFieldID: valueName//valueName
    });
}



