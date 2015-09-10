using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public void NextLevelButton(int index)
    {
        Application.LoadLevel(index);
    }

    public void NextLevelButton(string name)
    {
        Application.LoadLevel(name);
    }
}
