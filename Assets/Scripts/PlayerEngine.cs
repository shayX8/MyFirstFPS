using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEngine : MonoBehaviour
{
    Text win;
    int enemys, flipCount;
    float mouseX, mouseY, speed, cameraX, dirX, dirZ, moveSpeed ,runSpeed, gravity, maxDis;
    bool isGround, isFlip;
    public Transform cameraTRN;
    public GameObject playerCamera;
    public GameObject playerLight;

    CharacterController chCo;
    Vector3 v3, velosity, origin;

    public Transform ShoeTRN;
    public LayerMask groundLayerMask;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        //Locking the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        speed = 300;
        cameraX = 0;
        moveSpeed = 10;
        runSpeed = 12;
        chCo = GetComponent<CharacterController>();
        isGround = false;
        gravity = -9.81f;
        maxDis = 150f;
        enemys = 12;
        win = FindObjectOfType<Text>();
        isFlip = false;
    }

    // Update is called once per frame
    void Update()
    {
        //unlock the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        //camera movement
        mouseX = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        transform.Rotate(0, mouseX, 0);
        mouseY = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
        cameraX -= mouseY;
        cameraX = Mathf.Clamp(cameraX, -60, 60);
        cameraTRN.localRotation = Quaternion.Euler(cameraX, 0, 0);

        //charecter movement
        dirX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        dirZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        v3 = transform.forward * dirZ + transform.right * dirX;
        chCo.Move(v3);

        //chek ground
        if (Physics.CheckSphere(ShoeTRN.position, 0.5f, groundLayerMask))
            isGround = true;
        else
            isGround = false;

        //move to ground
        if(!isGround)
            velosity.y += gravity * Time.deltaTime;
        else
            velosity.y = 0;

        //jump
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Sound.PlaySound("jump");
            velosity.y += 6;
        }

        //flip
        if (Input.GetKeyDown(KeyCode.Space) && !isGround && !isFlip)
        {
            isFlip = true;
            Sound.PlaySound("flip");
        }
        if(isFlip)
        {
            transform.Rotate(8, 0, 0);
            flipCount += 8;
            if(flipCount >= 360)
            {
                isFlip = false;
                flipCount = 0;
                transform.rotation = Quaternion.Euler(new Vector3(0, transform.localRotation.eulerAngles.y, 0));
            }
        }

        // run
        if (Input.GetKeyDown(KeyCode.LeftShift))
            moveSpeed += runSpeed;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            moveSpeed -= runSpeed;

        //light
        if (Input.GetKeyDown(KeyCode.V))
        {
            Sound.PlaySound("swich");
            if (playerLight.active)
                playerLight.SetActive(false);
            else
                playerLight.SetActive(true);
        }


        //player move in the game
        chCo.Move(velosity * Time.deltaTime);

        //shooting
        if(Input.GetMouseButtonDown(0))
        {
            Sound.PlaySound("shoot");
            origin = playerCamera.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            if(Physics.Raycast(origin,playerCamera.transform.forward,out hit,maxDis))
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    Sound.PlaySound("enemyDead");
                    Destroy(hit.transform.parent.gameObject);
                    enemys--;
                }
            }
        }
        if(enemys == 0)
        {
            Sound.PlaySound("win");
            win.text = "YOU WIN!!!";
        }
    }
}
