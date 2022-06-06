// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.1, 2022
// ------------------------------

using UnityEngine;
using ColdironTools.Scriptables;
using ColdironTools.Events;
using ColdironTools.EditorExtensions;

namespace ColdironTools.Gameplay2D
{
    /// <summary>
    /// A Component that handles physics and logic for how a 2D platformer character can move.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class CharacterMovement2DScriptable : MonoBehaviour
    {
        #region ExposedFields
        [Header("Movement")]
        
        [Tooltip("How fast the character's normal walkspeed is.")]
        [SerializeField] private FloatScriptableReference m_WalkSpeed = new FloatScriptableReference(80.0f);
        
        [Tooltip("The amount of movement smoothing.")]
        [SerializeField, Range(0, .3f)] private float m_MovementSmoothing = .05f;
        
        [Tooltip("The layers considered ground.")]
        [SerializeField] private LayerMask m_GroundLayers;

        [Tooltip("The layers that the character can travel through from beneath.")]
        [SerializeField] private LayerMask m_OneWayLayers;
        
        [Tooltip("When walking down slopes, how much the character snaps to follow the slope.")]
        [SerializeField] private FloatScriptableReference m_SlopeGroundSnap = new FloatScriptableReference(0.5f);

        [Header("Jumping")]
        
        [Tooltip("The strength of the jump.")]
        [SerializeField] private FloatScriptableReference m_JumpForce = new FloatScriptableReference(80.0f);
        
        [Tooltip("Can the character move while in the air?")]
        [SerializeField] private BoolScriptableReference m_JumpAirControl = new BoolScriptableReference(true);
        
        [Tooltip("How much control the character has while in the air.")]
        [SerializeField, ConditionalHide("m_JumpAirControl")] private FloatScriptableReference m_AirControl = new FloatScriptableReference(0.75f);
        
        [Tooltip("How long after landing before the character can jump again/")]
        [SerializeField] private FloatScriptableReference m_JumpCooldownTime = new FloatScriptableReference(0.1f);
        
        [Tooltip("The number of times the character can jump in air.")]
        [SerializeField] private IntScriptableReference m_InAirJumpCount = new IntScriptableReference();
        
        [Tooltip("The additional force of gravity while falling, as opposed to rising.")]
        [SerializeField] private FloatScriptableReference m_FallGravityScalar = new FloatScriptableReference(2.5f);

        [Tooltip("A Game Event run when the character jumps.")]
        [SerializeField] public GameEvent m_OnJump;

        [Tooltip("A Game Event run when the character jumps in air.")]
        [SerializeField] public GameEvent m_OnAirJump;

        [Tooltip("A Game Event run when the character lands.")]
        [SerializeField] public GameEvent m_OnLand;

        [Header("Wall Jump")]
        
        [Tooltip("Can the character wall jump?")]
        [SerializeField] private BoolScriptableReference m_CanWallJump = new BoolScriptableReference(false);
        
        [Tooltip("The time after a wall jump before the character can move in that direction.")]
        [SerializeField, ConditionalHide("m_CanWallJump")] private FloatScriptableReference m_WallJumpMovementCooldown = new FloatScriptableReference(0.5f);
        
        [Tooltip("The strength of the wall jump, both horizontally and vertically.")]
        [SerializeField, ConditionalHide("m_CanWallJump")] private FloatScriptableReference m_WallJumpForce = new FloatScriptableReference(60.0f);
        
        [Tooltip("This is multiplied against Wall Jump Force for farther horizontal movement.")]
        [SerializeField, ConditionalHide("m_CanWallJump")] private FloatScriptableReference m_WallJumpHorizontalMultiplier = new FloatScriptableReference(2.0f);

        [Tooltip("A Game Event run when the character wall jumps.")]
        [SerializeField, ConditionalHide("m_CanWallJump")] public GameEvent m_OnWallJump;

        [Header("Sprinting")]
        
        [Tooltip("Can the character sprint?")]
        [SerializeField] private BoolScriptableReference m_CanSprint = new BoolScriptableReference(false);
        
        [Tooltip("How much faster the character moves while sprinting.")]
        [SerializeField, ConditionalHide("m_CanSprint")] private FloatScriptableReference m_SprintSpeedMultiplier = new FloatScriptableReference(1.4f);

        [Tooltip("Does holding the sprint button make you move faster in air?")]
        [SerializeField, ConditionalHide("m_CanSprint")] private BoolScriptableReference m_DoesSprintWorkInAir = new BoolScriptableReference(true);

        [Tooltip("A Game Event run when the character starts sprinting.")]
        [SerializeField, ConditionalHide("m_CanSprint")] public GameEvent m_OnSprintStart;

        [Tooltip("A Game Event run when the character stops sprinting.")]
        [SerializeField, ConditionalHide("m_CanSprint")] public GameEvent m_OnSprintStop;

        [Header("Dash")]
        [Tooltip("Can the character perform a dash?")]
        [SerializeField] private BoolScriptableReference m_CanDash = new BoolScriptableReference(false);

        [Tooltip("Can the dash be performed in air?")]
        [SerializeField, ConditionalHide("m_CanDash")] private BoolScriptableReference m_CanAirDash = new BoolScriptableReference(true);

        [Tooltip("Can the dash be performed while crouching?")]
        [SerializeField, ConditionalHide("m_CanDash")] private BoolScriptableReference m_CanCrouchDash = new BoolScriptableReference(true);

        [Tooltip("The distance the dash should cover during the dash duration.")]
        [SerializeField, ConditionalHide("m_CanDash")] private FloatScriptableReference m_DashDistance = new FloatScriptableReference(60.0f);

        [Tooltip("The time it takes for a dash to cooldown.")]
        [SerializeField, ConditionalHide("m_CanDash")] private FloatScriptableReference m_DashCooldown = new FloatScriptableReference(1.0f);

        [Tooltip("The duration of the dash.")]
        [SerializeField, ConditionalHide("m_CanDash")] private FloatScriptableReference m_DashDuration = new FloatScriptableReference(1.0f);

        [Tooltip("A Game Event run when the character dashes.")]
        [SerializeField, ConditionalHide("m_CanDash")] public GameEvent m_OnDash;

        [Tooltip("A Game Event run when the character dash ends.")]
        [SerializeField, ConditionalHide("m_CanDash")] public GameEvent m_OnEndDash;

        [Header("Crouching")]
        
        [Tooltip("Can the character crouch?")]
        [SerializeField] private BoolScriptableReference m_CanCrouch = new BoolScriptableReference(false);

        [Tooltip("Can the character walk off ledges while crouched?")]
        [SerializeField, ConditionalHide("m_CanCrouch")] private BoolScriptableReference m_CanWalkOffLedgesWhileCrouched = new BoolScriptableReference(true);
        
        [Tooltip("This modifies Walk Speed when crouching.")]
        [SerializeField, ConditionalHide("m_CanCrouch")] private FloatScriptableReference m_CrouchSpeedMultiplier = new FloatScriptableReference(0.6f);
        
        [Tooltip("The height of the Capsule Collider 2D when crouched.")]
        [SerializeField, ConditionalHide("m_CanCrouch")] private FloatScriptableReference m_CrouchCapsuleHeight = new FloatScriptableReference(0.6f);

        [Tooltip("A Game Event run when the character crouches.")]
        [SerializeField, ConditionalHide("m_CanCrouch")] public GameEvent m_OnCrouch;

        [Tooltip("A Game Event run when the character uncrouches.")]
        [SerializeField, ConditionalHide("m_CanCrouch")] public GameEvent m_OnUncrouch;
        #endregion

        #region MemberValues
        private int m_JumpsRemaining;
        private bool m_IsJumpQueued = false;
        private float m_QueuedWallJump = 0.0f;
        private bool m_HasJumpedThisFrame = false;
        const float c_GroundedRadius = 0.2f;
        const float c_WallJumpCheck = 0.2f;
        private bool m_IsGrounded = false;
        private bool m_HasJumpInput = false;
        private bool m_IsFacingRight = true;
        private float m_JumpCooldownStartTime = 0.0f;
        private float m_RightMovementCooldownStartTime = 0.0f;
        private float m_LeftMovementCooldownStartTime = 0.0f;
        private float m_SpriteFlipCooldownEndTime = 0.0f;
        private bool m_IsSprinting = false;
        private float m_DashCooldownStartTime = 0.0f;
        private float m_DashDurationStartTime = 0.0f;
        private float m_DashLerpTime = 0.0f;
        private bool m_IsDashing = false;
        private Vector2 m_DashDestination = Vector2.zero;
        private Vector2 m_DashOrigin = Vector2.zero;
        private bool m_IsCrouching = false;
        private float m_CapsuleOriginalHeight = 0.0f;
        private float m_CapsuleOriginalYOffset = 0.0f;
        private PhysicsMaterial2D m_NoFriction;
        private PhysicsMaterial2D m_HighFriction;
        private Vector2 m_FrameVector;
        private Vector2 m_EmptyVector = Vector2.zero;
        private Rigidbody2D m_Rigidbody;
        private BoxCollider2D m_Collider;
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
        /// The Bottom Front of the Capsule Collider Bounds, based on the direction the character is not facing.
        /// </summary>
        private Vector2 ColliderBackwardFloor
        {
            get
            {
                float l_BoundsFront = !m_IsFacingRight ? m_Collider.bounds.center.x + m_Collider.bounds.extents.x : m_Collider.bounds.center.x - m_Collider.bounds.extents.x;
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
        /// Has the Character's Sprite flip restriction cooled down?
        /// </summary>
        private bool IsSpriteFlipCooledDown
        {
            get
            {
                return Time.time >= m_SpriteFlipCooldownEndTime;
            }
        }

        /// <summary>
        /// The Character's Movement Speed after factoring crouch, sprint, etc.
        /// </summary>
        private float MovementSpeedModified
        {
            get
            {
                float l_SprintValue = m_IsSprinting && (m_IsGrounded || m_DoesSprintWorkInAir) ? m_SprintSpeedMultiplier : 1.0f;
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
                return m_IsCrouching;
            }
        }

        public bool IsFacingRight
        {
            get
            {
                return m_IsFacingRight;
            }
        }
        #endregion

        #region GetReferences
        private void OnValidate()
        {
            AssignReferences();
        }

        /// <summary>
        /// Assigns the Rigidbody2D, BoxCollider2D, and SpriteRender, or creates new ones if they are missing.
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
                BoxCollider2D l_Collider = GetComponent<BoxCollider2D>();
                m_Collider = l_Collider == null ? new BoxCollider2D() : l_Collider;
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
            m_NoFriction.friction = 0.001f;

            m_HighFriction = new PhysicsMaterial2D("HighFriction");
            m_HighFriction.friction = 100000.0f;

            //Assign Game Events
            if (!m_OnCrouch) m_OnCrouch = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnUncrouch) m_OnUncrouch = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnJump) m_OnJump = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnAirJump) m_OnAirJump = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnLand) m_OnLand = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnWallJump) m_OnWallJump = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnSprintStart) m_OnSprintStart = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnSprintStop) m_OnSprintStop = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnDash) m_OnDash = ScriptableObject.CreateInstance<GameEvent>();
            if (!m_OnEndDash) m_OnEndDash = ScriptableObject.CreateInstance<GameEvent>();
        }

        private void FixedUpdate()
        {
            if (m_IsDashing)
            {
                PerformDash();
                return;
            }
            ApplyMovement();
            m_IsGrounded = IsGroundedCheck();
            if (m_IsJumpQueued) PerformJump();
            WallJump(m_QueuedWallJump);
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
        /// Prevents the character sprite from flipping for a period of time. The character can still move.
        /// </summary>
        /// <param name="l_cooldown">The time in seconds before the character can flip again</param>
        public void SetSpriteFlipCooldown(float l_cooldown)
        {
            m_SpriteFlipCooldownEndTime = Time.time + l_cooldown;
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
            if (l_movement == 0 || !IsSpriteFlipCooledDown) return;
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
            if (m_HasJumpedThisFrame)
            {
                m_HasJumpedThisFrame = false;
                return false;
            }
            bool l_WasGrounded = m_IsGrounded;
            bool l_GroundBelow = Physics2D.Raycast(ColliderForwardFloor, Vector2.down, c_GroundedRadius, m_GroundLayers) || Physics2D.Raycast(ColliderCenterFloor, Vector2.down, c_GroundedRadius, m_GroundLayers) || Physics2D.Raycast(ColliderBackwardFloor, Vector2.down, c_GroundedRadius, m_GroundLayers);
            bool l_OneWayBelow = m_Rigidbody.velocity.y <= 0 && (Physics2D.Raycast(ColliderForwardFloor, Vector2.down, c_GroundedRadius, m_OneWayLayers) || Physics2D.Raycast(ColliderCenterFloor, Vector2.down, c_GroundedRadius, m_OneWayLayers) || Physics2D.Raycast(ColliderBackwardFloor, Vector2.down, c_GroundedRadius, m_OneWayLayers));

            if (l_GroundBelow || l_OneWayBelow)
            {
                if (!l_WasGrounded)
                {
                    m_JumpCooldownStartTime = Time.time;
                    m_JumpsRemaining = m_InAirJumpCount;
                    m_Rigidbody.angularVelocity = 0;
                    m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0.0f);
                    SetFriction(0.0f);
                    if (m_IsDashing)
                    {
                        m_DashCooldownStartTime = Time.time;
                        m_IsDashing = false;
                    }
                    m_OnLand?.Raise();
                }
                return true;
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
        /// Prepare for jump if conditions allow.
        /// </summary>
        public void StartJump()
        {
            if (!m_IsGrounded && m_CanWallJump && !m_HasJumpInput)
            {
                WallJumpCheck();
            }
            
            if (m_QueuedWallJump != 0) return;

            if ((m_IsGrounded || (m_JumpsRemaining > 0 && !m_HasJumpInput)) && IsJumpCooledDown)
            {
                m_IsJumpQueued = true;
            }
        }

        /// <summary>
        /// Stop recieving jump input. Allows new in air jumps.
        /// </summary>
        public void StopJump()
        {
            m_HasJumpInput = false;
        }

        private void PerformJump() {
            m_Rigidbody.velocity = Vector2.zero;
            m_Rigidbody.angularVelocity = 0.0f;
            m_Rigidbody.AddForce(new Vector2(0f, m_JumpForce * 10.0f));
            if (m_IsGrounded)
            {

                m_OnJump?.Raise();
            }
            else
            {
                m_OnAirJump?.Raise();
                m_JumpsRemaining--;
            }
            m_IsGrounded = false;
            m_HasJumpInput = true;
            m_IsJumpQueued = false;
            m_HasJumpedThisFrame = true;
        }

        /// <summary>
        /// Check if we can perform a wall jump, and what direction we should jump.
        /// </summary>
        private void WallJumpCheck()
        {
            RaycastHit2D l_HitRight = Physics2D.Raycast(ColliderCenterFloor, Vector2.right, c_WallJumpCheck + m_Collider.bounds.extents.x, m_GroundLayers);
            if (l_HitRight)
            {
                m_QueuedWallJump = -1.0f;
                return;
            }

            RaycastHit2D l_HitLeft = Physics2D.Raycast(ColliderCenterFloor, Vector2.left, c_WallJumpCheck + m_Collider.bounds.extents.x, m_GroundLayers);
            if (l_HitLeft)
            {
                m_QueuedWallJump = 1.0f;
                return;
            }
        }

        /// <summary>
        /// Performs wall jump.
        /// </summary>
        /// <param name="l_Direction">The direction to jump in.</param>
        private void WallJump(float l_Direction)
        {
            if (l_Direction == 0.0f) return;

            Debug.DrawRay(ColliderForwardFloor, new Vector2(l_Direction, 0), Color.blue, 0.5f);
            m_Rigidbody.velocity = Vector2.zero;
            m_Rigidbody.angularVelocity = 0.0f;
            m_Rigidbody.AddForce(new Vector2((m_WallJumpForce * 10.0f * m_WallJumpHorizontalMultiplier) * l_Direction, m_WallJumpForce * 10.0f));
            SetFacingDirection(l_Direction);
            m_HasJumpInput = true;
            m_OnWallJump?.Raise();
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
            m_QueuedWallJump = 0.0f;
        }
        #endregion

        #region Sprinting
        /// <summary>
        /// Toggles the character to sprint if possible.
        /// </summary>
        /// <param name="l_NewSprint">Should the character sprint?</param>
        public void ToggleSprint(bool l_NewSprint)
        {
            bool l_OldSprint = m_IsSprinting;
            m_IsSprinting = m_CanSprint && l_NewSprint && !m_IsCrouching;

            if (m_IsSprinting != l_OldSprint)
            {
                OnSprintChanged(l_NewSprint);
            }
        }

        /// <summary>
        /// Called when the character's sprint state changed.
        /// </summary>
        /// <param name="l_NewSprint">Should the character sprint?</param>
        private void OnSprintChanged(bool l_NewSprint) {
            if (l_NewSprint)
            {
                m_OnSprintStart?.Raise();
            }
            else
            {
                m_OnSprintStop?.Raise();
            }
        }
        #endregion

        #region Dashing
        public void StartDash() {
            if (m_CanDash && !m_IsDashing && Time.time > m_DashCooldownStartTime + m_DashCooldown && (m_CanAirDash || m_IsGrounded) && (!IsCrouching || m_CanCrouchDash)) {
                float l_Distance = m_IsFacingRight ? m_DashDistance : -m_DashDistance;
                m_DashDestination = new Vector2(m_Rigidbody.position.x + l_Distance, m_Rigidbody.position.y);
                m_DashOrigin = m_Rigidbody.position;
                m_DashDurationStartTime = Time.time;
                m_DashLerpTime = 0.0f;
                m_IsDashing = true;
                m_OnDash.Raise();
            }
        }

        private void PerformDash() {
            m_IsGrounded = false;
            m_Rigidbody.velocity = Vector2.zero;
            m_Rigidbody.angularDrag = 0.0f;
            m_DashLerpTime += Time.fixedDeltaTime / m_DashDuration;
            m_Rigidbody.MovePosition(Vector2.Lerp(m_DashOrigin, m_DashDestination, m_DashLerpTime));
            if (Time.time > m_DashDuration + m_DashDurationStartTime)
            {
                EndDash();
            }
        }

        private void EndDash()
        {
            m_DashCooldownStartTime = Time.time;
            m_IsDashing = false;
            m_FrameVector = Vector2.zero;
            m_OnEndDash.Raise();
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

            if (l_NewCrouch)
            {
                m_OnCrouch?.Raise();
            }
            else
            {
                m_OnUncrouch?.Raise();
            }
        }
        #endregion
    }

}
