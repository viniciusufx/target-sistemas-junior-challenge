using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

const string json = """
{
  "vendas": [
    { "vendedor": "João Silva", "valor": 1200.50 },
    { "vendedor": "João Silva", "valor": 950.75 },
    { "vendedor": "João Silva", "valor": 1800.00 },
    { "vendedor": "João Silva", "valor": 1400.30 },
    { "vendedor": "João Silva", "valor": 1100.90 },
    { "vendedor": "João Silva", "valor": 1550.00 },
    { "vendedor": "João Silva", "valor": 1700.80 },
    { "vendedor": "João Silva", "valor": 250.30 },
    { "vendedor": "João Silva", "valor": 480.75 },
    { "vendedor": "João Silva", "valor": 320.40 },
    { "vendedor": "Maria Souza", "valor": 2100.40 },
    { "vendedor": "Maria Souza", "valor": 1350.60 },
    { "vendedor": "Maria Souza", "valor": 950.20 },
    { "vendedor": "Maria Souza", "valor": 1600.75 },
    { "vendedor": "Maria Souza", "valor": 1750.00 },
    { "vendedor": "Maria Souza", "valor": 1450.90 },
    { "vendedor": "Maria Souza", "valor": 400.50 },
    { "vendedor": "Maria Souza", "valor": 180.20 },
    { "vendedor": "Maria Souza", "valor": 90.75 },
    { "vendedor": "Carlos Oliveira", "valor": 800.50 },
    { "vendedor": "Carlos Oliveira", "valor": 1200.00 },
    { "vendedor": "Carlos Oliveira", "valor": 1950.30 },
    { "vendedor": "Carlos Oliveira", "valor": 1750.80 },
    { "vendedor": "Carlos Oliveira", "valor": 1300.60 },
    { "vendedor": "Carlos Oliveira", "valor": 300.40 },
    { "vendedor": "Carlos Oliveira", "valor": 500.00 },
    { "vendedor": "Carlos Oliveira", "valor": 125.75 },
    { "vendedor": "Ana Lima", "valor": 1000.00 },
    { "vendedor": "Ana Lima", "valor": 1100.50 },
    { "vendedor": "Ana Lima", "valor": 1250.75 },
    { "vendedor": "Ana Lima", "valor": 1400.20 },
    { "vendedor": "Ana Lima", "valor": 1550.90 },
    { "vendedor": "Ana Lima", "valor": 1650.00 },
    { "vendedor": "Ana Lima", "valor": 75.30 },
    { "vendedor": "Ana Lima", "valor": 420.90 },
    { "vendedor": "Ana Lima", "valor": 315.40 }
  ]
}
""";

var dados = JsonSerializer.Deserialize<DadosVendas>(json)
    ?? throw new InvalidOperationException("Não foi possível ler os dados.");

IComissaoCalculator calculadora = new ComissaoCalculator();
var resultados = calculadora.Calcular(dados.Vendas);

foreach (var resultado in resultados)
{
    Console.WriteLine(
        $"{resultado.Vendedor}: R$ {resultado.Comissao:N2}");
}

public sealed class DadosVendas
{
    [JsonPropertyName("vendas")]
    public List<Venda> Vendas { get; init; } = [];
}

public sealed class Venda
{
    [JsonPropertyName("vendedor")]
    public string Vendedor { get; init; } = string.Empty;
    [JsonPropertyName("valor")]
    public decimal Valor { get; init; }
}

public sealed class ResultadoComissao
{
    public string Vendedor { get; }
    public decimal Comissao { get; }

    public ResultadoComissao(string vendedor, decimal comissao)
    {
        Vendedor = vendedor;
        Comissao = comissao;
    }
}

public interface IComissaoCalculator
{
    IReadOnlyCollection<ResultadoComissao> Calcular(
        IEnumerable<Venda> vendas);
}

public sealed class ComissaoCalculator : IComissaoCalculator
{
    public IReadOnlyCollection<ResultadoComissao> Calcular(
        IEnumerable<Venda> vendas)
    {
        return vendas
            .GroupBy(venda => venda.Vendedor)
            .Select(CalcularComissaoDoVendedor)
            .ToList();
    }

    private static ResultadoComissao CalcularComissaoDoVendedor(
        IGrouping<string, Venda> vendasDoVendedor)
    {
        var comissao = vendasDoVendedor.Sum(CalcularComissaoDaVenda);

        return new ResultadoComissao(
            vendasDoVendedor.Key,
            comissao);
    }

    private static decimal CalcularComissaoDaVenda(Venda venda)
    {
        var percentual = ObterPercentualDeComissao(venda.Valor);

        return venda.Valor * percentual;
    }

    private static decimal ObterPercentualDeComissao(decimal valor)
    {
        if (valor < 100)
            return 0m;

        if (valor < 500)
            return 0.01m;

        return 0.05m;
    }
}