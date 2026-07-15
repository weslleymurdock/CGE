namespace CardGameEngine.Sample.Abstract.Adapters;

public interface ICarAdapter
{
    // Mapearemos o atributo de "Ataque" da carta real para Speed
    int Speed { get; } 
    
    // Mapearemos o atributo de "Vida/Defesa" da carta real para Handling
    int Handling { get; } 
}

