namespace MiWebAPI.Models
{
    public class EquipoComputo
    {
        public long IdEquipo { get; set; }
        public string? NumeroSerie { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Procesador { get; set; }
        public int MemoriaRamGb { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}