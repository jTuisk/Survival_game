using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.SolarSystem;
using Game.SolarSystem.Planet;
using Game.Player.Input;
using Game.Player.Inventory;

namespace Game.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private InputManager _inputManager;
        private Rigidbody _rb;


        [SerializeField] Transform player;
        [SerializeField] bool randomLocationAtStart = true;

        [SerializeField] IcosahedronPlanet planet;
        [SerializeField] bool useCustomGravity = true;
        [SerializeField] bool autoOrient = true;
        [SerializeField, Range(0.0f, 1000f)] float oirientSlerpSpeed = 10f;
        //[ReadOnly]
        [SerializeField] public CelestialBody celestialBody;
        [SerializeField] float gravityForce = -9.81f;
        [SerializeField] float stickToGroundGravity = -5f;

        [SerializeField, ReadOnly] Vector3 playerMoveInput = Vector3.zero;
        private Vector3 targetVelocity;
        private Vector3 smoothVelocity;
        private Vector3 smoothVectorRef;

        [SerializeField, ReadOnly] float currentSpeed = 0.0f;
        [SerializeField, Range(0.1f, 100f)] float walkSpeed = 5.4f;
        [SerializeField, Range(0.1f, 100f)] float runSpeed = 9.8f;
        [SerializeField, Range(0.1f, 100f)] float jumpForce = 10.0f;
        private float vSmoothTime = 0.1f;
        private float airSmoothTime = 0.5f;


        [SerializeField, Range(0.1f, 1000f)] float mouseSensitivity = 100f;
        [SerializeField, ReadOnly] bool useInvertMouse = false;


        [SerializeField] Transform firstPersonCamera;
        [SerializeField] Transform firstPersonCameraPosition;
        [SerializeField] float xRotationUpperLimit = -40f;
        [SerializeField] float xRotationLowerLimit = 80f;
        private float xRotation;

        [SerializeField] bool useCustomGroundCheck = true;
        [SerializeField] bool playerIsGrounded = false;
        [SerializeField, Range(0.0f, 2f)] float groundCheckRadiusMultiplier = 1f;
        [SerializeField, Range(-1f, 1f)] float groundCheckDistance = 0.05f;
        RaycastHit groundCheckHit = new RaycastHit();
        [SerializeField] LayerMask walkableMask;
        [SerializeField] Transform feet;
        [SerializeField] CapsuleCollider capsuleCollider;

        [SerializeField] Animator animator;
        private bool _hasAnimator;
        private int _speedHash;

        [SerializeField] float playerMass = 80.0f;

        private InteractionHandler interactionHandler;
        public InteractionSettingsScriptableObject interactionData;
        public InventorySystem inventory; 

        private void Awake()
        {
            HideCursor.ShowCurors(false, true);
            _inputManager = GetComponent<InputManager>();
            interactionHandler = new InteractionHandler(_inputManager, interactionData, inventory);
            InitializeRigidbody();
            InitializeAnimator();
        }

        private void Start()
        {
            if (randomLocationAtStart && planet != null)
            {
                RelocatePlayer(planet.GetRandomSurfacePoint());
            }
        }

        private void InitializeRigidbody()
        {
            if (!player.TryGetComponent<Rigidbody>(out _rb))
            {
                _rb = player.gameObject.AddComponent<Rigidbody>() as Rigidbody;
            }
            _rb.useGravity = false;
            _rb.isKinematic = false;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.mass = playerMass;
        }

        private void InitializeAnimator()
        {
            _hasAnimator = animator != null ? true : TryGetComponent<Animator>(out animator);

            _speedHash = Animator.StringToHash("Speed");
        }

        private void Update()
        {
            interactionHandler.Handle();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }


        private void HandleMovement()
        {
            if (Time.timeScale == 0)
                return;

            HandleGravity();

            HandleMovementInput();
            HandleCameraMovements();
            HandleAnimations();
        }

        private void HandleGravity()
        {
            if (!useCustomGravity)
                return;

            if (autoOrient)
            {
                if (celestialBody != null)
                {
                    Vector3 planetOrigin = celestialBody.transform.position;
                    Vector3 gravityUp = (player.position - planetOrigin).normalized;
                    Vector3 finalRotation = gravityUp;

                    RaycastHit hit;

                    if(Physics.Raycast(player.position, -gravityUp, out hit, Mathf.Infinity))
                    {
                        Vector3 point = planet.GetSurfacePoint(gravityUp);

                        finalRotation = (gravityUp + point) / 2;
                    }

                    _rb.rotation = Quaternion.FromToRotation(player.up, finalRotation) * _rb.rotation;

                }
            }
            //Get highest gravity Force or get nearest celestian body and get gravity force from that.

            if (!playerIsGrounded)
            {
                /*float sqrDst = (celestialBody.transform.position - _rb.position).sqrMagnitude;
                 Vector3 forceDir = (celestialBody.transform.position - _rb.position).normalized;
                 Vector3 acceleration = forceDir * Universe.GravitationalConstant * gravityForce * celestialBody.data.mass * 100000f / sqrDst;
                 _rb.AddForce(acceleration, ForceMode.Acceleration);*/ 
                
                //Debug.Log("Apply Gravity!");
                _rb.AddForce(player.transform.up * -gravityForce, ForceMode.Acceleration);
            }
        }

        private void OnDrawGizmos()
        {
            LayerMask layerMask = walkableMask;
            float sphereCastRadius = capsuleCollider.radius * groundCheckRadiusMultiplier;
            float range = groundCheckDistance;
            Gizmos.DrawWireSphere(transform.position, range);


            RaycastHit hit;
            if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward * range, out hit, range, layerMask))
            {
                Gizmos.color = Color.green;
                Vector3 sphereCastMidpoint = player.transform.position + (-player.transform.up * hit.distance);
                Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
                Gizmos.DrawSphere(hit.point, 0.1f);
                Debug.DrawLine(transform.position, sphereCastMidpoint, Color.green);
            }
            else
            {
                Gizmos.color = Color.red;
                Vector3 sphereCastMidpoint = player.transform.position + (-player.transform.up * (range - sphereCastRadius));
                Gizmos.DrawWireSphere(sphereCastMidpoint, sphereCastRadius);
                Debug.DrawLine(transform.position, sphereCastMidpoint, Color.red);
            }
        }

        private void HandleMovementInput()
        {
            playerIsGrounded = IsGrounded();
            playerMoveInput = GetMoveInput();

            targetVelocity = player.transform.TransformDirection(playerMoveInput.normalized) * GetMovementSpeed();
            smoothVelocity = Vector3.SmoothDamp(smoothVelocity, targetVelocity, ref smoothVectorRef, GetSmoothVelocityTime());

            currentSpeed = smoothVelocity.magnitude;

            if (_inputManager.Jump)
            {
                HandleJump();
            }
            else
            {
                StickPlayerToGround();
            }

            _rb.MovePosition(_rb.position + smoothVelocity * Time.fixedDeltaTime);
        }

        private void HandleCameraMovements()
        {
            float mouse_x = _inputManager.Look.x;
            float mouse_y = _inputManager.Look.y;
            firstPersonCamera.position = firstPersonCameraPosition.position;

            xRotation -= mouse_y * mouseSensitivity * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, xRotationUpperLimit, xRotationLowerLimit);

            firstPersonCamera.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

            player.Rotate(Vector3.up, mouse_x * mouseSensitivity * Time.fixedDeltaTime);
        }

        private void HandleAnimations()
        {
            if (_hasAnimator)
            {
                float multiplier = 1f;
                if (_inputManager.Move.y < 0)
                {
                    multiplier = _inputManager.Move.y;
                }
                animator.SetFloat(_speedHash, currentSpeed * multiplier);
            }
        }

        private void HandleJump()
        {
            if (playerIsGrounded || !useCustomGroundCheck)
            {
                _rb.AddForce(player.transform.up * jumpForce, ForceMode.VelocityChange);
                playerIsGrounded = false;
            }
        }

        private void StickPlayerToGround()
        {
            if (!useCustomGravity)
                return;

            if (playerIsGrounded)
            {
                //_rb.AddForce(player.transform.up * stickToGroundGravity, ForceMode.Acceleration);
            }
        }

        /*private bool IsGrounded()
        {
            if (celestialBody != null && useCustomGroundCheck)
            {
                Vector3 relativeVelocity = _rb.velocity - celestialBody.data.velocity;
                if (relativeVelocity.y <= jumpForce * 0.5f)
                {
                    float sphereCastRadius = capsuleCollider.radius * groundCheckRadiusMultiplier;
                    float sphereCastTravelDistance = capsuleCollider.bounds.extents.y + sphereCastRadius + groundCheckDistance;

                    RaycastHit hit;

                    if (Physics.SphereCast(_rb.position, sphereCastRadius, -player.transform.up, out hit, groundCheckDistance, walkableMask))
                    {
                        Debug.DrawRay(_rb.position, -player.up * sphereCastRadius, Color.green);
                    }
                    else
                    {
                        Debug.Log($"No hit");
                        Debug.DrawRay(_rb.position, -player.up * sphereCastRadius, Color.red);
                    }

                    //TODO: Add LayerMask?
                    return Physics.SphereCast(_rb.position, sphereCastRadius, -player.transform.up, out groundCheckHit, sphereCastTravelDistance);
                }
            }
            return false;
        }*/

        private bool IsGrounded()
        {
            // Sphere must not overlay terrain at origin otherwise no collision will be detected
            // so rayRadius should not be larger than controller's capsule collider radius
            const float rayRadius = .3f;
            const float groundedRayDst = .2f;
            bool grounded = false;

            if (celestialBody != null)
            {
                var relativeVelocity = _rb.velocity - celestialBody.data.velocity;
                // Don't cast ray down if player is jumping up from surface
                if (relativeVelocity.y <= jumpForce * .5f)
                {
                    RaycastHit hit;
                    Vector3 offsetToFeet = (feet.position - transform.position);
                    Vector3 rayOrigin = _rb.position + offsetToFeet + player.transform.up * rayRadius;
                    Vector3 rayDir = -transform.up;

                    grounded = Physics.SphereCast(rayOrigin, rayRadius, rayDir, out hit, groundedRayDst, walkableMask);

                    if(Physics.SphereCast(rayOrigin, rayRadius, rayDir, out hit, groundedRayDst, walkableMask))
                    {
                        Debug.DrawRay(rayOrigin, -player.up * rayRadius, Color.green);
                        //Debug.Log($"Hit: {hit.collider.name}");
                    }
                    else
                    {

                        //Debug.Log($"No hit");
                        Debug.DrawRay(rayOrigin, -player.up * rayRadius, Color.red);
                    }
                }
            }
            return grounded;
        }

        private float GetSmoothVelocityTime()
        {
            return playerIsGrounded ? vSmoothTime : airSmoothTime;
        }

        private float GetMovementSpeed()
        {
            if (_inputManager.Run)
                return runSpeed;

            return walkSpeed;
        }

        private Vector2 GetLookInput()
        {
            return new Vector3(_inputManager.Look.x, _inputManager.Look.y);
        }

        private Vector3 GetMoveInput()
        {
            return new Vector3(_inputManager.Move.x, 0.0f, _inputManager.Move.y);
        }

        private void RelocatePlayer(Vector3 toPos)
        {
            player.transform.position = toPos;
            Vector3 spawnRotation = toPos.normalized;
            Quaternion rotation = Quaternion.FromToRotation(player.transform.up, spawnRotation) * player.transform.rotation;
            player.transform.rotation = rotation;
        }
    }
}
