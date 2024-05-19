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
	public float madSpeed = 1;
	public float healthLossSpeed = 125;
	public float healthGainSpeed = 13;
	public float catchPositionSuccess = -15;
	public float catchPositionFailure = 4;
	public float happyMinDuration = 2f;
	public float happyMaxDuration = 3.5f;
	public float madMinDuration = .5f;
	public float madMaxDuration = 1.5f;
	private VisualElement healthBar;
	public GameObject madSprite;
	public GameObject happySprite;

	private float catchPosition = 0;
	private float nextInputTimer = 0;
	private bool fishHappy = false;
	private float health = 300;

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
				? Random.Range(happyMinDuration, happyMaxDuration)
				: Random.Range(madMinDuration, madMaxDuration);

			madSprite.SetActive(fishHappy == false);
			happySprite.SetActive(fishHappy == true);
		}

		var pulling = Input.GetKey(KeyCode.Space);

		madSprite.GetComponent<SpriteRenderer>().color = Color.white;

		//// lose health if pulling and fish is not happy
		if (pulling == true && fishHappy == false) {
			health -= healthLossSpeed * Time.deltaTime;
			madSprite.GetComponent<SpriteRenderer>().color = Color.red;
		} else {
			health += healthGainSpeed * Time.deltaTime;
		}

		// clip health
		health = Mathf.Clamp(health, 0, 300);

		//// can only pull if fish is happy
		if (pulling == true && fishHappy == true) {
			catchPosition -= catchSpeed * Time.deltaTime;
		} else {
			catchPosition += madSpeed * Time.deltaTime;
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
