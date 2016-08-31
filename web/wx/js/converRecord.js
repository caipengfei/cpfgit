// 滚动加载
var isOk = false,$ul;
var ajaxParam = { page: 1, pagesize: 10};
$(function () {
    $ul=$('.listsbox ul').eq(0);
    $('.listsbox').css({paddingTop:$('.header').height()});
    ajaxParam.type=$('.toolsbox li').eq(0).data('id');
    $('.toolsbox li').each(function () {
        util.addTapEvent(this,function(el){
            $(window).unbind('scroll', loadItem).bind('scroll', loadItem);
            $('.container').removeClass('nodata');
            $('.listsbox ul').empty();
            $('.container>.more').addClass('loading').text('');
            $(el).addClass('active').siblings('li').removeClass('active');
            ajaxParam.type=$(el).data('id');
            ajaxParam.page=1;
            $ul=$('.listsbox ul').eq($(el).index());
            getData(ajaxParam);
        })
    })
    // 获取初始数据
    getData(ajaxParam);
})
ajaxParam.userguid = util.getUrlParam('Guid');
sessionStorage.setItem('userguid', util.getUrlParam('Guid'));
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
        url: 'http://120.25.106.244:9001/api/convert/GetList',
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
            $ul.removeClass('hide').append(template('cjTemplate', { data: res })).siblings('ul').addClass('hide').empty();
            isOk = true;
            try{
                $('.subbox .image').each(function(){
                    $(this).css({height:$(this).width()});
                })
            }catch(e){}
            (typeof cb == 'function') && cb(res);
        }
    })
}
// 格式化日期 'yyyy/MM/dd hh:mm:ss' ---> '2016/08/07 12:05:08'
template.helper('formatDate',util.formatDate);
// 状态 0 待发货 1 已发货 2 已签收
template.helper('fhStatus',function(value){
    return value==0?'待发货':value==1?'已发货':'已签收';
})
template.helper('escText', function (text) { return encodeURI(text); })