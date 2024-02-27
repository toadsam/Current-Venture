using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCheck : MonoBehaviour
{
    public GameObject player;

    void LateUpdate()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity,
                            1 << LayerMask.NameToLayer("EnvironmentObject"));

        for (int i = 0; i < hits.Length; i++)
        {
            Transparent[] obj = hits[i].transform.GetComponentsInChildren<Transparent>();

            for (int j = 0; j < obj.Length; j++)
                obj[j]?.BecomeTransparent();
        }
    }
}
