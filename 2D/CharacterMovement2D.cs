// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.1, 2022
// ------------------------------

using UnityEngine;

namespace ColdironTools.Gameplay2D
{
    /// <summary>
    /// A Component that handles physics and logic for how a 2D platformer character can move.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
    public class CharacterMovement2D : MonoBehaviour
    {
        #region ExposedFields
        [Header("Movement")]
        
        [Tooltip("How fast the character's normal walkspeed is.")]
        [SerializeField] private float m_WalkSpeed = 80.0f;
        
        [Tooltip("The amount of movement smoothing.")]
        [SerializeField, Range(0, .3f)] private float m_MovementSmoothing = .05f;
        
        [Tooltip("The layers considered ground.")]
        [SerializeField] private LayerMask m_GroundLayers;
        
        [Tooltip("When walking down slopes, how much the character snaps to follow the slope.")]
        [SerializeField] private float m_SlopeGroundSnap = 0.5f;

        [Header("Jumping")]
        
        [Tooltip("The strength of the jump.")]
        [SerializeField] private float m_JumpForce = 80.0f;
        
        [Tooltip("Can the character move while in the air?")]
        [SerializeField] private bool m_JumpAirControl = true;
        
        [Tooltip("How much control the character has while in the air.")]
        [SerializeField, Range(0.0f, 1.0f)] private float m_AirControl = 0.75f;
        
        [Tooltip("How long after landing before the character can jump again/")]
        [SerializeField] private float m_JumpCooldownTime = 0.1f;
        
        [Tooltip("The number of times the character can jump in air.")]
        [SerializeField, Range(0, 10)] private int m_InAirJumpCount = 0;
        
        [Tooltip("The additional force of gravity while falling, as opposed to rising.")]
        [SerializeField] private float m_FallGravityScalar = 2.5f;
        
        [Tooltip("Can the character wall jump?")]
        [SerializeField] private bool m_CanWallJump = false;
        
        [Tooltip("The time after a wall jump before the character can move in that direction.")]
        [SerializeField] private float m_WallJumpMovementCooldown = 0.5f;
        
        [Tooltip("The strength of the wall jump, both horizontally and vertically.")]
        [SerializeField] private float m_WallJumpForce = 60.0f;
        
        [Tooltip("This is multiplied against Wall Jump Force for farther horizontal movement.")]
        [SerializeField] private float m_WallJumpHorizontalMultiplier = 2.0f;

        [Header("Sprinting")]
        
        [Tooltip("Can the character sprint?")]
        [SerializeField] private bool m_CanSprint = false;
        
        [Tooltip("How much faster the character moves while sprinting.")]
        [SerializeField] private float m_SprintSpeedMultiplier = 1.4f;

        [Header("Crouching")]
        
        [Tooltip("Can the character crouch?")]
        [SerializeField] private bool m_CanCrouch = false;

        [Tooltip("Can the character walk off ledges while crouched?")]
        [SerializeField] private bool m_CanWalkOffLedgesWhileCrouched = true;
        
        [Tooltip("This modifies Walk Speed when crouching.")]
        [SerializeField] private float m_CrouchSpeedMultiplier = 0.6f;
        
        [Tooltip("The height of the Capsule Collider 2D when crouched.")]
        [SerializeField] private float m_CrouchCapsuleHeight = 0.6f;
        #endregion

        #region MemberValues
        private int m_JumpsRemaining;
        const float c_GroundedRadius = 0.2f;
        const float c_WallJumpCheck = 0.2f;
        private bool m_IsGrounded = false;
        private bool m_HasJumpInput = false;
        private bool m_IsFacingRight = true;
        private float m_JumpCooldownStartTime = 0.0f;
        private float m_RightMovementCooldownStartTime = 0.0f;
        private float m_LeftMovementCooldownStartTime = 0.0f;
        private bool m_IsSprinting = false;
        private bool m_IsCrouching = false;
        private float m_CapsuleOriginalHeight = 0.0f;
        private float m_CapsuleOriginalYOffset = 0.0f;
        private PhysicsMaterial2D m_NoFriction;
        private PhysicsMaterial2D m_HighFriction;
        private Vector2 m_FrameVector;
        private Vector2 m_EmptyVector = Vector2.zero;
        private Rigidbody2D m_Rigidbody;
        private CapsuleCollider2D m_Collider;
        private SpriteRenderer m_SpriteRenderer;
        #endregion

