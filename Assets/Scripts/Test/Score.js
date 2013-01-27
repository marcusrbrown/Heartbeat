public var scoreText : int = 1;

function yay() 
{
scoreText+=1;
}

function Update()
{
this.guiText.text = scoreText.ToString();
}