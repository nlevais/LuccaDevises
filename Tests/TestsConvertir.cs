namespace Tests
{
	public class TestsConvertir
	{
		[Fact]
		public void cas_énoncé()
		{
			var change = new Change();
			change.Charger(new TauxChange[]
				{
					new(new("AUD", "CHF"), 0.9661m),
					new(new("JPY", "KRW"), 13.1151m),
					new(new("EUR", "CHF"), 1.2053m),
					new(new("AUD", "JPY"), 86.0305m),
					new(new("EUR", "USD"), 1.2989m),
					new(new("JPY", "INR"), 0.6571m),
				});
			Assert.True(change.Convertir("EUR", "JPY", 550m) == 59033);
		}

		[Fact]
		public void cas_direct_taux_connu()
		{
			var change = new Change();
			change.Charger(new TauxChange[]
				{
					new(new("AUD", "CHF"), 0.9661m),
					new(new("JPY", "KRW"), 13.1151m),
					new(new("EUR", "CHF"), 1.2053m),
					new(new("AUD", "JPY"), 86.0305m),
					new(new("EUR", "USD"), 1.2989m),
					new(new("JPY", "INR"), 0.6571m),
					new(new("EUR", "JPY"), 2m),
				});
			Assert.True(change.Convertir("EUR", "JPY", 550m) == 1100m);
		}

		[Fact]
		public void cas_énoncé_plus_taux_plus_direct()
		{
			var change = new Change();
			change.Charger(new TauxChange[]
				{
					new(new("AUD", "CHF"), 0.9661m),
					new(new("JPY", "KRW"), 13.1151m),
					new(new("EUR", "CHF"), 1.2053m),
					new(new("AUD", "JPY"), 86.0305m),
					new(new("EUR", "USD"), 1.2989m),
					new(new("JPY", "INR"), 0.6571m),
					new(new("USD", "JPY"), 2m),
				});
			Assert.True(change.Convertir("EUR", "JPY", 550m) == 1429m);
		}
	}
}
