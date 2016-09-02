function G(id) {
    return document.getElementById(id);
}
//创建和初始化地图函数：
function initMap() {
    createMap();//创建地图
    setMapEvent();//设置地图事件
    addMapControl();//向地图添加控件
    addMarker();//向地图中添加marker
}

//创建地图函数：
function createMap() {
    var map = new BMap.Map("dituContent");//在百度地图容器中创建一个地图
    var point = new BMap.Point(113.775899, 34.767463);//定义一个中心点坐标
    map.centerAndZoom(point, 16);//设定地图的中心点和坐标并将地图显示在地图容器中
    window.map = map;//将map变量存储在全局
}

//地图事件设置函数：
function setMapEvent() {
    map.enableDragging();//启用地图拖拽事件，默认启用(可不写)
    map.enableScrollWheelZoom();//启用地图滚轮放大缩小
    map.enableDoubleClickZoom();//启用鼠标双击放大，默认启用(可不写)
    map.enableKeyboard();//启用键盘上下左右键移动地图
    //单击获取点击的经纬度
    map.addEventListener("click", function (e) {
        //alert(e.point.lng + "," + e.point.lat);
        //给文本框设置经纬度的值
        $("#txtLng").val(e.point.lng);
        $("#txtLat").val(e.point.lat);
    });
}

//地图控件添加函数：
function addMapControl() {
    //向地图中添加缩放控件
    var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
    map.addControl(ctrl_nav);
    //向地图中添加缩略图控件
    var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
    map.addControl(ctrl_ove);
    //向地图中添加比例尺控件
    var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
    map.addControl(ctrl_sca);
}

//标注点数组
var markerArr = [{ title: "青创汇", content: "绿地之窗", point: "113.776052|34.767678", isOpen: 0, icon: { w: 21, h: 21, l: 46, t: 46, x: 1, lb: 10 } }
];
//创建marker
function addMarker() {
    for (var i = 0; i < markerArr.length; i++) {
        var json = markerArr[i];
        var p0 = json.point.split("|")[0];
        var p1 = json.point.split("|")[1];
        var point = new BMap.Point(p0, p1);
        var iconImg = createIcon(json.icon);
        var marker = new BMap.Marker(point, { icon: iconImg });
        var iw = createInfoWindow(i);
        var label = new BMap.Label(json.title, { "offset": new BMap.Size(json.icon.lb - json.icon.x + 10, -20) });
        marker.setLabel(label);
        map.addOverlay(marker);
        label.setStyle({
            borderColor: "#808080",
            color: "#333",
            cursor: "pointer"
        });

        (function () {
            var index = i;
            var _iw = createInfoWindow(i);
            var _marker = marker;
            _marker.addEventListener("click", function () {
                this.openInfoWindow(_iw);
            });
            _iw.addEventListener("open", function () {
                _marker.getLabel().hide();
            })
            _iw.addEventListener("close", function () {
                _marker.getLabel().show();
            })
            label.addEventListener("click", function () {
                _marker.openInfoWindow(_iw);
            })
            if (!!json.isOpen) {
                label.hide();
                _marker.openInfoWindow(_iw);
            }
        })()
    }
}
//创建InfoWindow
function createInfoWindow(i) {
    var json = markerArr[i];
    var iw = new BMap.InfoWindow("<b class='iw_poi_title' title='" + json.title + "'>" + json.title + "</b><div class='iw_poi_content'>" + json.content + "</div>");
    return iw;
}
//创建一个Icon
function createIcon(json) {
    var icon = new BMap.Icon("http://app.baidu.com/map/images/us_mk_icon.png", new BMap.Size(json.w, json.h), { imageOffset: new BMap.Size(-json.l, -json.t), infoWindowOffset: new BMap.Size(json.lb + 5, 1), offset: new BMap.Size(json.x, json.h) })
    return icon;
}

initMap();//创建和初始化地图

var ac = new BMap.Autocomplete(    //建立一个自动完成的对象
    {
        "input": "suggestId"
    , "location": map
    });
