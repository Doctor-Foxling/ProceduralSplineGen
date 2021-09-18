using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueSplineMeshTiling : MonoBehaviour
{
    [HideInInspector] int ownerID;  // To ensure they have a unique mesh
    SplineMesh.SplineMeshTiling _tiling;

    public SplineMesh.SplineMeshTiling tiling   // Tries to find a mesh filter, adds one if it doesn't exist yet
    {
        get
        {
            _tiling = _tiling == null ? GetComponent<SplineMesh.SplineMeshTiling>() : _tiling;
            _tiling = _tiling == null ? gameObject.AddComponent<SplineMesh.SplineMeshTiling>() : _tiling;
            return _tiling;
        }
    }

    Mesh _mesh;
    public Mesh mesh
    {
        get
        {
            bool isOwner = ownerID == gameObject.GetInstanceID();
            if (tiling.mesh == null || !isOwner)
            {
                tiling.mesh = _mesh = new Mesh();
                ownerID = gameObject.GetInstanceID();
                _mesh.name = "Mesh [" + ownerID + "]";
            }
            return _mesh;
        }
    }

    //Constructor
    public UniqueSplineMeshTiling()
    {
        _tiling = this.tiling;
        _mesh = this.mesh; 
    }
}
