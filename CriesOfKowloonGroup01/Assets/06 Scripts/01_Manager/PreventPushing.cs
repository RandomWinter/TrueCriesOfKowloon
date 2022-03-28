using UnityEngine;

namespace _06_Scripts._01_Manager {
    public class PreventPushing : MonoBehaviour
    {
        public CapsuleCollider2D charCollider;
        public CapsuleCollider2D charBlockerCollider;

        private void Start(){
            Physics2D.IgnoreCollision(charCollider, charBlockerCollider, true);
        }
    }
}
