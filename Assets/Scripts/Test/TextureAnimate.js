    var frames : Texture[];
    var framesPerSecond: float = 10;
    var framesSlow: float = 10;
    var framesNormal: float = 10;
     
    function Update() {
        var index : int = (Time.time * framesPerSecond) % frames.Length;
        renderer.material.mainTexture = frames[index];
    }
    
function HeartSlowOn()
{
framesPerSecond = framesSlow;
}

function HeartSlowOff()
{
framesPerSecond = framesNormal;
}