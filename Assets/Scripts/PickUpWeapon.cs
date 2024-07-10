using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PickUpWeapon : NetworkBehaviour
{
    public GameObject playerCamera;
    public float distance = 15f;
    public Transform holdPoint;
    GameObject currentObject;
    bool canPickUp;

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentObject == null)
            {
                PickUp();
            }
            else
            {
                Drop();
            }
        }
    }

    void PickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, distance))
        {
            if (hit.transform.CompareTag("Cube"))
            {
                CmdPickUpObject(hit.transform.gameObject);
            }
        }
    }

    [Command]
    void CmdPickUpObject(GameObject obj)
    {
        RpcPickUpObject(obj);
    }

    [ClientRpc]
    void RpcPickUpObject(GameObject obj)
    {
        if (obj != null)
        {
            currentObject = obj;
            currentObject.GetComponent<Rigidbody>().isKinematic = true;
            currentObject.transform.SetParent(holdPoint);
            currentObject.transform.localPosition = Vector3.zero;
            currentObject.transform.localRotation = Quaternion.identity;
        }
    }

    void Drop()
    {
        if (currentObject != null)
        {
            CmdDropObject(currentObject);
            currentObject = null;
        }
    }

    [Command]
    void CmdDropObject(GameObject obj)
    {
        RpcDropObject(obj);
    }

    [ClientRpc]
    void RpcDropObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.transform.SetParent(null);
            obj.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
