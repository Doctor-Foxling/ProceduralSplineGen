using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plant))]
public class GrowthSystem : MonoBehaviour
{
    [System.Serializable]
    public class GrowthRule
    {
        public int[] parentDepth = {0}; 
        public Plant.PlantNode.PlantNodeType parentType = Plant.PlantNode.PlantNodeType.BRANCH;
        public float probability = 0.5f;
        public Plant.PlantNode.PlantNodeType growthType = Plant.PlantNode.PlantNodeType.BRANCH;
        public float energyRequirement = 1;
        public float branchLength = 1;
        public float angle = 0;
        public bool edgeNodeOnly = true;

        public void Apply(Plant.PlantNode n)
        {
            
            if (!(Random.value < probability))
                return;

            if (parentType == Plant.PlantNode.PlantNodeType.LEAF || parentType == Plant.PlantNode.PlantNodeType.FLOWER)
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

                if (growthType == Plant.PlantNode.PlantNodeType.BRANCH)
                {
                    float finalLength = branchLength + (Random.value - 0.5f) * branchLength * 0.2f;
                    float finalAngle = angle * Mathf.Deg2Rad + n.GrowthAngle() + (Random.value - 0.5f) * 10 * Mathf.Deg2Rad; 
                    Vector3 finalAngleVector = new Vector3(Mathf.Sin(finalAngle), Mathf.Cos(finalAngle), 0);
                    Vector3 finalPosition = n.position + finalLength * finalAngleVector;
                    if (finalPosition.y < 0) return; 
                    Plant.PlantNode newNode = new Plant.PlantNode(finalPosition, growthType);
                    n.AddChild(newNode);
                }
                else if(growthType == Plant.PlantNode.PlantNodeType.LEAF)
                {
                    Vector3 finalPosition = n.position;
                    Plant.PlantNode newNode = new Plant.PlantNode(finalPosition, growthType);
                    n.AddChild(newNode);
                }
                else if (growthType == Plant.PlantNode.PlantNodeType.FLOWER)
                {
                    Vector3 finalPosition = n.position;
                    Plant.PlantNode newNode = new Plant.PlantNode(finalPosition, growthType);
                    n.AddChild(newNode);
                }
            }
        }
    }

    public GrowthRule[] growthRules;

    Plant plant;

    float timer;


    void ApplyGrowthRules(Plant.PlantNode n)
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

    public void UpdatePlantNodes(Plant.PlantNode n)
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
        plant = GetComponent<Plant>();
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
