using UnityEngine;

namespace ProjectBoost.AI {
    public interface IEnemy {
        void OnSeePlayer(Vector3 lastPlayerPosition);
        void PlayerHidden();
    }
}
