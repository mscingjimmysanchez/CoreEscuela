using System.Collections.Generic;
using CoreEscuela.Entidades;
using System;
using System.Linq;
using CoreEscuela.Util;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObjEsc)
        {
            if (dicObjEsc == null)
                throw new ArgumentNullException(nameof(dicObjEsc));
            _diccionario = dicObjEsc;
        }
        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
            if (_diccionario.TryGetValue(LlaveDiccionario.Evaluación, out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluación>();
            }
            else
            {
                return new List<Evaluación>();
            }
        }
        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy);
        }
        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluación> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();

            return (from Evaluación ev in listaEvaluaciones
                    select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<object>> GetDicEvaluaxAsig()
        {
            var dicRta = new Dictionary<string, IEnumerable<object>>();
            var listaAsig = GetListaAsignaturas(out var listaEval);
            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;
                dicRta.Add(asig, evalsAsig);
            }
            return dicRta;
        }

        public Dictionary<string, IEnumerable<object>> GetPromAlumxAsig()
        {
            var rta = new Dictionary<string, IEnumerable<object>>();
            var dicEvalxAsig = GetDicEvaluaxAsig();

            foreach (var asigConEval in dicEvalxAsig)
            {
                var promsAlum = from eval in asigConEval.Value
                            group eval by new {
                                                ((Evaluación) eval).Alumno.UniqueId,
                                                ((Evaluación) eval).Alumno.Nombre
                                                }
                            into grupoEvalsAlumno
                            select new AlumnoPromedio
                            { 
                                alumnoId = grupoEvalsAlumno.Key.UniqueId,       
                                alumnoNombre = grupoEvalsAlumno.Key.Nombre,                          
                                promedio = grupoEvalsAlumno.Average(evaluacion => ((Evaluación) evaluacion).Nota)
                            };
                rta.Add(asigConEval.Key, promsAlum);
            }

            return rta;
        }

        public IEnumerable<object> GetMejoresPromxAsig(string asignatura, int cantidad = 5)
        {
            var promAlumxAsig = GetPromAlumxAsig();
            var rta = promAlumxAsig.GetValueOrDefault(asignatura).OrderByDescending(prom => ((AlumnoPromedio) prom).promedio).Take(cantidad);

            return rta;
        }

        public void ImprimirEnumerable(IEnumerable<object> coleccion)
        {
            foreach (var obj in coleccion)
            {                    
                Console.WriteLine(obj);                        
            }
        }

        public void ImprimirDiccionario(Dictionary<string, IEnumerable<object>> dic, 
                                        bool imprEval = false)
        {
            foreach (var objDic in dic)
            {
                Printer.WriteTitle(objDic.Key.ToString());

                foreach (var val in objDic.Value)
                {
                    switch (objDic.Key)
                    {
                        case "Evaluación":
                            if (imprEval)
                                Console.WriteLine(val);
                        break;
                        case "Escuela":
                            Console.WriteLine("Escuela: " + val);
                        break;
                        case "Alumno":
                            Console.WriteLine("Alumno: " + ((ObjetoEscuelaBase) val).Nombre);
                        break;
                        case "Curso":
                            var curtmp = val as Curso;
                            if (curtmp != null)
                            {
                                int count = curtmp.Alumnos.Count;
                                Console.WriteLine("Curso: " + ((ObjetoEscuelaBase) val).Nombre + " Cantidad Alumnos: " + count);
                            }                            
                        break;
                        default:
                            Console.WriteLine(val);
                        break;
                    }
                }
            }
        }
        public void ImprimirDiccionario(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dic, 
                                        bool imprEval = false)
        {
            foreach (var objDic in dic)
            {
                Printer.WriteTitle(objDic.Key.ToString());

                foreach (var val in objDic.Value)
                {
                    switch (objDic.Key)
                    {
                        case LlaveDiccionario.Evaluación:
                            if (imprEval)
                                Console.WriteLine(val);
                        break;
                        case LlaveDiccionario.Escuela:
                            Console.WriteLine("Escuela: " + val);
                        break;
                        case LlaveDiccionario.Alumno:
                            Console.WriteLine("Alumno: " + val.Nombre);
                        break;
                        case LlaveDiccionario.Curso:
                            var curtmp = val as Curso;
                            if (curtmp != null)
                            {
                                int count = curtmp.Alumnos.Count;
                                Console.WriteLine("Curso: " + val.Nombre + " Cantidad Alumnos: " + count);
                            }                            
                        break;
                        default:
                            Console.WriteLine(val);
                        break;
                    }
                }
            }
        }
    }
}