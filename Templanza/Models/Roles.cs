namespace Templanza.Models
{
    // Nombres de rol como constantes, para usar en [Authorize].
    public static class Roles
    {
        public const string Administrador = "Administrador";
        public const string Operador = "Operador";
        public const string Cliente = "Cliente";
        public const string AdministradorOperador = Administrador + "," + Operador;
    }
}
