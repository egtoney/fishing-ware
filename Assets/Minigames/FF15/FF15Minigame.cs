using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FF15Minigame : Minigame
{
	public override string Name => "Final Fin";
	public UIDocument fishingUI;
	public Transform fish;
	public float catchSpeed = 1;
	public float healthLossSpeed = 100;
	public float healthGainSpeed = 0;
	public float healthInputPenalty = 15;
	public float catchPositionSuccess = -15;
	private VisualElement healthBar;
	public GameObject button;
	public GameObject letterW;
	public GameObject letterS;
	public GameObject letterA;
	public GameObject letterD;

	private char[] inputOptions = new char[]{'w', 's', 'a', 'd'};

	private float catchPosition = 0;
	private float nextInputTimer = 1;
	private char nextInput = ' ';
	private bool waitingForInput = false;
	private float health = 300;
	private float colorTimer = 0;

	void Start()
    {
		var root = fishingUI.rootVisualElement;

		healthBar = root.Query("health-bar").First();
    }

	protected override void UpdateMinigame()
	{
		//// check if time to prompt user for input
		nextInputTimer -= Time.deltaTime;
		colorTimer -= Time.deltaTime;

		if (colorTimer <= 0) {
			button.GetComponent<SpriteRenderer>().color = Color.white;
		}

		if (waitingForInput == false && nextInputTimer <= 0) {
			int nextInputIndex = Random.Range(0, inputOptions.Length);
			nextInput = inputOptions[nextInputIndex];
			waitingForInput = true;

			button.SetActive(true);
			switch (nextInput) {
				case 'w':
					letterW.SetActive(true);
					break;
				case 's':
					letterS.SetActive(true);
					break;
				case 'a':
					letterA.SetActive(true);
					break;
				case 'd':
					letterD.SetActive(true);
					break;
			}
		}

		if (waitingForInput == true) {
			bool inputSuccess = false;
			bool inputFailure = false;

			bool inputW = Input.GetKeyDown(KeyCode.W);
			bool inputS = Input.GetKeyDown(KeyCode.S);
			bool inputA = Input.GetKeyDown(KeyCode.A);
			bool inputD = Input.GetKeyDown(KeyCode.D);

			switch (nextInput) {
				case 'w':
					inputSuccess = inputW;
					inputFailure = inputS || inputA || inputD;
					break;
				case 's':
					inputSuccess = inputS;
					inputFailure = inputW || inputA || inputD;
					break;
				case 'a':
					inputSuccess = inputA;
					inputFailure = inputW || inputS || inputD;
					break;
				case 'd':
					inputSuccess = inputD;
					inputFailure = inputW || inputS || inputA;
					break;
			}

			if (inputFailure) {
				button.GetComponent<SpriteRenderer>().color = Color.red;
				colorTimer = .25f;
				health -= healthInputPenalty;

			} else if (inputSuccess) {
				nextInputTimer = Random.Range(.5f, 1.5f);
				waitingForInput = false;

				button.SetActive(false);
				letterW.SetActive(false);
				letterS.SetActive(false);
				letterA.SetActive(false);
				letterD.SetActive(false);
			}
		}

		//// update health
		if (waitingForInput == true) {
			health -= healthLossSpeed * Time.deltaTime;
		} else {
			health += healthGainSpeed * Time.deltaTime;
		}

		// clip health
		health = Mathf.Clamp(health, 0, 300);

		//// fish is always moving toward us
		if (waitingForInput == false) {
			catchPosition -= catchSpeed * Time.deltaTime;
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
	}
}
