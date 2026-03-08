using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class ObstacleItem : MonoBehaviour
{
    [HideInInspector] public BoxCollider2D boxCollider2D;
    private InstantiatedRoom instantiatedRoom;
    private Vector3 previousPosition;

    private void Awake()
    {
        // Get component references
        boxCollider2D = GetComponent<BoxCollider2D>();
        instantiatedRoom = GetComponentInParent<InstantiatedRoom>();

        // Add this item to item obstacles array
        instantiatedRoom.obstacleItemsList.Add(this);
    }
    

    /// <summary>
    /// Update the obstacle positions
    /// </summary>
    private void UpdateObstacles()
    {
        // Make sure the item stays within the room
        ConfineItemToRoomBounds();

        // Update moveable items in obstacles array
        instantiatedRoom.UpdateObstacles();

        // capture new position post collision
        previousPosition = transform.position;
    }

    /// <summary>
    /// Confine the item to stay within the room bounds
    /// </summary>
    private void ConfineItemToRoomBounds()
    {
        Bounds itemBounds = boxCollider2D.bounds;
        Bounds roomBounds = instantiatedRoom.roomColliderBounds;

        // If the item is being pushed beyond the room bounds then set the item position to its
        // previous position
        if (itemBounds.min.x <= roomBounds.min.x ||
            itemBounds.max.x >= roomBounds.max.x ||
            itemBounds.min.y <= roomBounds.min.y ||
            itemBounds.max.y >= roomBounds.max.y)
        {
            transform.position = previousPosition;
        }

    }
}
