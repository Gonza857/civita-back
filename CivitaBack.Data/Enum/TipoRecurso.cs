using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.Enum;

[AttributeUsage(AttributeTargets.Field)]
public class DescripcionRecursoAttribute : Attribute
{
    public string Descripcion { get; }

    public DescripcionRecursoAttribute(string descripcion)
    {
        Descripcion = descripcion;
    }
}

public enum TipoRecurso
{
    [DescripcionRecurso("Energía")]
    Energia,

    [DescripcionRecurso("Felicidad")]
    Felicidad,

    [DescripcionRecurso("EcoCoins")]
    EcoCoins,

    [DescripcionRecurso("Contaminación")]
    Contaminacion


}

public static class TipoRecursoExtensions
{
    public static string GetDescription(this TipoRecurso value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = (DescripcionRecursoAttribute?)Attribute.GetCustomAttribute(field, typeof(DescripcionRecursoAttribute));
        return attr?.Descripcion ?? value.ToString();
    }
}


