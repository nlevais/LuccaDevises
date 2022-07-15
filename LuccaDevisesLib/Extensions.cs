namespace LuccaDevisesLib;
public static class Extensions
{
	/// <summary>
	/// Empêche les doublons sur la clé (sens)
	/// </summary>
	public static void Ajout(this Dictionary<SensChange, decimal> dictionary, SensChange sens, decimal taux)
	{
		if (dictionary.ContainsKey(sens))
			dictionary[sens] = taux;
		else
			dictionary.Add(sens, taux);
	}

}
