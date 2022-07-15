using LuccaDevisesLib;

try
{
	var change = new Change();
	change.Charger(new FileInfo(args[0]));
	decimal conv = change.Convertir();
	if (conv < 0)
		Console.WriteLine("Calcul impossible avec les données de conversion fournies.");
	else
		Console.WriteLine(conv);
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
