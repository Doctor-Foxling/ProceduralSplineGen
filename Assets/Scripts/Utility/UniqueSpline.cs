using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueSpline : MonoBehaviour
{
    SplineMesh.Spline _spline;
    public SplineMesh.Spline spline   // Tries to find a mesh filter, adds one if it doesn't exist yet
    {
        get
        {
            _spline = _spline == null ? GetComponent<SplineMesh.Spline>() : _spline;
            _spline = _spline == null ? gameObject.AddComponent<SplineMesh.Spline>() : _spline;
            return _spline;
        }
    }
}
