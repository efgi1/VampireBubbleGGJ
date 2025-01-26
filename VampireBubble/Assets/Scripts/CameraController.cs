using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TODO player access anywhere, one lookup
    private PlayerController[] _players;
    private float _cameraOffset;

    void Start()
    {
        _players = FindObjectsByType<PlayerController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        _cameraOffset = transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 middlePoint = CalculateMiddlePoint();
        transform.position = new Vector3(middlePoint.x, middlePoint.y, _cameraOffset);
    }

    private Vector3 CalculateMiddlePoint()
    {
        // TODO 4 player best middle
        //Vector3 sum = Vector3.zero;
        //foreach (PlayerController player in _players)
        //{
        //    sum += player.transform.position;
        //}
        //return sum / _players.Length;

        return GameManager.Instance.PlayerController.transform.position;
    }
}
