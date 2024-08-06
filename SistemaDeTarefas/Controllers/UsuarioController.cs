using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios;
using SistemaDeTarefas.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaDeTarefas.Controllers
{
    //rota padrão, puxando o nome da controller = "usuario"
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        //método de buscar todos usuarios (endpoint)
        //[HttpGet]
        //public async Task<ActionResult<List<UsuarioModel>>> BuscarTodosUsuarios()
        //{
        //    List<UsuarioModel> usuarios = await _usuarioRepositorio.BuscarTodosUsuarios();
        //    return Ok();//200
        //}

        [HttpGet]
        public async Task<ActionResult<List<UsuarioModel>>> BuscarTodosUsuarios()
        {
            List<UsuarioModel> usuarios = await _usuarioRepositorio.BuscarTodosUsuarios();
            if (usuarios == null || !usuarios.Any())
            {
                return NotFound(); // 404 se nenhum usuário for encontrado
            }

            return Ok(usuarios); // 200 se usuários forem encontrados
        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<List<UsuarioModel>>> BuscarPorId(int id)
        //{
        //    List<UsuarioModel> usuario = await _usuarioRepositorio.BuscarPorId(id);
        //    return Ok(usuario);//200
        //    //erro
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioModel>> BuscarPorId(int id)
        {
            var usuario = await _usuarioRepositorio.BuscarPorId(id);
            if (usuario == null)
            {
                return NotFound(); // 404
            }
            return Ok(usuario); // 200
        }


        [HttpPost]
        public async Task<ActionResult<UsuarioModel>> Cadastrar([FromBody] UsuarioModel usuarioModel)
        {
            UsuarioModel usuario = await _usuarioRepositorio.Adicionar(usuarioModel);

            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioModel>> Atualizar([FromBody] UsuarioModel usuarioModel, int id)
        {
            usuarioModel.Id = id;
            UsuarioModel usuario = await _usuarioRepositorio.Atualizar(usuarioModel, id);

            return Ok(usuario);
        }

        [HttpDelete("{id})")]
        public async Task<ActionResult<UsuarioModel>> Apagar(int id)
        {
            bool apagado = await _usuarioRepositorio.Apagar(id);
            return Ok(apagado);
        }

    }
}
