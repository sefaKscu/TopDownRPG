using Experimental.Casting;
using UnityEngine;

namespace Experimental.InputSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ClickToMove : MonoBehaviour
    {
        #region References
        private Camera mainCam;
        private SpellCaster spellCaster;
        private Rigidbody2D rb2d;
        #endregion
        #region Fields
        [SerializeField] private float speed = 150f ;
        private Vector2 mouseTarget;
        #endregion

        #region MonoBehaviour Methods

        private void OnValidate()
        {
            if(spellCaster == null) 
            {
                spellCaster = gameObject.GetComponent<SpellCaster>();
            }
        }

        void Start()
        {
            mainCam = Camera.main;
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }
                
        void Update()
        {
            GetInput();
        }



        private void FixedUpdate()
        {
            Move();
        }

        #endregion

        private void GetInput()
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 tmpPos = transform.position;
                mouseTarget = transform.position - mainCam.ScreenToWorldPoint(Input.mousePosition);
                mouseTarget = -mouseTarget;
            }
            if (Input.GetMouseButtonUp(0))
            {
                mouseTarget = Vector2.zero;
            }

            //Debug.Log(mouseTarget);
        }

        private void Move()
        {
            if(mouseTarget != Vector2.zero)
            {
                rb2d.velocity = mouseTarget.normalized * speed * Time.deltaTime;
            }
            else
            {
                rb2d.velocity = Vector2.zero;
            }
            
            //transform.position = Vector3.MoveTowards(transform.position, mouseTarget, speed * Time.deltaTime);
        }

    }
}

