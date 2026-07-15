namespace CardGameEngine.Sample.Concrete.Services;

public class RaceSimulatorService : IRaceSimulatorService
{
    public RaceResult SimulateRace(IGameAdapter gameAdapter, CircuitTrack track)
    {
        var results = gameAdapter.GetActivePlayers().Select(player => 
        {
            double totalScore = 0;

            foreach (var car in player.GetCarsOnTrack())
            {
                // Fórmula da corrida
                double carScore = (car.Speed * track.SpeedWeight) + (car.Handling * track.HandlingWeight);
                totalScore += carScore;
            }

            return new RaceResult(player.Name, totalScore);
        }).ToList();

        return results.OrderByDescending(r => r.Score).First();
    }
}