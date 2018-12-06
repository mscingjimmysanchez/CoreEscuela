using System;
using System.Collections.Generic;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;
using System.Linq;
using CoreEscuela.App;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;
            var engine = new EscuelaEngine();
            engine.Inicializar();
            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            Printer.WriteTitle("BIENVENIDO A LA ESCUELA");
            Printer.WriteTitle("Reportes:");
            WriteLine("1. Lista de Evaluaciones.");
            WriteLine("2. Lista de Asignaturas.");
            WriteLine("3. Lista de Evaluaciones por Asignatura.");
            WriteLine("4. Lista de Promedios de Alumnos por Asignatura.");
            WriteLine("5. Lista de Mejores Promedios por Asignatura.");
            WriteLine("Ingrese la opción correspondiente al reporte que desea ver:");
            Printer.PresioneENTER();
            string opcion = Console.ReadLine();

            try
            {
                if (string.IsNullOrWhiteSpace(opcion))
                {
                    Printer.WriteTitle("El valor de la opción no puede ser vacío");                    
                }
                else
                {
                    if (int.Parse(opcion) < 1 || int.Parse(opcion) > 5)
                    {
                        throw new ArgumentOutOfRangeException("El valor de la opción debe estar entre 1 y 5");
                    }
                    else
                    {
                        switch (int.Parse(opcion))
                        {
                            case 1:
                                var evalList = reporteador.GetListaEvaluaciones();
                                Printer.WriteTitle("LISTA DE EVALUACIONES");
                                reporteador.ImprimirEnumerable(evalList);
                                break;
                            case 2:
                                var listaAsig = reporteador.GetListaAsignaturas();
                                Printer.WriteTitle("LISTA DE ASIGNATURAS");
                                reporteador.ImprimirEnumerable(listaAsig);
                                break;
                            case 3:
                                var listaEvalxAsig = reporteador.GetDicEvaluaxAsig();
                                Printer.WriteTitle("LISTA DE EVALUACIONES POR ASIGNATURA");
                                reporteador.ImprimirDiccionario(listaEvalxAsig);
                                break;
                            case 4:
                                var listaPromxAlum = reporteador.GetPromAlumxAsig();
                                Printer.WriteTitle("LISTA DE PROMEDIOS DE ALUMNOS POR ASIGNATURA");
                                reporteador.ImprimirDiccionario(listaPromxAlum);
                                break;
                            case 5:
                                WriteLine("Ingrese la asignatura para el reporte:");
                                Printer.PresioneENTER();
                                string asignatura = Console.ReadLine();
                                WriteLine("Ingrese la cantidad de promedios para el reporte:");
                                Printer.PresioneENTER();
                                string cantidad = Console.ReadLine();
                                var listaMejoresPromxAlum = reporteador.GetMejoresPromxAsig(asignatura, int.Parse(cantidad));
                                Printer.WriteTitle($"LISTA DE LOS MEJORES {cantidad} PROMEDIOS PARA LA ASIGNATURA {asignatura.ToUpper()}");
                                reporteador.ImprimirEnumerable(listaMejoresPromxAlum);
                                break;
                        }

                        return;
                    }
                }
            }
            catch (ArgumentOutOfRangeException arg)
            {
                Printer.WriteTitle(arg.Message);
            }
            catch (Exception)
            {
                Printer.WriteTitle("El valor de la opción no es válido");
            }
            finally
            {
                Printer.WriteTitle("FINALIZANDO");
            }
        }
        private static void AccionDelEvento(object sender, EventArgs e)
        {
            Printer.WriteTitle("SALIENDO");

            for (int i = 0; i < 2; i++)
            {
                Printer.Beep(262, 500, 2);
                Printer.Beep(294, 500, 1);
                Printer.Beep(262, 500, 1);
                Printer.Beep(349, 500, 1);
                Printer.Beep(330, 500, 1);
            }
            
            Printer.WriteTitle("SALIÓ");
        }
        private static void ImprimirCursosEscuela(Escuela escuela)
        {
            Printer.WriteTitle("Cursos de la Escuela");
            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre}, Id {curso.UniqueId}");
                }
            }
        }
    }
}
