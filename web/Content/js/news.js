/********************************* 下载APP盒子 *********************************/
$(function() {
        // 视口改变大小
        $(window).resize(function() {
            $('#indexTc').length && $('#indexTc').css('display') != 'none' && appDownBoxPos();
        }).resize();
        // 下载app
        $('.appDown').click(function() {
                appDownBoxPos();
                $('#cover2').show();
                $('#indexTc').show();
                $('#indexTc .qieHover li').eq(0).click();
            })
            // 关闭下载盒子
        $('.guanbiQu', '#indexTc').click(function() {
                $(this).parents('#indexTc').hide();
                $('#cover2').hide();
            })
            // 切换下载和关注
        $('.qieHover li', '#indexTc').click(function() {
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
/********************************* banner *********************************/
var banner={
    init:function(selector){
        this.parent = $(selector);
        this.frame = this.parent.find('.frame');
        this.box = this.frame.find('.box');
        this.ctrls2 = this.parent.find('.controls_2');
        this.detailsBox = this.parent.find('.detailsbox');
        this.links = this.parent.find('.frame .box a');
        this.curIndex = 0;
        this.box.append(this.box.html());
        this.box = this.frame.find('.box');
        this.len = this.box.find('a').length / 2;
        this.imgWidth = this.frame.width();
        this.box.find('a').css({ width: this.imgWidth })
        this.box.css({ width: this.imgWidth * this.len * 2 })
        this.updateContent(this.curIndex);
        var that = this;
        that.tid = setInterval(function(){ banner.next(); }, 3000);
        $('.banner').mouseover(function(){
            clearInterval(banner.tid);
        }).mouseout(function(){
            that.tid = setInterval(function(){ banner.next(); }, 3000);
        });
        this.ctrls2.find('a').mouseover(function() {
            that.curIndex = $(this).index();
            $('.frame').stop().animate({
                scrollLeft: that.curIndex * that.imgWidth
            })
            $(this).addClass('active').siblings().removeClass('active');
            that.updateContent(that.curIndex);
        })
    },
    prev:function() {
        if (this.curIndex <= 0) {
            this.curIndex = this.len - 1;
            this.frame.scrollLeft(this.len * this.imgWidth);
        } else {
            this.curIndex--;
        }
        this.frame.stop().animate({
            scrollLeft: this.curIndex * this.imgWidth
        });
        this.ctrls2.find('a').eq(this.curIndex).addClass('active')
            .siblings().removeClass('active');
        this.updateContent(this.curIndex);
    },
    next:function() {
        if (this.curIndex >= this.len - 1) {
            this.curIndex = -1;
        } else {
            if (this.curIndex == -1) {
                $('.frame').scrollLeft(0);
                this.curIndex = 0;
            }
            this.curIndex++;
        }
        $('.frame').stop().animate({
            scrollLeft: (this.curIndex == -1 ? this.len : this.curIndex) * this.imgWidth
        })
        var cur = this.curIndex == -1 ? 0 : this.curIndex;
        this.ctrls2.find('a').eq(cur).addClass('active')
            .siblings().removeClass('active');
        this.updateContent(cur);
    },
    updateContent:function(index){
        banner.detailsBox.find('h2').text(banner.links.eq(index).data('h'));
        banner.detailsBox.find('p').text(banner.links.eq(index).data('p'));
        banner.detailsBox.find('span').text(banner.links.eq(index).data('date'));
        banner.detailsBox.data('url',banner.links.eq(index).attr('href'));
    }
}