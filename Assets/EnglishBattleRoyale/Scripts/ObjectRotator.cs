using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour
{

	private float _sensitivity;
	private Vector3 _mouseReference;
	private Vector3 _mouseOffset;
	private Vector3 _rotation;
	private bool _isRotating;
	public GameObject objectRotate;
	public Camera uiCamera;



	void Start ()
	{
		_sensitivity = 0.5f;
		_rotation = Vector3.zero;
	}


	public void OnKeyDown(){
		_isRotating = true;
		_mouseReference = Input.mousePosition;
	}

	public void OnKeyUp(){
		_isRotating = false;
	}

	void Update(){
		if (_isRotating) {
			// offset
			_mouseOffset = (Input.mousePosition - _mouseReference);

			// apply rotation
			_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

			// rotate
			objectRotate.transform.Rotate (_rotation);

			// store mouse
			_mouseReference = Input.mousePosition;
		}
	}
		

}