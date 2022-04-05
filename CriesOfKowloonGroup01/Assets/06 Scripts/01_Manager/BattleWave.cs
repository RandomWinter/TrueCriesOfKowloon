using System;
using _06_Scripts._04_Enemy;
using UnityEngine;

namespace _06_Scripts._01_Manager {
    
    
    public class BattleWave : MonoBehaviour {
        [Serializable]
        private class Wave {
            [SerializeField] private MeleeCombat[] cEnemy;
            [SerializeField] private float timer;

            public void Update(){
                if (timer > 0){
                    timer -= Time.deltaTime;
                    if (timer <= 0){
                        SpawnEnemies();
                    }
                }
            }
            
            //! Spawn Enemy
            private void SpawnEnemies(){
                foreach (MeleeCombat cEnemy in cEnemy){
                    //enemySpawn.Spawn();
                }
            }

            public bool IsWaveOver(){
                if (timer < 0){
                    foreach (MeleeCombat check in cEnemy){
                        if (!check.isDead){
                            return false;
                        }
                    }
                    return true;
                } else  {
                    return false;
                }
            }
        }
        
        public event EventHandler BattleStart;
        public event EventHandler BattleOver;
        
        private enum WaveState{
            Idle, Active, Finish
        }
        private WaveState _cState;

        [SerializeField] private Wave[] waveArray;
        [SerializeField] private WaveTrigger onTargetEnter;
        
        //! ===============================================

        private void Awake() {
            _cState = WaveState.Idle;
        }

        private void Start(){
            onTargetEnter.OnTargetEnter += Detector;
        }

        private void Update(){
            switch (_cState){
                case WaveState.Active:
                    foreach (Wave w in waveArray){
                        w.Update();
                    }
                    EndBattle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        //! ===============================================

        private void Detector(object sender, EventArgs e){
            if (_cState != WaveState.Idle) return;
            StartBattle();
            onTargetEnter.OnTargetEnter -= Detector;
        }
        
        //! ===============================================

        private void StartBattle(){
            _cState = WaveState.Active;
            BattleStart?.Invoke(this, EventArgs.Empty);
        }

        private void EndBattle(){
            if (_cState == WaveState.Active){
                if (IsBattleOver()) {
                    _cState = WaveState.Finish;
                    BattleOver?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
        private bool IsBattleOver(){
            foreach (Wave w in waveArray){
                if (w.IsWaveOver()){
                    //! Wave has finish
                } else {
                    return false;
                }
            }
            
            return true;
        }
    }
}
