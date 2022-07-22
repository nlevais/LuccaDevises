using System.Globalization;

namespace LuccaDevisesLib;

public class Change
{
	public Dictionary<SensChange, decimal> ListeTauxChange { get; } = new();
	public string DeviseDepart { get; set; } = string.Empty;
	public string DeviseArrivee { get; set; } = string.Empty;
	public decimal Montant { get; set; }

	public void Charger(FileInfo fileInfo)
	{
#nullable disable
		using var sr = new StreamReader(fileInfo.FullName);

		var ligne1 = sr.ReadLine().Split(';');
		DeviseDepart = ligne1[0];
		DeviseArrivee = ligne1[2];
		Montant = decimal.Parse(ligne1[1], CultureInfo.InvariantCulture);

		int nbTauxChange = int.Parse(sr.ReadLine());
		for (int i = 0; i < nbTauxChange; i++)
		{
			var champs = sr.ReadLine().Split(';');
			ListeTauxChange.Ajout(new SensChange(champs[0], champs[1])
				, decimal.Parse(champs[2], CultureInfo.InvariantCulture));
		}
#nullable restore
	}

	public void Charger(TauxChange[] tauxChanges)
	{
		ListeTauxChange.Clear();
		foreach (var tauxChange in tauxChanges)
		{
			ListeTauxChange.Ajout(tauxChange.Sens, tauxChange.Taux);
		}
	}

	public decimal Convertir() => Convertir(DeviseDepart, DeviseArrivee, Montant);

	public decimal Convertir(string deviseDepart, string deviseArrivee, decimal montant)
	{
		Dictionary<SensChange, decimal> liste = new(ListeTauxChange);

		// On complète la liste avec les taux de change inverses
		foreach (var item in ListeTauxChange)
		{
			var inverse = new TauxChange(item.Key, item.Value).Inverse();
			liste.Ajout(inverse.Sens, inverse.Taux);
		}

		IEnumerable<(SensChange, decimal)> ChercheChemins(
			IEnumerable<KeyValuePair<SensChange, decimal>> restants,
			KeyValuePair<SensChange, decimal> prec,
			List<KeyValuePair<SensChange, decimal>> pile)
		{
			if (prec.Key.DeviseDepart.Equals(deviseDepart)
			&& prec.Key.DeviseArrivee.Equals(deviseArrivee))
				// Cas où un taux de change existe directement entre les deux devises
				yield return (prec.Key, prec.Value);
			else
			{
				pile.Add(prec);
				restants = restants.Except(new[] { prec });
				foreach (var item in restants
					.Where(c => c.Key.DeviseDepart.Equals(prec.Key.DeviseArrivee)
						&& !c.Key.DeviseArrivee.Equals(prec.Key.DeviseDepart)
						&& !c.Key.DeviseArrivee.Equals(deviseDepart)))
				{
					if (item.Key.DeviseArrivee.Equals(deviseArrivee))
					{
						// On retourne la pile complète qui match avec la recherche
						foreach (var itempile in pile)
						{
							yield return (itempile.Key, itempile.Value);
						}
						yield return (item.Key, item.Value);
					}
					else
						foreach (var item2 in ChercheChemins(restants, item, pile))
						{
							// On retourne la pile éventuellement trouvée au niveau d'imbrication suivant
							yield return item2;
						}
				}
				pile.Remove(prec);
			}
		}

		// Calcul en lui même, une fois les étapes les plus directes connues
		bool cheminTrouved = false;
		int indice = 0;
		decimal calcul = montant;
		foreach (var item in liste
			.Where(c => c.Key.DeviseDepart.Equals(deviseDepart))
			.SelectMany(c => ChercheChemins(liste, c, new()))
			.Select((t, index) =>
				{
					if (t.Item1.DeviseDepart.Equals(deviseDepart)) indice++;
					return new
					{
						DeviseDepart = t.Item1.DeviseDepart,
						DeviseArrivee = t.Item1.DeviseArrivee,
						Taux = t.Item2,
						Indice = indice,
					};
				})
			.GroupBy(c => new { c.Indice })
			.Select(g => new { NbEtapes = g.Count(), Liste = g.ToList() })
			.OrderBy(g => g.NbEtapes)
			.Select(g => g.Liste)
			.FirstOrDefault() ?? new())
		{
			cheminTrouved = true;
			calcul = Math.Round(calcul * item.Taux, 4);
		}

		if (cheminTrouved)
			return Math.Round(calcul);
		else
			return decimal.MinusOne;
	}

}