using UnityEngine;

namespace ColdironTools.Gameplay2D
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class EnemyAI2D : MonoBehaviour
    {
        [SerializeField] private LayerMask m_GroundLayers;
        [SerializeField] private float m_MovementSpeed = 10.0f;
        [SerializeField] private HealthComponent m_HealthComponent;

        private bool m_IsFacingRight = true;
        private Collider2D m_Collider;
        private Rigidbody2D m_Rigidbody;
        private SpriteRenderer m_SpriteRenderer;

        /// <summary>
        /// The Bottom Center of the Capsule Collider Bounds.
        /// </summary>
        private Vector2 ColliderCenterFloor
        {
            get
            {
                return new Vector2(m_Collider.bounds.center.x, m_Collider.bounds.center.y - m_Collider.bounds.extents.y);
            }
        }

        /// <summary>
        /// The Top Center of the Capsule Collider Bounds, ignores crouch height.
        /// </summary>
        private Vector2 ColliderCenterCeiling
        {
            get
            {
                return new Vector2(m_Collider.bounds.center.x, m_Collider.bounds.center.y + m_Collider.bounds.extents.y);
            }
        }

        /// <summary>
        /// The Bottom Front of the Capsule Collider Bounds, based on the direction the character is facing.
        /// </summary>
        private Vector2 ColliderForwardFloor
        {
            get
            {
                float l_BoundsFront = m_IsFacingRight ? m_Collider.bounds.center.x + m_Collider.bounds.extents.x : m_Collider.bounds.center.x - m_Collider.bounds.extents.x;
                return new Vector2(l_BoundsFront, m_Collider.bounds.center.y - m_Collider.bounds.extents.y);
            }
        }

        /// <summary>
        /// The Middle Front of the Capsule Collider Bounds, based on the direction the character is facing.
        /// </summary>
        private Vector2 ColliderForwardCenter
        {
            get
            {
                float l_BoundsFront = m_IsFacingRight ? m_Collider.bounds.center.x + m_Collider.bounds.extents.x : m_Collider.bounds.center.x - m_Collider.bounds.extents.x;
                return new Vector2(l_BoundsFront, m_Collider.bounds.center.y);
            }
        }

        private void OnValidate()
        {
            if (m_Rigidbody == null)
            {
                Rigidbody2D l_Rigidbody = GetComponent<Rigidbody2D>();
                m_Rigidbody = l_Rigidbody == null ? new Rigidbody2D() : l_Rigidbody;
            }

            if (m_Collider == null)
            {
                Collider2D l_Collider = GetComponent<Collider2D>();
                m_Collider = l_Collider == null ? new CapsuleCollider2D() : l_Collider;
            }

            if (m_SpriteRenderer == null)
            {
                SpriteRenderer l_SpriteRenderer = GetComponent<SpriteRenderer>();
                m_SpriteRenderer = l_SpriteRenderer == null ? new SpriteRenderer() : l_SpriteRenderer;
            }
        }

        private void FixedUpdate()
        {
            if (m_HealthComponent.IsInvulnerable) return;
            PlatformPatrol();
            m_SpriteRenderer.flipX = m_IsFacingRight;
        }

        private void PlatformPatrol()
        {
            if (WallDetect() || !EdgeDetect())
            {
                m_IsFacingRight = !m_IsFacingRight;
            }

            ApplyMovement(new Vector2(m_IsFacingRight ? m_MovementSpeed : -m_MovementSpeed, 0.0f));
        }

        private bool WallDetect()
        {
            if (Physics2D.Raycast(ColliderForwardCenter, m_IsFacingRight ? Vector2.right : Vector2.left, 0.2f, m_GroundLayers))
            {
                return true;
            }
            return false;
        }

        private bool EdgeDetect()
        {
            if (Physics2D.Raycast(ColliderForwardCenter, Vector2.down, 1.0f + m_Collider.bounds.extents.y, m_GroundLayers))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Applies this frame's movement, factoring slope heights and smoothing the movement.
        /// </summary>
        private void ApplyMovement(Vector2 l_direction)
        {
            Vector2 l_SlopeVector = SlopeCheck();
            Vector2 m_EmptyVector = Vector2.zero;
            l_direction = new Vector2(l_direction.x * l_SlopeVector.x, m_Rigidbody.velocity.y * l_SlopeVector.y);
            m_Rigidbody.velocity = Vector2.SmoothDamp(m_Rigidbody.velocity, l_direction, ref m_EmptyVector, 0.05f);
        }

        /// <summary>
        /// Gets the perpindicular vector to the slope normal, allowing movement to travel along the direction of a slope.
        /// </summary>
        private Vector2 SlopeCheck()
        {
            RaycastHit2D l_Hit = Physics2D.Raycast(ColliderCenterFloor, Vector2.down, 0.4f, m_GroundLayers);

            if (l_Hit)
            {
                Vector2 l_SlopeNormalPerpendicular = Vector2.Perpendicular(l_Hit.normal).normalized;

                Debug.DrawRay(l_Hit.point, m_IsFacingRight ? -l_SlopeNormalPerpendicular : l_SlopeNormalPerpendicular, Color.red);

                return -l_SlopeNormalPerpendicular;
            }

            return new Vector2(1, 1);
        }
    }
}
