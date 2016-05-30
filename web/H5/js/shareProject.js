var banner = {
    init: function () {
        this.tid = this.curIndex = 0;
        this.timeout = 600;
        this.parent = document.getElementsByClassName('banner')[0];
        this.controls = this.parent.querySelectorAll('.tag span');
        this.box = this.parent.querySelector('.box');
        this.box.innerHTML += this.box.innerHTML + this.box.innerHTML;
        this.imgs = this.box.querySelectorAll('img');
        var padding = parseFloat(getComputedStyle(this.parent).paddingLeft) + parseFloat(getComputedStyle(this.parent).paddingRight);
        this.frameWidth = Math.ceil(this.parent.offsetWidth - padding);
        this.len = this.imgs.length / 3;
        this.baseLeft = -this.frameWidth * this.len;
        this.box.style.width = this.frameWidth * this.len * 3 + 'px';
        this.box.style.left = this.baseLeft + 'px';
        for (var i = 0; i < this.len * 3; i++) {
            this.imgs[i].style.width = this.frameWidth + 'px';
        }
        if (this.len < 2) return;
        this.box.addEventListener('touchstart', this.onTouchstart, false);
        this.box.addEventListener('touchmove', this.onTouchmove, false);
        this.box.addEventListener('touchend', this.onTouchend, false);
        this.timer();
    },
    onTouchstart: function (e) {
        e.preventDefault();
        banner.stopTimer();
        this.classList.remove('trans');
        if (banner.tid) {
            clearTimeout(banner.tid);
            banner.tid = 0;
            this.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
        }
        var touch = e.targetTouches[0];
        banner.startX = touch.clientX;
        banner.posX = parseFloat(this.style.left);
        banner.startTime = Date.now();
    },
    onTouchmove: function (e) {
        e.preventDefault();
        var touch = e.targetTouches[0];
        banner.endX = touch.clientX;
        this.style.left = banner.posX + banner.endX - banner.startX + 'px';
    },
    onTouchend: function (e) {
        e.preventDefault();
        var step = banner.endX - banner.startX;
        var time = Date.now() - banner.startTime;
        if (step < 0) {
            if ((step <= -banner.frameWidth / 2) || (step <= -banner.frameWidth / 8) && time < 300) {
                banner.curIndex++;
                this.classList.add('trans');
                this.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
                if (banner.curIndex > banner.len - 1) {
                    banner.curIndex = 0;
                    var that = this;
                    banner.tid = setTimeout(function () {
                        that.classList.remove('trans');
                        that.style.left = banner.baseLeft + 'px';
                    }, banner.timeout)
                }
            } else {
                this.classList.add('trans');
                this.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
            }
        } else if (step > 0) {
            if ((step > banner.frameWidth / 2) || (step > banner.frameWidth / 8 && time < 300)) {
                banner.curIndex--;
                this.classList.add('trans');
                this.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
                if (banner.curIndex < 0) {
                    banner.curIndex = banner.len - 1;
                    var that = this;
                    banner.tid = setTimeout(function () {
                        that.classList.remove('trans');
                        that.style.left = banner.baseLeft - (banner.frameWidth * banner.curIndex) + 'px';
                    }, banner.timeout)
                }
            } else {
                this.classList.add('trans');
                this.style.left = banner.baseLeft - banner.frameWidth * banner.curIndex + 'px';
            }
        }
        banner.setTag();
        banner.timer();
    },
    autoSwitch: function () {
        banner.curIndex++;
        this.box.classList.add('trans');
        this.box.style.left = this.baseLeft - this.frameWidth * this.curIndex + 'px';
        if (this.curIndex > this.len - 1) {
            this.curIndex = 0;
            var that = this;
            this.tid = setTimeout(function () {
                that.box.classList.remove('trans');
                that.box.style.left = that.baseLeft + 'px';
            }, this.timeout);
        }
        this.setTag();
    },
    setTag: function () {
        for (var i = 0; i < this.len; i++) {
            if (i == this.curIndex) {
                this.controls[i].classList.add('current');
            } else {
                this.controls[i].classList.remove('current');
            }
        }
    },
    timer: function () {
        this.interval = setInterval(this.autoSwitch.bind(this), 3000);
    },
    stopTimer: function () {
        clearInterval(this.interval);
        this.interval = 0;
    }
}