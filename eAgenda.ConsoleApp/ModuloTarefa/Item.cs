using eAgenda.ConsoleApp.Compartilhado;
using System;
using System.Text;

namespace eAgenda.ConsoleApp.ModuloTarefa
{
    public class Item : EntidadeBase
    {
        public string Descricao { get; set; }
        public bool EstaConcluido{ get; set; }
        public string Status { get => EstaConcluido ? "Concluído" : "Pendente"; }

        public Item(string descricao)
        {
            Descricao = descricao;
        }

        public override string Validar()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(Descricao))
                sb.AppendLine("A descrição do item da tarefa é obrigatória!");

            if (sb.Length == 0)
                sb.Append("REGISTRO_VALIDO");

            return sb.ToString();
        }

        public void Concluir()
        {
            if (EstaConcluido)
                return;

            EstaConcluido = true;
        }

        public override string ToString()
        {
            return "\tDescrição: " + Descricao + Environment.NewLine +
                "\tStatus: " + Status + Environment.NewLine;
        }
    }
}
