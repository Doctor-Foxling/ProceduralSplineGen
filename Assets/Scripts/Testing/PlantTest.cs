using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlantTest : MonoBehaviour
{
    public class BoundingBox
    {
        public Vector2 min = new Vector2(0, 0);
        public Vector2 max = new Vector2(0, 0);
    }

    public class PlantNode
    {
        public enum PlantNodeType { BRANCH, LEAF, FLOWER };

        public PlantNode parent;
        public List<PlantNode> children;
        public Vector3 position;
        public int age;
        public PlantNodeType plantBlockType;
        public int depth;
        public BoundingBox boundingBox;
        public float size; 

        public PlantNode()
        {
            depth = 0;
            parent = null;
            children = new List<PlantNode>();
            position = new Vector3(0, 0, 0);
            age = 0;
            plantBlockType = PlantNodeType.BRANCH;
            size = 0;
        }

        public PlantNode(Vector3 pos)
        {
            depth = 0;
            parent = null;
            children = new List<PlantNode>();
            position = pos;
            age = 0;
            plantBlockType = PlantNodeType.BRANCH;
            size = 0;
        }

        public PlantNode(Vector3 pos, PlantNodeType t)
        {
            depth = 0;
            parent = null;
            children = new List<PlantNode>();
            position = pos;
            age = 0;
            plantBlockType = t;
            size = 0;
        }

        public void AddChild(PlantNode pn)
        {
            children.Add(pn);
            pn.parent = this;
            pn.depth = depth + 1;
        }

        public float GrowthAngle()
        {
            if(parent == null)
            {
                return 0; 
            }
            
            Vector3 a = (position - parent.position).normalized;
            return Mathf.Atan2(a.x, a.y);
        }

        public float Length()
        {
            if (parent == null)
            {
                return 0;
            }

            return (position - parent.position).magnitude;
        }


        BoundingBox FindBoundingBox()
        {
            BoundingBox bb = new BoundingBox();
            bb.min = position;
            bb.max = position;
            for (int i = 0; i < children.Count; i++)
            {
                BoundingBox cb = children[i].FindBoundingBox();
                
                bb.min.x = Mathf.Min(bb.min.x, cb.min.x);
                bb.min.y = Mathf.Min(bb.min.y, cb.min.y);
                bb.max.x = Mathf.Min(bb.max.x, cb.max.x);
                bb.max.y = Mathf.Min(bb.max.y, cb.max.y);
            }
            boundingBox = bb;
            return bb;
        }
    }
    
    public PlantNode rootNode = new PlantNode(new Vector3(0, 0, 0), PlantNode.PlantNodeType.BRANCH);

    public UnityEvent onGrowth;

   

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
