using eAgenda.ConsoleApp.Compartilhado;
using eAgenda.ConsoleApp.ModuloContato;
using System;
using System.Collections.Generic;

namespace eAgenda.ConsoleApp.ModuloCompromisso
{
    public class TelaCadastroCompromisso : TelaBase, ITelaCadastravel
    {
        private readonly TelaCadastroContato _telaCadastroContato;
        private readonly RepositorioCompromisso _repositorioCompromisso;
        private readonly RepositorioContato _repositorioContato;
        private readonly INotificador _notificador;

        public TelaCadastroCompromisso(RepositorioCompromisso repositorioCompromisso,
            RepositorioContato repositorioContato,
            TelaCadastroContato telaCadastroContato,
            INotificador notificador) : base("Cadastro de Compromissos")
        {
            _repositorioContato = repositorioContato;
            _notificador = notificador;
            _repositorioCompromisso = repositorioCompromisso;
            _telaCadastroContato = telaCadastroContato;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar Compromissos por Período");
            Console.WriteLine("Digite 5 para Visualizar Compromissos Passados");
            Console.WriteLine("Digite 6 para Visualizar Compromissos Futuros");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Compromisso");

            Contato novoContato = ObterContato();

            Compromisso novoCompromisso = ObterCompromisso(novoContato);

            string resultadoValidacao = _repositorioCompromisso.Inserir(novoCompromisso);

            if (resultadoValidacao != "REGISTRO_VALIDO")
                _notificador.ApresentarMensagem(resultadoValidacao, TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Compromisso cadastrado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Compromisso");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum compromisso disponível para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroRegistro = ObterNumeroRegistro();

            Contato contatoRelacionado = ObterContato();

            Compromisso compromissoAtualizado = ObterCompromisso(contatoRelacionado);

            bool conseguiuEditar = _repositorioCompromisso.Editar(numeroRegistro, compromissoAtualizado);

            if (!conseguiuEditar)
                _notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Compromisso editado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Compromisso");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum compromisso disponível para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroRegistro = ObterNumeroRegistro();

            bool conseguiuExcluir = _repositorioCompromisso.Excluir(numeroRegistro);

            if (!conseguiuExcluir)
                _notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Compromisso excluído com sucesso!", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Compromissos");

            List<Compromisso> compromissos = _repositorioCompromisso.SelecionarTodos();

            if (compromissos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum compromisso disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Compromisso compromisso in compromissos)
                Console.WriteLine(compromisso.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarCompromissosPassados(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Compromissos Passados");

            List<Compromisso> compromissos = _repositorioCompromisso
                    .Filtrar(compromisso => compromisso.DataCompromisso < DateTime.Today);

            if (compromissos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum compromisso disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Compromisso compromisso in compromissos)
                Console.WriteLine(compromisso.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarCompromissosFuturos(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Compromissos Futuros");

            List<Compromisso> compromissos = _repositorioCompromisso
                    .Filtrar(compromisso => compromisso.DataCompromisso >= DateTime.Today);

            if (compromissos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum compromisso disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Compromisso compromisso in compromissos)
                Console.WriteLine(compromisso.ToString());

            Console.ReadLine();

            return true;
        }

        public bool VisualizarCompromissosPorPeriodo(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Compromissos Por Período");

            Console.Write("Digite a data de início do período: ");
            DateTime dataInicio = DateTime.Parse(Console.ReadLine());

            Console.Write("Digite a data de término do período: ");
            DateTime dataTermino = DateTime.Parse(Console.ReadLine());

            if (dataInicio > dataTermino)
            {
                _notificador.ApresentarMensagem("A data de início não pode ser maior que a data de término do período", TipoMensagem.Erro);
                return VisualizarCompromissosPorPeriodo("Tela");
            }

            List<Compromisso> compromissos = _repositorioCompromisso
                                            .SelecionarRegistrosPorPeriodo(dataInicio, dataTermino);

            if (compromissos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum compromisso disponível durante o período.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Compromisso compromisso in compromissos)
                Console.WriteLine(compromisso.ToString());

            Console.ReadLine();

            return true;
        }

        private int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID do compromisso que deseja selecionar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = _repositorioCompromisso.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    _notificador.ApresentarMensagem("ID do compromisso não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }

        private Contato ObterContato()
        {
            bool temContatosDisponiveis = _telaCadastroContato.VisualizarRegistros("Pesquisando");

            if (!temContatosDisponiveis)
            {
                _notificador.ApresentarMensagem("Não há nenhum contato disponível.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o Id do contato relacionado ao compromisso (opcional): ");
            int numeroContato = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Contato contatoSelecionado = _repositorioContato.SelecionarRegistro(numeroContato);

            return contatoSelecionado;
        }

        private Compromisso ObterCompromisso(Contato contato)
        {
            Console.WriteLine("Digite o assunto do compromisso: ");
            Console.Write("> ");
            string assunto = Console.ReadLine();

            Console.WriteLine("Digite o local do compromisso: ");
            Console.Write("> ");
            string local = Console.ReadLine();

            Console.WriteLine("Digite o data do compromisso: ");
            Console.Write("> ");
            DateTime dataCompromisso = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Digite o a hora de início do compromisso: ");
            Console.Write("> ");
            TimeSpan horaInicio = TimeSpan.Parse(Console.ReadLine());

            Console.WriteLine("Digite o a hora de término do compromisso: ");
            Console.Write("> ");
            TimeSpan horaTermino = TimeSpan.Parse(Console.ReadLine());

            return new Compromisso(assunto, local, dataCompromisso, horaInicio, horaTermino, contato);
        }
    }
}
