using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;//animal ini yang ada dalam scene bukan prefab
    [SerializeField] Vector3 offset; //perbadaan jarak kamera dengan animal

    private void Start () {
        offset = this.transform.position - player.transform.position;
    }

    //menambahkan performan
    Vector3 lastAnimalPos;

    void Update()
    {
        //ketika sama frame sebelumnya, maka animal tidak akan bergerak
        if(player.IsDie || lastAnimalPos == player.transform.position) //biar kamera ga follow animal
            return;

        //kamera relatif terhadap posisi animal
        var targetAnimalPos = new Vector3(
            player.transform.position.x ,
            0,
            player.transform.position.z
        );

        transform.position = targetAnimalPos + offset;

        lastAnimalPos = player.transform.position;
    }
}
