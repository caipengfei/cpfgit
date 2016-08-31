function dom(id){
	return document.getElementById(id);
}
window.onload=function(){
	var oBigImg=document.getElementById("bigImg");
	var oSmallImg=document.getElementById("smallImg");
	var aBigLi=oBigImg.getElementsByTagName("li");
	var aSmallLi=oSmallImg.getElementsByTagName("li");
	var iNow=0;
	for(var i=0;i<aSmallLi.length;i++){
		aSmallLi[i].index=i;
		aSmallLi[i].onclick=function(){
			iNow=this.index;
			tabImg();
		}
		aSmallLi[i].onmouseover=function(){
			iNow=this.index;
			tabImg();
		}
		function tabImg(){
			for(var i=0;i<aBigLi.length;i++){
				aBigLi[i].className="";
				aSmallLi[i].className="";
			}
			aBigLi[iNow].className="active";
			aSmallLi[iNow].className="active";
		}		
	}
	dom("shenqing").onmouseover=function(){
		dom("cover").style.opacity=0.2;
		dom("cover").style.filter="alpha(opacity:"+20+")";
	}
	dom("shenqing").onmouseout=function(){
		dom("cover").style.opacity=0;
		dom("cover").style.filter="alpha(opacity:"+0+")";
	}
}