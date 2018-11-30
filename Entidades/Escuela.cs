using System.Collections.Generic;

namespace CoreEscuela.Entidades
{
    public class Escuela
    {
        string nombre;
        public string Nombre 
        { 
            get { return "Copia:" + nombre; }
            set { nombre = value.ToUpper(); } 
        }
        public int AñoDeCreación {get; set;}
        public string País { get; set; }
        public string Ciudad { get; set; }
        public TiposEscuela TipoEscuela { get; set; }
        public List<Curso> Cursos { get; set; }
        public Escuela(string nombre, int año) => (Nombre, AñoDeCreación) = (nombre, año);

        public Escuela(string nombre, int año, TiposEscuela tipos, 
                        string país = "", string ciudad = "")
        {
            (Nombre, AñoDeCreación) = (nombre, año);
            País = país;
            Ciudad = ciudad;
        }

        public override string ToString()
        {
            return $"Nombre: \"{Nombre}\", Tipo: {TipoEscuela} {System.Environment.NewLine} País: {País}, Ciudad: {Ciudad}";
        }
    }
}