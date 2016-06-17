// http://120.25.106.244:9001/api/place/GetMyServices?page=1&pagesize=20&typeId=1&UserGuid='0c799012-8fa2-44d1-bd96-b57690d36873'
$(function () {
    $('.toolsbox li').each(function () {
        addTapEvent(this,function(el){
            $(window).unbind('scroll', loadItem).bind('scroll', loadItem);
            $('.container').removeClass('nodata');
            $('.listsbox ul').empty();
            $('.container>.more').addClass('loading').text('');
            $(el).addClass('active').siblings('li').removeClass('active');
            ajaxParam.typeId=$(el).index()+1;
            ajaxParam.page=1;
            getData(ajaxParam);
        })
    })
    // 获取初始数据
    getData(ajaxParam);
})
// 滚动加载
var isOk = false;
var ajaxParam = { page: 1, pagesize: 10, typeId: 2};
var templateIds=['spaceTemplate','activityTemplate','courseTemplate'];
ajaxParam.UserGuid=GetUrlParam('UserGuid')||GetUrlParam('userGuid');
$(window).bind('scroll', loadItem);
function loadItem() {
    var winHeight = $(window).height();
    var sTop = $(window).scrollTop();
    var boxHeight = $('.listsbox').height();
    if ((winHeight + sTop >= boxHeight) && isOk) {
        isOk = false;
        ajaxParam.page++;
        getData(ajaxParam, function (res) {
            if (ajaxParam.page >= res.totalPages) {
                $(window).unbind('scroll', loadItem);
                $('.container>.more').removeClass('loading').text('没有更多了');
            }
        });
    }
}
//获取Url中的参数
function GetUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
// 获取数据
function getData(param, cb) {
    $.ajax({
        url: 'http://120.25.106.244:9001/api/place/GetMyServices',
        data: param,
        cache:false,
        success: function (res) {
            if ((!res)||res.totalItems == 0) {
                $('.container').addClass('nodata');
                $('.listsbox ul').addClass('hide').empty();
                return;
            }
            if(res.items.length<param.pagesize){
                $('.container>.more').removeClass('loading').text('');
            }
            var $ul=$('.listsbox ul').eq(param.typeId-1);
            $ul.removeClass('hide').append(template(templateIds[param.typeId-1], { data: res })).siblings('ul').addClass('hide').empty();
            isOk = true;
            (typeof cb == 'function') && cb(res);
        }
    })
}
// 解析日期 视图助手函数
template.helper('formatDate', function (dateStr, opt) {
    var ymd = opt && opt.ymd ? opt.ymd : '/';
    var hms = opt && opt.hms ? opt.hms : ':';
    var date = new Date(/^(\d)+$/.test(dateStr) ? parseInt(dateStr) : dateStr.replace(/-/g,'/'));
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    month = month > 9 ? month : '0' + month;
    var day = date.getDate();
    day = day > 9 ? day : '0' + day;
    var hour = date.getHours();
    hour = hour > 9 ? hour : '0' + hour;
    var minute = date.getMinutes();
    minute = minute > 9 ? minute : '0' + minute;
    var sec = date.getSeconds();
    sec = sec > 9 ? sec : '0' + sec;
    return year+ymd+month + ymd + day + ' ' + hour + hms + minute;
})
template.helper('expire',function(value){
    return new Date(value.replace(/-/g,'/'))<(new Date());
})
function addTapEvent(el,cb){
    el.addEventListener('touchstart', function (e) {
        var touch = e.targetTouches[0];
        this.endX = this.startX = touch.clientX;
        this.endY = this.startY = touch.clientY;
    })
    el.addEventListener('touchmove', function (e) {
        var touch = e.targetTouches[0];
        this.endX = touch.clientX;
        this.endY = touch.clientY;
    })
    el.addEventListener('touchend', function (e) {
        e.preventDefault();
        var x = Math.abs(this.endX - this.startX);
        var y = Math.abs(this.endY - this.startY);
        if (x <= 5 && y <= 5) {
            (typeof cb == 'function') && cb(this);
        }
    })
}