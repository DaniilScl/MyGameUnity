using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0,1,0);
    public float cubeChangPlaceSpeed = 0.5f;
    public Transform CubeToPlace;

    public GameObject CubeToCreate, allCubes;
    public GameObject[] canvasStartPage;

    private Rigidbody allCubesRb;

    private bool IsLose, firstCube;

    private List<Vector3> allCubesPositions = new List<Vector3>
    {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
        new Vector3(1,0,1),
        new Vector3(-1,0,-1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1),
    };

    private Coroutine showCubePlace;

    private void Start()
    {
        allCubesRb = allCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());
    }

    private void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && CubeToPlace != null && !EventSystem.current.IsPointerOverGameObject())
        {
            //#if !UNITY_EDITOR
            //            if (Input.GetTouch(0).phase != TouchPlase.Began)
            //                return;

            //#endIf

           

            if(!firstCube)
            {
                firstCube = true;
                foreach (GameObject obj in canvasStartPage)
                    Destroy(obj);
            }
            

            GameObject newCube = Instantiate(CubeToCreate, CubeToPlace.position, Quaternion.identity) as GameObject;

            newCube.transform.SetParent(allCubes.transform);
            nowCube.SetVector(CubeToPlace.position);
            allCubesPositions.Add(nowCube.GetVector());

            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;
            SpawnPositions();
        }

        if(!IsLose && allCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(CubeToPlace.gameObject);
            IsLose = true;
            StopCoroutine(showCubePlace);
        }
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPositions();

            yield return new WaitForSeconds(cubeChangPlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> position = new List<Vector3> ();
        if(IsPositionsEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z))
            && nowCube.x + 1 != CubeToPlace.position.x)
            position.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        
        if (IsPositionsEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z))
            && nowCube.x - 1 != CubeToPlace.position.x)
            position.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        
        if (IsPositionsEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z))
            && nowCube.y + 1 != CubeToPlace.position.x)
            position.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        
        if (IsPositionsEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z))
            && nowCube.y - 1 != CubeToPlace.position.x)
            position.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        
        if (IsPositionsEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1))
            && nowCube.z + 1 != CubeToPlace.position.x)
            position.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        
        if (IsPositionsEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1))
            && nowCube.z - 1 != CubeToPlace.position.x)
            position.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        

        CubeToPlace.position = position[UnityEngine.Random.Range(0, position.Count)];
       
    }
    private bool IsPositionsEmpty(Vector3 targetpos)
    {
        if(targetpos.y == 0)
            return false;

        foreach (Vector3 pos in allCubesPositions)
        {
            if (pos.x == targetpos.x && pos.y == targetpos.y && pos.z == targetpos.z)
                return false;
        }
        return true;
    }

}

struct CubePos { 
    public int x, y, z;

    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;

    }

    public Vector3 GetVector()
    {
        return new Vector3 (x, y, z);
    }

    public void SetVector(Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}
