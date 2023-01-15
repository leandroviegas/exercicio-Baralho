namespace Exercicio.Baralho
{
    public static class Extensions
    {
        private static Random rng = new Random();

        public static void Embaralhar<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public class Baralho
    {
        public List<Carta> TodasCartas = new();

        public void MontarBaralho()
        {
            var naipes = new List<string> { "Copas", "Espadas", "Ouros", "Paus" };
            var nomesValores = new Dictionary<string, int>
            {
                { "A", 1 },
                { "2", 2 },
                { "3", 3 },
                { "4", 4 },
                { "5", 5 },
                { "6", 6 },
                { "7", 7 },
                { "8", 8 },
                { "9", 9 },
                { "10", 10 },
                { "J", 10 },
                { "K", 10 },
                { "Q", 10 }
            };

            foreach (var naipe in naipes)
            {
                foreach (var nomeValor in nomesValores)
                {
                    TodasCartas.Add(new Carta { Naipe = naipe, Nome = nomeValor.Key + " de " + naipe, Valor = nomeValor.Value });
                }
            }
        }
    }
}