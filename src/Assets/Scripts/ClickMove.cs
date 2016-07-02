using UnityEngine;

public class ClickMove : MonoBehaviour, IClickable {

    public GameObject player;

    #region IClickable implementation

    public void OnClick (RaycastHit hit)
    {
        Debug.Log("Moving to " + hit.point);

        var navigator = player.GetComponent<Navigator>();
        var netMove = player.GetComponent<NetworkMove>();
        navigator.NavigateTo(hit.point);

        netMove.OnMove(hit.point);
	}

    #endregion
}
