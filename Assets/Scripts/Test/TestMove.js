var animTime : float = 1;
var speedVar : float = 1;
var delayTime : float = 1;
var animPause : float = 1;
var flip : boolean = true;
private var localTime = 0.0;
private var delayOver : boolean = true;
private var initialDelay : boolean = true;


function Update () {

if(initialDelay)
{
	    localTime += Time.deltaTime;
	    if(localTime>=delayTime)
	    {
	    initialDelay=false;
	    }
}

if(delayOver==true && initialDelay==false){
	    localTime += Time.deltaTime;
	    if(flip==false)
	    {
	    transform.Rotate(0, speedVar, 0);
	    if(localTime>animTime)
		    {
		    localTime=0;
		    delayOver=false;
		    Delay();
		    }
		}
		 if(flip==true)
	    {
	    transform.Rotate(0, -speedVar, 0);
	    if(localTime>animTime)
		    {
		    localTime=0;
		    delayOver=false;
		    Delay();
		    }
		}
	}	    
}


function Delay () {
			yield WaitForSeconds(animPause);
			delayOver=true;
			if(flip)
			{
			flip=false;
			}else{
			flip=true;
			}
}