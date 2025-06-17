using System;

public class Palabra
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int Longitud { get; set; }
    public string Pista { get; set; } = string.Empty; // ¡Propiedad de pista!

    // Propiedad para obtener las letras del texto como un array de caracteres en mayúsculas
    public char[] Letras => Texto.ToUpper().ToCharArray();

    // Método para encontrar la posición de una letra específica en la palabra
    public int PosicionLetra(char letra)
    {
        return Array.IndexOf(Letras, char.ToUpper(letra));
    }
}