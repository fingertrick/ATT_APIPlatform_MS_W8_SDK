// <copyright file="LocalFileMmsStorage.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using ATT.Services;

namespace ATT.Controls
{
	/// <summary>
	/// Class that can save and load mms into a local file 
	/// </summary>
	public class LocalFileMmsStorage : IMmsStorage
	{
		private const string MmsLibraryFolder = "MMSLibrary";
		private const string MmsFileExtension = ".xml";
		private const string MmsElementName = "MMS"; 
		private const string MmsCollectionElementName = "MMSs";
		private const string IdElementName = "Id";
		private const string PhoneElementName = "Phone";
		private const string BodyElementName = "Body";
		private const string PicturesCollectionElementName = "Pictures";
		private const string PictureElementName = "Picture";
	  
		/// <summary>
		/// Loads MMS messages collection from local storage.
		/// </summary>
		/// <param name="shortCode">A short code is an alternate phone number you can use instead of a phone number.  This way your computer can receive messages as if it were a phone. Learn more about short codes, or get one, at developer.att.com.</param>
		/// <returns>List of <see cref="InboundMms"/> loaded MMS messages collection.</returns>
		public async Task<IEnumerable<InboundMms>> GetMmsMessages(string shortCode)
		{
			StorageFile mmsFileStorage;
			string shortCodeFolderPath;

			try
			{
				string fileName = Path.ChangeExtension(shortCode, MmsFileExtension);
				shortCodeFolderPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, MmsLibraryFolder, shortCode);
				string filePath = Path.Combine(shortCodeFolderPath, fileName);

				mmsFileStorage = await StorageFile.GetFileFromPathAsync(filePath);
			}
			catch (FileNotFoundException)
			{
				return Enumerable.Empty<InboundMms>();
			}
		   
			var xmlMmsCollection = XDocument.Load(await mmsFileStorage.OpenStreamForReadAsync());
			var savedMessages = 
						from mms in xmlMmsCollection.Descendants(MmsElementName)
							  select new
							  {
									Id = mms.Element(IdElementName).Value,
									Phone = mms.Element(PhoneElementName).Value,
									Body = mms.Element(BodyElementName).Value,
									ImagePaths = mms.Element(PicturesCollectionElementName)
													.Descendants(PictureElementName)
													.Select(p => Path.Combine(shortCodeFolderPath, p.Value))
							  };

			return savedMessages.Select(message => new InboundMms(message.Id, new PhoneNumber(message.Phone), message.Body, message.ImagePaths)).ToList();
		}

		/// <summary>
		/// Saves MMS into storage.
		/// </summary>
		/// <param name="mmsCollection">List of <see cref="InboundMms"/> MMS.</param>.
		/// <param name="shortCode">A short code is an alternate phone number you can use instead of a phone number.  This way your computer can receive messages as if it were a phone. Learn more about short codes, or get one, at developer.att.com.</param>
		public async Task StoreMmsMessages(IEnumerable<InboundMms> mmsCollection, string shortCode)
		{		   
			string fileName = Path.ChangeExtension(shortCode, MmsFileExtension);
			StorageFolder rootFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(MmsLibraryFolder, CreationCollisionOption.OpenIfExists);
			StorageFolder mmsStorage = await rootFolder.CreateFolderAsync(shortCode, CreationCollisionOption.OpenIfExists);

			bool fileExist = true;
			try
			{
				await mmsStorage.GetFileAsync(fileName);			   
			}
			catch(FileNotFoundException)
			{
				fileExist = false;
			}

			StorageFile mmsFileStorage = await mmsStorage.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

			XDocument xmlMmsCollection = fileExist ? XDocument.Load(await mmsFileStorage.OpenStreamForWriteAsync()) : new XDocument(new XElement(MmsCollectionElementName));

			foreach (InboundMms mms in mmsCollection)
			{
				xmlMmsCollection.Element(MmsCollectionElementName).Add(new XElement(MmsElementName,
					new XElement(IdElementName, mms.Id),
					new XElement(PhoneElementName, mms.SenderNumber.Number),
					new XElement(BodyElementName, mms.Body),
					new XElement(PicturesCollectionElementName, 
								 mms.Attachments.Select(a => new XElement(PictureElementName, a)))));			 
			}

			IRandomAccessStream writeStream = await mmsFileStorage.OpenAsync(FileAccessMode.ReadWrite);
			Stream stream = writeStream.AsStreamForWrite();			  
			xmlMmsCollection.Save(stream);
		}
	}
}
