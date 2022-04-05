using System;
using System.Collections;
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
            if (_cState == WaveState.Wait){
                if (EnemyAlive()) return;
                WaveEnd(); return;
            }

            if (waveCountDown <= 0 || _cState != WaveState.Finish){
                if (_cState != WaveState.Active){
                    _cState = WaveState.Active;
                    StartCoroutine(WaveSpawn(waves[_nextWave]));
                }
            } else if(waveCountDown >= 0){
                waveCountDown -= Time.deltaTime;
            }
        }

        private void Founder(object sender, EventArgs e){
            if (_cState != WaveState.Idle) return;
            targetEnter.OnTargetEnter -= Founder;
            WaveBegin();
        }
        
        private IEnumerator WaveSpawn(Wave w){
            for(int i = 0; i < w.count; i++){
                SpawnEnemy();
                yield return new WaitForSeconds(1f);
            }
            
            _cState = WaveState.Wait;
        }

        private void WaveBegin(){
            _cState = WaveState.Active;
            BattleBegin?.Invoke(this, EventArgs.Empty); //! Instruct the Door to Shutdown
        }

        private void WaveEnd(){
            _cState = WaveState.Count;
            if (_nextWave + 1 > waves.Length - 1){
                BattleOver?.Invoke(this, EventArgs.Empty);
                _cState = WaveState.Finish;
            } else {
                waveCountDown = timeBetweenWaves;
                _nextWave++;
            }
        }

        private bool EnemyAlive(){
            _searchCountDown -= Time.deltaTime;
            if (_searchCountDown <= 0f){
                _searchCountDown = 1f;
                if (GameObject.FindGameObjectWithTag("MeleeCombat") == null || GameObject.FindGameObjectWithTag("Ranger") == null){
                    return false;
                }
            }
            
            return true;
        }

        void SpawnEnemy(){
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var spawn = Random.Range(0, 2);
            switch (spawn){
                case 1: Instantiate(cEnemy, sp.position, sp.rotation); break;
                case 2: Instantiate(rEnemy, sp.position, sp.rotation); break;
            }
            
        }
    }
}
