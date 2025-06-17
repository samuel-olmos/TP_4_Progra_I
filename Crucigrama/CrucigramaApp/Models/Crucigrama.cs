using System.Collections.Generic;
using System.Linq;
using System; // Agregado para Math

public class Crucigrama
{
    public Palabra PalabraRaiz { get; set; } = new Palabra();
    public List<Palabra> PalabrasSecundarias { get; set; } = new List<Palabra>();
    public char[,] Matriz { get; set; } = new char[0, 0];
    // Diccionario para almacenar la posición (x,y) y orientación (horizontal) de cada palabra en la matriz
    public Dictionary<Palabra, (int x, int y, bool horizontal)> Posiciones { get; set; } =
        new Dictionary<Palabra, (int x, int y, bool horizontal)>();

    public void ConstruirMatriz()
    {
        // Si no hay palabra raíz, no se puede construir el crucigrama
        if (PalabraRaiz == null || string.IsNullOrEmpty(PalabraRaiz.Texto))
        {
            Matriz = new char[0, 0];
            Posiciones.Clear();
            return;
        }

        // El alto de la matriz siempre será el largo de la palabra raíz
        int alto = PalabraRaiz.Texto.Length;

        // --- INICIO DE LA MODIFICACIÓN CLAVE ---
        // Calcular el ancho necesario para acomodar todas las palabras horizontales.
        // Empezamos con un ancho mínimo que permita la palabra raíz (que es vertical).
        int minAnchoNecesario = 1; // Para la columna de la palabra raíz

        // Para cada palabra secundaria, calcula la extensión máxima a izquierda y derecha
        int maxExtensionIzquierda = 0;
        int maxExtensionDerecha = 0;

        foreach (var palabra in PalabrasSecundarias)
        {
            // Encontrar la letra común que conecta la palabra secundaria con la palabra raíz
            char letraComun = palabra.Letras.FirstOrDefault(c => PalabraRaiz.Letras.Contains(c));
            if (letraComun == default) continue; // Si no hay letra común, ignorar esta palabra

            int posicionEnSecundaria = palabra.PosicionLetra(letraComun);

            // Calcular cuántos caracteres necesita a la izquierda y derecha del punto de cruce
            int extensionIzquierda = posicionEnSecundaria; // Caracteres a la izquierda del cruce
            int extensionDerecha = palabra.Texto.Length - 1 - posicionEnSecundaria; // Caracteres a la derecha del cruce

            // Actualizar las extensiones máximas
            maxExtensionIzquierda = Math.Max(maxExtensionIzquierda, extensionIzquierda);
            maxExtensionDerecha = Math.Max(maxExtensionDerecha, extensionDerecha);
        }

        // El ancho total necesario es la extensión izquierda + 1 (para la columna central) + la extensión derecha
        int ancho = maxExtensionIzquierda + 1 + maxExtensionDerecha;

        // Asegurarse de que el ancho sea al menos el largo de la palabra raíz si esta fuera horizontal
        // (Aunque la raíz es vertical, esto previene anchos muy pequeños en crucigramas simples).
        // Y asegurarnos de un mínimo razonable si no hay palabras secundarias o son muy cortas.
        ancho = Math.Max(ancho, PalabraRaiz.Longitud); // Si la palabra raíz es muy larga
        ancho = Math.Max(ancho, 5); // Un ancho mínimo razonable para la visualización

        // --- FIN DE LA MODIFICACIÓN CLAVE ---


        int xRaiz = maxExtensionIzquierda; // La columna de la palabra raíz será donde termina la extensión izquierda.

        Matriz = new char[alto, ancho];

        // Inicializar matriz con caracteres nulos ('\0')
        for (int y = 0; y < alto; y++)
        {
            for (int x = 0; x < ancho; x++)
            {
                Matriz[y, x] = '\0';
            }
        }

        // Colocar palabra raíz verticalmente
        for (int y = 0; y < PalabraRaiz.Texto.Length; y++)
        {
            Matriz[y, xRaiz] = PalabraRaiz.Letras[y];
        }
        // Guardar la posición de la palabra raíz (columna xRaiz, fila 0, no horizontal)
        Posiciones[PalabraRaiz] = (xRaiz, 0, false);

        // Colocar palabras secundarias horizontalmente
        foreach (var palabra in PalabrasSecundarias)
        {
            char letraComun = palabra.Letras.FirstOrDefault(c => PalabraRaiz.Letras.Contains(c));
            if (letraComun == default) continue;

            int posicionEnRaiz = PalabraRaiz.PosicionLetra(letraComun); // Fila donde cruza
            int posicionEnSecundaria = palabra.PosicionLetra(letraComun); // Posición de la letra común en la palabra secundaria

            // La columna de inicio de la palabra secundaria se calcula a partir de xRaiz y la posición de la letra común
            int startX = xRaiz - posicionEnSecundaria;

            // Asegurarse de que la palabra secundaria no se salga de los límites de la matriz
            if (posicionEnRaiz >= 0 && posicionEnRaiz < alto)
            {
                for (int x = 0; x < palabra.Texto.Length; x++)
                {
                    int currentX = startX + x;
                    if (currentX >= 0 && currentX < ancho)
                    {
                        // Colocar la letra si la celda está vacía o es la letra de cruce
                        if (Matriz[posicionEnRaiz, currentX] == '\0' || Matriz[posicionEnRaiz, currentX] == palabra.Letras[x])
                        {
                            Matriz[posicionEnRaiz, currentX] = palabra.Letras[x];
                        }
                    }
                }
                // Guardar la posición de la palabra secundaria (columna startX, fila posicionEnRaiz, horizontal)
                Posiciones[palabra] = (startX, posicionEnRaiz, true);
            }
        }
    }
}