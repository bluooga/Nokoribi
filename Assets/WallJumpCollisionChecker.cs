using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpCollisionChecker : MonoBehaviour
{

    PlayerController PC;

    void Start(){
        PC = GetComponentInParent<PlayerController>();
    }
 
  void OnTriggerEnter(Collider other){
      if(other.gameObject.CompareTag("Jumpable")){
          if(gameObject.name == "WallCheckRight"){
              Debug.Log("Wall On Right");
              PC.canJump = true;
          }else if(gameObject.name == "WallCheckLeft"){  
              Debug.Log("Wall On Left");
              PC.canJump = true;

            }
        }
    }
}
