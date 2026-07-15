namespace CardGameEngine.Sample.Abstract.Services;

public interface IRaceSimulatorService
{
    RaceResult SimulateRace(IGameAdapter gameAdapter, CircuitTrack track);
}