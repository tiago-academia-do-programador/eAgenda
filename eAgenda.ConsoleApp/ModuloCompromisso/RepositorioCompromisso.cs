using eAgenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;

namespace eAgenda.ConsoleApp.ModuloCompromisso
{
    public class RepositorioCompromisso : RepositorioBase<Compromisso>, IRepositorio<Compromisso>
    {
        public List<Compromisso> SelecionarRegistrosPorPeriodo(DateTime dataInicio, DateTime dataTermino)
        {
            return Filtrar(compromisso => compromisso.DataCompromisso > dataInicio && compromisso.DataCompromisso < dataTermino);
        }
    }
}
