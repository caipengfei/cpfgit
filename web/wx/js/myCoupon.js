$(function () {
    $ul=$('.listsbox ul').eq(0);
    curTemplateId=$('.toolsbox li').eq(0).data('tp');
    $('.toolsbox li').each(function () {
        util.addTapEvent(this,function(el){
            $(window).unbind('scroll', loadItem).bind('scroll', loadItem);
            $('.container').removeClass('nodata');
            $('.listsbox ul').empty();
            $('.container>.more').addClass('loading').text('');
            $(el).addClass('active').siblings('li').removeClass('active');
            ajaxParam.typeid=$(el).data('id');
            curTemplateId=$(el).data('tp');
            ajaxParam.page=1;
            $ul=$('.listsbox ul').eq($(el).index());
            getData(ajaxParam);
        })
    })
    // 获取初始数据
    getData(ajaxParam);
})
// 滚动加载
var isOk = false,curTemplateId,$ul;
var ajaxParam = { page: 1, pagesize: 10, typeid: 1};
ajaxParam.userguid=util.getUrlParam('guid');
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
// 获取数据
function getData(param, cb) {
    $.ajax({
        url: 'http://120.25.106.244:9001/api/voucher/GetAlluvByUser',
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
            $ul.removeClass('hide').append(template(curTemplateId, { data: res })).siblings('ul').addClass('hide').empty();
            isOk = true;
            (typeof cb == 'function') && cb(res);
        }
    })
}
/**
 * 视图助手定义
 */
// 格式化日期 'yyyy/MM/dd hh:mm:ss' ---> '2016/08/07 12:05:08'
template.helper('formatDate',util.formatDate);
// 是否过期 true ---> 过期 false ---> 没过期
template.helper('expire',function(value){
    return new Date(value.replace(/-/g,'/'))<(new Date());
})
// 转化为整型 '20.00' ---> 20
template.helper('toInt',function(value){
    return parseInt(value);
})