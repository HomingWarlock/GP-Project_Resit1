using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    private float move_speed;
    private float run_speed;
    private float jump_speed;
    private float X_value;
    private float Z_value;
    private bool is_walking;
    private bool is_running;
    private bool is_jumping;
    private bool is_double_jumping;
    public bool grounded;
    private bool jump_input_check;
    public bool attack_input_check;
    public bool single_attack_check;
    private Vector3 back_dir;
    private Vector3 right_dir;
    private Vector3 true_dir;
    private GameObject cam_point;

    public int coins;
    private float boosted_speed;
    public bool extra_jump;
    public bool collected_Speed;
    public bool collected_Jump;
    public GameObject speed_effect;
    public GameObject jump_effect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();

        move_speed = 500;
        run_speed = 0;
        jump_speed = 10;
        X_value = 0;
        Z_value = 0;
        is_walking = false;
        is_running = false;
        is_jumping = false;
        is_double_jumping = false;
        grounded = false;
        jump_input_check = false;
        attack_input_check = false;
        single_attack_check = false;
        cam_point = GameObject.Find("CamPoint");

        coins = 0;
        boosted_speed = 0;
        extra_jump = false;
        collected_Speed = false;
        collected_Jump = false;
        speed_effect = GameObject.Find("Speed Effect");
        speed_effect.SetActive(false);
        jump_effect = GameObject.Find("Jump Effect");
        jump_effect.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKey(KeyCode.X))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        back_dir = Input.GetAxisRaw("Vertical") * cam_point.transform.forward;
        right_dir = Input.GetAxisRaw("Horizontal") * cam_point.transform.right;
        true_dir = back_dir + right_dir;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            is_walking = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            is_walking = false;
        }

        if (Input.GetAxisRaw("Horizontal") > 0.05f)
        {
            X_value = 1;
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.05f)
        {
            X_value = -1;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0.05f && Input.GetAxisRaw("Horizontal") > -0.05f)
        {
            X_value = 0;
        }

        if (Input.GetAxisRaw("Vertical") > 0.05f)
        {
            Z_value = 1;
        }
        else if (Input.GetAxisRaw("Vertical") < -0.05f)
        {
            Z_value = -1;
        }
        else if (Input.GetAxisRaw("Vertical") < 0.05f && Input.GetAxisRaw("Vertical") > -0.05f)
        {
            Z_value = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            is_running = true;
            run_speed = 500;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.JoystickButton3))
        {
            is_running = false;
            run_speed = 0;
        }

        anim.SetFloat("XValue", X_value);
        anim.SetFloat("ZValue", Z_value);
        anim.SetBool("isRunning", is_running);
        anim.SetBool("isJumping", is_jumping);
        anim.SetBool("isGrounded", grounded);
        anim.SetBool("isDoubleJumping", is_double_jumping);

        if (collected_Speed)
        {
            boosted_speed = 500;
        }
        else if (!collected_Speed)
        {
            boosted_speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0) && !jump_input_check)
        {
            jump_input_check = true;
            StartCoroutine(JumpInputDelay());

            if (collected_Jump)
            {
                if (!grounded && extra_jump)
                {
                    extra_jump = false;
                    is_double_jumping = true;
                    rb.velocity = new Vector3(rb.velocity.x, jump_speed, rb.velocity.z);
                }
                else if (grounded)
                {
                    grounded = false;
                    is_jumping = true;
                    rb.velocity = new Vector3(rb.velocity.x, jump_speed, rb.velocity.z);
                }
            }
            else
            {
                if (grounded)
                {
                    grounded = false;
                    is_jumping = true;
                    rb.velocity = new Vector3(rb.velocity.x, jump_speed, rb.velocity.z);
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && !attack_input_check)
        {
            attack_input_check = true;
            StartCoroutine(AttackInputDelay());
        }

    }

    private void FixedUpdate()
    {
        if (is_walking)
        {
            rb.velocity = new Vector3(true_dir.x * (move_speed + run_speed + boosted_speed) * Time.deltaTime, rb.velocity.y, true_dir.z * (move_speed + run_speed + boosted_speed) * Time.deltaTime);
        }
        else if (!is_walking)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private IEnumerator JumpInputDelay()
    {
        yield return new WaitForSeconds(0.2f);
        jump_input_check = false;
    }

    private IEnumerator AttackInputDelay()
    {
        yield return new WaitForSeconds(0.2f);
        attack_input_check = false;
        single_attack_check = false;
    }
}
