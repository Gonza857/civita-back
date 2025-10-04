using CivitaBack.Data.BO;

namespace CivitaBack.Logica;
public interface ITestRepositorio
{
    List<Test> ObtenerTests();
}


public class TestServicio
{

    private readonly ITestRepositorio _repo;

    public TestServicio(ITestRepositorio repo)
    {
        _repo = repo;
    }

    public List<Test> ObtenerTests()
    {
        return new();
    }
}
