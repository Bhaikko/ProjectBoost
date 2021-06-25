using System.Collections;
using System.Collections.Generic;
using ProjectBoost.Player;
using UnityEngine;

namespace ProjectBoost.AI.Components {
    public class KillAttach : MonoBehaviour
    {
        [SerializeField] GameObject bone = null;

        public void AttachBoneToPlayer() {
            FindObjectOfType<Diver>().SetKiller(ref bone);
        }
    }
}

