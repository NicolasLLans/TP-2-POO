namespace TP_2_DSOO
{
    using System;
    using System.Collections.Generic;

    // Interfaz ItemFacturable
    public interface ItemFacturable
    {
        decimal CalcularPrecioFinal();
        void MostrarDetalles();
    }

    // Clase Suplemento que implementa ItemFacturable
    public class Suplemento : ItemFacturable
    {
        public string Nombre { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public decimal PrecioLista { get; set; }

        public Suplemento(string nombre, decimal porcentajeGanancia, decimal precioLista)
        {
            Nombre = nombre;
            PorcentajeGanancia = porcentajeGanancia;
            PrecioLista = precioLista;
        }

        public decimal CalcularPrecioFinal()
        {
            decimal ganancia = PrecioLista * (PorcentajeGanancia / 100);
            decimal precioConGanancia = PrecioLista + ganancia;
            decimal precioFinal = precioConGanancia * 1.21m; // Agregar IVA del 21%
            return precioFinal;
        }

        public void MostrarDetalles()
        {
            Console.WriteLine($"Suplemento: {Nombre}, Precio Final: ${CalcularPrecioFinal():F2}");
        }
    }

    // Clase abstracta ServicioDeportivo que implementa ItemFacturable
    public abstract class ServicioDeportivo : ItemFacturable
    {
        public string Tipo { get; set; }

        public ServicioDeportivo(string tipo)
        {
            Tipo = tipo;
        }

        public abstract decimal CalcularPrecio();

        public decimal CalcularPrecioFinal()
        {
            decimal precioBase = CalcularPrecio();
            decimal precioFinal = precioBase * 1.105m; // Agregar la mitad del IVA (10.5%)
            return precioFinal;
        }

        public abstract void MostrarDetalles();
    }

    // Clase EntrenamientoPersonalizado que hereda de ServicioDeportivo
    public class EntrenamientoPersonalizado : ServicioDeportivo
    {
        public int DuracionHoras { get; set; }

        public EntrenamientoPersonalizado(string tipo, int duracionHoras) : base(tipo)
        {
            DuracionHoras = duracionHoras;
        }

        public override decimal CalcularPrecio()
        {
            return DuracionHoras * 2000m;
        }

        public override void MostrarDetalles()
        {
            Console.WriteLine($"Entrenamiento Personalizado: {Tipo}, Duración: {DuracionHoras} horas, Precio Final: ${CalcularPrecioFinal():F2}");
        }
    }

    // Clase ClaseGrupal que hereda de ServicioDeportivo
    public class ClaseGrupal : ServicioDeportivo
    {
        public int NumeroParticipantes { get; set; }
        public int DuracionMinutos { get; set; }

        public ClaseGrupal(string tipo, int numeroParticipantes, int duracionMinutos) : base(tipo)
        {
            NumeroParticipantes = numeroParticipantes;
            DuracionMinutos = duracionMinutos;
        }

        public override decimal CalcularPrecio()
        {
            decimal precioBase = DuracionMinutos * 80m;
            if (NumeroParticipantes > 10)
            {
                precioBase *= 0.8m; // Descuento del 20%
            }
            return precioBase;
        }

        public override void MostrarDetalles()
        {
            Console.WriteLine($"Clase Grupal: {Tipo}, Participantes: {NumeroParticipantes}, Duración: {DuracionMinutos} minutos, Precio Final: ${CalcularPrecioFinal():F2}");
        }
    }

    // Clase Factura
    public class Factura
    {
        public ItemFacturable Item { get; set; }
        public decimal Precio { get; set; }

        public Factura(ItemFacturable item)
        {
            Item = item;
            Precio = item.CalcularPrecioFinal();
        }
    }

    // Clase HistorialFacturacion
    public class HistorialFacturacion
    {
        public List<Factura> Facturas { get; private set; }

        public HistorialFacturacion()
        {
            Facturas = new List<Factura>();
        }

        public void AgregarFactura(Factura factura)
        {
            Facturas.Add(factura);
        }

        public void MostrarHistorial()
        {
            foreach (var factura in Facturas)
            {
                factura.Item.MostrarDetalles();
            }
        }

        public decimal MontoTotalFacturado()
        {
            decimal total = 0;
            foreach (var factura in Facturas)
            {
                total += factura.Precio;
            }
            return total;
        }

        public int CantServiciosSimples()
        {
            int count = 0;
            foreach (var factura in Facturas)
            {
                if (factura.Item is ClaseGrupal claseGrupal && claseGrupal.NumeroParticipantes < 10)
                {
                    count++;
                }
            }
            return count;
        }
    }

    // Clase Test con el menú
    internal class Test
    {
        static void Main(string[] args)
        {
            HistorialFacturacion historial = new HistorialFacturacion();
            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("Menú:");
                Console.WriteLine("1. Agregar un nuevo servicio");
                Console.WriteLine("2. Mostrar detalles de los servicios");
                Console.WriteLine("3. Salir del programa");
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarNuevoServicio(historial);
                        break;
                    case "2":
                        historial.MostrarHistorial();
                        break;
                    case "3":
                        salir = true;
                        Console.WriteLine($"Monto Total Facturado: ${historial.MontoTotalFacturado():F2}");
                        Console.WriteLine($"Cantidad de Servicios Simples: {historial.CantServiciosSimples()}");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
            }
        }

        static void AgregarNuevoServicio(HistorialFacturacion historial)
        {
            Console.WriteLine("Tipos de Servicios:");
            Console.WriteLine("1. Suplemento");
            Console.WriteLine("2. Entrenamiento Personalizado");
            Console.WriteLine("3. Clase Grupal");
            Console.Write("Seleccione el tipo de servicio: ");
            string tipoServicio = Console.ReadLine();

            switch (tipoServicio)
            {
                case "1":
                    Console.Write("Ingrese el nombre del suplemento: ");
                    string nombre = Console.ReadLine();
                    Console.Write("Ingrese el porcentaje de ganancia (sólo escriba el número): ");
                    decimal porcentajeGanancia = Convert.ToDecimal(Console.ReadLine());
                    Console.Write("Ingrese el precio de lista (sólo escriba el número): ");
                    decimal precioLista = Convert.ToDecimal(Console.ReadLine());
                    Suplemento suplemento = new Suplemento(nombre, porcentajeGanancia, precioLista);
                    historial.AgregarFactura(new Factura(suplemento));
                    break;
                case "2":
                    Console.Write("Ingrese el tipo de entrenamiento: ");
                    string tipoEntrenamiento = Console.ReadLine();
                    Console.Write("Ingrese la duración en horas: ");
                    int duracionHoras = Convert.ToInt32(Console.ReadLine());
                    EntrenamientoPersonalizado entrenamiento = new EntrenamientoPersonalizado(tipoEntrenamiento, duracionHoras);
                    historial.AgregarFactura(new Factura(entrenamiento));
                    break;
                case "3":
                    Console.Write("Ingrese el tipo de clase: ");
                    string tipoClase = Console.ReadLine();
                    Console.Write("Ingrese el número de participantes: ");
                    int numeroParticipantes = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Ingrese la duración en minutos: ");
                    int duracionMinutos = Convert.ToInt32(Console.ReadLine());
                    ClaseGrupal claseGrupal = new ClaseGrupal(tipoClase, numeroParticipantes, duracionMinutos);
                    historial.AgregarFactura(new Factura(claseGrupal));
                    break;
                default:
                    Console.WriteLine("Tipo de servicio no válido. Intente de nuevo.");
                    break;
            }
        }
    }
}
