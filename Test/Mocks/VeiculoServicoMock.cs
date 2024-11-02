using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Test.Mocks
{
    public class VeiculoServicoMock : IVeiculoServico
    {
        private static List<Veiculo> veiculos = new List<Veiculo>()
        {
            new Veiculo { Id = 1, Nome = "Fusca", Marca = "Volkswagen", Ano = 1970 },
            new Veiculo { Id = 2, Nome = "Civic", Marca = "Honda", Ano = 2020 }
        };

        public Veiculo? BuscaPorId(int id)
        {
            return veiculos.Find(v => v.Id == id);
        }

        public Veiculo Incluir(Veiculo veiculo)
        {
            if (veiculo == null)
                throw new ArgumentNullException(nameof(veiculo));

            veiculo.Id = veiculos.Max(v => v.Id) + 1; // Garante que o ID seja único
            veiculos.Add(veiculo);
            return veiculo;
        }

        public void Atualizar(Veiculo veiculo)
        {
            var existingVeiculo = BuscaPorId(veiculo.Id);
            if (existingVeiculo != null)
            {
                existingVeiculo.Nome = veiculo.Nome;
                existingVeiculo.Marca = veiculo.Marca;
                existingVeiculo.Ano = veiculo.Ano;
            }
        }

        public void Apagar(Veiculo veiculo)
        {
            veiculos.Remove(veiculo);
        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            var query = veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => v.Nome.ToLower().Contains(nome.ToLower()));
            }
            if (!string.IsNullOrEmpty(marca))
            {
                query = query.Where(v => v.Marca.ToLower().Contains(marca.ToLower()));
            }

            int itensPorPagina = 10;
            if (pagina != null)
                query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

            return query.ToList();
        }
    }
}
