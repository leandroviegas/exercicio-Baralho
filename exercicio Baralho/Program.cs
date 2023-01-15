using Exercicio.Baralho;

public class Jogo
{
    public static Baralho baralho = new Baralho();
    public static List<Jogador> jogadores = new List<Jogador>();
    public static Table table = new Table();

    static void Regras()
    {
        List<WriteText> displayText = new List<WriteText>();

        displayText.Add(new WriteText
        {
            text = "Bem vindo às regras de jogo\n\n" +
            "1. Quem conseguir 21 somando os valores das cartas ganha um ponto\n" +
            "2. Se uma pessoa tirou uma carta as outras pessoas tem que tirar a mesma quantidade de cartas mesmo alguém tendo completado 21\n" +
            "3. Caso duas pessoas ou mais tenham conseguido 21 o ponto vai para os que fizeram 21\n" +
            "4. ultrapassar o valor de 21 resulta em desqualificação da rodada\n" +
            "5. Empates de pontos quando ninguém conseguiu completar 21 não resultará em ponto\n\n",
            color = ConsoleColor.White
        });

        string[] opts = {
            "1. Voltar para a tela inicial",
            "2. Ver cartas e seus valores"
        };

        Checkbox checkbox = new Checkbox(displayText, opts);
        int opcao = checkbox.Select()[0].Index;
        if (opcao != 0)
        {
            DetalhesCartas();
        }
    }
    static void DetalhesCartas()
    {
        List<WriteText> displayText = new List<WriteText>();
        displayText.Add(new WriteText
        {
            text = table.PrintLine() + "\n" + table.PrintRow("nome", "valor") + "\n" + table.PrintLine(),
            color = ConsoleColor.White
        });

        foreach (Carta carta in baralho.TodasCartas)
        {

            displayText.Add(new WriteText
            {
                text = "\n" + table.PrintRow(carta.Nome, carta.Valor.ToString()) + "\n"+ table.PrintLine(),
                color = ConsoleColor.White
            });
        }

        displayText.Add(new WriteText { text = "\n\n", color = ConsoleColor.White });

        string[] opts = {
            "1. Voltar para a tela inicial",
            "2. Ir para regras"
        };

        Checkbox checkbox = new Checkbox(displayText, opts);
        int opcao = checkbox.Select()[0].Index;
        if (opcao != 0)
        {
            Regras();
        }
    }

