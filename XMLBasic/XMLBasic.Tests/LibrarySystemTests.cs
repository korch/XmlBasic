using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XMLBasic.Entities;
using XMLBasic.Interfaces;
using XMLBasic.Readers;
using XMLBasic.Writers;

namespace XMLBasic.Tests
{
    public class LibrarySystemTests
    {
        [TestFixture]
        public class CatalogSystemTest
        {
            private LibrarySystem _catalog;



            [SetUp]
            public void Init()
            {
                _catalog = new LibrarySystem();
                _catalog.AddParsers(new BookReader(), new PaperReader(), new PatentReader());
                _catalog.AddWriters(new BookWriter(), new PaperWriter(), new PatentWriter());
            }
            private Paper CreatePaper()
            {
                return new Paper
                {
                    Name = "Paper",
                    PublicationCity = "Ratomka",
                    PublishingHouseName = "The best of Ratomka stories",
                    PublicationYear = 2018,
                    PapersCount = 22,
                    Date = new DateTime(2018, 10, 25),
                    ISBN = "225544-4343",
                    Number = 1,
                    Note = "The best paper ever!"
                };
            }

            private Patent CreatePatent()
            {
                return new Patent
                {
                    Name = "Flying Car",
                    Country = "Belarus",
                    RegistrationNumber = "BY251044322",
                    ClaimDate = new DateTime(2018, 10, 25),
                    PublicationDate = new DateTime(2018, 12, 15),
                    PapersCount = 345,
                    Note = "The first a true flying car in the world!",
                    Inventors = new List<Inventor> {
                        new Inventor() {Name = "Egor", SurName = "Sobalevsky"},
                        new Inventor {Name = "Ivan", SurName = "Tatur"}
                    }
                };
            }

            private Book CreateBook()
            {
                return new Book
                {
                    Name = "How you can waste you time without the positive result",
                    PublicationCity = "Minsk",
                    PublishingHouseName = "Minsk",
                    PublicationYear = 2018,
                    PapersCount = 254,
                    ISBN = "111-222-333-444-555",
                    Note = "Just a book.",
                    Authors = new List<Author> {
                        new Author {Name = "Egor", SurName = "Sobalevsky"}
                    }
                };
            }

            private string GetBookXml()
            {
                return @"<book name=""How you can waste you time without the positive result"" " +
                       @"publicationCity=""Minsk"" " +
                       @"publishingHouseName=""Minsk"" " +
                       @"publicationYear=""2018"" " +
                       @"papersCount=""254"" " +
                       @"isbn=""111-222-333-444-555"">" +
                       "<note>Just a book.</note>" +
                       "<authors>1" +
                       @"<author name=""Egor"" surname=""Sobalevsky"" />" +
                       "</authors>" +
                       "</book>";
            }

            private string GetPaperXml()
            {
                return "<paper name=\"Paper\" " +
                       "publicationCity=\"Ratomka\" " +
                       "publishingHouseName=\"The best of Ratomka stories\" " +
                       "publicationYear=\"2018\" " +
                       "papersCount=\"22\" " +
                       "date=\"25/10/2018\" " +
                       "isbn=\"225544-4343\" " +
                       "number=\"1\">" +
                       "<note>The best paper ever!</note>" +
                       "</paper>";
            }
         
            private string GetPatentXml()
            {
                return "<patent name=\"Flying Car\" " +
                       "country=\"Belarus\" " +
                       "registrationNumber=\"BY251044322\" " +
                       "claimDate=\"25/10/2018\" " +
                       "publicationDate=\"15/12/2018\" " +
                       "papersCount=\"345\">" +
                       "<note>The first a true flying car in the world!</note>" +
                       "<inventors>1" +
                       "<inventor name=\"Egor\" surname=\"Sobalevsky\" />" +
                       "<inventor name=\"Ivan\" surname=\"Tatur\" />" +
                       "</inventors>" +
                       "</patent>";
            }

            [Test]
            public void Test_Books_Read()
            {
                TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                                 "<catalog>" +
                                                 GetBookXml() +
                                                 "</catalog>");

                IEnumerable<IEntity> books = _catalog.ReadFrom(sr).ToList();

