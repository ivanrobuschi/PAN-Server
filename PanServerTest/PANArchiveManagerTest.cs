using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using System.Transactions;
using PANserver;

namespace PANserver
{
    class PANArchiveManagerTest
    {
        PANserverEntities _db;
        TransactionScope ts;
        PANArchiveManager _sut;

        string PAN = "pan";
        string mask = "mask";

        [SetUp]
        public void SetUp()
        {
            _db = new PANserverEntities();
            _db.Database.CreateIfNotExists();
            ts = new TransactionScope(TransactionScopeOption.RequiresNew);
            _sut = new PANArchiveManager(_db);
        }

        [TearDown]
        public void TearDown()
        {
            ts.Dispose();
        }

        [Test]
        public void AddPanAndMaskShouldAddANewRecordInTheTable()
        {
            _sut.AddPanAndMask(PAN, mask);

            var panData = _db.PANs.Single();
            panData.PAN.Trim().Should().Be(PAN);
            panData.MaskedPAN.Trim().Should().Be(mask);
        }

        [Test]
        public void SearchPANShouldReturnTheRelatedPan()
        {
            PANs newRecord = new PANs();
            newRecord.PAN = PAN;
            newRecord.MaskedPAN = mask;
            _db.PANs.Add(newRecord);
            _db.SaveChanges();
            _sut.SearchPAN(mask).Should().Be(PAN);
        }

        [Test]
        public void SearchPanShouldReturnNullWhenMaskIsNotFound()
        {
            _sut.SearchPAN(mask).Should().Be(null);
        }

        [Test]
        public void SearchMaskShouldReturnTheRelatedMask()
        {
            PANs newRecord = new PANs();
            newRecord.PAN = PAN;
            newRecord.MaskedPAN = mask;
            _db.PANs.Add(newRecord);
            _db.SaveChanges();
            _sut.SearchMask(PAN).Should().Be(mask);
        }

        [Test]
        public void SearchMaskShouldReturnNullWhenPanIsNotFound()
        {
            _sut.SearchPAN(PAN).Should().Be(null);
        }

    }
}
