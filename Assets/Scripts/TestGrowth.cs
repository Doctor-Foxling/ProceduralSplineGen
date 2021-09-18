using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Plant))]
public class TestGrowth : MonoBehaviour
{

    public float clickHeight = 1;

    float timer;

    Plant plant;

    // Start is called before the first frame update
    void Start()
    {
        plant = GetComponent<Plant>();
    }

    void TestGrow(Plant.PlantNode n, float height)
    {
        if (n.children.Count == 0)
        {
            if (n.plantBlockType == Plant.PlantNode.PlantNodeType.BRANCH)
            {
                float rotationRange = Mathf.PI / 4 * 7 / height;
                int depth = (int)(height * 1.5); //9;
                if (Random.value < 0.8 && n.depth > depth)
                {
                    Plant.PlantNode c = new Plant.PlantNode(n.position, Plant.PlantNode.PlantNodeType.LEAF);
                    n.AddChild(c);
                }
                else
                {

                    float randf = (Random.value - 0.5f) * rotationRange;
                    float randLen = Random.value * 0.8f + 0.2f;
                    float angle = n.GrowthAngle() + randf;
                    Plant.PlantNode a = new Plant.PlantNode(n.position + randLen * new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0));
                    n.AddChild(a);
                    if (Random.value > 0.5)
                    {

                        randf = (Random.value - 0.5f) * rotationRange;
                        angle = n.GrowthAngle() + randf;
                        randLen = Random.value * 0.8f + 0.2f;
                        Plant.PlantNode b = new Plant.PlantNode(n.position + randLen * new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0));
                        n.AddChild(b);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < n.children.Count; i++)
            {
                TestGrow(n.children[i], height);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.0)
        {
            TestGrow(plant.rootNode, clickHeight);
            plant.onGrowth.Invoke();
            timer = 0;
        }
    }
}
