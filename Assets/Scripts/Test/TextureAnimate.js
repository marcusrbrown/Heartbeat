    var frames : Texture[];
    var framesPerSecond: float = 10;
    var framesSlow: float = 15;
    var framesNormal: float = 22;
    var framesFast : float = 29;
    var sizeSmall : float;
    var sizeNormal : float;
    var sizeLarge : float;
    
    private var currentFrames : float = framesNormal;
    private var currentSize : float = sizeNormal;
     
    function Update() {
        var index : int = (Time.time * currentFrames) % frames.Length;
        renderer.material.mainTexture = frames[index];
        //need to stretch the z of this plane over time to equal currentsize whether that be lower or higher
    }
    
function HeartSlow()
{
currentFrames = framesSlow;
currentSize = sizeSmall;
}

function HeartNormal()
{
currentFrames = framesNormal;
currentSize = sizeNormal;
}

function HeartFast()
{
currentFrames = framesFast;
currentSize = sizeLarge;
}