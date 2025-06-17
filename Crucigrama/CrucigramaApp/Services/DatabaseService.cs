using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO; // Asegúrate de tener esto

public class DatabaseService
{
    private static readonly string connectionString = "Data Source=Data/crucigrama.db;Version=3;";

    public static SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(connectionString);
    }

    public static void InitializeDatabase()
    {
        var dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }

        using (var conn = GetConnection())
        {
            conn.Open();

            var cmd = new SQLiteCommand(conn);

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Palabras (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Texto TEXT NOT NULL UNIQUE,
                                    Longitud INTEGER NOT NULL,
                                    Pista TEXT NOT NULL
                                )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS LetraPalabra (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Letra CHAR(1) NOT NULL,
                                    Posicion INTEGER NOT NULL,
                                    PalabraId INTEGER NOT NULL,
                                    FOREIGN KEY(PalabraId) REFERENCES Palabras(Id),
                                    UNIQUE(Letra, Posicion, PalabraId) -- Agregamos PalabraId para unicidad correcta
                                )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT COUNT(*) FROM Palabras";
            var count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count == 0)
            {
                var palabrasEjemplo = new (string texto, string pista)[] {
                    ("PROGRAMACION", "Arte de escribir código."),
                    ("COMPUTADORA", "Máquina electrónica para procesar información."),
                    ("ALGORITMO", "Serie de pasos para resolver un problema."),
                    ("BASE", "Fundamento, apoyo o conjunto de datos."),
                    ("DATOS", "Información procesada o sin procesar."),
                    ("CODIGO", "Conjunto de instrucciones para un programa."),
                    ("VARIABLE", "Espacio de memoria que puede cambiar su valor."),
                    ("FUNCION", "Bloque de código que realiza una tarea específica."),
                    ("CLASE", "Molde para crear objetos."),
                    ("OBJETO", "Instancia de una clase."),
                    ("METODO", "Acción o comportamiento de un objeto."),
                    ("INTERFAZ", "Punto de conexión o comunicación."),
                    ("HERENCIA", "Mecanismo para reutilizar código en clases."),
                    ("POLIMORFISMO", "Capacidad de un objeto de tomar múltiples formas."),
                    ("ENCAPSULAMIENTO", "Ocultamiento de detalles internos de un objeto."),
                    ("SOFTWARE", "Programas de una computadora."),
                    ("HARDWARE", "Componentes físicos de una computadora."),
                    ("RED", "Conexión de computadoras."),
                    ("INTERNET", "Red global de computadoras."),
                    ("SERVIDOR", "Computadora que provee servicios."),
                    ("CLIENTE", "Computadora que consume servicios."),
                    ("BYTE", "Unidad de información digital."),
                    ("BIT", "Unidad mínima de información digital."),
                    ("BUG", "Error en un programa."),
                    ("DEBUG", "Proceso de eliminar errores.")
                };

                foreach (var (texto, pista) in palabrasEjemplo)
                {
                    cmd.CommandText = "INSERT INTO Palabras (Texto, Longitud, Pista) VALUES (@texto, @longitud, @pista)";
                    cmd.Parameters.AddWithValue("@texto", texto);
                    cmd.Parameters.AddWithValue("@longitud", texto.Length);
                    cmd.Parameters.AddWithValue("@pista", pista);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    // Insertar letras en LetraPalabra para cada palabra agregada
                    long lastId = conn.LastInsertRowId; // Obtener el ID de la última palabra insertada
                    for (int i = 0; i < texto.Length; i++)
                    {
                        cmd.CommandText = @"INSERT OR IGNORE INTO LetraPalabra (Letra, Posicion, PalabraId)
                                             VALUES (@letra, @pos, @palabraId)";
                        cmd.Parameters.AddWithValue("@letra", texto[i]);
                        cmd.Parameters.AddWithValue("@pos", i);
                        cmd.Parameters.AddWithValue("@palabraId", lastId);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
            }
        }
    }

    // ¡CAMBIO AQUÍ! Nombre del método y parámetros
    public async Task<Palabra?> ObtenerPalabraAleatoriaConRangoLongitudAsync(int longitudMinima, int longitudMaxima)
    {
        using (var conn = GetConnection())
        {
            await conn.OpenAsync();

            var cmd = new SQLiteCommand(conn);
            // La consulta ahora busca palabras cuya Longitud esté entre el mínimo y el máximo
            cmd.CommandText = "SELECT Id, Texto, Longitud, Pista FROM Palabras WHERE Longitud >= @minLongitud AND Longitud <= @maxLongitud ORDER BY RANDOM() LIMIT 1";
            cmd.Parameters.AddWithValue("@minLongitud", longitudMinima);
            cmd.Parameters.AddWithValue("@maxLongitud", longitudMaxima);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Palabra
                    {
                        Id = reader.GetInt32(0),
                        Texto = reader.GetString(1),
                        Longitud = reader.GetInt32(2),
                        Pista = reader.GetString(3)
                    };
                }
            }
        }
        return null;
    }

    public async Task<Palabra?> BuscarPalabraConLetraAsync(char letra, string excluirPalabra, List<string>? excluirAdicionales = null)
    {
        using (var conn = GetConnection())
        {
            await conn.OpenAsync();

            var cmd = new SQLiteCommand(conn);

            string query = "SELECT Id, Texto, Longitud, Pista FROM Palabras WHERE Texto LIKE @letra AND Texto != @excluir";

            if (excluirAdicionales != null && excluirAdicionales.Any())
            {
                var excludeParams = new List<string>();
                for (int i = 0; i < excluirAdicionales.Count; i++)
                {
                    string paramName = $"@excluirAdd{i}";
                    query += $" AND Texto != {paramName}";
                    cmd.Parameters.AddWithValue(paramName, excluirAdicionales[i]);
                }
            }

            query += " ORDER BY RANDOM() LIMIT 1";

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@letra", $"%{letra}%");
            cmd.Parameters.AddWithValue("@excluir", excluirPalabra);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Palabra
                    {
                        Id = reader.GetInt32(0),
                        Texto = reader.GetString(1),
                        Longitud = reader.GetInt32(2),
                        Pista = reader.GetString(3)
                    };
                }
            }
        }
        return null;
    }

    public async Task<List<Palabra>> ObtenerTodasLasPalabrasAsync()
    {
        var palabras = new List<Palabra>();

        using (var conn = GetConnection())
        {
            await conn.OpenAsync();

            var cmd = new SQLiteCommand("SELECT Id, Texto, Longitud, Pista FROM Palabras ORDER BY Texto", conn);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    palabras.Add(new Palabra
                    {
                        Id = reader.GetInt32(0),
                        Texto = reader.GetString(1),
                        Longitud = reader.GetInt32(2),
                        Pista = reader.GetString(3)
                    });
                }
            }
        }

        return palabras;
    }

    public async Task AgregarPalabraAsync(string palabra, string pista)
    {
        using (var conn = GetConnection())
        {
            await conn.OpenAsync();

            var cmd = new SQLiteCommand(conn);
            cmd.CommandText = "INSERT INTO Palabras (Texto, Longitud, Pista) VALUES (@texto, @longitud, @pista)";
            cmd.Parameters.AddWithValue("@texto", palabra.ToUpper());
            cmd.Parameters.AddWithValue("@longitud", palabra.Length);
            cmd.Parameters.AddWithValue("@pista", pista);

            await cmd.ExecuteNonQueryAsync();

            // Insertar letras en LetraPalabra para la nueva palabra
            long lastId = conn.LastInsertRowId;
            for (int i = 0; i < palabra.Length; i++)
            {
                cmd.CommandText = @"INSERT OR IGNORE INTO LetraPalabra (Letra, Posicion, PalabraId)
                                     VALUES (@letra, @pos, @palabraId)";
                cmd.Parameters.AddWithValue("@letra", palabra.ToUpper()[i]);
                cmd.Parameters.AddWithValue("@pos", i);
                cmd.Parameters.AddWithValue("@palabraId", lastId);
                await cmd.ExecuteNonQueryAsync();
                cmd.Parameters.Clear();
            }
        }
    }
}