$(function () {
    $(window).scroll(function () {
        var sT = $(this).scrollTop();
        var oT = $('.menunav')[0].offsetTop;
        var tH = $('.downloadbox').height();
        var nH = $('.menunav')[0].offsetHeight;
        var vH = $('.videobox')[0].offsetHeight;
        var bFixed = $('body').hasClass('fixed');
        if (sT >= oT - tH - vH) {
            $('body').addClass('fixed');
        }
        if ($('.detailsbox')[0].offsetTop - nH - tH - vH > sT && bFixed) {
            $('body').removeClass('fixed');
        }
        // if(bFixed){
        //     $('.contentbox>div').each(function(i){
        //         var oTop=this.offsetTop;
        //         var parTop=parseFloat($('.menunav').css('top'));
        //         var sTop=$(window).scrollTop();
        //         if(i==1){
        //             console.log(oTop,parTop,sTop);
        //         }
        //     })
        // }
    })

})

$(document).ajaxStop(function () {
    $('.container').removeClass('loading');
    $('.shade-div').remove();
});
function slide() {
    if ($(this).hasClass('slideup')) {
        $(this).siblings('p').find('span').removeClass('hide');
        $(this).removeClass('slideup').text('折叠');
    } else {
        $(this).siblings('p').find('span').addClass('hide');
        $(this).addClass('slideup').text('展开');
    }
}
function initpage() {
    $('.recommendbox li>img').css({
        height: $('.recommendbox li>img').width() * 2 / 3
    })
    $('.menunav').css({
        top: $('.downloadbox')[0].offsetHeight + $('.videobox')[0].offsetHeight - 1
    })
    $('.videobox video').bind('contextmenu', function () { return false; });
    //$('.videobox')[0].addEventListener('touchstart',stopScroll,false);
    $('header .menunav')[0].addEventListener('touchstart', function (e) {
        if ($('body').hasClass('fixed')) {
            e.preventDefault();
        }
    }, false);
    $('header .menunav a').each(function () {
        this.addEventListener('touchend', function (e) {
            e.preventDefault();
            var $tar = $($(this).data('tar'));
            var offsetTop = $tar[0].offsetTop;
            var tableHeight = $('.downloadbox').height();
            var vH = $('.videobox')[0].offsetHeight;
            var scTop = offsetTop - tableHeight - $('.menunav')[0].offsetHeight - vH;
            $(this).addClass('active').siblings().removeClass('active');
            $('html,body').animate({
                scrollTop: scTop
            })
        }, false)
    })
    $('.detailsbox a').click(slide);
    $('.recommendbox li').click(function () {
        location.href = '/h5/shareCourse.html' + '?guid=' + $(this).data('guid');
    })
    $('.tutorbox li').click(function () {
        location.href = '/h5/ShareTutor.html' + '?guid=' + $(this).data('guid');
    })
}
// 解析日期字符串
function parseDate(dateString, opt) {
    var ymd = opt && opt.ymd ? opt.ymd : '/';
    var hms = opt && opt.hms ? opt.hms : ':';
    var date = new Date(dateString);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    month = month > 9 ? month : '0' + month;
    var day = date.getDay();
    day = day > 9 ? day : '0' + day
    var hour = date.getHours();
    hour = hour > 9 ? hour : '0' + hour;
    var minute = date.getMinutes();
    minute = minute > 9 ? minute : '0' + minute;
    var sec = date.getSeconds();
    sec = sec > 9 ? sec : '0' + sec;
    return year + ymd + month + ymd + day + ' ' + hour + hms + minute + hms + sec;
}
function GetUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function godown() {
    location.href = 'http://www.cn-qch.com/app';
}
function stopScroll(e) {
    e.preventDefault();
}
// 图片懒加载
$(window).bind('scroll', lazy_img);
function lazy_img() {
    var len = $('img.lazy').length;
    !len && $(window).unbind('scroll', lazy_img);
    function getOffsetTop(el) {
        var y = el.offsetTop;
        while (el = el.offsetParent) {
            y += el.offsetTop;
        }
        return y;
    }
    $('img.lazy').each(function () {
        var oTop = getOffsetTop(this);
        var sTop = $(document).scrollTop();
        var winH = $(window).height();
        if (sTop + winH >= oTop) {
            $(this).attr('src', $(this).data('src')).removeClass('lazy');
            this.removeAttribute('data-src');
        }
    })
}