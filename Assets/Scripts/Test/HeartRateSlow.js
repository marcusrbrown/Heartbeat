var shrinking : boolean = false;
var growing : boolean = false;
var shrinkSpeed : float = 1;
var targetScaleShrink : float = 0.75;
var targetScaleGrow : float;

function Update() {

	if (shrinking == true) {
	gameObject.transform.localScale.x -= Time.deltaTime*shrinkSpeed;
	gameObject.transform.localScale.y -= Time.deltaTime*shrinkSpeed;
	gameObject.transform.localScale.z -= Time.deltaTime*shrinkSpeed;
	}

	if (gameObject.transform.localScale.x <= targetScaleShrink)
	{
	gameObject.transform.localScale.x = targetScaleShrink;
	gameObject.transform.localScale.y = targetScaleShrink;
	gameObject.transform.localScale.z = targetScaleShrink;
	shrinking = false;
	}
	
	if (growing == true) {
	gameObject.transform.localScale.x += Time.deltaTime*shrinkSpeed;
	gameObject.transform.localScale.y += Time.deltaTime*shrinkSpeed;
	gameObject.transform.localScale.z += Time.deltaTime*shrinkSpeed;
	}

	if (gameObject.transform.localScale.x >= targetScaleGrow)
	{
	gameObject.transform.localScale.x = targetScaleGrow;
	gameObject.transform.localScale.y = targetScaleGrow;
	gameObject.transform.localScale.z = targetScaleGrow;
	growing = false;
	}
}

function HeartSlowOn()
{
shrinking = true;
growing = false;
}

function HeartSlowOff()
{
shrinking = false;
growing = true;
}