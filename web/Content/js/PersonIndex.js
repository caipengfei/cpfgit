function row1(){
	var oBanner=document.getElementsByClassName("banner")[0];
	var aLi=oBanner.getElementsByTagName("li");
	var oControl=document.getElementsByClassName("control")[0];
	var oPre=oControl.getElementsByClassName("pre")[0];
	var oNext=oControl.getElementsByClassName("next")[0];
	var oControl2=document.getElementById("control_2");
	var aA2=oControl2.getElementsByTagName("a");
	var iNow=0;
	for(var i=0;i<aA2.length;i++){
		aA2[i].index=i;
		aA2[i].onclick=function(){			
			iNow=this.index;		
			tabImg();
		}
	}
	oPre.onclick=function(){
		iNow--;
		tabImg();
	}
	oNext.onclick=function(){
		iNow++;
		tabImg();
	}
	setInterval(function(){
		iNow++;
		tabImg();
	},3000);
	function tabImg(){
		if(iNow<0){
			iNow=aLi.length-1;
		}
		else if(iNow>aLi.length-1){
			iNow=0;
		}
		for(var i=0;i<aA2.length;i++){
			aA2[i].className="";
		}
		aA2[iNow].className="current";
		for(var i=0;i<aLi.length;i++){
			aLi[i].className="";
		}
		aLi[iNow].className="current";
	}	
}
row1();
function row5(){
	var row5=document.getElementsByClassName("row5")[0];
			var row5Box=row5.getElementsByClassName("box")[0];
			var aSpan=row5Box.getElementsByTagName("span");
			var num=0;
			var num1=0;
			var num2=-100;
			setInterval(function(){
				if(num==0){
					aSpan[0].className="position0";
					aSpan[1].className="position1";
					aSpan[2].className="position2";
					aSpan[3].className="position3";
					aSpan[4].className="position4";
					if(num1==0){
						document.getElementsByClassName("position4")[0].style.zIndex=num2;
							num1=1;
					}
					else{
						document.getElementsByClassName("position4")[0].style.zIndex=num2;
							num1=0;
						//
						

					}
					
					startMove(aSpan[0],{left:0});
					startMove(aSpan[1],{left:200});
					startMove(aSpan[2],{left:450});
					startMove(aSpan[3],{left:750});
					startMove(aSpan[4],{left:1000});
				}
				else if(num==1){
					aSpan[0].className="position4";
					aSpan[1].className="position0";
					
					aSpan[2].className="position1";
					aSpan[3].className="position2";
					aSpan[4].className="position3";
					document.getElementsByClassName("position4")[0].style.zIndex=num2;
					startMove(aSpan[0],{left:1000});
					startMove(aSpan[1],{left:0});
					startMove(aSpan[2],{left:200});
					startMove(aSpan[3],{left:450});
					startMove(aSpan[4],{left:750});
				}
				else if(num==2){
					aSpan[0].className="position3";
					aSpan[1].className="position4";
					aSpan[2].className="position0";
					aSpan[3].className="position1";
					aSpan[4].className="position2";
					document.getElementsByClassName("position4")[0].style.zIndex=num2;
					startMove(aSpan[0],{left:750});
					startMove(aSpan[1],{left:1000});
					startMove(aSpan[2],{left:0});
					startMove(aSpan[3],{left:200});
					startMove(aSpan[4],{left:450});
				}
				else if(num==3){
					aSpan[0].className="position2";
					aSpan[1].className="position3";
					aSpan[2].className="position4";
					aSpan[3].className="position0";
					aSpan[4].className="position1";
			document.getElementsByClassName("position4")[0].style.zIndex=num2;
					startMove(aSpan[0],{left:450});
					startMove(aSpan[1],{left:750});
					startMove(aSpan[2],{left:1000});
					startMove(aSpan[3],{left:0});
					startMove(aSpan[4],{left:200});
				}
				else if(num==4){
					aSpan[0].className="position1";
					aSpan[1].className="position2";
					aSpan[2].className="position3";
					aSpan[3].className="position4";
					aSpan[4].className="position0";
					document.getElementsByClassName("position4")[0].style.zIndex=num2;
					startMove(aSpan[0],{left:200});
					startMove(aSpan[1],{left:450});
					startMove(aSpan[2],{left:750});
					startMove(aSpan[3],{left:1000});
					startMove(aSpan[4],{left:0});

				}
			num++;
			num2--;
			if(num==5){
				num=0;
			}
			},2000);
				
}
row5();
function row6(){
	var row6=document.getElementsByClassName('row6')[0];
	var aCompany=row6.getElementsByClassName("company");
	for(var i=0;i<aCompany.length;i++){
		aCompany[i].index=i;
		aCompany[i].onmouseover=function(){
			var img1=aCompany[this.index].getElementsByTagName("img")[0];
			var img2=aCompany[this.index].getElementsByTagName("img")[1];
			img1.style.display="none";
			img2.style.display="block";
			aCompany[this.index].style.boxShadow="0 0 10px #ccc";
		}
		aCompany[i].onmouseout=function(){
			var img1=aCompany[this.index].getElementsByTagName("img")[0];
			var img2=aCompany[this.index].getElementsByTagName("img")[1];
			img1.style.display="block";
			img2.style.display="none";
			aCompany[this.index].style.boxShadow="";

		}
	}
}
row6();
