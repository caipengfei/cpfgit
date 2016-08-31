		function getStyle(obj,attr){
			if(obj.currentStyle){
				return obj.currentStyle[attr];
			}
			else{
				return getComputedStyle(obj,false)[attr];
			}
		}
		function startMove(obj,json,fn){
			clearInterval(obj.timer);
			obj.timer=setInterval(function(){		
				var oBut=true;	
				for(var attr in json){
					if(attr=="opacity"){
						var iCur=parseFloat(getStyle(obj,attr))*100;
					}
					else{
						 iCur=parseInt(getStyle(obj,attr));
					}				
					var iSpeed=(json[attr]-iCur)/8;
					iSpeed=iSpeed>0?Math.ceil(iSpeed):Math.floor(iSpeed);
					if(iCur!=json[attr])
					{
						oBut=false;
					}
					if(attr=="opacity"){
						obj.style[attr]="alpha(opacity:"+(iCur+iSpeed)+")";
						obj.style[attr]=(iCur+iSpeed)/100;
					}
					else{
						obj.style[attr]=iCur+iSpeed+'px';
					}
				}
				if(oBut){
						clearInterval(obj.timer);
						if(fn)
						{
							fn();
						}
					}
				
			},30);		
		}