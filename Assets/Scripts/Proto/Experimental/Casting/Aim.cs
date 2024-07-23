using UnityEngine;

namespace Experimental.Casting
{
    public class Aim : MonoBehaviour
    {
        private Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            cam= Camera.main; // this should be changed
        }

        // Update is called once per frame
        void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            Vector3 mousePosition = GetMousePosition();
            Vector3 rotation = CalculateRotation(mousePosition);
            float rotZ = ConvertRotationToDegrees(rotation);
            SetRotation(rotZ);
        }

        #region Micro Methods
        private Vector3 CalculateRotation(Vector3 mousePosition)
        {
            return mousePosition - transform.position;
        }

        private Vector3 GetMousePosition()
        {
            return cam.ScreenToWorldPoint(Input.mousePosition);
        }

        private float ConvertRotationToDegrees(Vector3 rotation)
        {
            return Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        }

        private void SetRotation(float rotZ)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }
        #endregion
    }
}