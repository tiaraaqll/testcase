using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBlock : MonoBehaviour
{
   [SerializeField] GameObject main;
   [SerializeField] GameObject repeat;
   private int extent;
   public int Extent {get => extent;}

 public void Build (int extent) {
    this.extent=extent; //sebelah kiri ambil yang private int extent

        // //cara1 buat block batas kiri kanan
        // //posisi kiri
        // m.transform.SetParent(this.transform);
        // m.transform.localPosition = new Vector3(-(extent+1),0,0);
        // m.transform.GetComponentInChildren<Renderer>().material.color = Color.grey;

        // //posisi kanan
        // m.transform.SetParent(this.transform);
        // m.transform.localPosition = new Vector3((extent+1),0,0);
        // m.transform.GetComponentInChildren<Renderer>().material.color = Color.grey;

        //cara2 buat block batas kiri kanan
        for(int i = -1; i <= 1 ; i++) {
            if(i==0) {
                continue;
            }
            var m = Instantiate(main);
            m.transform.SetParent(this.transform);
            m.transform.localPosition = new Vector3((extent+1)*i,0,0);
            m.transform.GetComponentInChildren<Renderer>().material.color *= Color.grey;//ubah warna grass, untuk warna gelap itu di *=
        }
    
    //buat block utama
    main.transform.localScale = new Vector3 (
        x : extent * 2 + 1, 
        y : main.transform.localScale.y,
        z : main.transform.localScale.z
    );  //simetris ganjil

    //membuat repeat jika ada
    if(repeat==null) {
        return;
    }

    //untuk membuat garis jalan di block abu-abu
    for (int x = -(extent+1); x <= extent+1; x++) {

        //membuat clone repeat extent
        if(x==0){
            continue;
        }

        //Instantiate(repeat, new Vector3 (x,0,0), Quaternion.identity);

        //untuk repeat blok jalan
        var r = Instantiate(repeat);//method overrriding
        r.transform.SetParent(this.transform);
        r.transform.localPosition = new Vector3(x,0,0);        
    }
  }
}
