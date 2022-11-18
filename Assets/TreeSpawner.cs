using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    //memanggil prefab tree
    [SerializeField] GameObject treePrefab;
    [SerializeField] TerrainBlock terrain;
    [SerializeField] int count = 3;

   private void Start () {
    //List untuk posisi pohon
        List <Vector3> emptyPos = new List <Vector3> ();

        for(int x = - terrain.Extent; x <= terrain.Extent; x++) //untuk posisi random
        {
            //biar pohon ngga respawn deket hewan 
            if(transform.position.z==0 && x==0)
                continue;
            
            emptyPos.Add(transform.position + Vector3.right * x);
        }

        for (int i = 0; i < count; i++)
        {
            var index = Random.Range(0, emptyPos.Count); //memilih index dari list
            var spawnPos = emptyPos [index];  

            Instantiate (
                treePrefab, 
                spawnPos, 
                Quaternion.identity, 
                this.transform); //untuk mengambil gameobject prefab
            emptyPos.RemoveAt(index);//posisi yang terisi sudah dibuang (tidak ada posisi sama)
        }

        //mempercantik ujung kiri kanan

        //ujung kanan
        Instantiate (
            treePrefab,
            transform.position + Vector3.right * - (terrain.Extent + 1), 
            Quaternion.identity, 
            this.transform); 
            
        //ujung kiri
        Instantiate (
            treePrefab, 
            transform.position + Vector3.right * (terrain.Extent + 1), 
            Quaternion.identity, 
            this.transform); 
   }
}
