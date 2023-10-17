using UnityEngine;

public class PauseEvents : MonoBehaviour
{
    void OnMouseDown()
    {
        gameObject.SetActive(false);
    }
}
