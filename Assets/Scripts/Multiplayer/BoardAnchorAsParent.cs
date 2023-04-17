using UnityEngine;

public class BoardAnchorAsParent : MonoBehaviour
{
    private void Start()
    {
        if (BoardAnchor.instance != null)
        {
            transform.parent = BoardAnchor.instance.transform;
        }
    }
}
