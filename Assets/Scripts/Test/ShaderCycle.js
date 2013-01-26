var colorStart : Color = Color(00, 0, 0, 1);
var colorEnd : Color = Color(1, 1, 1, 1);
var duration : float = 1.75;

function Update () {
    var lerp : float = Mathf.PingPong (Time.time, duration) / duration;
    renderer.material.color = Color.Lerp (colorStart, colorEnd, lerp);
}