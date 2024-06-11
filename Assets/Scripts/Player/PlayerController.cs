using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Movimiento")]
    public float movespeed; // Velocidad de movimiento del jugador.

    [Header("Salto")]
    public float jumpforce; // Fuerza del salto del jugador.

    [Header("Componentes")]
    public Rigidbody2D hero; // Componente Rigidbody2D del jugador.
    public Transform punchBox; // Puño del jugador (Es como su arma)

    [Header("GroundCheck")]
    public bool isGrounded; // Indica si el jugador está en el suelo.
    public bool isWalking;
    public Transform groundCkeckpoint; // Punto de chequeo para el suelo.
    public LayerMask whatIsGorund; //define qué es considerado suelo.

    [Header("Animator")]
    public Animator anim;
    private SpriteRenderer spriterd;

    // Variables para el efecto de knockback.
    public float knockbackLenght, knockbackForce, bounceForce;
    private float knockbackCounter;

    // Variables para el cooldown del ataque.
    private float CouldDownAttack;
    public float CouldDownAttackMaxValue;

    // Variables para habilidades especiales del jugador.
    public bool doubleJumpPower = false;
    public bool dashpower = false;
    [Header("Dash")]
    public float dashSpeed = 30; // Velocidad del dash.
    private float dashCooldown = 1f; // Cooldown del dash en segundos.
    private float dashCooldownCounter; // Contador de cooldown para el dash.
    private bool isDashing; // Indica si el jugador está haciendo dash.
    private float dashStartY; // Almacena la posición y del jugador al iniciar el dash.


    public bool wallslidepower = false;
    public bool isDead;

    private bool hasDoubleJumped; // Indica si el jugador ha realizado un doble salto en el aire.
    private bool doubleJumpAvailable; // Indica si el jugador puede realizar un doble salto en el aire.
    public bool dash = false; // Indica si el jugador está realizando un dash.
    public bool wallslide = false; // Indica si el jugador está deslizándose por una pared.
    bool endBattleText = true;

    private void Awake()
    {
        instance = this;
        //FinalBoss.instance.endBattleText = true;
    }

    // Método llamado al inicio.
    void Start()
    {
        hero = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriterd = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int indiceDeLaEscena = SceneManager.GetActiveScene().buildIndex;
        if (indiceDeLaEscena == 5)
        {
            endBattleText = FinalBoss.instance.endBattleText;
        }
        if (endBattleText && !isDead)
        {
            anim.SetBool("idle", false);
            // Si no está pausado el juego.
            // ! if (!PauseMenu.instance.isPaused)
            // ! {
            if (knockbackCounter <= 0)
            {
                CouldDownAttack -= Time.deltaTime; // Actualizar el cooldown del ataque.
                dashCooldownCounter -= Time.deltaTime; // Actualizar el cooldown del dash.

                // Si el jugador no está haciendo dash.
                if (!isDashing)
                {
                    hero.velocity = new Vector2(movespeed * Input.GetAxisRaw("Horizontal"), hero.velocity.y); // Movimiento horizontal del jugador.
                    hero.velocity = new Vector3(hero.velocity.x, hero.velocity.y, transform.position.z); // Mantener la posición en el eje Z.
                }
                else
                {
                    // Mantener la posición y durante el dash.
                    hero.velocity = new Vector2(hero.velocity.x, 0f); // Eliminar la caída mientras hace dash.
                    hero.transform.position = new Vector3(hero.transform.position.x, dashStartY, hero.transform.position.z); // Mantener la posición en el eje Z.
                }

                isGrounded = Physics2D.OverlapCircle(groundCkeckpoint.position, .3f, whatIsGorund); // Verificar si el jugador está en el suelo.

                // Permitir el doble salto después del primer salto en el suelo y reiniciar el estado del doble salto al tocar el suelo.
                if (isGrounded)
                {
                    doubleJumpAvailable = true;
                    hasDoubleJumped = false;
                }

                // Realizar salto.
                if (Input.GetButtonDown("Jump"))
                {
                    if (isGrounded)
                    {
                        hero.velocity = new Vector2(hero.velocity.x, jumpforce);
                        AudioManager.instance.PlaySFX(3); // Reproducir sonido de salto.
                    }
                    // Verificar si tiene el poder de doble salto y no se ha realizado un doble salto en el aire.
                    else if (doubleJumpPower && doubleJumpAvailable && !hasDoubleJumped)
                    {
                        hero.velocity = new Vector2(hero.velocity.x, jumpforce);
                        AudioManager.instance.PlaySFX(3); // Reproducir sonido de salto.
                        hasDoubleJumped = true; // Marcar que se ha realizado un doble salto en el aire.
                    }
                }

                // Realizar dash.
                if (Input.GetKeyDown(KeyCode.K) && dashpower && dashCooldownCounter <= 0)
                {
                    isDashing = true;
                    dashCooldownCounter = dashCooldown;
                    dashStartY = hero.transform.position.y; // Almacenar la posición y al iniciar el dash.
                    AudioManager.instance.PlaySFX(5); // Reproducir sonido de dash.
                    hero.velocity = new Vector2(spriterd.flipX ? -dashSpeed : dashSpeed, 0f); // Aplica la velocidad de dash y elimina la caída.
                    Invoke("StopDash", 0.2f); // Detener el dash después de 0.2 segundos.
                }

                // Cambiar dirección del sprite y realizar sonido al caminar.
                if (hero.velocity.x < 0 && CouldDownAttack <= 0)
                {
                    spriterd.flipX = true;
                    punchBox.localScale = new Vector3(-1, 1, 1);
                    if (isGrounded) AudioManager.instance.PlaySFX(4); // Reproducir sonido de caminar.
                    isWalking = isGrounded;
                }
                else if (hero.velocity.x > 0 && CouldDownAttack <= 0)
                {
                    spriterd.flipX = false;
                    punchBox.localScale = new Vector3(1, 1, 1);
                    if (isGrounded) AudioManager.instance.PlaySFX(4); // Reproducir sonido de caminar.
                    isWalking = isGrounded;
                }
                else
                {
                    isWalking = false;
                }
            }
            else
            {
                knockbackCounter -= Time.deltaTime;
                if (!spriterd.flipX)
                {
                    hero.velocity = new Vector2(-knockbackForce, hero.velocity.y);
                }
                else
                {
                    hero.velocity = new Vector2(knockbackForce, hero.velocity.y);
                }
            }

            anim.SetFloat("moveSpeed", Mathf.Abs(hero.velocity.x)); // Actualizar la animación de movimiento.
            anim.SetBool("isGrounded", isGrounded); // Actualizar la animación de estar en el suelo.
            Attack(); //Llamada a su función de ataque.

            // !}
        }
        else
        {
            anim.SetFloat("movespeed", 0f);
            anim.SetBool("isGrounded", true);
            anim.SetBool("idle", true);
            hero.velocity = new Vector2(0f, 0f);
        }
    }

    // Detener el dash.
    private void StopDash()
    {
        isDashing = false;
    }

    // Aplica un efecto de knockback al jugador.
    public void Knockback()
    {
        knockbackCounter = knockbackLenght; // Establece el contador de knockback.
        hero.velocity = new Vector2(0f, knockbackForce); // Aplica una fuerza de knockback al jugador.
    }

    // Aplica un efecto de rebote al jugador.
    public void Bounce()
    {
        hero.velocity = new Vector2(bounceForce, hero.velocity.y); // Aplica una fuerza de rebote al jugador.
    }

    // Maneja el ataque del jugador.
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1")) // Si se presiona el botón de ataque.
        {
            if (CouldDownAttack <= 0) // Si el cooldown de ataque está completo.
            {
                anim.SetBool("Attack", true); // Activa la animación de ataque.
                AudioManager.instance.PlaySFX(1); // Reproduce el sonido de ataque.
                CouldDownAttack = CouldDownAttackMaxValue; // Reinicia el cooldown de ataque.
            }
        }
        else
        {
            anim.SetBool("Attack", false); // Desactiva la animación de ataque.
        }
    }
}