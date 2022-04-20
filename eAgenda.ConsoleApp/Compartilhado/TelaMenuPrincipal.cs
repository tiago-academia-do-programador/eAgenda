using eAgenda.ConsoleApp.ModuloCompromisso;
using eAgenda.ConsoleApp.ModuloContato;
using eAgenda.ConsoleApp.ModuloTarefa;
using System;

namespace eAgenda.ConsoleApp.Compartilhado
{
    public class TelaMenuPrincipal
    {
        private readonly INotificador _notificador;

        private readonly RepositorioContato _repositorioContato;
        private readonly TelaCadastroContato _telaCadastroContato;

        private readonly RepositorioTarefa _repositorioTarefa;
        private readonly TelaCadastroTarefa _telaCadastroTarefa;

        private readonly RepositorioCompromisso _repositorioCompromisso;
        private readonly TelaCadastroCompromisso _telaCadastroCompromisso;


        public TelaMenuPrincipal(INotificador notificador)
        {
            _notificador = notificador;
            _repositorioContato = new RepositorioContato();
            _telaCadastroContato = new TelaCadastroContato(_repositorioContato, _notificador);

            _repositorioTarefa = new RepositorioTarefa();
            _telaCadastroTarefa = new TelaCadastroTarefa(_repositorioTarefa, _notificador);

            _repositorioCompromisso = new RepositorioCompromisso();
            _telaCadastroCompromisso = new TelaCadastroCompromisso(_repositorioCompromisso, _repositorioContato, _telaCadastroContato, notificador);

            PopularAplicacao();
        }

        public string MostrarOpcoes()
        {
            Console.Clear();

            Console.WriteLine("e-Agenda v1.0");

            Console.WriteLine();

            Console.WriteLine("Digite 1 para Gerenciar Contatos");
            Console.WriteLine("Digite 2 para Gerenciar Tarefas");
            Console.WriteLine("Digite 3 para Gerenciar Compromissos");

            Console.WriteLine("Digite s para sair");

            string opcaoSelecionada = Console.ReadLine();

            return opcaoSelecionada;
        }

        public TelaBase ObterTela()
        {
            string opcao = MostrarOpcoes();

            TelaBase tela = null;

            if (opcao == "1")
                tela = _telaCadastroContato;

            else if (opcao == "2")
                tela = _telaCadastroTarefa;

            else if (opcao == "3")
                tela = _telaCadastroCompromisso;

            return tela;
        }

        private void PopularAplicacao()
        {
            var contato = new Contato("Juninho", "testador@academiadoprogramador.net", "981120982", "Academia do Programador", "Testador");
            _repositorioContato.Inserir(contato);

            var tarefaPrioridadeBaixa = new Tarefa("Corrigir Provas", PrioridadeTarefa.Baixa);
            var item = new Item("Baixar repositórios");
            tarefaPrioridadeBaixa.AdicionarItem(item);

            var tarefaPrioridadeAlta = new Tarefa("Testar Aplicação", PrioridadeTarefa.Alta);

            _repositorioTarefa.Inserir(tarefaPrioridadeAlta);
            _repositorioTarefa.Inserir(tarefaPrioridadeBaixa);

            var compromisso = new Compromisso("Reunião Diária", "meet", DateTime.Today, TimeSpan.Parse("15:30"), TimeSpan.Parse("17:00"), contato);
            _repositorioCompromisso.Inserir(compromisso);
        }
    }
}
