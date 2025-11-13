namespace CivitaBack.Logica.Interfaces
{
    public interface IAccesoUsuarios // La dejo en Logica/Interfaces porque depende de HttpContextAccessor
    {
        int ObtenerIdUsuarioActual();

        bool EsDios();
    }
}