    static void Main()
    {
        int rodada = 0;

        baralho.MontarBaralho();

        Console.WriteLine();

        List<WriteText> displayTextInicial = new List<WriteText>();


        foreach (var item in "Bem vindo ao 21".Select((value, i) => new { i, value }))
        {
            displayTextInicial.Add(new WriteText
            {
                text = item.value.ToString(),
                color = item.i % 2 == 0 ? ConsoleColor.Cyan : ConsoleColor.Magenta
            });
        }
        displayTextInicial.Add(new WriteText
        {
            text = "\n\n",
            color = ConsoleColor.White
        });

        string[] optsInicial = {
            "1. Começar jogo",
            "2. Ver regras de jogo",
            "3. Ver cartas e seus valores"
        };

        while(rodada == 0) 
        {
            Checkbox checkbox = new Checkbox(displayTextInicial, optsInicial);
            int opcao = checkbox.Select()[0].Index;
            switch (opcao)
            {
                case 0:
                    rodada++;
                    break;
                case 1:
                    Regras();
                    break;
                case 2:
                    DetalhesCartas();
                    break;
            }
        }

        Console.Write("\nDigite quantos jogadores irão participar: ");
        int quantidadeJogadores;

        while (!int.TryParse(Console.ReadLine(), out quantidadeJogadores) || quantidadeJogadores < 2 || quantidadeJogadores > 12)
        {
            Console.Clear();
            Console.Write("Você colocou um valor inválido, tente outro valor: ");
        }

        for (int n = 0; n < quantidadeJogadores; n++)
        {
            Console.Clear();
            Console.Write("Digite o nome do " + (n + 1) + "º jogador: ");
            string? nome = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nome))
            {
                nome = "Jogador " + (n + 1);
            }
            jogadores.Add(new Jogador { Nome = nome, Pontuacao = 0, Congelado = false, Cartas = new List<Carta>() });
        }


        baralho.TodasCartas.Embaralhar();


        for (int cartaAtual = 0; cartaAtual < baralho.TodasCartas.Count;)
        {
            List<Jogador> jogadoresFiltrados = jogadores.Where(jogador => jogador.Congelado == false).ToList();

            foreach (Jogador jogador in jogadoresFiltrados)
            {
                if (!jogadores.Any(j => j.Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor) == 21) && (jogadores.Where(j => j.Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor) > 21).Count() != jogadores.Count - 1))
                {
                    Console.WriteLine();
                    jogadores.OrderBy(j => j.Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor));

                    string[] opts = {
            "1. Pegar uma carta",
            "2. parar por aqui"
        };
                    List<WriteText> displayText = new List<WriteText>();

                    displayText.Add(new WriteText
                    {
                        text = "Rodada de número " + rodada + "\n\n",
                        color = ConsoleColor.White
                    });

                    displayText.Add(new WriteText
                    {
                        text = "Tem atualmente " + jogadores.Count + " jogadores.\n",
                        color = ConsoleColor.White
                    });
                    displayText.Add(new WriteText
                    {
                        text = table.PrintLine() + "\n" + table.PrintRow("nome", "pontuação", "Soma das cartas") + "\n" + table.PrintLine(),
                        color = ConsoleColor.White
                    });

                    foreach (Jogador j in jogadores)
                    {

                        displayText.Add(new WriteText
                        {
                            text = "\n" + table.PrintRow(j.Nome, j.Pontuacao.ToString(), j.Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor).ToString()) + "\n",
                            color = j == jogador ? ConsoleColor.Blue : ConsoleColor.White
                        });
                        displayText.Add(new WriteText
                        {
                            text = table.PrintLine(),
                            color = ConsoleColor.White
                        });
                    }

                    displayText.Add(new WriteText
                    {
                        text = "\n\n",
                        color = ConsoleColor.White
                    });

                    var opcao = 0;

                    if (jogador.Cartas?.Count >= 2)
                    {
                        displayText.Add(new WriteText
                        {
                            text = "É a vez do jogador ",
                            color = ConsoleColor.White
                        });

                        displayText.Add(new WriteText
                        {
                            text = jogador.Nome + "\n\n",
                            color = ConsoleColor.Blue
                        });

                        displayText.Add(new WriteText
                        {
                            text = "Deseja pegar uma carta ou parar por aqui?\n",
                            color = ConsoleColor.White
                        });
                        Checkbox checkbox = new Checkbox(displayText, opts);
                        opcao = checkbox.Select()[0].Index;
                    }
                    else
                    {
                        Console.Clear();
                        foreach (var writeText in displayText)
                        {
                            Console.ForegroundColor = writeText.color;
                            Console.Write(writeText.text);
                            Console.ResetColor();
                        }
                    }

                    if (opcao == 0)
                    {
                        jogador.Cartas?.Add(baralho.TodasCartas[cartaAtual]);
                        var cartasSoma = jogador.Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor);

                        Console.Write("Carta pega por ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(jogador.Nome);
                        Console.ResetColor();
                        Console.Write(": ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(baralho.TodasCartas[cartaAtual].Nome + "\n");
                        Console.ResetColor();

                        if (cartasSoma == 21)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("Parabéns " + jogador.Nome + ". Você ganhou essa rodada.");
                            Console.ResetColor();
                            jogador.Congelado = true;
                        }
                        else if (cartasSoma > 21)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(jogador.Nome + " perdeu :(. Mais sorte na próxima rodada.");
                            Console.ResetColor();
                            jogador.Congelado = true;
                        }

                        cartaAtual++;
                    }
                    else
                    {
                        Console.WriteLine("O jogador " + jogador.Nome + " parou por aqui");
                        jogador.Congelado = true;
                    }

                    Console.ReadLine();
                }
                else
                {
                    foreach (Jogador j in jogadores)
                    {
                        j.Congelado = true;
                    }
                }
            }

            List<Jogador> ganhadores = jogadores.Where(jogador => jogador.Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor) == 21).ToList();

            // alguém fez 21
            if (ganhadores.Count() > 0)
            {
                foreach (Jogador ganhador in ganhadores)
                {
                    ganhador.Pontuacao += 1;
                }
            }
            // se todos foram congelados mas não tiveram vitoriosos
            else if (jogadores.Where(jogador => jogador.Congelado).Count() == jogadores.Count)
            {
                foreach (Jogador jogador in jogadores)
                {
                    if (jogador.Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor) > 21)
                    {
                        jogador.Cartas = new List<Carta>();
                    }
                }
                List<Jogador> proximosDo21 = jogadores.OrderBy(jogador => jogador.Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor)).ToList();

                if (proximosDo21[0].Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor) != proximosDo21[1].Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor))
                {
                    if (proximosDo21[0].Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor) > proximosDo21[1].Cartas?.Aggregate(0, (acc, carta) => acc + carta.Valor))
                    {
                        proximosDo21[0].Pontuacao += 1;
                        Console.WriteLine(proximosDo21[0].Nome + " mais se aproximou de 21, e por isso ganhou essa rodada.");
                    }
                    else
                    {
                        proximosDo21[1].Pontuacao += 1;
                        Console.WriteLine(proximosDo21[1].Nome + " mais se aproximou de 21, e por isso ganhou essa rodada.");
                    }
                }
                else
                {
                    Console.WriteLine("Não houve ganhador nessa rodada.");
                }
            }

            // Reiniciar rodada
            if ((ganhadores.Count() > 0) || (jogadores.Where(jogador => jogador.Congelado).Count() == jogadores.Count))
            {
                cartaAtual = 0;
                baralho.TodasCartas.Embaralhar();
                rodada++;
                foreach (Jogador jogador in jogadores)
                {
                    jogador.Congelado = false;
                    jogador.Cartas = new List<Carta>();
                }
                Console.ReadLine();
            }

        }
    }
}