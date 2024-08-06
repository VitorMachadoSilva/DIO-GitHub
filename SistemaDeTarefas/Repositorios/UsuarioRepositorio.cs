using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaDeTarefas.Repositorios
{
    public class UsuarioNaoEncontradoException : Exception
    {
        public UsuarioNaoEncontradoException(int id)
            : base($"Usuário com o ID {id} não foi encontrado.")
        {
        }
    }
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        
        private readonly SistemaTarefasDBContex _dbContext;
        public UsuarioRepositorio(SistemaTarefasDBContex sistemaTarefasDBContex)
        {
            _dbContext = sistemaTarefasDBContex;
        }

        //public async Task<UsuarioModel> BuscarPorId(int id)
        //{
        //    var usuario = await _dbContext.Usuarios
        //                                  .FirstOrDefaultAsync(x => x.Id == id);

        //    //IMPLEMENTAR UM TRY CATCH PARA MELHORAR A SEGURANÇA
        //    if (usuario == null)
        //    {
        //        throw new UsuarioNaoEncontradoException(id);
        //    }

        //    return usuario;
        //}
        public async Task<UsuarioModel> BuscarPorId(int id)
        {
            try
            {
                // Tenta buscar o usuário no banco de dados
                var usuario = await _dbContext.Usuarios
                                              .FirstOrDefaultAsync(x => x.Id == id);

                // Verifica se o usuário foi encontrado
                if (usuario == null)
                {
                    // Lança uma exceção personalizada se o usuário não for encontrado
                    throw new UsuarioNaoEncontradoException(id);
                }

                // Retorna o usuário encontrado
                return usuario;
            }
            catch (UsuarioNaoEncontradoException)
            {
                // Aqui você pode lidar com a exceção de usuário não encontrado
                // Se necessário, você pode fazer algo específico ou apenas relançar a exceção
                throw;
            }
            
        }



        public async Task<List<UsuarioModel>> BuscarTodosUsuarios()
        {
            return await _dbContext.Usuarios.ToListAsync();

        }

        public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
        {
            await _dbContext.Usuarios.AddAsync(usuario);
            await _dbContext.SaveChangesAsync();

            return usuario;
        }

        public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
        {
            
            UsuarioModel usuarioPorId = await BuscarPorId(id);

            
            usuarioPorId.Nome = usuario.Nome;
            usuarioPorId.Email = usuario.Email;

            _dbContext.Usuarios.Update(usuarioPorId);
            await _dbContext.SaveChangesAsync();
            return usuarioPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            UsuarioModel usuarioPorId = await BuscarPorId(id);      
            _dbContext.Usuarios.Remove(usuarioPorId);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        
    }
}
