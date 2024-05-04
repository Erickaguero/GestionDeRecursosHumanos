namespace PrototipoFuncionalRecursosHumanos.Models
{
    public class Departamento
    {
        public int? IdDepartamento { get; set; }
        public string? Nombre { get; set; }

        public Departamento()
        {
        }

        public Departamento(int idDepartamento, string nombre)
        {
            IdDepartamento = idDepartamento;
            Nombre = nombre;
        }
    }
}
