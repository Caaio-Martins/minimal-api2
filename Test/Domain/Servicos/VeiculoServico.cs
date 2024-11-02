using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;

namespace Test.Domain.Servicos;

[TestClass]
public class VeiculoServicoTest
{
    private DbContexto CriarContextoDeTeste()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<DbContexto>();
        optionsBuilder.UseMySql(configuration.GetConnectionString("MySql"), ServerVersion.AutoDetect(configuration.GetConnectionString("MySql")));

        return new DbContexto(optionsBuilder.Options, configuration);
    }

    [TestMethod]
    public void TestandoIncluirVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo { Nome = "Carro Teste", Marca = "Marca Teste" };
        var veiculoServico = new VeiculoServico(context);

        // Act
        veiculoServico.Incluir(veiculo);

        // Assert
        Assert.AreEqual(1, veiculoServico.Todos().Count);
    }

    [TestMethod]
    public void TestandoBuscaPorId()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo { Nome = "Carro Teste", Marca = "Marca Teste" };
        var veiculoServico = new VeiculoServico(context);
        veiculoServico.Incluir(veiculo);

        // Act
        var veiculoDoBanco = veiculoServico.BuscaPorId(veiculo.Id);

        // Assert
        Assert.AreEqual(veiculo.Nome, veiculoDoBanco?.Nome);
    }

    [TestMethod]
    public void TestandoAtualizarVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo { Nome = "Carro Teste", Marca = "Marca Teste" };
        var veiculoServico = new VeiculoServico(context);
        veiculoServico.Incluir(veiculo);

        veiculo.Nome = "Carro Teste Atualizado";

        // Act
        veiculoServico.Atualizar(veiculo);
        var veiculoAtualizado = veiculoServico.BuscaPorId(veiculo.Id);

        // Assert
        Assert.AreEqual("Carro Teste Atualizado", veiculoAtualizado?.Nome);
    }

    [TestMethod]
    public void TestandoApagarVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo { Nome = "Carro Teste", Marca = "Marca Teste" };
        var veiculoServico = new VeiculoServico(context);
        veiculoServico.Incluir(veiculo);

        // Act
        veiculoServico.Apagar(veiculo);

        // Assert
        Assert.AreEqual(0, veiculoServico.Todos().Count);
    }
}
