using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerMS : MonoBehaviour
{
    public float movespeed; // Velocidad de movimiento del enemigo.
    public Transform leftPoint, rightPoint; // Puntos que definen el rango de movimiento del enemigo.
    public Transform leftAttackPoint, rightAttackPoint; // Puntos que definen el rango de ataque del enemigo.
    private bool movingright; // Indica si el enemigo se está moviendo hacia la derecha.
    public bool endAnim; // Indica si la animación de fin ha terminado.
    private Rigidbody2D theRB; // Referencia al Rigidbody2D del enemigo.
    public SpriteRenderer theSR; // Referencia al SpriteRenderer del enemigo.
    public int enemyLife = 1; // Vida del enemigo.
    private Animator anim; // Referencia al Animator del enemigo.

    public Collider2D punchCollider; // Collider usado para los ataques del enemigo.
    public bool enemyWithAtack; // Indica si el enemigo tiene un ataque más especial.
    public bool isAgressive; // Indica si el enemigo es agresivo y atacará al jugador.
    public float moveTime, waitTime; // Tiempo de movimiento y espera del e_nemigo.
    private float moveCount, waitCount; // Contadores para el tiempo de movimiento y espera.
    private Vector2 attackTarget; // Objetivo del ataque del enemigo.
    public float distanceToAtackPlayer, chaseSpeed; // Distancia para atacar al jugador y velocidad de persecución.
    public float waitAfterAttack; // Tiempo de espera después de un ataque.
    private float initialZ; // Posición inicial en el eje Z.
    private float attackCounter; // Contador para el tiempo de espera después de un ataque.

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>(); // Inicializa el Rigidbody2D.
        anim = GetComponent<Animator>(); // Inicializa el Animator.

        // Desvincula los puntos de ataque y movimiento del enemigo para que no se muevan con él.
        leftAttackPoint.parent = null;
        rightAttackPoint.parent = null;

        leftPoint.parent = null;
        rightPoint.parent = null;
        endAnim = false; // Inicializa endAnim como falso.

        movingright = true; // Inicializa el movimiento hacia la derecha.
        moveCount = moveTime; // Inicializa el contador de movimiento.

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
            if (enemyWithAtack)
            {
                anim.SetBool("canAttack", false);
                anim.SetBool("isAttacking", false);
            }
        }
        else
        {
            // Si el jugador está fuera del rango de ataque, el enemigo se mueve.
            if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) > distanceToAtackPlayer)
            {
                movement();
            }
            else
            {
                // Si el enemigo es agresivo, ataca al jugador, de lo contrario, sigue moviéndose.
                if (isAgressive)
                {
                    AttackPlayer();
                }
                else
                {
                    movement();
                }
            }
        }

        // Mantiene la posición Z constante para evitar movimientos no deseados en el eje Z.
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
    }

    // Método para manejar el movimiento del enemigo.
    private void movement()
    {
        // Si el enemigo está atacando, no se mueve.
        if (anim.GetBool("isAttacking"))
        {
            theRB.velocity = Vector2.zero;
            return;
        }

        // Si el contador de movimiento es mayor que cero, el enemigo se mueve.
        if (moveCount > 0)
        {
            moveCount -= Time.deltaTime;
            if (movingright)
            {
                // Movimiento hacia la derecha.
                theRB.velocity = new Vector2(movespeed, theRB.velocity.y);
                theSR.flipX = false;
                FlipPunchColliderToRight();

                if (transform.position.x > rightPoint.position.x)
                {
                    movingright = false; // Cambia la dirección a la izquierda.
                }
            }
            else
            {
                // Movimiento hacia la izquierda.
                theRB.velocity = new Vector2(-movespeed, theRB.velocity.y);
                theSR.flipX = true;
                FlipPunchColliderToLeft();

                if (transform.position.x < leftPoint.position.x)
                {
                    movingright = true; // Cambia la dirección a la derecha.
                }
            }

            // Si el contador de movimiento es cero, inicia el contador de espera.
            if (moveCount <= 0)
            {
                waitCount = Random.Range(waitTime * .75f, waitTime * 1.25f);
            }
            anim.SetBool("isMoving", true);
        }
        else if (waitCount > 0)
        {
            waitCount -= Time.deltaTime;
            theRB.velocity = new Vector2(0f, theRB.velocity.y);

            if (waitCount <= 0)
            {
                moveCount = Random.Range(moveTime * .75f, waitTime * 1.25f);
            }
            anim.SetBool("isMoving", false);
        }
    }

    // Método para manejar el ataque del enemigo al jugador.
    private void AttackPlayer()
    {
        anim.SetBool("isAttacking", true);
        anim.SetBool("isMoving", false);
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
            if (enemyWithAtack)
            {
                anim.SetBool("canAttack", true);
            }
            attackTarget = Vector2.zero;
            attackCounter = waitAfterAttack;
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