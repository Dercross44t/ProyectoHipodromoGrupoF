namespace ProyectoHipodromoGrupoF.Modelo
{
    public class CatProvincia
    {
        public int    Id     { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class CatCanton
    {
        public int    Id          { get; set; }
        public int    IdProvincia { get; set; }
        public string Nombre      { get; set; } = string.Empty;
    }

    public class CatDistrito
    {
        public int    Id       { get; set; }
        public int    IdCanton { get; set; }
        public string Nombre   { get; set; } = string.Empty;
    }

    public class CatBarrio
    {
        public int    Id         { get; set; }
        public int    IdDistrito { get; set; }
        public string Nombre     { get; set; } = string.Empty;
    }
}
