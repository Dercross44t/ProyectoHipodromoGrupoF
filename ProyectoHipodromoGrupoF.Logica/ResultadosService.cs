using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class ResultadosService
    {
        private readonly ResultadosRepository _resultadosRepo;

        public ResultadosService(ResultadosRepository resultadosRepo)
        {
            _resultadosRepo = resultadosRepo;
        }

        public List<ResultadoCarrera> ListarResultados()
        {
            return _resultadosRepo.ListarResultados();
        }

        public void InsertarResultado(ResultadoCarrera resultado)
        {
            if (string.IsNullOrWhiteSpace(resultado.CodigoInscripcion))
                throw new System.Exception("El código de inscripción es obligatorio.");
            
            if (resultado.Posicion < 1)
                throw new System.Exception("La posición debe ser un número positivo mayor o igual a 1.");

            _resultadosRepo.InsertarResultado(resultado);
        }

        public void ActualizarResultado(ResultadoCarrera resultado)
        {
            if (string.IsNullOrWhiteSpace(resultado.Codigo))
                throw new System.Exception("El código del resultado es obligatorio.");

            _resultadosRepo.ActualizarResultado(resultado);
        }

        public void EliminarResultado(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new System.Exception("El código del resultado es obligatorio.");

            _resultadosRepo.EliminarResultado(codigo);
        }
    }
}
