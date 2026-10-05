using System.Globalization;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

Console.Write("Digite o valor: R$ ");
var valor = decimal.Parse(
    Console.ReadLine()!,
    CultureInfo.GetCultureInfo("pt-BR"));

Console.Write("Digite a data de vencimento (dd/MM/yyyy): ");
var dataVencimento = DateTime.ParseExact(
    Console.ReadLine()!,
    "dd/MM/yyyy",
    CultureInfo.InvariantCulture);

var dataAtual = DateTime.Today;
var diasEmAtraso = Math.Max(0, (dataAtual - dataVencimento).Days);

const decimal percentualJurosDiario = 0.025m;

var juros = valor * percentualJurosDiario * diasEmAtraso;
var valorFinal = valor + juros;

Console.WriteLine();
Console.WriteLine($"Dias em atraso: {diasEmAtraso}");
Console.WriteLine($"Juros: {juros:C}");
Console.WriteLine($"Valor final: {valorFinal:C}");