using NUnit.Framework;
using UnityEngine;

namespace InsaneOne.Modifiers.Tests
{
	public class FilterTests
	{
		GameObject teamOneA, teamOneB, teamTwo;

		[SetUp]
		public void SetUp()
		{
			teamOneA = CreateWithTeam("TeamOneA", 1);
			teamOneB = CreateWithTeam("TeamOneB", 1);
			teamTwo = CreateWithTeam("TeamTwo", 2);
		}

		[TearDown]
		public void TearDown()
		{
			foreach (var go in new[] { teamOneA, teamOneB, teamTwo })
				if (go)
					Object.DestroyImmediate(go);
		}

		static GameObject CreateWithTeam(string name, float team)
		{
			var go = new GameObject(name);
			go.AddComponent<Modifiable>().SetValue("Team", team);

			return go;
		}

		[Test]
		public void Make_IncludeFilter_ReturnsOnlyMatchingObjects()
		{
			var filter = Filter.Make("Team", 1);

			var results = filter.GetResults();

			CollectionAssert.Contains(results, teamOneA);
			CollectionAssert.Contains(results, teamOneB);
			CollectionAssert.DoesNotContain(results, teamTwo);
		}

		[Test]
		public void Make_ExcludeFilter_ReturnsNonMatchingObjects()
		{
			// Regression test: a freshly created exclude filter used to populate itself via the equals-only
			// Modifiable.FindAllWith, so it started out with exactly the wrong (matching, not non-matching) set.
			var filter = Filter.Make("Team", 1, isExclude: true);

			var results = filter.GetResults();

			CollectionAssert.DoesNotContain(results, teamOneA);
			CollectionAssert.DoesNotContain(results, teamOneB);
			CollectionAssert.Contains(results, teamTwo);
		}

		[Test]
		public void Make_SameDefinitionTwice_ReturnsSameCachedInstance()
		{
			var first = Filter.Make("Team", 1);
			var second = Filter.Make("Team", 1);

			Assert.AreSame(first, second);
		}

		[Test]
		public void Make_SameTypeAndValueButDifferentIsExclude_ReturnsDistinctFilters()
		{
			// Regression test: Make() used to reuse a cached filter based on whether the requested value
			// satisfied the *existing* filter's own predicate, ignoring the requested isExclude, so this used
			// to hand back the include-filter instead of creating a proper exclude one.
			var include = Filter.Make("Team", 1, isExclude: false);
			var exclude = Filter.Make("Team", 1, isExclude: true);

			Assert.AreNotSame(include, exclude);
			Assert.IsFalse(include.IsExclude);
			Assert.IsTrue(exclude.IsExclude);
		}

		[Test]
		public void GetResults_ExcludesInactiveGameObjects()
		{
			var filter = Filter.Make("Team", 1);
			teamOneA.SetActive(false);

			var results = filter.GetResults();

			CollectionAssert.DoesNotContain(results, teamOneA);
			CollectionAssert.Contains(results, teamOneB);
		}

		[Test]
		public void SetValue_AfterFilterCreated_UpdatesMembershipDynamically()
		{
			// Regression test: Filter.UpdateAll was never invoked from anywhere, so a Modifiable's value
			// change after the filter was created never updated its result set.
			var filter = Filter.Make("Team", 1);
			var newcomer = CreateWithTeam("Newcomer", 2);

			try
			{
				CollectionAssert.DoesNotContain(filter.GetResults(), newcomer);

				newcomer.GetComponent<Modifiable>().SetValue("Team", 1);

				CollectionAssert.Contains(filter.GetResults(), newcomer);
			}
			finally
			{
				Object.DestroyImmediate(newcomer);
			}
		}

		[Test]
		public void AddModifier_AfterFilterCreated_UpdatesMembershipDynamically()
		{
			var filter = Filter.Make("Team", 2);
			var modifiable = teamOneA.GetComponent<Modifiable>();
			var switchToTeamTwo = TestUtils.CreateUnityModifier("SwitchTeam", ("Team", 1));

			CollectionAssert.DoesNotContain(filter.GetResults(), teamOneA);

			modifiable.Add(switchToTeamTwo);

			CollectionAssert.Contains(filter.GetResults(), teamOneA);

			modifiable.Remove(switchToTeamTwo);

			CollectionAssert.DoesNotContain(filter.GetResults(), teamOneA);
		}
	}
}
