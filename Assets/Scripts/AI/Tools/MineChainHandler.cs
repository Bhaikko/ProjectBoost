using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectBoost.AI.Tools {

    [ExecuteInEditMode]
    public class MineChainHandler : MonoBehaviour
    {
        [SerializeField] GameObject chainMesh;
        [SerializeField] [Range(1, 5)] int numberOfChains = 1;

        [SerializeField] List<GameObject> chainRefs = new List<GameObject>();

        void Start() {
            if (EditorApplication.isPlaying) {
                return;
            }
        }

        private void InstantiateChain()
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
            if (EditorApplication.isPlaying) {
                return;
            }


            if (chainRefs.Count == numberOfChains) {
                return;
            }

            if (chainRefs.Count == 0) {
                chainRefs.Add(chainMesh);
                return;
            }

            if (chainRefs.Count < numberOfChains) {
                InstantiateChain();
            } else {
                DestroyImmediate(chainRefs[chainRefs.Count - 1]);
                
                chainRefs.RemoveAt(chainRefs.Count - 1);
            }
        }
    }
}
