using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player2Controller : MonoBehaviour
{
    public float moveSpeed;
    public Transform movePoint;
    public LayerMask nonWalkableLayer;
    public Animator m_animator;

    private bool m_interactable = false;

    private Player2Action m_playerActions;

    public delegate void useMovement();
    public static useMovement _useMovement;

    public CharacterStats stats;

    private void Awake()
    {
        m_playerActions = new Player2Action();
    }

    private void OnEnable()
    {
        m_playerActions.Player2_map1.Enable();
        m_playerActions.Player2_map1.Interact.performed += InteractAction;
    }

    private void OnDisable()
    {
        m_playerActions.Player2_map1.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {

        float playerMoveX = m_playerActions.Player2_map1.Move.ReadValue<Vector2>().x;
        float playerMoveY = m_playerActions.Player2_map1.Move.ReadValue<Vector2>().y;

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.5f)
        {
            if (Mathf.Abs(playerMoveX) == 1)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(playerMoveX, 0, 0), 0.2f, nonWalkableLayer))
                {
                    if (!GameManager.turno)
                    {
                        movePoint.position += new Vector3(playerMoveX, 0, 0);
                        _useMovement.Invoke();
                    }
                }
            }
            if (Mathf.Abs(playerMoveY) == 1)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, playerMoveY, 0), 0.2f, nonWalkableLayer))
                {
                    if (!GameManager.turno)
                    {
                        movePoint.position += new Vector3(0, playerMoveY, 0);
                        _useMovement.Invoke();
                    }
                }
            }
            m_animator.SetBool("movement", false);
        }
        else
        {
            m_animator.SetBool("movement", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            m_interactable = true;
        }
        if (collision.gameObject.layer == 14)
        {
            stats.attack += 20;
            stats.combo += 40;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.layer == 15)
        {
            stats.attack += 5;
            stats.combo += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.layer == 15)
        {
            stats.attack += 10;
            stats.combo += 20;
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            m_interactable = false;
        }
    }

    private void InteractAction(InputAction.CallbackContext callbackContext)
    {
        if (m_interactable && !GameManager.turno)
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
