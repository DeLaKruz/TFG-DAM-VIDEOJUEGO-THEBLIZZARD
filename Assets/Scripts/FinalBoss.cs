using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public static FinalBoss instance;
    public Transform leftAttackPoint, rightAttackPoint; // Puntos que definen el rango de ataque del enemigo.
    private bool movingright; // Indica si el enemigo se está moviendo hacia la derecha.
    public bool endAnim; // Indica si la animación de fin ha terminado.
    private Rigidbody2D theRB; // Referencia al Rigidbody2D del enemigo.
    public SpriteRenderer theSR; // Referencia al SpriteRenderer del enemigo.
    public int enemyLife = 1; // Vida del enemigo.
    private Animator anim; // Referencia al Animator del enemigo.
    public bool endBattleText = true;
    public bool canStartBattle;

    public Collider2D punchCollider; // Collider usado para los ataques del enemigo.
    private Vector2 attackTarget; // Objetivo del ataque del enemigo.
    public float distanceToAtackPlayer, chaseSpeed; // Distancia para atacar al jugador y velocidad de persecución.
    public float waitAfterAttack; // Tiempo de espera después de un ataque.
    private float initialZ; // Posición inicial en el eje Z.
    private float attackCounter; // Contador para el tiempo de espera después de un ataque.
    public GameObject bossJail;


    void Awake()
    {
        instance = this;
        endBattleText = true;
    }

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Desvincula los puntos de ataque y movimiento del enemigo para que no se muevan con él.
        leftAttackPoint.parent = null;
        rightAttackPoint.parent = null;

        endAnim = false; // Inicializa endAnim como falso.

        movingright = true; // Inicializa el movimiento hacia la derecha.

        initialZ = transform.position.z; // Guarda la posición inicial en el eje Z.
    }

    void Update()
    {
        // Controla la activación del enemigo según el estado de endAnim.
        theRB.gameObject.SetActive(!endAnim);

        // Maneja el tiempo de espera después de un ataque.
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
            anim.SetBool("canAttack", false);
            anim.SetBool("isAttacking", false);
        }
        else
        {
            AttackPlayer();

        }

        // Mantiene la posición Z constante para evitar movimientos raros en el eje Z.
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
    }

    // Método para manejar el ataque del enemigo al jugador.
    private void AttackPlayer()
    {
        if (endBattleText && canStartBattle)
        {
            bossJail.SetActive(true);
            anim.SetBool("isAttacking", true);
            theRB.velocity = Vector2.zero;

            // Si no hay objetivo de ataque, lo establece en la posición del jugador.
            if (attackTarget == Vector2.zero)
            {
                attackTarget = PlayerController.instance.transform.position;
            }

            // Ajusta la posición del objetivo de ataque.
            attackTarget.y = transform.position.y;
            attackTarget.x = Mathf.Clamp(attackTarget.x, leftAttackPoint.position.x, rightAttackPoint.position.x);

            // Ajusta la dirección del enemigo según la posición del objetivo.
            if (attackTarget.x > transform.position.x)
            {
                theSR.flipX = false;
                FlipPunchColliderToRight();
            }
            else
            {
                theSR.flipX = true;
                FlipPunchColliderToLeft();
            }

            // Mueve al enemigo hacia el objetivo de ataque.
            transform.position = Vector2.MoveTowards(transform.position, attackTarget, chaseSpeed * Time.deltaTime);

            // Si el enemigo está cerca del objetivo, detiene el ataque.
            if (Vector2.Distance(transform.position, attackTarget) <= .1f)
            {
                anim.SetBool("canAttack", true);
                attackTarget = Vector2.zero;
                attackCounter = waitAfterAttack;
            }
        }
    }

    // Método para voltear el collider de ataque hacia la izquierda.
    void FlipPunchColliderToLeft()
    {
        if (theRB != null && punchCollider != null)
        {
            Vector2 originalOffset = punchCollider.offset;
            punchCollider.offset = new Vector2(-Mathf.Abs(originalOffset.x), originalOffset.y);
        }
    }

    // Método para voltear el collider de ataque hacia la derecha.
    void FlipPunchColliderToRight()
    {
        if (theRB != null && punchCollider != null)
        {
            Vector2 originalOffset = punchCollider.offset;
            punchCollider.offset = new Vector2(Mathf.Abs(originalOffset.x), originalOffset.y);
        }
    }
}
