using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class GeneradorCrucigrama
{
    private readonly PalabraService _palabraService;
    private readonly Random _random = new Random();

    public GeneradorCrucigrama(PalabraService palabraService)
    {
        _palabraService = palabraService;
    }

    public async Task<Crucigrama> GenerarCrucigramaAsync(int longitudMinimaRaiz = 3, int longitudMaximaRaiz = 10)
    {
        var crucigrama = new Crucigrama();

        // 1. Obtener palabra raíz aleatoria con rango de longitud
        crucigrama.PalabraRaiz = await _palabraService.ObtenerPalabraRaizAleatoriaAsync(longitudMinimaRaiz, longitudMaximaRaiz) ??
            throw new Exception("No se encontraron palabras en la base de datos que cumplan con el rango de longitud especificado para la palabra raíz.");

        // Definir la cantidad de palabras secundarias deseada
        int palabrasSecundariasDeseadas = 2 + (int)Math.Ceiling(crucigrama.PalabraRaiz.Longitud / 3.0);
        palabrasSecundariasDeseadas = Math.Min(palabrasSecundariasDeseadas, crucigrama.PalabraRaiz.Longitud);
        palabrasSecundariasDeseadas = Math.Max(palabrasSecundariasDeseadas, 2);

        // 2. Recorrer cada letra de la palabra raíz para buscar cruces
        int palabrasEncontradas = 0;
        HashSet<string> palabrasYaUsadas = new HashSet<string> { crucigrama.PalabraRaiz.Texto };

        // ¡NUEVO! HashSet para registrar las posiciones (índices) de la palabra raíz que ya tienen un cruce.
        HashSet<int> posicionesRaizUsadasParaCruce = new HashSet<int>();

        // Creamos una lista de índices de las letras de la palabra raíz para barajarla
        List<int> indicesLetrasRaiz = Enumerable.Range(0, crucigrama.PalabraRaiz.Longitud).ToList();
        indicesLetrasRaiz = indicesLetrasRaiz.OrderBy(x => _random.Next()).ToList(); // Barajar

        foreach (int i in indicesLetrasRaiz)
        {
            if (palabrasEncontradas >= palabrasSecundariasDeseadas)
            {
                break; // Ya encontramos suficientes palabras secundarias
            }

            // ¡NUEVO! Si esta posición de la palabra raíz ya fue usada para un cruce, saltamos.
            if (posicionesRaizUsadasParaCruce.Contains(i))
            {
                continue;
            }

            char letraActual = crucigrama.PalabraRaiz.Letras[i];

            // 3. Buscar palabra que contenga la letra actual, excluyendo la raíz y las ya añadidas
            // No limitamos el largo de la palabra secundaria aquí, se busca cualquiera que cruce
            var palabraSecundaria = await _palabraService.BuscarPalabraConLetraAsync(
                letraActual,
                crucigrama.PalabraRaiz.Texto,
                palabrasYaUsadas.ToList());

            if (palabraSecundaria != null)
            {
                int posicionEnSecundaria = palabraSecundaria.PosicionLetra(letraActual);

                if (posicionEnSecundaria >= 0)
                {
                    crucigrama.PalabrasSecundarias.Add(palabraSecundaria);
                    palabrasYaUsadas.Add(palabraSecundaria.Texto);
                    palabrasEncontradas++;

                    // ¡NUEVO! Marcar esta posición de la palabra raíz como usada para un cruce.
                    posicionesRaizUsadasParaCruce.Add(i);
                }
            }
        }

        if (palabrasEncontradas == 0 && crucigrama.PalabraRaiz.Longitud > 0)
        {
            System.Diagnostics.Debug.WriteLine("Advertencia: No se encontraron palabras secundarias para el crucigrama generado.");
        }

        crucigrama.ConstruirMatriz();

        return crucigrama;
    }
}