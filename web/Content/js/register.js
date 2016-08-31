function reLayout(){
    var conHeight=$('.container').height();
    var winHeight=$(window).height();
    var headHeight=$('.header').height();
    var conTop=(winHeight>=conHeight+headHeight*2+20)?(winHeight-conHeight)/2-headHeight:10;
    $('body').css({height:winHeight});
    $('.container').css({marginTop:conTop});
}
$(function(){
    $('.container').removeClass('hide');
    $(document).on('click','a.back',function(){ history.back(); })
})
$(window).bind('resize',reLayout).trigger('resize');
var regPhone = /^([1]((3|5|7|8)[0-9]{1})[0-9]{8})$/;
var regPassword = /^(\w){6,16}$/;
$(function() {
    $('.btn-submit').click(signUpHandler);
    $('.phone,.validcode').keyup(banNaN);
    $('.eye').click(function(){
        $(this).toggleClass('visible');
        if($(this).hasClass('visible')){
            $(this).siblings('.password').attr('type','text');
        }else{
            $(this).siblings('.password').attr('type','password');
        }
    })
    $('.btn-getcode.enabled').click(getvalidcode);
})
// 表单提交
function signUpHandler() {
    var $phone = $('.phone'); 
    var $password = $('.password'); 
    var $validcode = $('.validcode'); 
    var $imgvalidcode=$('.imgvalidcode');
    if (!validField($phone, regPhone, '请输入手机号！', '手机号格式有误')) return;
    if (!validField($validcode, null, '请输入验证码！')) return;
    if (!validField($password, null, '密码不能为空！')) return;
    if (!validField($imgvalidcode, null, '请输入图片验证码！')) return;
    if($password.val().length<6){
        $('.help').text('密码至少6位数').removeClass('hidden');
        return;
    }
    if($password.val().length>16){
        $('.help').text('密码长度超过限制').removeClass('hidden');
        return;
    }
    var data = {
        phone: $phone.val(),
        password: $password.val(),
        validcode: $validcode.val(),
        imgvalidcode: $imgvalidcode.val()
    };
    console.log(data);
}
// 验证文本框
function validField($input, reg, helpText1, helpText2) {
    var value = $.trim($input[0].value);
    if (!value) {
        $('.help').text(helpText1).removeClass('hidden');
        $input[0].value = '';
        return false;
    } else {
        if (reg && !reg.test(value)) {
            $('.help').text(helpText2).removeClass('hidden');
            return false;
        }
    }
    $('.help').text('').addClass('hidden');
    return true;
}
// 禁止输入非数值
function banNaN() {
    $(this).val($(this).val().replace(/[^(\d)]/g, ''));
}
// 获取验证码
var waitTime;
function getvalidcode() {
    if (window.td) return;
    if (!validField($('.phone'), regPhone, '请输入手机号！', '手机号格式有误')) return;
    $(this).removeClass('enabled');
    var that = this;
    waitTime = 60;
    sendsms($('.phone').val());
    $(this).text(waitTime + 's后重新获取');
    window.td = setInterval(function() {
        waitTime--;
        if (waitTime == 0) {
            clearInterval(window.td);
            window.td = 0;
            $(that).addClass('enabled').text('发送验证码');
            return;
        }
        $(that).text(waitTime + 's后重新获取');
    }, 1000)
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