        #region MemberProperties
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
                float l_YCenter = m_Collider.bounds.center.y + m_Collider.bounds.extents.y;
                if (m_IsCrouching)
                {
                    l_YCenter = m_Collider.bounds.center.y + Mathf.Abs(m_CapsuleOriginalYOffset - m_Collider.offset.y) + (m_CapsuleOriginalHeight / 2);
                }
                return new Vector2(m_Collider.bounds.center.x, l_YCenter);
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
        /// Has the Jump Cooldown Time elapsed since the Character Landed?
        /// </summary>
        private bool IsJumpCooledDown
        {
            get
            {
                return Time.time >= m_JumpCooldownStartTime + m_JumpCooldownTime;
            }
        }

        /// <summary>
        /// Has the Character's Right Movement cooled down after performing a right wall jump?
        /// </summary>
        private bool IsRightMovementCooledDown
        {
            get
            {
                return Time.time >= m_RightMovementCooldownStartTime + m_WallJumpMovementCooldown;
            }
        }

        /// <summary>
        /// Has the Character's Right Movement cooled down after performing a right wall jump?
        /// </summary>
        private bool IsLeftMovementCooledDown
        {
            get
            {
                return Time.time >= m_LeftMovementCooldownStartTime + m_WallJumpMovementCooldown;
            }
        }

        /// <summary>
        /// The Character's Movement Speed after factoring crouch, sprint, etc.
        /// </summary>
        private float MovementSpeedModified
        {
            get
            {
                float l_SprintValue = m_IsSprinting && m_IsGrounded ? m_SprintSpeedMultiplier : 1.0f;
                float l_CrouchValue = m_IsCrouching && m_IsGrounded ? m_CrouchSpeedMultiplier : 1.0f;
                float l_AirValue = m_IsGrounded ? 1.0f : m_AirControl;
                return m_WalkSpeed * l_CrouchValue * l_SprintValue * m_AirControl * 10.0f;
            }
        }
        #endregion

        #region PublicProperties
        /// <summary>
        /// The X velocity of the character.
        /// </summary>
        public float HorizontalMovementSpeed
        {
            get
            {
                return m_Rigidbody.velocity.x;
            }
        }

        /// <summary>
        /// Is the character falling?
        /// </summary>
        public bool IsFalling
        {
            get
            {
                return m_Rigidbody.velocity.y < 0 && !m_IsGrounded;
            }
        }

        /// <summary>
        /// Is the character jumping?
        /// </summary>
        public bool IsJumping
        {
            get
            {
                return m_Rigidbody.velocity.y > 0 && !m_IsGrounded;
            }
        }

        /// <summary>
        /// Is the character sprinting?
        /// </summary>
        public bool IsSprinting
        {
            get
            {
                return m_IsSprinting && Mathf.Abs(m_Rigidbody.velocity.x) > 0 && m_IsGrounded;
            }
        }

        /// <summary>
        /// Is the character crouched?
        /// </summary>
        public bool IsCrouching
        {
            get
            {
                return m_IsCrouching && m_IsGrounded;
            }
        }
        #endregion

        #region GetReferences
        private void OnValidate()
        {
            AssignReferences();
        }

