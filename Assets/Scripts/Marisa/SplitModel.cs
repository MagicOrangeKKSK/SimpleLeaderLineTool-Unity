using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitModel : MonoBehaviour
{
    public Transform Root;
    public float AnimationLength = 0.3f;

    public Transform[] Nodes;
    public Transform[] DirectionNodes;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SplitAnimation();   
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            MergeAnimation();
        }
    }

    private void MergeAnimation()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i].DOBlendableMoveBy( Nodes[i].position - DirectionNodes[i].position, AnimationLength)
                .SetEase(Ease.InOutQuad);
        }
    }

    private void SplitAnimation()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i].DOBlendableMoveBy(DirectionNodes[i].position - Nodes[i].position, AnimationLength)
                .SetEase(Ease.InOutQuad);
        }
    }

#if UNITY_EDITOR
    [Button]
    public void GenerateAndBindDirectionNodes()
    {
        GameObject[] allDirectionNodes = GameObject.FindGameObjectsWithTag("Direction");
        for(int i = 0;i<allDirectionNodes.Length;i++)
            GameObject.DestroyImmediate(allDirectionNodes[i]); 

        int childCount = Root.childCount;
        Nodes = new Transform[childCount];
        DirectionNodes = new Transform[childCount];

        for(int i =0;i< childCount; i++) 
        {
            Nodes[i] = Root.GetChild(i);
            DirectionNodes[i] = AddDirectionNode(Nodes[i]);
        }
    }


    public Transform AddDirectionNode(Transform node)
    {
        GameObject gameObject = new GameObject("Direction");
        gameObject.tag = "Direction";
        gameObject.transform.SetParent(node);
        gameObject.transform.position = node.position + (node.position - Root.position).normalized;
        return gameObject.transform;
    }


    public void OnDrawGizmos()
    {
        if(Nodes == null || DirectionNodes == null) 
            return;
        if(Nodes.Length != DirectionNodes.Length) 
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < Nodes.Length; i++)
        {
            Gizmos.DrawLine(Nodes[i].position, DirectionNodes[i].position);
        }
    }
#endif
}
