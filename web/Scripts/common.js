var defaultValidateErrorPlacement = function (lable, element)
{
    if (element.hasClass("l-textarea")) element.ligerTip({ content: lable.html(), appendIdTo: lable });
    else if (element.hasClass("l-text-field")) element.parent().ligerTip({ content: lable.html(), appendIdTo: lable });
    else lable.appendTo(element.parents("td:first").next("td"));
};

var defaultValidateSuccess = function (lable)
{
    lable.ligerHideTip();
};

var deafultValidate = function (validateElements)
{
    return  validateElements.validate({
        errorPlacement: function (lable, element)
        {
            defaultValidateErrorPlacement(lable, element);
        },
        success: function (lable)
        {
            defaultValidateSuccess(lable);
        }
    });
};
$(function ()
{
    if (jQuery.validator)
    {
        //字母数字
        jQuery.validator.addMethod("alnum", function (value, element)
        {
            return this.optional(element) || /^[a-zA-Z0-9]+$/.test(value);
        }, "只能包括英文字母和数字");

        // 手机号码验证   
        jQuery.validator.addMethod("cellphone", function (value, element)
        {
            var length = value.length;
            return this.optional(element) || (length == 11 && /^(1\d{10})$/.test(value));
        }, "请正确填写手机号码");

        // 电话号码验证   
        jQuery.validator.addMethod("telephone", function (value, element)
        {
            var tel = /^(\d{3,4}-?)?\d{7,9}$/g;
            return this.optional(element) || (tel.test(value));
        }, "请正确填写电话号码");

        // 邮政编码验证
        jQuery.validator.addMethod("zipcode", function (value, element)
        {
            var tel = /^[0-9]{6}$/;
            return this.optional(element) || (tel.test(value));
        }, "请正确填写邮政编码");

        // 汉字
        jQuery.validator.addMethod("chcharacter", function (value, element)
        {
            var tel = /^[\u4e00-\u9fa5]+$/;
            return this.optional(element) || (tel.test(value));
        }, "请输入汉字");

        // 汉字
        jQuery.validator.addMethod("qq", function (value, element)
        {
            var tel = /^[1-9][0-9]{4,}$/;
            return this.optional(element) || (tel.test(value));
        }, "请输入正确的QQ");

        // 用户名
        jQuery.validator.addMethod("username", function (value, element)
        { 
            return this.optional(element) || /^[a-zA-Z][a-zA-Z0-9_]+$/.test(value);
        }, "用户名格式不正确");
    }
});


var GetUrlParam = function(name)
{
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
var SetButtons = function (toolbar ,url)
{
    if (!url)
    { 
        url = '../service/SystemData.ashx?Action=GetButton';
        url += '&MenuNo=' + GetUrlParam('MenuNo');
    }
    url += "&rnd" + Math.random(); 
    $.getJSON(url, function (data)
    {
        if (!data) return;
        var buttons = []; 
        $(data).each(function (i, dataitem)
        {
            var btn = { text: this.name, icon: this.icon, id: this.id };
            if (f_btnClick) btn.click = f_btnClick;
            buttons.push(btn);
        });
        toolbar.ligerToolBar({ items: buttons });
    });
};
/**
*textarea文本框输入字数检测
*textareaId：textarea的dom标识
*maxLen：要求的最大字节长度
*/
function chkTextareaLen(textareaId, counterId, maxLen) {
    try {
        var textareaValue = document.getElementById(textareaId).value;
        var curLen = 0, substrLen = 0;

        for (var i = 0; i < textareaValue.length; i++) {
            if (textareaValue.charCodeAt(i) > 127 || textareaValue.charCodeAt(i) == 94) {
                curLen += 2;
            } else {
                curLen++;
            }

            if (curLen > maxLen) {
                substrLen = i;
                break;
            }
        }

        if (curLen > maxLen) {
            if (substrLen == 0) substrLen = maxLen;
            document.getElementById(textareaId).value = textareaValue.substring(0, substrLen);
            alert("文本长度不能大于" + maxLen + "个字节(中文占2个字节)");
        } else {
            document.getElementById(counterId).innerHTML = maxLen - curLen;
        }
    } catch (e) { }
}

//获取指定的URL参数值
//URL:http://www.blogjava.net/blog?name=bainian
//参数：paramName URL参数
//调用方法:getParam("name")
//返回值:bainian

function getParam(paramName) {
    paramValue = "";
    isFound = false;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        arrSource = unescape(this.location.search).substring(1, this.location.search.length).split("&");
        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].split("=")[1];
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;
}
function GetDate(date) {
    if (date == null)
        return null;
    var index = 0;
    if (date < 0)
        index = date.indexOf('(');
    else
        index = date.indexOf('(') + 1;

    var endIndex = date.indexOf(')');

    var dat = date.substring(index, endIndex) - 1 + 1;
    var dateTime = new Date(dat);
    var year = dateTime.getFullYear();
    var month = dateTime.getMonth() + 1;
    var day = dateTime.getDate();

    if (month < 10) {
        month = "0" + month;
    }
    if (day < 10) {
        day = "0" + day;
    }
    return year + "-" + month + "-" + day;
}
function GetDateTime(date) {
    if (date == null)
        return null;
    var index = 0;
    if (date < 0)
        index = date.indexOf('(');
    else
        index = date.indexOf('(') + 1;

    var endIndex = date.indexOf(')');

    var dat = date.substring(index, endIndex) - 1 + 1;
    var dateTime = new Date(dat);
    var year = dateTime.getFullYear();
    var month = dateTime.getMonth() + 1;
    var day = dateTime.getDate();
    var h = dateTime.getHours();
    var m = dateTime.getMinutes();
    var s = dateTime.getSeconds();

    if (month < 10) {
        month = "0" + month;
    }
    if (day < 10) {
        day = "0" + day;
    }
    if (h < 10) {
        h = "0" + h;
    }
    if (m < 10) {
        m = "0" + m;
    }
    if (s < 10) {
        s = "0" + s;
    }
    return year + "-" + month + "-" + day + " " + h + ":" + m + ":" + s;
}