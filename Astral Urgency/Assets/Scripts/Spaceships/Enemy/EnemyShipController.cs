using System.Collections.Generic;
using CST.Shared.Resources;
using UnityEngine;

public class EnemyShipController : SpaceshipControllerBase
{
	[Header("Repeling"), Space]
	[SerializeField] private float repelRange;
	[SerializeField] private float repelAmplitude;

	private static HashSet<Rigidbody2D> _alertedEnemies;

	protected virtual void Start()
	{
		_alertedEnemies ??= new HashSet<Rigidbody2D>();
		_alertedEnemies.Add(rb2D);
	}

	private void OnDestroy()
	{
		_alertedEnemies?.Remove(rb2D);
	}

	protected override void FixedUpdate()
	{

	}

	protected void ChaseTarget(Vector2 targetPosition)
	{
		rb2D.linearVelocity = UpdateVelocity(transform.right, stats.GetDynamicStat(StatType.MoveSpeed));
		LookAt(targetPosition);
	}

	protected override Vector2 UpdateVelocity(Vector2 direction, float speed)
	{
		// Enemies will try to avoid each other.
		Vector2 repelForce = Vector2.zero;

		foreach (Rigidbody2D enemy in _alertedEnemies)
		{
			if (enemy == rb2D)
				continue;

			if (Vector2.Distance(enemy.position, rb2D.position) <= repelRange)
			{
				Vector2 repelDirection = (rb2D.position - enemy.position).normalized;
				repelForce += repelDirection;
			}
		}

		Vector2 velocity = direction * speed;
		velocity += repelForce.normalized * repelAmplitude;

		return velocity;
	}
	
	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(rb2D.position, repelRange);
	}
}