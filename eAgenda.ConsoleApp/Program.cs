using eAgenda.ConsoleApp.Compartilhado;
using eAgenda.ConsoleApp.ModuloCompromisso;
using eAgenda.ConsoleApp.ModuloContato;
using eAgenda.ConsoleApp.ModuloTarefa;
using System;

namespace eAgenda.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TelaMenuPrincipal telaMenuPrincipal = new TelaMenuPrincipal(new Notificador());

            while (true)
            {
                TelaBase telaSelecionada = telaMenuPrincipal.ObterTela();

                if (telaSelecionada is null)
                    break;

                string opcaoSelecionada = telaSelecionada.MostrarOpcoes();

                if (telaSelecionada is TelaCadastroTarefa)
                    GerenciarTarefas(telaSelecionada, opcaoSelecionada);

                else if (telaSelecionada is TelaCadastroCompromisso)
                    GerenciarCompromissos(telaSelecionada, opcaoSelecionada);

                if (telaSelecionada is ITelaCadastravel)
                    GerenciarCadastrosBasicos(telaSelecionada, opcaoSelecionada);
            }
        }

        private static void GerenciarCadastrosBasicos(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            ITelaCadastravel telaCadastroBasico = (ITelaCadastravel)telaSelecionada;

            if (opcaoSelecionada == "1")
                telaCadastroBasico.Inserir();

            else if (opcaoSelecionada == "2")
                telaCadastroBasico.Editar();

            else if (opcaoSelecionada == "3")
                telaCadastroBasico.Excluir();

            else if (opcaoSelecionada == "4")
                telaCadastroBasico.VisualizarRegistros("Tela");

            else if (telaSelecionada is TelaCadastroContato)
            {
                TelaCadastroContato telaCadastroContato = telaSelecionada as TelaCadastroContato;

                if (opcaoSelecionada == "5")
                    telaCadastroContato.VisualizarRegistrosAgrupadosPorCargo("Tela");
            }
        }

        private static void GerenciarCompromissos(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            TelaCadastroCompromisso telaCadastroCompromisso = (TelaCadastroCompromisso)telaSelecionada;

            if (opcaoSelecionada == "4")
                telaCadastroCompromisso.VisualizarCompromissosPorPeriodo("Tela");

            else if (opcaoSelecionada == "5")
                telaCadastroCompromisso.VisualizarCompromissosPassados("Tela");

            else if (opcaoSelecionada == "6")
                telaCadastroCompromisso.VisualizarCompromissosFuturos("Tela");
        }

        private static void GerenciarTarefas(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            TelaCadastroTarefa telaCadastroTarefa = (TelaCadastroTarefa)telaSelecionada;

            if (opcaoSelecionada == "5")
                telaCadastroTarefa.VisualizarTarefasPendentes("Tela");

            else if (opcaoSelecionada == "6")
                telaCadastroTarefa.VisualizarTarefasConcluidas("Tela");

            else if (opcaoSelecionada == "7")
                telaCadastroTarefa.AdicionarNovosItens();

            else if (opcaoSelecionada == "8")
                telaCadastroTarefa.ConcluirItens();
        }
    }
}
