using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;


namespace minimal_api.Dominio.Interfaces;

    public interface IAdministradorServico
{
    Administrador? Login(LoginDTO loginDTO);
}
