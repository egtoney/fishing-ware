using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PotionPermitMinigame : Minigame
{
	public override string Name => "Potion Fishing";
	public UIDocument fishingUI;
	public Transform fish;
	public float catchSpeed = 1;
	public float healthLossSpeed = 125;
	public float catchPositionSuccess = -15;
	public float catchPositionFailure = 4;
	private VisualElement healthBar;
	public GameObject madSprite;
	public GameObject happySprite;

	private float catchPosition = 0;
	private float nextInputTimer = 0;
	private bool fishHappy = false;
	private float health = 250;

	void Start()
    {
		var root = fishingUI.rootVisualElement;

		healthBar = root.Query("health-bar").First();
    }

	protected override void UpdateMinigame()
	{
		//// check if time to prompt user for input
		nextInputTimer -= Time.deltaTime;

		if (nextInputTimer <= 0) {
			fishHappy = !fishHappy;

			nextInputTimer = fishHappy
				? Random.Range(2, 3.5f)
				: Random.Range(.5f, 1.5f);

			madSprite.SetActive(fishHappy == false);
			happySprite.SetActive(fishHappy == true);
		}

		var pulling = Input.GetKey(KeyCode.Space);

		//// lose health if pulling and fish is not happy
		if (pulling == true && fishHappy == false) {
			health -= healthLossSpeed * Time.deltaTime;
		} else {
			health += .1f * healthLossSpeed * Time.deltaTime;
		}

		// clip health
		health = Mathf.Clamp(health, 0, 250);

		//// can only pull if fish is happy
		if (pulling == true && fishHappy == true) {
			catchPosition -= catchSpeed * Time.deltaTime;
		}

		// fish swims away when mad
		if (fishHappy == false) {
			catchPosition += .5f * catchSpeed * Time.deltaTime;
		}

		healthBar.style.width = health;
		fish.localPosition = new Vector3(catchPosition, fish.localPosition.y, fish.localPosition.z);

		//// check for end states
		if (health == 0) {
			State = MinigameState.Failure;
		}

		if (catchPosition <= catchPositionSuccess) {
			State = MinigameState.Success;
		}

		if (catchPosition >= catchPositionFailure) {
			State = MinigameState.Failure;
		}
	}
}
