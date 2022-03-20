using UnityEditor.SearchService;
using UnityEngine;

namespace _06_Scripts._02_Player {
    public class DeathChecker : MonoBehaviour {
        void Awake(){
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            if (player.Length > 1){
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }
        
        
        
    }
}
