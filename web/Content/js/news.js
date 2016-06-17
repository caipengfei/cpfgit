/********************************* 下载APP盒子 *********************************/
$(function () {
    // 视口改变大小
    $(window).resize(function () {
        $('#indexTc').length && $('#indexTc').css('display') != 'none' && appDownBoxPos();
    }).resize();
    // 下载app
    $('.appDown').click(function () {
        appDownBoxPos();
        $('#cover2').show();
        $('#indexTc').show();
        $('#indexTc .qieHover li').eq(0).click();
    })
    // 关闭下载盒子
    $('.guanbiQu', '#indexTc').click(function () {
        $(this).parents('#indexTc').hide();
        $('#cover2').hide();
    })
    // 切换下载和关注
    $('.qieHover li', '#indexTc').click(function () {
        $(this).addClass('thisOver').siblings().removeClass('thisOver');
        $('.tcNr li').eq($(this).index()).show().siblings('li').hide();
    })
})
// 调节下载盒子的位置
function appDownBoxPos() {
    var boxHeight = $('#indexTc').height();
    var boxWidth = $('#indexTc').width();
    var winHeight = $(window).height();
    var winWidth = $(window).width();
    $('#indexTc').css({
        top: boxHeight > winHeight ? 0 : (winHeight - boxHeight) / 2,
        left: boxWidth > winWidth ? 0 : (winWidth - boxWidth) / 2
    });
}