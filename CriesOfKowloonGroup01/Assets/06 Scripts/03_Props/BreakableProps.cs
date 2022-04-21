using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _06_Scripts._03_Props {
    public class BreakableProps : MonoBehaviour{
        [Header("Setup Field")]
        [SerializeField] private GameObject healthFood1;
        [SerializeField] private GameObject healthFood2;
        [SerializeField] private GameObject healthFood3;
        public int maxHealth;
        public string sceneName;

        public void Awake(){
            var currentStage = SceneManager.GetActiveScene();
            sceneName = currentStage.name;
        }

         public void ReceivingDamage(int dmg){
            maxHealth -= dmg;
            if(maxHealth <= 0){
                var radNum1 = Random.Range(0, 3);
                var radNum2 = Random.Range(0, 10);
                var radNum3 = Random.Range(0, 12); 
                var foodSelect = Random.Range(0, 3); 
                switch(sceneName){
                   case "level 1-1": case "level 1-2":
                       if (radNum1 == 1)
                           SpawnObject(foodSelect);
                       break;
                   case "lvl 2 - 1": case "lvl 2 - 2":
                       if (radNum2 is >= 0 and <= 4)
                           SpawnObject(foodSelect);
                       break;
                   case "lvl 3 - 1": case "lvl 3 - 2":
                       if (radNum3 is >= 0 and <= 1)
                           SpawnObject(foodSelect);
                       break;
                }
                Destroy(gameObject);
            }
        }

        private void SpawnObject(int select){
            switch(select){
                case 0:
                    Instantiate(healthFood1, transform.position, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(healthFood2, transform.position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(healthFood3, transform.position, Quaternion.identity);
                    break;
            }
        }
    }
}
