namespace LuccaDevisesLib;

public class TauxChange
{
	public SensChange Sens { get; }
	public decimal Taux { get; }

	public TauxChange(SensChange sens, decimal taux)
	{
		Sens = sens;
		Taux = taux;
	}

	public TauxChange Inverse()
		=> new TauxChange(new SensChange(Sens.DeviseArrivee, Sens.DeviseDepart), Math.Round(1 / Taux, 4));
}
