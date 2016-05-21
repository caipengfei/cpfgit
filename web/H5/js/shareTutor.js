/* 图片轮播 构造函数 */ 
function Banner(selector) {
    this.scope = $(selector);
    this.startX = 0;
    this.endX = 0;
    this.posX = 0;
    this.curIndex = 0;
    this.box=this.scope.find('.box')[0];
    this.imgs = this.scope.find('img');
    this.startTime = 0;
    this.bLeft = true;
    this.tid=null;
    
    this.imgs.css({
        width: $('.banner .frame').width()
    });
    $(this.box).css({
        width: this.imgs.width() * this.imgs.length
    });
    
    this.initSwitch = function () {
        var that=this;
        this.box.addEventListener('touchstart', function (e) {
            e.preventDefault();
            that.stopTimer();
            this.className = 'box';
            var touch = e.targetTouches[0];
            that.posX = parseFloat(window.getComputedStyle(this).left);
            that.startX = touch.clientX;
            that.startTime = Date.now();
        }, false);
        this.box.addEventListener('touchmove', function (e) {
            e.preventDefault();
            var touch = e.targetTouches[0];
            var len = that.imgs.length;
            that.endX = touch.clientX;
            if ((that.curIndex <= 0 && that.endX > that.startX) || (that.curIndex >= len - 1 && that.endX < that.startX)) return;
            this.style.left = (that.posX + that.endX - that.startX) + 'px';
        }, false);
        this.box.addEventListener('touchend', function (e) {
            e.preventDefault();
            that.timer();
            var imgWidth = that.imgs.eq(0).width();
            var len = that.imgs.length;
            var countTime = Date.now() - that.startTime;
            if (that.startX > that.endX) {
                if (that.curIndex >= len - 1) {
                    return;
                } else {
                    that.curIndex = (that.startX - that.endX >= imgWidth / 2) || ((that.startX - that.endX >= imgWidth / 4) && countTime <= 200) ? that.curIndex + 1 : that.curIndex;
                }
            } else if (that.startX < that.endX) {
                if (that.curIndex <= 0) {
                    return;
                } else {
                    that.curIndex = (that.endX - that.startX >= imgWidth / 2) || ((that.endX - that.startX >= imgWidth / 4) && countTime <= 200) ? that.curIndex - 1 : that.curIndex;
                }
            }
            this.className = 'box trans';
            this.style.left = -that.curIndex * imgWidth + 'px';
            that.scope.find('.tag span').removeClass('current').eq(that.curIndex).addClass('current');
        }, false);
    };
    this.autoSwitch = function () {
        var len = this.imgs.length;
        var imgWidth = this.imgs.eq(0).width();
        if (this.bLeft) {
            if (this.curIndex >= len - 1) {
                this.curIndex = this.curIndex - 1;
                this.bLeft = false;
            } else {
                this.curIndex = this.curIndex + 1;
            }
        } else {
            if (this.curIndex <= 0) {
                this.curIndex = this.curIndex + 1;
                this.bLeft = true;
            } else {
                this.curIndex = this.curIndex - 1;
            }
        }
        this.box.className = 'box trans';
        this.box.style.left = -this.curIndex * imgWidth + 'px';
        this.scope.find('.tag span').removeClass('current').eq(this.curIndex).addClass('current');
    };
    this.timer = function () {
        this.tid = setInterval(this.autoSwitch.bind(this), 3000);
    };
    this.stopTimer = function () {
        clearInterval(this.tid);
        this.tid = 0;
    };
    this.initSwitch();
    this.timer();
}
function godown() {
    location.href = 'http://www.cn-qch.com/app';
}
function initpage() {
    var banner=new Banner('.banner');
    $('.recommendbox li>img').css({
        height: $('.recommendbox li>img').width()*2/3
    });
    $('.recommendbox li').each(function(){
        this.addEventListener('click', function (e) {
            location.href='/h5/shareCourse.html'+'?guid='+$(this).data('guid');
        })
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
    day = day > 9 ? day : '0' + day;
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
// 图片懒加载 对有lazy类的图片进行懒加载
$(window).bind('scroll', lazy_img);
function lazy_img() {
    var len = $('img.lazy').length;
    !len && $(window).unbind('scroll', lazy_img);
    $('img.lazy').each(function () {
        var oTop = this.offsetTop;
        var sTop = $(document).scrollTop();
        var winH = $(window).height();
        if (sTop + winH >= oTop) {
            $(this).attr('src', $(this).data('src')).removeClass('lazy');
            this.removeAttribute('data-src');
        }
    })
}