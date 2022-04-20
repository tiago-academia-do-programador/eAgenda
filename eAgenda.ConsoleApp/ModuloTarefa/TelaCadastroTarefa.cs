using eAgenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.ConsoleApp.ModuloTarefa
{
    public class TelaCadastroTarefa : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioTarefa _repositorioTarefa;
        private readonly INotificador _notificador;

        public TelaCadastroTarefa(RepositorioTarefa repositorioTarefa, INotificador notificador)
            : base("Cadastro de Contatos")
        {
            _repositorioTarefa = repositorioTarefa;
            _notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar Tarefas Por Prioridade");
            Console.WriteLine("Digite 5 para Visualizar Tarefas Pendentes");
            Console.WriteLine("Digite 6 para Visualizar Tarefas Concluídas");
            Console.WriteLine("Digite 7 para Adicionar Novos Itens");
            Console.WriteLine("Digite 8 para Concluir Itens da Tarefa");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Tarefa");

            Tarefa novaTarefa = ObterTarefa();

            string resultadoValidacao = _repositorioTarefa.Inserir(novaTarefa);

            if (resultadoValidacao != "REGISTRO_VALIDO")
                _notificador.ApresentarMensagem(resultadoValidacao, TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Tarefa cadastrada com sucesso!", TipoMensagem.Sucesso);
        }

        public void AdicionarNovosItens()
        {
            MostrarTitulo("Inserindo Novos Itens de Tarefa");

            bool temTarefasPendentes = VisualizarTarefasPendentes("Pesquisando");

            if (!temTarefasPendentes)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa pendente.", TipoMensagem.Atencao);
                return;
            }

            int numeroTarefa = ObterNumeroRegistro();

            Tarefa tarefaSelecionada = _repositorioTarefa.SelecionarRegistro(numeroTarefa);

            Console.WriteLine("Digite a descrição do item: ");
            Console.Write("> ");
            string descricao = Console.ReadLine();

            Item item = new Item(descricao);

            string resultadoValidacao = item.Validar();

            if (resultadoValidacao != "RESULTADO_VALIDACAO")
            {
                _notificador.ApresentarMensagem(resultadoValidacao, TipoMensagem.Erro);
                return;
            }

            tarefaSelecionada.AdicionarItem(item);

            _notificador.ApresentarMensagem("Item cadastrado com sucesso!", TipoMensagem.Sucesso);
        }

        public void ConcluirItens()
        {
            MostrarTitulo("Editando Tarefa");

            bool temTarefasPendentes = VisualizarTarefasPendentes("Pesquisando");

            if (temTarefasPendentes == false)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa pendente.", TipoMensagem.Atencao);
                return;
            }

            int numeroRegistro = ObterNumeroRegistro();

            Tarefa tarefaSelecionada = _repositorioTarefa.SelecionarRegistro(numeroRegistro);

            if (tarefaSelecionada.Itens.Count < 1)
            {
                _notificador.ApresentarMensagem("Nenhuma item para concluir.", TipoMensagem.Atencao);
                return;
            }

            foreach (Item item in tarefaSelecionada.Itens)
            {
                Console.WriteLine(item.ToString());

                Console.WriteLine("Deseja concluir o item da tarefa?");
                Console.WriteLine("1 - Sim, concluir item. ");
                Console.WriteLine("2 - Não.");
                Console.Write("> ");
                string opcaoSelecionada = Console.ReadLine();

                if (opcaoSelecionada == "1")
                {
                    item.Concluir();
                    _notificador.ApresentarMensagem("Item concluído com sucesso!", TipoMensagem.Sucesso);
                }
            }

            bool estaConcluida = _repositorioTarefa.VerificarConclusao(tarefaSelecionada);

            if (estaConcluida)
                _notificador.ApresentarMensagem("A Tarefa foi concluída!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Tarefa");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa disponível para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroRegistro = ObterNumeroRegistro();

            Tarefa tarefaSelecionada = _repositorioTarefa.SelecionarRegistro(numeroRegistro);

            Tarefa tarefaAtualizada = ObterTarefa();

            tarefaAtualizada.DataCriacao = tarefaSelecionada.DataCriacao;
            tarefaAtualizada.Itens = tarefaSelecionada.Itens;

            bool conseguiuEditar = _repositorioTarefa.Editar(numeroRegistro, tarefaAtualizada);

            if (!conseguiuEditar)
                _notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Tarefa editada com sucesso!", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Tarefa");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa disponível para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroRegistro = ObterNumeroRegistro();

            bool conseguiuExcluir = _repositorioTarefa.Excluir(numeroRegistro);

            if (!conseguiuExcluir)
                _notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Tarefa excluída com sucesso!", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Tarefas");

            List<Tarefa> tarefas = _repositorioTarefa.SelecionarTodos().OrderBy(x => x.Prioridade).ToList();

            if (tarefas.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Tarefa tarefa in tarefas)
                Console.WriteLine(tarefa.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarTarefasPendentes(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Tarefas Pendentes");

            List<Tarefa> tarefas = _repositorioTarefa.Filtrar(x => !x.EstaConcluida);

            if (tarefas.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa pendente.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Tarefa tarefa in tarefas)
                Console.WriteLine(tarefa.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarTarefasConcluidas(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Tarefas Concluídas");

            List<Tarefa> tarefas = _repositorioTarefa.Filtrar(x => x.EstaConcluida);

            if (tarefas.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa concluída.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Tarefa tarefa in tarefas)
                Console.WriteLine(tarefa.ToString());

            Console.ReadLine();

            return true;
        }

        private Tarefa ObterTarefa()
        {
            Console.Write("Digite o título da tarefa: ");
            string titulo = Console.ReadLine();

            Console.WriteLine("Escolha a prioridade da tarefa: ");

            Console.WriteLine("0 - Baixa");
            Console.WriteLine("1 - Normal");
            Console.WriteLine("2 - Alta");

            Console.Write("> ");
            int numPrioridade = Convert.ToInt32(Console.ReadLine());

            PrioridadeTarefa prioridade = (PrioridadeTarefa)numPrioridade;

            return new Tarefa(titulo, prioridade);
        }

        private int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID da tarefa que deseja selecionar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = _repositorioTarefa.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    _notificador.ApresentarMensagem("ID da tarefa não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }
    }
}
