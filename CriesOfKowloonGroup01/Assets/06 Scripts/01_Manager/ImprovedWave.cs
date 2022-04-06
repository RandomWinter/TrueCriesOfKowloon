using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _06_Scripts._01_Manager{
    public class ImprovedWave : MonoBehaviour{
        private enum WaveState{
            Idle, Active, Wait, Count, Finish
        } private WaveState _cState;
        
        [SerializeField] private GameObject cEnemy;
        [SerializeField] private GameObject rEnemy;
        public Transform[] spawnPoints;

        [Serializable]
        private class Wave{
            public string waveName;
            public float waveTimer;
            public int count;
        }
        
        private int _nextWave;
        
        private float timeBetweenWaves = 5f;
        private float _searchCountDown = 1f;
        public float waveCountDown;

        [SerializeField] private Wave[] waves;
        [SerializeField] private WaveTrigger targetEnter;
        public event EventHandler BattleBegin;
        public event EventHandler BattleOver;
        
        private void Awake(){
            _cState = WaveState.Idle;
        }   

        private void Start(){
            targetEnter.OnTargetEnter += Founder;
            waveCountDown = timeBetweenWaves;
        }

        private void Update(){
            if (_cState != WaveState.Idle){
                if (_cState == WaveState.Wait){
                    if (!EnemyAlive()){
                        WaveEnd();
                        return;
                    }
                    return;
                }

                if (waveCountDown <= 0 || _cState != WaveState.Finish){
                    if (_cState != WaveState.Active){
                        _cState = WaveState.Active;
                        StartCoroutine(WaveSpawn(waves[_nextWave]));
                    }
                } else {
                    waveCountDown -= Time.deltaTime;
                }
            }
        }

        private void Founder(object sender, EventArgs e){
            if (_cState != WaveState.Idle) return;
            print("Stage 0: Begin");
            targetEnter.OnTargetEnter -= Founder;
            WaveBegin();
        }
        
        private IEnumerator WaveSpawn(Wave w){
            for(int i = 0; i < w.count; i++){
                print("Stage 2: Spawn");
                SpawnEnemy();
                yield return new WaitForSeconds(1f);
            }
            _cState = WaveState.Wait;
        }

        private void WaveBegin(){
            print("Stage 1: Start");
            _cState = WaveState.Count;
            BattleBegin?.Invoke(this, EventArgs.Empty); //! Instruct the Door to Shutdown
        }

        private void WaveEnd(){
            _cState = WaveState.Count;
            if (_nextWave + 1 > waves.Length - 1){
                print("Stage 5: End Battle");
                BattleOver?.Invoke(this, EventArgs.Empty);
                _cState = WaveState.Finish;
            } else {
                print("Next Wave");
                waveCountDown = timeBetweenWaves;
                _nextWave++;
            }
        }

        private bool EnemyAlive(){
            _searchCountDown -= Time.deltaTime;
            if (_searchCountDown <= 0f){
                _searchCountDown = 1f;
                if (GameObject.FindGameObjectWithTag("MeleeEnemy") == null){
                    return false;
                }
            }

            return true;
        }

        void SpawnEnemy(){
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var spawn = Random.Range(0, 2);
            switch (spawn){
                case 0: Instantiate(cEnemy, sp.position, sp.rotation); break;
                case 1: Instantiate(rEnemy, sp.position, sp.rotation); break;
            }
            
        }
    }
}
