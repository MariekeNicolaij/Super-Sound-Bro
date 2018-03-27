using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugManager : MonoBehaviour
{
    public static PlugManager instance;
    public List<Plug> plugs;
    public List<GameObject> finishPlugs;


    void Awake()
    {
        instance = this;

        GetPlugs();
    }

    void GetPlugs()
    {
        foreach (GameObject plug in GameObject.FindGameObjectsWithTag("Plug"))
            plugs.Add(plug.GetComponent<Plug>());
        foreach (GameObject fPlug in GameObject.FindGameObjectsWithTag("Finish Plug"))
            finishPlugs.Add(fPlug);
    }

    public void RemovePlugFromList(Plug plug)
    {
        plug.tag = "Untagged";
        plugs.Remove(plug);
    }

    public GameObject GetClosestFinishPlug(Vector3 startPosition)
    {
        GameObject closestPlug = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject plug in finishPlugs)
        {
            float distance = Vector2.Distance(startPosition, plug.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlug = plug;
            }
        }

        return closestPlug;
    }
}