ac.addEventListener("onhighlight", function (e) {  //鼠标放在下拉列表上的事件
    var str = "";
    var _value = e.fromitem.value;
    var value = "";
    if (e.fromitem.index > -1) {
        value = _value.province + _value.city + _value.district + _value.street + _value.business;
    }
    str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;

    value = "";
    if (e.toitem.index > -1) {
        _value = e.toitem.value;
        value = _value.province + _value.city + _value.district + _value.street + _value.business;
    }
    str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
    G("searchResultPanel").innerHTML = str;
});

var myValue;
ac.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
    var _value = e.item.value;
    myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
    G("searchResultPanel").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;

    setPlace();
});

function setPlace() {
    map.clearOverlays();    //清除地图上所有覆盖物
    function myFun() {
        var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
        $("#txtLng").val(pp.lng);
        $("#txtLat").val(pp.lat);
        map.centerAndZoom(pp, 18);
        map.addOverlay(new BMap.Marker(pp));    //添加标注
    }
    var local = new BMap.LocalSearch(map, { //智能搜索
        onSearchComplete: myFun
    });
    local.search(myValue);
}
//表单提交
function Save() {
    $("#form0").ajaxSubmit({
        type: 'POST',
        url: '/place/upload',
        data: $("#form0").serialize(),
        beforeSubmit: function () {
            //表单提交前
            var txtName = $("#txtName").val(); //众创空间名称                    
            var txtDisc = $("#txtDisc").val();  //城市区域
            var suggestId = $("#suggestId").val(); //详细地址
            var summaryContent = $("#summaryContent").val();  //一句话简介
            var serviceContent = $("#serviceContent").val();  //提供服务
            var introduceContent = $("#introduceContent").val();  //详细介绍
            //var entryContent = $("#entryContent").val();  //入驻条件
            //var policyContent = $("#policyContent").val();  //政策支持
            var labelContent = $("#labelContent").val();  //标签

            if (!$("#file").val()) {
                alert("请上传封面图片！");
                return false;
            }
            if (txtName == "") {
                alert("请填写空间名称");
                $("#groupInfor").show();
                return false;
            }
            if (txtDisc == "") {
                alert("请选择所在地");
                return false;
            }
            if (suggestId == "") {
                alert("请填写详细地址");
                $("#addressInfor").show();
                return false;
            }
            if (summaryContent == "") {
                alert("请填写一句话简介");
                $("#summaryInfor").show();
                return false;
            }
            if (serviceContent == "") {
                alert("请填写提供服务");
                $("#serviceInfor").show();
                return false;
            }
            if (introduceContent == "") {
                alert("请填写详细介绍");
                $("#introduceInfor").show();
                return false;
            }
            if (labelContent == "") {
                alert("请填写标签");
                $("#labelInfor").show();
                return false;
            }
        },
        success: function (msg) {
            alert(msg.Data);
            //if (msg.type == "success") {
            //    location.href = '/'
            //}
        },
        error: function (data, status, e) {
            alert('上传出错:' + e);
        }

    });
}
//三级联动
$(function () {
    $('body')

        // 显示城市列表
        .on('click', '#ulpro li', function () {
            var pid = this.id;
            //根据省份获取
            $.get('/activity/GetCity', { Id: pid }, function (msg) {
                $("#txtCity").val('');
                $("#txtDisc").val('');
                if (msg) {
                    //替换内容                    
                    $("#ulcity").html(msg);
                    $('#provId').val(pid);
                }
            })
        })
        // 显示区县列表
        .on('click', '#ulcity li', function () {
            var cid = this.id;
            //根据省份获取
            $.get('/activity/GetDis', { Id: cid }, function (msg) {
                $("#txtDisc").val('');
                if (msg) {
                    //替换内容                    
                    $("#uldisc").html(msg);
                    $('#cityId').val(cid);
                }
            })
        })
    //点击区县
    .on('click', '#uldisc li', function () {
        var did = this.id;
        $('#quId').val(this.id);
    })

})