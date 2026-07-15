namespace CardGameEngine.Sample.Concrete.Adapters;

public class PlayerAdapter(Player realPlayer) : IPlayerAdapter
{
    public string Name => "Player 1"; // Extraia do objeto real

    public IEnumerable<ICarAdapter> GetCarsOnTrack()
    {
        // Aqui você acessa a propriedade real da lib que contém as cartas na mesa.
        // Exemplo: return realPlayer.Board.Cards.Select(c => new CarAdapter(c));
        
        return Enumerable.Empty<ICarAdapter>(); 
    }
}
