using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class AgentRenderer : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        GameInput.Instance.OnMousePositionChanged += GameInput_OnMousePositionChanged;
    }
    private void OnDestroy()
    {
        GameInput.Instance.OnMousePositionChanged -= GameInput_OnMousePositionChanged;
    }

    private void GameInput_OnMousePositionChanged(object sender, GameInput.OnMousePositionChangedEventArgs e)
    {
        FaceDirection(e.mousePosition);
    }


    public void FaceDirection(Vector2 pointerInput)
    {
        Vector3 direction = (Vector3)pointerInput - this.transform.position;
        Vector3 result = Vector3.Cross(Vector2.up, direction);
        if (result.z > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (result.z < 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}
