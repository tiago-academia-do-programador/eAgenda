using eAgenda.ConsoleApp.Compartilhado;
using System;

namespace eAgenda.ConsoleApp.ModuloTarefa
{
    public class RepositorioTarefa : RepositorioBase<Tarefa>, IRepositorio<Tarefa>
    {
        public bool VerificarConclusao(Tarefa tarefa)
        {
            if (tarefa.EstaConcluida)
            {
                tarefa.DataConclusao = DateTime.Now;
                return true;
            }

            return false;
        }
    }
}
