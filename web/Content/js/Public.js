/*---返回到顶部----*/
function getScrollTop(){   
    var scrollTop=0;   
    if(document.documentElement&&document.documentElement.scrollTop){   
        scrollTop=document.documentElement.scrollTop;   
    }else if(document.body){   
        scrollTop=document.body.scrollTop;   
    }   
    return scrollTop;   
}
function backTop(){
	var topImg=document.getElementById("backTop");
	var scrollTop=getScrollTop();
	scrollTop=parseInt(scrollTop);
	if(scrollTop>200){
		topImg.style.display="block";
	}
	else{
		topImg.style.display="none";
	}
	topImg.onmouseover=function(){
		topImg.getElementsByTagName("img")[0].src="Content/image/ding2.png";
	}
	topImg.onmouseout=function(){
		topImg.getElementsByTagName("img")[0].src="Content/image/ding.png";
	}
	topImg.onclick=function(){
		 document.documentElement.scrollTop = document.body.scrollTop =0;
	}
}
backTop();
window.onscroll=function(){
	backTop();
}
/*---返回到顶部结束----*/

/*--个人---*/
function geren(){
	var oGrenNoLogin=document.getElementsByClassName("headerRightNoLogin")[0];
	var oGrenLogin=document.getElementsByClassName("headerRightLogin")[0];
	var oImg=oGrenNoLogin.getElementsByTagName("img")[1];
	var oImg1=oGrenLogin.getElementsByTagName("img")[1];
	oGrenNoLogin.onmouseover=function(){
		oImg.src = "/Content/image/geren.jpg";
	}
	oGrenNoLogin.onmouseout=function(){
		oImg.src = "/Content/image/geren1.png";
	}
	oGrenLogin.onmouseover=function(){
		oImg1.src = "/Content/image/geren.jpg";
	}
	oGrenLogin.onmouseout=function(){
		oImg1.src = "/Content/image/geren1.png";
	}
}
geren();