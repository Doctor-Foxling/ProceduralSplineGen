using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlantTest))]
public class PlantRendererTest : MonoBehaviour
{

    public Material branchMaterial;
    public Material leafMaterial;
    public Material flowerMaterial;

    List<Vector3> branchVerticies = new List<Vector3>();
    List<Color> branchColors = new List<Color>();
    List<int> branchIndicies = new List<int>();

    List<Vector3> leafVerticies = new List<Vector3>();
    List<Color> leafColors = new List<Color>();
    List<int> leafIndicies = new List<int>();

    List<Vector3> flowerVerticies = new List<Vector3>();
    List<Color> flowerColors = new List<Color>();
    List<int> flowerIndicies = new List<int>();

    Mesh branchMesh;
    Mesh leafMesh;
    Mesh flowerMesh;

    PlantTest plant;

    private SplineMesh.Spline spline;

    private SplineMesh.SplineMeshTiling _meshTiling;

    List<Vector3> directions;
    List<Vector3> points;

    public float startScale = 0.2f, endScale = 0.2f;
    public float startRoll = 0, endRoll = 0;

    public int curve_num;

    void RecursiveBuildMeshes(PlantTest.PlantNode node, int parentVInx)
    {
        int myVInx;
        if (node.plantBlockType == PlantTest.PlantNode.PlantNodeType.BRANCH)
        {
            //Add Vertex
            branchVerticies.Add(node.position);
            branchColors.Add(new Color(1, 1, 1));
            myVInx = branchVerticies.Count - 1;

            //if parent not null add line
            if (node.parent != null)
            {
                branchIndicies.Add(parentVInx);
                branchIndicies.Add(myVInx);
            }

            //recurse for each child
            for (int i = 0; i < node.children.Count; i++)
            {
                RecursiveBuildMeshes(node.children[i], myVInx);
            }
        }
        else if(node.plantBlockType == PlantTest.PlantNode.PlantNodeType.LEAF)
        {
            //Add Vertex
            leafVerticies.Add(node.position);
            leafColors.Add(new Color(1, 1, 1));
            leafIndicies.Add(leafIndicies.Count);
            
        }
        else if (node.plantBlockType == PlantTest.PlantNode.PlantNodeType.FLOWER)
        {
            //Add Vertex
            flowerVerticies.Add(node.position);
            flowerColors.Add(new Color(1, 1, 1));
            flowerIndicies.Add(flowerIndicies.Count);

        }
    }

    void BuildRebuildMeshes()
    {
        //clear 
        branchVerticies.Clear();
        branchColors.Clear();
        branchIndicies.Clear();

        leafVerticies.Clear();
        leafColors.Clear();
        leafIndicies.Clear();

        flowerVerticies.Clear();
        flowerColors.Clear();
        flowerIndicies.Clear();

        //Compute Branches
        RecursiveBuildMeshes(plant.rootNode, -1);

        branchMesh.vertices = branchVerticies.ToArray();
        branchMesh.colors = branchColors.ToArray();
        branchMesh.SetIndices(branchIndicies.ToArray(), MeshTopology.Lines, 0);
        RenderBranches3D();

        leafMesh.vertices = leafVerticies.ToArray();
        leafMesh.colors = leafColors.ToArray();
        leafMesh.SetIndices(leafIndicies.ToArray(), MeshTopology.Points, 0);

        flowerMesh.vertices = flowerVerticies.ToArray();
        flowerMesh.colors = flowerColors.ToArray();
        flowerMesh.SetIndices(flowerIndicies.ToArray(), MeshTopology.Points, 0);
    }


    private void Awake()
    {
        plant = GetComponent<PlantTest>();

        plant.onGrowth.AddListener(() =>
        {
            BuildRebuildMeshes();
        });

    }

    // Start is called before the first frame update
    void Start()
    {
        branchMesh = new Mesh();
        leafMesh = new Mesh();
        flowerMesh = new Mesh();

        leafMaterial = Object.Instantiate(leafMaterial);
        branchMaterial = Object.Instantiate(branchMaterial);
        flowerMaterial = Object.Instantiate(flowerMaterial);

        leafMaterial.color = Random.ColorHSV(0.1f, 0.4f, 0.5f, 0.8f, 0.5f, 0.8f);

        flowerMaterial.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);

        if (gameObject.GetComponent<SplineMesh.Spline>() == null)

            spline = gameObject.AddComponent<SplineMesh.Spline>();

        if (gameObject.GetComponent<SplineMesh.SplineMeshTiling>() == null)
            _meshTiling = gameObject.AddComponent<SplineMesh.SplineMeshTiling>();

        directions = new List<Vector3>()
        {
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f)
        };

        points = new List<Vector3>()
        {
            new Vector3(-4.0f, 4.0f, 1.0f),
            new Vector3(4.0f, 4.0f, 1.0f),
            new Vector3(-4.0f, 4.0f, 1.0f),
            new Vector3(4.0f, -4.0f, 1.0f)
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RenderBranches3D()
    {
        spline = gameObject.GetComponent<SplineMesh.Spline>();
        _meshTiling = gameObject.GetComponent<SplineMesh.SplineMeshTiling>();

        _meshTiling.rotation = new Vector3(0.0f, 90.0f, 0.0f);

        GameObject my_mesh = Resources.Load("Mesh/cylinder_1") as GameObject;

        _meshTiling.mesh = my_mesh.GetComponent<MeshFilter>().sharedMesh;
        _meshTiling.material = my_mesh.GetComponent<MeshRenderer>().sharedMaterial;

        List<SplineMesh.SplineNode> my_nodes = new List<SplineMesh.SplineNode>();

        for (int i = 0; i < branchMesh.vertices.Length; i++)
        {
            my_nodes.Add(new SplineMesh.SplineNode(branchMesh.vertices[i], new Vector3(1.0f, 1.0f, 1.0f)));
        }

        /*for (int i = 0; i < points.Count; i++)
        {
            my_nodes.Add(new SplineMesh.SplineNode(points[i], directions[i]));
        }*/

        spline.ResetAndAddNode(my_nodes);

        curve_num = 0;
        //spline.AddNode(newNode);
        // apply scale and roll at each node
        float currentLength = 0;
        foreach (SplineMesh.CubicBezierCurve curve in spline.GetCurves())
        {
            float startRate = currentLength / spline.Length;
            currentLength += curve.Length;
            float endRate = currentLength / spline.Length;

            curve.n1.Scale = Vector2.one * (startScale + (endScale - startScale) * startRate);
            curve.n2.Scale = Vector2.one * (startScale + (endScale - startScale) * endRate);

            curve.n1.Roll = startRoll + (endRoll - startRoll) * startRate;
            curve.n2.Roll = startRoll + (endRoll - startRoll) * endRate;

            curve_num++;
        }
    }

    void OnRenderObject()
    {
        if (branchMaterial == null || leafMaterial == null || flowerMaterial == null)
            return;

        branchMaterial.SetPass(0);
        Graphics.DrawMeshNow(branchMesh, transform.position, transform.rotation);

        leafMaterial.SetPass(0);
        Graphics.DrawMeshNow(leafMesh, transform.position, transform.rotation);

        flowerMaterial.SetPass(0); 
        Graphics.DrawMeshNow(flowerMesh, transform.position, transform.rotation);
    }
}
