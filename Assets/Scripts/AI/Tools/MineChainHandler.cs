using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectBoost.AI.Tools {
    public class MineChainHandler : MonoBehaviour
    {
        [SerializeField] GameObject chainMesh;
        
        [SerializeField] 
        [Range(1, 5)] 
        [Tooltip("Number of Chains to Spawn below the Mine at runtime.")]
        int numberOfChains = 1;

        List<GameObject> chainRefs;

        void Start() {
            chainRefs = new List<GameObject>();
            chainRefs.Add(chainMesh);
            for (int i = 1; i < numberOfChains; i++) {
                InstantiateChain(i);
            }
        }

        private void InstantiateChain(int index)
        {
            Vector3 newPositionToAttach = chainMesh.transform.position;
            newPositionToAttach.y -= chainRefs.Count * chainRefs[chainRefs.Count - 1].GetComponent<Renderer>().bounds.size.y;

            GameObject newChain = Instantiate<GameObject>(chainMesh, Vector3.up, Quaternion.identity);
            newChain.transform.parent = transform;
            newChain.transform.position = newPositionToAttach;
            newChain.transform.localScale = new Vector3(125.0f, 125.0f, 125.0f);

            chainRefs.Add(newChain);

        }

        void Update() {

        }
    }
}
