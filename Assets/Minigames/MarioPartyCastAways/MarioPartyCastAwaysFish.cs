using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioPartyCastAwaysFish : MonoBehaviour
{
	public bool up = true;
	public float speed = 1;
	public float removeAtY = 3;

	void Start()
	{
		if (up == false) {
			var diff = removeAtY - gameObject.transform.position.y;

			gameObject.transform.localPosition = new Vector3(
				gameObject.transform.localPosition.x,
				gameObject.transform.localPosition.y + diff,
				gameObject.transform.localPosition.z
			);

			speed *= -1;
			removeAtY -= diff;
		}
	}

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = new Vector3(
			gameObject.transform.localPosition.x,
			gameObject.transform.localPosition.y + speed * Time.deltaTime,
			gameObject.transform.localPosition.z
		);

		if (up == false) {
			GetComponent<SpriteRenderer>().flipY = true;
		}

		if (
			(up == true && gameObject.transform.position.y >= removeAtY) ||
			(up == false && gameObject.transform.position.y <= removeAtY)
		) {
			Destroy(gameObject);
		}
    }
}
