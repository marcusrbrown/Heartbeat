public var scoreText : int = 1;

function yay() 
{
scoreText+=Random.Range(60, 80);
}

function Update()
{
this.guiText.text = scoreText.ToString();
}