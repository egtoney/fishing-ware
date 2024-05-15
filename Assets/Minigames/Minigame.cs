using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameState {
	Waiting,
	InProgress,
	Success,
	Failure,
	Done
}

public abstract class Minigame : MonoBehaviour
{
	public abstract string Name { get; }
	public MinigameState State = MinigameState.Waiting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (State == MinigameState.InProgress) {
			FixedUpdateGame();
		}
    }

	protected abstract void FixedUpdateGame();
}
