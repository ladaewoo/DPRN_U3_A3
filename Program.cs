using System;

// Esta librería nos permitirá utilizar la clase List para crear colecciones
using System.Collections.Generic;

namespace DPRN2_U3_A2_JOMV
{
    // Creamos dos clases para las excepciones 
    // utilizando los principios de sobrecarga y polimorfismo
    // así podemos cambiar la firma funcionalidad de la excepción
    class EdadException : Exception
    {
        static string mensaje;
        public EdadException() : base("No tienes la edad mínima para entrar")
        {
        }

        public EdadException(string mensaje) : base(mensaje)
        {
        }

        public EdadException(string mensaje, int edad) : base(mensaje) 
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensaje);
            Console.WriteLine("Has declarado que tienes {0} años", edad);
            Console.Clear();
        }
    }

    class FechaException : Exception
    {
        static string mensaje;
        public FechaException() : base("La fecha ingresada es mayor a la actual")
        {
        }

        public FechaException(string mensaje) : base(mensaje)
        {
        }
    
    }

    // Construimos una clase padre que representa un punto de venta
    // así no solo serviría para vender boletos de cine sino cualquier cosa
    abstract class PuntoDeVenta
    {
        public Boolean ValidarFecha(string fecha)
        {
            // Intentamos parsear la fecha dentro de un bloque try-catch-finally, 
            // de esta manera si la fecha tiene un formato incorrecto lanzará una excepción
            try {
                DateTime.Parse(fecha);
                return true;
            } catch (Exception e) {
                Console.WriteLine("La captura no es una fecha");
                Console.WriteLine("presiona una tecla para continuar...");
                Console.ReadKey();
                return false;
            }
        }

        public int anyosTranscurridos(string fecha) 
        {
            DateTime fechaActual = DateTime.Now;
            DateTime fechaCapturada = DateTime.Parse(fecha);
            int anyosTranscurridos = fechaActual.Year - fechaCapturada.Year;

            // Si los años transcurridos son menores a cero
            // significa que se ingresó una fecha futura
            // por lo cual lanzamos una excepción
            if(anyosTranscurridos < 0)
            {
                throw new FechaException();
            }

            return anyosTranscurridos;
        }

        public abstract string ToString(int vector);
    }

    // Sellamos la clase para que ya no pueda ser derivada
    sealed class Pelicula : PuntoDeVenta
    {
        // bloqueamos los parametros con el modificador de acceso private
        private string pelicula;
        private string clasificacion;
        private string rangos;
        private int sala;

        // Con este método implementamos el concepto de encapsulamiento
        // ya que regresamos los valores de un dato miembro privado
        public string Nombre {
            get {
                return this.pelicula;
            }
        }

        public string Clasificacion {
            get {
                return this.clasificacion;
            }
        }

        public string Rangos {
            get {
                return this.rangos;
            }
        }

        public string Sala {
            get {
                return this.sala.ToString();
            }
        }

        private int edadMinima;

        // Tenemos que definir el método debido a que la herencia lo requiere
        // ya que en la super clase se definió como método abstracto
        public override string ToString(int numero)
        {
            return "--------------------------------------"
                + "\nPelicula: " + pelicula 
                + "\nBoleto: #" + (numero + 1)
                + "\nClasificación: " + clasificacion + " " + rangos
                + "\nSala: " + sala 
                + "\nPrecio: $40"
                + "\n--------------------------------------";
        }

        public Array ToArray()
        {
            return new string[] { pelicula, clasificacion, rangos, sala.ToString() };
        }

        public Pelicula(string pelicula, string clasificacion, string rangos, int sala, int edadMinima) : base()
        {
            this.pelicula = pelicula;
            this.clasificacion = clasificacion;
            this.rangos = rangos;
            this.sala = sala;
            this.edadMinima = edadMinima;
        }
        public Boolean validarAcceso(int edad)
        {
            // Comprobamos la edad si es menor al atributo edadMinima lanzamos excepción
            if (edad < edadMinima) throw new Exception("No tiene la edad mínima para ingresar");
            return true;
        }
    }

    // Creamos una clase para manejar la matríz de boletos
    class Carrito
    {
        // Encapsulamos la matriz de boletos
        private string[,] carritoCompras = new string[10, 4];
    
        // Creamos un método para agregar un boleto a la matriz
        // que recibe como parámetro un objeto de tipo pelicula
        public void Add(Pelicula pelicula)
        {
            // Recorremos el carrito de compras
            for (int i = 0; i < 10; i++)
            {
                // Si el carrito está vacío
                if (carritoCompras[i, 0] == null)
                {
                    // Agregamos la pelicula al carrito
                    carritoCompras[i, 0] = pelicula.Nombre;
                    carritoCompras[i, 1] = pelicula.Clasificacion;
                    carritoCompras[i, 2] = pelicula.Rangos;
                    carritoCompras[i, 3] = pelicula.Sala;
                    break;
                }
            }
        }

        // Creamos un método para mostrar los elementos del carrito de compras por índice
        public Pelicula Get(int index)
        {
            try {
                return new Pelicula(carritoCompras[index, 0], carritoCompras[index, 1], carritoCompras[index, 2], int.Parse(carritoCompras[index, 3]), 0);
            } 
            catch (Exception e) {
                return null;
            }
            
        }

        
        public int Count
        {
            get {
                int contador = 0;

                for (int i = 0; i < 10; i++)
                {
                    if (carritoCompras[i, 0] != null)
                    {
                        contador++;
                    }
                }

                return contador;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < 10; i++)
            {
                if (carritoCompras[i, 0] != null)
                {
                    carritoCompras[i, 0] = null;
                    carritoCompras[i, 1] = null;
                    carritoCompras[i, 2] = null;
                    carritoCompras[i, 3] = null;
                }
            }
        }

    }

    class Program
    {
        // Preparamos un arreglo con 4 casillas en donde guardaremos las instancias de la clase Pelicula
        //private static Pelicula[] peliculas = new Pelicula[4];
        private static int opcion = 0;
        // Definimos una colección de peliculas creando una instancia de la clase List

        static string[,] matriz = new string[4, 5];
        public static Carrito carrito = new Carrito();

        public static string[,] vendidos = new string[10, 3];

        private static void CargarPeliculas()
        {
            matriz[0, 0] = "Película 1";
            matriz[0, 1] = "A";
            matriz[0, 2] = "Para todo público      ";
            matriz[0, 3] = "1";
            matriz[0, 4] = "0";

            matriz[1, 0] = "Película 2";
            matriz[1, 1] = "B";
            matriz[1, 2] = "Para mayores de 15 años";
            matriz[1, 3] = "2";
            matriz[1, 4] = "15";

            matriz[2, 0] = "Película 3";
            matriz[2, 1] = "C";
            matriz[2, 2] = "Para mayores de 18 años";
            matriz[2, 3] = "3";
            matriz[2, 4] = "18";

            matriz[3, 0] = "Película 4";
            matriz[3, 1] = "D";
            matriz[3, 2] = "Para mayores de 21 años";
            matriz[3, 3] = "4";
            matriz[3, 4] = "21";
        }

        public static void generarBoleto()
        {
            // instanciamos la clase DateTime para obtener la fecha de hoy
            DateTime fecha = DateTime.Today;

            string boleto = "Cinema S.A. de C.V. " + fecha.ToString("dd/MM/yyyy") + "\n";
            int numero = 0;

            try {
                for (; numero < 10; numero++)
                {
                    Pelicula pelicula = carrito.Get(numero);

                    if(pelicula != null){
                        Console.WriteLine(pelicula.ToString(numero));
                        boleto += pelicula.ToString(numero) + "\n";
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            // Precio total por todos los boletos
            boleto += "Total: $" + (numero * 40) + "\n";

            Console.WriteLine(boleto);

            try {
                // Intentamos guardar nuestro boleto
                System.IO.StreamWriter file = new System.IO.StreamWriter("archivo.txt");
                file.WriteLine(boleto);
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.ReadKey();
            } finally {
                // Independientemente de si se pudo guardar o no el boleto 
                // limpiamos la pantalla y el carrito
                carrito.Clear();
                Console.Clear();
                Console.WriteLine("presiona una tecla para continuar...");
                Console.ReadKey();
            }
        }

        static void Main(string[] args)
        {
            
            // En el bloque principal preparamos la pantalla para interactuar con el usuario 
            // Cambiamos los colores de fondo y letra
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            // limpiamos toda la pantalla
            Console.Clear();          
            // Cargamos la cartelera
            CargarPeliculas();

            while(true)
            {
                Console.Clear();

                Console.WriteLine();
                Console.WriteLine("|-------------------------------------------------------|");
                Console.WriteLine(String.Format("|{0,10}|{1,5}|{2,23}|{3,6}|", "Película", "Clasificación", "Rango de edades", "Sala"));
                Console.WriteLine("|-------------------------------------------------------|");
                
                // Recorremos la matriz para mostrar la cartelera
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine(String.Format("|{0,10}|{1,13}|{2,23}|{3,6}|", matriz[i, 0], matriz[i, 1], matriz[i, 2], matriz[i, 3]));
                }

                Console.WriteLine("|-------------------------------------------------------|");
                Console.WriteLine();
                Console.WriteLine("Elige un número de película para agregarla al carrito");
                Console.WriteLine();


                if(carrito.Count > 0){
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Boletos agregados: {0} ", carrito.Count);
                    Console.WriteLine("Lista de personas [L] Comprar boletos [C] Vaciar carrito [V] Salir [S]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" - #");
                } else {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Salir [S]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" - #");
                } 

                try
                {
                    string entrada = Console.ReadLine();

                    // Revisamos si la tecla presionada corresponde a las opciones del programa
                    if (entrada == "C" || entrada == "c")
                    {
                        generarBoleto();
                    }
                    else if(entrada == "L" || entrada == "l")
                    {
                        Console.Clear();
                        Console.WriteLine("Lista de personas");
                        Console.WriteLine();
                        Console.WriteLine("|-----------------------------------------|");
                        Console.WriteLine(String.Format("|{0,10}|{1,7}|{2,10}|", "Persona", "Fecha de nacimiento", "Edad"));
                        Console.WriteLine("|-----------------------------------------|");
                        // Recorremos la matriz vendidos para mostrar los items
                        for (int i = 0; i < 10; i++)
                        {
                            if (vendidos[i, 0] != null)
                            {
                                Console.WriteLine(String.Format("|{0,10}|{1,19}|{2,10}|", vendidos[i, 0], vendidos[i, 1], vendidos[i, 2]));
                            }
                        }

                        Console.WriteLine("|-----------------------------------------|");
                        Console.WriteLine();
                        Console.WriteLine("presiona una tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if (entrada == "V" || entrada == "v")
                    {
                        carrito.Clear();
                    }
                    else if (entrada == "S" || entrada == "s")
                    {
                        // Reseteamos la consola 
                        Console.ResetColor();
                        Console.Clear();
                        // Rompemos el bucle
                        break;
                    }
                    else
                    {
                        // Si no fue una de las opciones que ofrece el programa
                        // solo atrapamos los números que pudieran ser el índice de alguna pelicula
                        // así evitamos errores si se introduce cualquier otro caracter
                        opcion = Convert.ToInt32(entrada);

                        if (opcion > 0 && opcion < 5)
                        {
                            DateTime hoy = DateTime.Today;
                            Console.WriteLine($"Ingresa la fecha de nacimiento, ej. {hoy.ToString("dd/MM/yyyy")}");
                            string fecha = Console.ReadLine();

                            Pelicula pelicula = new Pelicula(matriz[opcion - 1, 0], matriz[opcion - 1, 1], matriz[opcion - 1, 2], Convert.ToInt32(matriz[opcion - 1, 3]), Convert.ToInt32(matriz[opcion - 1, 4]));

                            if (pelicula.ValidarFecha(fecha))
                            {
                                // Aquí implementamos la excepción por defecto 
                                // y las dos excepciones que hemos construido
                                // ya que según el error mostrará diferentes mensajes en pantalla
                                try {
                                    int edad = pelicula.anyosTranscurridos(fecha);
                                    pelicula.validarAcceso(edad);
                                    // Agregamos la pelicula al carrito
                                    // utilizando los métodos que construimos
                                    carrito.Add(pelicula);
                                    // Agregamos un registro a la matriz de personas que ingresaron a la sala
                                    vendidos[carrito.Count, 0] = "Persona " + carrito.Count;
                                    vendidos[carrito.Count, 1] = fecha;
                                    vendidos[carrito.Count, 2] = edad.ToString();
                                } catch(FechaException e){
                                    Console.WriteLine("Error: " + e.Message);
                                    Console.WriteLine("presiona una tecla para continuar...");
                                    Console.ReadKey();
                                } catch (EdadException e) {
                                    carrito.Clear();
                                    Console.WriteLine("Error: " + e.Message);
                                    Console.WriteLine("Todos los boletos se han cancelado.");
                                    Console.WriteLine("presiona una tecla para continuar...");
                                    Console.ReadKey();
                                } catch (Exception e) {
                                    carrito.Clear();
                                    Console.WriteLine("Error: " + e.Message);
                                    Console.WriteLine("Todos los boletos se han cancelado.");
                                    Console.WriteLine("presiona una tecla para continuar...");
                                    Console.ReadKey();
                                }
                            }
                        }
                    }

                    opcion = Convert.ToInt32(entrada);
                } 
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }  
            }
        }
    }
}
