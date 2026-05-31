namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Persona
    {
        public string Cedula { get; set; } // 9 dígitos exactos
        public string Nombre1 { get; set; } // máximo 50 caracteres
        public string Nombre2 { get; set; } // máximo 50 caracteres
        public string Apellido1 { get; set; } // máximo 50 caracteres
        public string Apellido2 { get; set; } // máximo 50 caracteres
        public int IdCatBarrio { get; set; } // FK hacia catálogo de barrio
    }
}
