namespace CivitaBack.Logica.Interfaces
{
    public interface IAccesoUsuarios // La dejo en Logica/Interfaces porque depende de HttpContextAccessor y lo exige el proceso de Autenticacion 
    {
        int ObtenerIdUsuarioActual();
        bool EsDios();
        void ValidarAcceso(int idUsuarioPartida);
    }
}
