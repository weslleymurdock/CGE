namespace CardGameEngine.Sample.Abstract.Adapters;


public interface IPlayerAdapter
{
    string Name { get; }
    
    // Retorna apenas as cartas que estão "na mesa" (na pista)
    IEnumerable<ICarAdapter> GetCarsOnTrack(); 
}