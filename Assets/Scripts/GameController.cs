using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != gameObject)
            {
                //Destroy(gameObject);
            }
        }
    }

}
