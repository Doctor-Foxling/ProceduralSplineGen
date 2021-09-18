using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlantTest))]
public class GrowthSystemTest : MonoBehaviour
{
    [System.Serializable]
    public class GrowthRule
    {
        public int[] parentDepth = {0}; 
        public PlantTest.PlantNode.PlantNodeType parentType = PlantTest.PlantNode.PlantNodeType.BRANCH;
        public float probability = 0.5f;
        public PlantTest.PlantNode.PlantNodeType growthType = PlantTest.PlantNode.PlantNodeType.BRANCH;
        public float energyRequirement = 1;
        public float branchLength = 1;
        public float angle = 0;
        public bool edgeNodeOnly = true;

        public void Apply(PlantTest.PlantNode n)
        {
            
            if (!(Random.value < probability))
                return;

            if (parentType == PlantTest.PlantNode.PlantNodeType.LEAF || parentType == PlantTest.PlantNode.PlantNodeType.FLOWER)
                return;

            bool depthChecksOut = false; 
            for(int i = 0; i < parentDepth.Length; i++)
            {
                if(n.depth == parentDepth[i])
                {
                    depthChecksOut = true; 
                    break;
                }
            }

            if(n.plantBlockType == parentType && depthChecksOut)
            {

                if (growthType == PlantTest.PlantNode.PlantNodeType.BRANCH)
                {
                    float finalLength = branchLength + (Random.value - 0.5f) * branchLength * 0.2f;
                    float finalAngle = angle * Mathf.Deg2Rad + n.GrowthAngle() + (Random.value - 0.5f) * 10 * Mathf.Deg2Rad; 
                    Vector3 finalAngleVector = new Vector3(Mathf.Sin(finalAngle), Mathf.Cos(finalAngle), 0);
                    Vector3 finalPosition = n.position + finalLength * finalAngleVector;
                    if (finalPosition.y < 0) return; 
                    PlantTest.PlantNode newNode = new PlantTest.PlantNode(finalPosition, growthType);
                    n.AddChild(newNode);
                }
                else if(growthType == PlantTest.PlantNode.PlantNodeType.LEAF)
                {
                    Vector3 finalPosition = n.position;
                    PlantTest.PlantNode newNode = new PlantTest.PlantNode(finalPosition, growthType);
                    n.AddChild(newNode);
                }
                else if (growthType == PlantTest.PlantNode.PlantNodeType.FLOWER)
                {
                    Vector3 finalPosition = n.position;
                    PlantTest.PlantNode newNode = new PlantTest.PlantNode(finalPosition, growthType);
                    n.AddChild(newNode);
                }
            }
        }
    }

    public GrowthRule[] growthRules;

    PlantTest plant;

    float timer;


    void ApplyGrowthRules(PlantTest.PlantNode n)
    {
       
        for (int i = 0; i < n.children.Count; i++)
        {
            ApplyGrowthRules(n.children[i]);
        }

        bool zeroChildren = n.children.Count == 0;
        for (int i = 0; i < growthRules.Length; i++)
        {
            if (zeroChildren && growthRules[i].edgeNodeOnly || !growthRules[i].edgeNodeOnly)
                growthRules[i].Apply(n);
        }

    }

    public void UpdatePlantNodes(PlantTest.PlantNode n)
    {
        //
        n.age++;

        //Update children
        for (int i = 0; i < n.children.Count; i++)
        {
            ApplyGrowthRules(n.children[i]);
        }
    }


    public void Grow()
    {
        UpdatePlantNodes(plant.rootNode); 
        ApplyGrowthRules(plant.rootNode);
    }


    // Start is called before the first frame update
    void Start()
    {
        plant = GetComponent<PlantTest>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.0)
        {
            Grow();
            plant.onGrowth.Invoke();
            timer = 0;
        }
    }
}
