using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using System.Collections.Generic;

namespace Test.Mocks
{
    public class VeiculoServicoMock : IVeiculoServico
    {
        private static List<Veiculo> veiculos = new List<Veiculo>()
        {
            new Veiculo { Id = 1, Nome = "Palio", Marca = "Fiat", Ano = 2010 },
            new Veiculo { Id = 2, Nome = "Civic", Marca = "Honda", Ano = 2019 }
        };

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            return veiculos;
        }

        public Veiculo? BuscaPorId(int id)
        {
            return veiculos.Find(v => v.Id == id);
        }

        public void Incluir(Veiculo veiculo)
        {
            veiculo.Id = veiculos.Count + 1; // Atribui um novo Id
            veiculos.Add(veiculo); // Adiciona o veículo à lista
        }

        public void Atualizar(Veiculo veiculo)
        {
            var index = veiculos.FindIndex(v => v.Id == veiculo.Id);
            if (index != -1)
            {
                veiculos[index] = veiculo; // Atualiza o veículo na lista
            }
        }

        public void Apagar(Veiculo veiculo)
        {
            veiculos.Remove(veiculo); // Remove o veículo da lista
        }
    }
}
