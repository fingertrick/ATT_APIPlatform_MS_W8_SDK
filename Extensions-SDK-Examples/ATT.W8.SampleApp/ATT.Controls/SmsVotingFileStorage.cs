// <copyright file="SmsVotingFileStorage.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using ATT.Services;
using ATT.Utility;

namespace ATT.Controls
{
	/// <summary>
	/// Implementation of SMS Voting local file storage.
	/// </summary>
	public class SmsVotingFileStorage : ISmsVotingStorage
	{
		#region Constants

		private const string VotingNode = "voting";
		private const string ShortCodeNode = "shortCode";
		private const string VoteNode = "vote";
		private const string KeywordNode = "keyword";
		private const string CounterNode = "counter";

		#endregion Constants

		private readonly string _fileName;
		private readonly string _shortCode;
		private StorageFile _storageFile;
		private XmlDocument _xmlDoc;

		private static readonly SemaphoreSlim _threadSemaphore = new SemaphoreSlim(initialCount: 1);

		/// <summary>
		/// Creates instance of <see cref="SmsVotingFileStorage"/>.
		/// </summary>
		/// <param name="fileName">Physical file name.</param>
		/// <param name="shortCode">SMS short code.</param>
		/// <exception cref="System.ArgumentNullException">fileName is null or shortCode is null.</exception>
		public SmsVotingFileStorage(string fileName, string shortCode)
		{
			Argument.ExpectNotNull(() => fileName);
			Argument.ExpectNotNull(() => shortCode);

			_fileName = fileName;
			_shortCode = shortCode;
			OpenFileStorage();
		}

		/// <summary>
		/// Opens file storage.
		/// </summary>
		private async void OpenFileStorage()
		{
			await _threadSemaphore.WaitAsync();
			try
			{
				try
				{
					_storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(_fileName);
					var xmlSettings = new XmlLoadSettings();
					xmlSettings.ElementContentWhiteSpace = true;
					_xmlDoc = await XmlDocument.LoadFromFileAsync(_storageFile, xmlSettings);
				}
				catch (FileNotFoundException)
				{
					_storageFile = null;
				}
				catch // If file exists in incorrect format then LoadFromFileAsync throws System.Exception
				{
					_xmlDoc = null;
				}

				if (_storageFile == null)
				{
					_storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(_fileName, CreationCollisionOption.OpenIfExists);
				}

				if (_xmlDoc == null)
				{
					_xmlDoc = new XmlDocument();
					_xmlDoc.AppendChild(_xmlDoc.CreateElement("root"));
				}
				await _xmlDoc.SaveToFileAsync(_storageFile);
			}
			finally
			{
				_threadSemaphore.Release();
			}
		}

		/// <summary>
		/// Gets SMS voting statistics from local file storage.
		/// </summary>
		/// <returns>SMS voting statistics.</returns>
		public IEnumerable<KeyValuePair<string, int>> GetStatistics()
		{
			return GetStatisticsAsync().Result;
		}

		/// <summary>
		/// Adds voting results to the local file storage.
		/// </summary>
		/// <param name="msg">SMS message text.</param>
		/// <param name="count">Count of votes.</param>
		public void AddVote(string msg, int count)
		{
			AddVoteAsync(msg, count).RunSynchronously();
		}

		/// <summary>
		/// Adds voting results to the local file storage.
		/// </summary>
		/// <param name="messages">Set of <see cref="InboundSms"/> voting SMS messages.</param>
		public void AddVotes(IEnumerable<InboundSms> messages)
		{
			AddVotesAsync(messages).RunSynchronously();
		}

		/// <summary>
		/// Clears voting statistics from local file storage.
		/// </summary>
		public void ClearStatistics()
		{
			ClearStatisticsAsync().RunSynchronously();
		}

		/// <summary>
		/// Gets SMS voting statistics from local file storage.
		/// </summary>
		/// <returns>Returns Task as a result of asynchronous operation. Task result is SMS voting statistics.</returns>
		public async Task<IEnumerable<KeyValuePair<string, int>>> GetStatisticsAsync()
		{
			var result = new Dictionary<string, int>();
			await _threadSemaphore.WaitAsync();

			try
			{
				XmlElement votingElement = GetVotingElement(_shortCode);
				if (votingElement != null)
				{
					foreach (var node in votingElement.GetElementsByTagName("*"))
					{
						string shortCode = node.Attributes.GetNamedItem(KeywordNode).NodeValue.ToString();
						int counter = Convert.ToInt32(node.Attributes.GetNamedItem(CounterNode).NodeValue);
						result[shortCode] = counter;
					}
				}
			}
			finally
			{
				_threadSemaphore.Release();
			}

			return result;
		}

