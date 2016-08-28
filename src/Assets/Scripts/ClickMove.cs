using UnityEngine;

public class ClickMove : MonoBehaviour, IClickable {

    public GameObject player;

    #region IClickable implementation

    public void OnClick (RaycastHit hit)
    {
        if (GetComponent<Hittable>() && GetComponent<Hittable>().IsDead)
            return;

        Debug.Log("Moving to " + hit.point);

        var navigator = player.GetComponent<Navigator>();
        navigator.NavigateTo(hit.point);

        Network.Move(hit.point);
	}

    #endregion
}
