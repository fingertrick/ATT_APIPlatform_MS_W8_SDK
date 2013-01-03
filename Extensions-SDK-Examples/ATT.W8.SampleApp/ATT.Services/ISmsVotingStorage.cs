// <copyright file="ISmsVotingStorage.cs" company="AT&amp;T">
// Licensed by AT&amp;T under 'Software Development Kit Tools Agreement.' 2012
// TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION: http://developer.att.com/sdk_agreement/
// Copyright 2012 AT&amp;T Intellectual Property. All rights reserved. http://developer.att.com
// For more information contact developer.support@att.com
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATT.Services
{
	/// <summary>
	/// Interface for getting SMS voting results from storage or setting SMS voting results into storage.
	/// </summary>
	public interface ISmsVotingStorage
	{
		/// <summary>
		/// Gets SMS voting statistics from storage.
		/// </summary>
		/// <returns>SMS voting statistics.</returns>
		IEnumerable<KeyValuePair<string, int>> GetStatistics();

		/// <summary>
		/// Adds voting results to the storage.
		/// </summary>
		/// <param name="msg">SMS message text.</param>
		/// <param name="count">Count of votes.</param>
		void AddVote(string msg, int count);

		/// <summary>
		/// Adds voting results to the storage.
		/// </summary>
		/// <param name="messages">Set of <see cref="InboundSms"/> voting SMS messages.</param>
		void AddVotes(IEnumerable<InboundSms> messages);

		/// <summary>
		/// Clears voting statistics in storage.
		/// </summary>
		void ClearStatistics();

		/// <summary>
		/// Gets SMS voting statistics from storage.
		/// </summary>
		/// <returns>Returns Task as a result of asynchronous operation. Task result is SMS voting statistics.</returns>
		Task<IEnumerable<KeyValuePair<string, int>>> GetStatisticsAsync();

		/// <summary>
		/// Adds voting results to the storage.
		/// </summary>
		/// <param name="msg">SMS message text.</param>
		/// <param name="count">Count of votes.</param>
		/// <returns>Returns Task as a result of asynchronous operation.</returns>
		Task AddVoteAsync(string msg, int count);

		/// <summary>
		/// Adds voting results to the storage.
		/// </summary>
		/// <param name="messages">Set of <see cref="InboundSms"/> voting SMS messages.</param>
		/// <returns>Returns Task as a result of asynchronous operation.</returns>
		Task AddVotesAsync(IEnumerable<InboundSms> messages);

		/// <summary>
		/// Clears voting statistics in storage.
		/// </summary>
		/// <returns>Returns Task as a result of asynchronous operation.</returns>
		Task ClearStatisticsAsync();
	}
}
