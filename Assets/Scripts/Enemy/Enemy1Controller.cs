using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy1Controller : MonoBehaviour
{
    private enum States { IDLE, WALK, HIT}
    private States m_CurrentState;

    private Animator m_Animator;
    private SpriteRenderer sr;

    private float m_StateDeltaTime;

    public static Action<int> lvlJ1_txt;
    public static Action<int> lvlJ2_txt;
    public static Action<int> expJ1_txt;
    public static Action<int> expJ2_txt;

    [SerializeField] private EnemyStats m_stats;
    [SerializeField] private EnemyDamageController m_damageController;

    [SerializeField] private CharacterStats m_characterStats;
    [SerializeField] private CharacterStats m_characterStats2;
    private int health;

    private bool areaDetector = false;
    private bool hittable = false;


    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        health = m_stats.health;
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
                m_Animator.Play("Walk");
                break;
            case States.HIT:
                m_Animator.Play("Attack");
                m_damageController.damage = m_stats.attack;
                break;
            default:
                break;
        }
    }


    private void UpdateState(States updateState)
    {

        m_StateDeltaTime += Time.deltaTime;

        switch (updateState)
        {
            case States.IDLE:

                if (areaDetector)
                {
                    ChangeState(States.WALK);
                }
                if (hittable)
                {
                    ChangeState(States.HIT);
                }
                break;

            case States.WALK:

                if (hittable)
                {
                    ChangeState(States.HIT);
                }
                if (m_StateDeltaTime >= 0.5f)
                    ChangeState(States.IDLE);

                break;

            case States.HIT:

                if (m_StateDeltaTime >= 0.7f)
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
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            int damage = collision.gameObject.GetComponent<damageController>().damage;

            if (health - damage > 0)
            {
                health -= damage;
            }
            else
            {
                if (GameManager.turno)
                {
                    if (m_characterStats.experiencia+100 < 150)
                    {
                        m_characterStats.experiencia += 100;
                        expJ1_txt.Invoke(m_characterStats.experiencia);
                    }
                    else
                    {
                        m_characterStats.experiencia = 0;
                        m_characterStats.nivel++;
                        expJ1_txt.Invoke(m_characterStats.experiencia);
                        lvlJ1_txt.Invoke(m_characterStats.nivel);
                    }
                }
                else
                {
                    if (m_characterStats2.experiencia + 100 < 150)
                    {
                        m_characterStats2.experiencia += 100;
                        expJ1_txt.Invoke(m_characterStats2.experiencia);
                    }
                    else
                    {
                        m_characterStats2.experiencia = 0;
                        m_characterStats2.nivel++;
                        expJ1_txt.Invoke(m_characterStats2.experiencia);
                        lvlJ1_txt.Invoke(m_characterStats2.nivel);
                    }
                }
                SceneManager.LoadScene("Main");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            
            areaDetector = true;

            float distancia = Vector3.Distance(collision.gameObject.transform.position, transform.position);

            Vector3 direccion = collision.gameObject.transform.position - transform.position;
            direccion.Normalize();

            if (direccion.x > 0)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }

            if (distancia >= 2)
            {
                transform.position += direccion * m_stats.velocity * Time.deltaTime;
            }

            if (distancia < 2)
            {
                transform.position = transform.position;
                hittable = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            areaDetector = false;
        }
    }
}
