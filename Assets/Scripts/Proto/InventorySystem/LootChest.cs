using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChest : MonoBehaviour
{

    [SerializeField] ItemStash stash;
    [SerializeField] InventoryManager inventoryManager;

    #region CanBeAbstract
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
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            OpenStash();
        }

    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision, true);
    }

    //OnTriggerExit is different from stash chest
    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckCollision(collision, false);
    }

    #region CanBeAbstract
    private void CheckCollision(Collider2D _collision, bool _state)
    {
        Player _player = _collision.gameObject.GetComponent<Player>();
        if (_player != null)
        {
            isInRange = _state;
        }
    }


    private void OpenStash()
    {
        // Check if it is allready open, if so return early. This is for preventing subscription to desync
        if (isOpen) return;
        isOpen = true;
        HandleAnimation();
        //random itemOnGround instantioation
    }

    private void HandleAnimation()
    {
        animator.SetBool("ChestOpen", isOpen);
    }
    #endregion
}
