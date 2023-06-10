using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#endif


namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		//footsteps elements
		[SerializeField] private AudioClip[] gravel;
		[SerializeField] private AudioClip[] grass;
		[SerializeField] private AudioClip[] dryGrass;
		[SerializeField] private AudioClip[] snow;
		[SerializeField] private AudioClip[] rock;
		[SerializeField] private AudioClip[] water;
		private AudioClip clip;
		private AudioSource audioSource;
		public TerrainDetector terrain;
		private bool isMoving = false;
		float distanceCovered;

		//pickup system
		//each trigger is a chime collider
		private bool triggerActive1 = false;
		private bool triggerActive2 = false;
		private bool triggerActive3 = false;
		private bool triggerActive4 = false;
		private bool triggerActive5 = false;
		private bool triggerActive6 = false;
		private bool triggerActive7 = false;
		private bool triggerActive8 = false;
		private bool triggerActive9 = false;
		private bool triggerActive10 = false;

		//private int chimesCollected = 0;
		public ChimeManager chimeManager;

		//Final Bell
		public bool triggerActiveBell = false;
		

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
			audioSource = GetComponent<AudioSource>();
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
			Pickup();

			if(isMoving == true)
            {
				distanceCovered += (_speed * Time.deltaTime) * 0.25f;
				if(distanceCovered > 1)
                {
					clip = GetRandomClip();
					audioSource.PlayOneShot(clip);
					distanceCovered = 0;
                }
            }
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

			

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero)
			{
				targetSpeed = 0.0f;
				isMoving = false;
			}

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
				isMoving = true;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		

		}

		

		private AudioClip GetRandomClip()
		{
			int terrainTextureIndex = terrain.GetTerrainAtPosition(transform.position);
			Debug.Log("random clip got");

			switch (terrainTextureIndex)
			{
				case 0:
					return rock[UnityEngine.Random.Range(0, rock.Length)];
				case 1:
					return dryGrass[UnityEngine.Random.Range(0, dryGrass.Length)];
				case 2:
					return grass[UnityEngine.Random.Range(0, grass.Length)];
				case 3:
					return gravel[UnityEngine.Random.Range(0, gravel.Length)];
				case 4:
					return rock[UnityEngine.Random.Range(0, rock.Length)];
				case 5:
					return snow[UnityEngine.Random.Range(0, snow.Length)];
				case 6:
					return water[UnityEngine.Random.Range(0, water.Length)];
				default:
					return gravel[UnityEngine.Random.Range(0, gravel.Length)];
			}
			

		}

		//controlling all chime collisions from the FPScontroller is quite disgusting but we have no time to re-learn input and learn to comunicate between scripts properly
		public void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Chime1"))
			{
				triggerActive1 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime2"))
			{
				triggerActive2 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime3"))
			{
				triggerActive3 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime4"))
			{
				triggerActive4 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime5"))
			{
				triggerActive5 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime6"))
			{
				triggerActive6 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime7"))
			{
				triggerActive7 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime8"))
			{
				triggerActive8 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime9"))
			{
				triggerActive9 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime10"))
			{
				triggerActive10 = true;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("FinalBell"))
			{
				triggerActiveBell = true;
				//text.SetActive(true);
				print("Collision detected");
			}
		}
		public void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Chime1"))
			{
				triggerActive1 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime2"))
			{
				triggerActive2 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime3"))
			{
				triggerActive3 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime4"))
			{
				triggerActive4 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime5"))
			{
				triggerActive5 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime6"))
			{
				triggerActive6 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime7"))
			{
				triggerActive7 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime8"))
			{
				triggerActive8 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime9"))
			{
				triggerActive9 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("Chime10"))
			{
				triggerActive10 = false;
				//text.SetActive(true);
				print("Collision detected");
			}
			if (other.CompareTag("FinalBell"))
			{
				triggerActiveBell = false;
				//text.SetActive(true);
				print("Collision detected");
			}

		}

		private void Pickup()
        {
			if (triggerActive1 == true && _input.pickup == true)
			{
				chimeManager.chime1 = true;
				Debug.Log("Input chime 1");						
			}
			if (triggerActive2 == true && _input.pickup == true)
			{
				chimeManager.chime2 = true;
				Debug.Log("Input chime 2");
			}
			if (triggerActive3 == true && _input.pickup == true)
			{
				chimeManager.chime3 = true;
				Debug.Log("Input chime 3");
			}
			if (triggerActive4 == true && _input.pickup == true)
			{
				chimeManager.chime4 = true;
				Debug.Log("Input chime 4");
			}
			if (triggerActive5 == true && _input.pickup == true)
			{
				chimeManager.chime5 = true;
				Debug.Log("Input chime 5");
			}
			if (triggerActive6 == true && _input.pickup == true)
			{
				chimeManager.chime6 = true;
				Debug.Log("Input chime 6");
			}
			if (triggerActive7 == true && _input.pickup == true)
			{
				chimeManager.chime7 = true;
				Debug.Log("Input chime 7");
			}
			if (triggerActive8 == true && _input.pickup == true)
			{
				chimeManager.chime8 = true;
				Debug.Log("Input chime 8");
			}
			if (triggerActive9 == true && _input.pickup == true)
			{
				chimeManager.chime9 = true;
				Debug.Log("Input chime 9");
			}
			if (triggerActive10 == true && _input.pickup == true)
			{
				chimeManager.chime10 = true;
				Debug.Log("Input chime 10");
			}
			if (triggerActiveBell == true && _input.pickup == true)
			{
				chimeManager.finalBell = true;
				Debug.Log("Input Final Bell");
			}
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}