using System;
using System.Data.SQLite;
using System.IO;
using System.Linq; // Necesario para .Any()

public static class DatabaseHelper
{
    private static readonly string connectionString = "Data Source=Data/crucigrama.db;Version=3;";

    public static SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(connectionString);
    }

    public static void InitializeDatabase()
    {
        // Asegura que la carpeta 'Data' exista
        var dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }

        using (var conn = GetConnection())
        {
            conn.Open();

            var cmd = new SQLiteCommand(conn);

            // Tabla de palabras
            // ¡IMPORTANTE! Si ya tienes la base de datos creada, tendrás que borrar el archivo
            // 'crucigrama.db' en la carpeta 'Data' para que se recree con la nueva columna 'Pista'.
            // O bien, ejecutar un ALTER TABLE manualmente.
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Palabras (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Texto TEXT NOT NULL UNIQUE,
                                    Longitud INTEGER NOT NULL,
                                    Pista TEXT NOT NULL
                                )";
            cmd.ExecuteNonQuery();

            // Tabla de relación letra-palabra, con restricción única por letra y posición
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS LetraPalabra (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Letra CHAR(1) NOT NULL,
                                    Posicion INTEGER NOT NULL,
                                    PalabraId INTEGER NOT NULL,
                                    FOREIGN KEY(PalabraId) REFERENCES Palabras(Id),
                                    UNIQUE(Letra, Posicion)
                                )";
            cmd.ExecuteNonQuery();

            // Insertar palabras de ejemplo si la tabla está vacía
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
                    ("ENCAPSULAMIENTO", "Ocultamiento de detalles internos de un objeto.")
                };

                foreach (var (texto, pista) in palabrasEjemplo)
                {
                    cmd.CommandText = "INSERT INTO Palabras (Texto, Longitud, Pista) VALUES (@texto, @longitud, @pista)";
                    cmd.Parameters.AddWithValue("@texto", texto);
                    cmd.Parameters.AddWithValue("@longitud", texto.Length);
                    cmd.Parameters.AddWithValue("@pista", pista);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }

                // Asociar solo una palabra por cada letra/posición de la palabra raíz
                string palabraRaiz = "PROGRAMACION"; // Puedes elegir otra palabra raíz si quieres
                for (int i = 0; i < palabraRaiz.Length && i < palabrasEjemplo.Length; i++)
                {
                    // Obtener el Id de la palabra
                    cmd.CommandText = "SELECT Id FROM Palabras WHERE Texto = @texto";
                    cmd.Parameters.AddWithValue("@texto", palabrasEjemplo[i].texto);
                    var palabraIdObj = cmd.ExecuteScalar();
                    if (palabraIdObj == DBNull.Value || palabraIdObj == null) continue; // Asegurarse de que existe
                    var palabraId = Convert.ToInt32(palabraIdObj);
                    cmd.Parameters.Clear();

                    // Insertar solo si no existe ya una asociación para esa letra/posición
                    cmd.CommandText = @"INSERT OR IGNORE INTO LetraPalabra (Letra, Posicion, PalabraId)
                                         VALUES (@letra, @pos, @palabraId)";
                    cmd.Parameters.AddWithValue("@letra", palabraRaiz[i]);
                    cmd.Parameters.AddWithValue("@pos", i);
                    cmd.Parameters.AddWithValue("@palabraId", palabraId);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }
    }
}