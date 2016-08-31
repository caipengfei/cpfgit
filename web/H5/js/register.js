var regPhone = /^([1]((3|5|7|8)[0-9]{1})[0-9]{8})$/;
var regPassword=/^[0-9a-zA-Z]{6,16}$/
$(function () {
    $('body').css({
        height: $(window).height()
    });
    //$('.btn-submit').click(signUpHandler);
    $('.getvalidcode.enabled').click(getvalidcode);
    $('.phone,.validcode').keyup(banNaN);
})

// 验证文本框
function validField($input, reg, helpText1, helpText2) {
    var value = $.trim($input[0].value);
    if (!value) {
        util.msgBox(helpText1);
        $input[0].value='';
        return false;
    } else {
        if (reg && !reg.test(value)) {
            util.msgBox(helpText2);
            return false;
        }
    }
    return true;
}
// 显示帮助信息
function tipInfo(t) {
    $('p.tipinfo').remove();
    if (window.tid) {
        clearTimeout(window.tid);
        window.tid = 0;
    }
    var winWidth = $(window).width();
    var $tipinfo = $('<p class="tipinfo">').text(t).css({
        position: 'fixed',
        textAlign: 'center',
        fontSize: '1rem',
        color: '#fff',
        borderRadius: '.5rem',
        background: 'rgba(0,0,0,.8)',
        padding: '.6rem 1rem',
        transform: 'translate3d(0,0,0)'
    }).appendTo('body').css({
        left: (winWidth - $('p.tipinfo')[0].offsetWidth) / 2,
        top: '2rem'
    });
    window.tid = setTimeout(function () {
        $tipinfo.css({ top: '3rem', opacity: 0, transition: 'top .6s,opacity .6s' });
        setTimeout(function () { $tipinfo.remove(); }, 600);
    }, 1200)
}

// 表单提交
function signUpHandler() {
    $('input').blur();
    var $phone = $('.phone-1'); // 用户手机号
    var $reference = $('.phone-2'); // 推荐人号码
    var $password = $('.password'); // 密码
    var $validcode = $('.validcode'); // 验证码
    var bReference=($reference.val().trim()&&validField($reference, regPhone, '', '推荐人手机号格式不正确！'))||true;
    
    // 用户手机号验证
    if(!validField($phone, regPhone, '手机号不能为空！', '用户手机号格式不正确！')) return;
    // 密码验证
    if(!validField($password, regPassword, '密码不能为空！', '密码格式不正确！')) return;
    // 验证码验证
    if(!validField($validcode, null, '验证码不能为空！')) return;
    // 推荐人验证
    if(!bReference) return;
    
    // 验证通过...
    var data = {
        phone: $phone.val(),
        password:$password.val(),
        code: $validcode.val(),
        reference:$reference.val()
    };
}
//发送验证码
function sendsms(phone) {
    if (phone == "") {
        util.msgBox("手机号不能为空");
        return false;
    }
    $.post('/wxUser/CheckPhone', { Phone: phone }, function (msg) {
        if (msg.type == "error") {
            util.msgBox("手机号已被注册");
            return false;
        } else {
            $.post('/home/SendSMS', { phone: phone }, function (msg) {
                util.msgBox(msg.Data);
            })
        }
    })
    
}
// 获取验证码
var waitTime;
function getvalidcode() {
    $('input').blur();
    if (window.td) return;
    if (!validField($('.phone-1'), regPhone, '请输入手机号！', '用户手机号格式不正确！')) return;
    $(this).siblings('[type=text]').focus().end().removeClass('enabled');
    var that=this;
    waitTime = 60;
    sendsms($('.phone-1').val());
    window.td = setInterval(function () {
        waitTime--;
        if (waitTime == 0) {
            clearInterval(window.td);
            window.td = 0;
            $(that).addClass('enabled').text('获取验证码');
            return;
        }
        $(that).text(waitTime + 's后重新获取');
    }, 1000)
}
// 禁止输入非数值
function banNaN(){
    $(this).val($(this).val().replace(/[^(\d)]/g, ''));
}