		/// <summary>
		/// Adds voting results to the local file storage.
		/// </summary>
		/// <param name="voteKey">SMS voting key.</param>
		/// <param name="count">Count of votes.</param>
		/// <returns>Returns Task as a result of asynchronous operation.</returns>
		public async Task AddVoteAsync(string voteKey, int count)
		{
			await _threadSemaphore.WaitAsync();
			try
			{
				IncreaseVoting(voteKey, count);
				await _xmlDoc.SaveToFileAsync(_storageFile);
			}
			finally
			{
				_threadSemaphore.Release();
			}
		}

		/// <summary>
		/// Increases voting in xml storage.
		/// </summary>
		/// <param name="voteKey">Voting key.</param>
		/// <param name="count">Voting count.</param>
		private void IncreaseVoting(string voteKey, int count)
		{
			XmlElement votingElement = GetVotingElement(_shortCode);
			if (votingElement == null)
			{
				votingElement = _xmlDoc.CreateElement(VotingNode);
				_xmlDoc.DocumentElement.AppendChild(votingElement);
				votingElement.SetAttribute(ShortCodeNode, _shortCode);
			}

			bool nodeExists = false;
			foreach (var node in votingElement.ChildNodes)
			{
				string keyword = node.Attributes.GetNamedItem(KeywordNode).NodeValue.ToString();
				if (String.Equals(keyword, voteKey))
				{
					IXmlNode voteAttr = node.Attributes.GetNamedItem(CounterNode);
					int vote = Convert.ToInt32(voteAttr.NodeValue);
					voteAttr.NodeValue = (vote + count).ToString();
					nodeExists = true;
					break;
				}
			}

			if (!nodeExists)
			{
				XmlElement element = _xmlDoc.CreateElement(VoteNode);
				votingElement.AppendChild(element);
				element.SetAttribute(KeywordNode, voteKey);
				element.SetAttribute(CounterNode, Convert.ToString(count));
			}
		}

		/// <summary>
		/// Gets main voting element by short code from xml storage
		/// </summary>
		/// <param name="shortCode">short code</param>
		/// <returns>Upper voting xml element</returns>
		private XmlElement GetVotingElement(string shortCode)
		{
			return _xmlDoc.DocumentElement.ChildNodes.FirstOrDefault(item => String.Equals(((XmlElement)item).GetAttribute(ShortCodeNode), shortCode)) as XmlElement;
		}

		/// <summary>
		/// Adds voting results to the local file storage.
		/// </summary>
		/// <param name="messages">Set of <see cref="InboundSms"/> voting SMS messages.</param>
		/// <returns>Returns Task as a result of asynchronous operation.</returns>
		public async Task AddVotesAsync(IEnumerable<InboundSms> messages)
		{
			var votes = new Dictionary<string, int>();
			foreach (var message in messages)
			{
				if (votes.ContainsKey(message.Body))
				{
					votes[message.Body] = votes[message.Body] + 1;
				}
				else
				{
					votes[message.Body] = 1;
				}
			}

			if (votes.Count == 0)
			{
				return;
			}

			await _threadSemaphore.WaitAsync();

			try
			{
				foreach (var key in votes.Keys)
				{
					IncreaseVoting(key, votes[key]);
				}

				await _xmlDoc.SaveToFileAsync(_storageFile);
			}
			finally
			{
				_threadSemaphore.Release();
			}
		}

		/// <summary>
		/// Clears voting statistics from local file storage.
		/// </summary>
		/// <returns>Returns Task as a result of asynchronous operation.</returns>
		public async Task ClearStatisticsAsync()
		{
			await _threadSemaphore.WaitAsync();
			try
			{
				XmlElement votingElement = GetVotingElement(_shortCode);
				if (votingElement != null)
				{
					_xmlDoc.DocumentElement.RemoveChild(votingElement);
					await _xmlDoc.SaveToFileAsync(_storageFile);
				}
			}
			finally
			{
				_threadSemaphore.Release();
			}
		}
	}
}
