window.onload=function(){
	var aName=document.getElementsByClassName("name");
	var now=0;
	for(var i=0;i<aName.length;i++){
		if(aName[i].innerHTML.length>6){
			aName[i].innerHTML=aName[i].innerHTML.substring(0,6);
		}
	}
}