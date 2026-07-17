using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 5f; 
    private float currentPosX;

    private void Update()
    {
        Vector3 targetPosition = new Vector3(currentPosX, transform.position.y, transform.position.z);
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}