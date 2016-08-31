function dom(id){
	return document.getElementById(id);
}
window.onload=function(){
	var aA=dom("citys").getElementsByTagName("a");
	for(var i=0;i<aA.length;i++){
		aA[i].index=i;
		aA[i].onclick=function(){
			for(var i=0;i<aA.length;i++){
				aA[i].className="";
			}
			aA[this.index].className="active";

		}
	}
	var bDown=true;
	dom("more").onclick=function(){
		if(bDown){
			dom("city").style.height="120px";
			dom("moreImg").src="Content/image/more2.png";
		}
		else{
			dom("city").style.height="40px";
			dom("city").style.overflow="hidden";
			dom("moreImg").src="Content/image/more.png";
		}
		bDown=!bDown;				
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