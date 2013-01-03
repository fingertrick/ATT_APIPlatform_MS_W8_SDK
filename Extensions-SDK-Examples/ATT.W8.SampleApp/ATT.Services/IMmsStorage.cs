// <copyright file="IMmsStorage.cs" company="AT&amp;T">
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
	/// Interface for getting MMS from storage or setting into storage.
	/// </summary>
	public interface IMmsStorage
	{
		/// <summary>
		/// Loads MMS messages collection from storage.
		/// </summary>
		/// <param name="shortCode">Short code that represent MMS.</param>
		/// <returns>List of <see cref="InboundMms"/> loaded MMS messages collection.</returns>
		Task<IEnumerable<InboundMms>> GetMmsMessages(string shortCode);

		/// <summary>
		/// Saves MMS into storage.
		/// </summary>
		/// <param name="mmsCollection">List of <see cref="InboundMms"/> MMS.</param>.
		/// <param name="shortCode">Short code that represents MMS collection.</param>
		Task StoreMmsMessages(IEnumerable<InboundMms> mmsCollection, string shortCode);
	}
}
