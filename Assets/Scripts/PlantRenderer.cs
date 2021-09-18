using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plant))]
public class PlantRenderer : MonoBehaviour
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

    Plant plant;


    void RecursiveBuildMeshes(Plant.PlantNode node, int parentVInx)
    {
        int myVInx;
        if (node.plantBlockType == Plant.PlantNode.PlantNodeType.BRANCH)
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
        else if(node.plantBlockType == Plant.PlantNode.PlantNodeType.LEAF)
        {
            //Add Vertex
            leafVerticies.Add(node.position);
            leafColors.Add(new Color(1, 1, 1));
            leafIndicies.Add(leafIndicies.Count);
            
        }
        else if (node.plantBlockType == Plant.PlantNode.PlantNodeType.FLOWER)
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

        leafMesh.vertices = leafVerticies.ToArray();
        leafMesh.colors = leafColors.ToArray();
        leafMesh.SetIndices(leafIndicies.ToArray(), MeshTopology.Points, 0);

        flowerMesh.vertices = flowerVerticies.ToArray();
        flowerMesh.colors = flowerColors.ToArray();
        flowerMesh.SetIndices(flowerIndicies.ToArray(), MeshTopology.Points, 0);
    }


    private void Awake()
    {
        plant = GetComponent<Plant>();

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
