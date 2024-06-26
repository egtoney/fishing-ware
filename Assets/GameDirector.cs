using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class GameDirector : MonoBehaviour
{
	public bool debugMode;
	public GameObject mainMenuObject;
	public GameObject endGameObject;
	public UIDocument endGameUI;
	public GameObject creditsObject;
	public GameObject staticBobble;
	public UIDocument transitionUI;
	public Animator playerAnimator;
	public List<Minigame> minigames;

	private float inputDelay = 0;
	private bool inEndGame = false;
	private bool inMainMenu = true;
	private int activeMinigameIndex;
	private Minigame activeMinigame;
	private float startMinigameCountdown = 0;
	private float nextMinigameCountdown = 0;
	private int minigameSuccessCount = 0;

	private Label countdownUI;
	private VisualElement countdownWrapperUI;
	private Label minigameNameUI;

	private Label successCountUI;

	private void UpdateUiReferences() {
		var transitionRoot = transitionUI.rootVisualElement;

		if (transitionRoot != null) {
			countdownWrapperUI = transitionRoot.Query("countdown-wrapper").First();
			countdownUI = transitionRoot.Query<Label>("countdown").First();
			minigameNameUI = transitionRoot.Query<Label>("minigame-name").First();
		}

		var endRoot = endGameUI.rootVisualElement;

		if (endRoot != null) {
			successCountUI = endRoot.Query<Label>("success-count").First();
		}
	}

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
		Assert.IsTrue(minigames.Count > 0, "minigames array contains at least one item");

		inputDelay -= Time.deltaTime;

		if (inMainMenu) {
			if (mainMenuObject.activeSelf == false) {
				mainMenuObject.SetActive(true);
				UpdateUiReferences();
			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				inMainMenu = false;
				creditsObject.SetActive(false);
				mainMenuObject.SetActive(false);
				transitionUI.gameObject.SetActive(true);
				UpdateUiReferences();
			}
			return;
		}

		if (inEndGame) {
			if (transitionUI.gameObject.activeSelf == true) {
				transitionUI.gameObject.SetActive(false);
			}
			if (endGameObject.activeSelf == false) {
				endGameObject.SetActive(true);
				UpdateUiReferences();
				successCountUI.text = minigameSuccessCount.ToString();
			}
			if (inputDelay < 0 && Input.GetKeyDown(KeyCode.Space)) {
				inEndGame = false;
				endGameObject.SetActive(false);
				transitionUI.gameObject.SetActive(true);
				UpdateUiReferences();
				minigameSuccessCount = 0;
			}
			return;
		}

		startMinigameCountdown -= Time.deltaTime;
		nextMinigameCountdown -= Time.deltaTime;

		if (
			// no minigame
			activeMinigame == null ||
			// need a new minigame
			(
				activeMinigame.State == MinigameState.Done &&
				nextMinigameCountdown < 0
			)
		) {
			int nextMinigameIndex = -1;

			// randomly choose next minigame but it can't be the same one
			while (
				nextMinigameIndex == -1 ||
				(
					debugMode == false &&
					activeMinigameIndex == nextMinigameIndex
				)
			) {
				nextMinigameIndex = debugMode ? 0 : Random.Range(0, minigames.Count);
			}

			activeMinigameIndex = nextMinigameIndex;
			var minigamePrefab = minigames[activeMinigameIndex];

			// remove from game scene if already in scene
			if (activeMinigame != null) {
				Destroy(activeMinigame.gameObject);
			}

			activeMinigame = Instantiate(minigamePrefab, transform.position, transform.rotation, transform);
			minigameNameUI.text = activeMinigame.Name;
			startMinigameCountdown = 3;

			var minigame = activeMinigame.GetComponent<Minigame>();
			minigame.director = this;
			if (minigame.PlayerAnimationShouldCast) {
				PlayerSetToggle("is_cast");
			}
			if (minigame.HideStaticBobble) {
				staticBobble.SetActive(false);
			} else {
				staticBobble.SetActive(true);
			}
		}

		if (activeMinigame.State == MinigameState.Waiting) {
			if (startMinigameCountdown <= 0) {
				countdownUI.text = "";
				countdownWrapperUI.style.display = DisplayStyle.None;

				// start minigame
				activeMinigame.State = MinigameState.InProgress;

			} else {
				countdownWrapperUI.style.display = DisplayStyle.Flex;
				var num = Mathf.RoundToInt(startMinigameCountdown).ToString();
				if (num == "0") {
					countdownUI.text = "go!";
				} else {
					countdownUI.text = num;
				}
			}
		}

		// check if game is over
		if (
			activeMinigame.State == MinigameState.Success ||
			activeMinigame.State == MinigameState.Failure
		) {
			countdownWrapperUI.style.display = DisplayStyle.Flex;
			nextMinigameCountdown = 1.5f;

			if (activeMinigame.State == MinigameState.Success) {
				countdownUI.text = "Success!";
				minigameSuccessCount++;
			} else if (activeMinigame.State == MinigameState.Failure) {
				countdownUI.text = "Failure!";
				inEndGame = true;
				inputDelay = 1;
			}
			activeMinigame.State = MinigameState.Done;
		}
    }

	public void PlayerSetBool(string name, bool value) {
		playerAnimator.SetBool(name, value);
	}

	public void PlayerSetToggle(string name) {
		playerAnimator.SetTrigger(name);
	}
}