        /// <summary>
        /// Assigns the Rigidbody2D, CapsuleCollider2D, and SpriteRender, or creates new ones if they are missing.
        /// </summary>
        private void AssignReferences()
        {
            if (m_Rigidbody == null)
            {
                Rigidbody2D l_Rigidbody = GetComponent<Rigidbody2D>();
                m_Rigidbody = l_Rigidbody == null ? new Rigidbody2D() : l_Rigidbody;
            }

            if (m_Collider == null)
            {
                CapsuleCollider2D l_Collider = GetComponent<CapsuleCollider2D>();
                m_Collider = l_Collider == null ? new CapsuleCollider2D() : l_Collider;
            }

            if (m_SpriteRenderer == null)
            {
                SpriteRenderer l_SpriteRenderer = GetComponent<SpriteRenderer>();
                m_SpriteRenderer = l_SpriteRenderer == null ? new SpriteRenderer() : l_SpriteRenderer;
            }
        }

        #endregion

        #region UnityMethods
        private void Awake()
        {
            //Assigns default values
            m_CapsuleOriginalHeight = m_Collider.size.y;
            m_CapsuleOriginalYOffset = m_Collider.offset.y;

            m_NoFriction = new PhysicsMaterial2D("NoFriction");
            m_NoFriction.friction = 0.0f;

            m_HighFriction = new PhysicsMaterial2D("HighFriction");
            m_HighFriction.friction = 100000.0f;
        }

        private void FixedUpdate()
        {
            ApplyMovement();
            m_IsGrounded = IsGroundedCheck();
        }
        #endregion

        #region HorizontalMovement
        /// <summary>
        /// Determines how much movement should be applied this frame.
        /// </summary>
        /// <param name="l_movement">The movement input to be added.</param>
        public void AddHorizontalMovementInput(float l_movement)
        {
            bool l_CantMoveRight = l_movement > 0 && !IsRightMovementCooledDown;
            bool l_CantMoveLeft = l_movement < 0 && !IsLeftMovementCooledDown;
            if (!m_JumpAirControl && !m_IsGrounded || (l_CantMoveRight || l_CantMoveLeft)) return;
            SetFacingDirection(l_movement);
            SetFriction(l_movement);
            
            bool l_CantWalkOffLedge = !m_CanWalkOffLedgesWhileCrouched && m_IsCrouching && !LedgeDetect();
            if (l_CantWalkOffLedge) return;
            m_FrameVector.x += l_movement * MovementSpeedModified;
        }

        /// <summary>
        /// Applies this frame's movement, factoring slope heights and smoothing the movement.
        /// </summary>
        private void ApplyMovement()
        {
            Vector2 l_SlopeVector = SlopeCheck();
            m_FrameVector = new Vector2(m_FrameVector.x * l_SlopeVector.x, m_Rigidbody.velocity.y * l_SlopeVector.y);
            m_Rigidbody.velocity = Vector2.SmoothDamp(m_Rigidbody.velocity, m_FrameVector, ref m_EmptyVector, m_MovementSmoothing);
            m_FrameVector = Vector2.zero;
            FallGravityScale();
        }

        /// <summary>
        /// Gets the perpindicular vector to the slope normal, allowing movement to travel along the direction of a slope.
        /// </summary>
        private Vector2 SlopeCheck()
        {
            RaycastHit2D l_Hit = Physics2D.Raycast(ColliderCenterFloor, Vector2.down, c_GroundedRadius * 1.5f, m_GroundLayers);

            if (l_Hit)
            {
                Vector2 l_SlopeNormalPerpendicular = Vector2.Perpendicular(l_Hit.normal).normalized;

                Debug.DrawRay(l_Hit.point, m_IsFacingRight ? -l_SlopeNormalPerpendicular : l_SlopeNormalPerpendicular, Color.red);

                return -l_SlopeNormalPerpendicular;
            }

            return new Vector2(1, 1);
        }

