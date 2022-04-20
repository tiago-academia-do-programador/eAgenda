using eAgenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.ConsoleApp.ModuloContato
{
    public class TelaCadastroContato : TelaBase, ITelaCadastravel
    {
        private readonly RepositorioContato _repositorioContato;
        private readonly INotificador _notificador;

        public TelaCadastroContato(RepositorioContato repositorioContato, INotificador notificador) : base("Cadastro de Contatos")
        {
            _repositorioContato = repositorioContato;
            _notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Visualizar Contatos Agrupados por Cargo");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Cadastro de Contato");

            Contato novoContato = ObterContato();

            string resultadoValidacao = _repositorioContato.Inserir(novoContato);

            if (resultadoValidacao != "REGISTRO_VALIDO")
                _notificador.ApresentarMensagem(resultadoValidacao, TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Contato cadastrado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Contato");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum contato disponível para editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroRegistro = ObterNumeroRegistro();

            Contato contatoAtualizado = ObterContato();

            bool conseguiuEditar = _repositorioContato.Editar(numeroRegistro, contatoAtualizado);

            if (!conseguiuEditar)
                _notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Contato editado com sucesso!", TipoMensagem.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Contato");

            bool temRegistrosCadastrados = VisualizarRegistros("Pesquisando");

            if (temRegistrosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhuma contato disponível para excluir.", TipoMensagem.Atencao);
                return;
            }

            int numeroRegistro = ObterNumeroRegistro();

            bool conseguiuExcluir = _repositorioContato.Excluir(numeroRegistro);

            if (!conseguiuExcluir)
                _notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagem.Erro);
            else
                _notificador.ApresentarMensagem("Contato excluído com sucesso!", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Contatos");

            List<Contato> contatos = _repositorioContato.SelecionarTodos();

            if (contatos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum contato disponível.", TipoMensagem.Atencao);
                return false;
            }

            foreach (Contato contato in contatos)
            {
                Console.WriteLine(contato.ToString());
                Console.WriteLine("\tCargo: " + contato.Cargo);
                Console.WriteLine();
            }

            Console.ReadLine();

            return true;
        }

        public bool VisualizarRegistrosAgrupadosPorCargo(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Contatos Agrupados por Cargo");

            List<Contato> contatos = _repositorioContato.SelecionarTodos();

            if (contatos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum contato disponível.", TipoMensagem.Atencao);
                return false;
            }

            var contatosAgrupados = _repositorioContato
                .SelecionarTodos()
                .GroupBy(x => x.Cargo);

            foreach (var contatoAgrupado in contatosAgrupados)
            {
                Console.WriteLine(contatoAgrupado.Key);

                foreach (Contato contatoDisponivel in contatos)
                    if (contatoDisponivel.Cargo == contatoAgrupado.Key)
                        Console.WriteLine(contatoDisponivel.ToString());
            }

            Console.ReadLine();

            return true;
        }

        private Contato ObterContato()
        {
            Console.WriteLine("Digite o nome do contato: ");
            Console.Write("> ");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite o email do contato: ");
            Console.Write("> ");
            string email = Console.ReadLine();

            Console.WriteLine("Digite o telefone do contato: ");
            Console.Write("> ");
            string telefone = Console.ReadLine();

            Console.WriteLine("Digite a empresa que o contato trabalha: ");
            Console.Write("> ");
            string empresa = Console.ReadLine();

            Console.WriteLine("Digite o cargo do contato: ");
            Console.Write("> ");
            string cargo = Console.ReadLine();

            return new Contato(nome, email, telefone, empresa, cargo);
        }

        private int ObterNumeroRegistro()
        {
            int numeroRegistro;
            bool numeroRegistroEncontrado;

            do
            {
                Console.Write("Digite o ID do contato que deseja selecionar: ");
                numeroRegistro = Convert.ToInt32(Console.ReadLine());

                numeroRegistroEncontrado = _repositorioContato.ExisteRegistro(numeroRegistro);

                if (numeroRegistroEncontrado == false)
                    _notificador.ApresentarMensagem("ID do contato não foi encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRegistroEncontrado == false);

            return numeroRegistro;
        }
    }
}
