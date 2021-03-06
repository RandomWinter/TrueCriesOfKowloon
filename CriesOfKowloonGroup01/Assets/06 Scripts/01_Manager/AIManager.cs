using System.Collections.Generic;
using _06_Scripts._04_Enemy;
using UnityEngine;

public class AIManager : MonoBehaviour {
    [Header("Manager Component")]
    [SerializeField] public List<GameObject> enemyMember;
    [SerializeField] private int numEnemy = 1;

    public enum ManagerState{
        Grooming, InCombat
    } [SerializeField] public ManagerState currentStage;

    public void Awake(){
        enemyMember.AddRange(GameObject.FindGameObjectsWithTag("MeleeEnemy"));
        currentStage = ManagerState.Grooming;
    }

    public void Update(){
        switch(currentStage){
            case ManagerState.Grooming:
                Grooming();
                break;
            case ManagerState.InCombat:
                InCombat();
                break;
        }
    }
    
    private void Grooming() {
        if (!NoOneIsFighting()){
            currentStage = ManagerState.InCombat;
        }
    } 

    private void InCombat(){
        foreach (var c in enemyMember) {
            if (c.GetComponent<EnemyBehavior>().sawPlayer && !c.GetComponent<EnemyBehavior>().prepareForAttack && numEnemy < 2) {
                c.GetComponent<EnemyBehavior>().prepareForAttack = true;
                numEnemy++;
            }

            if (c.GetComponent<EnemyBehavior>().isDead) {
                if (c.GetComponent<EnemyBehavior>().prepareForAttack){
                    numEnemy--;
                }
                enemyMember.Remove(c);
                c.gameObject.SetActive(false);
            }
        }

        //IF ALL ENEMY DIDN'T WITNESS PLAYER RESET TO GROOMING
        if (NoOneIsFighting()) {
            currentStage = ManagerState.Grooming;
        }
    }

    private bool NoOneIsFighting() {
        foreach (var d in enemyMember){
            if (d.GetComponent<EnemyBehavior>().sawPlayer){
                return false;
            }
        }
        return true;
    }
}
