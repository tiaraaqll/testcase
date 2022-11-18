using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    //static akan membuat variabel ini saling terbagi pada semua tree
    public static List <Vector3> AllPositions = new List <Vector3> ();

    //mendaftarkan diri semua pohon
    public void OnEnable() {
        AllPositions.Add(this.transform.position);
        Debug.Log(AllPositions.Count); //untuk mengetahui sudah dijalankan apa belum daftar pohon
    }

    public void OnDisable() {
        AllPositions.Remove(this.transform.position);
    }
}
