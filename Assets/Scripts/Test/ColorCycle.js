var colorStart : Color;
var colorEnd : Color;
var duration : float = 0.01;

private var color1R : float = 1;
private var color1G : float = 1;
private var color1B : float = 1;
private var color2R : float = 0.4;
private var color2G : float = 0.6;
private var color2B : float = 0.8;

private var color2Rping : boolean = false;
private var color2Gping : boolean = false;
private var color2Bping : boolean = false;

function Update () {
   colorStart = Color(color1R, color1G, color1B, 0.25);
    var lerp : float = Mathf.PingPong (Time.time, duration) / duration;
 	if(lerp>=0.90)
 	{
   colorEnd = Color(color2R, color2G, color2B, 0.75);
   
 		if(color2Rping == true)
 		{
 		color2R += 0.025;
 			if(color2R>=1)
 			{
 			color2Rping = false;
 			}
 		}else{
 		color2R -= 0.0075;
 		 	if(color2R<=0)
 			{
 			color2Rping = true;
 			}	
 		}
 		if(color2Gping == true)
 		{
 		color2G += 0.005;
 			if(color2G>=1)
 			{
 			color2Gping = false;
 			}
 		}else{
 		color2G -= 0.02;
 		 	if(color2G<=0)
 			{
 			color2Gping = true;
 			}	
 		}
 	
 		if(color2Bping == true)
 		{
 		color2B += 0.0025;
 			if(color2B>=1)
 			{
 			color2Bping = false;
 			}
 		}else{
 		color2B -= 0.015;
 		 	if(color2B<=0)
 			{
 			color2Bping = true;
 			}	
 		}
 	}
    renderer.material.color = Color.Lerp (colorStart, colorEnd, 2);
}