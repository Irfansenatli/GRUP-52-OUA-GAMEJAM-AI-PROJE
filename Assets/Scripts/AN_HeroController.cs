using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AN_HeroController : MonoBehaviour
{
    [Tooltip("Character settings (rigid body)")]
    public float MoveSpeed = 30f, JumpForce = 200f, Sensitivity = 70f;
    bool jumpFlag = true; // to jump from surface only

    CharacterController character;
    Rigidbody rb;
    Vector3 moveVector;
    public GameObject[] AI;
    public GameObject[] Dedeler;
    public GameObject[] KonuşmaTuşları;
    public GameObject[] Kapılar;
    public GameObject TeleportLoc;
    public GameObject[] Notlar;
    public bool isMouseMoving;
    Transform Cam;
    float yRotation;

    void Start()
    {
        character = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cam = Camera.main.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // invisible cursor
        isMouseMoving = true;

     
    }

    void Update()
    {
        

        if (isMouseMoving){

            float xmouse = Input.GetAxis("Mouse X") * Time.deltaTime * Sensitivity;
            float ymouse = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensitivity;
            transform.Rotate(Vector3.up * xmouse);
            yRotation -= ymouse;
            yRotation = Mathf.Clamp(yRotation, -85f, 60f);
            Cam.localRotation = Quaternion.Euler(yRotation, 0, 0);
        }

        if (ApiActivate.hasAnsweredCorrectly)
        {
            Kapılar[0].SetActive(false);
        }

        if (ApiActivate2.hasAnsweredCorrectly)
        {
            Kapılar[1].SetActive(false);
        }

        if (ApiActivate3.hasAnsweredCorrectly)
        {
            Kapılar[2].SetActive(false);
        }
        if (ApiActivate4.hasAnsweredCorrectly)
        {
            Kapılar[3].SetActive(false);
        }
    }

    void FixedUpdate()
    {
        // body moving
        moveVector = transform.forward * MoveSpeed * Input.GetAxis("Vertical") +
            transform.right * MoveSpeed * Input.GetAxis("Horizontal") +
            transform.up * rb.velocity.y;
        rb.velocity = moveVector;

        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Teleport"))
        {
            SceneManager.LoadScene(1);
        }

        if (other.gameObject.CompareTag("Dede1"))
        {
            KonuşmaTuşları[0].SetActive(true);      
        }

        if (other.gameObject.CompareTag("Dede2"))
        {
            KonuşmaTuşları[1].SetActive(true);
        }

        if (other.gameObject.CompareTag("Dede3"))
        {
            KonuşmaTuşları[2].SetActive(true);
        }

        if (other.gameObject.CompareTag("Dede4"))
        {
            KonuşmaTuşları[3].SetActive(true);
        }
        

        if (other.gameObject.CompareTag("Kitap"))
        {
            Notlar[0].SetActive(true);

            BoxCollider eklenenCollider = Dedeler[0].AddComponent<BoxCollider>();      
            eklenenCollider.isTrigger = true;
            eklenenCollider.center = new Vector3(0, 1, 1);
            eklenenCollider.size = new Vector3(2, 2, 2);
            isMouseMoving = false;
        }

        if (other.gameObject.CompareTag("kitap2"))
        {
            Notlar[1].SetActive(true);
            BoxCollider eklenenCollider = Dedeler[1].AddComponent<BoxCollider>();
            eklenenCollider.isTrigger = true;
            eklenenCollider.center = new Vector3(0, 1, 1);
            eklenenCollider.size = new Vector3(2, 2, 2);
            isMouseMoving = false;
        }

        if (other.gameObject.CompareTag("kitap3"))
        {
            Notlar[2].SetActive(true);
            BoxCollider eklenenCollider = Dedeler[2].AddComponent<BoxCollider>();
            eklenenCollider.isTrigger = true;
            eklenenCollider.center = new Vector3(0, 1, 1);
            eklenenCollider.size = new Vector3(2, 2, 2);
            isMouseMoving = false;
        }

        if (other.gameObject.CompareTag("kitap4"))
        {
            Notlar[3].SetActive(true);
            BoxCollider eklenenCollider = Dedeler[3].AddComponent<BoxCollider>();
            eklenenCollider.isTrigger = true;
            eklenenCollider.center = new Vector3(0, 1, 1);
            eklenenCollider.size = new Vector3(2, 2, 2);
            isMouseMoving = false;
        }



    }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dede1"))
        {
            if (Input.GetKeyDown((KeyCode.E)))
            {
                moveVector = Vector3.zero;
                AI[0].SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isMouseMoving = false;
            }
            
        }

        if (other.gameObject.CompareTag("Dede2"))
        {
            if (Input.GetKeyDown((KeyCode.E)))
            {
                moveVector = Vector3.zero;
                AI[1].SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isMouseMoving = false;
            }

        }

        if (other.gameObject.CompareTag("Dede3"))
        {
            if (Input.GetKeyDown((KeyCode.E)))
            {
                moveVector = Vector3.zero;
                AI[2].SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isMouseMoving = false;
            }

        }

        if (other.gameObject.CompareTag("Dede4"))
        {
            if (Input.GetKeyDown((KeyCode.E)))
            {
                moveVector = Vector3.zero;
                AI[3].SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isMouseMoving = false;
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Dede1"))
        {
            KonuşmaTuşları[0].SetActive(false);
            AI[0].SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isMouseMoving = true;
        }

        if (other.gameObject.CompareTag("Dede2"))
        {
            KonuşmaTuşları[1].SetActive(false);
            AI[1].SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isMouseMoving = true;
        }


        if (other.gameObject.CompareTag("Dede3"))
        {
            KonuşmaTuşları[2].SetActive(false);
            AI[2].SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isMouseMoving = true;
        }

        if (other.gameObject.CompareTag("Dede4"))
        {
            KonuşmaTuşları[3].SetActive(false);
            AI[3].SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isMouseMoving = true;
        }




        if (other.gameObject.CompareTag("Kitap"))
        {
            Notlar[0].SetActive(false);
            isMouseMoving = true;

        }
        if (other.gameObject.CompareTag("kitap2"))
        {
            Notlar[1].SetActive(false);
            isMouseMoving = true;
        }
        if (other.gameObject.CompareTag("kitap3"))
        {
            Notlar[2].SetActive(false);
            isMouseMoving = true;
        }
        if (other.gameObject.CompareTag("kitap4"))
        {
            Notlar[3].SetActive(false);
            isMouseMoving = true;
        }
    }





  
}
