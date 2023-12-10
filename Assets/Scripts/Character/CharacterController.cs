using System;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    private enum States { IDLE, WALK, HIT, COMBO }
    private States m_CurrentState;

    private Animator m_Animator;
    private SpriteRenderer sr;

    private float m_StateDeltaTime;

    //[SerializeField] private GameManager gameManager;

    [SerializeField] private CharacterStats m_stats;
    [SerializeField] private CharacterStats m_stats2;
    [SerializeField] private damageController m_damageController;
    private int health;
    private PlayerActions m_playerActions;
    private Rigidbody2D m_rigidBody;

    private AudioSource sword;

    private void Awake()
    {
        m_playerActions = new PlayerActions();
    }

    private void OnEnable()
    {
        m_playerActions.Player1_map1.Enable();
        m_playerActions.Player1_map1.Atack.performed += AttackAction;
    }

    private void OnDisable()
    {
        m_playerActions.Player1_map1.Disable();
    }

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();

        if (GameManager.turno)
        {
            health = m_stats.health;
        }
        else{
            health = m_stats2.health;
        }
        sword = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState(m_CurrentState);
    }
    private void ChangeState(States newState)
    {
        if (newState == m_CurrentState)
            return;

        ExitState(m_CurrentState);
        InitState(newState);
    }

    private void InitState(States initState)
    {
        m_CurrentState = initState;
        m_StateDeltaTime = 0;

        switch (m_CurrentState)
        {
            case States.IDLE:
                m_Animator.Play("Idle");
                break;
            case States.WALK:
                if (m_rigidBody.velocity.x > 0)
                {
                    sr.flipX = false;
                }
                else
                {
                    sr.flipX = true;
                }

                m_Animator.Play("Run");
                break;
            case States.HIT:
                m_Animator.Play("Attack1");
                sword.Play();
                if (GameManager.turno)
                {
                    m_damageController.damage = m_stats.attack;
                }
                else
                {
                    m_damageController.damage = m_stats2.attack;
                }
                break;
            case States.COMBO:
                m_Animator.Play("Attack2");
                sword.Play();
                m_damageController.damage = m_stats.combo;
                break;
            default:
                break;
        }
    }


    private void UpdateState(States updateState)
    {
        
        m_StateDeltaTime += Time.deltaTime;
        if (GameManager.turno)
        {
            m_rigidBody.velocity = m_playerActions.Player1_map1.Move2.ReadValue<Vector2>() * m_stats.velocity;
        }
        else
        {
            m_rigidBody.velocity = m_playerActions.Player1_map1.Move2.ReadValue<Vector2>() * m_stats2.velocity;
        }

        switch (updateState)
        {
            case States.IDLE:
                if (m_playerActions.Player1_map1.Move2.ReadValue<Vector2>() != Vector2.zero)
                    ChangeState(States.WALK);
                break;

            case States.WALK:

                if (m_StateDeltaTime >= 0.5f && m_playerActions.Player1_map1.Move2.ReadValue<Vector2>() == Vector2.zero)
                    ChangeState(States.IDLE);

                break;

            case States.HIT:

                if (m_StateDeltaTime >= 0.5f)
                {
                    ChangeState(States.IDLE);
                }
                else if(m_playerActions.Player1_map1.Combo.IsPressed() && m_StateDeltaTime < 0.5f)
                {
                    ChangeState(States.COMBO);
                }

                break;
            case States.COMBO:

                if (m_StateDeltaTime >= 0.5f)
                {
                    ChangeState(States.IDLE);
                }

                break;
            default:
                break;
        }
    }

    private void ExitState(States exitState)
    {
        switch (exitState)
        {
            case States.IDLE:
                break;
            case States.WALK:
                break;
            case States.HIT:
                break;
            case States.COMBO:
                break;
            default:
                break;
        }
    }

    private void AttackAction(InputAction.CallbackContext callbackContext)
    {
        ChangeState(States.HIT);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            int damage = collision.gameObject.GetComponent<EnemyDamageController>().damage;
            if (health - damage > 0)
            {
                health -= damage;
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
