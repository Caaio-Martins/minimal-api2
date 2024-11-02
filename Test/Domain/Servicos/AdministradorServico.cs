using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;
using minimal_api.Dominio.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Domain.Entidades
{
    [TestClass]
    public class AdministradorServicoTest
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

            // Criar as opções do DbContexto
            var optionsBuilder = new DbContextOptionsBuilder<DbContexto>();
            optionsBuilder.UseMySql(configuration.GetConnectionString("MySql"), ServerVersion.AutoDetect(configuration.GetConnectionString("MySql")));

            // Retornar uma nova instância do DbContexto usando as opções e a configuração
            return new DbContexto(optionsBuilder.Options, configuration);
        }

        [TestMethod]
        public void TestandoSalvarAdministrador()
        {
            // Arrange
            using var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradors");

            var adm = new Administrador
            {
                Email = "teste@teste.com",
                Senha = "teste",
                Perfil = "Adm"
            };

            var administradorServico = new AdministradorServico(context);

            // Act
            administradorServico.Incluir(adm);

            // Assert
            Assert.AreEqual(1, administradorServico.Todos(1).Count());
        }

        [TestMethod]
        public void TestandoBuscaPorId()
        {
            // Arrange
            using var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradors");

            var adm = new Administrador
            {
                Email = "teste@teste.com",
                Senha = "teste",
                Perfil = "Adm"
            };

            var administradorServico = new AdministradorServico(context);

            // Act
            administradorServico.Incluir(adm);
            var admDoBanco = administradorServico.BuscaPorId(adm.Id);

            // Assert
            Assert.IsNotNull(admDoBanco);
            Assert.AreEqual(adm.Email, admDoBanco?.Email); // Verifica se o email é o mesmo
        }

        [TestMethod]
        public void TestandoLogin()
        {
            // Arrange
            using var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradors");

            var adm = new Administrador
            {
                Email = "teste@teste.com",
                Senha = "teste",
                Perfil = "Adm"
            };

            var administradorServico = new AdministradorServico(context);
            administradorServico.Incluir(adm);

            var loginDTO = new LoginDTO
            {
                Email = adm.Email,
                Senha = adm.Senha
            };

            // Act
            var resultadoLogin = administradorServico.Login(loginDTO);

            // Assert
            Assert.IsNotNull(resultadoLogin);
            Assert.AreEqual(adm.Email, resultadoLogin?.Email); // Verifica se o email do resultado é o mesmo
        }

        [TestMethod]
        public void TestandoListaAdministradores()
        {
            // Arrange
            using var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradors");

            var administradorServico = new AdministradorServico(context);

            // Adicionar múltiplos administradores
            for (int i = 0; i < 25; i++)
            {
                var adm = new Administrador
                {
                    Email = $"teste{i}@teste.com",
                    Senha = "teste",
                    Perfil = "Adm"
                };
                administradorServico.Incluir(adm);
            }

            // Act
            var todosAdministradores = administradorServico.Todos(1);

            // Assert
            Assert.AreEqual(10, todosAdministradores.Count()); // Verifica se a paginação está funcionando
        }
    }
}
