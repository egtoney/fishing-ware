using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
	public List<Minigame> minigames;

	private Minigame activeMinigame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeMinigame == null) {
			var minigamePrefab = minigames[0];

			activeMinigame = Instantiate(minigamePrefab, transform.position, transform.rotation, transform);
		}

		var state = activeMinigame.GetState();
		if (state == MinigameState.Success || state == MinigameState.Failure) {
			Debug.Log(state);
		}
    }
}
