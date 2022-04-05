using _06_Scripts._01_Manager;
using UnityEngine;

namespace _06_Scripts {
    public class InvisibleWall : MonoBehaviour {
        [SerializeField] private BattleWave combatSystem;
        [SerializeField] private GameObject leftGate;
        [SerializeField] private GameObject rightGate;

        private void Start() {
            combatSystem.BattleStart += BattleSystem_BattleStart;
            combatSystem.BattleOver += BattleSystem_BattleOver;
        }
        
        private void BattleSystem_BattleStart(object sender, System.EventArgs e){
            rightGate.SetActive(true);
            leftGate.SetActive(true);
        }

        private void BattleSystem_BattleOver(object sender, System.EventArgs e){
            rightGate.SetActive(false);
            rightGate.SetActive(false);
        }
    }
}
