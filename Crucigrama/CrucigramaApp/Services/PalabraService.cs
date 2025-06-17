using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class PalabraService
{
    private readonly DatabaseService _databaseService;

    public PalabraService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    // ¡CAMBIO AQUÍ! Ahora recibimos longitudMinima y longitudMaxima
    public async Task<Palabra> ObtenerPalabraRaizAleatoriaAsync(int longitudMinima, int longitudMaxima)
    {
        // Pasamos ambos parámetros a DatabaseService
        return await _databaseService.ObtenerPalabraAleatoriaConRangoLongitudAsync(longitudMinima, longitudMaxima) ??
            throw new Exception("No se encontró palabra raíz con el rango de longitud especificado. Asegúrate de tener palabras en la base de datos.");
    }

    public async Task<Palabra?> BuscarPalabraConLetraAsync(char letra, string excluirPalabra, List<string>? excluirAdicionales = null)
    {
        return await _databaseService.BuscarPalabraConLetraAsync(letra, excluirPalabra, excluirAdicionales);
    }

    public async Task<List<Palabra>> ObtenerTodasLasPalabrasAsync()
    {
        return await _databaseService.ObtenerTodasLasPalabrasAsync();
    }

    public async Task AgregarPalabraAsync(string palabra, string pista)
    {
        await _databaseService.AgregarPalabraAsync(palabra, pista);
    }
}