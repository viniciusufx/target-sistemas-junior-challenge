using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

const string Json = """
{
  "estoque": [
    {
      "codigoProduto": 101,
      "descricaoProduto": "Caneta Azul",
      "estoque": 150
    },
    {
      "codigoProduto": 102,
      "descricaoProduto": "Caderno Universitário",
      "estoque": 75
    },
    {
      "codigoProduto": 103,
      "descricaoProduto": "Borracha Branca",
      "estoque": 200
    },
    {
      "codigoProduto": 104,
      "descricaoProduto": "Lápis Preto HB",
      "estoque": 320
    },
    {
      "codigoProduto": 105,
      "descricaoProduto": "Marcador de Texto Amarelo",
      "estoque": 90
    }
  ]
}
""";

var dados = JsonSerializer.Deserialize<DadosEstoque>(Json)
    ?? throw new InvalidOperationException("Não foi possível carregar o estoque.");

var gerenciadorEstoque = new GerenciadorEstoque(dados.Estoque);

while (true)
{
    ExibirProdutos(dados.Estoque);

    Console.Write("Digite o código do produto (0 para sair): ");
    var codigoProduto = LerInteiro();

    if (codigoProduto == 0)
    {
        break;
    }

    Console.Write("Digite o tipo da movimentação (E = Entrada / S = Saída): ");
    var tipoMovimentacao = Console.ReadLine()?.Trim().ToUpper();

    Console.Write("Digite a quantidade: ");
    var quantidade = LerInteiro();

    Console.Write("Digite a descrição da movimentação: ");
    var descricao = Console.ReadLine()?.Trim();

    var movimentacao = gerenciadorEstoque.Movimentar(
        codigoProduto,
        tipoMovimentacao,
        quantidade,
        descricao);

    Console.WriteLine();
    Console.WriteLine($"Movimentação: {movimentacao.Id}");
    Console.WriteLine($"Descrição: {movimentacao.Descricao}");
    Console.WriteLine($"Produto: {movimentacao.Produto.Descricao}");
    Console.WriteLine($"Estoque final: {movimentacao.Produto.QuantidadeEstoque}");
    Console.WriteLine();
}

static void ExibirProdutos(IEnumerable<Produto> produtos)
{
    Console.WriteLine("=== ESTOQUE ===");

    foreach (var produto in produtos)
    {
        Console.WriteLine(
            $"{produto.Codigo} - {produto.Descricao} - Estoque: {produto.QuantidadeEstoque}");
    }

    Console.WriteLine();
}

static int LerInteiro()
{
    var entrada = Console.ReadLine();

    if (!int.TryParse(entrada, out var valor) || valor <= 0)
    {
        throw new ArgumentException("Informe um número inteiro positivo.");
    }

    return valor;
}

public sealed class DadosEstoque
{
    [JsonPropertyName("estoque")]
    public List<Produto> Estoque { get; init; } = [];
}

public sealed class Produto
{
    [JsonPropertyName("codigoProduto")]
    public int Codigo { get; init; }
    [JsonPropertyName("descricaoProduto")]
    public string Descricao { get; init; } = string.Empty;
    [JsonPropertyName("estoque")]
    public int QuantidadeEstoque { get; private set; }

    public void AdicionarEstoque(int quantidade)
    {
        QuantidadeEstoque += quantidade;
    }

    public void RemoverEstoque(int quantidade)
    {
        if (quantidade > QuantidadeEstoque)
        {
            throw new InvalidOperationException(
                "A quantidade de saída não pode ser maior que o estoque disponível.");
        }

        QuantidadeEstoque -= quantidade;
    }
}

public sealed class Movimentacao
{
    public Guid Id { get; }
    public string Descricao { get; }
    public Produto Produto { get; }

    public Movimentacao(
        string descricao,
        Produto produto)
    {
        Id = Guid.NewGuid();
        Descricao = descricao;
        Produto = produto;
    }
}

public sealed class GerenciadorEstoque
{
    private readonly List<Produto> produtos;

    public GerenciadorEstoque(List<Produto> produtos)
    {
        this.produtos = produtos;
    }

    public Movimentacao Movimentar(
        int codigoProduto,
        string? tipoMovimentacao,
        int quantidade,
        string? descricao)
    {
        var produto = EncontrarProduto(codigoProduto);
        ValidarDescricao(descricao);

        if (tipoMovimentacao == "E")
        {
            produto.AdicionarEstoque(quantidade);
        }

        if (tipoMovimentacao == "S")
        {
            produto.RemoverEstoque(quantidade);
        }

        if (tipoMovimentacao != "E" && tipoMovimentacao != "S")
        {
            throw new ArgumentException(
                "O tipo de movimentação deve ser E ou S.");
        }

        return new Movimentacao(descricao!, produto);
    }

    private Produto EncontrarProduto(int codigoProduto)
    {
        return produtos.FirstOrDefault(
                   produto => produto.Codigo == codigoProduto)
               ?? throw new InvalidOperationException(
                   "Produto não encontrado.");
    }

    private static void ValidarDescricao(string? descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException(
                "A descrição da movimentação é obrigatória.");
        }
    }
}

