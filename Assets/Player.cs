using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;//menambahkan kalimat steps
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.5f;
    private int minZPos;
    private int extent;
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    [SerializeField] private int maxTravel;
    public int MaxTravel {get => maxTravel;}//membuat skor
    [SerializeField] private int currentTravel;
    public int CurrentTravel {get => currentTravel;}
    public bool IsDie {get => this.enabled == false;}

    public void SetUp (int minZPos, int extent) {
        backBoundary = minZPos - 1;
        leftBoundary = - (extent + 1);
        rightBoundary = extent + 1;
    }

    private void Update () { 
    //memasukkan input per frame per 1 klik button (1 klik = 1 perintah frame)
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        //     Debug.Log("forward");//buat maju

        // if (Input.GetKeyDown(KeyCode.DownArrow))
        //     Debug.Log("back");//buat mundur

    //memasukkan input setiap frame per 1 klik button (sekali klik perintah frame banyak)
        // if (Input.GetKey(KeyCode.UpArrow))
        //     Debug.Log("forward");//buat maju

        //  if (Input.GetKey(KeyCode.DownArrow))
        //     Debug.Log("back");//buat mundur

    //Agar si kucing bisa kiri kanan
        var moveDir = Vector3.zero;//membuat vector 2 (x,y = 0)

        //x = kanan kiri, y = maju mundur
        if (Input.GetKey(KeyCode.UpArrow))
            moveDir += new Vector3(0,0,1);

        else if (Input.GetKey(KeyCode.DownArrow))
            moveDir += new Vector3(0,0,-1);

        //jalan kanan kiri
        else if (Input.GetKey(KeyCode.RightArrow))
            moveDir += new Vector3(1,0,0);

        else if (Input.GetKey(KeyCode.LeftArrow))
            moveDir += new Vector3(-1,0,0);
        
        //tidak ingin jump ketika tidak ada input
        if(moveDir == Vector3.zero)
            return;

        if(isJumping()==false)//ngga kepengen dia loncat pas lagi loncat
        Jump(moveDir); //biar bisa gerak setiap frame
   }

   //membuat movement
       private void Jump (Vector3 targetDirection) {
            //var targetDirection = new Vector3(targetDirection.x,0,targetDirection.y); //(x,y,z)

            //Atur rotasi
            var TargetPosition = transform.position + targetDirection;
            transform.LookAt(TargetPosition);//melihat target dengan arah muka selalu sumbu z

            //cara1 loncat
            // transform.DOMoveY(2f, 0.1f)
            //     .OnComplete( () => transform.DOMoveY(0, 0.1f));//loncat keatas

            //cara2 loncat

            //loncst keatas
            var moveSeq = DOTween.Sequence(transform);
            moveSeq.Append(transform.DOMoveY(2f, jumpHeight/2));//untuk durasi gerak agar selesai berbarengan dengan kiri kanan
            moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

            //mengecek target
            if( TargetPosition.z <= backBoundary ||
                TargetPosition.x <= leftBoundary ||
                TargetPosition.x >= rightBoundary)
                return;

            //biar animal ngga nabrak pohon
            if(Tree.AllPositions.Contains(TargetPosition))
                return;

            //gerak maju/mundur/samping
            transform.DOMoveX(TargetPosition.x, moveDuration);//kecepatan bergerak
            transform.
                DOMoveZ(TargetPosition.z, moveDuration)
                .OnComplete(UpdateTravel); 
       }
        //perhitungan skor
      private void UpdateTravel () {
        currentTravel = (int) this.transform.position.z;
        if(currentTravel > maxTravel)
            maxTravel = currentTravel;

        //menambahkan kata-kata step
        stepText.text = "STEP : " + maxTravel.ToString();
      }

    //lagi loncat atau ngga
      public bool isJumping () {
            return DOTween.IsTweening(transform);
      }

      // OnTriggerEnter is called when the Collider other enters the trigger.
      private void OnTriggerEnter(Collider other) {//other = nabrak, collider = collider yang ditambahkan
        //di execute sekali pada frame ketika nempel pertama kali

        // var car = other.GetComponent <Car>();
        // if(car != null) {//membuat tabrakan
        //     Debug.Log("Enter" + car.name);
        // }
        if(other.tag == "Car") {
            AnimateCrash();
        }
      }

        private void AnimateCrash () {
            //biar kucing atasnya gepeng
            transform.DOScaleY(0.1f, -0.27f);
            //biar kucing sampingnya gepeng
            transform.DOScaleX(3f, 0.2f);
            transform.DOScaleZ(2f, 0.2f);

            //ketika udah gepeng gabisa digerakin
            this.enabled=false;
        }

      private void OnTriggerStay(Collider other) {
        //di execute setiap frame selama masih nempel
        //Debug.Log("Stay");
      }

      private void OnTriggerExit(Collider other) {
        //di execute sekali pada frame ketika tidak nempel
        //Debug.Log("Exit");
      }

}
