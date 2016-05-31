$(function(){
    $('.toolsbox li>div').css({height:$(window).height()-$('.toolsbox').height()});
    $('.toolsbox li>a').click(function(){
        $(this).addClass('active')
        .siblings('div').toggleClass('hide')
        .parent().siblings('li').children('div').addClass('hide')
        .siblings('a').removeClass('active');
    })
    .siblings('div').children('div').each(function(){
        this.addEventListener('touchstart',function(e){
            e.preventDefault();
        },false);
        this.addEventListener('touchend',function(e){
            $(this).parent('div').addClass('hide');
        },false);
    })
    $(document)
    // 跳转到活动详情页面
    .on('click','.listbox>li',function(){
        location.href='http://test.cn-qch.com/h5/shareActivity.html?guid='+$(this).data('guid');
    })
    // 删选
    .on('click','.toolsbox li>div>a',function(){
        $(this).parent().addClass('hide');
        console.log($(this).text());
    })
    // 获取初始数据
    getData({ page: 1, pagesize: 10 });
})
// 滚动加载
var pageCount = 1,isOK=false;
$(window).bind('scroll',loadItem);
function loadItem(){
    var winHeight = $(window).height();
    var sTop = $(window).scrollTop();
    var boxHeight = $('.listbox').height();
    var $more=$('.container>.more');
    var moreHeight=$more.height();
    if ((winHeight + sTop >= boxHeight+moreHeight)&&isOk) {
        isOK=false;
        getData({ page: ++pageCount, pagesize: 10 },function(res){
            if(pageCount>=res.totalPages){
                $(window).unbind('scroll',loadItem);
                $more.removeClass('loading').text('没有更多了');
            }
        });
    }
}
// 获取数据
function getData(param,cb) {
    $.ajax({
        url: 'http://120.25.106.244:9001/api/Activity/GetAll',
        data: param,
        success: function (res) {
            if(res.totalItems==0){
                $('.container').addClass('nodata');
                return;
            }
            $('.listbox').append(template('listTemplate', { data: res }));
            isOk=true;
            (typeof cb=='function')&&cb(res);
        }
    })
}
// 解析日期 视图助手函数
template.helper('formatDate', function (dateStr, opt) {
    var ymd = opt && opt.ymd ? opt.ymd : '/';
    var hms = opt && opt.hms ? opt.hms : ':';
    var date = new Date(/^(\d)+$/.test(dateStr) ? parseInt(dateStr) : dateStr);
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