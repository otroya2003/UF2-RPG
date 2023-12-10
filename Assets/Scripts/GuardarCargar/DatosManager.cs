using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DatosManager : MonoBehaviour
{
    public PlayerController jugador;
    public Player2Controller jugador2;

    private Vector3 posIniJ1;
    private Vector3 posIniJ2;

    public string archivoDeGuardado;

    public DatosJuego datosJuego = new DatosJuego();

    private void Awake()
    {
        archivoDeGuardado = Application.dataPath + "/datosJuego.json";

        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        jugador2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player2Controller>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GuardarDatos();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CargarDatos();
        }
    }

    private void CargarDatos()
    {
        if (File.Exists(archivoDeGuardado))
        {
            string contenido = File.ReadAllText(archivoDeGuardado);
            datosJuego = JsonUtility.FromJson<DatosJuego>(contenido);

            jugador.transform.position = datosJuego.posicionJ1;
            jugador.movePoint.position = posIniJ1;
            jugador2.transform.position = datosJuego.posicionJ2;
            jugador2.movePoint.position = posIniJ2;

            jugador.stats.health = datosJuego.health1;
            jugador.stats.attack = datosJuego.attack1;
            jugador.stats.combo = datosJuego.combo1;
            jugador.stats.velocity = datosJuego.velocity1;

            jugador2.stats.health = datosJuego.health2;
            jugador2.stats.attack = datosJuego.attack2;
            jugador2.stats.combo = datosJuego.combo2;
            jugador2.stats.velocity = datosJuego.velocity2;
        }
        else
        {
            Debug.Log("El archivo no existe");
        }
    }

    private void GuardarDatos()
    {
        DatosJuego nuevosDatos = new DatosJuego()
        {
            posicionJ1 = jugador.transform.position,
            posicionJ2 = jugador2.transform.position,
            health1 = jugador.stats.health,
            attack1 = jugador.stats.attack,
            combo1 = jugador.stats.combo,
            velocity1 = jugador.stats.velocity,
            health2 = jugador2.stats.health,
            attack2 = jugador2.stats.attack,
            combo2 = jugador2.stats.combo,
            velocity2 = jugador2.stats.velocity,
        };

        posIniJ1 = jugador.transform.position;
        posIniJ2 = jugador2.transform.position;

        string cadenaJSON = JsonUtility.ToJson(nuevosDatos);

        File.WriteAllText(archivoDeGuardado, cadenaJSON);
        Debug.Log("Archivo Guardado");
    }
}
