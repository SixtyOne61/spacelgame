using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

public class EntWorld : SpacelEntity
{
    public Tool.SCRMap ParamMap;
    public Tool.SCROneValue ParamNbChunck;

    private Transform _refTransform;
    public Transform RefTransform
    {
        set { _refTransform = value; SpawnChunck(); }
        get { return _refTransform; }
    }

    private GameObject[,,] _chuncks;
    private HashSet<Vector3Int> _chuncksActive = new HashSet<Vector3Int>();
    private Vector3Int _curr = new Vector3Int(-1, -1, -1); // invalid

    public override void Start()
    {
        base.Start();
        Spawn();
    }
    
    public void Spawn()
    {
    	GameObject[] prefabs = Resources.LoadAll<GameObject>("Assets/Prefab/World/Generate/");
    	foreach(GameObject go in prefabs)
    	{
    		GameObject chunck = Instantiate(go, Vector3.zero, Quaternion.identity);
    		chunck.transform.SetParent(transform);
    	}
    }

    public override void Update()
    {
    	return; // for test if it's work, remove
        if(_curr != EstimateCurrCoord())
        {
            // update chunck active
            UpdateActive();
        }
        base.Update();
    }

#if (UNITY_EDITOR)
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (Tool.DebugWindowAccess.Instance.Serialize.EnableDrawChunck)
        {
            int nbChunck = (int)ParamNbChunck.Value;
            for (int x = 0; x < nbChunck; ++x)
            {
                for (int y = 0; y < nbChunck; ++y)
                {
                    for (int z = 0; z < nbChunck; ++z)
                    {
                        if(_chuncks[x, y, z].activeSelf)
                        {
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawWireCube(new Vector3(x * ParamMap.Width + ParamMap.Width / 2.0f, y * ParamMap.Height + ParamMap.Height / 2.0f, z * ParamMap.Depth + ParamMap.Depth / 2.0f), new Vector3(ParamMap.Width, ParamMap.Height, ParamMap.Depth));
                        }
                    }
                }
            }
        }
    }
#endif

    private void SpawnChunck()
    {
        int nbChunck = (int)ParamNbChunck.Value;
        _chuncks = new GameObject[nbChunck, nbChunck, nbChunck];

        for (int x = 0; x < nbChunck; ++x)
        {
            for(int y = 0; y < nbChunck; ++y)
            {
                for(int z = 0; z < nbChunck; ++z)
                {
                    Vector3 startPos = new Vector3(x * ParamMap.Width, y * ParamMap.Height, z * ParamMap.Depth);
                    _chuncks[x, y, z] = Builder.Instance.Build(Builder.FactoryType.World, (int)BuilderWorld.Type.Chunck, transform.position + startPos, Quaternion.identity, transform);
                    _chuncks[x, y, z].SetActive(false);
                    _chuncks[x, y, z].name = x + " " + y + " " + z;
                }
            }
        }
    }

    private Vector3Int EstimateCurrCoord()
    {
        if (RefTransform == null)
        {
            Debug.LogError("Ref transform was null, no active chunck.");
            return _curr;
        }

        // determine chunck coord of this ref
        return new Vector3Int((int)RefTransform.position.x / ParamMap.Width, (int)RefTransform.position.y / ParamMap.Height, (int)RefTransform.position.z / ParamMap.Depth);
    }

    private void UpdateActive()
    {
        foreach(Vector3Int disableCoord in _chuncksActive)
        {
            _chuncks[disableCoord.x, disableCoord.y, disableCoord.z].SetActive(false);
        }
        _chuncksActive.Clear();

        _curr = EstimateCurrCoord();
        AddInActive(_curr);

        for(int x = _curr.x - 2; x <= _curr.x + 2; ++x)
        {
            for (int y = _curr.y - 2; y <= _curr.y + 2; ++y)
            {
                for (int z = _curr.z - 2; z <= _curr.z + 2; ++z)
                {
                    StartCoroutine(AddInActive(new Vector3Int(x, y, z)));
                }
            }
        }
    }

    private bool IsValidChunckCoord(Vector3Int coord)
    {
        return coord.x >= 0 && coord.x < ParamNbChunck.Value
            && coord.y >= 0 && coord.y < ParamNbChunck.Value
            && coord.z >= 0 && coord.z < ParamNbChunck.Value;
    }

    private IEnumerator AddInActive(Vector3Int coord)
    {
        if (IsValidChunckCoord(coord))
        {
            _chuncksActive.Add(coord);
            _chuncks[coord.x, coord.y, coord.z].SetActive(true);
        }
        yield return null;
    }
}
    
