namespace CardGameEngine.Sample.Abstract.Adapters;


public interface IEngineAdapter
{
    // Abstrai como o CardGameEngine gerencia os jogadores internamente
    IEnumerable<IPlayerAdapter> GetActivePlayers(); 
}