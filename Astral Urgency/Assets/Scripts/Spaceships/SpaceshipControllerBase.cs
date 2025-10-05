using CST.Shared.Resources;
using UnityEngine;

public abstract class SpaceshipControllerBase : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] protected Rigidbody2D rb2D;

	[Header("Stats"), Space]
	[SerializeField] protected Stats stats;

	[Header("Movement Settings"), Space]
	[SerializeField] protected float acceleration;
	[SerializeField] protected float deceleration;

	protected abstract void FixedUpdate();

	protected void LookAt(Vector2 targetPosition)
	{
		Vector2 targetDirection = (targetPosition - rb2D.position).normalized;

		float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
		float maxTurnAngle = stats.GetDynamicStat(StatType.TurnAngleDegree);

		rb2D.rotation = Mathf.MoveTowardsAngle(rb2D.rotation, targetAngle, maxTurnAngle * Time.deltaTime);
	}

	/// <summary>
	/// Calculate the final velocity after applying other movement-involved factors.
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	protected abstract Vector2 UpdateVelocity(Vector2 direction, float speed);

	protected virtual void OnDrawGizmosSelected() { }
}