                CollectionAssert.AreEqual(books, new[]
                {
                    CreateBook()
                }, new BooksComparer());

                sr.Dispose();
            }

            [Test]
            public void Test_Papers_Read()
            {
                TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                                 "<catalog>" +
                                                 GetPaperXml() +
                                                 "</catalog>");

                IEnumerable<IEntity> papers = _catalog.ReadFrom(sr);

                CollectionAssert.AreEqual(papers, new[] {
                    CreatePaper()
                }, new NewsPaperComparer());

                sr.Dispose();
            }

            [Test]
            public void Test_Patents_Read()
            {
                TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                                 "<catalog>" +
                                                 GetPatentXml() +
                                                 "</catalog>");

                IEnumerable<IEntity> newspapers = _catalog.ReadFrom(sr);

                CollectionAssert.AreEqual(newspapers, new[]
                {
                    CreatePatent()
                }, new PatentComparer());

                sr.Dispose();
            }

            [Test]
            public void Test_MixedEntities_Read()
            {
                TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                                 "<catalog>" +
                                                 GetPatentXml() +
                                                 GetBookXml() +
                                                 GetPaperXml() +
                                                 "</catalog>");

                List<IEntity> entities = _catalog.ReadFrom(sr).ToList();

                Assert.IsTrue(new PatentComparer().Compare(entities[0], CreatePatent()) == 0);
                Assert.IsTrue(new BooksComparer().Compare(entities[1], CreateBook()) == 0);
                Assert.IsTrue(new NewsPaperComparer().Compare(entities[2], CreatePaper()) == 0);

                sr.Dispose();
            }

            [Test]
            public void Test_MixedEntities_Write()
            {
                StringBuilder actualResult = new StringBuilder();
                StringWriter sw = new StringWriter(actualResult);
                var book = CreateBook();
                var paper = CreatePaper();
                var patent = CreatePatent();
                var entities = new IEntity[] {
                    book,
                    paper,
                    patent
                };
                string expectedResult = @"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                        "<catalog>" +
                                        GetBookXml() +
                                        GetPaperXml() +
                                        GetPatentXml() +
                                        "</catalog>";

                _catalog.WriteTo(sw, entities);
                sw.Dispose();

                Assert.AreEqual(expectedResult, actualResult.ToString());
            }

          
        }

        internal class BooksComparer : IComparer, IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.PapersCount == y.PapersCount
                       && x.Name == y.Name
                       && x.ISBN == y.ISBN
                       && x.Note == y.Note
                       && x.PapersCount == y.PapersCount
                       && x.PublicationYear == y.PublicationYear
                       && x.PublicationCity == y.PublicationCity
                       && x.PublishingHouseName == y.PublishingHouseName
                    ? 0
                    : 1;
            }

            public int Compare(object x, object y)
            {
                return Compare(x as Book, y as Book);
            }
        }

        internal class NewsPaperComparer : IComparer, IComparer<Paper>
        {
            public int Compare(object x, object y)
            {
                return Compare(x as Paper, y as Paper);
            }

            public int Compare(Paper x, Paper y)
            {
                return x.Name == y.Name &&
                       x.PublicationCity == y.PublicationCity &&
                       x.PublishingHouseName == y.PublishingHouseName &&
                       x.PublicationYear == y.PublicationYear &&
                       x.PapersCount == y.PapersCount &&
                       x.Note == y.Note &&
                       x.Number == y.Number &&
                       x.Date == y.Date &&
                       x.ISBN == y.ISBN
                    ? 0
                    : 1;
            }
        }

        internal class PatentComparer : IComparer, IComparer<Patent>
        {
            public int Compare(object x, object y)
            {
                return Compare(x as Patent, y as Patent);
            }

            public int Compare(Patent x, Patent y)
            {
                return x.Name == y.Name &&
                       x.Country == y.Country &&
                       x.RegistrationNumber == y.RegistrationNumber &&
                       x.ClaimDate == y.ClaimDate &&
                       x.PublicationDate == y.PublicationDate &&
                       x.PapersCount == y.PapersCount &&
                       x.Note == y.Note
                    ? 0
                    : 1;
            }
        }
    }
}
