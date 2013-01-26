var colorStart : Color = Color(0.25, 0.25, 0.25, 1);
var colorEnd : Color = Color(1, 1, 1, 1);
var duration : float = 1.75;

function Update () {
    var lerp : float = Mathf.PingPong (Time.time, duration) / duration;
    renderer.material.color = Color.Lerp (colorStart, colorEnd, lerp);
}