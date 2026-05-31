namespace ProyectoHipodromoGrupoF.Modelo
{
    /// <summary>
    /// Modelo genérico para cualquier catálogo de la BD (Id + Nombre).
    /// Se usa para llenar dropdowns en los formularios.
    /// </summary>
    public class CatalogoItem
    {
        public int    Id     { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
