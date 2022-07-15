namespace Tests
{
	public class TestsChanges
	{
		[Fact]
		public void un_seul_change_modified()
		{
			var change = new Change();
			change.Charger(new TauxChange[]
				{
					new(new("EUR", "USD"), 1.2989m),
					new(new("EUR", "USD"), 1.9m),
				});
			Assert.True(change.ListeTauxChange.Count == 1);
		}
		[Fact]
		public void deux_changes()
		{
			var change = new Change();
			change.Charger(new TauxChange[]
				{
					new(new("EUR", "USD"), 1.2989m),
					new(new("EUR", "CHF"), 1.2053m),
				});
			Assert.True(change.ListeTauxChange.Count == 2);
		}
		[Fact]
		public void un_change_et_son_inverse()
		{
			var change = new Change();
			change.Charger(new TauxChange[]
				{
					new(new("EUR", "USD"), 1.2989m),
					new(new("USD", "EUR"), 0.7699m),
				});
			Assert.True(change.ListeTauxChange.Count == 2);
		}
	}
}