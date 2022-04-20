using eAgenda.ConsoleApp.Compartilhado;
using eAgenda.ConsoleApp.ModuloContato;
using System;
using System.Text;

namespace eAgenda.ConsoleApp.ModuloCompromisso
{
    public class Compromisso : EntidadeBase
    {
        public string Assunto { get; set; }
        public string Local { get; set; }

        private DateTime _dataCompromisso;
        public DateTime DataCompromisso { get => _dataCompromisso.Add(HoraInicio); set => _dataCompromisso = value ; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraTermino { get; set; }
        public Contato Contato { get; }

        public Compromisso(string assunto, string local, DateTime dataCompromisso, TimeSpan horaInicio, TimeSpan horaTermino, Contato contato)
        {
            Assunto = assunto;
            Local = local;
            HoraInicio = horaInicio;
            HoraTermino = horaTermino;
            DataCompromisso = dataCompromisso;
            Contato = contato;
        }

        public override string ToString()
        {
            string contatoRelacionado = Contato is null ? "Não disponível" : Contato.Nome;

            return "Id: " + id + Environment.NewLine +
                "Assunto: " + Assunto + Environment.NewLine +
                "Local: " + Local + Environment.NewLine +
                "Data do compromisso: " + DataCompromisso.ToShortDateString() + Environment.NewLine +
                "Hora de ínicio: " + HoraInicio + Environment.NewLine +
                "Hora de término: " + HoraTermino + Environment.NewLine +
                "Contato: " + contatoRelacionado + Environment.NewLine;
        }

        public override string Validar()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(Assunto))
                sb.AppendLine("O assunto do compromisso é obrigatório!");

            if (string.IsNullOrEmpty(Local))
                sb.AppendLine("O local do compromisso é obrigatório!");

            if (DataCompromisso == DateTime.MinValue)
                sb.AppendLine("A data do compromisso obrigatória!");

            if (DataCompromisso < DateTime.Today)
                sb.AppendLine("A data do compromisso não pode ser antes de hoje!");

            if (HoraInicio > HoraTermino)
                sb.AppendLine("A hora de início não pode ser depois do término!");

            if (sb.Length == 0)
                 sb.Append("REGISTRO_VALIDO");

            return sb.ToString();
        }
    }
}
