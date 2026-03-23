using System.Collections.Generic;
using UnityEngine;
namespace Movement
{
	/// <summary>
	///     Follow the player in fixed isometric perspective, and fades objects that are blocking the view.
	/// </summary>
	[RequireComponent(typeof(Transform))]
	[RequireComponent(typeof(LayerMask))]
	public class CameraFollow : MonoBehaviour
	{
		private static readonly Vector3 Offset = new(4, 8, -4);
		private static readonly Quaternion Rotation = Quaternion.Euler(50, -45, 0);
		private static readonly List<Fadeable> CurrentObstacles = new();

		public Transform player;
		public LayerMask obstacleMask;

		private void LateUpdate()
		{
			MaybeFade();
		}

		private void MaybeFade()
		{
			// Clear all previously faded objects first
			foreach (var fadeable in CurrentObstacles)
				fadeable.SetVisible(true);

			CurrentObstacles.Clear();

			// Cast ray from camera to player
			var dir = player.position - transform.position;
			var dist = dir.magnitude;

			// TODO think about replacement
			var hits = Physics.RaycastAll(transform.position, dir.normalized, dist, obstacleMask);

			foreach (var hit in hits)
			{
				var fadeable = hit.collider.GetComponentInParent<Fadeable>();
				if (!fadeable)
				{
					continue;
				}
				fadeable.SetVisible(false);
				CurrentObstacles.Add(fadeable);
			}

			transform.position = player.position + Offset;
			transform.rotation = Rotation;
		}
	}
}
