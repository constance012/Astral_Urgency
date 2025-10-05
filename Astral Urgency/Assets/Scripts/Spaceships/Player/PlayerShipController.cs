using CST.Shared.Resources;
using UnityEngine;

public class PlayerShipController : SpaceshipControllerBase
{
	[Header("Steering"), Space]
	[SerializeField] private float steerAngleDegree;

	public static Vector2 Position { get; private set; }

	private Transform _playerTransform;
	private Vector3 _movementDirection;
	private int _throttleValue;
	private float _currentSpeed;
	private float _maxSpeed;

	private void Awake()
	{
		_playerTransform = transform;
	}

	private void Start()
	{
		_currentSpeed = 0f;
		_maxSpeed = stats.GetDynamicStat(StatType.MoveSpeed);
	}

	private void Update()
	{
		ReadInputValues();
	}

	protected override void FixedUpdate()
	{
		HandleMovement();

		Position = rb2D.position;
	}

	protected override Vector2 UpdateVelocity(Vector2 direction, float speed = 0f)
	{
		if (direction.sqrMagnitude > .01f)
		{
			_currentSpeed += acceleration * Time.deltaTime;
			_currentSpeed = Mathf.Min(_maxSpeed, _currentSpeed);

			return direction * _currentSpeed;
		}
		else if (_currentSpeed > 0f)
		{
			_currentSpeed -= deceleration * Time.deltaTime;
			_currentSpeed = Mathf.Max(0f, _currentSpeed);

			return _playerTransform.right * _currentSpeed;
		}

		return Vector2.zero;
	}

	Vector3 _steerDirection;

	public void ReadInputValues()
	{
#if ENABLE_LEGACY_INPUT_MANAGER
		_throttleValue = LegacyInputManager.Instance.GetKey(KeybindingAction.MoveUp) ? 1 : 0;
#endif

		Vector3 forwardDirection = _playerTransform.right * _throttleValue;
		float steerAngle = LegacyInputManager.Instance.GetAxisRaw("Horizontal") * steerAngleDegree;
		_steerDirection = Quaternion.Euler(0f, 0f, steerAngle) * forwardDirection;

		_movementDirection = forwardDirection + _steerDirection;
		_movementDirection.Normalize();
	}

	private void HandleMovement()
	{
		rb2D.linearVelocity = UpdateVelocity(_movementDirection);

		if (rb2D.linearVelocity.sqrMagnitude < .01f)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			LookAt(mousePosition);
		}
		else
		{
			Vector2 lookDirection = _throttleValue == 1 ? _movementDirection : _playerTransform.right;
			Vector2 aheadPosition = rb2D.position + lookDirection;
			LookAt(aheadPosition);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(rb2D.position, rb2D.position + (Vector2)_steerDirection);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(rb2D.position, rb2D.position + (Vector2)_movementDirection);
	}
}