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
                { display: '活动标题', isSort: true, name: 't_Activity_Title', align: "center" },
                { display: '城市', isSort: true, name: 't_Activity_CityName', align: "center" },
                { display: '开始时间', isSort: true, name: 't_Activity_sDate', align: "center", width: '13%' },
                { display: '结束时间', isSort: true, name: 't_Activity_eDate', align: "center", width: '13%' },
                 //{ display: '限制人数', isSort: true, name: 't_Activity_LimitPerson', align: "center" },
                  {
                      display: '限制人数', isAllowHide: false, isSort: true, width: "6%",
                      render: function (_data) {
                          var html = "";
                          html = "<span style='color:Red'>" + _data.applyCount + "</span>" + "/" + _data.t_Activity_LimitPerson;
                          return html;
                      }
                  },
                  { display: '费用类型', isSort: true, name: 't_Activity_FeeType', align: "center" },
                  { display: '费用(￥)', isSort: true, name: 't_Activity_Fee', align: "center" },
                  { display: '发起人', isSort: true, name: 't_User_LoginId', align: "center" },
                  {
                      display: '是否审核', isAllowHide: false, isSort: true, width: "6%",
                      render: function (_data) {
                          var html = "";
                          if (_data.t_Activity_Audit == 0) {
                              html = "<span style='color:Red'>否</span>";
                          }
                          else if (_data.t_Activity_Audit == 1) {
                              html = "<span style='color:Green'>是</span>";
                          }
                          return html;
                      }
                  },
                {
                    display: '是否推荐', isAllowHide: false, isSort: true, width: "6%",
                    render: function (_data) {
                        var html = "";
                        if (_data.t_Activity_Recommand == 0) {
                            html = "<span style='color:Red'>否</span>";
                        }
                        else if (_data.t_Activity_Recommand == 1) {
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
        url: 'Ajax_Activity.ashx',
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
        case "map"://设置地图位置
            f_map(guid);
            break;
        case "apply"://报名人员列表
            f_apply(guid);
            break;
        case "reply"://评论人员列表
            f_reply(guid);
            break;
    }
}
//获取评论列表
function f_reply(Guid) {
    f_openWindow('ActivityReply.aspx?Guid=' + Guid, '评论列表', 850, 500);
}
//获取报名列表
function f_apply(Guid) {
    f_openWindow('ActivityApply.aspx?Guid=' + Guid, '报名人员', 850, 500);
}
//设置位置
function f_map(Guid) {
    f_openWindow('ActivityMap.aspx?Guid=' + Guid, '设置位置', 850, 650);
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
            $.post("Ajax_Activity.ashx", JsonData, function (result) {
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
    $.ligerDialog.confirm("确认推荐此信息吗?", function (result) {
        if (result) {
            var JsonData = { Action: "Recommend", GUID: idStr };
            $.post("Ajax_Activity.ashx", JsonData, function (result) {
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
    f_openWindow('ActivityView.aspx?Guid=' + guid, '活动明细', 850, 650);
}
//添加
function f_add() {
    f_openWindow('ActivityAddOrModify.aspx?action=add', '添加活动信息', 850, 550);
}
///修改
function f_modify(guid) {
    f_openWindow("ActivityAddOrModify.aspx?action=modify&Guid=" + guid, '修改活动信息', 850, 550);
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
            $.post("Ajax_Activity.ashx", JsonData, function (result) {
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
        alert("标题不能为空!");
        return false;
    }
    var instruction = $.trim(UE.getEditor('content1').getContent());
    if (instruction.trim() == "") {
        alert("活动说明不能为空!");
        return false;
    }
    var userguid = $.trim($("#selUsers").val());
    if (userguid == "") {
        alert("发起人不能为空!");
        return false;
    }
    var province = $.trim($("#selProvince").val());
    //城市id
    var city = $.trim($("#selCity").val());
    //城市名称
    var cityname = $.trim($("#selCity option:selected").text());
    var sdate = $.trim($("#txtsDate").val());
    if (sdate.trim() == "") {
        alert("活动开始时间不能为空!");
        return false;
    }
    var edate = $.trim($("#txteDate").val());
    if (edate.trim() == "") {
        alert("活动结束时间不能为空!");
        return false;
    }
    var coverpic = $.trim($("#hidFileName").val());
    var street = $.trim($("#txtStreet").val());
    if (street.trim() == "") {
        alert("活动地址不能为空!");
        return false;
    }
    var limitperson = $.trim($("#txtLimitPerson").val());
    var feetype = $.trim($("#selFeeType").val());
    var fee = $.trim($("#txtFee").val());
    var tel = $.trim($("#txtTel").val());
    var holder = $.trim($("#txtHolder").val());
    var methodType = $.trim($("#hdType").val());
    var Guid = $.trim($("#hdId").val());
    var JsonData = { Action: "AddOrModify", UserGuid: userguid, Title: title, CoverPic: coverpic, sDate: sdate, eDate: edate, Province: province, City: city, CityName: cityname, Street: street, Instruction: instruction, LimitPerson: limitperson, FeeType: feetype, Fee: fee, Tel: tel,Holder:holder, Guid: Guid, MethodType: methodType };
    $.post("Ajax_Activity.ashx", JsonData, function (result) {
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
            url: "Ajax_Activity.ashx",
            data: { Action: "getModel", Guid: guid },
            dataType: "json",
            async: false,
            beforeSend: function () {

            },
            success: function (data) {
                $("#selUsers").val(data.t_User_Guid);
                $("#selProvince").val(data.t_Activity_Province);
                $("#selCity").val(data.t_Activity_City);
            },
            complete: function (XMLHttpRequest, textStatus) {

            }
        });
    }
}
//设置地图位置
function btnMap() {
    var jing = $.trim($("#jing").val());
    var wei = $.trim($("#wei").val());
    if (jing == "") {
        alert("地理位置经度不能为空!");
        return false;
    }
    if (wei == "") {
        alert("地理位置纬度不能为空!");
        return false;
    }
    var guid = GetUrlParam("Guid");
    var JsonData = { Action: "SetMap", Guid: guid, Jing: jing, Wei: wei };
    $.post("Ajax_Activity.ashx", JsonData, function (result) {
        if (result == "ok") {
            //                    f_success();
            alert("位置设置成功!");
            location.reload();
        }
        else if (result == "fail") {
            alert("位置设置失败!");
        }
        else {
            alert(result);
        }
    });
}