        /// <summary>
        /// Checks if the character is facing a ledge.
        /// </summary>
        private bool LedgeDetect() {
            RaycastHit2D l_Hit = Physics2D.Raycast(ColliderForwardFloor, Vector2.down, c_GroundedRadius * 2.0f, m_GroundLayers);

            if (l_Hit)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the direction the character is facing, and sets the sprite renderer accordingly.
        /// </summary>
        /// <param name="l_movement">The movement direction.</param>
        private void SetFacingDirection(float l_movement)
        {
            if (l_movement == 0) return;
            m_IsFacingRight = l_movement < 0 ? false : true;
            m_SpriteRenderer.flipX = !m_IsFacingRight;
        }
        
        /// <summary>
        /// Sets the Collider's friction based on whether the character is currently grounded and moving.
        /// </summary>
        /// <param name="l_movement">The movement input.</param>
        private void SetFriction(float l_movement)
        {
            if (l_movement == 0 && m_IsGrounded)
            {
                m_Collider.sharedMaterial = m_HighFriction;
            }
            else
            {
                m_Collider.sharedMaterial = m_NoFriction;
            }
        }
        #endregion

        #region FallHandling
        /// <summary>
        /// Increases falling speed when needed.
        /// </summary>
        private void FallGravityScale()
        {
            if (m_Rigidbody.velocity.y < 0)
            {
                m_Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (m_FallGravityScalar - 1) * Time.fixedDeltaTime;
            }
            else if (m_Rigidbody.velocity.y > 0 && !m_HasJumpInput)
            {
                m_Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (m_FallGravityScalar - 1) * Time.fixedDeltaTime;
            }
        }

        /// <summary>
        /// is the character currently standing on ground?
        /// </summary>
        private bool IsGroundedCheck()
        {
            bool l_WasGrounded = m_IsGrounded;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(ColliderCenterFloor, c_GroundedRadius, m_GroundLayers);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    if (!l_WasGrounded)
                    {
                        m_JumpCooldownStartTime = Time.time;
                        m_JumpsRemaining = m_InAirJumpCount;
                    }
                    return true;
                }
            }

            if (l_WasGrounded && IsJumpCooledDown)
            {
                return SnapToGround();
            }
            return false;
        }

        /// <summary>
        /// Push the character towards the ground if walking off a gradual slope.
        /// </summary>
        private bool SnapToGround()
        {
            RaycastHit2D l_Hit = Physics2D.Raycast(ColliderCenterFloor, Vector2.down, m_SlopeGroundSnap, m_GroundLayers);

            if (l_Hit)
            {
                m_Rigidbody.AddForce(Vector2.down * 200.0f);
                Debug.DrawRay(l_Hit.point, Vector3.down, Color.blue);
                return true;
            }

            return false;
        }
        #endregion

        #region Jumping
        /// <summary>
        /// Perform Jump if conditions allow.
        /// </summary>
        public void StartJump()
        {
            if (!m_IsGrounded && m_CanWallJump && !m_HasJumpInput)
            {
                WallJumpCheck();
            }

            if ((m_IsGrounded || (m_JumpsRemaining > 0 && !m_HasJumpInput)) && IsJumpCooledDown)
            {
                m_Rigidbody.velocity = Vector2.zero;
                m_Rigidbody.angularVelocity = 0.0f;
                m_Rigidbody.AddForce(new Vector2(0f, m_JumpForce * 10.0f));
                m_JumpsRemaining--;
                m_IsGrounded = false;
            }
            m_HasJumpInput = true;
        }

        /// <summary>
        /// Stop recieving jump input. Allows new in air jumps.
        /// </summary>
        public void StopJump()
        {
            m_HasJumpInput = false;
        }

        /// <summary>
        /// Check if we can perform a wall jump, and what direction we should jump.
        /// </summary>
        private void WallJumpCheck()
        {
            RaycastHit2D l_HitRight = Physics2D.Raycast(ColliderCenterFloor, Vector2.right, c_WallJumpCheck + m_Collider.bounds.extents.x, m_GroundLayers);
            if (l_HitRight)
            {
                WallJump(-1.0f);
                return;
            }

            RaycastHit2D l_HitLeft = Physics2D.Raycast(ColliderCenterFloor, Vector2.left, c_WallJumpCheck + m_Collider.bounds.extents.x, m_GroundLayers);
            if (l_HitLeft)
            {
                WallJump(1.0f);
                return;
            }
        }

