using System.Collections.Generic;
using System;
using CoreEscuela.Util;

namespace CoreEscuela.Entidades
{
    public class Escuela : ObjetoEscuelaBase, ILugar
    {       
        public int AñoDeCreación {get; set;}
        public string País { get; set; }
        public string Ciudad { get; set; }
        public string Dirección { get; set; }
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

        public void LimpiarLugar()
        {
            Printer.DrawLine();
            Console.WriteLine("Limpiando Escuela...");
            
            foreach (var curso in Cursos)
            {
                curso.LimpiarLugar();
            }

            Printer.WriteTitle($"Escuela {Nombre} Limpia");
            Printer.Beep(1000, cantidad:3);
        }
    }
}