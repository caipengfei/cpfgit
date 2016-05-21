<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityMap.aspx.cs" Inherits="Maticsoft.Web.QCH_Activity.ActivityMap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../lib/jquery/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.3"></script>
    <script src="Activity.js" type="text/javascript"></script>
</head>
<body style="background: #CBE1FF">
    <div style="width: 730px; margin: auto;">
        地址：<input id="text_" type="text" value="" style="margin-right: 100px;" />
        <%--查询结果(经纬度)：<input id="result_" type="text" />--%>
        经度:<input id="jing" type="text" style="width:80px" /> 纬度:<input id="wei" type="text" style="width:80px" />
        <input type="button" value="查询" onclick="searchByStationName();" />
         <input type="button" value="提交" id="Submit1" runat="server" class="btnStyle" onclick="return btnMap();" />
        <div id="container"
            style="position: absolute; margin-top: 30px; width: 530px; height: 490px; top: 50px; border: 1px solid gray; overflow: hidden;">
        </div>
    </div>
</body>
<script type="text/javascript">
    ///自动定位
    //    $(function () {
    //        navigator.geolocation.getCurrentPosition(translatePoint); //定位 
    //    });
    //    function translatePoint(position) {
    //        var currentLat = position.coords.latitude;
    //        var currentLon = position.coords.longitude;
    //        alert("经度:" + currentLat + "纬度:" + currentLon);
    //    }
    var map = new BMap.Map("container");
    map.centerAndZoom("郑州", 12);
    map.enableScrollWheelZoom();    //启用滚轮放大缩小，默认禁用
    map.enableContinuousZoom();    //启用地图惯性拖拽，默认禁用

    map.addControl(new BMap.NavigationControl());  //添加默认缩放平移控件
    map.addControl(new BMap.OverviewMapControl()); //添加默认缩略地图控件
    map.addControl(new BMap.OverviewMapControl({ isOpen: true, anchor: BMAP_ANCHOR_BOTTOM_RIGHT }));   //右下角，打开

    var localSearch = new BMap.LocalSearch(map);
    localSearch.enableAutoViewport(); //允许自动调节窗体大小
    function searchByStationName() {
        map.clearOverlays(); //清空原来的标注
        var keyword = document.getElementById("text_").value;
        localSearch.setSearchCompleteCallback(function (searchResult) {
            var poi = searchResult.getPoi(0);
            //document.getElementById("jing").value = poi.point.lng;
            //document.getElementById("wei").value = poi.point.lat;
            map.centerAndZoom(poi.point, 13);
            var marker = new BMap.Marker(new BMap.Point(poi.point.lng, poi.point.lat));  // 创建标注，为要查询的地方对应的经纬度
            map.addOverlay(marker);
            var content = document.getElementById("text_").value + "<br/><br/>经度：" + poi.point.lng + "<br/>纬度：" + poi.point.lat;
            var infoWindow = new BMap.InfoWindow("<p style='font-size:14px;'>" + content + "</p>");
            marker.addEventListener("click", function () { a(poi.point.lng, poi.point.lat) }); //this.openInfoWindow(infoWindow);
            // marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
        });
        localSearch.search(keyword);
    }
    function a(j, w) {
        document.getElementById("jing").value = j;
        document.getElementById("wei").value = w;
        //alert("经度:" + j + "纬度:" + w);
    }
</script>
</html>

