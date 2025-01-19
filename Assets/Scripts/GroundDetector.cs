using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private List<GameObject> collisionObjects = new List<GameObject>();
    private bool _isGrounded;

    public bool GetGroundedState()
    {
        _isGrounded = collisionObjects.Count > 0 ? true : false;
        return _isGrounded;
    }

    public void SetGroundParentState(GameObject playerObject)
    {
        if (collisionObjects.Count <= 0)
            return;

        if (collisionObjects[0].gameObject.tag != "MovingGround")
            playerObject.transform.SetParent(collisionObjects[0].gameObject.transform);
        else
            playerObject.transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D collision) => collisionObjects.Add(collision.gameObject);
    private void OnTriggerExit2D(Collider2D collision) => collisionObjects.Remove(collision.gameObject);
}
