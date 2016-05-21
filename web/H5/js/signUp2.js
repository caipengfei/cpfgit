$(function () {
    $('#saveSign')[0].addEventListener('touchend', function (e) {
        e.preventDefault();
        var winHeight = $(window).height();
        var winWidth = $(window).width();
        var bodyHeight = $('html')[0].offsetHeight;
        var signWidth = $('#cardBox').width();
        var signHeight = $('#cardBox').height();
        var scale = signWidth / signHeight;
        $('#clipBox').css({
            height: bodyHeight,
            width: winWidth
        }).removeClass('hide');

        html2canvas($('#cardBox')[0], {
            onrendered: function (canvas) {
                var canvasData = canvas.toDataURL();
                $('.myClip img').attr('src', canvasData);
                var w = $('.myClip').width();
                $('.myClip img').css({
                    height: w / scale
                });

                $('.myClip').css({
                    top: (winHeight - $('.myClip')[0].offsetHeight) / 2,
                    left: (winWidth - $('.myClip')[0].offsetWidth) / 2
                }).css({
                    opacity: 1
                });
            }
        })

    }, false);

    $('.shade')[0].addEventListener('touchend', closeClip, false);
    $('#clipBox .close')[0].addEventListener('touchend', closeClip, false);
    $('.address').html(text2html($('.address').text()));
})
function closeClip(e) {
    e.preventDefault();
    $(this).parents('#clipBox').addClass('hide');
}

function text2html(text) {
    var html = '', s = 0, step = 14;
    for (var i = step; i < text.length + step; i += step) {
        html += '<b style="font-weight:normal;">' + text.slice(s, i) + '</b>';
        s = i;
    }
    return html;
}