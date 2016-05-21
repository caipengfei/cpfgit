/********************************* 轮播图切换开始 *********************************/
var current = 0, dur = 400;
$(function () {
    var bannerHeight = $('.banner')[0].offsetHeight;
    var controlsHeight = $('.banner .controls a')[0].offsetHeight;
    $('.banner .controls a').css({
        top: (bannerHeight - controlsHeight) / 2
    })
    $('body')
        .on('click', '.banner .controls a.prev', prev)
        .on('click', '.banner .controls a.next', next)
        .on('click', '.banner .controls_2 a', function () {
            var $li = $('.banner .box li');
            $li.eq(current).stop().fadeOut(dur);
            current = $(this).index();
            $li.eq(current).stop().fadeIn(dur);
            $(this).addClass('current').siblings().removeClass('current');
        })
        .on('mouseout', '.banner a', timer)
        .on('mouseover', '.banner a', stopTimer)
    window.onfocus = timer;
    window.onblur = stopTimer;
    timer();
})
function prev() {
    var $li = $('.banner .box li');
    $li.eq(current).stop().fadeOut(dur);
    current = (current <= 0) ? $li.length - 1 : current - 1;
    $li.eq(current).stop().fadeIn(dur);
    $('.banner .controls_2 a').eq(current).addClass('current').siblings().removeClass('current');
}
function next() {
    var $li = $('.banner .box li');
    $li.eq(current).stop().fadeOut(dur);
    current = (current >= $li.length - 1) ? 0 : current + 1;
    $li.eq(current).stop().fadeIn(dur);
    $('.banner .controls_2 a').eq(current).addClass('current').siblings().removeClass('current');
}
function timer() {
    window.tid && stopTimer();
    window.tid = setInterval(next, 4000);
}
function stopTimer() {
    clearInterval(window.tid);
    window.tid = 0;
}

/********************************* 视频播放控制 *********************************/
var isplaying = false;
$(function () {
    var video = $('.row6 video')[0];
    if (video != undefined) {
        setInterval(function () {
            var video = $('.row6 video')[0];
            var offsetTop = video.offsetTop;
            var scrollTop = $(window).scrollTop();
            var winHeight = $(window).height();
            var screenHeight = screen.height;
            if (winHeight == screenHeight) return;
            if (isplaying) {
                if (scrollTop + winHeight < offsetTop) {
                    video.pause();
                    video.currentTime = 0;
                    isplaying = false;
                }
            } else {
                if (scrollTop + winHeight >= offsetTop) {
                    video.play();
                    isplaying = true;
                }
            }
        }, 1000)
    }
})

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