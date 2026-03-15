using UnityEngine;

public class Reveal : MonoBehaviour
{
    public GameObject[] hiddenTextObject;

    public void ShowText()
    {
        foreach (GameObject hidden in hiddenTextObject)
        {
            hidden.SetActive(true);
        }
        
    }
}
