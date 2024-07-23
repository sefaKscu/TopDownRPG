// StashChest v0.2

/* Version History
 *  v0.1
 * - ToggleStash() method implemented
 * - isOpen bool implemented
 *  v0.2
 * - Event Subscription bug has been resolved
 * - isOpen bool is checked in both OpenStash() and CloseStash() methods.
 * - Early return implemented to both OpenStash() and CloseStash() methods.
 */


using Unity.VisualScripting;
using UnityEngine;


namespace InventorySystem
{
    public class StashChest : MonoBehaviour
    {
        [SerializeField] ItemStash stash;
        [SerializeField] InventoryManager inventoryManager;

        private Animator animator;

        [SerializeField, Range(.1f, 5f)] private float radius = 1f;

        private CircleCollider2D circleCollider;

        private bool isInRange = false;

        private bool isOpen = false;



        private void Start()
        {
            GetAndSetComponents();
        }

        private void GetAndSetComponents()
        {
            animator = GetComponent<Animator>();
            circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.radius = radius;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E) && isInRange)
            {
                ToogleStash();
            }

        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            CheckCollision(collision, true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            CheckCollision(collision, false);
            if (isOpen)
            {
                CloseStash();
            }
        }

        private void CheckCollision(Collider2D _collision, bool _state)
        {
            Player _player = _collision.gameObject.GetComponent<Player>();
            if (_player != null)
            {
                isInRange= _state;
            }
        }

        private void ToogleStash()
        {
            if(isOpen) { CloseStash(); }
            else { OpenStash(); }
        }

        private void OpenStash()
        {
            // Check if it is allready open, if so return early. This is for preventing subscription to desync
            if (isOpen) return;
            isOpen= true;
            HandleAnimation();
            UIManager.MyInstance.OpenMenu(2); // open inventory panel
            UIManager.MyInstance.OpenMenu(3); // open stash panel
            inventoryManager.SetSubscriptionsOnOpenItemStash();
        }

        private void CloseStash()
        {
            // Check if it is allready closed, if so return early. This is for preventing subscription to desync
            if (!isOpen) return;
            isOpen = false;
            HandleAnimation();
            //UIManager.MyInstance.CloseMenu(2); // close inventory panel
            UIManager.MyInstance.CloseMenu(3); // close stash panel
            inventoryManager.SetSubscriptionsOnCloseItemStash();
        }

        private void HandleAnimation()
        {
            animator.SetBool("ChestOpen", isOpen);
        }

        //Depricated Code
        //private void HandleUI()
        //{
        //    if (isOpen)
        //    {
        //        UIManager.MyInstance.OpenMenu(2); // open inventory panel
        //        UIManager.MyInstance.OpenMenu(3); // open stash panel
        //    }
        //    else
        //    {
        //        //UIManager.MyInstance.CloseMenu(2); // close inventory panel
        //        UIManager.MyInstance.CloseMenu(3); // close stash panel
        //    }
        //}
    }
}
