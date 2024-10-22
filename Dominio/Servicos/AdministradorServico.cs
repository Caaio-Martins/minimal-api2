using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;

using minimal_api.Dominio.DTOs;
using System.Data.Common;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Servicos
{
    public class AdministradorServico : IAdministradorServico
    {
        private readonly DbContexto _contexto;
        public AdministradorServico(DbContexto contexto)
        {
            _contexto = contexto;
        }

        public Administrador? BuscaPorId(int id)
        {
            return _contexto.Administradors.Where(v => v.Id == id).FirstOrDefault();
        }

        public Administrador Incluir(Administrador administrador)
        {
            _contexto.Administradors.Add(administrador);
            _contexto.SaveChanges();

            return administrador;
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Administradors.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;

        }

public List<Administrador> Todos(int? pagina)
{
    var query = _contexto.Administradors.AsQueryable();

    int itensPorPagina = 10;

    // Verificar se a página é nula ou menor que 1
    if (pagina.HasValue && pagina.Value > 0)
    {
        query = query
            .Skip((pagina.Value - 1) * itensPorPagina)
            .Take(itensPorPagina);
    }
    else
    {
        // Retornar os primeiros resultados caso a paginação não seja válida
        query = query.Take(itensPorPagina);
    }

    return query.ToList();
}
    }
}