using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Executa o MenuPrincipal, responsável por chamar todos os outros procedimentos
            MenuPrincipal();
        }

        //Procedimento principal que mostra um menu interativo
        static void MenuPrincipal()
        {

            //Inicialização de algumas variáveis que serão usadas nos procedimentos, para que possam ser passadas como ref e ter seu valor alterado
            int opcao = 0;
            int [] qtdEstoque = new int[0];
            int[] qtdVendidaEstoque = new int[0];
            int [,] matrizVendas = new int [30, 4];
            bool arquivoImportado = false;

            do
            {
                Console.Clear();
                Console.WriteLine("1 - Importar arquivo de produtos");
                Console.WriteLine("2 - Registrar venda");
                Console.WriteLine("3 - Relatório de vendas no mês");
                Console.WriteLine("4 - Relatório de estoque");
                Console.WriteLine("5 - Criar arquivo de vendas");
                Console.WriteLine("6 - Sair");

                Console.Write("\nOpção: ");
                opcao = int.Parse(Console.ReadLine());
                Console.Write("\n");

                //Switch case que controla as chamadas dos procedimentos de acordo com a opção digitada pelo usuário
                switch (opcao)
                {
                    case 1:
                        ImportaArquivo(ref qtdEstoque, ref qtdVendidaEstoque, ref arquivoImportado);
                        break;
                    case 2:
                        RegistraVenda(ref qtdEstoque, ref matrizVendas, ref qtdVendidaEstoque, arquivoImportado);
                        break;
                    case 3:
                        RelatorioVendas(ref matrizVendas);
                        break;
                    case 4:
                        RelatorioEstoque(qtdEstoque, arquivoImportado);
                        break;
                    case 5:
                        CriaArquivoVendas(qtdVendidaEstoque);
                        break;
                    case 6:
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Escolha uma opção de 1 a 6");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                        break;
                }

            } while (opcao != 6);
        }

        //Procedimento que importa o arquivo de produtos
        static void ImportaArquivo(ref int [] qtdEstoque, ref int [] qtdVendidaEstoque, ref bool arquivoImportado)
        {
            try
            {
                string caminhoArquivo = @"C:\trabalho-atp-lab\ConsoleApp1\produtos_estoque.txt";

                //Faz a leitura do arquivo.txt e armazena no vetor de estoque dos produtos
                string[] produtos = File.ReadAllLines(caminhoArquivo);

                //Os vetores pegam o tamanho de linhas do arquivo, para que possam ser mais flexíveis
                qtdEstoque = new int[produtos.Length];
                qtdVendidaEstoque = new int[produtos.Length];

                //Exibe no console a quantidade do estoque
                Console.WriteLine("Quantidade em estoque:\n");
                foreach (string produto in produtos) Console.WriteLine(produto);

                //Percorre o vetor de produtos e separa a quantidade deles com o método Split e armazena essa quantidade no vetor de estoque
                for(int i = 0; i < produtos.Length; i++)
                {
                    string[] separa = produtos[i].Split('-');
                    int quantidade = int.Parse(separa[1].Trim());
                    qtdEstoque[i] = quantidade;
                }

                //Com isso a variável de arquivo importado recebe o valor de true para que possa controlar o registro de venda
                arquivoImportado = true;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nArquivo importado com sucesso!");
                Console.ResetColor();
            } 
            catch  (Exception ex)
            {
                //Tratamento de erro caso tenha algum problema com importar o arquivo
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao importar o arquivo: {ex.Message}");
                Console.WriteLine($"Tipo: {ex.GetType()}");
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("\nAperta alguma tecla para continuar...");
                Console.ReadKey();
            }
        }

        //Procedimento que realiza a venda dos produtos
        static void RegistraVenda(ref int[] qtdEstoque, ref int [,] matrizVendas, ref int[] qtdVendidaEstoque, bool arquivoImportado)
        { 

            //Se o arquivo está importado é possível registrar uma venda, se não exibe uma mensagem para importar o arquivo
            if (arquivoImportado)
            {
                char opcao = 's';
                //Do while para que o usuário possa registrar mais de uma venda sem precisar retornar ao menu principal a cada nova venda

                do
                {
                    //Pega informações da venda como dia, produto vendido, e quantidade vendida
                    Console.Clear();
                    Console.Write("Digite o dia da venda: ");
                    int dia = int.Parse(Console.ReadLine()) - 1;

                    Console.WriteLine("\nProduto A = 1 \nProduto B = 2 \nProduto C = 3 \nProduto D = 4");
                    Console.Write("\nInforme qual o número do produto vendido: ");
                    int produtoVendido = int.Parse(Console.ReadLine()) - 1;

                    //Se o usuário digitar um número que não corresponde a um produto existente, exibe uma mensagem de erro
                    if (produtoVendido >= 0 && produtoVendido < qtdEstoque.Length)
                    {
                        Console.Write("\nQuantas quantidades foram vendidas? ");
                        int qtdVendidas = int.Parse(Console.ReadLine());

                        //O usuário só consegue vender se a quantidade no estoque for suficiente
                        if (qtdEstoque[produtoVendido] >= qtdVendidas)
                        {
                            //Passa para a matriz de venda as "coordenadas" para que seja possível criar um relatório das vendas no mês, essa variável será usada no próximo procedimento
                            matrizVendas[dia, produtoVendido] += qtdVendidas;

                            //Subtrai a quantidade do estoque
                            qtdEstoque[produtoVendido] -= qtdVendidas;

                            //adiciona a quantidade ao vetor de quantidade produtos vendidos
                            qtdVendidaEstoque[produtoVendido] += qtdVendidas;

                            Console.WriteLine($"\nVenda de {qtdVendidas} unidades do produto {produtoVendido + 1} registrada com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("\nNão é possível vender uma quantidade maior do que o estoque");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nDigite o número válido correspondente a um produto");
                    }

                    Console.WriteLine("\nGostaria de registrar mais uma venda? digite \"s\" ou \"n\"?");
                    opcao = char.Parse(Console.ReadLine().ToLower());

                } while (opcao == 's');
            } else
            {
                Console.WriteLine("Você precisa importar o arquivo de estoque para registrar a venda");
            }

            Console.WriteLine("\nAperta alguma tecla para continuar...");
            Console.ReadKey();
        }

        //Procedimento que exibe uma relatório das vendas no período de um mês
        static void RelatorioVendas(ref int [,] matrizVendas)
        {
            Console.WriteLine("Relatório de vendas:\n");
            Console.WriteLine("          A  B  C  D\n");

            //Obtem as duas dimensões da matriz de vendas que é preenchida ao registrar uma venda no procedimento "RegistraVenda"
            int linhas = matrizVendas.GetLength(0);
            int colunas = matrizVendas.GetLength(1);

            //Percorre a matriz de vendas nas duas dimensões para obter o dia que representa a quantidade de linhas e a quantidade dos produtos vendidos que representa as colunas
            for (int i = 0; i < linhas; i++)
            {
                Console.Write($"Dia {i + 1:D2} - ");
                for (int j = 0; j < colunas; j++)
                {
                    Console.Write($" {matrizVendas[i, j]} ");
                }
                Console.WriteLine();  
            }

            Console.WriteLine("\nAperta alguma tecla para continuar...");
            Console.ReadKey();
        }

        //Procedimento que exibe um relatório atualizado do estoque de produtos
        static void RelatorioEstoque(int[] qtdEstoque, bool arquivoImportado)
        {
            //Se o arquivo estiver importado é possível exibir o relatório do estoque, se não, o usuário precisa importar o arquivo de estoque antes
            if(arquivoImportado)
            {
                Console.WriteLine("Relatório de estoque:\n");
                //Percorre o vetor de estoque e mostra e estoque equivalente de cada produto, através de alguns if para relacionar o nome do produto com a quantidade tornando mais fácil a identificação
                for(int i = 0;i < qtdEstoque.Length; i++)
                {
                    if (i == 0)
                    {
                        Console.WriteLine($"Produto A - {qtdEstoque[i]}");
                    }
                    if (i == 1)
                    {
                        Console.WriteLine($"Produto B - {qtdEstoque[i]}");
                    }
                    if (i == 2)
                    {
                        Console.WriteLine($"Produto C - {qtdEstoque[i]}");
                    }
                    if (i == 3)
                    {
                        Console.WriteLine($"Produto D - {qtdEstoque[i]}");
                    }
                }
            } else
            {
                Console.WriteLine("Você precisa importar o arquivo para visualizar o relatório de estoque");
            }
            Console.WriteLine("\nAperta alguma tecla para continuar...");
            Console.ReadKey();
        }

        //Procedimento que cria um arquivo.txt com o relatório da quantidade de produtos vendidos ao total
        static void CriaArquivoVendas(int [] qtdVendidaEstoque)
        {
            try
            {
                string nomeArquivo = "C:\\trabalho-atp-lab\\ConsoleApp1\\vendas-mes.txt";

                using (StreamWriter sw = new StreamWriter(nomeArquivo, false, Encoding.ASCII))
                {
                    string[] produtos = { "Produto A", "Produto B", "Produto C", "Produto D"};

                    //Aqui eu associei o vetor de quantidade vendida, com um vetor de produtos para melhor visualização no Console
                    for (int i = 0; i < qtdVendidaEstoque.Length; i++)
                    {
                        string linha = $"{produtos[i]} - {qtdVendidaEstoque[i]}";
                        sw.WriteLine(linha);
                        Console.WriteLine(linha);
                    }
        
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao criar o arquivo de vendas: {ex.Message}");
                Console.WriteLine($"Tipo: {ex.GetType()}");
                Console.ResetColor();
            }

            finally
            {
                Console.WriteLine("\nAperta alguma tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}