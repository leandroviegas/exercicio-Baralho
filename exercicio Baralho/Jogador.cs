namespace Exercicio.Baralho
{
    public class Jogador
    {
        public string? Nome { get; set; }
        public int Pontuacao { get; set; }
        public bool Congelado { get; set; }
        public List<Carta>? Cartas { get; set; }

    }
}
