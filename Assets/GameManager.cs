using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    //menentukan parameter permainan
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel; //tipe data untuk memanggil game objek
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent = 7; //untuk menentukan langkah hewan
    [SerializeField] int frontDistance = 10; //agar jalan bisa ke depany
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;

    Dictionary <int, TerrainBlock> map = new Dictionary<int, TerrainBlock> (50);//tipe data untuk menyimpan gameobject
    TMP_Text gameOverText;

   private void Start () {

    //untuk mengaktifkan panel game over
    gameOverPanel.SetActive(false);
    gameOverText = gameOverPanel.GetComponentInChildren <TMP_Text> ();

    //membuat grass (belakang)
        for(int z = backDistance; z <= 0; z++) {
            CreateTerrain(grass, z);
        }

    //membuat road (depan)
        for (int z = 1; z <= frontDistance; z++) {
            
            //penentuan terrain block dengan probabilitas 50%
            var prefab = GetNextRandomTerrainPrefab(z);

            //instantiate blocknya
            CreateTerrain(prefab, z);

    //kebanyakan game menggunakan Quarternion.identity dibandingkan Euler
        }

    //mensetting player
        player.SetUp(backDistance, extent);
   }    

   private int playerLastMaxTravel;
   private void Update () {
        //cek player masih hidup atau ngga
         if(player.IsDie && gameOverPanel.activeInHierarchy==false) 
             StartCoroutine (ShowGameOverPanel());
        
        //infinite terrain system, posisi maksimum bebek
        if(player.MaxTravel == playerLastMaxTravel)
            return;
        
        playerLastMaxTravel = player.MaxTravel;

        //bikin ke depan block jadi banyak
        var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance);
        CreateTerrain(randTbPrefab, player.MaxTravel + frontDistance);

        //hapus yang di belakang
        //cara1 mudah
        var lastTB = map[player.MaxTravel - 1 + backDistance];
        
        //cara2 susah
        // TerrainBlock lastTB = map[player.MaxTravel + frontDistance];
        // int lastPos = player.MaxTravel;
        // foreach(var (pos, tb) in map) {
        //     if(pos < lastPos) {
        //         lastPos = pos;
        //         lastTB = tb;
        //     }
        // }

        //remove dari map (hapus data daftar dari map)
        //map menyimpan daftar semua yang ada di scene yang masih aktif. Daftar punya posisi (x,y,z)
        map.Remove(player.MaxTravel -1 + backDistance);

        //menghilangkan dari scene
        Destroy(lastTB.gameObject);

        //setup lagi biar player gerakin animal ngga balik ke belakang
        player.SetUp(player.MaxTravel + backDistance, extent);        
   }

   IEnumerator ShowGameOverPanel() {
        yield return new WaitForSeconds(3); //yield return = keluar function, return 3 detik akan lanjut kebawah
        Debug.Log("Game Over");
        gameOverText.text = "YOUR SCORE : "+player.MaxTravel;
        gameOverPanel.SetActive(true);
   }

    private void CreateTerrain (GameObject prefab, int zPos) {
        var go = Instantiate(prefab, new Vector3 (0,0,zPos), Quaternion.identity); //Quarternion.identity = representasi orientasi yang lebih performen perhitungannya
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);
        
        map.Add(zPos, tb);
        //cara1
        // Debug.Log(map [zPos].GetType());

        //cara2 (bernilai true / false)
        Debug.Log(map [zPos] is Road);
    }

    private GameObject GetNextRandomTerrainPrefab (int nextPos) 
    {    
        bool isUniform = true;
        var tbRef = map[nextPos - 1];//posisi terakhir paling ujung yang diambil

        for(int distance = 2; distance <= maxSameTerrainRepeat; distance++) 
        {
            if(map[nextPos - distance].GetType() != tbRef.GetType()) 
            {
                isUniform = false;
                break;
            }
        }

        if(isUniform) {
            if(tbRef is Grass) 
                return road;
            else
                return grass;
        }
        
        //penentuan terrain block dengan probabilitas 50%
        return Random.value > 0.5f ? road : grass; //menghasilkan range probabilitas (0-1)
    }  
}

//rigidbodi ditaruh di player untuk triggered collider animal  (interaksi animal dengan car)
