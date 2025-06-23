using System;
using TransformHandles.Utils;
using UnityEditor;
using UnityEngine;

namespace TransformHandles
{
	public class TorusColliderController : MonoBehaviour
	{
		[SerializeField] private int segmentCount = 32;
		[SerializeField] private int sideCount = 15;
		[SerializeField] private float radius;
		[SerializeField] private float thickness;
		[SerializeField] private KeyCode generateHandleKey = KeyCode.K;
		[SerializeField] private Transform colliderTransform;
		
		private MeshCollider _meshCollider;
		private MeshFilter _meshFilter;

		private void Awake()
		{
			_meshCollider = colliderTransform.GetComponent<MeshCollider>();
			_meshFilter = colliderTransform.GetComponent<MeshFilter>();
		}

		private void Start()
		{
			UpdateCollider();
			
			// If no keyCode set, disable the component for optimization
			if (generateHandleKey == KeyCode.None)
			{
				enabled = false;
			}
		}
		
		private void Update()
		{
			if (Input.GetKeyDown(generateHandleKey))
			{
				UpdateCollider();
			}
		}

		private Mesh UpdateCollider()
		{
			var newMesh = MeshUtils.CreateTorus(radius, thickness, segmentCount, sideCount);
			newMesh.name = "torus";
			
			_meshFilter.sharedMesh = newMesh;
			_meshCollider.sharedMesh = newMesh;
			
			return newMesh;
		}

		[ContextMenu("Generate torus and save to asset")]
		private void SaveAsset()
		{
			_meshCollider = colliderTransform.GetComponent<MeshCollider>();
			_meshFilter = colliderTransform.GetComponent<MeshFilter>();
			var newMesh = UpdateCollider();
			AssetDatabase.CreateAsset(newMesh, $"Assets/torus_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.asset");
		}
	}
}