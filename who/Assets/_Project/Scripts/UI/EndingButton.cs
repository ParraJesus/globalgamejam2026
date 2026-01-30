using UnityEngine;

public class EndingButton : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("GAME OVER.");
    }
}
