using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(this.gameObject);
    }
}
