using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrucigramaApp
{
    public partial class MainForm : Form
    {
        private readonly GeneradorCrucigrama _generador;
        private Crucigrama? _crucigramaActual;
        private Dictionary<Point, TextBox> _celdasCrucigrama = new Dictionary<Point, TextBox>();

        // Variable para controlar el modo desarrollador
        private bool modoDesarrollador = true;

        public MainForm()
        {
            InitializeComponent();
            _generador = new GeneradorCrucigrama(new PalabraService(new DatabaseService()));
            DatabaseHelper.InitializeDatabase();
        }

        private async void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                btnGenerar.Enabled = false;

                // ¡CAMBIOS AQUÍ! Obtenemos ambos valores
                int longitudMinima = (int)numLongitudMinimaPalabraRaiz.Value;
                int longitudMaxima = (int)numLongitudMaximaPalabraRaiz.Value;

                // Asegurarse de que el mínimo no sea mayor que el máximo
                if (longitudMinima > longitudMaxima)
                {
                    MessageBox.Show("La longitud mínima no puede ser mayor que la longitud máxima.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _crucigramaActual = await _generador.GenerarCrucigramaAsync(longitudMinima, longitudMaxima);

                MostrarCrucigrama(_crucigramaActual);
                btnValidar.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar crucigrama: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnGenerar.Enabled = true;
            }
        }

        private void MostrarCrucigrama(Crucigrama crucigrama)
        {
            panelCrucigrama.Controls.Clear();
            _celdasCrucigrama.Clear();

            if (crucigrama?.Matriz == null) return;

            int filas = crucigrama.Matriz.GetLength(0);
            int columnas = crucigrama.Matriz.GetLength(1);

            int cellSize = 40;
            int margin = 5;

            panelCrucigrama.Width = columnas * (cellSize + margin) + margin;
            panelCrucigrama.Height = filas * (cellSize + margin) + margin;

            for (int y = 0; y < filas; y++)
            {
                for (int x = 0; x < columnas; x++)
                {
                    var cell = new TextBox
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Location = new Point(x * (cellSize + margin) + margin, y * (cellSize + margin) + margin),
                        TextAlign = HorizontalAlignment.Center,
                        Font = new Font("Arial", 14, FontStyle.Bold),
                        MaxLength = 1,
                        CharacterCasing = CharacterCasing.Upper,
                        ReadOnly = crucigrama.Matriz[y, x] == '\0',
                        BackColor = crucigrama.Matriz[y, x] != '\0' ? Color.White : Color.LightGray,
                        TabStop = crucigrama.Matriz[y, x] != '\0'
                    };

                    cell.Text = "";
                    cell.Name = $"cell_{x}_{y}";
                    cell.Tag = new Point(x, y);

                    if (crucigrama.Matriz[y, x] != '\0')
                    {
                        cell.TextChanged += Cell_TextChanged;
                    }

                    panelCrucigrama.Controls.Add(cell);
                    _celdasCrucigrama.Add(new Point(x, y), cell);
                }
            }

            lstPalabras.Items.Clear();
            lstPalabras.Items.Add($"Pistas:");

            var todasLasPalabrasConPos = new List<(Palabra palabra, (int x, int y, bool horizontal) posInfo)>();
            if (crucigrama.PalabraRaiz != null && crucigrama.Posiciones.ContainsKey(crucigrama.PalabraRaiz))
            {
                todasLasPalabrasConPos.Add((crucigrama.PalabraRaiz, crucigrama.Posiciones[crucigrama.PalabraRaiz]));
            }
            foreach (var palabra in crucigrama.PalabrasSecundarias)
            {
                if (crucigrama.Posiciones.TryGetValue(palabra, out var posInfo))
                {
                    todasLasPalabrasConPos.Add((palabra, posInfo));
                }
            }

            var sortedPalabras = todasLasPalabrasConPos
                .OrderBy(p => p.posInfo.horizontal)
                .ThenBy(p => p.posInfo.horizontal ? p.posInfo.y : p.posInfo.x)
                .ThenBy(p => p.posInfo.horizontal ? p.posInfo.x : p.posInfo.y)
                .ToList();

            int horizontalCounter = 1;
            int verticalCounter = 1;

            foreach (var (palabra, posInfo) in sortedPalabras)
            {
                string orientacion = posInfo.horizontal ? "Horizontal" : "Vertical";
                string numero = "";

                if (posInfo.horizontal)
                {
                    numero = $"{horizontalCounter}. ";
                    horizontalCounter++;
                }
                else
                {
                    numero = $"{verticalCounter}. ";
                    verticalCounter++;
                }

                string displayWord = modoDesarrollador ? $" [{palabra.Texto}]" : "";
                lstPalabras.Items.Add($"{numero}{orientacion}: {palabra.Pista}{displayWord}");
            }
        }

        private void Cell_TextChanged(object sender, EventArgs e)
        {
            TextBox changedCell = (TextBox)sender;
            Point cellPosition = (Point)changedCell.Tag;
            char enteredChar = changedCell.Text.Length > 0 ? changedCell.Text.ToUpper()[0] : '\0';

            changedCell.BackColor = Color.White;

            if (enteredChar != '\0')
            {
                MoveToNextCell(cellPosition);
            }
        }

        private void MoveToNextCell(Point currentPosition)
        {
            if (_crucigramaActual == null) return;

            int filas = _crucigramaActual.Matriz.GetLength(0);
            int columnas = _crucigramaActual.Matriz.GetLength(1);

            int currentX = currentPosition.X;
            int currentY = currentPosition.Y;

            for (int y = currentY; y < filas; y++)
            {
                for (int x = (y == currentY ? currentX + 1 : 0); x < columnas; x++)
                {
                    if (_crucigramaActual.Matriz[y, x] != '\0')
                    {
                        TextBox nextCell = _celdasCrucigrama[new Point(x, y)];
                        if (nextCell != null)
                        {
                            nextCell.Focus();
                            nextCell.SelectAll();
                            return;
                        }
                    }
                }
            }
            btnValidar.Focus();
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            if (_crucigramaActual == null)
            {
                MessageBox.Show("Primero genera un crucigrama.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool todoCorrecto = true;
            List<Palabra> palabrasIncorrectas = new List<Palabra>();

            foreach (var kvp in _celdasCrucigrama)
            {
                if (!_crucigramaActual.Matriz[kvp.Key.Y, kvp.Key.X].Equals('\0'))
                {
                    kvp.Value.BackColor = Color.White;
                }
            }

            if (_crucigramaActual.PalabraRaiz != null && _crucigramaActual.Posiciones.ContainsKey(_crucigramaActual.PalabraRaiz))
            {
                if (!ValidarYColorearPalabra(_crucigramaActual.PalabraRaiz, _crucigramaActual.Posiciones[_crucigramaActual.PalabraRaiz]))
                {
                    palabrasIncorrectas.Add(_crucigramaActual.PalabraRaiz);
                    todoCorrecto = false;
                }
            }

            foreach (var palabra in _crucigramaActual.PalabrasSecundarias)
            {
                if (_crucigramaActual.Posiciones.TryGetValue(palabra, out var posInfo))
                {
                    if (!ValidarYColorearPalabra(palabra, posInfo))
                    {
                        palabrasIncorrectas.Add(palabra);
                        todoCorrecto = false;
                    }
                }
            }

            if (todoCorrecto)
            {
                MessageBox.Show("¡Felicidades, Capricho! Has resuelto el crucigrama correctamente.", "Crucigrama Resuelto", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string mensaje = "Aún hay errores en el crucigrama. Revisa las siguientes pistas:\n\n";
                foreach (var p in palabrasIncorrectas)
                {
                    string orientacion = _crucigramaActual.Posiciones.TryGetValue(p, out var pos) ? (pos.horizontal ? "Horizontal" : "Vertical") : "Desconocida";
                    mensaje += $"- {orientacion} (Pista: {p.Pista})\n";
                }
                MessageBox.Show(mensaje, "Revisa tu Crucigrama", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidarYColorearPalabra(Palabra palabraEsperada, (int x, int y, bool horizontal) posInfo)
        {
            bool esCorrecta = true;
            int startX = posInfo.x;
            int startY = posInfo.y;

            if (posInfo.horizontal)
            {
                for (int i = 0; i < palabraEsperada.Longitud; i++)
                {
                    Point p = new Point(startX + i, startY);
                    if (_celdasCrucigrama.ContainsKey(p))
                    {
                        TextBox cell = _celdasCrucigrama[p];
                        char caracterEsperado = palabraEsperada.Texto.ToUpper()[i];
                        char caracterIngresado = cell.Text.Length > 0 ? cell.Text.ToUpper()[0] : '\0';

                        if (caracterIngresado == caracterEsperado)
                        {
                            cell.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            cell.BackColor = Color.LightCoral;
                            esCorrecta = false;
                        }
                    }
                    else
                    {
                        esCorrecta = false;
                    }
                }
            }
            else // Vertical
            {
                for (int i = 0; i < palabraEsperada.Longitud; i++)
                {
                    Point p = new Point(startX, startY + i);
                    if (_celdasCrucigrama.ContainsKey(p))
                    {
                        TextBox cell = _celdasCrucigrama[p];
                        char caracterEsperado = palabraEsperada.Texto.ToUpper()[i];
                        char caracterIngresado = cell.Text.Length > 0 ? cell.Text.ToUpper()[0] : '\0';

                        if (caracterIngresado == caracterEsperado)
                        {
                            cell.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            cell.BackColor = Color.LightCoral;
                            esCorrecta = false;
                        }
                    }
                    else
                    {
                        esCorrecta = false;
                    }
                }
            }
            return esCorrecta;
        }

        private void btnAgregarPalabra_Click(object sender, EventArgs e)
        {
            string nuevaPalabra = txtNuevaPalabra.Text.Trim().ToUpper();
            string nuevaPista = txtNuevaPista.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevaPalabra))
            {
                MessageBox.Show("Por favor ingresa una palabra válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(nuevaPista))
            {
                MessageBox.Show("Por favor ingresa una pista para la palabra.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nuevaPalabra.Length < 2)
            {
                MessageBox.Show("La palabra debe tener al menos 2 letras.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!nuevaPalabra.All(char.IsLetter))
            {
                MessageBox.Show("La palabra solo debe contener letras.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var palabraService = new PalabraService(new DatabaseService());
                palabraService.AgregarPalabraAsync(nuevaPalabra, nuevaPista).Wait();

                MessageBox.Show($"Palabra '{nuevaPalabra}' con pista agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNuevaPalabra.Clear();
                txtNuevaPista.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar palabra: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}