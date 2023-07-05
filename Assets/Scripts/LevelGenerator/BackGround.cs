using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private Dictionary<string, GameObject> backgroundPrefabs;
    private Transform bgParent;
    private void Awake()
    {
        Object[] resourseBackGround = Resources.LoadAll("Prefabs/BackGround", typeof(GameObject));
        backgroundPrefabs = new Dictionary<string, GameObject>();
        if (resourseBackGround.Length == 0)
        {
            Debug.LogError("The path is wrong");
        }
        foreach (Object resource in resourseBackGround)
        {
            GameObject background = (GameObject)resource;
            backgroundPrefabs.Add(background.name, background);
        }
        bgParent = GameObject.Find("BackGround").GetComponent<Transform>();
    }
    public void CreateBackGround(int n,int m)
    {
        for(int y = 0; y < m; y += 64)
        {
            for(int x = 0; x < n; x += 64)
            {
                if (Random.value < 0.1f)
                {
                    if (Random.value < 0.5f)
                    {
                        Instantiate(backgroundPrefabs["bgHmm"],new Vector3(x+Random.Range(-16,16),y+Random.Range(-16,16),0),Quaternion.identity,bgParent);
                    }
                    else
                    {
                        Instantiate(backgroundPrefabs["bgTrees"], new Vector3(x + Random.Range(-16, 16), y + Random.Range(-16, 16), 0), Quaternion.identity, bgParent);

                    }
                }              
                Instantiate(backgroundPrefabs["Background"], new Vector3(x,y,0), Quaternion.identity, bgParent);
            }
        }
    }
}
