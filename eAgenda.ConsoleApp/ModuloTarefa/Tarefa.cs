using eAgenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Text;

namespace eAgenda.ConsoleApp.ModuloTarefa
{
    public class Tarefa : EntidadeBase
    {
        public string Titulo { get; set; }
        public PrioridadeTarefa Prioridade{ get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataConclusao { get; set; }
        public bool EstaConcluida { get => ObterPercentualConclusao() == 100 ? true : false; }
        public List<Item> Itens { get; set; } = new List<Item>();

        public Tarefa(string titulo , PrioridadeTarefa prioridade)
        {
            Titulo = titulo;
            DataCriacao = DateTime.Now;
            Prioridade = prioridade;
        }

        public void AdicionarItem(Item item)
        {
            Itens.Add(item);
        }

        public override string Validar()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(Titulo))
                sb.AppendLine("O título da tarefa é obrigatório!");

            if (sb.Length == 0)
                sb.Append("REGISTRO_VALIDO");

            return sb.ToString();
        }

        public override string ToString()
        {
            string dataConclusao
                = DataConclusao != DateTime.MinValue ? DataConclusao.ToShortDateString() : "Não concluída";

            return "Id: " + id + Environment.NewLine +
                "Prioridade: " + Prioridade.ToString() + Environment.NewLine +
                "Título: " + Titulo + Environment.NewLine +
                "Data de criação: " + DataCriacao.ToShortDateString() + Environment.NewLine +
                "Percentual de conclusão: " + ObterPercentualConclusao() + "%" + Environment.NewLine +
                "Data de conclusão: " + dataConclusao + Environment.NewLine +
                "Itens: " + Environment.NewLine + ExibirItens() + Environment.NewLine;
        }

        private string ExibirItens()
        {
            if (Itens.Count < 1)
                return string.Empty;

            string itens = string.Empty;

            foreach (Item item in Itens)
                itens += item.ToString();

            return itens;
        }

        private int ObterPercentualConclusao()
        {
            double percentualConclusao = 0;

            if (Itens.Count < 1)
                return (int)percentualConclusao;

            double fracaoCompletude = 100 / Itens.Count;

            foreach (Item item in Itens)
            {
                if (item.EstaConcluido)
                    percentualConclusao += fracaoCompletude;
            }

            return (int)percentualConclusao;
        }
    }
}