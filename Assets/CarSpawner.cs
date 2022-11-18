using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] TerrainBlock terrain;
    [SerializeField] float minSpawnDuration = 2;
    [SerializeField] float maxSpawnDuration = 4;
    
    bool isRight;

    float timer;

    private void Start() {
        isRight = Random.value > 0.5f ? true : false;
        timer = Random.Range(minSpawnDuration, maxSpawnDuration);
    }

    private void Update() {
        //buat timer, ketika nilai diaats 0 timer dikurangi setiap update
        if(timer>0) {
            timer -= Time.deltaTime;
            return;
        }

        //akan disimpan disini
        timer = Random.Range(minSpawnDuration, maxSpawnDuration);

        //time out menjalankan perintah ini
        var spawnPos = this.transform.position + 
            Vector3.right * (isRight ? - (terrain.Extent + 1) : terrain.Extent + 1); //bergerak ke kanan, spawn di ujung kiri
        
        var go = Instantiate(
            original : carPrefab, 
            position : spawnPos,
            rotation : Quaternion.Euler(0, isRight ? 90 : -90 ,0), //mobil diputar
            parent : this.transform); //untuk memutar sumbu y

        var car = go.GetComponent <Car> ();
        car.SetUp(terrain.Extent);
    }
}
