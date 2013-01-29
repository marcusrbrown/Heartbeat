public var scoreText : int = 1;

function yay() 
{
<<<<<<< HEAD
scoreText+=Random.Range(60, 80);
=======
scoreText+=Random.Range(60,80);
>>>>>>> Checked in score thing
}

function Update()
{
this.guiText.text = scoreText.ToString();
}