using UnityEngine;

public class PatrullandoGlob : MonoBehaviour
{
    public static PatrullandoGlob Instance;
    public static bool patrullando = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