        /// <summary>
        /// Performs wall jump.
        /// </summary>
        /// <param name="l_Direction">The direction to jump in.</param>
        private void WallJump(float l_Direction)
        {
            Debug.DrawRay(ColliderForwardFloor, new Vector2(l_Direction, 0), Color.blue, 0.5f);
            m_Rigidbody.velocity = Vector2.zero;
            m_Rigidbody.angularVelocity = 0.0f;
            m_Rigidbody.AddForce(new Vector2((m_WallJumpForce * 10.0f * m_WallJumpHorizontalMultiplier) * l_Direction, m_WallJumpForce * 10.0f));
            SetFacingDirection(l_Direction);
            m_HasJumpInput = true;
            if (l_Direction < 0)
            {
                m_RightMovementCooldownStartTime = Time.time;
                m_LeftMovementCooldownStartTime = Time.time - m_WallJumpMovementCooldown;
            }
            else
            {
                m_LeftMovementCooldownStartTime = Time.time;
                m_RightMovementCooldownStartTime = Time.time - m_WallJumpMovementCooldown;
            }
        }
        #endregion

        #region Sprinting
        /// <summary>
        /// Toggles the character to sprint if possible.
        /// </summary>
        /// <param name="l_NewSprint">Should the character sprint?</param>
        public void ToggleSprint(bool l_NewSprint)
        {
            m_IsSprinting = m_CanSprint && l_NewSprint && !m_IsCrouching;
        }
        #endregion

        #region Crouching
        /// <summary>
        /// Toggles the character to crouch or uncrouch if possible.
        /// </summary>
        /// <param name="l_NewCrouch">Should the character crouch?</param>
        public void ToggleCrouch(bool l_NewCrouch)
        {
            if (m_IsCrouching && !l_NewCrouch && !CanUncrouch()) return;

            bool l_OldCrouch = m_IsCrouching;
            m_IsCrouching = m_CanCrouch && l_NewCrouch;

            if (m_IsCrouching != l_OldCrouch)
            {
                OnCrouchChanged(l_NewCrouch);
            }
        }

        /// <summary>
        /// Is there ground above the character that would prevent them from uncrouching?
        /// </summary>
        private bool CanUncrouch()
        {
            Vector2 l_CrouchedCeiling = new Vector2(m_Collider.bounds.center.x, m_Collider.bounds.center.y + m_Collider.bounds.extents.y);
            Debug.DrawRay(l_CrouchedCeiling, ColliderCenterCeiling - l_CrouchedCeiling, Color.green, 1.0f);
            RaycastHit2D l_Hit = Physics2D.Raycast(l_CrouchedCeiling, Vector2.up, (ColliderCenterCeiling - l_CrouchedCeiling).y, m_GroundLayers);
            if (l_Hit)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Called when the character's crouched state changed this frame. Handles collider size.
        /// </summary>
        /// <param name="l_NewCrouch">Should the character crouch?</param>
        private void OnCrouchChanged(bool l_NewCrouch)
        {
            float l_Size = l_NewCrouch ? m_CrouchCapsuleHeight : m_CapsuleOriginalHeight;
            float l_YExtent = m_Collider.bounds.extents.y;
            m_Collider.size = new Vector2(m_Collider.size.x, l_Size);

            float l_Offset = l_NewCrouch ? m_Collider.offset.y - (l_YExtent - m_Collider.bounds.extents.y) : m_CapsuleOriginalYOffset;
            m_Collider.offset = new Vector2(m_Collider.offset.x, l_Offset);
        }
        #endregion
    }

}
