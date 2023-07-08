using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class AgentRenderer : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    [field:SerializeField]
    public UnityEvent<int> OnBackwardsMovement { get; set; }

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

    public void CheckBackWardMovement(Vector2 movementVector)
    {
        float angle = 0;
        if(spriteRenderer.flipX == true)
        {
            angle = Vector2.Angle(-transform.right, movementVector);
        }
        else
        {
            angle = Vector2.Angle(transform.right, movementVector);
        }
        if (angle > 90)
        {
            if (OnBackwardsMovement != null)
            {
                OnBackwardsMovement.Invoke(-1);
            }
        }
        else
        {
            if (OnBackwardsMovement != null)
            {
                OnBackwardsMovement.Invoke(1);
            }
        }
    }
}
