using N2.Engine.Castle;
using N2.Definitions;
using N2.Details;
using N2.Engine;
using N2.Engine.MediumTrust;
using N2.Installation;
using N2.Integrity;
using N2.Persistence;
using N2.Persistence.Finder;
using N2.Plugin;
using N2.Security;
using N2.Web;
using NUnit.Framework;
using N2.Edit.Installation;

namespace N2.Tests.Engine
{
	[TestFixture]
	public class WindsorCastleServiceContainerTests : ContainerSanityTests
	{
		protected override IEngine CreateEngine()
		{
			var engine = new ContentEngine(new WindsorServiceContainer());

			return engine;
		}
	}

	[TestFixture]
	public class MediumTrustServiceContainerTests : ContainerSanityTests
	{
		protected override IEngine CreateEngine()
		{
			var engine = new ContentEngine(new MediumTrustServiceContainer());

			return engine;
		}
	}

	/// <summary>
	/// Class that allows you to unit test any IEngine implementations
	/// </summary>
	public abstract class ContainerSanityTests
	{
		IServiceContainer container;

		[SetUp]
		public void SetUp()
		{
			container = CreateEngine().Container;
		}

		protected abstract IEngine CreateEngine();

		[Test]
		public void CanRetrieve_ImportantServices()
		{
			Assert.That(container.Resolve<IRequestDispatcher>(), Is.Not.Null);
			Assert.That(container.Resolve<IWebContext>(), Is.Not.Null);
			Assert.That(container.Resolve<IHost>(), Is.Not.Null);

			Assert.That(container.Resolve<IRepository<int, ContentItem>>(), Is.Not.Null);
			Assert.That(container.Resolve<IRepository<int, ContentDetail>>(), Is.Not.Null);
			Assert.That(container.Resolve<IRepository<int, AuthorizedRole>>(), Is.Not.Null);
			Assert.That(container.Resolve<IRepository<int, DetailCollection>>(), Is.Not.Null);
			Assert.That(container.Resolve<IPersister>(), Is.Not.Null);
			Assert.That(container.Resolve<IItemFinder>(), Is.Not.Null);
			Assert.That(container.Resolve<IItemNotifier>(), Is.Not.Null);

			Assert.That(container.Resolve<IIntegrityManager>(), Is.Not.Null);
			Assert.That(container.Resolve<IIntegrityEnforcer>(), Is.Not.Null);

			Assert.That(container.Resolve<ISecurityManager>(), Is.Not.Null);
			Assert.That(container.Resolve<ISecurityEnforcer>(), Is.Not.Null);

			Assert.That(container.Resolve<IDefinitionManager>(), Is.Not.Null);

			Assert.That(container.Resolve<IEngine>(), Is.Not.Null);

			Assert.That(container.Resolve<IPluginBootstrapper>(), Is.Not.Null);

			Assert.That(container.Resolve<InstallationManager>(), Is.Not.Null);
		}

		[Test]
		public void AddComponentLifeStyle_DoesNotReturnSameServiceTwiceWhenSingleton()
		{
			container.AddComponentLifeStyle("testing", typeof(object), ComponentLifeStyle.Singleton);

			var class1 = container.Resolve<object>();
			var class2 = container.Resolve<object>();

			Assert.That(class1, Is.SameAs(class2));
		}

		[Test]
		public void AddComponentLifeStyle_DoesNotReturnSameServiceTwiceWhenTransient()
		{
			container.AddComponentLifeStyle("testing", typeof(object), ComponentLifeStyle.Transient);

			var class1 = container.Resolve<object>();
			var class2 = container.Resolve<object>();

			Assert.That(class1, Is.Not.SameAs(class2));
		}
	}
}