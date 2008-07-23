using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using N2.Persistence;
using N2.Web;

namespace N2.Tests.Web
{
	[TestFixture]
	public class UrlParserTests : ParserTestsBase
	{
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            parser = new UrlParser(persister, wrapper, notifier, host);
            CreateDefaultStructure();
        }

		#region Parse Page Tests
		[Test]
		public void CanParseStartPageUrl()
		{
			ContentItem parsedItem = parser.Parse("/");
			Assert.AreEqual(startItem, parsedItem);
		}

		[Test]
		public void CanParseItemOneLevelDown()
		{
			ContentItem parsedItem = parser.Parse("/item1.aspx");
			Assert.AreEqual(item1, parsedItem);
		}

		[Test]
		public void CanParseItemTwoLevelsDown()
		{
			ContentItem parsedItem = parser.Parse("/item1/item1_1.aspx");
			Assert.AreEqual(item1_1, parsedItem);
		}

		[Test]
		public void CanParseItemOneStepOneLevelDown()
		{
			ContentItem parsedItem = parser.Parse("/item2.aspx");
			Assert.AreEqual(item2, parsedItem);
		}

		[Test]
		public void CanParseItemOneStepTwoLevelsDown()
		{
			ContentItem parsedItem = parser.Parse("/item2/item2_1.aspx");
			Assert.AreEqual(item2_1, parsedItem);
		}

		[Test]
		public void ParseNonExistantItemYeldsNull()
		{
			ContentItem parsedItem = parser.Parse("/item3.aspx");
			Assert.IsNull(parsedItem);
		}

		[Test]
		public void CanParseItemWithMixedCase()
		{
			ContentItem parsedItem = parser.Parse("/iTeM2/ItEm2_1.AsPx");
			Assert.AreEqual(item2_1, parsedItem);
		}

		[Test]
		public void CanParseItemWithHash()
		{
			ContentItem parsedItem = parser.Parse("/item1.aspx#someHash");
			Assert.AreEqual(item1, parsedItem);
		}
		#endregion

		#region Parse Data Tests
		[Test]
		public void CanParseDataItemOnStartPage()
		{
			ContentItem parsedItem = parser.Parse("/?item=6");
			Assert.AreEqual(data1, parsedItem);
		}

		[Test]
		public void CanParseDataItemOneLevelDown()
		{
			ContentItem parsedItem = parser.Parse("/item2.aspx?item=7");
			Assert.AreEqual(data2, parsedItem);
		}

		[Test]
		public void CanParseDataItemTwoLevelsDown()
		{
			ContentItem parsedItem = parser.Parse("/item2/item2_1.aspx?item=8");
			Assert.AreEqual(data3, parsedItem);
		}

		[Test]
		public void ParseNonExistantDataItemReturnsPage()
		{
			ContentItem parsedItem = parser.Parse("/item2/item2_1.aspx?item=32");
			Assert.AreEqual(item2_1, parsedItem);
		}

		[Test]
		public void CanParseDataItemWithMixedCase()
		{
			ContentItem parsedItem = parser.Parse("/?iTeM=6");
			Assert.AreEqual(data1, parsedItem);
		}


		#endregion

		#region Page BuildUrl Tests
		[Test]
		public void CanCreateStartItemUrl()
		{
			string url = parser.BuildUrl(startItem);
			Assert.AreEqual("/", url);
		}

		[Test]
		public void CanCreateItemOneLevelDownUrl()
		{
			string url = parser.BuildUrl(item1);
			Assert.AreEqual("/item1.aspx", url);
		}

		[Test]
		public void CanCreateItemOneStepTwoLevelsDownUrl()
		{
			string url = parser.BuildUrl(item2_1);
			Assert.AreEqual("/item2/item2_1.aspx", url);
		} 

		#endregion

		#region Data BuildUrl Tests
		[Test]
		public void CanCreateDataItemUrlOnStartPage()
		{
			string url = parser.BuildUrl(data1);
			Assert.AreEqual("/?item=6", url);
		}

		[Test]
		public void CanCreateDataItemUrlOnPageOneLevelDown()
		{
			string url = parser.BuildUrl(data2);
			Assert.AreEqual("/item2.aspx?item=7", url);
		}

		[Test]
		public void CanCreateDataItemUrlOnPageTwoLevelsDown()
		{
			string url = parser.BuildUrl(data3);
			Assert.AreEqual("/item2/item2_1.aspx?item=8", url);
		}

		#endregion


		//TODO
		//[RowTest]
		//[Row("http://www.n2cms.com/")]
		//[Row("http://n2.libardo.com/")]
		//public void ParsingHostUrlYeldsNull(string url)
		//{
		//    using (mocks.Record())
		//    {
		//        startItem = CreateOneItem<PageItem>(1, "root", null);
		//        Expect.On(wrapper).Call(wrapper.ToAppRelative("/")).Return("/");
		//        mocks.ReplayAll();
		//    }
		//    ContentItem item = parser.Parse(url);
		//    Assert.IsNull(item, "Parsed item wasn't null");
		//}
	}
}
