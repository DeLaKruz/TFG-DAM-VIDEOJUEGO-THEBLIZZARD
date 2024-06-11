using UnityEngine;

public class FlipColl : MonoBehaviour
{
    private Rigidbody2D heroRigidbody;
    public Collider2D punchCollider;

    void Start()
    {
        // Obtener el Rigidbody2D del héroe
        heroRigidbody = GetComponent<Rigidbody2D>();

        // Verificar si el componente está adjunto
        if (heroRigidbody == null)
        {
            Debug.LogWarning("Rigidbody2D is not attached to the parent object.");
        }
    }

    void Update()
    {
        // Verificar la dirección en la que se está moviendo el héroe
        if (heroRigidbody != null && punchCollider != null)
        {
            // Obtener la velocidad actual del héroe
            float heroVelocityX = heroRigidbody.velocity.x;
            Debug.Log(heroRigidbody.velocity.x);

            // Flip el objeto punch basado en la dirección del movimiento
            if (heroVelocityX < 0)
            {
                // Si el héroe está yendo hacia la izquierda, voltear el collider2D
                punchCollider.offset = new Vector2(-Mathf.Abs(punchCollider.offset.x), punchCollider.offset.y);
            }
            else if (heroVelocityX > 0)
            {
                // Si el héroe está yendo hacia la derecha, ajustar el collider2D
                punchCollider.offset = new Vector2(Mathf.Abs(punchCollider.offset.x), punchCollider.offset.y);
            }
        }
    }

    
}
