// <copyright file="MMSGalleryControlPresenterClass.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using ATT.Controls.Presenters;
using ATT.Services;

namespace ATT.Controls.Tests
{
    [TestClass]
    public class MmsGalleryControlPresenterClass
    {
        [TestClass]
        public class MmsCollectionProperty
        {
            [TestMethod]
            public void ShouldBeNotEmptyInitialized()
            {
                var presenter = new MmsGalleryControlPresenter("89654123", new StubMmsStorageNotEmpty());
                Assert.IsFalse(presenter.MmsCollection.Count == 0);
            }

            [TestMethod]
            public void ShouldBeEmptyInitialized()
            {
                var presenter = new MmsGalleryControlPresenter("89654123", new StubMmsStorageEmpty());
                Assert.IsTrue(presenter.MmsCollection.Count == 0);
            }
        }
    }

    internal class StubMmsStorageNotEmpty : IMmsStorage
    {
        public Task<IEnumerable<InboundMms>> GetMmsMessages(string shortCode)
        {
            IEnumerable<InboundMms> result = new[] { new InboundMms("123", new PhoneNumber("234"), "test") };
            return Task.FromResult(result);
        }

        public Task StoreMmsMessages(IEnumerable<InboundMms> mmsCollection, string shortCode)
        {
            throw new NotImplementedException();
        }
    }

    internal class StubMmsStorageEmpty : IMmsStorage
    {
        public Task<IEnumerable<InboundMms>> GetMmsMessages(string shortCode)
        {
            var result = Enumerable.Empty<InboundMms>();
            return Task.FromResult(result);
        }

        public Task StoreMmsMessages(IEnumerable<InboundMms> mmsCollection, string shortCode)
        {
            throw new NotImplementedException();
        }
    }
}
