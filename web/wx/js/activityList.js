$(function () {
    // 动态设置下拉菜单的高度
    $('.toolsbox li>div').css({ height: $(window).height() - $('.toolsbox').height()+1 });
    // 切换显示删选下拉菜单
    $('.toolsbox li>a').each(function () {
        addTapEvent(this,function(el){
            $(el).addClass('active')
                .siblings('div').toggleClass('hide')
                .parent().siblings('li').children('div').addClass('hide')
                .siblings('a').removeClass('active');
            var $div=$(el).siblings('div');
            if($div.hasClass('hide')){
                $('.container').removeClass('fixed');
                $('body').scrollTop(Math.abs(parseInt($('.container').css('top'))));
            }else{
                $('.container').css({
                    top:-$(window).scrollTop()
                }).addClass('fixed');
            }
        })
    })
    // 遮罩层触摸结束关闭下拉菜单
    .siblings('div').children('div').each(function () {
        this.addEventListener('touchstart', function (e) {
            e.preventDefault();
        }, false);
        this.addEventListener('touchend', function (e) {
            $(this).parent('div').addClass('hide');
            $('.container').removeClass('fixed');
            $('body').scrollTop(Math.abs(parseInt($('.container').css('top'))));
        }, false);
    })
    // 删选
    $('.toolsbox li>div>a').each(function(){
        addTapEvent(this,function(el){
            $(window).unbind('scroll', loadItem).bind('scroll', loadItem);
            $('.listbox li').remove();
            $('.container').removeClass('nodata fixed');
            $('.container>.more').addClass('loading').text('');
            $(el).parent().addClass('hide').siblings('a').find('i').text($(el).text());
            var index=$(el).parent().parent().index();
            switch (index){
                case 0:
                    ajaxParam.CityName=$(el).data('city');
                    break;
                case 1:
                    ajaxParam.days=$(el).data('days');
                    break;
                case 2:
                    ajaxParam.payType=$(el).data('type');
            }
            ajaxParam.page=1;
            getData(ajaxParam);
        })
    })
    // 获取初始数据
    getData(ajaxParam);
})
// 滚动加载
var isOk = false;
var ajaxParam = { page: 1, pagesize: 10, CityName: '', days: 0, payType: ''};
$(window).bind('scroll', loadItem);
function loadItem() {
    var winHeight = $(window).height();
    var sTop = $(window).scrollTop();
    var boxHeight = $('.listbox').height();
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
// 获取数据
function getData(param, cb) {
    $.ajax({
        url: 'http://120.25.106.244:9001/api/Activity/GetAll',
        data: param,
        cache:false,
        success: function (res) {
            if (res.totalItems == 0) {
                $('.container').addClass('nodata');
                return;
            }
            if(res.items.length<ajaxParam.pagesize){
                $('.container>.more').removeClass('loading').text('');
            }
            $('.listbox').append(template('listTemplate', { data: res }));
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
    return month + ymd + day + ' ' + hour + hms + minute;
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
        var x = Math.abs(this.endX - this.startX);
        var y = Math.abs(this.endY - this.startY);
        if (x <= 5 && y <= 5) {
            (typeof cb == 'function') && cb(this);
        }
    })
}