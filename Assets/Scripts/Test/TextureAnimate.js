    var frames : Texture[];
    var framesPerSecond: float = 10;
    
   //Used for faint heart rate
    var framesSlow: float = 15;
    var yStretchSlow : float;
    var yOffsetSlow : float;

    //Used for normal heart rate
    var framesNormal: float = 22;
    var yStretchNormal : float;
    var yOffsetNormal : float;

    
    //Used for fast heart rate
    var framesFast : float = 29;
    var yStretchFast : float;
    var yOffsetFast : float;
    
    //Used for my start screen hack code. Delete this once we have a real UI.
    var titleScreen : boolean = false;
   var stopAnimatingVar : boolean = false;
   
   
   //These variables allow the thing to change in real time.
   private var currentFrames : float;
    private var currentYStretch : float;
    private var currentYOffset : float;
  
    function Start(){  
	currentFrames = framesNormal;
	currentYStretch = yStretchNormal;
	currentYOffset = yOffsetNormal;
     }
     
    function Update() {
        if(titleScreen)
        {

        //the hackiest hack that ever hacked (used for the title screen)
   			if(stopAnimatingVar==false)
   			{
           var index2 : int = (Time.time * framesPerSecond) % frames.Length;
           if (this.guiTexture)
            this.guiTexture.texture = frames[index2];
            else
            renderer.material.mainTexture = frames[index2];
            if(index2==1)
            {
            stopAnimatingVar=true;
            }
            }
        }else{
        
        //this is where all the magic is. The hackiest hack that ever hacked was to reuse this script for the
        //title screen in the worst way ever. Seriously. 
        
        var index : int = (Time.time * currentFrames) % frames.Length;
        if (this.guiTexture)
        this.guiTexture.texture = frames[index];
        else
        renderer.material.mainTexture = frames[index];
        
        //Code needs to be here. Lerp between the Pixel inset Y value of the GUITexture and 
        //private variabe currentYOffet.
        
        //At the same time you need to lerp between the pixel inset height value of the GUITexture and
        //private variable currentYStretch.
        
        //Then, as long as you send this the messages "HeartSlow", "HeartNormal", and "HeartFast" it
        //should just work on its own.
    }
    }
    
function HeartSlow()
{
currentFrames = framesSlow;
currentYStretch = yStretchSlow;
currentYOffset = yOffsetSlow;
}

function HeartNormal()
{
currentFrames = framesNormal;
currentYStretch = yStretchNormal;
currentYOffset = yOffsetNormal;
}

function HeartFast()
{
currentFrames = framesFast;
currentYStretch = yStretchFast;
currentYOffset = yOffsetFast;
}