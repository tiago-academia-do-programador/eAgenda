using eAgenda.ConsoleApp.Compartilhado;

namespace eAgenda.ConsoleApp.Compartilhado
{
    public interface INotificador
    {
        public void ApresentarMensagem(string mensagem, TipoMensagem tipoMensagem);
    }
}
