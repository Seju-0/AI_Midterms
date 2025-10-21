using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    public Renderer playerRenderer;
    public Color playerColor = Color.red;

    void Start()
    {
        if (playerRenderer == null)
            playerRenderer = GetComponent<Renderer>();

        playerRenderer.material.color = playerColor;
    }
}
