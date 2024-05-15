using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameState {
	InProgress,
	Success,
	Failure
}

public abstract class Minigame : MonoBehaviour
{
	public abstract string Name { get; }
	public abstract MinigameState GetState();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
