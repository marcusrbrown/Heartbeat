#pragma strict

var explodeCheck : boolean = false;
private var x : float;
private var y : float;
private var z : float;

function Explode()
{
print("test");
x=Random.Range(-0.5, 0.5);
y=Random.Range(-0.5, 0.5);
z=Random.Range(-0.5, 0.5);
explodeCheck=true;
}

function Update() {
if (explodeCheck==true)
	{
	print("I exploded");
    transform.Translate(x,y,z);
    }
}

