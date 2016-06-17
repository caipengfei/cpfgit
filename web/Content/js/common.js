/*公共类*/

//消息弹框
function tipInfo(t) {
    $('p.tipinfo').remove();
    clearTimeout(window.tid);
    var winWidth = $(window).width();
    var winHeight = $(window).height();
    var $tipinfo = $('<p class="tipinfo">').text(t).css({
        position: 'fixed',
        textAlign: 'center',
        fontSize: '1.1rem',
        color: '#000',
        borderRadius: '.5rem',
        background: '#fff',
        padding: '3rem',
        boxShadow: '#888 0 0 5px',
        transform: 'translate3d(0,0,0)'
    }).appendTo('body');
    var left = (winWidth - $('p.tipinfo')[0].offsetWidth) / 2;
    var top = (winHeight - $('p.tipinfo')[0].offsetHeight) / 2;
    $tipinfo.css({ left: left, top: top });
    window.tid = setTimeout(function () {
        $tipinfo.css({ top: top + 30, opacity: 0, transition: 'top .6s,opacity .6s,width .6s' });
        setTimeout(function () { $tipinfo.remove(); }, 600);
    }, 1200)
}